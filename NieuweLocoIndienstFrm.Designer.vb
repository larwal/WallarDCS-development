<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NieuweLocoIndienstFrm
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
        Me.txtLocoNR = New System.Windows.Forms.TextBox()
        Me.txtBlokNR = New System.Windows.Forms.TextBox()
        Me.btnIndienstname = New System.Windows.Forms.Button()
        Me.radPaar = New System.Windows.Forms.RadioButton()
        Me.radOnpaar = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(65, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Nieuw Loconummer:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(85, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Op bloknummer:"
        '
        'txtLocoNR
        '
        Me.txtLocoNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLocoNR.Location = New System.Drawing.Point(175, 15)
        Me.txtLocoNR.MaxLength = 2
        Me.txtLocoNR.Name = "txtLocoNR"
        Me.txtLocoNR.Size = New System.Drawing.Size(34, 22)
        Me.txtLocoNR.TabIndex = 0
        Me.txtLocoNR.Text = "0"
        Me.txtLocoNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtBlokNR
        '
        Me.txtBlokNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBlokNR.Location = New System.Drawing.Point(175, 49)
        Me.txtBlokNR.MaxLength = 3
        Me.txtBlokNR.Name = "txtBlokNR"
        Me.txtBlokNR.Size = New System.Drawing.Size(34, 22)
        Me.txtBlokNR.TabIndex = 1
        Me.txtBlokNR.Text = "0"
        Me.txtBlokNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnIndienstname
        '
        Me.btnIndienstname.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnIndienstname.Location = New System.Drawing.Point(15, 128)
        Me.btnIndienstname.Name = "btnIndienstname"
        Me.btnIndienstname.Size = New System.Drawing.Size(281, 60)
        Me.btnIndienstname.TabIndex = 2
        Me.btnIndienstname.Text = "Plaats eerst  loco op blok vooralleer op knop te drukken van Indienstname"
        Me.btnIndienstname.UseVisualStyleBackColor = True
        '
        'radPaar
        '
        Me.radPaar.AutoSize = True
        Me.radPaar.Location = New System.Drawing.Point(57, 87)
        Me.radPaar.Name = "radPaar"
        Me.radPaar.Size = New System.Drawing.Size(47, 17)
        Me.radPaar.TabIndex = 3
        Me.radPaar.Text = "Paar"
        Me.radPaar.UseVisualStyleBackColor = True
        '
        'radOnpaar
        '
        Me.radOnpaar.AutoSize = True
        Me.radOnpaar.Checked = True
        Me.radOnpaar.Location = New System.Drawing.Point(110, 87)
        Me.radOnpaar.Name = "radOnpaar"
        Me.radOnpaar.Size = New System.Drawing.Size(111, 17)
        Me.radOnpaar.TabIndex = 4
        Me.radOnpaar.TabStop = True
        Me.radOnpaar.Text = "Onpaar bitnummer"
        Me.radOnpaar.UseVisualStyleBackColor = True
        '
        'NieuweLocoIndienstFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(308, 200)
        Me.Controls.Add(Me.radOnpaar)
        Me.Controls.Add(Me.radPaar)
        Me.Controls.Add(Me.btnIndienstname)
        Me.Controls.Add(Me.txtBlokNR)
        Me.Controls.Add(Me.txtLocoNR)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NieuweLocoIndienstFrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Nieuwe Loco Indienstnemen"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtLocoNR As System.Windows.Forms.TextBox
    Friend WithEvents txtBlokNR As System.Windows.Forms.TextBox
    Friend WithEvents btnIndienstname As System.Windows.Forms.Button
    Friend WithEvents radPaar As System.Windows.Forms.RadioButton
    Friend WithEvents radOnpaar As System.Windows.Forms.RadioButton
End Class
