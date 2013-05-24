<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class QueryBlockLocofrm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(QueryBlockLocofrm))
        Me.lstLocoBlok = New System.Windows.Forms.ListBox()
        Me.btnShowOnSynop = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lstLocoBlok
        '
        Me.lstLocoBlok.FormattingEnabled = True
        Me.lstLocoBlok.Location = New System.Drawing.Point(11, 28)
        Me.lstLocoBlok.Name = "lstLocoBlok"
        Me.lstLocoBlok.Size = New System.Drawing.Size(120, 693)
        Me.lstLocoBlok.Sorted = True
        Me.lstLocoBlok.TabIndex = 0
        '
        'btnShowOnSynop
        '
        Me.btnShowOnSynop.Location = New System.Drawing.Point(137, 15)
        Me.btnShowOnSynop.Name = "btnShowOnSynop"
        Me.btnShowOnSynop.Size = New System.Drawing.Size(76, 55)
        Me.btnShowOnSynop.TabIndex = 1
        Me.btnShowOnSynop.Text = "Toon op Synoptiek"
        Me.btnShowOnSynop.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Loco  ===>  blok"
        '
        'QueryBlockLocofrm
        '
        Me.AcceptButton = Me.btnShowOnSynop
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(218, 730)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnShowOnSynop)
        Me.Controls.Add(Me.lstLocoBlok)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "QueryBlockLocofrm"
        Me.Text = "Waar staat loco ?"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstLocoBlok As System.Windows.Forms.ListBox
    Friend WithEvents btnShowOnSynop As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
