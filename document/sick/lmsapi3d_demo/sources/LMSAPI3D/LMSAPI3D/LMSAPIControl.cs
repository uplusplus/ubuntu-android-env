using System;
using System.Collections.Generic;
using System.Text;
using LMSAPI;

namespace LMSAPI_csharp
{
    /// <summary>
    /// Clase para adminstrar las propiedades de conexión y la captura del sensor.
    /// Contiene una copia offline de los parámetros de medición y almacena los objetos de 
    /// conexión (LMSAPI_CONNECTION) y de lectura de datos (LMSAPI_DATA)
    /// </summary>
    public class LMSAPIControl
    {
        public LMSAPI_CONNECTION laser_connection = null;
        public LMSAPI_DATA laser_data = null;
        public int port = 1;
        /// <summary>
        /// Rango angular: 100 180
        /// </summary>
        public int angular_range = 100;
        /// <summary>
        /// Resolución: 100 (1 grado),50(0.5 grados),25 (0.25 grados).
        /// </summary>
        public int angular_resolution = 100;
        /// <summary>
        /// Rango de distancias: 8, 80
        /// </summary>
        public int distance_range = 8;
        /// <summary>
        /// Escala de medición.
        /// </summary>
        public float scale_distance = 1000.0f;// en metros

        /// <summary>
        /// Establece una conexión con el sensor según los parámetros asignados.
        /// </summary>
        /// <returns>true si la conexión se realizó con exito</returns>
        public bool stablish_connection()
        {
            laser_connection = null;//cerrar conexión anterior

            // Conectarnos con el sensor
            laser_connection = LMSAPI_LIB.create_connection_object(port, angular_range, angular_resolution, distance_range, 0);

            if (laser_connection == null)
            {
                System.Windows.Forms.MessageBox.Show("Error!!:Configuraciòn erronea del sensor, no puede establecerse conexiòn.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Cierra la conexión con el sensor
        /// </summary>
        public void end_connection()
        {            
            laser_connection = null;
        }

        /// <summary>
        /// Crea un objeto de lectura de datos (LMSAPI_DATA) con datos simulados, sin 
        /// necesidad de conexión con el sensor. 
        /// POST: laser_data != null.
        /// </summary>
        /// <returns></returns>
        public bool simulate_laser_data()
        {
            laser_data = LMSAPI_LIB.create_simulation_laser_data(angular_range, angular_resolution, distance_range, 0, scale_distance);
            if (laser_data == null)
            {
                System.Windows.Forms.MessageBox.Show("Error al solicitar datos de mediciòn");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Obtiene datos del sensor en el objeto de lectura laser_data. Si no hay conexión
        /// entonces obtiene datos simulados.
        /// </summary>
        /// <returns></returns>
        public bool request_laser_data()
        {
            if (laser_connection == null)
            {
                return simulate_laser_data();
            }

            laser_connection.m_scale_distance = scale_distance;//distancias en metros

            laser_data = laser_connection.request_measurement();
            if (laser_data == null)
            {
                System.Windows.Forms.MessageBox.Show("Error al solicitar datos de mediciòn");
                return false;
            }
            return true;

        }
    }
}
