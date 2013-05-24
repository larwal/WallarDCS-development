<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WijzigGridFrm
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
        Me.lstBox = New System.Windows.Forms.ListBox()
        Me.lstGridStep = New System.Windows.Forms.ListBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lstBox
        '
        Me.lstBox.Dock = System.Windows.Forms.DockStyle.Left
        Me.lstBox.FormattingEnabled = True
        Me.lstBox.Location = New System.Drawing.Point(0, 0)
        Me.lstBox.Name = "lstBox"
        Me.lstBox.ScrollAlwaysVisible = True
        Me.lstBox.Size = New System.Drawing.Size(634, 510)
        Me.lstBox.TabIndex = 0
        '
        'lstGridStep
        '
        Me.lstGridStep.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstGridStep.FormattingEnabled = True
        Me.lstGridStep.Items.AddRange(New Object() {"12 x 12", "16 x 16", "20 x 20", "24 x 24", "28 x 28", "32 x 32"})
        Me.lstGridStep.Location = New System.Drawing.Point(663, 12)
        Me.lstGridStep.Name = "lstGridStep"
        Me.lstGridStep.Size = New System.Drawing.Size(50, 82)
        Me.lstGridStep.TabIndex = 1
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(653, 424)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(60, 23)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(653, 463)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(60, 35)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'WijzigGridFrm
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(723, 510)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lstGridStep)
        Me.Controls.Add(Me.lstBox)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "WijzigGridFrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "WijzigGridFrm"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstBox As System.Windows.Forms.ListBox
    Friend WithEvents lstGridStep As System.Windows.Forms.ListBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
End Class
