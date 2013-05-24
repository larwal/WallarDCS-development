<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BlokLocoFrm
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lstBlokLocoData = New System.Windows.Forms.ListBox()
        Me.txtLoco = New System.Windows.Forms.TextBox()
        Me.txtBlok = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnPasAan = New System.Windows.Forms.Button()
        Me.btnVerlaat = New System.Windows.Forms.Button()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstBlokLocoData)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtLoco)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtBlok)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnPasAan)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnVerlaat)
        Me.SplitContainer1.Size = New System.Drawing.Size(180, 666)
        Me.SplitContainer1.SplitterDistance = 581
        Me.SplitContainer1.TabIndex = 0
        '
        'lstBlokLocoData
        '
        Me.lstBlokLocoData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstBlokLocoData.FormattingEnabled = True
        Me.lstBlokLocoData.Location = New System.Drawing.Point(0, 0)
        Me.lstBlokLocoData.Name = "lstBlokLocoData"
        Me.lstBlokLocoData.Size = New System.Drawing.Size(180, 581)
        Me.lstBlokLocoData.TabIndex = 0
        '
        'txtLoco
        '
        Me.txtLoco.Location = New System.Drawing.Point(42, 44)
        Me.txtLoco.MaxLength = 3
        Me.txtLoco.Name = "txtLoco"
        Me.txtLoco.Size = New System.Drawing.Size(44, 20)
        Me.txtLoco.TabIndex = 5
        '
        'txtBlok
        '
        Me.txtBlok.Location = New System.Drawing.Point(42, 17)
        Me.txtBlok.Name = "txtBlok"
        Me.txtBlok.ReadOnly = True
        Me.txtBlok.Size = New System.Drawing.Size(44, 20)
        Me.txtBlok.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Loco"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Blok"
        '
        'btnPasAan
        '
        Me.btnPasAan.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPasAan.Location = New System.Drawing.Point(93, 15)
        Me.btnPasAan.Name = "btnPasAan"
        Me.btnPasAan.Size = New System.Drawing.Size(75, 23)
        Me.btnPasAan.TabIndex = 1
        Me.btnPasAan.Text = "Pas aan"
        Me.btnPasAan.UseVisualStyleBackColor = True
        '
        'btnVerlaat
        '
        Me.btnVerlaat.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVerlaat.Location = New System.Drawing.Point(93, 42)
        Me.btnVerlaat.Name = "btnVerlaat"
        Me.btnVerlaat.Size = New System.Drawing.Size(75, 23)
        Me.btnVerlaat.TabIndex = 0
        Me.btnVerlaat.Text = "Verlaat"
        Me.btnVerlaat.UseVisualStyleBackColor = True
        '
        'BlokLocoFrm
        '
        Me.AcceptButton = Me.btnVerlaat
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(180, 666)
        Me.Controls.Add(Me.SplitContainer1)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(196, 704)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(196, 704)
        Me.Name = "BlokLocoFrm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blok Loco"
        Me.TopMost = True
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents lstBlokLocoData As System.Windows.Forms.ListBox
    Friend WithEvents txtLoco As System.Windows.Forms.TextBox
    Friend WithEvents txtBlok As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnPasAan As System.Windows.Forms.Button
    Friend WithEvents btnVerlaat As System.Windows.Forms.Button
End Class
