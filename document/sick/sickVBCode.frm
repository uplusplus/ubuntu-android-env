VERSION 5.00
Object = "{648A5603-2C6E-101B-82B6-000000000014}#1.1#0"; "MSCOMM32.OCX"
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   3090
   ClientLeft      =   60
   ClientTop       =   450
   ClientWidth     =   4680
   LinkTopic       =   "Form1"
   ScaleHeight     =   3090
   ScaleWidth      =   4680
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdRecordScan 
      Caption         =   "Record Scan"
      Height          =   615
      Left            =   480
      TabIndex        =   2
      Top             =   1920
      Width           =   1575
   End
   Begin VB.CommandButton cmdStopSensor 
      Caption         =   "Stop Sensor"
      Height          =   615
      Left            =   480
      TabIndex        =   1
      Top             =   1080
      Width           =   1575
   End
   Begin VB.CommandButton cmdStartSensor 
      Caption         =   "Start Sensor"
      Height          =   615
      Left            =   480
      TabIndex        =   0
      Top             =   240
      Width           =   1575
   End
   Begin MSCommLib.MSComm MSComm1 
      Left            =   3720
      Top             =   2160
      _ExtentX        =   1005
      _ExtentY        =   1005
      _Version        =   393216
      DTREnable       =   -1  'True
   End
   Begin VB.Label recordLabel 
      Caption         =   "Recording...."
      Height          =   375
      Left            =   2400
      TabIndex        =   3
      Top             =   1200
      Width           =   1935
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim startString As String   'Stores the string used to activate the sensor
Dim stopString As String    'Store the string used to deactivate the sensor
Dim gatherData As Boolean   'A flag used to begin recording data

'The "Record Scan" button is used to initiate the writing of scan data to a text file
Private Sub cmdRecordScan_Click()
    
    'When the button is clicked, first set the gatherData flag
    gatherData = True
    'Next, display "Recording..." so that the user knows data is being written
    recordLabel.Visible = True

End Sub

'The "Start Sensor" button is used to actiavte the SICK
Private Sub cmdStartSensor_Click()

    'When the button is clicked, send the start string to the SICK
    MSComm1.Output = startString

End Sub

'The "Stop Sensor" button is used to deactiavte the SICK
Private Sub cmdStopSensor_Click()

    'When the button is clicked, send the stop string to the SICK
    MSComm1.Output = stopString

End Sub

'When the form loads, run initiliaztion routines for the program
Private Sub Form_Load()

Dim port, baud As String    'These store the comm port and baud rate inputs from the user

'Define the start and stop strings
startString = Chr(2) + Chr(0) + Chr(2) + Chr(0) + Chr(32) + Chr(36) + Chr(52) + Chr(8)
stopString = Chr(2) + Chr(0) + Chr(2) + Chr(0) + Chr(32) + Chr(37) + Chr(53) + Chr(8)

'Hide the "Recording..." text
recordLabel.Visible = False

'Prompt the user to enter the baud rate and comm port
baud = InputBox("Enter baud rate:", "Baud Rate", "9600")
port = InputBox("Enter comm port:", "Comm Port", "4")

'Initialize RS232 communication
With MSComm1
   .CommPort = Val(port)
   .InputMode = comInputModeBinary
   .InputLen = 2000
   .Handshaking = 0 - comNone
   .RThreshold = 2000
   .RTSEnable = False
   .Settings = baud + ",N,8,1"
   .SThreshold = 1
   .PortOpen = True
   ' Leave all other settings as default values.
End With

'Open the text file where data will be written to
'NOTE: This is where you change the path and filename where data is written
Open "E:\Keith\research\sickVBCode\data.txt" For Output As #1

End Sub

'When the form unloads, shut everything down
Private Sub Form_Unload(Cancel As Integer)

    'Close the comm port
    MSComm1.PortOpen = False
    'Close the data file
    Close #1

End Sub

'This code excutes when ever the RS232 buffer fills up. This is
'where the majority of the code is located
Private Sub MSComm1_OnComm()

    Dim buffData As Variant 'This stores the incoming data from the buffer
    Dim i, c As Integer     'These are general counters
    Dim deg As Double       'This store the count for degrees
        
    Dim headerString(8) As Integer   'This stores the header to a packet of data
    
    'This is where the header for a data packet is assigned
    'NOTE: This changes based on settings of the SICK
    headerString(1) = 2
    headerString(2) = 128
    headerString(3) = 214
    headerString(4) = 2
    headerString(5) = 176
    headerString(6) = 105
    headerString(7) = 65
    
    i = 0   'This is the counter for the buffer
    c = 0   'This is the counter for the header string
    
    Select Case MSComm1.CommEvent
    
        'If data has been received
        Case comEvReceive
            
            'Store the data in buffData
            buffData = MSComm1.Input
                
            'If the gatherData flag has been set
            If gatherData Then
                
                'This loop looks for the header in the data
                Do Until ((c > 7) Or (i > (UBound(buffData) - 1)))
                    c = 1
                    i = i + 1
                    While (headerString(c) = buffData(i))
                        c = c + 1
                        i = i + 1
                    Wend
                Loop
                
                'If the header was found and there are still 722 points left
                If ((c > 7) And ((UBound(buffData) - i) > 722)) Then
                    deg = 0
                
                    'Write the column headers to the file
                    Print #1,
                    Print #1, ("Angle [deg]" & vbTab & "Distance [m]")
                
                    'Write the angle and distance measurement to the file
                    For i = i To i + 721
                        'calculate the distance measurement from the lower and upper byte
                        Print #1, (deg & vbTab & (CLng(buffData(i)) + CLng(buffData(i + 1)) * 256))
                        deg = deg + 0.5
                        i = i + 1
                    Next i
                    
                    'After writing the data to the file, reset the flag and hide the text "Recording..."
                    recordLabel.Visible = False
                    gatherData = False
                End If
            End If
    End Select

End Sub
