'WallarDCS-2 van 28/01/2012
'Larno Walter

Option Explicit On
Option Compare Binary
Option Strict On

Imports System.IO

Public Class Input
    Inherits System.Windows.Forms.Form

    'forms
    Private helpMacrofrm As Help
    Private macroHelptekst As String

    'Class variables
    Private _lstDoubleClick As Boolean  'true= waarde opgehaald uit list
    Private _max As Integer
    Private _vanBlokNR As String
    Private _naarBlokNR As String
    Private _startBlokNR As String
    Private _tabel(,) As String
    Private _conditieStartBlokNR As String
    Private _telblok As Integer
    Private _lineNumber As Integer

    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnMarcoNR As System.Windows.Forms.Button
    Friend WithEvents txtMacro As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents btnPerpetiumMobile As System.Windows.Forms.Button
    Friend WithEvents txtPerpetiumMobile As System.Windows.Forms.TextBox
    Friend WithEvents btnPerpMobileActivate As System.Windows.Forms.Button
    Friend WithEvents btnOpkuisRW As System.Windows.Forms.Button
    Friend WithEvents ChkTRW As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox12 As System.Windows.Forms.GroupBox
    Friend WithEvents txtHaltPassagier As System.Windows.Forms.TextBox
    Friend WithEvents chkHaltPassagier As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox13 As System.Windows.Forms.GroupBox
    Friend WithEvents txtTussenhalt As System.Windows.Forms.TextBox
    Friend WithEvents btnTussenhalt As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtKeerTijd As System.Windows.Forms.TextBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents lstKeywords As System.Windows.Forms.ListBox
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents btnMacroHelp As System.Windows.Forms.Button
    Friend WithEvents btnAddMacro As System.Windows.Forms.Button
    Friend WithEvents lblAantalMacros As System.Windows.Forms.Label
    Friend WithEvents txtMacroNR As System.Windows.Forms.TextBox
    Friend WithEvents btnLoadMacro As System.Windows.Forms.Button
    Friend WithEvents btnSaveMacro As System.Windows.Forms.Button
    Friend WithEvents txtInputMacro As System.Windows.Forms.TextBox
    Friend WithEvents btnResetLijnNR As System.Windows.Forms.Button
    Friend WithEvents txtStartLijnNR As System.Windows.Forms.TextBox
    Friend WithEvents chkLijnNRSToevoegen As System.Windows.Forms.CheckBox
    Friend WithEvents chkProgrammalijnenWissen As System.Windows.Forms.CheckBox
    Friend WithEvents btnBewarenInArray As System.Windows.Forms.Button
    Private _tmpIndex(15) As Integer    'veronderstelt nooit meer dan 16 wisselstraten

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lstWisselStraten As System.Windows.Forms.ListBox
    Friend WithEvents btnEinde As System.Windows.Forms.Button
    Friend WithEvents lstNaarNR As System.Windows.Forms.ListBox
    Friend WithEvents txtReisweg As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents radFrontseinUit As System.Windows.Forms.RadioButton
    Friend WithEvents txtFrontseinUIT As System.Windows.Forms.TextBox
    Friend WithEvents chkF1F4 As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents txtVanBlokNR As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkKeren As System.Windows.Forms.CheckBox
    Friend WithEvents chkFrontseinAan As System.Windows.Forms.CheckBox
    Friend WithEvents radF1F4 As System.Windows.Forms.RadioButton
    Friend WithEvents radKerenFrontsein As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents chk_F1 As System.Windows.Forms.CheckBox
    Friend WithEvents chk_F4 As System.Windows.Forms.CheckBox
    Friend WithEvents chk_F3 As System.Windows.Forms.CheckBox
    Friend WithEvents chk_F2 As System.Windows.Forms.CheckBox
    Friend WithEvents btnFuncties As System.Windows.Forms.Button
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents txtHaltTijd As System.Windows.Forms.TextBox
    Friend WithEvents btnHaltTijd As System.Windows.Forms.Button
    Friend WithEvents btnKeerBevel As System.Windows.Forms.Button
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents btnConditie As System.Windows.Forms.Button
    Friend WithEvents txtConditie As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents btnMultiBlok As System.Windows.Forms.Button
    Friend WithEvents txtMultiBlok As System.Windows.Forms.TextBox
    Friend WithEvents btnFuit As System.Windows.Forms.Button
    Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Friend WithEvents btnFrontseinUIT As System.Windows.Forms.Button
    Friend WithEvents btnFrontseinAAN As System.Windows.Forms.Button
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtRPReisweg As System.Windows.Forms.TextBox
    Friend WithEvents txtReisplan As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtRPDeltaSnelheid As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtStartBlokNR As System.Windows.Forms.TextBox
    Friend WithEvents lstReiswegen As System.Windows.Forms.ListBox
    Friend WithEvents btnRWophalen As System.Windows.Forms.Button
    Friend WithEvents btnRWBewaren As System.Windows.Forms.Button
    Friend WithEvents btnRWToevoegen As System.Windows.Forms.Button
    Friend WithEvents txtChangeNR As System.Windows.Forms.TextBox
    Friend WithEvents btnRPvoegtoe As System.Windows.Forms.Button
    Friend WithEvents btnRPToevoegen As System.Windows.Forms.Button
    Friend WithEvents btnRPophalen As System.Windows.Forms.Button
    Friend WithEvents btnRPBewaren As System.Windows.Forms.Button
    Friend WithEvents lstReisplannen As System.Windows.Forms.ListBox
    Friend WithEvents txtNR As System.Windows.Forms.TextBox
    Friend WithEvents chkF4 As System.Windows.Forms.CheckBox
    Friend WithEvents chkF3 As System.Windows.Forms.CheckBox
    Friend WithEvents chkF2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkF1 As System.Windows.Forms.CheckBox
    Friend WithEvents txtNowLocoNR As System.Windows.Forms.TextBox
    Friend WithEvents btnRWUitvoeren As System.Windows.Forms.Button
    Friend WithEvents radConditieRW As System.Windows.Forms.RadioButton
    Friend WithEvents radGewoneRW As System.Windows.Forms.RadioButton
    Friend WithEvents chkEindeKeren As System.Windows.Forms.CheckBox
    Friend WithEvents chkHelpVertraging As System.Windows.Forms.CheckBox
    Friend WithEvents txtVertragingHalt As System.Windows.Forms.TextBox
    Friend WithEvents chkPulsFunctie As System.Windows.Forms.CheckBox
    Friend WithEvents btnSAve As System.Windows.Forms.Button
    Friend WithEvents btnQuit As System.Windows.Forms.Button
    Friend WithEvents chkCRW As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents btnTerminus As System.Windows.Forms.Button
    Friend WithEvents txtTerminus As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtBeschrijvingRP As System.Windows.Forms.TextBox
    Friend WithEvents btnExtraRWtoevoegen As System.Windows.Forms.Button
    Friend WithEvents btnResetRP As System.Windows.Forms.Button
    Friend WithEvents txtPlanNRnextIB As System.Windows.Forms.TextBox
    Friend WithEvents grpF1F4 As System.Windows.Forms.GroupBox

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Input))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.txtKeerTijd = New System.Windows.Forms.TextBox()
        Me.ChkTRW = New System.Windows.Forms.CheckBox()
        Me.GroupBox10 = New System.Windows.Forms.GroupBox()
        Me.btnPerpetiumMobile = New System.Windows.Forms.Button()
        Me.txtPerpetiumMobile = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnMarcoNR = New System.Windows.Forms.Button()
        Me.txtMacro = New System.Windows.Forms.TextBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.btnFuit = New System.Windows.Forms.Button()
        Me.btnFuncties = New System.Windows.Forms.Button()
        Me.chk_F2 = New System.Windows.Forms.CheckBox()
        Me.chk_F3 = New System.Windows.Forms.CheckBox()
        Me.chk_F4 = New System.Windows.Forms.CheckBox()
        Me.chk_F1 = New System.Windows.Forms.CheckBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.btnTerminus = New System.Windows.Forms.Button()
        Me.txtTerminus = New System.Windows.Forms.TextBox()
        Me.chkCRW = New System.Windows.Forms.CheckBox()
        Me.radGewoneRW = New System.Windows.Forms.RadioButton()
        Me.radConditieRW = New System.Windows.Forms.RadioButton()
        Me.txtNowLocoNR = New System.Windows.Forms.TextBox()
        Me.btnRWUitvoeren = New System.Windows.Forms.Button()
        Me.txtChangeNR = New System.Windows.Forms.TextBox()
        Me.btnRWToevoegen = New System.Windows.Forms.Button()
        Me.lstReiswegen = New System.Windows.Forms.ListBox()
        Me.btnRWophalen = New System.Windows.Forms.Button()
        Me.btnRWBewaren = New System.Windows.Forms.Button()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.btnMultiBlok = New System.Windows.Forms.Button()
        Me.txtMultiBlok = New System.Windows.Forms.TextBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.btnConditie = New System.Windows.Forms.Button()
        Me.txtConditie = New System.Windows.Forms.TextBox()
        Me.btnKeerBevel = New System.Windows.Forms.Button()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.txtVertragingHalt = New System.Windows.Forms.TextBox()
        Me.chkHelpVertraging = New System.Windows.Forms.CheckBox()
        Me.btnHaltTijd = New System.Windows.Forms.Button()
        Me.txtHaltTijd = New System.Windows.Forms.TextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.btnOpkuisRW = New System.Windows.Forms.Button()
        Me.grpF1F4 = New System.Windows.Forms.GroupBox()
        Me.chkPulsFunctie = New System.Windows.Forms.CheckBox()
        Me.chkF4 = New System.Windows.Forms.CheckBox()
        Me.chkF3 = New System.Windows.Forms.CheckBox()
        Me.chkF2 = New System.Windows.Forms.CheckBox()
        Me.chkF1 = New System.Windows.Forms.CheckBox()
        Me.chkKeren = New System.Windows.Forms.CheckBox()
        Me.chkFrontseinAan = New System.Windows.Forms.CheckBox()
        Me.radF1F4 = New System.Windows.Forms.RadioButton()
        Me.radKerenFrontsein = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.txtVanBlokNR = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.chkEindeKeren = New System.Windows.Forms.CheckBox()
        Me.chkF1F4 = New System.Windows.Forms.CheckBox()
        Me.txtFrontseinUIT = New System.Windows.Forms.TextBox()
        Me.radFrontseinUit = New System.Windows.Forms.RadioButton()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.btnEinde = New System.Windows.Forms.Button()
        Me.txtReisweg = New System.Windows.Forms.TextBox()
        Me.lstNaarNR = New System.Windows.Forms.ListBox()
        Me.lstWisselStraten = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox11 = New System.Windows.Forms.GroupBox()
        Me.btnFrontseinUIT = New System.Windows.Forms.Button()
        Me.btnFrontseinAAN = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.lstReisplannen = New System.Windows.Forms.ListBox()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.txtNR = New System.Windows.Forms.TextBox()
        Me.GroupBox13 = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtTussenhalt = New System.Windows.Forms.TextBox()
        Me.btnTussenhalt = New System.Windows.Forms.Button()
        Me.btnRPToevoegen = New System.Windows.Forms.Button()
        Me.GroupBox12 = New System.Windows.Forms.GroupBox()
        Me.txtHaltPassagier = New System.Windows.Forms.TextBox()
        Me.chkHaltPassagier = New System.Windows.Forms.CheckBox()
        Me.btnRPophalen = New System.Windows.Forms.Button()
        Me.txtBeschrijvingRP = New System.Windows.Forms.TextBox()
        Me.btnRPBewaren = New System.Windows.Forms.Button()
        Me.txtPlanNRnextIB = New System.Windows.Forms.TextBox()
        Me.btnResetRP = New System.Windows.Forms.Button()
        Me.btnExtraRWtoevoegen = New System.Windows.Forms.Button()
        Me.txtReisplan = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnPerpMobileActivate = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtStartBlokNR = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnRPvoegtoe = New System.Windows.Forms.Button()
        Me.txtRPReisweg = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtRPDeltaSnelheid = New System.Windows.Forms.TextBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lstKeywords = New System.Windows.Forms.ListBox()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.btnBewarenInArray = New System.Windows.Forms.Button()
        Me.chkProgrammalijnenWissen = New System.Windows.Forms.CheckBox()
        Me.chkLijnNRSToevoegen = New System.Windows.Forms.CheckBox()
        Me.txtStartLijnNR = New System.Windows.Forms.TextBox()
        Me.btnResetLijnNR = New System.Windows.Forms.Button()
        Me.btnMacroHelp = New System.Windows.Forms.Button()
        Me.btnAddMacro = New System.Windows.Forms.Button()
        Me.lblAantalMacros = New System.Windows.Forms.Label()
        Me.txtMacroNR = New System.Windows.Forms.TextBox()
        Me.btnLoadMacro = New System.Windows.Forms.Button()
        Me.btnSaveMacro = New System.Windows.Forms.Button()
        Me.txtInputMacro = New System.Windows.Forms.TextBox()
        Me.btnSAve = New System.Windows.Forms.Button()
        Me.btnQuit = New System.Windows.Forms.Button()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.grpF1F4.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox11.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox13.SuspendLayout()
        Me.GroupBox12.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1182, 815)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.ControlLight
        Me.TabPage1.Controls.Add(Me.txtKeerTijd)
        Me.TabPage1.Controls.Add(Me.ChkTRW)
        Me.TabPage1.Controls.Add(Me.GroupBox10)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.GroupBox5)
        Me.TabPage1.Controls.Add(Me.GroupBox4)
        Me.TabPage1.Controls.Add(Me.chkCRW)
        Me.TabPage1.Controls.Add(Me.radGewoneRW)
        Me.TabPage1.Controls.Add(Me.radConditieRW)
        Me.TabPage1.Controls.Add(Me.txtNowLocoNR)
        Me.TabPage1.Controls.Add(Me.btnRWUitvoeren)
        Me.TabPage1.Controls.Add(Me.txtChangeNR)
        Me.TabPage1.Controls.Add(Me.btnRWToevoegen)
        Me.TabPage1.Controls.Add(Me.lstReiswegen)
        Me.TabPage1.Controls.Add(Me.btnRWophalen)
        Me.TabPage1.Controls.Add(Me.btnRWBewaren)
        Me.TabPage1.Controls.Add(Me.GroupBox9)
        Me.TabPage1.Controls.Add(Me.GroupBox7)
        Me.TabPage1.Controls.Add(Me.btnKeerBevel)
        Me.TabPage1.Controls.Add(Me.GroupBox6)
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.txtReisweg)
        Me.TabPage1.Controls.Add(Me.lstNaarNR)
        Me.TabPage1.Controls.Add(Me.lstWisselStraten)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.GroupBox11)
        Me.TabPage1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(1174, 789)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Reiswegen"
        '
        'txtKeerTijd
        '
        Me.txtKeerTijd.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKeerTijd.Location = New System.Drawing.Point(928, 258)
        Me.txtKeerTijd.MaxLength = 3
        Me.txtKeerTijd.Name = "txtKeerTijd"
        Me.txtKeerTijd.Size = New System.Drawing.Size(52, 23)
        Me.txtKeerTijd.TabIndex = 33
        Me.txtKeerTijd.Text = "5"
        Me.txtKeerTijd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ChkTRW
        '
        Me.ChkTRW.Location = New System.Drawing.Point(365, 425)
        Me.ChkTRW.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.ChkTRW.Name = "ChkTRW"
        Me.ChkTRW.Size = New System.Drawing.Size(271, 24)
        Me.ChkTRW.TabIndex = 32
        Me.ChkTRW.Text = "Terminus Reisweg Bewerkingen"
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.btnPerpetiumMobile)
        Me.GroupBox10.Controls.Add(Me.txtPerpetiumMobile)
        Me.GroupBox10.Location = New System.Drawing.Point(701, 201)
        Me.GroupBox10.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(158, 48)
        Me.GroupBox10.TabIndex = 31
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Perpetium Mobile Trigger"
        '
        'btnPerpetiumMobile
        '
        Me.btnPerpetiumMobile.Location = New System.Drawing.Point(72, 16)
        Me.btnPerpetiumMobile.Name = "btnPerpetiumMobile"
        Me.btnPerpetiumMobile.Size = New System.Drawing.Size(73, 21)
        Me.btnPerpetiumMobile.TabIndex = 0
        Me.btnPerpetiumMobile.Text = "Nummer"
        '
        'txtPerpetiumMobile
        '
        Me.txtPerpetiumMobile.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPerpetiumMobile.Location = New System.Drawing.Point(16, 16)
        Me.txtPerpetiumMobile.MaxLength = 1
        Me.txtPerpetiumMobile.Name = "txtPerpetiumMobile"
        Me.txtPerpetiumMobile.Size = New System.Drawing.Size(40, 23)
        Me.txtPerpetiumMobile.TabIndex = 1
        Me.txtPerpetiumMobile.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnMarcoNR)
        Me.GroupBox1.Controls.Add(Me.txtMacro)
        Me.GroupBox1.Location = New System.Drawing.Point(866, 341)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(1, 1, 3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(127, 66)
        Me.GroupBox1.TabIndex = 30
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Macro"
        '
        'btnMarcoNR
        '
        Me.btnMarcoNR.Location = New System.Drawing.Point(65, 26)
        Me.btnMarcoNR.Name = "btnMarcoNR"
        Me.btnMarcoNR.Size = New System.Drawing.Size(49, 24)
        Me.btnMarcoNR.TabIndex = 0
        Me.btnMarcoNR.Text = "nr"
        '
        'txtMacro
        '
        Me.txtMacro.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMacro.Location = New System.Drawing.Point(7, 28)
        Me.txtMacro.MaxLength = 3
        Me.txtMacro.Name = "txtMacro"
        Me.txtMacro.Size = New System.Drawing.Size(52, 23)
        Me.txtMacro.TabIndex = 1
        Me.txtMacro.Text = "1"
        Me.txtMacro.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.btnFuit)
        Me.GroupBox5.Controls.Add(Me.btnFuncties)
        Me.GroupBox5.Controls.Add(Me.chk_F2)
        Me.GroupBox5.Controls.Add(Me.chk_F3)
        Me.GroupBox5.Controls.Add(Me.chk_F4)
        Me.GroupBox5.Controls.Add(Me.chk_F1)
        Me.GroupBox5.Location = New System.Drawing.Point(865, 96)
        Me.GroupBox5.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(127, 153)
        Me.GroupBox5.TabIndex = 9
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Functies"
        '
        'btnFuit
        '
        Me.btnFuit.Location = New System.Drawing.Point(10, 113)
        Me.btnFuit.Name = "btnFuit"
        Me.btnFuit.Size = New System.Drawing.Size(107, 24)
        Me.btnFuit.TabIndex = 5
        Me.btnFuit.Text = "Allemaal Uit"
        '
        'btnFuncties
        '
        Me.btnFuncties.Location = New System.Drawing.Point(10, 81)
        Me.btnFuncties.Name = "btnFuncties"
        Me.btnFuncties.Size = New System.Drawing.Size(107, 24)
        Me.btnFuncties.TabIndex = 2
        Me.btnFuncties.Text = "Zet F1,2,3,4"
        '
        'chk_F2
        '
        Me.chk_F2.Location = New System.Drawing.Point(10, 59)
        Me.chk_F2.Name = "chk_F2"
        Me.chk_F2.Size = New System.Drawing.Size(40, 16)
        Me.chk_F2.TabIndex = 3
        Me.chk_F2.Text = "F2"
        '
        'chk_F3
        '
        Me.chk_F3.Location = New System.Drawing.Point(77, 27)
        Me.chk_F3.Name = "chk_F3"
        Me.chk_F3.Size = New System.Drawing.Size(40, 16)
        Me.chk_F3.TabIndex = 1
        Me.chk_F3.Text = "F3"
        '
        'chk_F4
        '
        Me.chk_F4.Location = New System.Drawing.Point(77, 59)
        Me.chk_F4.Name = "chk_F4"
        Me.chk_F4.Size = New System.Drawing.Size(40, 16)
        Me.chk_F4.TabIndex = 4
        Me.chk_F4.Text = "F4"
        '
        'chk_F1
        '
        Me.chk_F1.Location = New System.Drawing.Point(10, 27)
        Me.chk_F1.Name = "chk_F1"
        Me.chk_F1.Size = New System.Drawing.Size(40, 16)
        Me.chk_F1.TabIndex = 0
        Me.chk_F1.Text = "F1"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.btnTerminus)
        Me.GroupBox4.Controls.Add(Me.txtTerminus)
        Me.GroupBox4.Location = New System.Drawing.Point(701, 150)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(158, 46)
        Me.GroupBox4.TabIndex = 29
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Terminus Reisweg"
        '
        'btnTerminus
        '
        Me.btnTerminus.Location = New System.Drawing.Point(71, 16)
        Me.btnTerminus.Name = "btnTerminus"
        Me.btnTerminus.Size = New System.Drawing.Size(74, 21)
        Me.btnTerminus.TabIndex = 0
        Me.btnTerminus.Text = "Nummer"
        '
        'txtTerminus
        '
        Me.txtTerminus.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTerminus.Location = New System.Drawing.Point(15, 16)
        Me.txtTerminus.MaxLength = 3
        Me.txtTerminus.Name = "txtTerminus"
        Me.txtTerminus.Size = New System.Drawing.Size(40, 23)
        Me.txtTerminus.TabIndex = 1
        Me.txtTerminus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkCRW
        '
        Me.chkCRW.Location = New System.Drawing.Point(366, 405)
        Me.chkCRW.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.chkCRW.Name = "chkCRW"
        Me.chkCRW.Size = New System.Drawing.Size(271, 24)
        Me.chkCRW.TabIndex = 11
        Me.chkCRW.Text = "Conditie- of PerpetiumMoblie Reisweg Bewerkingen"
        '
        'radGewoneRW
        '
        Me.radGewoneRW.Checked = True
        Me.radGewoneRW.Location = New System.Drawing.Point(32, 96)
        Me.radGewoneRW.Name = "radGewoneRW"
        Me.radGewoneRW.Size = New System.Drawing.Size(96, 16)
        Me.radGewoneRW.TabIndex = 27
        Me.radGewoneRW.TabStop = True
        Me.radGewoneRW.Text = "Reisweg start"
        '
        'radConditieRW
        '
        Me.radConditieRW.Location = New System.Drawing.Point(136, 96)
        Me.radConditieRW.Name = "radConditieRW"
        Me.radConditieRW.Size = New System.Drawing.Size(112, 16)
        Me.radConditieRW.TabIndex = 26
        Me.radConditieRW.Text = "Reisweg vervolg"
        '
        'txtNowLocoNR
        '
        Me.txtNowLocoNR.BackColor = System.Drawing.Color.Chartreuse
        Me.txtNowLocoNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNowLocoNR.ForeColor = System.Drawing.Color.Red
        Me.txtNowLocoNR.Location = New System.Drawing.Point(8, 413)
        Me.txtNowLocoNR.MaxLength = 2
        Me.txtNowLocoNR.Multiline = True
        Me.txtNowLocoNR.Name = "txtNowLocoNR"
        Me.txtNowLocoNR.Size = New System.Drawing.Size(48, 35)
        Me.txtNowLocoNR.TabIndex = 0
        Me.txtNowLocoNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtNowLocoNR.Visible = False
        '
        'btnRWUitvoeren
        '
        Me.btnRWUitvoeren.BackColor = System.Drawing.Color.Chartreuse
        Me.btnRWUitvoeren.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRWUitvoeren.Location = New System.Drawing.Point(56, 412)
        Me.btnRWUitvoeren.Name = "btnRWUitvoeren"
        Me.btnRWUitvoeren.Size = New System.Drawing.Size(303, 36)
        Me.btnRWUitvoeren.TabIndex = 1
        Me.btnRWUitvoeren.Text = "<- LocoNR      Onmiddelijk Uitvoeren"
        Me.btnRWUitvoeren.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRWUitvoeren.UseVisualStyleBackColor = False
        '
        'txtChangeNR
        '
        Me.txtChangeNR.BackColor = System.Drawing.SystemColors.Info
        Me.txtChangeNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChangeNR.Location = New System.Drawing.Point(662, 422)
        Me.txtChangeNR.MaxLength = 3
        Me.txtChangeNR.Name = "txtChangeNR"
        Me.txtChangeNR.Size = New System.Drawing.Size(40, 26)
        Me.txtChangeNR.TabIndex = 3
        Me.txtChangeNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnRWToevoegen
        '
        Me.btnRWToevoegen.BackColor = System.Drawing.SystemColors.Info
        Me.btnRWToevoegen.Location = New System.Drawing.Point(709, 424)
        Me.btnRWToevoegen.Name = "btnRWToevoegen"
        Me.btnRWToevoegen.Size = New System.Drawing.Size(127, 24)
        Me.btnRWToevoegen.TabIndex = 4
        Me.btnRWToevoegen.Text = "Toevoegen"
        Me.btnRWToevoegen.UseVisualStyleBackColor = False
        '
        'lstReiswegen
        '
        Me.lstReiswegen.BackColor = System.Drawing.Color.LightYellow
        Me.lstReiswegen.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lstReiswegen.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstReiswegen.FormattingEnabled = True
        Me.lstReiswegen.HorizontalScrollbar = True
        Me.lstReiswegen.ItemHeight = 14
        Me.lstReiswegen.Location = New System.Drawing.Point(0, 463)
        Me.lstReiswegen.Margin = New System.Windows.Forms.Padding(3, 2, 3, 3)
        Me.lstReiswegen.Name = "lstReiswegen"
        Me.lstReiswegen.Size = New System.Drawing.Size(1174, 326)
        Me.lstReiswegen.TabIndex = 15
        '
        'btnRWophalen
        '
        Me.btnRWophalen.BackColor = System.Drawing.SystemColors.Control
        Me.btnRWophalen.Location = New System.Drawing.Point(843, 424)
        Me.btnRWophalen.Name = "btnRWophalen"
        Me.btnRWophalen.Size = New System.Drawing.Size(72, 24)
        Me.btnRWophalen.TabIndex = 2
        Me.btnRWophalen.Text = "Ophalen"
        Me.btnRWophalen.UseVisualStyleBackColor = False
        '
        'btnRWBewaren
        '
        Me.btnRWBewaren.BackColor = System.Drawing.SystemColors.Control
        Me.btnRWBewaren.Location = New System.Drawing.Point(922, 424)
        Me.btnRWBewaren.Margin = New System.Windows.Forms.Padding(3, 3, 1, 3)
        Me.btnRWBewaren.Name = "btnRWBewaren"
        Me.btnRWBewaren.Size = New System.Drawing.Size(72, 24)
        Me.btnRWBewaren.TabIndex = 5
        Me.btnRWBewaren.Text = "Bewaren"
        Me.btnRWBewaren.UseVisualStyleBackColor = False
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.btnMultiBlok)
        Me.GroupBox9.Controls.Add(Me.txtMultiBlok)
        Me.GroupBox9.Location = New System.Drawing.Point(701, 255)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(158, 75)
        Me.GroupBox9.TabIndex = 10
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "MultiBlok reeks"
        '
        'btnMultiBlok
        '
        Me.btnMultiBlok.Location = New System.Drawing.Point(64, 19)
        Me.btnMultiBlok.Name = "btnMultiBlok"
        Me.btnMultiBlok.Size = New System.Drawing.Size(81, 40)
        Me.btnMultiBlok.TabIndex = 0
        Me.btnMultiBlok.Text = "Aantal blokken"
        '
        'txtMultiBlok
        '
        Me.txtMultiBlok.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMultiBlok.Location = New System.Drawing.Point(16, 28)
        Me.txtMultiBlok.MaxLength = 3
        Me.txtMultiBlok.Name = "txtMultiBlok"
        Me.txtMultiBlok.Size = New System.Drawing.Size(40, 23)
        Me.txtMultiBlok.TabIndex = 1
        Me.txtMultiBlok.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.btnConditie)
        Me.GroupBox7.Controls.Add(Me.txtConditie)
        Me.GroupBox7.Location = New System.Drawing.Point(701, 96)
        Me.GroupBox7.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(158, 48)
        Me.GroupBox7.TabIndex = 4
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Conditie Reisweg"
        '
        'btnConditie
        '
        Me.btnConditie.Location = New System.Drawing.Point(72, 16)
        Me.btnConditie.Name = "btnConditie"
        Me.btnConditie.Size = New System.Drawing.Size(73, 21)
        Me.btnConditie.TabIndex = 0
        Me.btnConditie.Text = "Nummer"
        '
        'txtConditie
        '
        Me.txtConditie.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConditie.Location = New System.Drawing.Point(16, 16)
        Me.txtConditie.MaxLength = 3
        Me.txtConditie.Name = "txtConditie"
        Me.txtConditie.Size = New System.Drawing.Size(40, 23)
        Me.txtConditie.TabIndex = 1
        Me.txtConditie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnKeerBevel
        '
        Me.btnKeerBevel.Location = New System.Drawing.Point(866, 258)
        Me.btnKeerBevel.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.btnKeerBevel.Name = "btnKeerBevel"
        Me.btnKeerBevel.Size = New System.Drawing.Size(59, 23)
        Me.btnKeerBevel.TabIndex = 7
        Me.btnKeerBevel.Text = "Keren"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.txtVertragingHalt)
        Me.GroupBox6.Controls.Add(Me.chkHelpVertraging)
        Me.GroupBox6.Controls.Add(Me.btnHaltTijd)
        Me.GroupBox6.Controls.Add(Me.txtHaltTijd)
        Me.GroupBox6.Location = New System.Drawing.Point(701, 329)
        Me.GroupBox6.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(158, 79)
        Me.GroupBox6.TabIndex = 6
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "HaltTijd"
        '
        'txtVertragingHalt
        '
        Me.txtVertragingHalt.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVertragingHalt.Location = New System.Drawing.Point(121, 13)
        Me.txtVertragingHalt.MaxLength = 1
        Me.txtVertragingHalt.Name = "txtVertragingHalt"
        Me.txtVertragingHalt.Size = New System.Drawing.Size(24, 23)
        Me.txtVertragingHalt.TabIndex = 1
        Me.txtVertragingHalt.Text = "0"
        Me.txtVertragingHalt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkHelpVertraging
        '
        Me.chkHelpVertraging.Location = New System.Drawing.Point(8, 13)
        Me.chkHelpVertraging.Name = "chkHelpVertraging"
        Me.chkHelpVertraging.Size = New System.Drawing.Size(120, 24)
        Me.chkHelpVertraging.TabIndex = 0
        Me.chkHelpVertraging.Text = "Extra Rijvertraging"
        '
        'btnHaltTijd
        '
        Me.btnHaltTijd.Location = New System.Drawing.Point(67, 42)
        Me.btnHaltTijd.Name = "btnHaltTijd"
        Me.btnHaltTijd.Size = New System.Drawing.Size(78, 24)
        Me.btnHaltTijd.TabIndex = 3
        Me.btnHaltTijd.Text = "HaltTijd"
        '
        'txtHaltTijd
        '
        Me.txtHaltTijd.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHaltTijd.Location = New System.Drawing.Point(16, 43)
        Me.txtHaltTijd.MaxLength = 2
        Me.txtHaltTijd.Name = "txtHaltTijd"
        Me.txtHaltTijd.Size = New System.Drawing.Size(40, 23)
        Me.txtHaltTijd.TabIndex = 2
        Me.txtHaltTijd.Text = "30"
        Me.txtHaltTijd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnOpkuisRW)
        Me.GroupBox3.Controls.Add(Me.grpF1F4)
        Me.GroupBox3.Controls.Add(Me.chkKeren)
        Me.GroupBox3.Controls.Add(Me.chkFrontseinAan)
        Me.GroupBox3.Controls.Add(Me.radF1F4)
        Me.GroupBox3.Controls.Add(Me.radKerenFrontsein)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.btnStart)
        Me.GroupBox3.Controls.Add(Me.txtVanBlokNR)
        Me.GroupBox3.Location = New System.Drawing.Point(8, 120)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(351, 152)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Start cmd"
        '
        'btnOpkuisRW
        '
        Me.btnOpkuisRW.Location = New System.Drawing.Point(268, 14)
        Me.btnOpkuisRW.Name = "btnOpkuisRW"
        Me.btnOpkuisRW.Size = New System.Drawing.Size(69, 36)
        Me.btnOpkuisRW.TabIndex = 7
        Me.btnOpkuisRW.Text = "Opkuisen reisweg"
        '
        'grpF1F4
        '
        Me.grpF1F4.Controls.Add(Me.chkPulsFunctie)
        Me.grpF1F4.Controls.Add(Me.chkF4)
        Me.grpF1F4.Controls.Add(Me.chkF3)
        Me.grpF1F4.Controls.Add(Me.chkF2)
        Me.grpF1F4.Controls.Add(Me.chkF1)
        Me.grpF1F4.Location = New System.Drawing.Point(144, 56)
        Me.grpF1F4.Name = "grpF1F4"
        Me.grpF1F4.Size = New System.Drawing.Size(201, 88)
        Me.grpF1F4.TabIndex = 6
        Me.grpF1F4.TabStop = False
        Me.grpF1F4.Text = "Functie F1 - F4"
        '
        'chkPulsFunctie
        '
        Me.chkPulsFunctie.Checked = True
        Me.chkPulsFunctie.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPulsFunctie.Location = New System.Drawing.Point(8, 35)
        Me.chkPulsFunctie.Name = "chkPulsFunctie"
        Me.chkPulsFunctie.Size = New System.Drawing.Size(80, 16)
        Me.chkPulsFunctie.TabIndex = 4
        Me.chkPulsFunctie.Text = "Puls functie"
        '
        'chkF4
        '
        Me.chkF4.Location = New System.Drawing.Point(153, 49)
        Me.chkF4.Name = "chkF4"
        Me.chkF4.Size = New System.Drawing.Size(40, 16)
        Me.chkF4.TabIndex = 3
        Me.chkF4.Text = "F4"
        '
        'chkF3
        '
        Me.chkF3.Location = New System.Drawing.Point(153, 25)
        Me.chkF3.Name = "chkF3"
        Me.chkF3.Size = New System.Drawing.Size(40, 16)
        Me.chkF3.TabIndex = 1
        Me.chkF3.Text = "F3"
        '
        'chkF2
        '
        Me.chkF2.Location = New System.Drawing.Point(105, 49)
        Me.chkF2.Name = "chkF2"
        Me.chkF2.Size = New System.Drawing.Size(40, 16)
        Me.chkF2.TabIndex = 2
        Me.chkF2.Text = "F2"
        '
        'chkF1
        '
        Me.chkF1.Location = New System.Drawing.Point(105, 25)
        Me.chkF1.Name = "chkF1"
        Me.chkF1.Size = New System.Drawing.Size(40, 16)
        Me.chkF1.TabIndex = 0
        Me.chkF1.Text = "F1"
        '
        'chkKeren
        '
        Me.chkKeren.Location = New System.Drawing.Point(16, 128)
        Me.chkKeren.Name = "chkKeren"
        Me.chkKeren.Size = New System.Drawing.Size(56, 16)
        Me.chkKeren.TabIndex = 5
        Me.chkKeren.Text = "Keren"
        '
        'chkFrontseinAan
        '
        Me.chkFrontseinAan.Checked = True
        Me.chkFrontseinAan.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFrontseinAan.Location = New System.Drawing.Point(16, 104)
        Me.chkFrontseinAan.Name = "chkFrontseinAan"
        Me.chkFrontseinAan.Size = New System.Drawing.Size(96, 16)
        Me.chkFrontseinAan.TabIndex = 4
        Me.chkFrontseinAan.Text = "Frontsein aan"
        '
        'radF1F4
        '
        Me.radF1F4.Location = New System.Drawing.Point(16, 56)
        Me.radF1F4.Name = "radF1F4"
        Me.radF1F4.Size = New System.Drawing.Size(112, 16)
        Me.radF1F4.TabIndex = 2
        Me.radF1F4.Text = "Functies F1 - F4"
        '
        'radKerenFrontsein
        '
        Me.radKerenFrontsein.Checked = True
        Me.radKerenFrontsein.Location = New System.Drawing.Point(16, 80)
        Me.radKerenFrontsein.Name = "radKerenFrontsein"
        Me.radKerenFrontsein.Size = New System.Drawing.Size(112, 16)
        Me.radKerenFrontsein.TabIndex = 3
        Me.radKerenFrontsein.TabStop = True
        Me.radKerenFrontsein.Text = "Frontsein - Keren"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 24)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Startblok NR:"
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(144, 16)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(102, 34)
        Me.btnStart.TabIndex = 1
        Me.btnStart.Text = "START blokNR"
        '
        'txtVanBlokNR
        '
        Me.txtVanBlokNR.BackColor = System.Drawing.Color.Lime
        Me.txtVanBlokNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVanBlokNR.Location = New System.Drawing.Point(88, 16)
        Me.txtVanBlokNR.MaxLength = 3
        Me.txtVanBlokNR.Name = "txtVanBlokNR"
        Me.txtVanBlokNR.Size = New System.Drawing.Size(40, 29)
        Me.txtVanBlokNR.TabIndex = 0
        Me.txtVanBlokNR.Text = "1"
        Me.txtVanBlokNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(438, 104)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 16)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Geldige wisselstraten:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkEindeKeren)
        Me.GroupBox2.Controls.Add(Me.chkF1F4)
        Me.GroupBox2.Controls.Add(Me.txtFrontseinUIT)
        Me.GroupBox2.Controls.Add(Me.radFrontseinUit)
        Me.GroupBox2.Controls.Add(Me.RadioButton1)
        Me.GroupBox2.Controls.Add(Me.btnEinde)
        Me.GroupBox2.Location = New System.Drawing.Point(9, 279)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(351, 128)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Einde cmd"
        '
        'chkEindeKeren
        '
        Me.chkEindeKeren.Location = New System.Drawing.Point(151, 19)
        Me.chkEindeKeren.Name = "chkEindeKeren"
        Me.chkEindeKeren.Size = New System.Drawing.Size(94, 16)
        Me.chkEindeKeren.TabIndex = 4
        Me.chkEindeKeren.Text = "Keren AAN"
        '
        'chkF1F4
        '
        Me.chkF1F4.Location = New System.Drawing.Point(10, 19)
        Me.chkF1F4.Name = "chkF1F4"
        Me.chkF1F4.Size = New System.Drawing.Size(120, 16)
        Me.chkF1F4.TabIndex = 3
        Me.chkF1F4.Text = "Functies F1 - F4 uit"
        '
        'txtFrontseinUIT
        '
        Me.txtFrontseinUIT.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFrontseinUIT.Location = New System.Drawing.Point(157, 80)
        Me.txtFrontseinUIT.MaxLength = 2
        Me.txtFrontseinUIT.Name = "txtFrontseinUIT"
        Me.txtFrontseinUIT.Size = New System.Drawing.Size(40, 23)
        Me.txtFrontseinUIT.TabIndex = 2
        Me.txtFrontseinUIT.Text = "10"
        Me.txtFrontseinUIT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'radFrontseinUit
        '
        Me.radFrontseinUit.Checked = True
        Me.radFrontseinUit.Location = New System.Drawing.Point(6, 80)
        Me.radFrontseinUit.Name = "radFrontseinUit"
        Me.radFrontseinUit.Size = New System.Drawing.Size(137, 24)
        Me.radFrontseinUit.TabIndex = 1
        Me.radFrontseinUit.TabStop = True
        Me.radFrontseinUit.Text = "Frontsein UIT na sec="
        '
        'RadioButton1
        '
        Me.RadioButton1.Location = New System.Drawing.Point(6, 50)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(96, 24)
        Me.RadioButton1.TabIndex = 0
        Me.RadioButton1.Text = "Frontsein AAN"
        '
        'btnEinde
        '
        Me.btnEinde.Location = New System.Drawing.Point(214, 50)
        Me.btnEinde.Name = "btnEinde"
        Me.btnEinde.Size = New System.Drawing.Size(122, 63)
        Me.btnEinde.TabIndex = 9
        Me.btnEinde.Text = "EIND blokNR"
        '
        'txtReisweg
        '
        Me.txtReisweg.BackColor = System.Drawing.Color.Gainsboro
        Me.txtReisweg.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtReisweg.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReisweg.Location = New System.Drawing.Point(0, 0)
        Me.txtReisweg.Multiline = True
        Me.txtReisweg.Name = "txtReisweg"
        Me.txtReisweg.Size = New System.Drawing.Size(1174, 88)
        Me.txtReisweg.TabIndex = 0
        '
        'lstNaarNR
        '
        Me.lstNaarNR.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstNaarNR.FormattingEnabled = True
        Me.lstNaarNR.ItemHeight = 18
        Me.lstNaarNR.Location = New System.Drawing.Point(366, 128)
        Me.lstNaarNR.Name = "lstNaarNR"
        Me.lstNaarNR.Size = New System.Drawing.Size(64, 274)
        Me.lstNaarNR.TabIndex = 4
        '
        'lstWisselStraten
        '
        Me.lstWisselStraten.Font = New System.Drawing.Font("Courier New", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstWisselStraten.FormattingEnabled = True
        Me.lstWisselStraten.HorizontalScrollbar = True
        Me.lstWisselStraten.ItemHeight = 18
        Me.lstWisselStraten.Location = New System.Drawing.Point(430, 128)
        Me.lstWisselStraten.Name = "lstWisselStraten"
        Me.lstWisselStraten.Size = New System.Drawing.Size(264, 274)
        Me.lstWisselStraten.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(374, 104)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "NAAR"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.btnFrontseinUIT)
        Me.GroupBox11.Controls.Add(Me.btnFrontseinAAN)
        Me.GroupBox11.Location = New System.Drawing.Point(867, 283)
        Me.GroupBox11.Margin = New System.Windows.Forms.Padding(2, 1, 2, 2)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(126, 55)
        Me.GroupBox11.TabIndex = 9
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "Frontsein"
        '
        'btnFrontseinUIT
        '
        Me.btnFrontseinUIT.Location = New System.Drawing.Point(64, 23)
        Me.btnFrontseinUIT.Name = "btnFrontseinUIT"
        Me.btnFrontseinUIT.Size = New System.Drawing.Size(49, 24)
        Me.btnFrontseinUIT.TabIndex = 1
        Me.btnFrontseinUIT.Text = "Uit"
        '
        'btnFrontseinAAN
        '
        Me.btnFrontseinAAN.Location = New System.Drawing.Point(7, 23)
        Me.btnFrontseinAAN.Name = "btnFrontseinAAN"
        Me.btnFrontseinAAN.Size = New System.Drawing.Size(51, 24)
        Me.btnFrontseinAAN.TabIndex = 0
        Me.btnFrontseinAAN.Text = "Aan"
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.ControlLight
        Me.TabPage2.Controls.Add(Me.lstReisplannen)
        Me.TabPage2.Controls.Add(Me.GroupBox8)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(1174, 789)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Reisplannen"
        '
        'lstReisplannen
        '
        Me.lstReisplannen.BackColor = System.Drawing.Color.LightYellow
        Me.lstReisplannen.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lstReisplannen.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstReisplannen.FormattingEnabled = True
        Me.lstReisplannen.HorizontalScrollbar = True
        Me.lstReisplannen.ItemHeight = 14
        Me.lstReisplannen.Location = New System.Drawing.Point(0, 197)
        Me.lstReisplannen.Name = "lstReisplannen"
        Me.lstReisplannen.Size = New System.Drawing.Size(1174, 592)
        Me.lstReisplannen.TabIndex = 1
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.txtNR)
        Me.GroupBox8.Controls.Add(Me.GroupBox13)
        Me.GroupBox8.Controls.Add(Me.btnRPToevoegen)
        Me.GroupBox8.Controls.Add(Me.GroupBox12)
        Me.GroupBox8.Controls.Add(Me.btnRPophalen)
        Me.GroupBox8.Controls.Add(Me.txtBeschrijvingRP)
        Me.GroupBox8.Controls.Add(Me.btnRPBewaren)
        Me.GroupBox8.Controls.Add(Me.txtPlanNRnextIB)
        Me.GroupBox8.Controls.Add(Me.btnResetRP)
        Me.GroupBox8.Controls.Add(Me.btnExtraRWtoevoegen)
        Me.GroupBox8.Controls.Add(Me.txtReisplan)
        Me.GroupBox8.Controls.Add(Me.Label8)
        Me.GroupBox8.Controls.Add(Me.btnPerpMobileActivate)
        Me.GroupBox8.Controls.Add(Me.Label5)
        Me.GroupBox8.Controls.Add(Me.txtStartBlokNR)
        Me.GroupBox8.Controls.Add(Me.Label7)
        Me.GroupBox8.Controls.Add(Me.btnRPvoegtoe)
        Me.GroupBox8.Controls.Add(Me.txtRPReisweg)
        Me.GroupBox8.Controls.Add(Me.Label6)
        Me.GroupBox8.Controls.Add(Me.txtRPDeltaSnelheid)
        Me.GroupBox8.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox8.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(1174, 182)
        Me.GroupBox8.TabIndex = 0
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Reisplan"
        '
        'txtNR
        '
        Me.txtNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNR.Location = New System.Drawing.Point(751, 148)
        Me.txtNR.MaxLength = 3
        Me.txtNR.Name = "txtNR"
        Me.txtNR.Size = New System.Drawing.Size(48, 20)
        Me.txtNR.TabIndex = 1
        Me.txtNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox13
        '
        Me.GroupBox13.Controls.Add(Me.Label10)
        Me.GroupBox13.Controls.Add(Me.txtTussenhalt)
        Me.GroupBox13.Controls.Add(Me.btnTussenhalt)
        Me.GroupBox13.Location = New System.Drawing.Point(980, 90)
        Me.GroupBox13.Name = "GroupBox13"
        Me.GroupBox13.Size = New System.Drawing.Size(179, 43)
        Me.GroupBox13.TabIndex = 17
        Me.GroupBox13.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(149, 19)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(24, 13)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "sec"
        '
        'txtTussenhalt
        '
        Me.txtTussenhalt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTussenhalt.Location = New System.Drawing.Point(95, 16)
        Me.txtTussenhalt.MaxLength = 3
        Me.txtTussenhalt.Name = "txtTussenhalt"
        Me.txtTussenhalt.Size = New System.Drawing.Size(48, 20)
        Me.txtTussenhalt.TabIndex = 16
        Me.txtTussenhalt.Text = "30"
        Me.txtTussenhalt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnTussenhalt
        '
        Me.btnTussenhalt.Location = New System.Drawing.Point(6, 13)
        Me.btnTussenhalt.Name = "btnTussenhalt"
        Me.btnTussenhalt.Size = New System.Drawing.Size(83, 24)
        Me.btnTussenhalt.TabIndex = 15
        Me.btnTussenhalt.Text = "Tussenhalt="
        '
        'btnRPToevoegen
        '
        Me.btnRPToevoegen.BackColor = System.Drawing.SystemColors.Info
        Me.btnRPToevoegen.Location = New System.Drawing.Point(818, 144)
        Me.btnRPToevoegen.Name = "btnRPToevoegen"
        Me.btnRPToevoegen.Size = New System.Drawing.Size(143, 27)
        Me.btnRPToevoegen.TabIndex = 2
        Me.btnRPToevoegen.Text = "Toevoegen"
        Me.btnRPToevoegen.UseVisualStyleBackColor = False
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.txtHaltPassagier)
        Me.GroupBox12.Controls.Add(Me.chkHaltPassagier)
        Me.GroupBox12.Location = New System.Drawing.Point(487, 93)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Size = New System.Drawing.Size(319, 40)
        Me.GroupBox12.TabIndex = 16
        Me.GroupBox12.TabStop = False
        '
        'txtHaltPassagier
        '
        Me.txtHaltPassagier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHaltPassagier.Location = New System.Drawing.Point(264, 14)
        Me.txtHaltPassagier.MaxLength = 3
        Me.txtHaltPassagier.Name = "txtHaltPassagier"
        Me.txtHaltPassagier.Size = New System.Drawing.Size(48, 20)
        Me.txtHaltPassagier.TabIndex = 8
        Me.txtHaltPassagier.Text = "315"
        Me.txtHaltPassagier.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkHaltPassagier
        '
        Me.chkHaltPassagier.Location = New System.Drawing.Point(6, 14)
        Me.chkHaltPassagier.Name = "chkHaltPassagier"
        Me.chkHaltPassagier.Size = New System.Drawing.Size(257, 20)
        Me.chkHaltPassagier.TabIndex = 7
        Me.chkHaltPassagier.Text = "Peronhalttijd passagierstrein :  vertraging+sec: "
        '
        'btnRPophalen
        '
        Me.btnRPophalen.BackColor = System.Drawing.SystemColors.Control
        Me.btnRPophalen.Location = New System.Drawing.Point(980, 144)
        Me.btnRPophalen.Name = "btnRPophalen"
        Me.btnRPophalen.Size = New System.Drawing.Size(89, 27)
        Me.btnRPophalen.TabIndex = 0
        Me.btnRPophalen.Text = "Ophalen"
        Me.btnRPophalen.UseVisualStyleBackColor = False
        '
        'txtBeschrijvingRP
        '
        Me.txtBeschrijvingRP.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBeschrijvingRP.Location = New System.Drawing.Point(9, 71)
        Me.txtBeschrijvingRP.Name = "txtBeschrijvingRP"
        Me.txtBeschrijvingRP.Size = New System.Drawing.Size(1150, 20)
        Me.txtBeschrijvingRP.TabIndex = 5
        '
        'btnRPBewaren
        '
        Me.btnRPBewaren.BackColor = System.Drawing.SystemColors.Control
        Me.btnRPBewaren.Location = New System.Drawing.Point(1075, 144)
        Me.btnRPBewaren.Name = "btnRPBewaren"
        Me.btnRPBewaren.Size = New System.Drawing.Size(84, 27)
        Me.btnRPBewaren.TabIndex = 3
        Me.btnRPBewaren.Text = "Bewaren"
        Me.btnRPBewaren.UseVisualStyleBackColor = False
        '
        'txtPlanNRnextIB
        '
        Me.txtPlanNRnextIB.Enabled = False
        Me.txtPlanNRnextIB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPlanNRnextIB.Location = New System.Drawing.Point(433, 102)
        Me.txtPlanNRnextIB.MaxLength = 3
        Me.txtPlanNRnextIB.Name = "txtPlanNRnextIB"
        Me.txtPlanNRnextIB.Size = New System.Drawing.Size(48, 20)
        Me.txtPlanNRnextIB.TabIndex = 11
        Me.txtPlanNRnextIB.Text = "000"
        Me.txtPlanNRnextIB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtPlanNRnextIB.Visible = False
        '
        'btnResetRP
        '
        Me.btnResetRP.Location = New System.Drawing.Point(818, 97)
        Me.btnResetRP.Name = "btnResetRP"
        Me.btnResetRP.Size = New System.Drawing.Size(143, 36)
        Me.btnResetRP.TabIndex = 3
        Me.btnResetRP.Text = "Reset Reisplan"
        '
        'btnExtraRWtoevoegen
        '
        Me.btnExtraRWtoevoegen.Location = New System.Drawing.Point(318, 144)
        Me.btnExtraRWtoevoegen.Name = "btnExtraRWtoevoegen"
        Me.btnExtraRWtoevoegen.Size = New System.Drawing.Size(163, 24)
        Me.btnExtraRWtoevoegen.TabIndex = 4
        Me.btnExtraRWtoevoegen.Text = "Extra reiswegen toevoegen"
        '
        'txtReisplan
        '
        Me.txtReisplan.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.txtReisplan.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReisplan.Location = New System.Drawing.Point(6, 19)
        Me.txtReisplan.Name = "txtReisplan"
        Me.txtReisplan.Size = New System.Drawing.Size(1153, 20)
        Me.txtReisplan.TabIndex = 4
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(6, 52)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(610, 16)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Beschrijving van het reisplan  (wordt alfabetisch gerangschikt in display mode)"
        '
        'btnPerpMobileActivate
        '
        Me.btnPerpMobileActivate.Location = New System.Drawing.Point(487, 144)
        Me.btnPerpMobileActivate.Name = "btnPerpMobileActivate"
        Me.btnPerpMobileActivate.Size = New System.Drawing.Size(150, 24)
        Me.btnPerpMobileActivate.TabIndex = 13
        Me.btnPerpMobileActivate.Text = "PerpetiumMobile activeren"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 105)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(97, 18)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Start Bloknummer"
        '
        'txtStartBlokNR
        '
        Me.txtStartBlokNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartBlokNR.Location = New System.Drawing.Point(109, 97)
        Me.txtStartBlokNR.MaxLength = 3
        Me.txtStartBlokNR.Name = "txtStartBlokNR"
        Me.txtStartBlokNR.Size = New System.Drawing.Size(48, 26)
        Me.txtStartBlokNR.TabIndex = 0
        Me.txtStartBlokNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(47, 150)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(56, 16)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "Reisweg"
        '
        'btnRPvoegtoe
        '
        Me.btnRPvoegtoe.Location = New System.Drawing.Point(171, 144)
        Me.btnRPvoegtoe.Name = "btnRPvoegtoe"
        Me.btnRPvoegtoe.Size = New System.Drawing.Size(141, 24)
        Me.btnRPvoegtoe.TabIndex = 2
        Me.btnRPvoegtoe.Text = "Eerste reisweg toevoegen"
        '
        'txtRPReisweg
        '
        Me.txtRPReisweg.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRPReisweg.Location = New System.Drawing.Point(109, 142)
        Me.txtRPReisweg.MaxLength = 3
        Me.txtRPReisweg.Name = "txtRPReisweg"
        Me.txtRPReisweg.Size = New System.Drawing.Size(48, 26)
        Me.txtRPReisweg.TabIndex = 1
        Me.txtRPReisweg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(168, 99)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(129, 24)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "'B' blok Snelheidswijziging"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label6.Visible = False
        '
        'txtRPDeltaSnelheid
        '
        Me.txtRPDeltaSnelheid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRPDeltaSnelheid.Location = New System.Drawing.Point(303, 102)
        Me.txtRPDeltaSnelheid.MaxLength = 3
        Me.txtRPDeltaSnelheid.Name = "txtRPDeltaSnelheid"
        Me.txtRPDeltaSnelheid.Size = New System.Drawing.Size(48, 20)
        Me.txtRPDeltaSnelheid.TabIndex = 3
        Me.txtRPDeltaSnelheid.Text = "+00"
        Me.txtRPDeltaSnelheid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtRPDeltaSnelheid.Visible = False
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.SplitContainer1)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(1174, 789)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Macro Editor"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstKeywords)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(1174, 789)
        Me.SplitContainer1.SplitterDistance = 214
        Me.SplitContainer1.TabIndex = 6
        '
        'lstKeywords
        '
        Me.lstKeywords.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstKeywords.Enabled = False
        Me.lstKeywords.FormattingEnabled = True
        Me.lstKeywords.Items.AddRange(New Object() {"beep", "end", "getbl", "getlok", "gosub", "goto", "if", "ifb", "ifbp", "ifi", "ifip", "ifs", "ifsp", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "clearallanalog", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "clearalldigital", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "clearanalogchannel", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "cleardigitalchannel", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "closedevice", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "opendevice", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "outputallanalog", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "outputanalogchannel", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "readallanalog", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "readalldigital", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "readanalogchannel", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "readcounter", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "readdigitalchannel", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "resetcounter", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "setallanalog", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "setalldigital", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "setanalogchannel", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "setcounterdebouncetime", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "setdigitalchannel", "k8055" & Global.Microsoft.VisualBasic.ChrW(9) & "writealldigital", "nop", "return", "run" & Global.Microsoft.VisualBasic.ChrW(9) & "opafrem", "run" & Global.Microsoft.VisualBasic.ChrW(9) & "oprij", "runk83", "runk84", "say", "sblok", "setbl", "setlok", "titel", "trein"})
        Me.lstKeywords.Location = New System.Drawing.Point(0, 0)
        Me.lstKeywords.Name = "lstKeywords"
        Me.lstKeywords.Size = New System.Drawing.Size(214, 789)
        Me.lstKeywords.Sorted = True
        Me.lstKeywords.TabIndex = 5
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnBewarenInArray)
        Me.SplitContainer2.Panel1.Controls.Add(Me.chkProgrammalijnenWissen)
        Me.SplitContainer2.Panel1.Controls.Add(Me.chkLijnNRSToevoegen)
        Me.SplitContainer2.Panel1.Controls.Add(Me.txtStartLijnNR)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnResetLijnNR)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnMacroHelp)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnAddMacro)
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblAantalMacros)
        Me.SplitContainer2.Panel1.Controls.Add(Me.txtMacroNR)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnLoadMacro)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnSaveMacro)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.txtInputMacro)
        Me.SplitContainer2.Size = New System.Drawing.Size(956, 789)
        Me.SplitContainer2.SplitterDistance = 42
        Me.SplitContainer2.TabIndex = 0
        '
        'btnBewarenInArray
        '
        Me.btnBewarenInArray.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBewarenInArray.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnBewarenInArray.Enabled = False
        Me.btnBewarenInArray.Location = New System.Drawing.Point(510, 3)
        Me.btnBewarenInArray.Name = "btnBewarenInArray"
        Me.btnBewarenInArray.Size = New System.Drawing.Size(96, 36)
        Me.btnBewarenInArray.TabIndex = 16
        Me.btnBewarenInArray.Text = "Nieuwe macro toevoegen"
        Me.btnBewarenInArray.UseVisualStyleBackColor = False
        '
        'chkProgrammalijnenWissen
        '
        Me.chkProgrammalijnenWissen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkProgrammalijnenWissen.AutoSize = True
        Me.chkProgrammalijnenWissen.Checked = True
        Me.chkProgrammalijnenWissen.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkProgrammalijnenWissen.Enabled = False
        Me.chkProgrammalijnenWissen.Location = New System.Drawing.Point(612, 22)
        Me.chkProgrammalijnenWissen.Name = "chkProgrammalijnenWissen"
        Me.chkProgrammalijnenWissen.Size = New System.Drawing.Size(138, 17)
        Me.chkProgrammalijnenWissen.TabIndex = 15
        Me.chkProgrammalijnenWissen.Text = "Programmalijnen wissen"
        Me.chkProgrammalijnenWissen.UseVisualStyleBackColor = True
        '
        'chkLijnNRSToevoegen
        '
        Me.chkLijnNRSToevoegen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkLijnNRSToevoegen.AutoSize = True
        Me.chkLijnNRSToevoegen.Checked = True
        Me.chkLijnNRSToevoegen.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLijnNRSToevoegen.Enabled = False
        Me.chkLijnNRSToevoegen.Location = New System.Drawing.Point(612, 7)
        Me.chkLijnNRSToevoegen.Name = "chkLijnNRSToevoegen"
        Me.chkLijnNRSToevoegen.Size = New System.Drawing.Size(119, 17)
        Me.chkLijnNRSToevoegen.TabIndex = 14
        Me.chkLijnNRSToevoegen.Text = "LijnNRS toevoegen"
        Me.chkLijnNRSToevoegen.UseVisualStyleBackColor = True
        '
        'txtStartLijnNR
        '
        Me.txtStartLijnNR.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStartLijnNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartLijnNR.Location = New System.Drawing.Point(756, 10)
        Me.txtStartLijnNR.Name = "txtStartLijnNR"
        Me.txtStartLijnNR.Size = New System.Drawing.Size(38, 23)
        Me.txtStartLijnNR.TabIndex = 13
        Me.txtStartLijnNR.Text = "0"
        Me.txtStartLijnNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnResetLijnNR
        '
        Me.btnResetLijnNR.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnResetLijnNR.Enabled = False
        Me.btnResetLijnNR.Location = New System.Drawing.Point(800, 3)
        Me.btnResetLijnNR.Name = "btnResetLijnNR"
        Me.btnResetLijnNR.Size = New System.Drawing.Size(88, 36)
        Me.btnResetLijnNR.TabIndex = 12
        Me.btnResetLijnNR.Text = "Start van LijnNR"
        Me.btnResetLijnNR.UseVisualStyleBackColor = True
        '
        'btnMacroHelp
        '
        Me.btnMacroHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMacroHelp.Location = New System.Drawing.Point(894, 3)
        Me.btnMacroHelp.Name = "btnMacroHelp"
        Me.btnMacroHelp.Size = New System.Drawing.Size(51, 36)
        Me.btnMacroHelp.TabIndex = 11
        Me.btnMacroHelp.Text = "Help"
        '
        'btnAddMacro
        '
        Me.btnAddMacro.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddMacro.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnAddMacro.Location = New System.Drawing.Point(408, 3)
        Me.btnAddMacro.Name = "btnAddMacro"
        Me.btnAddMacro.Size = New System.Drawing.Size(96, 36)
        Me.btnAddMacro.TabIndex = 9
        Me.btnAddMacro.Text = "Nieuwe macro aanmaken"
        Me.btnAddMacro.UseVisualStyleBackColor = False
        '
        'lblAantalMacros
        '
        Me.lblAantalMacros.AutoSize = True
        Me.lblAantalMacros.Location = New System.Drawing.Point(3, 17)
        Me.lblAantalMacros.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.lblAantalMacros.Name = "lblAantalMacros"
        Me.lblAantalMacros.Size = New System.Drawing.Size(0, 13)
        Me.lblAantalMacros.TabIndex = 10
        '
        'txtMacroNR
        '
        Me.txtMacroNR.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMacroNR.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMacroNR.Location = New System.Drawing.Point(205, 7)
        Me.txtMacroNR.Name = "txtMacroNR"
        Me.txtMacroNR.Size = New System.Drawing.Size(45, 26)
        Me.txtMacroNR.TabIndex = 6
        Me.txtMacroNR.Text = "1"
        Me.txtMacroNR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnLoadMacro
        '
        Me.btnLoadMacro.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoadMacro.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnLoadMacro.Location = New System.Drawing.Point(256, 3)
        Me.btnLoadMacro.Name = "btnLoadMacro"
        Me.btnLoadMacro.Size = New System.Drawing.Size(58, 36)
        Me.btnLoadMacro.TabIndex = 7
        Me.btnLoadMacro.Text = "Editeer Macro"
        Me.btnLoadMacro.UseVisualStyleBackColor = False
        '
        'btnSaveMacro
        '
        Me.btnSaveMacro.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveMacro.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.btnSaveMacro.Location = New System.Drawing.Point(320, 3)
        Me.btnSaveMacro.Name = "btnSaveMacro"
        Me.btnSaveMacro.Size = New System.Drawing.Size(82, 36)
        Me.btnSaveMacro.TabIndex = 8
        Me.btnSaveMacro.Text = "Bewaar wijzigingen"
        Me.btnSaveMacro.UseVisualStyleBackColor = False
        '
        'txtInputMacro
        '
        Me.txtInputMacro.AcceptsReturn = True
        Me.txtInputMacro.AcceptsTab = True
        Me.txtInputMacro.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtInputMacro.Enabled = False
        Me.txtInputMacro.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInputMacro.Location = New System.Drawing.Point(0, 0)
        Me.txtInputMacro.Margin = New System.Windows.Forms.Padding(3, 2, 3, 3)
        Me.txtInputMacro.Multiline = True
        Me.txtInputMacro.Name = "txtInputMacro"
        Me.txtInputMacro.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtInputMacro.Size = New System.Drawing.Size(956, 743)
        Me.txtInputMacro.TabIndex = 1
        '
        'btnSAve
        '
        Me.btnSAve.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSAve.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnSAve.Location = New System.Drawing.Point(883, 821)
        Me.btnSAve.Name = "btnSAve"
        Me.btnSAve.Size = New System.Drawing.Size(295, 32)
        Me.btnSAve.TabIndex = 32
        Me.btnSAve.Text = "Afsluiten en op HD-bewaren Bewaren"
        Me.btnSAve.UseVisualStyleBackColor = False
        '
        'btnQuit
        '
        Me.btnQuit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnQuit.BackColor = System.Drawing.Color.Red
        Me.btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnQuit.ForeColor = System.Drawing.Color.White
        Me.btnQuit.Location = New System.Drawing.Point(703, 821)
        Me.btnQuit.Name = "btnQuit"
        Me.btnQuit.Size = New System.Drawing.Size(160, 32)
        Me.btnQuit.TabIndex = 31
        Me.btnQuit.Text = "Afsluiten Zonder bewaren"
        Me.btnQuit.UseVisualStyleBackColor = False
        '
        'Input
        '
        Me.CancelButton = Me.btnQuit
        Me.ClientSize = New System.Drawing.Size(1182, 855)
        Me.Controls.Add(Me.btnSAve)
        Me.Controls.Add(Me.btnQuit)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "Input"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Besturingspost"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.grpF1F4.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox11.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.GroupBox13.ResumeLayout(False)
        Me.GroupBox13.PerformLayout()
        Me.GroupBox12.ResumeLayout(False)
        Me.GroupBox12.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.PerformLayout()
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.Panel2.PerformLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Initialisatie en afsluiten dialogbox"

    Private Sub Input_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim MyStreamReader As StreamReader
        Dim i As Integer
        Dim s As String
        Me.Text = "WallarDCS:     Ingaves voor reiswegen en reisplannen en directe uitvoering"
        'Inlezen van "Bevelen.txt"
        Try
            MyStreamReader = File.OpenText(DataDir & "\Bevelen.txt")
            _max = CInt(MyStreamReader.ReadLine) - 1
            ReDim _tabel(_max, 3)
            For i = 0 To _max
                s = MyStreamReader.ReadLine
                _tabel(i, 3) = s.Substring(0, 1)
                _tabel(i, 0) = s.Substring(1, 3)
                _tabel(i, 1) = s.Substring(4, 3)
                _tabel(i, 2) = s.Substring(7)
            Next
            MyStreamReader.Close()
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: : Bevelen.txt " & exc.Message)
        End Try

        'helptekstMacro
        Try
            MyStreamReader = File.OpenText(DataDir & "\HelpMacroInput.txt")
            macroHelptekst = MyStreamReader.ReadToEnd
            MyStreamReader.Close()
        Catch ex As Exception
            MessageBox.Show("Fout in Bestand: HelpMacroInput.txt =" & ex.Message)
        End Try
        lstReiswegen.Items.Clear()
        lstReisplannen.Items.Clear()
        loadReiswegen()
        LoadReisplannen()
        btnSaveMacro.Enabled = False
        txtReisweg.Text = ""
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        Select Case TabControl1.SelectedIndex()
            Case 0
                TabControl1.Focus()
                txtVanBlokNR.Focus()
                txtVanBlokNR.SelectAll()
            Case 1
                TabControl1.Focus()
                txtStartBlokNR.Focus()
                txtStartBlokNR.SelectAll()
            Case 2
                lblAantalMacros.Text = "Aantal macro's = " + g_macroArray(0)
                TabControl1.Focus()
                txtInputMacro.Focus()
                txtInputMacro.SelectAll()
                btnBewarenInArray.Enabled = False
                btnLoadMacro.Enabled = True
                btnSaveMacro.Enabled = True
                chkLijnNRSToevoegen.Enabled = False
                chkProgrammalijnenWissen.Enabled = False
                txtInputMacro.Enabled = False
                lstKeywords.Enabled = False
                btnResetLijnNR.Enabled = False
                btnSaveMacro.Enabled = False

            Case Else
        End Select
    End Sub

    Private Sub btnSave_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSAve.Click
        SaveRW()
        SaveRP()
        SaveMacro()
        btnStart.Enabled = True
        Me.Hide()
    End Sub

    Private Sub btnQuit_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuit.Click
        btnStart.Enabled = True
        Me.Hide()
    End Sub

#End Region

#Region "Reiswegen"

    Private Sub btnStart_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        Dim i As Integer
        Dim DirFL As String
        If IsNumberOK(txtVanBlokNR.Text) Then i = (CInt(Val(txtVanBlokNR.Text))) Else txtVanBlokNR.Focus() : Exit Sub
        If chkCRW.Checked Then
            _conditieStartBlokNR = Format(CInt(txtVanBlokNR.Text), "000")
            btnStart.Enabled = False
        End If
        If ChkTRW.Checked Then
            _conditieStartBlokNR = Format(CInt(txtVanBlokNR.Text), "000")
            btnStart.Enabled = False
        End If
        Dim s As String = ""
        _lstDoubleClick = False
        lstWisselStraten.Items.Clear()
        If radGewoneRW.Checked = True Then 'gewone reisweg
            txtReisweg.Clear()
            _vanBlokNR = Format(i, "000")
            txtStartBlokNR.Text = _vanBlokNR
            _startBlokNR = _vanBlokNR
            If radKerenFrontsein.Checked Then
                If chkFrontseinAan.Checked Then
                    If chkKeren.Checked Then
                        DirFL = "3"
                    Else
                        DirFL = "1" 'standaard
                    End If
                Else
                    If chkKeren.Checked Then
                        DirFL = "2"
                    Else
                        DirFL = "0"
                    End If
                End If
            Else
                DirFL = "1" 'standaard
            End If
            i = 64   'reset
            If radF1F4.Checked Then
                If chkPulsFunctie.Checked Then
                    If chkF1.Checked Then i += 1
                    If chkF2.Checked Then i += 2
                    If chkF3.Checked Then i += 4
                    If chkF4.Checked Then i += 8
                    InvullenNaarBlok("P" & DirFL & Chr(i) & s) 'pulsfunctie
                Else
                    If chkF1.Checked Then i += 1
                    If chkF2.Checked Then i += 2
                    If chkF3.Checked Then i += 4
                    If chkF4.Checked Then i += 8
                    InvullenNaarBlok("D" & DirFL & Chr(i) & s) 'duurfunctie
                End If
            Else
                InvullenNaarBlok("#" & DirFL & "@" & s)
            End If
            'invullen van de locoNR
            SyncLock syncOK1
                txtNowLocoNR.Text = g_BlokLocoArray(CInt(_startBlokNR)).Substring(4)
            End SyncLock
            If txtNowLocoNR.Text = "000" Then

                MessageBox.Show("Op de Startblok staat geen loco?", _
                "Fout bij ingave startblok", MessageBoxButtons.OK, MessageBoxIcon.Question)
            End If
        Else    'conditie/ terminus reisweg
            If chkCRW.Checked Then
                _vanBlokNR = Format(i, "000")
                txtStartBlokNR.Text = _vanBlokNR
                _startBlokNR = _vanBlokNR
                InvullenNaarBlok("")
            Else
                _vanBlokNR = Format(i, "000")
                txtStartBlokNR.Text = _vanBlokNR
                _startBlokNR = _vanBlokNR
                InvullenNaarBlok("")
            End If
        End If
    End Sub

    Private Sub lstNaarNR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstNaarNR.Click

        If lstNaarNR.SelectedIndex >= 0 Then
            If chkCRW.Checked Or ChkTRW.Checked Then _telblok += 1
            Dim i, teller As Integer
            Dim newBlokNR As String = CStr(lstNaarNR.Items.Item(lstNaarNR.SelectedIndex))
            lstWisselStraten.Items.Clear()
            For i = 0 To _max
                If _tabel(i, 0) = _vanBlokNR AndAlso _tabel(i, 1) = newBlokNR Then
                    _tmpIndex(teller) = i   'bewaar index van Bevelen
                    teller += 1
                End If
            Next
            If teller = 1 Then
                If txtReisweg.Text.Length = 3 Then
                    txtReisweg.Text &= _tabel(_tmpIndex(0), 3)
                ElseIf txtReisweg.Text.Length = 7 Then
                    txtReisweg.Text = txtReisweg.Text.Substring(0, 3) & _tabel(_tmpIndex(0), 3) & "|" & txtReisweg.Text.Substring(3)
                End If
                If txtReisweg.Text.Length = 9 Then
                    txtReisweg.Text = txtReisweg.Text & "|" & _tabel(_tmpIndex(0), 2).Substring(1) & "b" & newBlokNR
                Else
                    If txtReisweg.Text.LastIndexOf("k") = txtReisweg.Text.Length - 4 Then
                        txtReisweg.Text = txtReisweg.Text & _tabel(_tmpIndex(0), 2).Substring(1) & "b" & newBlokNR
                    Else
                        txtReisweg.Text = txtReisweg.Text & _tabel(_tmpIndex(0), 2) & "b" & newBlokNR
                    End If
                End If
                If Not ChkTRW.Checked Then
                    txtVanBlokNR.Text = newBlokNR
                    _vanBlokNR = newBlokNR
                End If
                lstNaarNR.Items.Clear()
                InvullenNaarBlok("")
            ElseIf teller > 1 Then
                lstWisselStraten.Items.Clear()
                _naarBlokNR = CStr(lstNaarNR.Items.Item(lstNaarNR.SelectedIndex))
                For i = 0 To teller - 1
                    lstWisselStraten.Items.Add(_tabel(_tmpIndex(i), 2))
                Next
            End If
            If chkCRW.Checked AndAlso _telblok = 2 Then
                txtVanBlokNR.Text = _conditieStartBlokNR
                _vanBlokNR = _conditieStartBlokNR
                InvullenNaarBlok("")
                _telblok = 0    'reset
            End If
        End If

    End Sub

    Private Sub InvullenNaarBlok(ByVal Dxx As String)
        Dim i As Integer
        Dim s As String
        lstNaarNR.Items.Clear()
        For i = 0 To _max
            If _tabel(i, 0) = _vanBlokNR Then
                lstNaarNR.Items.Add(_tabel(i, 1))
                s = _tabel(i, 1)
            End If
        Next
        txtReisweg.Text = txtReisweg.Text & Dxx
    End Sub

    Private Sub btnEinde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEinde.Click
        Dim i As Integer
        If IsNumberOK(txtFrontseinUIT.Text) Then i = (CInt(Val(txtFrontseinUIT.Text))) Else txtFrontseinUIT.Focus() : Exit Sub
        txtReisweg.Text = txtReisweg.Text & "E" & Format(i, "00")
        If radFrontseinUit.Checked Then 'frontsein uit
            If chkF1F4.Checked = True Then
                If chkEindeKeren.Checked = True Then
                    txtReisweg.Text = txtReisweg.Text & "M"
                Else
                    txtReisweg.Text = txtReisweg.Text & "F"
                End If
            Else
                If chkEindeKeren.Checked = True Then
                    txtReisweg.Text = txtReisweg.Text & "K"
                Else
                    txtReisweg.Text = txtReisweg.Text & "U"
                End If
            End If
        Else    'frontsein aan
            If chkF1F4.Checked = True Then
                If chkEindeKeren.Checked = True Then
                    txtReisweg.Text = txtReisweg.Text & "N"
                Else
                    txtReisweg.Text = txtReisweg.Text & "G"
                End If
            Else
                If chkEindeKeren.Checked = True Then
                    txtReisweg.Text = txtReisweg.Text & "A"
                Else
                    txtReisweg.Text = txtReisweg.Text & "A"
                End If
            End If
        End If
        'TODO later de v0000 verwijderen 
        txtReisweg.Text = txtReisweg.Text & instellingen.StopAfremSnelheid & "000"
    End Sub

    Private Sub btnFuncties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFuncties.Click
        Dim i As Integer = 0
        If chk_F1.Checked Then i += 1
        If chk_F2.Checked Then i += 2
        If chk_F3.Checked Then i += 4
        If chk_F4.Checked Then i += 8
        If i > 0 Then
            txtReisweg.Text = txtReisweg.Text & "f" & Format(i, "000")  '1 tot 15
        End If
    End Sub

    Private Sub btnFuit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFuit.Click
        txtReisweg.Text = txtReisweg.Text & "f000"
    End Sub

    Private Sub btnHaltTijd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHaltTijd.Click
        Dim tijd As Integer
        If IsNumberOK(txtHaltTijd.Text) Then tijd = (CInt(Val(txtHaltTijd.Text))) Else txtHaltTijd.Focus() : Exit Sub
        Dim vertraging As Integer = CInt(Val(txtVertragingHalt.Text))
        If tijd = 0 And vertraging = 0 Then Exit Sub
        If chkHelpVertraging.Checked = True Then
            'vertragen en halttijd
            txtReisweg.Text = txtReisweg.Text & "j" & Format(vertraging, "0") & Format(tijd, "00")
        Else
            txtReisweg.Text = txtReisweg.Text & "h0" & Format(tijd, "00")
        End If
    End Sub

    Private Sub btnKeerBevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKeerBevel.Click
        If IsNumberOK(txtKeerTijd.Text) Then
            txtReisweg.Text = txtReisweg.Text + "k" + Format(CInt(txtKeerTijd.Text), "000")
        Else
            txtReisweg.Text = txtReisweg.Text + "k005"  'standaard 5 seconden
        End If
    End Sub

    Private Sub btnMarcoNR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarcoNR.Click
        Dim macroCmd As Integer
        If IsNumberOK(txtMacro.Text) Then macroCmd = (CInt(Val(txtMacro.Text))) Else txtMacro.Focus() : Exit Sub
        txtReisweg.Text = txtReisweg.Text & "q" & Format(macroCmd, "000")
    End Sub

    Private Sub btnConditie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConditie.Click
        Dim conditieRWnr As Integer
        If IsNumberOK(txtConditie.Text) Then conditieRWnr = (CInt(Val(txtConditie.Text))) Else txtConditie.Focus() : Exit Sub
        Dim i, j As Integer
        Try
            If conditieRWnr > 0 And conditieRWnr < MAX Then
                Dim rw As String
                SyncLock syncOK2
                    If Not g_ConditieRWArray(conditieRWnr) Is Nothing Then
                        rw = g_ConditieRWArray(conditieRWnr)
                    Else

                        MessageBox.Show("Geen geldige conditiereiswegnummer beschikbaar," & vbNewLine & "Let op : werkt enkel indien START actief is!!")
                        Exit Sub
                    End If
                End SyncLock
                j = rw.IndexOfAny("bB".ToCharArray) + 1
                Dim newBlokNR As String = rw.Substring(j, 3)
                Dim naConditieBlokNR As String = rw.Substring(rw.IndexOfAny("bB".ToCharArray, j), 4)
                j = 0   'reset
                For i = 0 To _max
                    If _tabel(i, 0) = _vanBlokNR AndAlso _tabel(i, 1) = newBlokNR Then
                        _tmpIndex(j) = i   'bewaar index van Bevelen
                        Exit For
                    End If
                Next
                Select Case txtReisweg.Text.Length
                    Case 3
                        txtReisweg.Text = txtReisweg.Text & _tabel(_tmpIndex(0), 3)
                    Case 7, 11, 15, 19
                        txtReisweg.Text = txtReisweg.Text.Substring(0, 3) & _tabel(_tmpIndex(0), 3) & txtReisweg.Text.Substring(3)
                    Case Else
                End Select
                txtReisweg.Text = txtReisweg.Text & "c" & Format(conditieRWnr, "000") & naConditieBlokNR
                lstWisselStraten.Items.Clear()
                _vanBlokNR = naConditieBlokNR.Substring(1)
                txtVanBlokNR.Text = _vanBlokNR
                lstNaarNR.Items.Clear()
                InvullenNaarBlok("")
            End If
        Catch ex As Exception

            'no actions,just exit sub
        End Try
    End Sub

    Private Sub btnPerpetiumMobile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPerpetiumMobile.Click
        Dim perpetiumRWnr As Integer
        If IsNumberOK(txtPerpetiumMobile.Text) Then perpetiumRWnr = (CInt(Val(txtPerpetiumMobile.Text))) Else txtPerpetiumMobile.Focus() : Exit Sub
        If perpetiumRWnr > 9 Then

            MessageBox.Show("Enkel nummers 0 tot en met 9 zijn mogelijk", "Input PerpetiumMobile nummer", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        txtReisweg.Text = txtReisweg.Text & "|p" & Format(perpetiumRWnr, "000")
    End Sub

    Private Sub btnMultiBlok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMultiBlok.Click
        Dim i As Integer
        If IsNumberOK(txtMultiBlok.Text) Then i = (CInt(Val(txtMultiBlok.Text))) Else txtMultiBlok.Focus() : Exit Sub
        If i > 0 And i < 16 Then txtReisweg.Text &= "m" & Format(i, "000") Else txtReisweg.Text &= "m016"
        'If txtReisweg.Text.Length = 3 Then txtReisweg.Text &= "§"
    End Sub

    Private Sub btnFrontseinAAN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFrontseinAAN.Click
        txtReisweg.Text = txtReisweg.Text & "l001"
    End Sub

    Private Sub btnFrontseinUIT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFrontseinUIT.Click
        txtReisweg.Text = txtReisweg.Text & "l000"
    End Sub

    Private Sub lstReiswegen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstReiswegen.Click
        txtChangeNR.Text = CStr(lstReiswegen.SelectedIndex + 1)
    End Sub

    Private Sub lstReiswegen_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstReiswegen.DoubleClick
        If lstReiswegen.SelectedIndex >= 0 Then
            Dim s As String = CStr(lstReiswegen.Items.Item(lstReiswegen.SelectedIndex))
            txtReisweg.Text = s.Substring(s.IndexOf(vbTab) + 1)
            txtChangeNR.Text = (lstReiswegen.SelectedIndex + 1).ToString
            _lstDoubleClick = True
        End If
    End Sub

    Private Sub lstWisselStraten_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstWisselStraten.Click
        If lstWisselStraten.SelectedIndex >= 0 Then
            Select Case txtReisweg.Text.Length
                Case 3
                    txtReisweg.Text = txtReisweg.Text & _tabel(_tmpIndex(0), 3)
                Case 7, 11, 15, 19
                    txtReisweg.Text = txtReisweg.Text.Substring(0, 3) & _tabel(_tmpIndex(0), 3) & txtReisweg.Text.Substring(3)
                Case Else
            End Select
            txtReisweg.Text = txtReisweg.Text & CStr(lstWisselStraten.Items.Item(lstWisselStraten.SelectedIndex)) & "b" & _naarBlokNR
            lstWisselStraten.Items.Clear()
            _vanBlokNR = CStr(lstNaarNR.Items.Item(lstNaarNR.SelectedIndex))
            txtVanBlokNR.Text = _vanBlokNR
            lstNaarNR.Items.Clear()
            InvullenNaarBlok("")
        End If
    End Sub

    Private Sub btnRWophalen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRWophalen.Click
        loadReiswegen()
    End Sub

    Private Sub loadReiswegen()
        If chkCRW.Checked Or ChkTRW.Checked Then  'ophalen ConditieRW gegevens
            Try
                Dim objReader As StreamReader = New StreamReader(DataDir & "\ConditieRW.txt")
                Dim DataRW As String
                Dim i As Integer
                DataRW = objReader.ReadLine
                lstReiswegen.Items.Clear()
                For i = 1 To CInt(DataRW)
                    DataRW = objReader.ReadLine
                    lstReiswegen.Items.Add(DataRW)
                Next
                objReader.Close()
                objReader = Nothing
                radConditieRW.Checked = True
            Catch exc As Exception
                MessageBox.Show("ConditieRW.txt Fout in Bestand: " _
                & exc.Message, "Input - Reiswegen", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else    'ophalen gewone reiswegen
            Try
                Dim objReader As StreamReader = New StreamReader(DataDir & "\Reiswegen.txt")
                Dim DataRW As String
                Dim i As Integer
                DataRW = objReader.ReadLine
                lstReiswegen.Items.Clear()
                For i = 1 To CInt(DataRW)
                    DataRW = objReader.ReadLine
                    lstReiswegen.Items.Add(DataRW)
                Next
                objReader.Close()
                objReader = Nothing
                radGewoneRW.Checked = True
            Catch exc As Exception
                MessageBox.Show("Reiswegen.txt Fout in Bestand: " _
                & exc.Message, "Input - Reiswegen", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnRWBewaren_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRWBewaren.Click
        SaveRW()
    End Sub

    Private Sub SaveRW()
        If chkCRW.Checked Then 'conditieRW.txt
            If lstReiswegen.Items.Count > 0 Then
                Dim i As Integer
                Dim myStreamWriter As StreamWriter
                Try
                    myStreamWriter = File.CreateText(DataDir & "\ConditieRW.txt")
                    myStreamWriter.WriteLine(lstReiswegen.Items.Count.ToString)
                    For i = 0 To lstReiswegen.Items.Count - 1
                        myStreamWriter.WriteLine(lstReiswegen.Items.Item(i))
                    Next
                    myStreamWriter.Close()
                Catch exc As Exception
                    MessageBox.Show("Fout in Bestand: : ConditieRW.txt " & exc.Message)
                End Try
            End If

        Else    'gewone reiswegen
            If lstReiswegen.Items.Count > 0 Then
                Dim i As Integer
                Dim myStreamWriter As StreamWriter
                Try
                    myStreamWriter = File.CreateText(DataDir & "\Reiswegen.txt")
                    myStreamWriter.WriteLine(lstReiswegen.Items.Count.ToString)
                    For i = 0 To lstReiswegen.Items.Count - 1
                        myStreamWriter.WriteLine(lstReiswegen.Items.Item(i))
                    Next
                    myStreamWriter.Close()
                Catch exc As Exception
                    MessageBox.Show("Fout in Bestand: : Reiswegen.txt " & _
                    exc.Message, "Input - Reiswegen", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If

        End If
    End Sub

    Private Sub btnRWToevoegen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRWToevoegen.Click
        If txtChangeNR.Text <> String.Empty AndAlso CInt(txtChangeNR.Text) > lstReiswegen.Items.Count Then Exit Sub
        Dim ok As Boolean = False
        Dim i, j, k, l, t As Integer
        If chkCRW.Checked = False And ChkTRW.Checked = False Then
            'kontrole van de gevormde txtReisweg.txt
            i = txtReisweg.Text.Length
            j = txtReisweg.Text.LastIndexOfAny("Ee".ToCharArray)
            k = txtReisweg.Text.LastIndexOfAny("Bb".ToCharArray)
            t = txtReisweg.Text.LastIndexOfAny("Tt".ToCharArray)
            If j - t = 4 Then
                If j > 0 Then ok = True
            Else
                l = i - k
                If j > 0 AndAlso k > 0 AndAlso j - k = 4 AndAlso l = 12 Then ok = True
            End If
            If ok Then
                If lstReiswegen.Items.Count > 0 Then
                    If txtChangeNR.Text <> "" Then
                        i = CInt(Val(txtChangeNR.Text) - 1)
                        If i > lstReiswegen.Items.Count Then
                            lstReiswegen.Items.Add((lstReiswegen.Items.Count + 1).ToString & vbTab & txtReisweg.Text)
                            txtRPReisweg.Text = (lstReiswegen.Items.Count).ToString
                        ElseIf i >= 0 AndAlso i <= lstReiswegen.Items.Count Then
                            lstReiswegen.Items.RemoveAt(i)
                            'lstReiswegen.Items.Insert(i, (i + 1).ToString & vbTab & txtReisweg.Text)       ' ==== 12345   ========
                            lstReiswegen.Items.Insert(i, txtReisweg.Text)
                            txtRPReisweg.Text = Format(i + 1, "000")
                        End If
                    Else
                        lstReiswegen.Items.Add(txtReisweg.Text)
                        'lstReiswegen.Items.Add((lstReiswegen.Items.Count + 1).ToString & vbTab & txtReisweg.Text) '=== 12345 ===
                        txtRPReisweg.Text = (lstReiswegen.Items.Count).ToString
                    End If
                End If
            Else

                MessageBox.Show("Reisweg NIET in orde !", "Input - Reiswegen, Commandolijn controle", _
                MessageBoxButtons.OK, MessageBoxIcon.Question)
            End If
        Else
            If txtChangeNR.Text <> "" Then
                i = CInt(Val(txtChangeNR.Text) - 1)
                If i > lstReiswegen.Items.Count Then
                    'lstReiswegen.Items.Add((lstReiswegen.Items.Count + 1).ToString & vbTab & txtReisweg.Text)    '===  12345 ======
                    lstReiswegen.Items.Add(txtReisweg.Text)
                    txtRPReisweg.Text = (lstReiswegen.Items.Count).ToString
                ElseIf i >= 0 AndAlso i <= lstReiswegen.Items.Count Then
                    lstReiswegen.Items.RemoveAt(i)
                    lstReiswegen.Items.Insert(i, txtReisweg.Text)
                    txtRPReisweg.Text = Format(i + 1, "000")
                End If
            Else
                'lstReiswegen.Items.Add((lstReiswegen.Items.Count + 1).ToString & vbTab & txtReisweg.Text)   ' ==  12345  =========
                lstReiswegen.Items.Add(txtReisweg.Text)
                txtRPReisweg.Text = (lstReiswegen.Items.Count).ToString
            End If
        End If
    End Sub

    Private Sub btnRWUitvoeren_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRWUitvoeren.Click
        Dim ok As Boolean = False
        Dim s As String = String.Empty
        Dim i, j, k, l, m, t, index As Integer
        i = txtReisweg.Text.IndexOf("m")
        If i > 0 Then    'controle aantal te verwerken multiblokken
            l = i
            m = CInt(txtReisweg.Text.Substring(i + 1, 3))
            If m >= 2 Then
                s = txtReisweg.Text.Substring(i, 4)
                t = 0
                'vanaf deze plaats moeten er opvolgend 'm' "b"'s volgen vóór er een E komt
                Do
                    k = txtReisweg.Text.IndexOf("b", i)
                    If k < 0 Then Exit Do
                    i = k + 1
                    t += 1
                Loop
                t -= 1   'aantal toegelaten b's (de Endblok b weglaten)
                If t < m Then
                    k = MessageBox.Show("De waarde van de Multi-Blok is te groot, " _
                    & vbNewLine & "Wordt aangepast tot Multi-Blok= " & t.ToString _
                    & vbNewLine & "[en indien = 0, dan wordt de Multi-Blok instruktie verwijderd]" _
                    & vbNewLine & vbNewLine & "OK=        uitvoeren reisweg" _
                    & vbNewLine & "Cancel= herbegin de opmaak van de reisweg", _
                    "Input Reisweg, Multiblok ingave correctie", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                    If k = 2 Then
                        Exit Sub
                    Else
                        If t <= 0 Then
                            txtReisweg.Text = txtReisweg.Text.Remove(l, 4)
                        Else
                            txtReisweg.Text = txtReisweg.Text.Replace(s, "m" & Format(t, "000"))
                        End If
                    End If
                End If
            Else
                'verwijder mxxx
                txtReisweg.Text = txtReisweg.Text.Remove(l, 4)

            End If
        End If
        Application.DoEvents()
        i = txtReisweg.Text.Length
        j = txtReisweg.Text.LastIndexOfAny("Ee".ToCharArray)
        k = txtReisweg.Text.LastIndexOfAny("Bb".ToCharArray)
        t = txtReisweg.Text.LastIndexOfAny("Tt".ToCharArray)
        If j - t = 4 Then
            If j > 0 Then ok = True
        Else
            l = i - k
            If j > 0 AndAlso k > 0 AndAlso j - k = 4 AndAlso l = 12 Then ok = True
        End If
        If ok Then
            If IsNumberOK(txtNowLocoNR.Text) Then
                i = CInt(Val(txtNowLocoNR.Text))
            Else
                MessageBox.Show("Ongeldig locoNR")
                txtNowLocoNR.Focus()
                txtNowLocoNR.SelectAll()
                Exit Sub
            End If
            If i > 0 And i < 81 Then
                If _lstDoubleClick Then
                    Do
                        _startBlokNR = "000" & InputBox("Geef het Startblok nummer in ")
                        index = _startBlokNR.IndexOfAny("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray, 1)
                    Loop Until index < 0
                    _startBlokNR = _startBlokNR.Substring(_startBlokNR.Length - 3)
                End If
                g_TransitArray(0) = txtReisweg.Text
                g_TransitArray(8) = i.ToString
                g_TransitArray(9) = _startBlokNR & "+00h000p000000"
                'controle of blokNR(locoNR) combinatie bestaat
                s = g_TransitArray(9).Substring(0, 3) + " " + Format(CInt(g_TransitArray(8)), "000")    'formaat in g_bloklocoArray(i)
                ok = False
                For i = 1 To g_BlokLocoArray.GetUpperBound(0) - 1
                    If g_BlokLocoArray(i).IndexOf(s) = 0 Then
                        ok = True
                        Exit For
                    End If
                Next
                If Not ok Then
                    MessageBox.Show("Combinatie BlokNR LocoNR " + s + " is ongeldig " _
                        & vbNewLine & "BlokNR= " & _startBlokNR.ToString & vbNewLine _
                        & "LocoNR= " & g_TransitArray(8) _
                        & vbNewLine & " Bekijk Blokken bezet in 'Besturing', zie na of er geen bloknummer met loconummer =000 staat." _
                        & vbNewLine & "Zoja, neem de loco in dienst via 'Onderhoud'.", "WallarCMD ", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
                g_ReisWegenArray(0) = g_TransitArray(0)
            End If
            txtVanBlokNR.Focus()
            txtVanBlokNR.SelectAll()
            Directstart()
            Me.Close()
        Else

            MessageBox.Show("Reisweg NIET in orde !", "Input - Reiswegen, Commandolijn controle", _
            MessageBoxButtons.OK, MessageBoxIcon.Question)
        End If
    End Sub

    Private Sub btnTerminus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTerminus.Click
        Dim terminusRWnr As Integer
        Dim vanNR As String = txtVanBlokNR.Text
        If IsNumberOK(txtTerminus.Text) Then terminusRWnr = (CInt(Val(txtTerminus.Text))) Else txtTerminus.Focus() : Exit Sub
        Dim i, j As Integer
        If terminusRWnr > 0 And terminusRWnr < MAX Then
            Dim rw As String
            If Not g_ConditieRWArray(terminusRWnr) Is Nothing Then
                rw = g_ConditieRWArray(terminusRWnr)
            Else

                MessageBox.Show("Geen geldige terminusreiswegnummer beschikbaar," & vbNewLine & "Let op : werkt enkel indien START actief is!!")
                Exit Sub
            End If
            j = rw.IndexOfAny("bB".ToCharArray) + 1
            Dim newBlokNR As String = rw.Substring(j, 3)
            j = 0   'reset
            For i = 0 To _max
                If _tabel(i, 0) = _vanBlokNR AndAlso _tabel(i, 1) = newBlokNR Then
                    _tmpIndex(j) = i   'bewaar index van Bevelen
                    Exit For
                End If
            Next
            Select Case txtReisweg.Text.Length
                Case 3
                    txtReisweg.Text = txtReisweg.Text & _tabel(_tmpIndex(0), 3)
                Case 7, 11, 15, 19
                    txtReisweg.Text = txtReisweg.Text.Substring(0, 3) & _tabel(_tmpIndex(0), 3) & txtReisweg.Text.Substring(3)
                Case Else
            End Select
            txtReisweg.Text = txtReisweg.Text & "t" & Format(terminusRWnr, "000")
            lstWisselStraten.Items.Clear()
            txtVanBlokNR.Text = vanNR
            lstNaarNR.Items.Clear()
            InvullenNaarBlok("")
        End If

    End Sub

    Private Sub radF1F4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles radF1F4.Click
        chkFrontseinAan.Visible = False
        chkKeren.Visible = False
        grpF1F4.Visible = True
    End Sub

    Private Sub radKerenFrontsein_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radKerenFrontsein.CheckedChanged
        chkFrontseinAan.Visible = True
        chkKeren.Visible = True
        grpF1F4.Visible = False
    End Sub

    Private Sub chkCRW_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCRW.CheckedChanged
        If chkCRW.Checked Then ChkTRW.Checked = False
        loadReiswegen()
    End Sub

    Private Sub ChkTRW_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkTRW.CheckedChanged
        If ChkTRW.Checked Then chkCRW.Checked = False
        loadReiswegen()
    End Sub

    Private Sub btnOpkuisRW_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpkuisRW.Click
        Dim i As Integer
        If txtReisweg.Text.Length > 0 Then
            i = MessageBox.Show("Reisweg veld opkuisen?", "Input", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If i <> 6 Then
                Exit Sub
            Else
                txtReisweg.Text = ""
                btnStart.Enabled = True
            End If
        End If
    End Sub
#End Region

#Region "Reisplan"

    Private Sub btnRPvoegtoe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRPvoegtoe.Click
        Dim i As Integer
        Dim s As String = ""
        i = CInt(Val(txtStartBlokNR.Text))
        If i > 0 And i < MAX Then
            s = Format(i, "000")
        Else
            MessageBox.Show("Ongeldig Startbloknummer, corrigeer", "Input - Reisplan" _
            , MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtStartBlokNR.Focus()
            txtStartBlokNR.SelectAll()
            Exit Sub
        End If
        i = CInt(Val(txtRPDeltaSnelheid.Text))
        Select Case i
            Case -9 To -1
                s += Format(i, "00")
            Case 1 To 9
                s += Format(i * -1, "00")
            Case Else
                s += "+00"
        End Select
        If chkHaltPassagier.Checked Then
            i = CInt(Val(txtHaltPassagier.Text))
            s += "j" & Format(i, "000")
        Else
            s += "h000"
        End If
        If txtPlanNRnextIB.Text.Length > 0 Then
            i = CInt(txtPlanNRnextIB.Text)
            s += "p" & Format(i, "000")
        Else
            s += "p000"
        End If
        i = CInt(Val(txtRPReisweg.Text))
        If i > 0 And i < MAX Then
            s += Format(i, "000")
            's += vbTab & txtBeschrijvingRP.Text
            s = txtBeschrijvingRP.Text & vbTab & s
        Else
            MessageBox.Show("Ongeldig Reiswegnummer, corrigeer", _
            "Input - Reisplan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtRPReisweg.Focus()
            txtRPReisweg.SelectAll()
            Exit Sub
        End If
        txtReisplan.Text = s
        btnRPvoegtoe.Enabled = False
        txtRPReisweg.Focus()
    End Sub

    Private Sub btnExtraRWtoevoegen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtraRWtoevoegen.Click
        Dim i As Integer
        Dim s As String = ""
        i = CInt(Val(txtRPReisweg.Text))
        If i > 0 And i < MAX Then
            s += Format(i, "000")
            'txtReisplan.Text = txtReisplan.Text.Substring(0, txtReisplan.Text.IndexOf(vbTab)) & s & vbTab & txtBeschrijvingRP.Text
            txtReisplan.Text &= s
        Else
            MessageBox.Show("Ongeldig Reiswegnummer, corrigeer", _
            "Input - Reisplan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtRPReisweg.Focus()
            txtRPReisweg.SelectAll()
            Exit Sub
        End If
    End Sub

    Private Sub btnPerpMobileActivate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPerpMobileActivate.Click
        Dim s As String = ""
        s += "ppp"
        txtReisplan.Text &= s
    End Sub

    Private Sub btnTussenhalt_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTussenhalt.Click
        Dim s As String = "h"
        Dim h As Integer
        If IsNumberOK(txtTussenhalt.Text) Then
            h = CInt(txtTussenhalt.Text)
            s += Format(h, "000")
            txtReisplan.Text &= s
        Else
            txtTussenhalt.Focus() : Exit Sub
        End If
    End Sub

    Private Sub btnResetRP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetRP.Click
        txtReisplan.Clear()
        btnRPvoegtoe.Enabled = True
        txtStartBlokNR.Focus()
        txtStartBlokNR.SelectAll()
    End Sub

    Private Sub btnRPToevoegen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRPToevoegen.Click
        If txtNR.Text <> String.Empty AndAlso CInt(txtNR.Text) > lstReisplannen.Items.Count Then Exit Sub
        Dim i As Integer
        If lstReisplannen.Items.Count > 0 Then
            i = CInt(Val(txtNR.Text)) - 1
            If i >= 0 Then
                If i <= lstReisplannen.Items.Count Then
                    lstReisplannen.Items.RemoveAt(i)
                    lstReisplannen.Items.Insert(i, txtBeschrijvingRP.Text & vbTab & txtReisplan.Text)
                End If
            Else
                lstReisplannen.Items.Add((lstReisplannen.Items.Count + 1).ToString & vbTab & txtReisplan.Text)
            End If
        End If
    End Sub

    Private Sub btnRPophalen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRPophalen.Click
        LoadReisplannen()
    End Sub

    Private Sub LoadReisplannen()
        Try
            Dim objReader As StreamReader = New StreamReader(DataDir & "\Reisplannen.txt")
            Dim DataRP As String
            Dim i As Integer
            DataRP = objReader.ReadLine
            lstReisplannen.Items.Clear()
            For i = 1 To CInt(DataRP)
                DataRP = objReader.ReadLine
                lstReisplannen.Items.Add(DataRP)
            Next
            objReader.Close()
            objReader = Nothing
        Catch exc As Exception
            MessageBox.Show("Reisplannen.txt Fout in Bestand: " & exc.Message, _
            "Input - Reisplan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnRPBewaren_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRPBewaren.Click
        SaveRP()
    End Sub

    Private Sub SaveRP()
        If lstReisplannen.Items.Count > 0 Then
            Dim i As Integer
            Dim myStreamWriter As StreamWriter
            Try
                myStreamWriter = File.CreateText(DataDir & "\Reisplannen.txt")
                myStreamWriter.WriteLine(lstReisplannen.Items.Count.ToString)
                For i = 0 To lstReisplannen.Items.Count - 1
                    myStreamWriter.WriteLine(lstReisplannen.Items.Item(i))
                Next
                myStreamWriter.Close()
            Catch exc As Exception
                MessageBox.Show("Fout in Bestand: : Reisplannen.txt " & exc.Message, _
                "Input - Reisplan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub lstReisplannen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstReisplannen.Click
        txtNR.Text = (lstReisplannen.SelectedIndex + 1).ToString
    End Sub

    Private Sub lstReisplannen_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstReisplannen.DoubleClick
        If lstReisplannen.SelectedIndex >= 0 Then
            Dim s As String = CStr(lstReisplannen.Items.Item(lstReisplannen.SelectedIndex))
            txtReisplan.Text = s.Substring(s.IndexOf(vbTab) + 1)
            txtNR.Text = (lstReisplannen.SelectedIndex + 1).ToString
        End If
    End Sub

#End Region

#Region "MacroInterpreter"
    Private Sub SaveMacro()
        Dim s As String = g_macroArray(0) + vbNewLine + MACRODELIMITER + vbNewLine
        Dim i As Integer
        For i = 1 To CInt(g_macroArray(0))
            s += g_macroArray(i) + vbNewLine + MACRODELIMITER + vbNewLine
        Next
        Dim mystreamwriter As StreamWriter
        mystreamwriter = File.CreateText(DataDir & "\MacroData.txt")
        mystreamwriter.Write(s)
        mystreamwriter.Close()
    End Sub

    Private Sub lstKeywords_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstKeywords.SelectedIndexChanged
        If chkLijnNRSToevoegen.Checked Then
            txtInputMacro.Text &= _lineNumber.ToString + vbTab + CStr(lstKeywords.SelectedItem)
        Else
            txtInputMacro.Text &= vbTab + CStr(lstKeywords.SelectedItem)
        End If
        txtInputMacro.Focus()
        txtInputMacro.Select(txtInputMacro.TextLength, 0)
        _lineNumber += 1
    End Sub

    Private Sub btnLoadMacro_Click_1(sender As System.Object, e As System.EventArgs) Handles btnLoadMacro.Click
        Dim nr As Integer
        If IsNumberOK(txtMacroNR.Text) Then
            nr = CInt(txtMacroNR.Text)
            If nr > 0 And nr <= CInt(g_macroArray(0)) Then              'geldigheid macroNR
                txtInputMacro.Text = g_macroArray(nr)                   'plaats in editeer tekstvenster
                txtInputMacro.Enabled = True
                lstKeywords.Enabled = True
                btnResetLijnNR.Enabled = True
                btnSaveMacro.Enabled = True
            Else
                MessageBox.Show("Ongeldig macronummer", "Macro Editor")
            End If
        Else
            MessageBox.Show("Ongeldig macronummer", "Macro Editor")
        End If
    End Sub

    Private Sub btnSaveMacro_Click_1(sender As System.Object, e As System.EventArgs) Handles btnSaveMacro.Click
        If txtInputMacro.Text.Length > 0 Then
            g_macroArray(CInt(txtMacroNR.Text)) = txtInputMacro.Text

            btnBewarenInArray.Enabled = False
            btnLoadMacro.Enabled = True
            btnSaveMacro.Enabled = False
            chkLijnNRSToevoegen.Enabled = False
            chkProgrammalijnenWissen.Enabled = False
            txtInputMacro.Enabled = False
            lstKeywords.Enabled = False
            btnResetLijnNR.Enabled = False

        End If
    End Sub

    Private Sub btnAddMacro_Click_1(sender As System.Object, e As System.EventArgs) Handles btnAddMacro.Click
        Dim nr As Integer = CInt(g_macroArray(0)) + 1                   'nieuw macroNR
        lblAantalMacros.Text = "Nieuwe macroNR = " + nr.ToString
        If chkProgrammalijnenWissen.Checked = True Then
            txtInputMacro.Text = "0" + vbTab + "titel" + vbTab
            txtInputMacro.Focus()
            txtInputMacro.Select(txtInputMacro.TextLength, 0)
            _lineNumber = 1
            btnAddMacro.Enabled = False
            btnBewarenInArray.Enabled = True
            btnLoadMacro.Enabled = False
            btnSaveMacro.Enabled = False
            chkLijnNRSToevoegen.Enabled = True
            chkProgrammalijnenWissen.Enabled = True
            txtInputMacro.Enabled = True
            lstKeywords.Enabled = True
            btnResetLijnNR.Enabled = True
            txtInputMacro.Focus()
            txtInputMacro.Select(txtInputMacro.TextLength, 0)
            _lineNumber = 1

        End If
    End Sub

    Private Sub btnMacroHelp_Click_1(sender As System.Object, e As System.EventArgs) Handles btnMacroHelp.Click
        Me.helpMacrofrm = New Help
        Me.helpMacrofrm.txtHelp.Text = macroHelptekst
        Me.helpMacrofrm.Show()
    End Sub

    Private Sub btnResetLijnNR_Click(sender As System.Object, e As System.EventArgs) Handles btnResetLijnNR.Click
        _lineNumber = CInt(txtStartLijnNR.Text)   'reset
    End Sub

    Private Sub btnBewarenInArray_Click(sender As System.Object, e As System.EventArgs) Handles btnBewarenInArray.Click
        If txtInputMacro.Text.Length > 0 Then
            Dim macroNR As Integer = CInt(g_macroArray(0)) + 1
            txtMacroNR.Text = macroNR.ToString
            g_macroArray(0) = macroNR.ToString
            g_macroArray(macroNR) = txtInputMacro.Text

            btnAddMacro.Enabled = True
            btnBewarenInArray.Enabled = False
            btnLoadMacro.Enabled = True
            btnSaveMacro.Enabled = False
            chkLijnNRSToevoegen.Enabled = False
            chkProgrammalijnenWissen.Enabled = False
            txtInputMacro.Enabled = False
            lstKeywords.Enabled = False
            btnResetLijnNR.Enabled = False

        End If
    End Sub

#End Region

End Class

