<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EditorHelpfrm
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
        Me.txtHelpEditor = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtHelpEditor
        '
        Me.txtHelpEditor.BackColor = System.Drawing.Color.LemonChiffon
        Me.txtHelpEditor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtHelpEditor.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHelpEditor.Location = New System.Drawing.Point(0, 0)
        Me.txtHelpEditor.Multiline = True
        Me.txtHelpEditor.Name = "txtHelpEditor"
        Me.txtHelpEditor.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtHelpEditor.Size = New System.Drawing.Size(1191, 383)
        Me.txtHelpEditor.TabIndex = 0
        Me.txtHelpEditor.TabStop = False
        '
        'EditorHelpfrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1191, 383)
        Me.Controls.Add(Me.txtHelpEditor)
        Me.Name = "EditorHelpfrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.Text = "Help voor invullen van de gegevens"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtHelpEditor As System.Windows.Forms.TextBox
End Class
