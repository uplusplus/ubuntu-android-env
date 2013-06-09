using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LMSAPI;

namespace LMSAPI_csharp
{
    public partial class LMSAPIForm : Form
    {
        public System.Drawing.BufferedGraphics gfx;
        public System.Drawing.BufferedGraphicsContext context;
        public float scale_range = 1.0f;
        private uint capture_index = 0;
        public LaserData3D laser_data;


        public int old_rot_x = 0;
        public int old_rot_y = 0;
        public bool dragging_rot = false;
        public int old_pos_x = 0;
        public int old_pos_y = 0;
        public bool dragging_trans = false;

        public int old_pos_z = 0;        
        public bool dragging_z = false;

        /// <summary>
        /// Control laser
        /// </summary>
        public LMSAPIControl laser_control = null;

        /// <summary>
        /// Control Irrlicht 3D
        /// </summary>
        public LaserViewer3D laser_viewer3D = null;

        public LMSAPIForm()
        {
            InitializeComponent();

            laser_control = new LMSAPIControl();
            laser_data = new LaserData3D();
        }

        public void create_capture_model()
        {
            //close the connection
            laser_control.end_connection();
            //fill the model
            laser_data.export_to_model(laser_viewer3D._model3D);
        }

        public void request_laser_data()
        {
            laser_control.port = cbPuerto.SelectedIndex + 1;

            laser_control.angular_range = 100;
            if (cbRangoAngular.SelectedIndex == 1) laser_control.angular_range = 180;

            laser_control.angular_resolution = 100;
            if (cbResAngular.SelectedIndex == 1) laser_control.angular_resolution = 50;
            else if (cbResAngular.SelectedIndex == 2) laser_control.angular_resolution = 25;

            laser_control.distance_range = 8;
            if (cbRangoDistancia.SelectedIndex == 1) laser_control.distance_range = 80;

            // Establecer número de mediciones
            laser_data.m_vertical_sampler_count = (uint)udNumSamples.Value;
            laser_data.m_vertical_sampler_min = (float)udMinRange.Value;
            laser_data.m_vertical_sampler_max = (float)udMaxRange.Value;
            laser_data.m_vertical_sampler_angular = radioBAngular.Checked;
            laser_data.m_mesh_resolution = (uint)udMeshResolution.Value;


            System.Windows.Forms.MessageBox.Show("Confirmar Medición");         

            //obtenemos los datos de medición simulados
            if (checkSimulation.Checked == false)
            {
                // Conectarnos con el sensor
                if (!laser_control.stablish_connection())
                {
                    return;
                }
                laser_control.request_laser_data();

            }
            else
            {
                //obtener datos simulados
                laser_control.simulate_laser_data();
            }
                        
            laser_data.begin_capturing(laser_control.laser_data);
            timerLaser.Enabled = true;
            capture_index = 0;
        }

        
        private void initViewer3D()
        {
            laser_viewer3D = new LaserViewer3D();
            laser_viewer3D.InitDevices(canvasptc);
            laser_viewer3D.init_scene();
            timer3D.Enabled = true;
        }

        private void LMSAPIForm_Load(object sender, EventArgs e)
        {
            this.cbRangoAngular.SelectedIndex = 0;
            this.cbRangoDistancia.SelectedIndex = 0;
            this.cbResAngular.SelectedIndex = 0;
            this.cbPuerto.SelectedIndex = 0;

            //crear herramientas graficas
            context = BufferedGraphicsManager.Current;
            gfx = null;
            resize_canvas();

            initViewer3D();
        }

        private void canvasptc_Paint(object sender, PaintEventArgs e)
        {            
        }

        public void resize_canvas()
        {
            int w = this.Width - canvasptc.Left - 20;
            int h = this.Height - canvasptc.Top - 40;

            canvasptc.Width = w;
            canvasptc.Height = h;
            if (laser_viewer3D!=null)
            {
                laser_viewer3D.resize_video();
            }
        }

        private void LMSAPIForm_Resize(object sender, EventArgs e)
        {
            this.resize_canvas();
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            ExportImgDialog.ShowDialog();
            string newfile = ExportImgDialog.FileName;
            if (newfile.Length > 0)
            {
                if (ExportImgDialog.FilterIndex == 1)
                {
                    laser_viewer3D._model3D.export_to_obj(newfile);
                }
                else if (ExportImgDialog.FilterIndex == 2)
                {
                    laser_viewer3D._model3D.export_to_3ds(newfile);
                }
                else if (ExportImgDialog.FilterIndex == 3)
                {
                    laser_viewer3D._model3D.export_to_vrml(newfile);
                }
                
                
                
            }
            
        }

        private void btnGraficar_Click(object sender, EventArgs e)
        {
            request_laser_data();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RangeGroup.Text = "Rango en Grados";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RangeGroup.Text = "Rango en Centímetros";
        }

        

        private void timer3D_Tick(object sender, EventArgs e)
        {
            if (laser_viewer3D != null)
            {
                laser_viewer3D.render_scene();
            }
        }

        
        private void canvasptc_MouseMove(object sender, MouseEventArgs e)
        {
            if(dragging_rot)
            {
                if (dragging_z)
                {
                    laser_viewer3D.translateZ(old_rot_y, e.Y);                
                }
                else
                {
                    laser_viewer3D.rotateXY(old_rot_x, e.X, old_rot_y, e.Y);  
                }
                
                old_rot_x = e.X;
                old_rot_y = e.Y;
            }
            
            if (dragging_trans)
            {                
                laser_viewer3D.translateXY(old_pos_x, e.X, old_pos_y, e.Y);
                old_pos_x = e.X;
                old_pos_y = e.Y;
            }
        }

        private void canvasptc_MouseDown(object sender, MouseEventArgs e)
        {            
            if (e.Button == MouseButtons.Left)
            {   
                old_rot_x = e.X;
                old_rot_y = e.Y;
                dragging_rot = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                old_pos_x = e.X;
                old_pos_y = e.Y;                
                dragging_trans = true;
            }
        }

        private void canvasptc_MouseUp(object sender, MouseEventArgs e)
        {            
            if (e.Button == MouseButtons.Left)
            {
                dragging_rot = false;
                dragging_z = false;
            }
            if (e.Button == MouseButtons.Right)
            {
                dragging_trans = false;
            }
        }

                
        private void timerLaser_Tick(object sender, EventArgs e)
        {
            timerLaser.Enabled = false;
            System.Windows.Forms.MessageBox.Show("Confirmar Medición");
            laser_control.request_laser_data();
            bool is_capturing = laser_data.capture_data(laser_control.laser_data);
            capture_index++;
            if (capture_index >= laser_data.m_vertical_sampler_count)
            {
                timerLaser.Enabled = false;
                create_capture_model();
                return;
            }
            timerLaser.Enabled = true;
            
        }

        private void LMSAPIForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                dragging_z = true;
            }
        }

        private void LMSAPIForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                dragging_z = false;
            }
        }

        private void udMinRange_ValueChanged(object sender, EventArgs e)
        {

        }

        private void LMSAPIForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Autor Francisco León \n Universidad Manuela Beltrán");
        }     
    }
}