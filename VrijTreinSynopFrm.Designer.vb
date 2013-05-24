<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VrijTreinSynopFrm
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtVanBlokNR = New System.Windows.Forms.TextBox()
        Me.btnVoeruit = New System.Windows.Forms.Button()
        Me.txtNaarBlokNR = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Van blokNR"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Terminus BlokNR"
        '
        'txtVanBlokNR
        '
        Me.txtVanBlokNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVanBlokNR.Location = New System.Drawing.Point(108, 12)
        Me.txtVanBlokNR.MaxLength = 3
        Me.txtVanBlokNR.Name = "txtVanBlokNR"
        Me.txtVanBlokNR.Size = New System.Drawing.Size(39, 23)
        Me.txtVanBlokNR.TabIndex = 0
        Me.txtVanBlokNR.Text = "0"
        Me.txtVanBlokNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnVoeruit
        '
        Me.btnVoeruit.Location = New System.Drawing.Point(19, 71)
        Me.btnVoeruit.Name = "btnVoeruit"
        Me.btnVoeruit.Size = New System.Drawing.Size(128, 23)
        Me.btnVoeruit.TabIndex = 2
        Me.btnVoeruit.Text = "Voer uit"
        Me.btnVoeruit.UseVisualStyleBackColor = True
        '
        'txtNaarBlokNR
        '
        Me.txtNaarBlokNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNaarBlokNR.Location = New System.Drawing.Point(108, 42)
        Me.txtNaarBlokNR.MaxLength = 3
        Me.txtNaarBlokNR.Name = "txtNaarBlokNR"
        Me.txtNaarBlokNR.Size = New System.Drawing.Size(39, 23)
        Me.txtNaarBlokNR.TabIndex = 1
        Me.txtNaarBlokNR.Text = "0"
        Me.txtNaarBlokNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'VrijTreinSynopFrm
        '
        Me.AcceptButton = Me.btnVoeruit
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(190, 98)
        Me.Controls.Add(Me.txtNaarBlokNR)
        Me.Controls.Add(Me.btnVoeruit)
        Me.Controls.Add(Me.txtVanBlokNR)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(206, 136)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(206, 136)
        Me.Name = "VrijTreinSynopFrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Indienstname Vrije trein"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtVanBlokNR As System.Windows.Forms.TextBox
    Friend WithEvents btnVoeruit As System.Windows.Forms.Button
    Friend WithEvents txtNaarBlokNR As System.Windows.Forms.TextBox
End Class
