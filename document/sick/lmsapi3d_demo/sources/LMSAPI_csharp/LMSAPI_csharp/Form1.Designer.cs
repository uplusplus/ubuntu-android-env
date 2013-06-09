namespace LMSAPI_csharp
{
    partial class LMSAPIForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {            
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbrango = new System.Windows.Forms.Label();
            this.cbRangoAngular = new System.Windows.Forms.ComboBox();
            this.btnGraficar = new System.Windows.Forms.Button();
            this.cbResAngular = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbRangoDistancia = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.canvasptc = new System.Windows.Forms.PictureBox();
            this.btExport = new System.Windows.Forms.Button();
            this.ExportImgDialog = new System.Windows.Forms.SaveFileDialog();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cbPuerto = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.canvasptc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbrango
            // 
            this.lbrango.AutoSize = true;
            this.lbrango.Location = new System.Drawing.Point(12, 134);
            this.lbrango.Name = "lbrango";
            this.lbrango.Size = new System.Drawing.Size(78, 13);
            this.lbrango.TabIndex = 0;
            this.lbrango.Text = "Rango Angular";
            // 
            // cbRangoAngular
            // 
            this.cbRangoAngular.FormattingEnabled = true;
            this.cbRangoAngular.Items.AddRange(new object[] {
            "100",
            "180"});
            this.cbRangoAngular.Location = new System.Drawing.Point(15, 162);
            this.cbRangoAngular.Name = "cbRangoAngular";
            this.cbRangoAngular.Size = new System.Drawing.Size(121, 21);
            this.cbRangoAngular.TabIndex = 1;
            // 
            // btnGraficar
            // 
            this.btnGraficar.Location = new System.Drawing.Point(15, 13);
            this.btnGraficar.Name = "btnGraficar";
            this.btnGraficar.Size = new System.Drawing.Size(121, 23);
            this.btnGraficar.TabIndex = 2;
            this.btnGraficar.Text = "Graficar";
            this.btnGraficar.UseVisualStyleBackColor = true;
            this.btnGraficar.Click += new System.EventHandler(this.btnGraficar_Click);
            // 
            // cbResAngular
            // 
            this.cbResAngular.FormattingEnabled = true;
            this.cbResAngular.Items.AddRange(new object[] {
            "1",
            "0.5",
            "0.25"});
            this.cbResAngular.Location = new System.Drawing.Point(15, 224);
            this.cbResAngular.Name = "cbResAngular";
            this.cbResAngular.Size = new System.Drawing.Size(121, 21);
            this.cbResAngular.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 196);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Resolución Angular";
            // 
            // cbRangoDistancia
            // 
            this.cbRangoDistancia.FormattingEnabled = true;
            this.cbRangoDistancia.Items.AddRange(new object[] {
            "8m",
            "80m"});
            this.cbRangoDistancia.Location = new System.Drawing.Point(15, 290);
            this.cbRangoDistancia.Name = "cbRangoDistancia";
            this.cbRangoDistancia.Size = new System.Drawing.Size(121, 21);
            this.cbRangoDistancia.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 262);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Rango de Distancia";
            // 
            // canvasptc
            // 
            this.canvasptc.Location = new System.Drawing.Point(143, 13);
            this.canvasptc.Name = "canvasptc";
            this.canvasptc.Size = new System.Drawing.Size(384, 340);
            this.canvasptc.TabIndex = 7;
            this.canvasptc.TabStop = false;
            this.canvasptc.Paint += new System.Windows.Forms.PaintEventHandler(this.canvasptc_Paint);
            // 
            // btExport
            // 
            this.btExport.Location = new System.Drawing.Point(15, 51);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(121, 23);
            this.btExport.TabIndex = 8;
            this.btExport.Text = "Exportar a archivo";
            this.btExport.UseVisualStyleBackColor = true;
            this.btExport.Click += new System.EventHandler(this.btExport_Click);
            // 
            // ExportImgDialog
            // 
            this.ExportImgDialog.Filter = "(*.jpg)|*.jpg";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 1;
            this.numericUpDown1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown1.Location = new System.Drawing.Point(71, 100);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            160,
            0,
            0,
            65536});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown1.TabIndex = 9;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Zoom";
            // 
            // cbPuerto
            // 
            this.cbPuerto.FormattingEnabled = true;
            this.cbPuerto.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.cbPuerto.Location = new System.Drawing.Point(15, 353);
            this.cbPuerto.Name = "cbPuerto";
            this.cbPuerto.Size = new System.Drawing.Size(121, 21);
            this.cbPuerto.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 325);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Puerto COM";
            // 
            // LMSAPIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 395);
            this.Controls.Add(this.cbPuerto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.btExport);
            this.Controls.Add(this.canvasptc);
            this.Controls.Add(this.cbRangoDistancia);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbResAngular);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGraficar);
            this.Controls.Add(this.cbRangoAngular);
            this.Controls.Add(this.lbrango);
            this.Name = "LMSAPIForm";
            this.Text = "LMSAPI graphics";
            this.Resize += new System.EventHandler(this.LMSAPIForm_Resize);
            this.Load += new System.EventHandler(this.LMSAPIForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.canvasptc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbrango;
        private System.Windows.Forms.ComboBox cbRangoAngular;
        private System.Windows.Forms.Button btnGraficar;
        private System.Windows.Forms.ComboBox cbResAngular;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbRangoDistancia;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox canvasptc;
        private System.Windows.Forms.Button btExport;
        private System.Windows.Forms.SaveFileDialog ExportImgDialog;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbPuerto;
        private System.Windows.Forms.Label label4;
    }
}

