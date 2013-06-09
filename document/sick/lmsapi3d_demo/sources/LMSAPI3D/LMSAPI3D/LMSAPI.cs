using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace LMSAPI
{
    /// <summary>
    /// Clase Envoltura para encapsular las funciones de la libreria dinamica LMSAPI.
    /// </summary>
    /// <remarks>También puede crear objetos de conexión para manejar los comandos del sensor de forma segura. </remarks>
    /// <see cref="create_connection_object"/>
    public class LMSAPI_LIB
    {        
        /*! \defgroup FUNCIONES_ACCESO_MEDICIONES
        \brief
        Funciones para acceder a la estructura de datos de medición. Util para la comunicacion con
        otros lenguajes o entornos
        */
        /// @{

        /// <summary>
        /// Crea una estructura de medicion
        /// </summary>
        [DllImport("lmsapi.dll")]
        public static extern IntPtr lmsapi_laser_data_create();

        /// <summary>
        /// Libera la memoria de una estructura de medición
        /// </summary>
        [DllImport("lmsapi.dll")]
        public static extern void lmsapi_laser_data_destroy(IntPtr laserdata);

        /// <summary>
        /// Accede al miembro resolucion angular
        /// </summary>
        [DllImport("lmsapi.dll")]
        public static extern float lmsapi_laser_data_get_resolution(IntPtr laserdata);

        /// <summary>
        /// Accede al miembro angulo minimo
        /// </summary>
        [DllImport("lmsapi.dll")]
        public static extern float lmsapi_laser_data_get_min_angle(IntPtr laserdata);

        /// <summary>
        /// Accede al miembro angulo máximo
        /// </summary>
        [DllImport("lmsapi.dll")]
        public static extern float lmsapi_laser_data_get_max_angle(IntPtr laserdata);

        /// <summary>
        /// Obtiene el rango de distancia
        /// </summary>
        [DllImport("lmsapi.dll")]
        public static extern float lmsapi_laser_data_get_max_range(IntPtr laserdata);

        /// <summary>
        /// Obtiene la escala de distancias.
        /// </summary>
        [DllImport("lmsapi.dll")]
        public static extern float lmsapi_laser_data_get_scale_distance(IntPtr laserdata);

        /// <summary>
        /// Obtiene el número de distancias
        /// </summary>
        [DllImport("lmsapi.dll")]
        public static extern uint lmsapi_laser_data_get_distance_count(IntPtr laserdata);

        /// <summary>
        /// Obtiene una distancia del arreglo de distancias
        /// </summary>
        [DllImport("lmsapi.dll")]
        public static extern float lmsapi_laser_data_get_distance(IntPtr laserdata, int index);

        /// <summary>
        /// Obtiene el número de intensidades. 0 Si no hay intensidades
        /// </summary>
        [DllImport("lmsapi.dll")]
        public static extern uint lmsapi_laser_data_get_intensity_count(IntPtr laserdata);

        /// <summary>
        /// Obtiene un valor de intensidad del arreglo de intensidades
        /// </summary>
        [DllImport("lmsapi.dll")]
        public static extern uint lmsapi_laser_data_get_intensity(IntPtr laserdata, int index);
        
        /// @}

        /*! \defgroup FUNCIONES_COMANDOS_SENSOR
        \brief
        Funciones para acceder a las funciones del sensor.
        */
        /// @{

        /// <summary>
        /// Cambia el sensor a modo de configuracion
        /// </summary>
        /// <remarks>
        /// Pre: connection Debe ser una conexión abierta a un puerto serial.        
        /// </remarks>        
        /// <param name="connection"> Puntero a manejador de la conexión a puerto serial.</param>
        /// <returns> un entero positivo si el comando fue procesado con exito, si no entregara un numero negativo que indica un error. </returns>
        /// <seealso cref="lmsapi_open_terminal"/>
        [DllImport("lmsapi.dll")]
        public static extern int lmsapi_switch_configuration_mode(IntPtr connection);

        /// <summary>
        /// Establece la resolución angular del sensor.
        /// </summary>
        /// <remarks>pre: connection Debe ser una conexión abierta a un puerto serial.</remarks>
        /// <param name="connection">Puntero a manejador de la conexión a puerto serial.</param>
        /// <param name="res">Resolución angular, uno de los siguientes valores: 25, 50, 100 (Cada valor es un porcentaje de grado, 1/100).</param>
        /// <param name="width">Rango angular del barrido. Los valores válidos son: 100 y 180 grados de circunferencia.</param>
        /// <returns>0 si la operación fué exitosa, o un valor de error.</returns>
        /// <seealso cref="lmsapi_open_terminal"/>
        [DllImport("lmsapi.dll")]
        public static extern int lmsapi_set_resolution(IntPtr connection, int width, int res);

        /// <summary>
        /// Obtiene los datos de medición del sensor.
        /// </summary>
        /// <param name="connection">Puntero a manejador de la conexión a puerto serial. Vease lmsapi_open_terminal</param>
        /// <param name="data">Estructura donde se obtendran las distancias y otros datos de medición. Vease  <see cref="lmsapi_laser_data_create"/> </param>
        /// <param name="scale_distance">Escala para las distancias. Configure la escala de la siguiente manera: <br/><ul><li> 1.0f : Milimetros.</li><li> 10.0f : Centímetros</li><li> 100.0f : ecimetros</li><li> 1000.0f : Metros</li></ul></param>
        /// <returns>0 si la operacion fue exitosa, o un codigo de error.</returns>
        /// <remarks>pre:connection Debe ser una conexión abierta a un puerto serial.</remarks>
        [DllImport("lmsapi.dll")]
        public static extern int lmsapi_request_measurement(
            IntPtr connection, IntPtr data, float scale_distance);

        /// <summary>
        /// Configura el rango de distancias y habilita el manejo de intensidades en las mediciones.
        /// </summary>
        /// <param name="connection">Puntero a manejador de la conexión a puerto serial.</param>
        /// <param name="distance_range">Rango de distancias en Metros. Una de las constantes de distancias: 8, 80 o 150 metros respectivamente.</param>
        /// <param name="handle_intensity">Booleano con el cual se habilita la medición de distancias.Asignese 1 o 0.</param>
        /// <returns>0 si la operacion fue exitosa, o un codigo de error.</returns>
        /// <remarks>pre: connection Debe ser una conexión abierta a un puerto serial.</remarks>   
        [DllImport("lmsapi.dll")]
        public static extern int lmsapi_config(
            IntPtr connection,
            int distance_range, int handle_intensity);

        /// <summary>
        /// Establece una conexion con el sensor laser.
        /// </summary>
        /// <param name="distance_range">Rango de distancias en Metros. Una de las constantes de distancias: 8, 80 o 150 metros respectivamente.</param>
        /// <param name="handle_intensity">Booleano con el cual se habilita la medición de distancias. Asignese 1 o 0.</param>
        /// <param name="port">Entero positivo correspondiente al puerto COM. (COM1,COM2...)</param>
        /// <param name="resolution">Resolución angular, uno de los siguientes valores: 25, 50, 100 (Cada valor es un porcentaje de grado, 1/100).</param>
        /// <param name="width">Rango angular del barrido. Los valores válidos son: 100 y 180 grados de circunferencia.</param>
        /// <returns>Un puntero a una estructura LMSAPI_CONNECTION, o NULL si hubo un error</returns>  
        [DllImport("lmsapi.dll")]
        public static extern IntPtr lmsapi_open_terminal(int port,
                                 int width, int resolution,
                                 int distance_range, int handle_intensity);

        /// <summary>
        /// Cierra una conexion con el sensor laser.
        /// </summary>
        /// <param name="connection">Puntero a manejador de la conexión a puerto serial.</param>
        /// <returns>0 si la operacion fue exitosa, o un codigo de error.</returns>
        [DllImport("lmsapi.dll")]
        public static extern int lmsapi_close_terminal(IntPtr connection);


        /// <summary>
        /// Funcion que simula los datos de una medición laser.
        /// </summary>
        /// <param name="distance_range">Rango de distancias en Metros. Una de las constantes de distancias: 8, 80 o 150 metros respectivamente.</param>
        /// <param name="handle_intensity">Booleano con el cual se habilita la medición de distancias.</param>
        /// <param name="resolution">Resolución angular, uno de los siguientes valores: 25, 50, 100 (Cada valor es un porcentaje de grado, 1/100).</param>
        /// <param name="width">Rango angular del barrido. Los valores válidos son: 100 y 180 grados de circunferencia.</param>
        /// <param name="scale_distance">scale_distance Escala para las distancias. Configure la escala de la siguiente manera: 1.0f : Milimetros; 10.0f : Centímetros; 100.0f : Decimetros, 1000.0f : Metros</param>
        /// <param name="laserdata">Estructura donde se obtendran las distancias y otros datos de medición.</param>
        /// <remarks>pre: laserdata Debe estar inicializado con lmsapi_laser_data_create().</remarks>
        [DllImport("lmsapi.dll")]
        public static extern void lmsapi_laser_data_simulate_values(
            IntPtr laserdata,
	        int width, int resolution,
	        int distance_range, int handle_intensity,
	        float scale_distance);


        /// @}


        /// <summary>
        /// Factoria de clase para objetos LMSLASER_CONNECTION. Preferiblemente utilice esta función para crear conexiones al sensor y realizar operaciones de medición a través del objeto LMSLASER_CONNECTION
        /// </summary>
        /// <param name="distance_range">Rango de distancias en Metros. Una de las constantes de distancias: 8, 80 o 150 metros respectivamente.</param>
        /// <param name="handle_intensity">Booleano con el cual se habilita la medición de distancias. Asignese 1 o 0.</param>
        /// <param name="port">Entero positivo correspondiente al puerto COM. (COM1,COM2...)</param>
        /// <param name="resolution">Resolución angular, uno de los siguientes valores: 25, 50, 100 (Cada valor es un porcentaje de grado, 1/100).</param>
        /// <param name="width">Rango angular del barrido. Los valores válidos son: 100 y 180 grados de circunferencia.</param>
        /// <returns>Un objeto LMSAPI_CONNECTION, o null si hubo un error</returns>  
        public static LMSAPI_CONNECTION create_connection_object(int port,
                                 int width, int resolution,
                                 int distance_range, int handle_intensity)
        {
            IntPtr handle = lmsapi_open_terminal(port, width, resolution, distance_range, handle_intensity);

            if (handle == IntPtr.Zero) return null;

            LMSAPI_CONNECTION newconnection = new LMSAPI_CONNECTION(handle);
            return newconnection;
        }



        /// <summary>
        /// Obtiene un objeto LMSAPI_DATA (datos de medición) con datos simulados en memoria.
        /// </summary>
        /// <param name="distance_range">Rango de distancias en Metros. Una de las constantes de distancias: 8, 80 o 150 metros respectivamente.</param>
        /// <param name="handle_intensity">Booleano con el cual se habilita la medición de distancias.</param>
        /// <param name="resolution">Resolución angular, uno de los siguientes valores: 25, 50, 100 (Cada valor es un porcentaje de grado, 1/100).</param>
        /// <param name="width">Rango angular del barrido. Los valores válidos son: 100 y 180 grados de circunferencia.</param>
        /// <param name="scale_distance">scale_distance Escala para las distancias. Configure la escala de la siguiente manera: 1.0f : Milimetros; 10.0f : Centímetros; 100.0f : Decimetros, 1000.0f : Metros</param>
        /// <returns>Un objeto LMSAPI_DATA con los datos de medición, o null si hubo un error.</returns>
        public static LMSAPI_DATA create_simulation_laser_data(
            int width, int resolution,
            int distance_range, int handle_intensity,
            float scale_distance)
        {
            IntPtr laserdata = lmsapi_laser_data_create();
            if (laserdata == IntPtr.Zero) return null;// ?? fallo de memoria
            lmsapi_laser_data_simulate_values(laserdata, width, resolution, distance_range, handle_intensity, scale_distance);

            LMSAPI_DATA newdata = new LMSAPI_DATA(laserdata);
            return newdata;
        }
    }

       
    /// <summary>
    /// Clase envoltorio para administrar una conexión al sensor
    /// </summary>
    public class LMSAPI_CONNECTION
    {
        public IntPtr m_handle;
        /// <summary>
        /// Escala de distancias usada para la obtención de mediciones. (Por defecto en centímetros 10.0f)
        /// </summary>
        public float m_scale_distance;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="handle">Parámetro ingresado en la funcion LMSAPI_LIB.create_connection_object </param>
        /// <seealso cref="LMSAPI_LIB.create_connection_object"/>
        public LMSAPI_CONNECTION(IntPtr handle)
        {
            m_handle = handle;
            m_scale_distance = 10.0f;//centímetros
        }

        ~LMSAPI_CONNECTION()
        {
            LMSAPI_LIB.lmsapi_close_terminal(m_handle);
        }


        /// <summary>
        /// Configura el rango de distancias y habilita el manejo de intensidades en las mediciones.
        /// </summary>        
        /// <param name="distance_range">Rango de distancias en Metros. Una de las constantes de distancias: 8, 80 o 150 metros respectivamente.</param>
        /// <param name="handle_intensity">Booleano con el cual se habilita la medición de distancias.Asignese 1 o 0.</param>
        /// <returns>0 si la operacion fue exitosa, o un codigo de error.</returns>        
        public int config(int distance_range, int handle_intensity)
        {
            return LMSAPI_LIB.lmsapi_config(m_handle, distance_range, handle_intensity);
        }

        /// <summary>
        /// Establece la resolución angular del sensor.
        /// </summary>
        /// <param name="res">Resolución angular, uno de los siguientes valores: 25, 50, 100 (Cada valor es un porcentaje de grado, 1/100).</param>
        /// <param name="width">Rango angular del barrido. Los valores válidos son: 100 y 180 grados de circunferencia.</param>
        /// <returns>0 si la operación fué exitosa, o un valor de error.</returns>
        public int set_resolution(int width, int res)
        {
            return LMSAPI_LIB.lmsapi_set_resolution(m_handle,width, res);
        }

        /// <summary>
        /// Utiliza el miembro m_scale_distance para la distancia, y crea un objeto de datos del laser (LMSAPI_DATA) si la medición tuvo exito
        /// </summary>
        /// <returns>Un objeto LMSAPI_DATA con los datos de medición, o null si hubo un error</returns>
        public LMSAPI_DATA request_measurement()
        {
            IntPtr laserdata = LMSAPI_LIB.lmsapi_laser_data_create();
            if (laserdata == IntPtr.Zero) return null;// ?? fallo de memoria
            if (LMSAPI_LIB.lmsapi_request_measurement(m_handle, laserdata, m_scale_distance) == 0)
            {
                //operación exitosa
                LMSAPI_DATA newdata = new LMSAPI_DATA(laserdata);
                return newdata;
            }
            LMSAPI_LIB.lmsapi_laser_data_destroy(laserdata);
            return null;
        }          



    }


    /// <summary>
    /// Clase envoltorio para contener los datos de medición.
    /// </summary>
    /// <example>
    /// LMSAPI_CONNECTION newconnection = LMSAPI_LIB.create_connection_object(1,100,50,8,0);
    /// newconnection.m_scale_distance = 1000.0f;//Configurando la escala a metros.
    /// LMSAPI_DATA data = newconnection.request_measurement();//obteniendo los datos de medición
    /// </example>
    /// <remarks>
    /// Utilice un objeto LMSAPI_CONNECTION para crear este objeto de datos de medición    
    /// </remarks>    
    public class LMSAPI_DATA
    {
        public IntPtr m_handle;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="handle">Puntero de estructura de datos. Debe ser creado con la función  LMSAPI_LIB.lmsapi_laser_data_create(). </param>
        /// <seealso cref="LMSAPI_LIB.lmsapi_laser_data_create"/>
        public LMSAPI_DATA(IntPtr handle)
        {
            m_handle = handle;
        }

        ~LMSAPI_DATA()
        {
            LMSAPI_LIB.lmsapi_laser_data_destroy(m_handle);
        }

        /// <summary>
        /// Accede al miembro resolucion angular
        /// </summary>        
        public float get_resolution()
        {
            return LMSAPI_LIB.lmsapi_laser_data_get_resolution(m_handle);
        }

        /// <summary>
        /// Accede al miembro angulo minimo
        /// </summary>        
        public float get_min_angle()
        {
            return LMSAPI_LIB.lmsapi_laser_data_get_min_angle(m_handle);
        }

        /// <summary>
        /// Accede al miembro angulo máximo
        /// </summary>        
        public float get_max_angle()
        {
            return LMSAPI_LIB.lmsapi_laser_data_get_max_angle(m_handle);
        }

        /// <summary>
        /// Obtiene el rango de distancia
        /// </summary>        
        public float get_max_range()
        {
            return LMSAPI_LIB.lmsapi_laser_data_get_max_range(m_handle);
        }

        /// <summary>
        /// Obtiene la escala de distancias.
        /// </summary>        
        public float get_scale_distance()
        {
            return LMSAPI_LIB.lmsapi_laser_data_get_scale_distance(m_handle);
        }

        /// <summary>
        /// Obtiene el número de distancias
        /// </summary>        
        public uint get_distance_count()
        {
            return LMSAPI_LIB.lmsapi_laser_data_get_distance_count(m_handle);
        }

        /// <summary>
        /// Obtiene una distancia del arreglo de distancias
        /// </summary>        
        public float get_distance(int index)
        {
            return LMSAPI_LIB.lmsapi_laser_data_get_distance(m_handle, index);
        }

        /// <summary>
        /// Obtiene el número de intensidades. 0 Si no hay intensidades
        /// </summary>        
        public uint get_intensity_count()
        {
            return LMSAPI_LIB.lmsapi_laser_data_get_intensity_count(m_handle);
        }

        /// <summary>
        /// Obtiene un valor de intensidad del arreglo de intensidades
        /// </summary>        
        public uint get_intensity(int index)
        {
            return LMSAPI_LIB.lmsapi_laser_data_get_intensity(m_handle, index);
        }


    }
}
