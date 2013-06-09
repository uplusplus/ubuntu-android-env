<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cbPuerto = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cbRangoAngular = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cbResAngular = New System.Windows.Forms.ComboBox
        Me.btMedir = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'cbPuerto
        '
        Me.cbPuerto.FormattingEnabled = True
        Me.cbPuerto.Items.AddRange(New Object() {"COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10", "COM11", "COM12"})
        Me.cbPuerto.Location = New System.Drawing.Point(12, 44)
        Me.cbPuerto.Name = "cbPuerto"
        Me.cbPuerto.Size = New System.Drawing.Size(121, 21)
        Me.cbPuerto.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Puerto"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 91)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Rango Angular"
        '
        'cbRangoAngular
        '
        Me.cbRangoAngular.FormattingEnabled = True
        Me.cbRangoAngular.Items.AddRange(New Object() {"100", "180"})
        Me.cbRangoAngular.Location = New System.Drawing.Point(12, 107)
        Me.cbRangoAngular.Name = "cbRangoAngular"
        Me.cbRangoAngular.Size = New System.Drawing.Size(121, 21)
        Me.cbRangoAngular.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 152)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(152, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Resoulcion  Angular en grados"
        '
        'cbResAngular
        '
        Me.cbResAngular.FormattingEnabled = True
        Me.cbResAngular.Items.AddRange(New Object() {"1º", "0.5º", "0.25º"})
        Me.cbResAngular.Location = New System.Drawing.Point(12, 168)
        Me.cbResAngular.Name = "cbResAngular"
        Me.cbResAngular.Size = New System.Drawing.Size(121, 21)
        Me.cbResAngular.TabIndex = 4
        '
        'btMedir
        '
        Me.btMedir.Location = New System.Drawing.Point(159, 41)
        Me.btMedir.Name = "btMedir"
        Me.btMedir.Size = New System.Drawing.Size(121, 23)
        Me.btMedir.TabIndex = 6
        Me.btMedir.Text = "Realizar Medicion"
        Me.btMedir.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.btMedir)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cbResAngular)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cbRangoAngular)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cbPuerto)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbPuerto As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbRangoAngular As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cbResAngular As System.Windows.Forms.ComboBox
    Friend WithEvents btMedir As System.Windows.Forms.Button

End Class
