<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SynopBlokLocoFrm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SynopBlokLocoFrm))
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtBlokNR = New System.Windows.Forms.TextBox()
        Me.btnVoeruit = New System.Windows.Forms.Button()
        Me.txtLocoNR = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "LocoNR"
        '
        'txtBlokNR
        '
        Me.txtBlokNR.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtBlokNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBlokNR.Location = New System.Drawing.Point(62, 6)
        Me.txtBlokNR.MaxLength = 3
        Me.txtBlokNR.Name = "txtBlokNR"
        Me.txtBlokNR.ReadOnly = True
        Me.txtBlokNR.Size = New System.Drawing.Size(40, 16)
        Me.txtBlokNR.TabIndex = 0
        Me.txtBlokNR.TabStop = False
        Me.txtBlokNR.Text = "0"
        Me.txtBlokNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnVoeruit
        '
        Me.btnVoeruit.Location = New System.Drawing.Point(120, 21)
        Me.btnVoeruit.Name = "btnVoeruit"
        Me.btnVoeruit.Size = New System.Drawing.Size(63, 23)
        Me.btnVoeruit.TabIndex = 1
        Me.btnVoeruit.Text = "Voeruit"
        Me.btnVoeruit.UseVisualStyleBackColor = True
        '
        'txtLocoNR
        '
        Me.txtLocoNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLocoNR.Location = New System.Drawing.Point(62, 37)
        Me.txtLocoNR.MaxLength = 3
        Me.txtLocoNR.Name = "txtLocoNR"
        Me.txtLocoNR.Size = New System.Drawing.Size(40, 23)
        Me.txtLocoNR.TabIndex = 0
        Me.txtLocoNR.Text = "0"
        Me.txtLocoNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "BlokNR"
        '
        'SynopBlokLocoFrm
        '
        Me.AcceptButton = Me.btnVoeruit
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(193, 68)
        Me.Controls.Add(Me.txtLocoNR)
        Me.Controls.Add(Me.btnVoeruit)
        Me.Controls.Add(Me.txtBlokNR)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(209, 106)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(209, 106)
        Me.Name = "SynopBlokLocoFrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Aanpassen Blok-Loco"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtBlokNR As System.Windows.Forms.TextBox
    Friend WithEvents btnVoeruit As System.Windows.Forms.Button
    Friend WithEvents txtLocoNR As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
