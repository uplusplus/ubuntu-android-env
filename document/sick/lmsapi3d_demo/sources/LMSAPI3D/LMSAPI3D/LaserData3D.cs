using System;
using System.Collections.Generic;
using System.Text;
using IrrlichtNETCP;
using IrrlichtNETCP.Inheritable;
using LMSAPI;

namespace LMSAPI_csharp
{
    /// <summary>
    /// Clase para almacenar mediciones sucesivamente durante un barrido vertical. 
    /// Esto es útil para medir sólidos.
    /// También tiene funciones para la representación de los datos en 3D.
    /// Para iniciar una serie de mediciones, utilice begin_capturing y luego capture_data
    /// </summary>
    public class LaserData3D
    {

        /// <summary>
        /// Barrido verticla angular o lineal.
        /// </summary>
        public bool m_vertical_sampler_angular = false;
        /// <summary>
        /// Rango mínimo de distancia en el barrido.
        /// </summary>
        public float m_vertical_sampler_min = 0.0f;
        /// <summary>
        /// Rango máximo de distancia en el barrido.
        /// </summary>
        public float m_vertical_sampler_max = 0.0f;
        /// <summary>
        /// Numero de lecturas del laser durante el barrido.
        /// </summary>
        public uint m_vertical_sampler_count = 10;
        /// <summary>
        /// Escala de distancias en el barrido linear. 0.01 Indica que se trata de centímetros
        /// </summary>
        public float m_vertical_scale = 0.01f;

        /// <summary>
        /// Resolución angular para la representación de la malla 3D. 1 Es la máxima resolución.
        /// </summary>
        public uint m_mesh_resolution = 5;


        public LMSAPI_DATA m_current_laser_buffer;
        private uint m_vertex_count = 0;
        private float[] m_laser_distances;
        private uint m_buffer_index;       
        

        public LaserData3D()
        {            
        }
        
        /// <summary>
        /// Función para iniciar un barrido de mediciones.
        /// </summary>
        /// <param name="laser_buffer">Objeto de lectura de datos</param>
        public void begin_capturing(LMSAPI_DATA laser_buffer)
        {           
            // create buffer
            m_vertex_count = m_vertical_sampler_count * laser_buffer.get_distance_count();

            m_laser_distances = new float[m_vertex_count];
            m_buffer_index = 0;

            capture_data(laser_buffer);
        }

        /// <summary>
        /// Llamese despues de llamar begin_capturing()
        /// </summary>
        /// <param name="laser_buffer">Objeto de lectura de datos</param>
        /// <returns>false si se ha completado el número de capturas</returns>
        public bool capture_data(LMSAPI_DATA laser_buffer)
        {
            m_current_laser_buffer = laser_buffer;
            if (m_buffer_index >= m_vertical_sampler_count) return false; //end of measurement


            uint sample_count = m_current_laser_buffer.get_distance_count();
            for (int i = 0; i < sample_count; i++)
            {
                m_laser_distances[m_buffer_index * sample_count+i] = m_current_laser_buffer.get_distance(i);
                
            }
            m_buffer_index++;
            return true;
        }

        private double deg_to_rad(double angle_degrees)
        {
            return angle_degrees*Math.PI/180.0;
        }

        /// <summary>
        /// Convierte los datos almacenados en un Objeto 3D. Se toma en cuenta el 
        /// tipo de barrido (angular o linear vertical).
        /// </summary>
        /// <param name="node">El nodo Irrlicht donde se representarán los datos</param>
        public void export_to_model(LaserDataSceneNode node)
        {
            if (node.has_vertices() == false)
            {
                node.create_mesh();
            }


            if (m_vertical_sampler_angular)
            {
                export_to_model_angular(node);                
            }
            else
            {
                export_to_model_linear(node);
            }


            node._mgr.MeshManipulator.RecalculateNormals(node._mesh, true);
            
        }

        void create_model_indices(LaserDataSceneNode node, ushort sides, ushort slices)
        {
            // crear los indices
            MeshBuffer mbuffer = node.get_mesh_buffer();

            //uint ind_count =(uint)( (slices - 1) * (sides - 1) * 12);
            uint ind_count = (uint)((slices - 1) * (sides - 1) * 6);

            mbuffer.AllocateIndices(ind_count);
                     

            m_buffer_index = 0;
            for (ushort i = 0; i < slices - 1; i++)
            {
                for (ushort j = 0; j < sides - 1; j++)
                {
                    //first triangle
                    mbuffer.SetIndex(m_buffer_index, (ushort)((i + 1) * sides + j));
                    m_buffer_index++;
                    mbuffer.SetIndex(m_buffer_index, (ushort)(i * sides + j));
                    m_buffer_index++;
                    mbuffer.SetIndex(m_buffer_index,(ushort)((i + 1) * sides + j + 1));
                    m_buffer_index++;
                    //second triangle
                    mbuffer.SetIndex(m_buffer_index, (ushort)((i + 1) * sides + j + 1));
                    m_buffer_index++;                    
                    mbuffer.SetIndex(m_buffer_index, (ushort)(i * sides + j));
                    m_buffer_index++;                    
                    mbuffer.SetIndex(m_buffer_index, (ushort)(i * sides + j + 1));
                    m_buffer_index++;

                    /*
                    //third triangle
                    mbuffer.SetIndex(m_buffer_index, (ushort)((i + 1) * sides + j));
                    m_buffer_index++;
                    mbuffer.SetIndex(m_buffer_index, (ushort)(i * sides + j));
                    m_buffer_index++;                    
                    mbuffer.SetIndex(m_buffer_index, (ushort)((i + 1) * sides + j + 1));
                    m_buffer_index++;

                    //fourth triangle
                    mbuffer.SetIndex(m_buffer_index, (ushort)((i + 1) * sides + j + 1));
                    m_buffer_index++;
                    mbuffer.SetIndex(m_buffer_index, (ushort)(i * sides + j));
                    m_buffer_index++;                    
                    mbuffer.SetIndex(m_buffer_index, (ushort)(i * sides + j + 1));
                    m_buffer_index++;*/
                }
            }
        }



        private void export_to_model_linear(LaserDataSceneNode node)
        {
            float min_angle = m_current_laser_buffer.get_min_angle();
            float ang_increment = m_current_laser_buffer.get_resolution() * m_mesh_resolution;


            uint num_h_samples = m_current_laser_buffer.get_distance_count();

            uint horizontal_count = num_h_samples / m_mesh_resolution;

            float vertical_increment = m_vertical_scale * (m_vertical_sampler_max - m_vertical_sampler_min) / m_vertical_sampler_count;


            ushort sides = (ushort)horizontal_count;
            ushort slices = (ushort)m_vertical_sampler_count;

            
            // crear los vertices
            MeshBuffer mbuffer = node.get_mesh_buffer();

            uint vert_count = (uint)(sides * slices);

            mbuffer.AllocateVertices(vert_count);

            m_buffer_index = 0;
            float vertical_value = m_vertical_sampler_min * m_vertical_scale;
            float angular_value = min_angle;

            for (ushort i = 0; i < slices; i++)
            {
                angular_value = min_angle;
                for (ushort j = 0; j < sides; j++)
                {
                    float distance = m_laser_distances[num_h_samples*i + j*m_mesh_resolution];
                    Vector3D pivot = new Vector3D();
                    Vector3D normal = new Vector3D();
                    normal.Z = -(float)Math.Cos(deg_to_rad((double)angular_value));
                    normal.Y = -(float)Math.Sin(deg_to_rad((double)angular_value));
                    normal.X = 0;

                    pivot.Z = normal.Z * distance;
                    pivot.Y = normal.Y * distance;
                    pivot.X = vertical_value;

                    Vertex3D newvertex =  new Vertex3D();
                    
                    newvertex.Position = pivot;
                    newvertex.Normal = normal;
                    newvertex.Color = new Color(255, 0, 0, 150);
                    newvertex.TCoords = new Vector2D(1, 1);

                    mbuffer.SetVertex(m_buffer_index, newvertex);
                    m_buffer_index++;

                    angular_value += ang_increment;

                }

                vertical_value += vertical_increment;
            }

            create_model_indices(node, sides, slices);
            
        }

        private void export_to_model_angular(LaserDataSceneNode node)
        {
            float min_angle = m_current_laser_buffer.get_min_angle();
            float ang_increment = m_current_laser_buffer.get_resolution() * m_mesh_resolution;


            uint num_h_samples = m_current_laser_buffer.get_distance_count();

            uint horizontal_count = num_h_samples / m_mesh_resolution;


            float vertical_increment = (m_vertical_sampler_max - m_vertical_sampler_min) / m_vertical_sampler_count;


            ushort sides = (ushort)horizontal_count;
            ushort slices = (ushort)m_vertical_sampler_count;

            // crear los vertices

            MeshBuffer mbuffer = node.get_mesh_buffer();

            uint vert_count = (uint)(sides * slices);

            mbuffer.AllocateVertices(vert_count);

            
            m_buffer_index = 0;
            float vertical_value = m_vertical_sampler_min;
            float angular_value = min_angle;

            for (ushort i = 0; i < slices; i++)
            {
                angular_value = min_angle;
                for (ushort j = 0; j < sides; j++)
                {
                    float distance = m_laser_distances[num_h_samples * i + j * m_mesh_resolution];
                    Vector3D pivot = new Vector3D();
                    Vector3D normal = new Vector3D();
                    
                    normal.Z = -(float)Math.Cos(deg_to_rad((double)angular_value));
                    normal.Y = -(float)Math.Sin(deg_to_rad((double)angular_value));
                    normal.X = 0;
                    normal.RotateXYBy(vertical_value, new Vector3D());

                    //normal.X = (float)Math.Sin(deg_to_rad((double)vertical_value)); ;
                    
                    pivot.Z = normal.Z * distance;
                    pivot.Y = normal.Y * distance;
                    pivot.X = normal.X * distance;


                    Vertex3D newvertex = new Vertex3D();

                    newvertex.Position = pivot;
                    newvertex.Normal = normal;
                    newvertex.Color = new Color(255, 255, 0, 100);
                    newvertex.TCoords = new Vector2D(1, 1);

                    mbuffer.SetVertex(m_buffer_index, newvertex);
                    m_buffer_index++;

                    angular_value += ang_increment;

                }

                vertical_value += vertical_increment;
            }

            create_model_indices(node, sides, slices);            
        }
    }
}
