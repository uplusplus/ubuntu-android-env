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
            this.components = new System.ComponentModel.Container();
            this.btnGraficar = new System.Windows.Forms.Button();
            this.canvasptc = new System.Windows.Forms.PictureBox();
            this.btExport = new System.Windows.Forms.Button();
            this.ExportImgDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabConfiguration = new System.Windows.Forms.TabControl();
            this.tabConnectionConfig = new System.Windows.Forms.TabPage();
            this.checkSimulation = new System.Windows.Forms.CheckBox();
            this.cbPuerto = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbRangoDistancia = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbResAngular = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbRangoAngular = new System.Windows.Forms.ComboBox();
            this.lbrango = new System.Windows.Forms.Label();
            this.tabBarrido = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.udMeshResolution = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.udNumSamples = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.RangeGroup = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.udMaxRange = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.udMinRange = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioBAngular = new System.Windows.Forms.RadioButton();
            this.radioBVertical = new System.Windows.Forms.RadioButton();
            this.timer3D = new System.Windows.Forms.Timer(this.components);
            this.timerLaser = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.canvasptc)).BeginInit();
            this.tabConfiguration.SuspendLayout();
            this.tabConnectionConfig.SuspendLayout();
            this.tabBarrido.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMeshResolution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udNumSamples)).BeginInit();
            this.RangeGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMaxRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMinRange)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            // canvasptc
            // 
            this.canvasptc.Location = new System.Drawing.Point(225, 10);
            this.canvasptc.Name = "canvasptc";
            this.canvasptc.Size = new System.Drawing.Size(384, 134);
            this.canvasptc.TabIndex = 7;
            this.canvasptc.TabStop = false;
            this.canvasptc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvasptc_MouseDown);
            this.canvasptc.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvasptc_MouseMove);
            this.canvasptc.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvasptc_MouseUp);
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
            this.ExportImgDialog.Filter = "(*.obj)|*.obj|(*.3ds)|*.3ds|(*.wrl)|*.wrl";
            // 
            // tabConfiguration
            // 
            this.tabConfiguration.Controls.Add(this.tabConnectionConfig);
            this.tabConfiguration.Controls.Add(this.tabBarrido);
            this.tabConfiguration.Location = new System.Drawing.Point(15, 80);
            this.tabConfiguration.Name = "tabConfiguration";
            this.tabConfiguration.SelectedIndex = 0;
            this.tabConfiguration.Size = new System.Drawing.Size(204, 367);
            this.tabConfiguration.TabIndex = 13;
            // 
            // tabConnectionConfig
            // 
            this.tabConnectionConfig.Controls.Add(this.checkSimulation);
            this.tabConnectionConfig.Controls.Add(this.cbPuerto);
            this.tabConnectionConfig.Controls.Add(this.label4);
            this.tabConnectionConfig.Controls.Add(this.cbRangoDistancia);
            this.tabConnectionConfig.Controls.Add(this.label2);
            this.tabConnectionConfig.Controls.Add(this.cbResAngular);
            this.tabConnectionConfig.Controls.Add(this.label1);
            this.tabConnectionConfig.Controls.Add(this.cbRangoAngular);
            this.tabConnectionConfig.Controls.Add(this.lbrango);
            this.tabConnectionConfig.Location = new System.Drawing.Point(4, 22);
            this.tabConnectionConfig.Name = "tabConnectionConfig";
            this.tabConnectionConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabConnectionConfig.Size = new System.Drawing.Size(196, 341);
            this.tabConnectionConfig.TabIndex = 0;
            this.tabConnectionConfig.Text = "Conexion";
            this.tabConnectionConfig.UseVisualStyleBackColor = true;
            // 
            // checkSimulation
            // 
            this.checkSimulation.AutoSize = true;
            this.checkSimulation.Location = new System.Drawing.Point(10, 276);
            this.checkSimulation.Name = "checkSimulation";
            this.checkSimulation.Size = new System.Drawing.Size(134, 17);
            this.checkSimulation.TabIndex = 23;
            this.checkSimulation.Text = "Usar valores simulados";
            this.checkSimulation.UseVisualStyleBackColor = true;
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
            this.cbPuerto.Location = new System.Drawing.Point(9, 231);
            this.cbPuerto.Name = "cbPuerto";
            this.cbPuerto.Size = new System.Drawing.Size(121, 21);
            this.cbPuerto.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Puerto COM";
            // 
            // cbRangoDistancia
            // 
            this.cbRangoDistancia.FormattingEnabled = true;
            this.cbRangoDistancia.Items.AddRange(new object[] {
            "8m",
            "80m"});
            this.cbRangoDistancia.Location = new System.Drawing.Point(9, 168);
            this.cbRangoDistancia.Name = "cbRangoDistancia";
            this.cbRangoDistancia.Size = new System.Drawing.Size(121, 21);
            this.cbRangoDistancia.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Rango de Distancia";
            // 
            // cbResAngular
            // 
            this.cbResAngular.FormattingEnabled = true;
            this.cbResAngular.Items.AddRange(new object[] {
            "1",
            "0.5",
            "0.25"});
            this.cbResAngular.Location = new System.Drawing.Point(9, 102);
            this.cbResAngular.Name = "cbResAngular";
            this.cbResAngular.Size = new System.Drawing.Size(121, 21);
            this.cbResAngular.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Resolución Angular";
            // 
            // cbRangoAngular
            // 
            this.cbRangoAngular.FormattingEnabled = true;
            this.cbRangoAngular.Items.AddRange(new object[] {
            "100",
            "180"});
            this.cbRangoAngular.Location = new System.Drawing.Point(9, 40);
            this.cbRangoAngular.Name = "cbRangoAngular";
            this.cbRangoAngular.Size = new System.Drawing.Size(121, 21);
            this.cbRangoAngular.TabIndex = 14;
            // 
            // lbrango
            // 
            this.lbrango.AutoSize = true;
            this.lbrango.Location = new System.Drawing.Point(6, 12);
            this.lbrango.Name = "lbrango";
            this.lbrango.Size = new System.Drawing.Size(78, 13);
            this.lbrango.TabIndex = 13;
            this.lbrango.Text = "Rango Angular";
            // 
            // tabBarrido
            // 
            this.tabBarrido.Controls.Add(this.groupBox3);
            this.tabBarrido.Controls.Add(this.RangeGroup);
            this.tabBarrido.Controls.Add(this.groupBox1);
            this.tabBarrido.Location = new System.Drawing.Point(4, 22);
            this.tabBarrido.Name = "tabBarrido";
            this.tabBarrido.Padding = new System.Windows.Forms.Padding(3);
            this.tabBarrido.Size = new System.Drawing.Size(196, 341);
            this.tabBarrido.TabIndex = 1;
            this.tabBarrido.Text = "Barrido";
            this.tabBarrido.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.udMeshResolution);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.udNumSamples);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(7, 174);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(183, 95);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Resolución";
            // 
            // udMeshResolution
            // 
            this.udMeshResolution.Location = new System.Drawing.Point(6, 59);
            this.udMeshResolution.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.udMeshResolution.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udMeshResolution.Name = "udMeshResolution";
            this.udMeshResolution.Size = new System.Drawing.Size(50, 20);
            this.udMeshResolution.TabIndex = 4;
            this.udMeshResolution.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(62, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Resolución de malla";
            // 
            // udNumSamples
            // 
            this.udNumSamples.Location = new System.Drawing.Point(6, 21);
            this.udNumSamples.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.udNumSamples.Name = "udNumSamples";
            this.udNumSamples.Size = new System.Drawing.Size(50, 20);
            this.udNumSamples.TabIndex = 2;
            this.udNumSamples.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(62, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Número de mediciones";
            // 
            // RangeGroup
            // 
            this.RangeGroup.Controls.Add(this.label5);
            this.RangeGroup.Controls.Add(this.udMaxRange);
            this.RangeGroup.Controls.Add(this.label3);
            this.RangeGroup.Controls.Add(this.udMinRange);
            this.RangeGroup.Location = new System.Drawing.Point(7, 88);
            this.RangeGroup.Name = "RangeGroup";
            this.RangeGroup.Size = new System.Drawing.Size(183, 80);
            this.RangeGroup.TabIndex = 3;
            this.RangeGroup.TabStop = false;
            this.RangeGroup.Text = "Rango en Grados";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(87, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Rango Máximo";
            // 
            // udMaxRange
            // 
            this.udMaxRange.DecimalPlaces = 1;
            this.udMaxRange.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.udMaxRange.Location = new System.Drawing.Point(6, 47);
            this.udMaxRange.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udMaxRange.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.udMaxRange.Name = "udMaxRange";
            this.udMaxRange.Size = new System.Drawing.Size(75, 20);
            this.udMaxRange.TabIndex = 2;
            this.udMaxRange.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(87, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Rango Mínimo";
            // 
            // udMinRange
            // 
            this.udMinRange.DecimalPlaces = 1;
            this.udMinRange.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.udMinRange.Location = new System.Drawing.Point(6, 19);
            this.udMinRange.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.udMinRange.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.udMinRange.Name = "udMinRange";
            this.udMinRange.Size = new System.Drawing.Size(75, 20);
            this.udMinRange.TabIndex = 0;
            this.udMinRange.Value = new decimal(new int[] {
            30,
            0,
            0,
            -2147483648});
            this.udMinRange.ValueChanged += new System.EventHandler(this.udMinRange_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioBAngular);
            this.groupBox1.Controls.Add(this.radioBVertical);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 76);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Forma del Barrido";
            // 
            // radioBAngular
            // 
            this.radioBAngular.AutoSize = true;
            this.radioBAngular.Checked = true;
            this.radioBAngular.Location = new System.Drawing.Point(17, 19);
            this.radioBAngular.Name = "radioBAngular";
            this.radioBAngular.Size = new System.Drawing.Size(61, 17);
            this.radioBAngular.TabIndex = 0;
            this.radioBAngular.TabStop = true;
            this.radioBAngular.Text = "Angular";
            this.radioBAngular.UseVisualStyleBackColor = true;
            this.radioBAngular.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioBVertical
            // 
            this.radioBVertical.AutoSize = true;
            this.radioBVertical.Location = new System.Drawing.Point(17, 42);
            this.radioBVertical.Name = "radioBVertical";
            this.radioBVertical.Size = new System.Drawing.Size(60, 17);
            this.radioBVertical.TabIndex = 1;
            this.radioBVertical.Text = "Vertical";
            this.radioBVertical.UseVisualStyleBackColor = true;
            this.radioBVertical.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // timer3D
            // 
            this.timer3D.Interval = 1;
            this.timer3D.Tick += new System.EventHandler(this.timer3D_Tick);
            // 
            // timerLaser
            // 
            this.timerLaser.Tick += new System.EventHandler(this.timerLaser_Tick);
            // 
            // LMSAPIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 473);
            this.Controls.Add(this.tabConfiguration);
            this.Controls.Add(this.btExport);
            this.Controls.Add(this.canvasptc);
            this.Controls.Add(this.btnGraficar);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(702, 500);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "LMSAPIForm";
            this.Text = "Medición de Sólidos con Láser";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LMSAPIForm_FormClosed);
            this.Resize += new System.EventHandler(this.LMSAPIForm_Resize);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LMSAPIForm_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LMSAPIForm_KeyDown);
            this.Load += new System.EventHandler(this.LMSAPIForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.canvasptc)).EndInit();
            this.tabConfiguration.ResumeLayout(false);
            this.tabConnectionConfig.ResumeLayout(false);
            this.tabConnectionConfig.PerformLayout();
            this.tabBarrido.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMeshResolution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udNumSamples)).EndInit();
            this.RangeGroup.ResumeLayout(false);
            this.RangeGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udMaxRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMinRange)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGraficar;
        private System.Windows.Forms.PictureBox canvasptc;
        private System.Windows.Forms.Button btExport;
        private System.Windows.Forms.SaveFileDialog ExportImgDialog;
        private System.Windows.Forms.TabControl tabConfiguration;
        private System.Windows.Forms.TabPage tabConnectionConfig;
        private System.Windows.Forms.TabPage tabBarrido;
        private System.Windows.Forms.ComboBox cbPuerto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbRangoDistancia;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbResAngular;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbRangoAngular;
        private System.Windows.Forms.Label lbrango;
        private System.Windows.Forms.CheckBox checkSimulation;
        private System.Windows.Forms.RadioButton radioBVertical;
        private System.Windows.Forms.RadioButton radioBAngular;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox RangeGroup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown udMaxRange;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown udMinRange;
        private System.Windows.Forms.NumericUpDown udNumSamples;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer timer3D;
        private System.Windows.Forms.Timer timerLaser;
        private System.Windows.Forms.NumericUpDown udMeshResolution;
        private System.Windows.Forms.Label label7;
    }
}

