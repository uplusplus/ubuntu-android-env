
Public Class Form1

    Public Class coordenada_polar
        Public angulo As Single
        Public distancia As Single
    End Class


    Public mediciones() As coordenada_polar



    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub


    Public Function capturar_sensor(ByVal puerto_num As Integer, ByVal rango_angular As Integer, ByVal resolucion_angular As Integer) As Boolean

        Dim conn As LMSAPI.LMSAPI_CONNECTION
        conn = LMSAPI.LMSAPI.create_connection_object(puerto_num, rango_angular, resolucion_angular, 8, 0)

        If (conn Is Nothing) Then
            MsgBox(" Error en la conexion")

            capturar_sensor = False

            Exit Function
        End If


        ' Especificamos la escala de distancias. en metros
        conn.m_scale_distance = 1000.0F

        ' capturar datos del sensor
        Dim sensordata As LMSAPI.LMSAPI_DATA
        sensordata = conn.request_measurement()

        If (conn Is Nothing) Then
            MsgBox("No se puede obtener mediciones del sensor, parametros incorrectos")
            capturar_sensor = False
            Exit Function
        End If

        ' recorrer la colleccion de datos del objeto de mediciones

        ' 1) obtener el numero de mediciones

        Dim numero_mediciones As Integer
        numero_mediciones = sensordata.get_distance_count()

        ' 2) asignar el arreglo de destino
        mediciones = New coordenada_polar() {}
        ReDim mediciones(numero_mediciones - 1)

        ' 3 recorrer la estructura
        Dim i As Integer

        Dim angulo = sensordata.get_min_angle()
        Dim angulo_inc = sensordata.get_resolution()

        For i = 0 To numero_mediciones - 1
            mediciones(i) = New coordenada_polar

            mediciones(i).angulo = angulo
            mediciones(i).distancia = sensordata.get_distance(i)

            angulo = angulo + angulo_inc
        Next

        capturar_sensor = True

    End Function






    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btMedir.Click
        Dim puerto As Integer
        puerto = cbPuerto.SelectedIndex + 1

        Dim rango_angular As Integer

        If (cbRangoAngular.SelectedIndex = 1) Then
            rango_angular = 180
        Else
            rango_angular = 100
        End If


        Dim res_angular As Integer

        If (cbResAngular.SelectedIndex = 1) Then
            res_angular = 50
        ElseIf (cbResAngular.SelectedIndex = 2) Then
            res_angular = 25
        Else
            res_angular = 100
        End If

        Dim verificar As Boolean

        verificar = capturar_sensor(puerto, rango_angular, res_angular)
        If (verificar = False) Then
            Exit Sub
        End If


        ' exportat archivo
        Dim fil As System.IO.StreamWriter

        fil = New System.IO.StreamWriter("datos_sensor.txt")

        Dim i As Integer
        Dim text As String

        text = "angulo;distancia"

        fil.WriteLine(text)

        For i = 0 To UBound(mediciones)

            text = mediciones(i).angulo.ToString()
            text = text + ";" + mediciones(i).distancia.ToString()

            fil.WriteLine(text)
        Next i

        fil.Close()
    End Sub
End Class
