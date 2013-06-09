using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using IrrlichtNETCP;
using IrrlichtNETCP.Inheritable;

namespace LMSAPI_csharp
{
    /// <summary>
    /// Clase para representar una lectura de datos en una malla 3D.
    /// </summary>
    public class LaserDataSceneNode : ISceneNode
    {
        public Mesh _mesh;
        Material Material = new Material();

        public SceneManager _mgr;
        VideoDriver _driver;

        public LaserDataSceneNode(SceneNode parent, SceneManager mgr, int id)
            : base(parent, mgr, id)
        {
            _mgr = mgr;
            _driver = _mgr.VideoDriver;
            Material.Wireframe = false;
            Material.Lighting = false;

          
        

            create_mesh();


        }

        public void create_mesh()
        {
            _mesh = new Mesh();
            // add default mesh buffer
            MeshBuffer mbuffer = new MeshBuffer(VertexType.Standard);
            
            //////add a default shape

            ////vertices
            //mbuffer.AllocateVertices(4);
            //mbuffer.SetVertex(0,new Vertex3D(new Vector3D(0, 0, 10), new Vector3D(1, 1, 0), Color.From(255, 0, 255, 255), new Vector2D(0, 1)));

            //mbuffer.SetVertex(1, new Vertex3D(new Vector3D(10, 0, -10), new Vector3D(1, 0, 0), Color.From(255, 255, 0, 255), new Vector2D(1, 1)));

            //mbuffer.SetVertex(2, new Vertex3D(new Vector3D(0, 20, 0), new Vector3D(0, 1, 1), Color.From(255, 255, 255, 0), new Vector2D(1, 0)));

            //mbuffer.SetVertex(3, new Vertex3D(new Vector3D(-10, 0, -10), new Vector3D(0, 0, 1), Color.From(255, 0, 255, 0), new Vector2D(0, 0)));

            //// alocate indices

            //mbuffer.AllocateIndices(12);
            //mbuffer.SetIndex(0, 0);
            //mbuffer.SetIndex(1, 2);
            //mbuffer.SetIndex(2, 3);
            //mbuffer.SetIndex(3, 2);
            //mbuffer.SetIndex(4, 1);
            //mbuffer.SetIndex(5, 3);
            //mbuffer.SetIndex(6, 1);
            //mbuffer.SetIndex(7, 0);
            //mbuffer.SetIndex(8, 3);
            //mbuffer.SetIndex(9, 2);
            //mbuffer.SetIndex(10, 0);
            //mbuffer.SetIndex(11, 1);

            _mesh.AddMeshBuffer(mbuffer);
        
        }

        public MeshBuffer get_mesh_buffer()
        {
            return _mesh.GetMeshBuffer(0);
        }

        public bool has_vertices()
        {
            if (_mesh == null) return false;
            if (get_mesh_buffer() == null) return false;
            if (get_mesh_buffer().VertexCount == 0) return false;
            return true;
        }
             
        public override void OnRegisterSceneNode()
        {
            if (Visible)
                _mgr.RegisterNodeForRendering(this, SceneNodeRenderPass.Light);
            base.OnRegisterSceneNode();
        }

        
        public override void Render()
        {
            if (has_vertices())
            {
                //draw triangles
                Material.AmbientColor = new Color(255, 40, 40, 40);
                Material.DiffuseColor = new Color(255, 100, 255, 0);
                Material.SpecularColor = new Color(255, 255, 255, 255);
                Material.EmissiveColor = new Color(255, 0, 0, 0);
                Material.Wireframe = false;
                Material.Lighting = true;
                Material.BackfaceCulling = false;
                Material.NormalizeNormals = false;
                
                _driver.SetMaterial(Material);
                _driver.SetTransform(TransformationState.World, AbsoluteTransformation);

                _driver.DrawMeshBuffer(get_mesh_buffer());
                

                //draw lines                
                Material.Wireframe = true;
                Material.Lighting = false;
                Material.BackfaceCulling = false;
                Material.NormalizeNormals = false;

                _driver.SetMaterial(Material);

                _driver.DrawMeshBuffer(get_mesh_buffer());                
            }
        }

        public override Box3D BoundingBox
        {
            get
            {
                return _mesh.BoundingBox;
            }
        }

        public override uint MaterialCount
        {
            get
            {
                return 1;
            }
        }

        public override Material GetMaterial(int i)
        {
            return Material;
        }

        public void export_to_vrml(string filename)
        {
            if (has_vertices()==false)
            {
                System.Windows.Forms.MessageBox.Show("No hay grafica para exportar.");
                return;
            }

            System.IO.StreamWriter writer = new System.IO.StreamWriter(filename);
            
            // escribir encabezado
            writer.WriteLine("DEF laser_data_node Transform {");

            string buffer = "translation 0 0 0";
            writer.WriteLine(buffer);
            buffer = "rotation 1.0 0 0 0";
            writer.WriteLine(buffer);
            buffer = "scale 1.0 1.0 1.0";
            writer.WriteLine(buffer);
            buffer = "children [";
            writer.WriteLine(buffer);
            buffer = "DEF laser_data_mesh Group {";
            writer.WriteLine(buffer);
            buffer = "children [";
            writer.WriteLine(buffer);
            buffer = "Shape {";
            writer.WriteLine(buffer);
            buffer = "appearance Appearance {}";
            writer.WriteLine(buffer);
            buffer = "geometry IndexedFaceSet {";
            writer.WriteLine(buffer);
            buffer = "solid TRUE";
            writer.WriteLine(buffer);
            buffer = "coord DEF coord_Tube Coordinate {";
            writer.WriteLine(buffer);
            buffer = "point [";
            writer.WriteLine(buffer);
            // Escribir las coordenadas
            MeshBuffer mbuffer = get_mesh_buffer();
            for (uint i = 0; i < mbuffer.VertexCount; i++)
            {
                Vertex3D vert = mbuffer.GetVertex(i);
                buffer = vert.Position.X.ToString() + " " + vert.Position.Y.ToString() + " " + vert.Position.Z.ToString();
                writer.WriteLine(buffer);
            }


            buffer = "] } coordIndex [";
            writer.WriteLine(buffer);

            // Escribir los indices
            for (uint i = 0; i < mbuffer.IndexCount/3; i++)
            {
                ushort i0 = mbuffer.GetIndex(i * 3);
                ushort i1 = mbuffer.GetIndex(i * 3+1);
                ushort i2 = mbuffer.GetIndex(i * 3+2);

                buffer = i0.ToString() + " " + i1.ToString() + " " + i2.ToString() + " "+ i.ToString() + " " + "-1";
                writer.WriteLine(buffer);
            }


            buffer = "]	} } ] } ] }";
            writer.WriteLine(buffer);
            writer.Close();
            writer = null;

        }

        public void export_to_3ds(string filename)
        {
            if (has_vertices() == false)
            {
                System.Windows.Forms.MessageBox.Show("No hay grafica para exportar.");
                return;
            }
            get_mesh_buffer().ExportTo3DS(filename);
        }

        public void export_to_obj(string filename)
        {
            if (has_vertices() == false)
            {
                System.Windows.Forms.MessageBox.Show("No hay grafica para exportar.");
                return;
            }

            System.IO.StreamWriter writer = new System.IO.StreamWriter(filename);

            string buffer;
            // escribir encabezado
            writer.WriteLine("g laser_data_mesh");

            MeshBuffer mbuffer = get_mesh_buffer();

            // Escribir las coordenadas
            for (uint i = 0; i < mbuffer.VertexCount; i++)
            {
                Vertex3D vert = mbuffer.GetVertex(i);
                buffer = "v " + vert.Position.X.ToString() + " " + vert.Position.Y.ToString() + " " + vert.Position.Z.ToString();
                writer.WriteLine(buffer);
            }

            
            // Escribir las normales
            for (uint i = 0; i < mbuffer.VertexCount; i++)
            {
                Vertex3D vert = mbuffer.GetVertex(i);
                buffer = "vn " + vert.Normal.X.ToString() + " " + vert.Normal.Y.ToString() + " " + vert.Normal.Z.ToString();
                writer.WriteLine(buffer);
            }

           // Escribir los indices
            for (uint i = 0; i < mbuffer.IndexCount / 3; i++)
            {
                string i0 = (mbuffer.GetIndex(i * 3)+1).ToString();
                string i1 = (mbuffer.GetIndex(i * 3 + 1)+1).ToString();
                string i2 = (mbuffer.GetIndex(i * 3 + 2)+1).ToString();

                buffer = "f " + i0 + "//" + i0 + " " + i1 + "//" + i1 + " " + i2 + "//" + i2;
                writer.WriteLine(buffer);
            }

            writer.Close();
            writer = null;

        }


    }

    /// <summary>
    /// Clase para manejar una escena Irrlicht
    /// </summary>
    public class LaserViewer3D
    {
        public IrrlichtDevice _device;
        public SceneManager _scene;
        public VideoDriver _driver;
        public Control _target_window;
        public LaserDataSceneNode _model3D;
        public float rotation_factor = 0.5f;
        public float translation_factor = 0.2f;

        public LaserViewer3D()
        {        
        }

      

        public bool InitDevices(Control target_window)
        {
            _target_window = target_window;


            Dimension2D dim = new Dimension2D(target_window.Size.Width, target_window.Size.Height);

            _device = new IrrlichtDevice(DriverType.OpenGL, 
                                        dim,
                                        32, false, false, false,
                                        false,target_window.Handle);

            _device.Resizeable = true;
            
            //_device.OnEvent += new OnEventDelegate(_device_OnEvent);
            
            _scene = _device.SceneManager;
            _driver = _device.VideoDriver;

            return true;
        }

        public void rotateXY(int oldposX, int newposX, int oldposY, int newposY)
        {
            float xdiff = (newposX - oldposX) * rotation_factor;
            float ydiff = (newposY - oldposY) * rotation_factor;

            Vector3D rotation = new Vector3D(xdiff,0,ydiff);
                      
            _model3D.Rotation = _model3D.Rotation + rotation;
                  
        }

        public void translateXY(int oldposX, int newposX, int oldposY, int newposY)
        {
            float xdiff = (newposX - oldposX) * translation_factor;
            float ydiff = (newposY - oldposY) * translation_factor;

            Vector3D translation = new Vector3D(0,ydiff, xdiff);


            _model3D.Position = _model3D.Position + translation;

        }

        public void translateZ(int oldposZ, int newposZ)
        {
            float zdiff = (newposZ - oldposZ) * translation_factor;
            

            Vector3D translation = new Vector3D(zdiff,0 , 0);

            _model3D.Position = _model3D.Position + translation;

            
        }


        public bool init_scene()
        {
            

            _scene.AddCameraSceneNode(null);

            _scene.ActiveCamera.Position = new Vector3D(0, -15, 0);
            _scene.ActiveCamera.Target = new Vector3D();

            _scene.AddLightSceneNode(null, new Vector3D(20, -15, 3), new Colorf(1.0f, 1.0f, 1.0f, 1.0f), 100.0f, -1);

            _scene.AddLightSceneNode(null, new Vector3D(-20, -15, -4), new Colorf(1.0f, 1.0f, 1.0f, 1.0f), 100.0f, -1);

            _model3D = new LaserDataSceneNode(null, _scene, 666);
            
            return true;
        }

        public void resize_video()
        {
           /* Dimension2D dim = new Dimension2D(this._target_window.Size.Width, this._target_window.Size.Height);

            _driver.ScreenSize.Set(dim.Width, dim.Height);

            _driver.ViewPort.Set(new Position2D(0,0),dim);*/
            
            
        }

        public void render_scene()
        {
      
            
            _device.Run();           

            _driver.BeginScene(true, true, Color.Gray);

      
            _scene.DrawAll();
            
            _driver.EndScene();
        }
    }
}
