<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class s88ContactsFrm
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
        Me.txts88contacts = New System.Windows.Forms.TextBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.txtColons = New System.Windows.Forms.TextBox()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txts88contacts
        '
        Me.txts88contacts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txts88contacts.Font = New System.Drawing.Font("Courier New", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txts88contacts.Location = New System.Drawing.Point(0, 0)
        Me.txts88contacts.Multiline = True
        Me.txts88contacts.Name = "txts88contacts"
        Me.txts88contacts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txts88contacts.Size = New System.Drawing.Size(1003, 679)
        Me.txts88contacts.TabIndex = 0
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
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtColons)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.txts88contacts)
        Me.SplitContainer1.Size = New System.Drawing.Size(1003, 711)
        Me.SplitContainer1.SplitterDistance = 28
        Me.SplitContainer1.TabIndex = 1
        '
        'txtColons
        '
        Me.txtColons.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.txtColons.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtColons.Font = New System.Drawing.Font("Courier New", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtColons.Location = New System.Drawing.Point(0, 0)
        Me.txtColons.Name = "txtColons"
        Me.txtColons.ReadOnly = True
        Me.txtColons.Size = New System.Drawing.Size(1003, 29)
        Me.txtColons.TabIndex = 0
        Me.txtColons.TabStop = False
        '
        's88ContactsFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1003, 711)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "s88ContactsFrm"
        Me.Text = "s88ContactsFrm"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txts88contacts As System.Windows.Forms.TextBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents txtColons As System.Windows.Forms.TextBox
End Class
