<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ElementsFrm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pE = New System.Windows.Forms.Panel()
        Me.pbActive = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMaat = New System.Windows.Forms.TextBox()
        Me.btnHorizontaal = New System.Windows.Forms.Button()
        Me.btnVertikaal = New System.Windows.Forms.Button()
        Me.btnTekstV = New System.Windows.Forms.Button()
        Me.btnTekstH = New System.Windows.Forms.Button()
        Me.txtLengte = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pbtekst = New System.Windows.Forms.PictureBox()
        CType(Me.pbActive, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbtekst, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pE
        '
        Me.pE.BackColor = System.Drawing.SystemColors.Window
        Me.pE.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pE.Location = New System.Drawing.Point(0, 87)
        Me.pE.Name = "pE"
        Me.pE.Size = New System.Drawing.Size(294, 125)
        Me.pE.TabIndex = 0
        '
        'pbActive
        '
        Me.pbActive.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbActive.BackColor = System.Drawing.SystemColors.Window
        Me.pbActive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbActive.Location = New System.Drawing.Point(256, 3)
        Me.pbActive.Name = "pbActive"
        Me.pbActive.Size = New System.Drawing.Size(26, 26)
        Me.pbActive.TabIndex = 1
        Me.pbActive.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "BLOK maat:"
        '
        'txtMaat
        '
        Me.txtMaat.Location = New System.Drawing.Point(73, 9)
        Me.txtMaat.MaxLength = 3
        Me.txtMaat.Name = "txtMaat"
        Me.txtMaat.Size = New System.Drawing.Size(31, 20)
        Me.txtMaat.TabIndex = 3
        Me.txtMaat.Text = "4"
        Me.txtMaat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnHorizontaal
        '
        Me.btnHorizontaal.Location = New System.Drawing.Point(110, 6)
        Me.btnHorizontaal.Name = "btnHorizontaal"
        Me.btnHorizontaal.Size = New System.Drawing.Size(72, 23)
        Me.btnHorizontaal.TabIndex = 4
        Me.btnHorizontaal.Text = "Horizontaal"
        Me.btnHorizontaal.UseVisualStyleBackColor = True
        '
        'btnVertikaal
        '
        Me.btnVertikaal.Location = New System.Drawing.Point(188, 6)
        Me.btnVertikaal.Name = "btnVertikaal"
        Me.btnVertikaal.Size = New System.Drawing.Size(66, 23)
        Me.btnVertikaal.TabIndex = 5
        Me.btnVertikaal.Text = "Vertikaal"
        Me.btnVertikaal.UseVisualStyleBackColor = True
        '
        'btnTekstV
        '
        Me.btnTekstV.Location = New System.Drawing.Point(188, 35)
        Me.btnTekstV.Name = "btnTekstV"
        Me.btnTekstV.Size = New System.Drawing.Size(66, 23)
        Me.btnTekstV.TabIndex = 10
        Me.btnTekstV.Text = "Vertikaal"
        Me.btnTekstV.UseVisualStyleBackColor = True
        '
        'btnTekstH
        '
        Me.btnTekstH.Location = New System.Drawing.Point(110, 35)
        Me.btnTekstH.Name = "btnTekstH"
        Me.btnTekstH.Size = New System.Drawing.Size(72, 23)
        Me.btnTekstH.TabIndex = 9
        Me.btnTekstH.Text = "Horizontaal"
        Me.btnTekstH.UseVisualStyleBackColor = True
        '
        'txtLengte
        '
        Me.txtLengte.Location = New System.Drawing.Point(73, 38)
        Me.txtLengte.MaxLength = 3
        Me.txtLengte.Name = "txtLengte"
        Me.txtLengte.Size = New System.Drawing.Size(31, 20)
        Me.txtLengte.TabIndex = 8
        Me.txtLengte.Text = "4"
        Me.txtLengte.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(30, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Tekst:"
        '
        'pbtekst
        '
        Me.pbtekst.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbtekst.BackColor = System.Drawing.SystemColors.Window
        Me.pbtekst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbtekst.Location = New System.Drawing.Point(256, 32)
        Me.pbtekst.Name = "pbtekst"
        Me.pbtekst.Size = New System.Drawing.Size(26, 26)
        Me.pbtekst.TabIndex = 6
        Me.pbtekst.TabStop = False
        '
        'ElementsFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(294, 212)
        Me.Controls.Add(Me.btnTekstV)
        Me.Controls.Add(Me.btnTekstH)
        Me.Controls.Add(Me.txtLengte)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.pbtekst)
        Me.Controls.Add(Me.btnVertikaal)
        Me.Controls.Add(Me.btnHorizontaal)
        Me.Controls.Add(Me.txtMaat)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pbActive)
        Me.Controls.Add(Me.pE)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(310, 250)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(310, 250)
        Me.Name = "ElementsFrm"
        Me.Text = "Klik op element om het aktief te maken"
        Me.TopMost = True
        CType(Me.pbActive, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbtekst, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pE As System.Windows.Forms.Panel
    Friend WithEvents pbActive As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtMaat As System.Windows.Forms.TextBox
    Friend WithEvents btnHorizontaal As System.Windows.Forms.Button
    Friend WithEvents btnVertikaal As System.Windows.Forms.Button
    Friend WithEvents btnTekstV As System.Windows.Forms.Button
    Friend WithEvents btnTekstH As System.Windows.Forms.Button
    Friend WithEvents txtLengte As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pbtekst As System.Windows.Forms.PictureBox
End Class
