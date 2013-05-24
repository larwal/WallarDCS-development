<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ViewBlokLocoFrm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ViewBlokLocoFrm))
        Me.lstBlokLoco = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'lstBlokLoco
        '
        Me.lstBlokLoco.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstBlokLoco.FormattingEnabled = True
        Me.lstBlokLoco.Location = New System.Drawing.Point(0, 0)
        Me.lstBlokLoco.Name = "lstBlokLoco"
        Me.lstBlokLoco.Size = New System.Drawing.Size(131, 546)
        Me.lstBlokLoco.TabIndex = 0
        '
        'ViewBlokLocoFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(131, 546)
        Me.Controls.Add(Me.lstBlokLoco)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ViewBlokLocoFrm"
        Me.Text = "Bezetting"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstBlokLoco As System.Windows.Forms.ListBox
End Class
