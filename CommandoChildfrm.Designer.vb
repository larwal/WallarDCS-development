<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CommandoChildfrm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CommandoChildfrm))
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.UpDownSpeed = New System.Windows.Forms.NumericUpDown()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rBtnAchteruit = New System.Windows.Forms.RadioButton()
        Me.rBtnVooruit = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtLocoAddress = New System.Windows.Forms.TextBox()
        Me.chkF0 = New System.Windows.Forms.CheckBox()
        Me.chkF4 = New System.Windows.Forms.CheckBox()
        Me.chkF3 = New System.Windows.Forms.CheckBox()
        Me.chkF2 = New System.Windows.Forms.CheckBox()
        Me.chkF1 = New System.Windows.Forms.CheckBox()
        Me.btnSTOP = New System.Windows.Forms.Button()
        Me.btnGO = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.txtAddressTournout = New System.Windows.Forms.TextBox()
        Me.btnBendingTurnout = New System.Windows.Forms.Button()
        Me.btnStraightTurnout = New System.Windows.Forms.Button()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.radAchteruit = New System.Windows.Forms.RadioButton()
        Me.radVooruit = New System.Windows.Forms.RadioButton()
        Me.btnVrijGO = New System.Windows.Forms.Button()
        Me.txtNaar = New System.Windows.Forms.TextBox()
        Me.txtIS = New System.Windows.Forms.TextBox()
        Me.txtVan = New System.Windows.Forms.TextBox()
        Me.lblNaar = New System.Windows.Forms.Label()
        Me.lblIs = New System.Windows.Forms.Label()
        Me.lblVan = New System.Windows.Forms.Label()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lstMacro = New System.Windows.Forms.ListBox()
        Me.btnVoeruitMacro = New System.Windows.Forms.Button()
        Me.txtMacroNR = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TabControl.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.UpDownSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl
        '
        Me.TabControl.Controls.Add(Me.TabPage1)
        Me.TabControl.Controls.Add(Me.TabPage2)
        Me.TabControl.Controls.Add(Me.TabPage3)
        Me.TabControl.Controls.Add(Me.TabPage4)
        Me.TabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl.Location = New System.Drawing.Point(0, 0)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(384, 182)
        Me.TabControl.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.UpDownSpeed)
        Me.TabPage1.Controls.Add(Me.Panel1)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.txtLocoAddress)
        Me.TabPage1.Controls.Add(Me.chkF0)
        Me.TabPage1.Controls.Add(Me.chkF4)
        Me.TabPage1.Controls.Add(Me.chkF3)
        Me.TabPage1.Controls.Add(Me.chkF2)
        Me.TabPage1.Controls.Add(Me.chkF1)
        Me.TabPage1.Controls.Add(Me.btnSTOP)
        Me.TabPage1.Controls.Add(Me.btnGO)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(376, 156)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Loco"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'UpDownSpeed
        '
        Me.UpDownSpeed.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.UpDownSpeed.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UpDownSpeed.Location = New System.Drawing.Point(56, 39)
        Me.UpDownSpeed.Maximum = New Decimal(New Integer() {14, 0, 0, 0})
        Me.UpDownSpeed.Name = "UpDownSpeed"
        Me.UpDownSpeed.Size = New System.Drawing.Size(45, 24)
        Me.UpDownSpeed.TabIndex = 3
        Me.UpDownSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.UpDownSpeed.Value = New Decimal(New Integer() {7, 0, 0, 0})
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.rBtnAchteruit)
        Me.Panel1.Controls.Add(Me.rBtnVooruit)
        Me.Panel1.Location = New System.Drawing.Point(17, 69)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(84, 53)
        Me.Panel1.TabIndex = 14
        '
        'rBtnAchteruit
        '
        Me.rBtnAchteruit.AutoSize = True
        Me.rBtnAchteruit.Location = New System.Drawing.Point(3, 23)
        Me.rBtnAchteruit.Name = "rBtnAchteruit"
        Me.rBtnAchteruit.Size = New System.Drawing.Size(67, 17)
        Me.rBtnAchteruit.TabIndex = 1
        Me.rBtnAchteruit.Text = "Achteruit"
        Me.rBtnAchteruit.UseVisualStyleBackColor = True
        '
        'rBtnVooruit
        '
        Me.rBtnVooruit.AutoSize = True
        Me.rBtnVooruit.Checked = True
        Me.rBtnVooruit.Location = New System.Drawing.Point(3, 3)
        Me.rBtnVooruit.Name = "rBtnVooruit"
        Me.rBtnVooruit.Size = New System.Drawing.Size(58, 17)
        Me.rBtnVooruit.TabIndex = 0
        Me.rBtnVooruit.TabStop = True
        Me.rBtnVooruit.Text = "Vooruit"
        Me.rBtnVooruit.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Snelheid"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Adres"
        '
        'txtLocoAddress
        '
        Me.txtLocoAddress.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txtLocoAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLocoAddress.Location = New System.Drawing.Point(56, 8)
        Me.txtLocoAddress.MaxLength = 2
        Me.txtLocoAddress.Name = "txtLocoAddress"
        Me.txtLocoAddress.Size = New System.Drawing.Size(45, 26)
        Me.txtLocoAddress.TabIndex = 0
        Me.txtLocoAddress.Text = "0"
        Me.txtLocoAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkF0
        '
        Me.chkF0.AutoSize = True
        Me.chkF0.Checked = True
        Me.chkF0.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkF0.Location = New System.Drawing.Point(21, 131)
        Me.chkF0.Name = "chkF0"
        Me.chkF0.Size = New System.Drawing.Size(158, 17)
        Me.chkF0.TabIndex = 4
        Me.chkF0.Text = "Frontlight (enkel bij stilstand)"
        Me.chkF0.UseVisualStyleBackColor = True
        '
        'chkF4
        '
        Me.chkF4.AutoSize = True
        Me.chkF4.Location = New System.Drawing.Point(330, 128)
        Me.chkF4.Name = "chkF4"
        Me.chkF4.Size = New System.Drawing.Size(38, 17)
        Me.chkF4.TabIndex = 8
        Me.chkF4.Text = "F4"
        Me.chkF4.UseVisualStyleBackColor = True
        '
        'chkF3
        '
        Me.chkF3.AutoSize = True
        Me.chkF3.Location = New System.Drawing.Point(285, 128)
        Me.chkF3.Name = "chkF3"
        Me.chkF3.Size = New System.Drawing.Size(38, 17)
        Me.chkF3.TabIndex = 7
        Me.chkF3.Text = "F3"
        Me.chkF3.UseVisualStyleBackColor = True
        '
        'chkF2
        '
        Me.chkF2.AutoSize = True
        Me.chkF2.Location = New System.Drawing.Point(248, 128)
        Me.chkF2.Name = "chkF2"
        Me.chkF2.Size = New System.Drawing.Size(38, 17)
        Me.chkF2.TabIndex = 6
        Me.chkF2.Text = "F2"
        Me.chkF2.UseVisualStyleBackColor = True
        '
        'chkF1
        '
        Me.chkF1.AutoSize = True
        Me.chkF1.Location = New System.Drawing.Point(204, 128)
        Me.chkF1.Name = "chkF1"
        Me.chkF1.Size = New System.Drawing.Size(38, 17)
        Me.chkF1.TabIndex = 5
        Me.chkF1.Text = "F1"
        Me.chkF1.UseVisualStyleBackColor = True
        '
        'btnSTOP
        '
        Me.btnSTOP.BackColor = System.Drawing.Color.Red
        Me.btnSTOP.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnSTOP.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSTOP.Location = New System.Drawing.Point(177, 8)
        Me.btnSTOP.Name = "btnSTOP"
        Me.btnSTOP.Size = New System.Drawing.Size(193, 114)
        Me.btnSTOP.TabIndex = 2
        Me.btnSTOP.Text = "STOP"
        Me.btnSTOP.UseVisualStyleBackColor = False
        '
        'btnGO
        '
        Me.btnGO.BackColor = System.Drawing.Color.LightGreen
        Me.btnGO.Location = New System.Drawing.Point(115, 8)
        Me.btnGO.Name = "btnGO"
        Me.btnGO.Size = New System.Drawing.Size(56, 114)
        Me.btnGO.TabIndex = 1
        Me.btnGO.Text = "Start trein"
        Me.btnGO.UseVisualStyleBackColor = False
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.txtAddressTournout)
        Me.TabPage2.Controls.Add(Me.btnBendingTurnout)
        Me.TabPage2.Controls.Add(Me.btnStraightTurnout)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(376, 156)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Wissel/sein"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'txtAddressTournout
        '
        Me.txtAddressTournout.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txtAddressTournout.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddressTournout.Location = New System.Drawing.Point(73, 61)
        Me.txtAddressTournout.MaxLength = 3
        Me.txtAddressTournout.Name = "txtAddressTournout"
        Me.txtAddressTournout.Size = New System.Drawing.Size(51, 26)
        Me.txtAddressTournout.TabIndex = 0
        Me.txtAddressTournout.Text = "0"
        Me.txtAddressTournout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnBendingTurnout
        '
        Me.btnBendingTurnout.ForeColor = System.Drawing.Color.Green
        Me.btnBendingTurnout.Location = New System.Drawing.Point(162, 76)
        Me.btnBendingTurnout.Name = "btnBendingTurnout"
        Me.btnBendingTurnout.Size = New System.Drawing.Size(141, 23)
        Me.btnBendingTurnout.TabIndex = 2
        Me.btnBendingTurnout.Text = "Afbuigen - Groen"
        Me.btnBendingTurnout.UseVisualStyleBackColor = True
        '
        'btnStraightTurnout
        '
        Me.btnStraightTurnout.ForeColor = System.Drawing.Color.Red
        Me.btnStraightTurnout.Location = New System.Drawing.Point(162, 47)
        Me.btnStraightTurnout.Name = "btnStraightTurnout"
        Me.btnStraightTurnout.Size = New System.Drawing.Size(141, 23)
        Me.btnStraightTurnout.TabIndex = 1
        Me.btnStraightTurnout.Text = "Rechtdoor - Rood"
        Me.btnStraightTurnout.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.radAchteruit)
        Me.TabPage3.Controls.Add(Me.radVooruit)
        Me.TabPage3.Controls.Add(Me.btnVrijGO)
        Me.TabPage3.Controls.Add(Me.txtNaar)
        Me.TabPage3.Controls.Add(Me.txtIS)
        Me.TabPage3.Controls.Add(Me.txtVan)
        Me.TabPage3.Controls.Add(Me.lblNaar)
        Me.TabPage3.Controls.Add(Me.lblIs)
        Me.TabPage3.Controls.Add(Me.lblVan)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(376, 156)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Vrije trein"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'radAchteruit
        '
        Me.radAchteruit.AutoSize = True
        Me.radAchteruit.Location = New System.Drawing.Point(211, 121)
        Me.radAchteruit.Name = "radAchteruit"
        Me.radAchteruit.Size = New System.Drawing.Size(67, 17)
        Me.radAchteruit.TabIndex = 5
        Me.radAchteruit.Text = "Achteruit"
        Me.radAchteruit.UseVisualStyleBackColor = True
        '
        'radVooruit
        '
        Me.radVooruit.AutoSize = True
        Me.radVooruit.Checked = True
        Me.radVooruit.Location = New System.Drawing.Point(99, 121)
        Me.radVooruit.Name = "radVooruit"
        Me.radVooruit.Size = New System.Drawing.Size(58, 17)
        Me.radVooruit.TabIndex = 3
        Me.radVooruit.TabStop = True
        Me.radVooruit.Text = "Vooruit"
        Me.radVooruit.UseVisualStyleBackColor = True
        '
        'btnVrijGO
        '
        Me.btnVrijGO.BackColor = System.Drawing.Color.LightGreen
        Me.btnVrijGO.Location = New System.Drawing.Point(211, 25)
        Me.btnVrijGO.Name = "btnVrijGO"
        Me.btnVrijGO.Size = New System.Drawing.Size(136, 81)
        Me.btnVrijGO.TabIndex = 4
        Me.btnVrijGO.Text = "GO"
        Me.btnVrijGO.UseVisualStyleBackColor = False
        '
        'txtNaar
        '
        Me.txtNaar.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNaar.Location = New System.Drawing.Point(99, 85)
        Me.txtNaar.MaxLength = 3
        Me.txtNaar.Name = "txtNaar"
        Me.txtNaar.Size = New System.Drawing.Size(33, 23)
        Me.txtNaar.TabIndex = 2
        Me.txtNaar.Text = "120"
        Me.txtNaar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtIS
        '
        Me.txtIS.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIS.Location = New System.Drawing.Point(99, 57)
        Me.txtIS.MaxLength = 3
        Me.txtIS.Name = "txtIS"
        Me.txtIS.Size = New System.Drawing.Size(33, 23)
        Me.txtIS.TabIndex = 1
        Me.txtIS.Text = "0"
        Me.txtIS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtVan
        '
        Me.txtVan.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVan.Location = New System.Drawing.Point(99, 27)
        Me.txtVan.MaxLength = 3
        Me.txtVan.Name = "txtVan"
        Me.txtVan.Size = New System.Drawing.Size(33, 23)
        Me.txtVan.TabIndex = 0
        Me.txtVan.Text = "0"
        Me.txtVan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblNaar
        '
        Me.lblNaar.AutoSize = True
        Me.lblNaar.Location = New System.Drawing.Point(63, 88)
        Me.lblNaar.Name = "lblNaar"
        Me.lblNaar.Size = New System.Drawing.Size(30, 13)
        Me.lblNaar.TabIndex = 2
        Me.lblNaar.Text = "Naar"
        '
        'lblIs
        '
        Me.lblIs.AutoSize = True
        Me.lblIs.Location = New System.Drawing.Point(78, 60)
        Me.lblIs.Name = "lblIs"
        Me.lblIs.Size = New System.Drawing.Size(15, 13)
        Me.lblIs.TabIndex = 1
        Me.lblIs.Text = "Is"
        '
        'lblVan
        '
        Me.lblVan.AutoSize = True
        Me.lblVan.Location = New System.Drawing.Point(67, 30)
        Me.lblVan.Name = "lblVan"
        Me.lblVan.Size = New System.Drawing.Size(26, 13)
        Me.lblVan.TabIndex = 0
        Me.lblVan.Text = "Van"
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.SplitContainer1)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(376, 156)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Macro's"
        Me.TabPage4.UseVisualStyleBackColor = True
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
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstMacro)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnVoeruitMacro)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtMacroNR)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label3)
        Me.SplitContainer1.Size = New System.Drawing.Size(376, 156)
        Me.SplitContainer1.SplitterDistance = 121
        Me.SplitContainer1.TabIndex = 0
        '
        'lstMacro
        '
        Me.lstMacro.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstMacro.FormattingEnabled = True
        Me.lstMacro.Location = New System.Drawing.Point(0, 0)
        Me.lstMacro.Name = "lstMacro"
        Me.lstMacro.ScrollAlwaysVisible = True
        Me.lstMacro.Size = New System.Drawing.Size(376, 121)
        Me.lstMacro.TabIndex = 0
        '
        'btnVoeruitMacro
        '
        Me.btnVoeruitMacro.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVoeruitMacro.Location = New System.Drawing.Point(239, 7)
        Me.btnVoeruitMacro.Name = "btnVoeruitMacro"
        Me.btnVoeruitMacro.Size = New System.Drawing.Size(129, 21)
        Me.btnVoeruitMacro.TabIndex = 2
        Me.btnVoeruitMacro.Text = "Voeruit"
        Me.btnVoeruitMacro.UseVisualStyleBackColor = True
        '
        'txtMacroNR
        '
        Me.txtMacroNR.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txtMacroNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMacroNR.Location = New System.Drawing.Point(195, 5)
        Me.txtMacroNR.Name = "txtMacroNR"
        Me.txtMacroNR.Size = New System.Drawing.Size(38, 21)
        Me.txtMacroNR.TabIndex = 1
        Me.txtMacroNR.Text = "1"
        Me.txtMacroNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(181, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Geef een bestaand macronummer in:"
        '
        'CommandoChildfrm
        '
        Me.AcceptButton = Me.btnGO
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnSTOP
        Me.ClientSize = New System.Drawing.Size(384, 182)
        Me.Controls.Add(Me.TabControl)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(400, 220)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(400, 220)
        Me.Name = "CommandoChildfrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Actie Loco, Wissel en Vrije trein"
        Me.TopMost = True
        Me.TabControl.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.UpDownSpeed, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents btnSTOP As System.Windows.Forms.Button
    Friend WithEvents btnGO As System.Windows.Forms.Button
    Friend WithEvents btnBendingTurnout As System.Windows.Forms.Button
    Friend WithEvents btnStraightTurnout As System.Windows.Forms.Button
    Friend WithEvents chkF4 As System.Windows.Forms.CheckBox
    Friend WithEvents chkF3 As System.Windows.Forms.CheckBox
    Friend WithEvents chkF2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkF1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkF0 As System.Windows.Forms.CheckBox
    Friend WithEvents txtAddressTournout As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtLocoAddress As System.Windows.Forms.TextBox
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents radAchteruit As System.Windows.Forms.RadioButton
    Friend WithEvents radVooruit As System.Windows.Forms.RadioButton
    Friend WithEvents btnVrijGO As System.Windows.Forms.Button
    Friend WithEvents txtNaar As System.Windows.Forms.TextBox
    Friend WithEvents txtIS As System.Windows.Forms.TextBox
    Friend WithEvents txtVan As System.Windows.Forms.TextBox
    Friend WithEvents lblNaar As System.Windows.Forms.Label
    Friend WithEvents lblIs As System.Windows.Forms.Label
    Friend WithEvents lblVan As System.Windows.Forms.Label
    Friend WithEvents UpDownSpeed As System.Windows.Forms.NumericUpDown
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rBtnAchteruit As System.Windows.Forms.RadioButton
    Friend WithEvents rBtnVooruit As System.Windows.Forms.RadioButton
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents lstMacro As System.Windows.Forms.ListBox
    Friend WithEvents btnVoeruitMacro As System.Windows.Forms.Button
    Friend WithEvents txtMacroNR As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
