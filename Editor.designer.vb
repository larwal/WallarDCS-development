Public Partial Class Editor
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Editor))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtFormaat = New System.Windows.Forms.TextBox()
        Me.btnVerlaat = New System.Windows.Forms.Button()
        Me.btnBewaren = New System.Windows.Forms.Button()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.btnTelLijnen = New System.Windows.Forms.Button()
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.cboxBestanden = New System.Windows.Forms.ComboBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txtInput = New System.Windows.Forms.TextBox()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtFormaat)
        Me.Panel1.Controls.Add(Me.btnVerlaat)
        Me.Panel1.Controls.Add(Me.btnBewaren)
        Me.Panel1.Controls.Add(Me.lblInfo)
        Me.Panel1.Controls.Add(Me.btnTelLijnen)
        Me.Panel1.Controls.Add(Me.btnHelp)
        Me.Panel1.Controls.Add(Me.cboxBestanden)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(568, 64)
        Me.Panel1.TabIndex = 11
        '
        'txtFormaat
        '
        Me.txtFormaat.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFormaat.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormaat.Location = New System.Drawing.Point(3, 38)
        Me.txtFormaat.Name = "txtFormaat"
        Me.txtFormaat.Size = New System.Drawing.Size(460, 20)
        Me.txtFormaat.TabIndex = 12
        Me.txtFormaat.TabStop = False
        '
        'btnVerlaat
        '
        Me.btnVerlaat.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVerlaat.Location = New System.Drawing.Point(414, 8)
        Me.btnVerlaat.Name = "btnVerlaat"
        Me.btnVerlaat.Size = New System.Drawing.Size(49, 22)
        Me.btnVerlaat.TabIndex = 11
        Me.btnVerlaat.Text = "Verlaat"
        '
        'btnBewaren
        '
        Me.btnBewaren.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnBewaren.Location = New System.Drawing.Point(469, 9)
        Me.btnBewaren.Name = "btnBewaren"
        Me.btnBewaren.Size = New System.Drawing.Size(87, 22)
        Me.btnBewaren.TabIndex = 10
        Me.btnBewaren.Text = "Bewaar"
        '
        'lblInfo
        '
        Me.lblInfo.Location = New System.Drawing.Point(372, 9)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(37, 17)
        Me.lblInfo.TabIndex = 9
        Me.lblInfo.Text = "."
        Me.lblInfo.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'btnTelLijnen
        '
        Me.btnTelLijnen.Location = New System.Drawing.Point(303, 7)
        Me.btnTelLijnen.Name = "btnTelLijnen"
        Me.btnTelLijnen.Size = New System.Drawing.Size(63, 22)
        Me.btnTelLijnen.TabIndex = 6
        Me.btnTelLijnen.TabStop = False
        Me.btnTelLijnen.Text = "Tel aantal lijnen"
        '
        'btnHelp
        '
        Me.btnHelp.Location = New System.Drawing.Point(216, 7)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(81, 22)
        Me.btnHelp.TabIndex = 5
        Me.btnHelp.Text = "Invul Help"
        '
        'cboxBestanden
        '
        Me.cboxBestanden.FormattingEnabled = True
        Me.cboxBestanden.Items.AddRange(New Object() {"Bevelen", "BlokData", "HelpBestanden", "HelpMacroInput", "HelpProgramma", "K83K84Data", "LocoData", "Relaties", "s88MacroNrs"})
        Me.cboxBestanden.Location = New System.Drawing.Point(3, 9)
        Me.cboxBestanden.Name = "cboxBestanden"
        Me.cboxBestanden.Size = New System.Drawing.Size(207, 21)
        Me.cboxBestanden.Sorted = True
        Me.cboxBestanden.TabIndex = 1
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.txtInput)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 64)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(568, 312)
        Me.Panel2.TabIndex = 12
        '
        'txtInput
        '
        Me.txtInput.AcceptsReturn = True
        Me.txtInput.AcceptsTab = True
        Me.txtInput.AllowDrop = True
        Me.txtInput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtInput.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInput.Location = New System.Drawing.Point(0, 0)
        Me.txtInput.Multiline = True
        Me.txtInput.Name = "txtInput"
        Me.txtInput.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtInput.Size = New System.Drawing.Size(568, 312)
        Me.txtInput.TabIndex = 12
        '
        'Editor
        '
        Me.ClientSize = New System.Drawing.Size(568, 376)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Editor"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Editeer bestanden"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtFormaat As System.Windows.Forms.TextBox
    Friend WithEvents btnVerlaat As System.Windows.Forms.Button
    Friend WithEvents btnBewaren As System.Windows.Forms.Button
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents btnTelLijnen As System.Windows.Forms.Button
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents cboxBestanden As System.Windows.Forms.ComboBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents txtInput As System.Windows.Forms.TextBox
End Class
