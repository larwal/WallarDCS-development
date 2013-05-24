<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewSynopPropertiesFrm
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rb32 = New System.Windows.Forms.RadioButton()
        Me.rb28 = New System.Windows.Forms.RadioButton()
        Me.rb24 = New System.Windows.Forms.RadioButton()
        Me.rb20 = New System.Windows.Forms.RadioButton()
        Me.rb16 = New System.Windows.Forms.RadioButton()
        Me.rb12 = New System.Windows.Forms.RadioButton()
        Me.LblLabelText = New System.Windows.Forms.Label()
        Me.lblTittle = New System.Windows.Forms.Label()
        Me.txtLabel = New System.Windows.Forms.TextBox()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.lstAppearance = New System.Windows.Forms.ListBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rb32)
        Me.GroupBox1.Controls.Add(Me.rb28)
        Me.GroupBox1.Controls.Add(Me.rb24)
        Me.GroupBox1.Controls.Add(Me.rb20)
        Me.GroupBox1.Controls.Add(Me.rb16)
        Me.GroupBox1.Controls.Add(Me.rb12)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(291, 67)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GridStep"
        '
        'rb32
        '
        Me.rb32.AutoSize = True
        Me.rb32.Location = New System.Drawing.Point(220, 43)
        Me.rb32.Name = "rb32"
        Me.rb32.Size = New System.Drawing.Size(60, 17)
        Me.rb32.TabIndex = 5
        Me.rb32.Text = "32 x 32"
        Me.rb32.UseVisualStyleBackColor = True
        '
        'rb28
        '
        Me.rb28.AutoSize = True
        Me.rb28.Location = New System.Drawing.Point(220, 20)
        Me.rb28.Name = "rb28"
        Me.rb28.Size = New System.Drawing.Size(60, 17)
        Me.rb28.TabIndex = 4
        Me.rb28.Text = "28 x 28"
        Me.rb28.UseVisualStyleBackColor = True
        '
        'rb24
        '
        Me.rb24.AutoSize = True
        Me.rb24.Location = New System.Drawing.Point(118, 43)
        Me.rb24.Name = "rb24"
        Me.rb24.Size = New System.Drawing.Size(60, 17)
        Me.rb24.TabIndex = 3
        Me.rb24.Text = "24 x 24"
        Me.rb24.UseVisualStyleBackColor = True
        '
        'rb20
        '
        Me.rb20.AutoSize = True
        Me.rb20.Checked = True
        Me.rb20.Location = New System.Drawing.Point(118, 19)
        Me.rb20.Name = "rb20"
        Me.rb20.Size = New System.Drawing.Size(60, 17)
        Me.rb20.TabIndex = 2
        Me.rb20.TabStop = True
        Me.rb20.Text = "20 x 20"
        Me.rb20.UseVisualStyleBackColor = True
        '
        'rb16
        '
        Me.rb16.AutoSize = True
        Me.rb16.Location = New System.Drawing.Point(17, 43)
        Me.rb16.Name = "rb16"
        Me.rb16.Size = New System.Drawing.Size(60, 17)
        Me.rb16.TabIndex = 1
        Me.rb16.Text = "16 x 16"
        Me.rb16.UseVisualStyleBackColor = True
        '
        'rb12
        '
        Me.rb12.AutoSize = True
        Me.rb12.Location = New System.Drawing.Point(17, 20)
        Me.rb12.Name = "rb12"
        Me.rb12.Size = New System.Drawing.Size(60, 17)
        Me.rb12.TabIndex = 0
        Me.rb12.Text = "12 x 12"
        Me.rb12.UseVisualStyleBackColor = True
        '
        'LblLabelText
        '
        Me.LblLabelText.AutoSize = True
        Me.LblLabelText.Location = New System.Drawing.Point(13, 102)
        Me.LblLabelText.Name = "LblLabelText"
        Me.LblLabelText.Size = New System.Drawing.Size(57, 13)
        Me.LblLabelText.TabIndex = 3
        Me.LblLabelText.Text = "Label Text"
        '
        'lblTittle
        '
        Me.lblTittle.AutoSize = True
        Me.lblTittle.Location = New System.Drawing.Point(43, 76)
        Me.lblTittle.Name = "lblTittle"
        Me.lblTittle.Size = New System.Drawing.Size(30, 13)
        Me.lblTittle.TabIndex = 4
        Me.lblTittle.Text = "Tittle"
        '
        'txtLabel
        '
        Me.txtLabel.Location = New System.Drawing.Point(78, 99)
        Me.txtLabel.Name = "txtLabel"
        Me.txtLabel.Size = New System.Drawing.Size(202, 20)
        Me.txtLabel.TabIndex = 2
        Me.txtLabel.Text = "Algemene info van synoptiek"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(78, 73)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(202, 20)
        Me.txtTitle.TabIndex = 1
        Me.txtTitle.Text = "Titel scherm van synoptiek"
        '
        'lstAppearance
        '
        Me.lstAppearance.FormattingEnabled = True
        Me.lstAppearance.Items.AddRange(New Object() {"FitScreen", "Standaard", "Maximized"})
        Me.lstAppearance.Location = New System.Drawing.Point(17, 125)
        Me.lstAppearance.Name = "lstAppearance"
        Me.lstAppearance.Size = New System.Drawing.Size(83, 43)
        Me.lstAppearance.TabIndex = 3
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(118, 147)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.Location = New System.Drawing.Point(205, 147)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 6
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'NewSynopPropertiesFrm
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(291, 184)
        Me.Controls.Add(Me.lstAppearance)
        Me.Controls.Add(Me.txtTitle)
        Me.Controls.Add(Me.txtLabel)
        Me.Controls.Add(Me.lblTittle)
        Me.Controls.Add(Me.LblLabelText)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.GroupBox1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewSynopPropertiesFrm"
        Me.Text = "New Synoptic Properties"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rb32 As System.Windows.Forms.RadioButton
    Friend WithEvents rb28 As System.Windows.Forms.RadioButton
    Friend WithEvents rb24 As System.Windows.Forms.RadioButton
    Friend WithEvents rb20 As System.Windows.Forms.RadioButton
    Friend WithEvents rb16 As System.Windows.Forms.RadioButton
    Friend WithEvents rb12 As System.Windows.Forms.RadioButton
    Friend WithEvents LblLabelText As System.Windows.Forms.Label
    Friend WithEvents lblTittle As System.Windows.Forms.Label
    Friend WithEvents txtLabel As System.Windows.Forms.TextBox
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents lstAppearance As System.Windows.Forms.ListBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
End Class
