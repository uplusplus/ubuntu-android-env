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
        public LMSAPI_DATA laser_data = null;
        public int angular_range = 100;
        int angular_resolution = 100;
        int distance_range = 8;
        float scale_distance = 1000.0f;// en metros

        public LMSAPIForm()
        {
            InitializeComponent();
        }
           

        public void transformMathCoordToGUICoord(
            PointF srccoord,ref Point dstcoord,Rectangle area)
        {
	        int width = area.Right-area.Left;
	        int height = area.Bottom - area.Top;
	        dstcoord.X = (int)((1.0f + srccoord.X)*0.5f*width + area.Left);
	        dstcoord.Y = (int)((1.0f - srccoord.Y)*0.5f*height + area.Top);// invertir Y
        }

        public void rotatePoint(PointF centerpoint, float angle, ref PointF dstpoint)
        {
            System.Drawing.Drawing2D.Matrix mat = new System.Drawing.Drawing2D.Matrix();
            mat.RotateAt(angle, centerpoint);
            PointF[] arraypoints = new PointF[1];
            arraypoints[0] =  dstpoint;
            mat.TransformPoints(arraypoints);
            dstpoint = arraypoints[0];
        }

        public void drawGridLines(System.Drawing.Graphics g, Pen gradepen,float interval_dis, int rango_angular,PointF startvec, PointF center, Rectangle area)
        {
            float radius_len = interval_dis;
            Point linea1 = new Point();
            Point linea2 = new Point();
            while (radius_len <= 1.01f)
            {
                PointF vec = new PointF(startvec.X, startvec.Y);
                PointF oldvec = new PointF();
                for (int ang = 0; ang <= rango_angular; ang += 10)
                {
                    PointF radiusvec = new PointF(radius_len * (vec.X - center.X), radius_len * (vec.Y - center.Y));
                    PointF radius_point = new PointF(radiusvec.X + center.X, radiusvec.Y + center.Y);

                    if (ang > 0)
                    {
                        
                        transformMathCoordToGUICoord(oldvec, ref linea1, area);
                        transformMathCoordToGUICoord(radius_point, ref linea2, area);
                        g.DrawLine(gradepen, linea1, linea2);
                    }
                    rotatePoint(center, 10.0f, ref vec);
                    oldvec = new PointF(radius_point.X, radius_point.Y);
                }

                radius_len += interval_dis;
            }

        }

        public void request_laser_data()
        {
            int port = cbPuerto.SelectedIndex + 1;
            

            angular_range = 100;
            if (cbRangoAngular.SelectedIndex == 1) angular_range = 180;

            angular_resolution = 100;
            if (cbResAngular.SelectedIndex == 1) angular_resolution = 50;
            else if (cbResAngular.SelectedIndex == 2) angular_resolution = 25;

            distance_range = 8;
            if (cbRangoDistancia.SelectedIndex == 1) distance_range = 80;

            //obtenemos los datos de medición simulados

            // Conectarnos con el sensor
            LMSAPI_CONNECTION connection = LMSAPI_LIB.create_connection_object(port, angular_range, angular_resolution, distance_range, 0);

            if (connection == null)
            {
                System.Windows.Forms.MessageBox.Show("Error!!:Configuraciòn erronea del sensor, no puede establecerse conexiòn.");
                return;
            }

            connection.m_scale_distance = scale_distance;//distancias en metros

            laser_data = connection.request_measurement();
            if (laser_data == null)
            {
                System.Windows.Forms.MessageBox.Show("Error al solicitar datos de mediciòn");
                return;
            }

            //cerrar la conexion
            connection = null;
        }

        public void renderdata(Graphics g)
        {
            float scale_radius = 1.5f;
            Rectangle area = new Rectangle(0, 0, (int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height-5);

            g.Clear(Color.White);

            Pen radlines = new Pen(Color.FromArgb(0, 0, 0), 1.0f);

            //!Dibujar rango angular- lineas guia
            Point linea1 = new Point();
            Point linea2 = new Point();
		    PointF center = new PointF(0.0f,-1.0f);
		    PointF startvec= new PointF(scale_radius,-1.0f);

		    //Obtener el rango angular
		    
		    if( angular_range==100)
		    {
                rotatePoint(center,40.0f,ref startvec);
		    }
		    

		    PointF vec = new PointF(startvec.X,startvec.Y);
            PointF oldvec = new PointF();
		    int ang;
            for (ang = 0; ang <= angular_range; ang += 10)
		    {
			    //dibujar linea                
                transformMathCoordToGUICoord(center, ref linea1, area);
                transformMathCoordToGUICoord(vec, ref linea2, area);

                g.DrawLine(radlines,linea1,linea2);
			    oldvec = new PointF(vec.X,vec.Y);
                rotatePoint(center, 10.0f, ref vec);
		    }

		    //Dibujar lineas concentricas

            float rango_distancia = 0;
		    float interval_dis;
		    if(distance_range==8)
		    {
			    rango_distancia = 8*scale_range;
                interval_dis = 1.0f / rango_distancia;                
		    }
		    else
		    {
                rango_distancia = 80*scale_range;
                interval_dis = 4.0f / rango_distancia;                
		    }

            Pen gradepen = new Pen(Brushes.LightGray, 0.5f);
            if (rango_distancia <= 2.0f)
            {                
                drawGridLines(g, gradepen, interval_dis * 0.02f, angular_range, startvec, center, area);
            }            
		    gradepen = new Pen(Color.FromArgb(120, 120, 120), 1.0f);
            drawGridLines(g, gradepen, interval_dis * 0.1f, angular_range, startvec, center, area);

            gradepen = new Pen(Color.FromArgb(100, 100, 100), 2.0f);
            drawGridLines(g, gradepen, interval_dis, angular_range, startvec, center, area);


            //Dibujar la grafica de datos

            if (laser_data != null)
            {
                gradepen = new Pen(Brushes.Green, 2.0f);
                float radius_len = 0;

                vec = new PointF(startvec.X, startvec.Y);
                oldvec = new PointF();

                //obtener resolución angular
                float resolution = laser_data.get_resolution();
                //obtener cantidad de datos
                uint distance_count = laser_data.get_distance_count();

                for (ang = 0; ang < distance_count; ang ++)
                {
                    radius_len = laser_data.get_distance(ang);//obtener medida de distancia

                    radius_len /= rango_distancia;

                    PointF radiusvec = new PointF(radius_len * (vec.X - center.X), radius_len * (vec.Y - center.Y));
                    PointF radius_point = new PointF(radiusvec.X + center.X, radiusvec.Y + center.Y);

                    if (ang > 0)
                    {

                        transformMathCoordToGUICoord(oldvec, ref linea1, area);
                        transformMathCoordToGUICoord(radius_point, ref linea2, area);
                        g.DrawLine(gradepen, linea1, linea2);
                    }
                    rotatePoint(center, resolution, ref vec);
                    oldvec = new PointF(radius_point.X, radius_point.Y);
                }

            }

		    //dibujar rotulo de distancia
            vec.X = 0.0f;
            vec.Y = -1.0f + scale_radius;
            transformMathCoordToGUICoord(vec, ref linea1, area);
            
            string rotulodis = rango_distancia.ToString() + "m";
            Font ft = new Font("courier new",10.0f);
            g.DrawString(rotulodis, ft, System.Drawing.Brushes.Black, (float)linea1.X, (float)linea1.Y-15);
        }


        public void rendercontext()
        {
            gfx = null;
            Rectangle rc = new Rectangle(0, 0, canvasptc.Width, canvasptc.Height);
            gfx = context.Allocate(canvasptc.CreateGraphics(),rc);
            renderdata(gfx.Graphics);
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
        }

        private void canvasptc_Paint(object sender, PaintEventArgs e)
        {
            if (gfx != null)
            {               
                gfx.Render(e.Graphics);
            }
        }

        public void resize_canvas()
        {
            int w = this.Width - canvasptc.Left - 20;
            int h = this.Height - canvasptc.Top - 40;

            canvasptc.Width = w;
            canvasptc.Height = h;
            rendercontext();
            canvasptc.Refresh();
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
                System.Drawing.Bitmap saveimage = new Bitmap(1600, 1600);

                canvasptc.Width = saveimage.Width;
                canvasptc.Height = saveimage.Height;
                rendercontext();
                canvasptc.Refresh();
                canvasptc.DrawToBitmap(saveimage,
                    new Rectangle(0, 0, saveimage.Width, saveimage.Height));
                saveimage.Save(newfile, System.Drawing.Imaging.ImageFormat.Jpeg);
                resize_canvas();
            }
            
        }

        private void btnGraficar_Click(object sender, EventArgs e)
        {
            request_laser_data();
            rendercontext();
            canvasptc.Refresh();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            scale_range = 1.0f/(float)numericUpDown1.Value;
            rendercontext();
            canvasptc.Refresh();
        }
     
    }
}