'DicoStation program to use with the LFT Dicostation unit and Modellplan Digital-S-Inside interface.
'Programming in Visual Basic 2010 Express
'(c) Wallar 2011 walter@larno.be 
'20111104
'registerNB VB: 2KQT8-HV27P-GTTV9-2WBVV-M7X96

Imports System
Imports System.Threading
Imports System.Threading.Thread
Imports System.Diagnostics
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.Devices

' Hier is iets gewijzigd; bla bla
Public Class MDIParentfrm

#Region "Settings and declarations"

    ' constansts
    Private Const MINCLIENTSIZE = 75

    'class variables
    Private childFormNumber As Integer
    Private displayText As String
    Private threadDCS As Thread
    Private trackInfo As Boolean = False
    Private showMainDisplay As Boolean = False
    Private dataD As DecoderM
    Public basis As Integer = 0

    'delegates
    Private Delegate Sub handler_DelegateString(ByVal s As String)  'used for string events
    Private Delegate Sub handler_DelegateStringIntegerInteger(ByVal s As String, ByVal locoNR As Integer, ByVal locoNR As Integer)
    Private Delegate Sub handler_DelegateInteger(ByVal n As Integer)  'used for number events
    Private Delegate Sub handler_DelegateStringStringString(ByVal status As String, ByVal direction As String, ByVal address As String)
    Private Delegate Sub handler_DelegateBoolean(ByVal ok As Boolean)
    Private Delegate Sub handler_DelegateBitArray(ByVal bitsArray As BitArray)

    'Childforms
    Private commandoChildfrm As CommandoChildfrm
    Private DisplayS88Contacten As s88ContactsFrm
    Private DisplayInfo As DisplayInfoChildfrm
    Private EditorChldfrm As Editor
    Private Ingave As Input
    Private ReisPlan As ReisplanFrm
    Private HelpProgramma As Help
    Private NieuweLocoIndienst As NieuweLocoIndienstFrm

#End Region

#Region "Loading and closing subs and functions"

    Private Sub MDIParentfrm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If MessageBox.Show("Wil je het programma verlaten?" + vbNewLine + vbNewLine + "(Werkt enkel ogenblikkelijk indien LDT-Watchdogmodule in dienst is!)", "My Application", MessageBoxButtons.YesNo) = DialogResult.No Then
            ' Cancel the Closing event from closing the form.
            e.Cancel = True
            LoadMainSynopFrm()
        Else
            s88contactsView = False
            DCS.SETPowerOnOff(True)
            Sleep(200)
            StopModelRailway()
        End If
    End Sub

    Private Sub MDIParentfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '1.  Loading data and initialise settings
        InstellingenData()
        LoadModelRailwayData()
        InitializeHandlers()

        '2. MainSyopFrm loaded by startup
        LoadMainSynopFrm()
        '3. start dicostation class via ThreadEntry
        StartModelRailway()

        '4.  show the user interface (MDIParentfrm)
        Do
            Sleep(10)
            Application.DoEvents()
        Loop Until showMainDisplay
        Me.WindowState = FormWindowState.Maximized

    End Sub

    Private Sub LoadMainSynopFrm()
        'zelfde als in sub generalMenu_Click (dynamisch aanmaken van een synoptiek Menu)
        Dim i As Integer = 0        'temp. index van de synop() en synopGroupActive() arrays
        Dim j As Integer = 0        'temp. index van de synopGroup().prop array waar alle synoptiek gegevens in bewaard worden
        synopGroupActive(i) = True
        synop(i) = New MainSynopFrm
        synop(i).MdiParent = Me
        Select Case synopGroup(j).Prop.appearance
            Case CanvasAppearances.Fitscreen
                synop(i).Width = Me.ClientSize.Width - 4
                synop(i).Height = Me.ClientSize.Height - MINCLIENTSIZE

                synop(i).StartPosition = FormStartPosition.Manual
            Case CanvasAppearances.Standaard
                synop(i).StartPosition = FormStartPosition.Manual
            Case CanvasAppearances.Maximized
                synop(i).WindowState = FormWindowState.Maximized
        End Select
        synopMenuItemInUse(i) = True   ' boolean wordt "True" gezet om aan te duiden dat deze synoptiek geklikt werd
        synop(i).Text = synopGroup(j).Prop.tittle
        synop(i).tsCanvasInfo.Text = synopGroup(j).Prop.labelText
        synop(i).Show()         'start mainSynopFrm op
    End Sub

    Private Sub InstellingenData()
        'ProgramSetting data from file ProgramSetting.txt C:\Users\Walter\Documents\WallarDCS
        Dim myStreamReader As StreamReader
        Try
            myStreamReader = File.OpenText(DataDir & "\instellingen.txt")
            instellingen.LeaveHSI88USB = myStreamReader.ReadLine
            instellingen.s88modulesLinks = myStreamReader.ReadLine
            instellingen.s88modulesMidden = myStreamReader.ReadLine
            instellingen.s88modulesRechts = myStreamReader.ReadLine
            instellingen.startVertraging = myStreamReader.ReadLine
            instellingen.GoederentreinSnelheidsverlaging = myStreamReader.ReadLine
            instellingen.StopAfremSnelheid = myStreamReader.ReadLine
            instellingen.MacroStart = myStreamReader.ReadLine
            instellingen.MacroStop = myStreamReader.ReadLine
            instellingen.MacroFout = myStreamReader.ReadLine
            instellingen.HaltInStation = myStreamReader.ReadLine
            instellingen.OHblokken = myStreamReader.ReadLine
            instellingen.K8055 = myStreamReader.ReadLine
            instellingen.k8055B0 = myStreamReader.ReadLine
            instellingen.k8055B1 = myStreamReader.ReadLine
            instellingen.k8055B2 = myStreamReader.ReadLine
            instellingen.k8055B3 = myStreamReader.ReadLine
            instellingen.SchaduwUitDienstTijd = myStreamReader.ReadLine
            instellingen.turnoutPulsTime = myStreamReader.ReadLine
            instellingen.tempoRegeling = myStreamReader.ReadLine
            instellingen.PenWidth = myStreamReader.ReadLine
            instellingen.ColorVrij = myStreamReader.ReadLine
            instellingen.ColorBezet = myStreamReader.ReadLine
            instellingen.ColorAfremBezet = myStreamReader.ReadLine
            instellingen.ColorGereserveerd = myStreamReader.ReadLine
            instellingen.ColorPreReserved = myStreamReader.ReadLine
            instellingen.ColorVergrendeld = myStreamReader.ReadLine
            instellingen.ColorOnderhoud = myStreamReader.ReadLine
            instellingen.ColorSpook = myStreamReader.ReadLine
            instellingen.colorBlockPen = myStreamReader.ReadLine
            instellingen.colorHalt = myStreamReader.ReadLine
            instellingen.colorPassief = myStreamReader.ReadLine
            instellingen.ColorInit = myStreamReader.ReadLine
            instellingen.ColorInitBlok = myStreamReader.ReadLine
            instellingen.ColorGridLijnen = myStreamReader.ReadLine
            instellingen.ColorAchtergrondSynop = myStreamReader.ReadLine
            instellingen.ColorAchtergrondElement = myStreamReader.ReadLine
            instellingen.ColorHandbediening = myStreamReader.ReadLine
            instellingen.ColorRetrigger = myStreamReader.ReadLine
            instellingen.ColorTextBack = myStreamReader.ReadLine
            instellingen.ColorLocatieTrein = myStreamReader.ReadLine
            instellingen.StatusColorTurnout = myStreamReader.ReadLine
            instellingen.s88ColonRange = myStreamReader.ReadLine
            instellingen.gridlijnenTonen = CBool(myStreamReader.ReadLine)
            instellingen.showRailElements = CBool(myStreamReader.ReadLine)
            instellingen.K83K84detectie = CBool(myStreamReader.ReadLine)
            instellingen.uitvoeringsmode = CBool(myStreamReader.ReadLine)
            myStreamReader.Close()
            'generale programma waarden
            'setCanvasValues
            canvasValue.setGridUnit = 2 'fixed
            canvasValue.setGridText = 8 'fixed
            canvasValue.setPenWidth = CInt(instellingen.PenWidth)

        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: Instellingen.txt " & exc.Message _
            & vbNewLine & vbNewLine & "Zie alle bestanden na in: " & DataDir, "WallarDCS Opstart", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
            Exit Sub
        End Try
    End Sub

    Private Sub LoadModelRailwayData()
        mdiParentStatus.Text = "Opladen van alle gegevens tekst bestanden van My Documents\WallarDCS"
        LoadBevelen()
        LoadRelaties()
        LoadLocoData()
        LoadMacro()
        LoadK83K84()
        LoadBlokLocoArray()
        LoadReiswegenConditiereiswegen()
        LoadReisplannen()
        LoadS88MacroNRs()
        LoadSynoptics()
    End Sub

    Private Sub InitializeHandlers()
        'event handler for receiving information from central dicostation.vb module
        AddHandler DCS.handlerStatus, AddressOf HandlerStatus
        AddHandler DCS.handlerHalt, AddressOf handlerHalt
        AddHandler DCS.handlerLogData, AddressOf HandlerLogData
        AddHandler DCS.handlerChangeBlok, AddressOf handlerChangeBlok
        AddHandler DCS.handlerOK, AddressOf handlerInteger
        AddHandler DCS.handlerChangeTurnout, AddressOf handlerChangeTurnout
        AddHandler DCS.handlerThreadRunning, AddressOf handlerThreadRunning
        AddHandler DCS.handlerBitArray, AddressOf handlerBitArray
    End Sub

    Private Sub SaveModelrailwayData()
        StatusBarToolStripMenuItem.Text = "Wegschrijven van alle gegevens in Documents\WallarDCS"
        SaveBevelen()
        SaveRelaties()
        SaveLocoData()
        SaveS88MacroNRs()
        SaveBlokLocoArray()
    End Sub

    Private Sub StartModelRailway()
        'Always execute  these instructions
        threadDCS = New Thread(AddressOf DCS.ThreadEntry)
        threadDCS.IsBackground = True
        threadDCS.Priority = ThreadPriority.Highest
        threadDCS.Name = "DiCoStation thread"
        threadDCS.Start()
    End Sub

    Private Sub StopModelRailway()
        'end instance of DiCoStation module (sub threadEntry)
        DCS.inRunMode = False  'escape out of sub ThreadEntry
        threadDCS.Join()        'wait until DicoStation thread ends
        DCS = Nothing           'release resources
        'save ModelRailwayData
        SaveModelrailwayData()
    End Sub

    Private Sub LoadSynoptics()
        Try
            Dim i, d1, d2 As Integer   'indexen van de delimiters
            Dim maxElements As Integer = 0
            Dim d As String = "|"   'scheidingsteken tussen de gegevens in de string
            Dim dc As String = "#"  'hierna begint het canvas gedeelte
            Dim s As String = String.Empty
            Dim data As String = String.Empty
            Dim strData As String = String.Empty
            Dim uBoundStrSynopGroup As Integer = 0

            Dim mystreamreader As StreamReader
            mystreamreader = File.OpenText(DataDir & "\synoptics.txt")

            '1. add data to strSynopGroup()
            data = mystreamreader.ReadLine : uBoundStrSynopGroup = CInt(data)  'eerste lijn bevat het aantal synoptieken

            ReDim strSynopGroup(uBoundStrSynopGroup)
            strSynopGroup(0) = data     'bewaar hier ook het aantal synoptieken in de eerste lijn
            For i = 1 To uBoundStrSynopGroup   'voor alle synoptiek lijnen
                data = mystreamreader.ReadLine : strSynopGroup(i) = data
            Next
            mystreamreader.Close()

            '2. converteer de gegevens in strSynopGroup() naar de Array synopGroup()
            uBoundSynopGroup = uBoundStrSynopGroup - 1   'één minder, omdat het aantal de eerste record van strSynopGroup() geen synoptiek bevat
            ReDim synopGroup(uBoundSynopGroup)
            ReDim synop(uBoundSynopGroup)
            ReDim synopGroupActive(uBoundSynopGroup)
            ReDim synopMenuItemInUse(uBoundSynopGroup)
            ReDim synopIndexStr(uBoundSynopGroup)
            SynopticsToolStripMenuItem.DropDownItems.Clear()

            For i = 0 To uBoundSynopGroup    'voor alle synoptieken
                data = strSynopGroup(i + 1)         ' 1=eerste synop record

                'a) property gedeelte (.prop)
                synopGroup(i).Prop.appearance = CType(data.Substring(0, 1), CanvasAppearances)
                d1 = data.IndexOf(d, 0) + 1 : d2 = data.IndexOf(d, d1) : synopGroup(i).Prop.gridStep = CInt(data.Substring(d1, d2 - d1))
                d1 = d2 + 1 : d2 = data.IndexOf(d, d1) : synopGroup(i).Prop.tittle = data.Substring(d1, d2 - d1)
                d1 = d2 + 1 : d2 = data.IndexOf(d, d1) : synopGroup(i).Prop.labelText = data.Substring(d1, d2 - d1)

                'b) make new menuItem for "synoptieken" item under headmenu "Synoptieken"
                Dim NewMenu As New ToolStripMenuItem(synopGroup(i).Prop.tittle, Nothing, New EventHandler(AddressOf GeneralMenu_Click))
                SynopticsToolStripMenuItem.DropDownItems.Add(NewMenu)
                synopGroup(i).Prop.synopNR = i

                'c) canvas gedeelte (.canvas)
                data = data.Substring(data.IndexOf(dc) + 1) 'canvasgedeelte begint na delimiter dc="#"

                'bepaal aantal synoptiekelementen via aantal d delimiters (er zijn 8 waarden per element)
                Dim ndelim As Integer = 0   'delimiters count
                Dim z As Integer = 0
                For z = 0 To data.Length - 1
                    If data.Substring(z, 1) = d Then ndelim += 1 'tel de delimetes
                Next

                maxElements = (ndelim \ 9) - 1   ' index met 1 verminderen   (Let op: nu zijn er 9 items per element, zie hieronder  ' TODO )
                ReDim synopGroup(i).canvas(maxElements)

                For k As Integer = 0 To maxElements        'voor alle elements van deze synoptiek
                    d1 = 0 : d2 = data.IndexOf(d, d1)
                    synopGroup(i).canvas(k).posX = CInt(data.Substring(d1, d2 - d1))               'x
                    synopIndexStr(i) &= dc & data.Substring(d1, d2 - d1)
                    d1 = d2 + 1 : d2 = data.IndexOf(d, d1)
                    synopGroup(i).canvas(k).posY = CInt(data.Substring(d1, d2 - d1))               'y
                    synopIndexStr(i) &= "." & data.Substring(d1, d2 - d1) & d & k.ToString & d
                    d1 = d2 + 1 : d2 = data.IndexOf(d, d1)
                    synopGroup(i).canvas(k).elementNR = CInt(data.Substring(d1, d2 - d1))           'elementNR
                    d1 = d2 + 1 : d2 = data.IndexOf(d, d1)
                    synopGroup(i).canvas(k).elementLenght = CInt(data.Substring(d1, d2 - d1))       'elementLenght
                    d1 = d2 + 1 : d2 = data.IndexOf(d, d1)
                    synopGroup(i).canvas(k).itemNR = data.Substring(d1, d2 - d1)                    'itemNR: blok-, wissel-, magn.artikel- of tekstelement
                    d1 = d2 + 1 : d2 = data.IndexOf(d, d1)
                    synopGroup(i).canvas(k).strInfo = data.Substring(d1, d2 - d1)                   'strInfo
                    d1 = d2 + 1 : d2 = data.IndexOf(d, d1)
                    synopGroup(i).canvas(k).notes = data.Substring(d1, d2 - d1)                     'notes
                    d1 = d2 + 1 : d2 = data.IndexOf(d, d1)
                    synopGroup(i).canvas(k).status = data.Substring(d1, d2 - d1)                    'status: kan hier voor iets anders gebruikt worden  TODO
                    d1 = d2 + 1 : d2 = data.IndexOf(d, d1)
                    synopGroup(i).canvas(k).direction = data.Substring(d1, d2 - d1)                 'direction: wisselstand,seinen
                    data = data.Substring(d2 + 1)   'aanpassen data voor volgend element
                Next
            Next
        Catch ex As Exception
            MessageBox.Show("Fout in Loadsynoptic ex=" & ex.ToString)
        End Try
    End Sub

#End Region

#Region "Dynamic Synoptic menu's EVENTS"

    Private Sub GeneralMenu_Click(ByVal sender As Object, ByVal e As EventArgs)     ' synoptiek MenuItem_click event
        'GeneralMenu_click vervangt alle individuele" MenuItem_click" event subroutines, 
        'welke anders bij design time aan een "MenuItem_click" toegekend worden
        Dim strBlokNR As String = String.Empty
        Dim itemNR As Integer = 0
        Dim index As Integer = 0
        'toekennen van het geklikt menuItem, het nr zit als de eerste 3 cijfer-karacters van de MenuItem titel "xxx titel"
        Dim i As Integer = CInt(sender.ToString.Substring(0, 3))        'index synop() en synopGroupActive()  = 3 eerste cijfertekens van de titel
        Dim j As Integer = i    'index synopGroup().Prop  = 3 eerste cijfertekens van de titel
        If synopMenuItemInUse(i) = False Then  'deze synoptiek werd nog niet opgeroepen
            synopMenuItemInUse(i) = True   ' boolean wordt "True" gezet om aan te duiden dat deze synoptiek geklikt werd
            synopGroupActive(i) = True
            synop(i) = New MainSynopFrm
            synop(i).MdiParent = Me
            Select Case synopGroup(j).Prop.appearance
                Case CanvasAppearances.Fitscreen
                    synop(i).Width = Me.ClientSize.Width - 4
                    synop(i).Height = Me.ClientSize.Height - MINCLIENTSIZE
                    synop(i).StartPosition = FormStartPosition.Manual
                Case CanvasAppearances.Standaard
                    synop(i).StartPosition = FormStartPosition.Manual
                Case CanvasAppearances.Maximized
                    synop(i).WindowState = FormWindowState.Maximized
            End Select
            synop(i).Text = synopGroup(j).Prop.tittle
            synop(i).tsCanvasInfo.Text = synopGroup(j).Prop.labelText
            'status aanpassen aan actuele situatie  canvas(x).status bevat het gridElement 
            SyncLock syncOK1
                For index = 0 To UBound(synopGroup(j).canvas) - 1
                    itemNR = CInt((synopGroup(j).canvas(index).itemNR))
                    If synopGroup(j).canvas(index).elementNR > 0 And synopGroup(j).canvas(index).elementNR < 3 Then   'blokelement
                        If instellingen.OHblokken.Contains(Format(itemNR, "000")) Then
                            'onderhoud
                            synopGroup(j).canvas(index).status = "U"
                        Else
                            If m_blok(itemNR).locoNR > 0 Then
                                synopGroup(j).canvas(index).locoNR = m_blok(itemNR).locoNR.ToString
                                If m_blok(itemNR).statusSynop = "A" Then
                                    synopGroup(j).canvas(index).status = "A"    'wel al gerden
                                Else
                                    synopGroup(j).canvas(index).status = "C"    'nog niet gereden
                                End If
                            Else
                                synopGroup(j).canvas(index).status = "V"
                            End If
                        End If
                    ElseIf synopGroup(j).canvas(index).elementNR > 29 Then  ' And synopGroup(j).canvas(index).elementNR < 3 Then
                        synopGroup(j).canvas(index).status = "*"
                    End If
                Next
                synop(i).Show()
            End SyncLock
        Else

            MessageBox.Show("Deze synoptiek is reeds in dienst", "Synoptieken menu", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub 'EventHandler for dynamic created synoptic menu's

#End Region

#Region "MDIform menuStrip, toolStrip and statusStrip"

#Region "General MenuItems for UserInterface manipulation"

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CutToolStripMenuItem.Click
        ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CopyToolStripMenuItem.Click
        ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PasteToolStripMenuItem.Click
        'Use My.Computer.Clipboard.GetText() or My.Computer.Clipboard.GetData to retrieve information from the clipboard.
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Dim about As New SplashScreen
        'about.MdiParent = Me
        about.StartPosition = FormStartPosition.CenterScreen
        about.Show()
    End Sub

    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click
        Dim OpenFileDialog As New OpenFileDialog
        OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        OpenFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = OpenFileDialog.FileName
            ' TODO: Add code here to open the file.
        End If
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

        If (SaveFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = SaveFileDialog.FileName
            ' TODO: Add code here to save the current contents of the form to a file.
        End If
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        Me.StatusStrip.Visible = Me.StatusBarToolStripMenuItem.Checked
    End Sub

#End Region

#Region "Self created MenuItems for railway manipulation"

    Private Sub ReisplannenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReisplannenToolStripMenuItem.Click
        Reisplannen()
    End Sub

    Private Sub HandbesturingTreinenWisselsVrijetreinenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HandbesturingTreinenWisselsVrijetreinenToolStripMenuItem.Click
        HandBesturing()
    End Sub

    Private Sub LogRapportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogRapportToolStripMenuItem.Click
        Me.DisplayInfo = New DisplayInfoChildfrm
        Me.DisplayInfo.MdiParent = Me
        Me.DisplayInfo.txtInfo.Text = displayText
        Me.DisplayInfo.Width = Me.ClientSize.Width - 4
        Me.DisplayInfo.Height = Me.ClientSize.Height - MINCLIENTSIZE
        Me.DisplayInfo.StartPosition = FormStartPosition.Manual
        DisplayInfo.Show()

    End Sub

    Private Sub S88ContatenStatusToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles S88ContatenStatusToolStripMenuItem.Click
        s88contactsView = True
        Me.DisplayS88Contacten = New s88ContactsFrm
        Me.DisplayS88Contacten.MdiParent = Me
        Me.DisplayS88Contacten.StartPosition = FormStartPosition.Manual
        DisplayS88Contacten.Show()
    End Sub

    Private Sub mnuEditeerGegevens_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditeerGegevens.Click
        EditorChldfrm = New Editor
        EditorChldfrm.MdiParent = Me
        Me.EditorChldfrm.Width = Me.ClientSize.Width - 4
        Me.EditorChldfrm.Height = Me.ClientSize.Height - MINCLIENTSIZE
        Me.EditorChldfrm.StartPosition = FormStartPosition.Manual
        EditorChldfrm.Show()
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
        Dim options As New Optionsfrm
        options.TopMost = True
        options.Show()
    End Sub

    Private Sub NieuwToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NieuwToolStripMenuItem.Click
        Dim prop As New NewSynopPropertiesFrm
        If prop.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            EditMenu.Visible = True
            ' Read the contents of testDialog's TextBox.
            Dim NewSynop As New NewSynopticfrm
            NewSynop.MdiParent = Me
            Select Case canvasProperty.appearance
                Case CanvasAppearances.Fitscreen
                    NewSynop.Width = Me.ClientSize.Width - 4
                    NewSynop.Height = Me.ClientSize.Height - MINCLIENTSIZE
                    NewSynop.StartPosition = FormStartPosition.Manual
                Case CanvasAppearances.Maximized
                    NewSynop.WindowState = FormWindowState.Maximized
                Case Else
                    NewSynop.StartPosition = FormStartPosition.Manual
            End Select
            NewSynop.tsbtnProperties.Enabled = False
            Me.mdiParentStatus.Text = "Opmaak van een nieuwe synoptiek"
            synopActiveNR = -1  '-1 omdat vanaf 0 er bestaande synoptieken zijn
            NewSynop.Show()
        End If
        prop.Dispose()
    End Sub

    Private Sub AanpassenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AanpassenToolStripMenuItem.Click
        Dim choice As New EditSynopticFrm
        For i As Integer = 1 To CInt(strSynopGroup(0))
            choice.lstBox.Items.Add(strSynopGroup(i).Substring(0, strSynopGroup(i).IndexOf("#")))
        Next
        If choice.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            If choice.lstBox.SelectedIndex < 0 Then
                choice.Dispose()
                Exit Sub
            End If
            EditMenu.Visible = True
            ' Read the contents of testDialog's TextBox.
            Dim NewSynop As New NewSynopticfrm
            NewSynop.MdiParent = Me
            Dim nr As Integer = choice.lstBox.SelectedIndex + 1   'aanpassen aan index van strSynopGroup
            canvasProperty.appearance = CType(strSynopGroup(nr).Substring(0, 1), CanvasAppearances)
            Dim d1 As Integer = strSynopGroup(nr).IndexOf("|", 0) + 1
            Dim d2 As Integer = strSynopGroup(nr).IndexOf("|", d1)
            Dim gstep As Integer = CInt(strSynopGroup(nr).Substring(d1, d2 - d1))
            canvasProperty.gridStep = gstep
            d1 = d2 + 1 : d2 = strSynopGroup(nr).IndexOf("|", d1) : canvasProperty.tittle = strSynopGroup(nr).Substring(d1, d2 - d1)
            d1 = d2 + 1 : d2 = strSynopGroup(nr).IndexOf("|", d1) : canvasProperty.labelText = strSynopGroup(nr).Substring(d1, d2 - d1)
            synopActiveNR = nr - 1

            For k As Integer = 0 To synopGroup(synopActiveNR).canvas.GetUpperBound(0)
                synopGroup(synopActiveNR).canvas(k).status = "*"
            Next
            NewSynop.tsbtnProperties.Enabled = True
            'Always Fitscreen 
            NewSynop.Width = Me.ClientSize.Width - 4
            NewSynop.Height = Me.ClientSize.Height - MINCLIENTSIZE
            NewSynop.StartPosition = FormStartPosition.Manual
            NewSynop.Show()
            Me.mdiParentStatus.Text = "Verbeteren van een nieuwe synoptiek"
        End If
        choice.Dispose()
    End Sub

    Private Sub WijzigGridstepToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WijzigGridstepToolStripMenuItem.Click

        Dim gridStep As Integer = 20
        Dim choice As New WijzigGridFrm
        For i As Integer = 1 To CInt(strSynopGroup(0))
            choice.lstBox.Items.Add(strSynopGroup(i).Substring(0, strSynopGroup(i).IndexOf("#")))
        Next
        If choice.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            If choice.lstBox.SelectedIndex < 0 Then Exit Sub
            Dim nr As Integer = choice.lstBox.SelectedIndex   'aanpassen aan index van strSynopGroup
            If choice.lstGridStep.SelectedIndex >= 0 Then gridStep = CInt(choice.lstGridStep.SelectedItem.ToString.Substring(0, 2))
            synopGroup(nr).Prop.gridStep = gridStep
            'pas grid aan
            If Not IsNothing(synop(nr)) Then synop(nr).SetGridStep(nr)
        End If
    End Sub

    Private Sub GridlijnenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridlijnenToolStripMenuItem.Click
        If GridlijnenToolStripMenuItem.Checked Then
            GridlijnenToolStripMenuItem.Checked = True
            instellingen.gridlijnenTonen = True
        Else
            GridlijnenToolStripMenuItem.Checked = False
            instellingen.gridlijnenTonen = False
        End If
    End Sub

    Private Sub tsMenuIngaves_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsMenuIngaves.Click
        ' hier moet de code komen om ingaves scherm op te roepen
        Me.Ingave = New Input
        Me.Ingave.MdiParent = Me
        Me.Ingave.StartPosition = FormStartPosition.Manual
        Me.Ingave.Show()
    End Sub

    Private Sub StartOnderwegTreinenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartOnderwegTreinenToolStripMenuItem.Click
        TreinenOnderweg()
    End Sub

    Private Sub BlokkenInOnderhoudToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlokkenInOnderhoudToolStripMenuItem.Click
        Dim s As String
        Dim j As Integer = 1
        s = instellingen.OHblokken

        Dim str((s.Length \ 4) - 1) As String
        For i As Integer = 0 To str.GetUpperBound(0)
            str(i) = s.Substring(j, 3)
            j += 4
        Next
        Array.Sort(str)
        s = ""
        For i As Integer = 0 To str.GetUpperBound(0)
            s += " - " + str(i)
        Next
        MessageBox.Show("Informatie blokken in onderhoud aantal= " + (str.GetUpperBound(0) + 1).ToString + vbNewLine + "Adressen:" + vbNewLine + s)
    End Sub

    Private Sub TreinenInDienstToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TreinenInDienstToolStripMenuItem1.Click
        Dim s As String
        Dim j As Integer = 1
        s = m_treinenInDienst
        Dim str((s.Length \ 4) - 1) As String
        For i As Integer = 0 To str.GetUpperBound(0)
            str(i) = s.Substring(j, 3)
            j += 4
        Next
        Array.Sort(str)
        s = ""
        For i As Integer = 0 To str.GetUpperBound(0)
            s += " - " + str(i)
        Next
        MessageBox.Show("Informatie treinen in dienst aantal= " + (str.GetUpperBound(0) + 1).ToString + vbNewLine + "Adressen:" + vbNewLine + s)
    End Sub

    Private Sub TreinenInHaltToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TreinenInHaltToolStripMenuItem.Click
        Dim s As String
        Dim j As Integer = 0
        Dim i As Integer = 0
        s = m_haltBlokken.ToString

        Dim str((s.Length \ 3) - 1) As String
        For i = 0 To str.GetUpperBound(0)
            str(i) = s.Substring(j, 3)
            j += 3
        Next
        Array.Sort(str)
        s = ""
        For i = 0 To str.GetUpperBound(0)
            s += " - " + str(i)
        Next
        MessageBox.Show("Informatie treinen in halt: aantal= " + i.ToString + vbNewLine + "Adressen:" + vbNewLine + s)
    End Sub

    Private Sub BlokkenInRetriggeringToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlokkenInRetriggeringToolStripMenuItem.Click
        Dim s As String
        Dim j As Integer = 0
        Dim i As Integer = 0
        s = m_retriggerBlokken.ToString

        Dim str((s.Length \ 3) - 1) As String
        For i = 0 To str.GetUpperBound(0)
            str(i) = s.Substring(j, 3)
            j += 3
        Next
        Array.Sort(str)
        s = ""
        For i = 0 To str.GetUpperBound(0)
            s += " - " + str(i)
        Next
        MessageBox.Show("Informatie treinen in retriggering: aantal= " + i.ToString + vbNewLine + "Adressen:" + vbNewLine + s)
    End Sub

    Private Sub BlokLocoHerstellenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlokLocoHerstellenToolStripMenuItem.Click
        BlokLocoFrm.Show()
    End Sub

    Private Sub OpWelkeBlokStaatLocoXToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpWelkeBlokStaatLocoXToolStripMenuItem.Click
        QueryBlockLocofrm.Show()
    End Sub

    Private Sub S88BitNEWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles S88BitOldNewToolStripMenuItem.Click
        S88BitInfoFrm.Show()
    End Sub

    Private Sub BloklocoRelatieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BloklocoRelatieToolStripMenuItem.Click
        ViewBlokLocoFrm.Show()
    End Sub

    Private Sub UitvoeringsModeToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles UitvoeringsModeToolStripMenuItem.Click
        If UitvoeringsModeToolStripMenuItem.Checked Then
            UitvoeringsModeToolStripMenuItem.Checked = False
            instellingen.uitvoeringsmode = False
            PowerONToolStripMenuItem.Enabled = False
            Me.ToolStripStatuslblRunning.BackColor = Color.Yellow
            Me.ToolStripStatuslblRunning.Text = "  Inactief mode   "
        Else
            UitvoeringsModeToolStripMenuItem.Checked = True
            instellingen.uitvoeringsmode = True
            PowerONToolStripMenuItem.Enabled = True
            Me.ToolStripStatuslblRunning.BackColor = Color.Orange
            Me.ToolStripStatuslblRunning.Text = "  Inaktief mode   "
        End If
    End Sub

    Private Sub AlleWisselsRechtdoorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlleWisselsRechtdoorToolStripMenuItem.Click
        Dim i As Integer = 0
        Try
            dataD.port = "1"    'rechtdoor
            dataD.status = False
            dataD.timeout = instellingen.turnoutPulsTime
            Dim locoNR As Integer = 0
            'uitvoeren, enkel als alle treinen stilstaan
            If m_treinenInDienst.Length > 0 Then
                For i = 0 To m_treinenInDienst.Length - 1 Step 4            'er staat steeds een spatie voor het loconr
                    locoNR = CInt(m_treinenInDienst.Substring(i + 1, 3))
                    If m_treinen.Item(locoNR).Snelheid > 0 Then Exit Sub
                Next
            Else
                'geen treinen in dienst
            End If
            ToolStripProgressBar.Value = 0
            Dim progresStep As Single = CSng(100 / m_ArrayK83K84.GetUpperBound(0))
            For i = 1 To m_MaxK83K84
                If m_ArrayK83K84(i).active = "1" Then       'wissel indienst
                    m_ArrayK83K84(i).port = "1"
                    dataD.address = m_ArrayK83K84(i).address
                    DCS.SetTurnout(dataD)
                    ToolStripProgressBar.Value = CInt(i * progresStep)
                    Sleep(200)
                End If
            Next
            ToolStripProgressBar.Value = 0

        Catch ex As Exception
            MsgBox("Fout ex= " + ex.ToString, MsgBoxStyle.Exclamation, "Alle wissels rechtdoor")
        End Try
    End Sub

    Private Sub AlleWisselsAfbuigenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlleWisselsAfbuigenToolStripMenuItem.Click

        Dim i As Integer = 0
        Try
            dataD.port = "0"        'afbuigen
            dataD.status = False
            dataD.timeout = instellingen.turnoutPulsTime
            Dim locoNR As Integer = 0
            'uitvoeren, enkel als alle treinen stilstaan
            If m_treinenInDienst.Length > 0 Then
                For i = 0 To m_treinenInDienst.Length - 1 Step 4         'er staat steeds een spatie voor het loconr
                    locoNR = CInt(m_treinenInDienst.Substring(i + 1, 3))
                    If m_treinen.Item(locoNR).Snelheid > 0 Then Exit Sub
                Next               
            Else
                'geen treinen in dienst
            End If
            ToolStripProgressBar.Value = 0
            Dim progresStep As Single = CSng(100 / m_ArrayK83K84.GetUpperBound(0))
            For i = 1 To m_MaxK83K84
                If m_ArrayK83K84(i).active = "1" Then       'wissel indienst
                    m_ArrayK83K84(i).port = "0"
                    dataD.address = m_ArrayK83K84(i).address
                    DCS.SetTurnout(dataD)
                    ToolStripProgressBar.Value = CInt(i * progresStep)
                    Sleep(200)
                End If
            Next
            ToolStripProgressBar.Value = 0

        Catch ex As Exception
            MsgBox("Fout ex= " + ex.ToString, MsgBoxStyle.Exclamation, "Alle wissels afbuigen")
        End Try
    End Sub

    Private Sub AlleTreinenStoppenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlleTreinenStoppenToolStripMenuItem.Click
        Dim locoNR As Integer = 0
        Dim index As Integer = 1
        Try
            If m_treinenInDienst.Length > 0 Then
                ReDim ClipboardTrains(CInt(m_treinenInDienst.Length / 4) - 1)
                For i As Integer = 0 To ClipboardTrains.GetUpperBound(0)         'er staat steeds een spatie voor het loconr
                    locoNR = CInt(m_treinenInDienst.Substring(index, 3))
                    ClipboardTrains(i).address = CStr(locoNR)      'address
                    If Not IsNothing(m_treinen.Item(locoNR)) Then
                        ClipboardTrains(i).speed = CStr(m_treinen.Item(locoNR).Snelheid)    'snelheid
                    Else
                        ClipboardTrains(i).speed = "0"
                    End If
                    index += 4
                    'stop de trein
                    m_ArrayLOCO(locoNR).address = CStr(locoNR)
                    m_ArrayLOCO(locoNR).speed = "0"
                    DCS.SetLoco(locoNR)
                Next
            End If
        Catch ex As Exception
            MsgBox("FoutrijdenDataBtnHerstart in Alle treinen stoppen ex= " + ex.ToString, MsgBoxStyle.Exclamation, "Alle treinen stoppen")
        End Try

    End Sub

    Private Sub AlleTreinenHerstartenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlleTreinenHerstartenToolStripMenuItem.Click
        Dim locoNR As Integer = 0
        Try
            For i As Integer = 0 To ClipboardTrains.GetUpperBound(0)
                locoNR = CInt(ClipboardTrains(i).address)
                If m_treinenInDienst.Contains(" " + Format(locoNR, "000")) Then        'bestaat deze trein nog?
                    m_ArrayLOCO(locoNR).address = ClipboardTrains(i).address
                    m_ArrayLOCO(locoNR).speed = ClipboardTrains(i).speed
                    DCS.SetLoco(locoNR)
                End If
            Next
        Catch ex As Exception
            MsgBox("Fout  ex= " + ex.ToString, MsgBoxStyle.Exclamation, "Alle treinen Herstarten")
        End Try
    End Sub

    Private Sub BlokkenInDienstToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlokkenInDienstToolStripMenuItem1.Click
        Dim s As String = String.Empty
        If m_blokkenInDienst.Length > 0 Then
            For i As Integer = 0 To m_blokkenInDienst.Length - 7 Step 8
                s += m_blokkenInDienst.Substring(i, 8) + "   -   "
            Next
        End If
        MsgBox("Blokken met hun loco" + vbNewLine + s, MsgBoxStyle.SystemModal, "Actuele blokbezetting")        ' 
    End Sub

    Private Sub VergrendeldeWisselsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VergrendeldeWisselsToolStripMenuItem.Click
        If VergrendeldeWisselsToolStripMenuItem.Checked Then
            VergrendeldeWisselsToolStripMenuItem.Checked = False
        Else
            VergrendeldeWisselsToolStripMenuItem.Checked = True
        End If
    End Sub

    Private Sub OpkuisStatuslijnToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpkuisStatuslijnToolStripMenuItem.Click
        Opkuisstatuslijn()
    End Sub

    Private Sub ProgrammaHelpToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ProgrammaHelpToolStripMenuItem.Click
        Me.HelpProgramma = New Help
        Dim myStreamReader As StreamReader
        Try
            myStreamReader = File.OpenText(DataDir & "\HelpProgramma.txt")
            Me.HelpProgramma.txtHelp.Text = myStreamReader.ReadToEnd
            myStreamReader.Close()
            Me.HelpProgramma.MdiParent = Me
            Me.HelpProgramma.StartPosition = FormStartPosition.Manual
            Me.HelpProgramma.Show()
        Catch ex As Exception
            MessageBox.Show("Fout in Bestand: HelpProgramma.txt: " + ex.Message)
        End Try
    End Sub

    Private Sub FileMenu_Click(sender As System.Object, e As System.EventArgs) Handles FileMenu.Click
        If instellingen.uitvoeringsmode Then UitvoeringsModeToolStripMenuItem.Checked = True Else UitvoeringsModeToolStripMenuItem.Checked = False
        If instellingen.K83K84detectie Then K83K84GegevensToolStripMenuItem.Checked = True Else K83K84GegevensToolStripMenuItem.Checked = False
        If instellingen.gridlijnenTonen Then GridlijnenToolStripMenuItem.Checked = True Else GridlijnenToolStripMenuItem.Checked = False
    End Sub

    Private Sub S88ColonRangeToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles S88ColonRangeToolStripMenuItem.Click
        Dim message, title, defaultValue As String
        Dim myValue As Object
        ' Set prompt.
        message = "Enter a value between 1 and 4"
        ' Set title.
        title = "InputBox Demo"
        defaultValue = instellingen.s88ColonRange       ' "1"   ' Set default value.

        ' Display message, title, and default value.
        myValue = InputBox(message, title, defaultValue)

        ' If user has clicked Cancel, set myValue to defaultValue
        If myValue Is "" Then myValue = defaultValue Else instellingen.s88ColonRange = CStr(myValue)

    End Sub

#Region "Gegevensbestanden herladen"

    Private Sub AlleBestandenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AlleBestandenToolStripMenuItem.Click
        mdiParentStatus.Text = "Opladen van Alle gegevensbestanden van My Documents\WallarDCS"
        LoadModelRailwayData()
    End Sub

    Private Sub BlokGegevensToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles BlokGegevensToolStripMenuItem.Click
        mdiParentStatus.Text = "Opladen van Blok gegevens van My Documents\WallarDCS"
        LoadBlokLocoArray()
    End Sub

    Private Sub LocoGegevensToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LocoGegevensToolStripMenuItem.Click
        mdiParentStatus.Text = "Opladen van Loco gegevens van My Documents\WallarDCS"
        LoadLocoData()
    End Sub

    Private Sub BevelenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles BevelenToolStripMenuItem.Click
        mdiParentStatus.Text = "Opladen van Bevelen van My Documents\WallarDCS"
        LoadBevelen()
    End Sub

    Private Sub RelatiesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RelatiesToolStripMenuItem.Click
        mdiParentStatus.Text = "Opladen van Relaties van My Documents\WallarDCS"
        LoadRelaties()
    End Sub

    Private Sub K83K84GegevensToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles K83K84GegevensToolStripMenuItem.Click
        mdiParentStatus.Text = "Opladen van K83K84 gegevens van My Documents\WallarDCS"
        LoadK83K84()
    End Sub

    Private Sub s88MacroNummersToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles s88MacroNummersToolStripMenuItem.Click
        mdiParentStatus.Text = "Opladen van macro gegevens van My Documents\WallarDCS"
        LoadS88MacroNRs()
        LoadMacro()
    End Sub

    Private Sub SynoptiekenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SynoptiekenToolStripMenuItem.Click
        mdiParentStatus.Text = "Opladen van Synoptieken van My Documents\WallarDCS"
        LoadSynoptics()
    End Sub

    Private Sub ReiswegenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ReiswegenToolStripMenuItem.Click
        mdiParentStatus.Text = "Opladen van Reiswegen van My Documents\WallarDCS"
        LoadReiswegenConditiereiswegen()
    End Sub

    Private Sub ReisplannenToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles ReisplannenToolStripMenuItem1.Click
        mdiParentStatus.Text = "Opladen van Reisplannen van My Documents\WallarDCS"
        LoadReisplannen()
    End Sub

    Private Sub InstellingenToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles InstellingenToolStripMenuItem.Click
        mdiParentStatus.Text = "Opladen van Instellingen van My Documents\WallarDCS"
        InstellingenData()
    End Sub

#End Region

#End Region

#Region "Toolstrip subs and functions"

    Private Sub ToolStripNoodstop_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripNoodstop.Click
        DCS.SETPowerOnOff(False)
    End Sub

    Private Sub ToolStripOpkuisstatuslijn_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripOpkuisstatuslijn.Click
        Opkuisstatuslijn()
    End Sub

    Private Sub ToolStripStartTreinenOnderweg_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripStartTreinenOnderweg.Click
        TreinenOnderweg()
    End Sub

    Private Sub ToolStripReisplannen_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripReisplannen.Click
        Reisplannen()
    End Sub

    Private Sub ToolStripHandBesturing_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripHandBesturing.Click
        HandBesturing()
    End Sub

#End Region

#Region "Common Sub and functions from menuItems or Toolstrip"

    Private Sub Opkuisstatuslijn()
        SyncLock syncOK
            Me.mdiParentStatus.Text = String.Empty
        End SyncLock

    End Sub

    Private Sub TreinenOnderweg()
        DCS.StartTreinenOnderweg()
        StartOnderwegTreinenToolStripMenuItem.Visible = False     'maar één keer toelaten

    End Sub

    Private Sub Reisplannen()
        Me.ReisPlan = New ReisplanFrm
        Me.ReisPlan.Show()

    End Sub

    Private Sub HandBesturing()
        Me.commandoChildfrm = New CommandoChildfrm
        Me.commandoChildfrm.Show()
    End Sub

#End Region

#End Region

#Region "Commands from CommandoChildFrm "

    Public Sub AdaptTurnoutStatus(ByVal status As String, ByVal direction As String, ByVal address As String)
        'geef de status van het blok door aan de sub BlokStatus(,,) van in dienst zijnde synoptieken op het scherm
        For i As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
            If synopMenuItemInUse(i) Then synop(i).TurnoutStatus(status, direction, address)
        Next
    End Sub

#End Region

#Region "Commands for DiCoStation"

    Private Sub PowerONToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PowerONToolStripMenuItem.Click
        DCS.SETPowerOnOff(True)
        UitvoeringsModeToolStripMenuItem.Enabled = False
    End Sub

    Private Sub PowerOFFToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PowerOFFToolStripMenuItem.Click
        DCS.SETPowerOnOff(False)
        UitvoeringsModeToolStripMenuItem.Enabled = True
    End Sub

#End Region

#Region "Events Handler from DiCoStation"

    Private Sub HandlerStatus(ByVal statusLine As String)
        If Me.InvokeRequired Then
            Me.BeginInvoke(New handler_DelegateString(AddressOf HandlerStatus), New Object() {statusLine})
            Return
        End If
        'put data also in DisplayInfoChildfrm
        displayText = statusLine & vbNewLine & displayText
        If displayText.Length > 120000 Then displayText = String.Empty
        If Not IsNothing(DisplayInfo) Then
            Me.DisplayInfo.txtInfo.Text = displayText
        End If
        If statusLine.Contains(vbNewLine) Then statusLine = statusLine.Replace(vbNewLine, "")
        Me.mdiParentStatus.Text = statusLine
    End Sub

    Private Sub HandlerS88Status(ByVal statusLine As String)
        If Me.InvokeRequired Then
            Me.BeginInvoke(New handler_DelegateString(AddressOf HandlerS88Status), New Object() {statusLine})
            Return
        End If
        'put data also in s88Contactsfrm
        displayText = statusLine & vbNewLine & displayText
        If displayText.Length > 120000 Then displayText = String.Empty
        If Not IsNothing(DisplayInfo) Then
            Me.DisplayInfo.txtInfo.Text = displayText
        End If
        If statusLine.Contains(vbNewLine) Then statusLine = statusLine.Replace(vbNewLine, "")
        Me.mdiParentStatus.Text = statusLine
    End Sub

    Private Sub HandlerLogData(ByVal data As String)
        If Me.InvokeRequired Then
            Me.BeginInvoke(New handler_DelegateString(AddressOf HandlerStatus), New Object() {data})
            Return
        End If
        MessageBox.Show(data)
    End Sub

    Private Sub handlerChangeBlok(ByVal status As String, ByVal blokNR As Integer, ByVal locoNR As Integer)
        If Me.InvokeRequired Then
            Me.BeginInvoke(New handler_DelegateStringIntegerInteger(AddressOf handlerChangeBlok), _
                New Object() {status, blokNR, locoNR})
            Return
        End If
        'geef de status van het blok door aan de sub BlokStatus(,,) van in dienst zijnde synoptieken op het scherm
        For i As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
            If synopMenuItemInUse(i) Then synop(i).BlokStatus(status, blokNR, locoNR)
        Next
    End Sub

    Private Sub handlerChangeTurnout(ByVal status As String, ByVal direction As String, ByVal address As String)
        If Me.InvokeRequired Then
            Me.BeginInvoke(New handler_DelegateStringStringString(AddressOf handlerChangeTurnout), _
                New Object() {status, direction, address})
            Return
        End If
        'geef de status van het blok door aan de sub BlokStatus(,,) van in dienst zijnde synoptieken op het scherm
        For i As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
            If synopMenuItemInUse(i) Then synop(i).TurnoutStatus(status, direction, address)
        Next
    End Sub

    Private Sub handlerHalt(ByVal halt As String)
        Dim i As Integer
        Dim j As Integer = 0
        Dim s As String = String.Empty
        If Me.InvokeRequired Then
            Me.BeginInvoke(New handler_DelegateString(AddressOf handlerHalt), _
                New Object() {halt})
            Return
        End If
        If halt.Length > 0 Then
            s = halt
            Dim str((s.Length \ 3) - 1) As String
            For i = 0 To str.GetUpperBound(0)
                str(i) = s.Substring(j, 3)
                j += 3
            Next
            Array.Sort(str)
            s = "Halt="
            For i = 0 To str.GetUpperBound(0)
                s += " - " + str(i)
            Next
            Me.ToolStripStatusLabelHalt.Text = s
        Else
            Me.ToolStripStatusLabelHalt.Text = ""       'reset
        End If
    End Sub

    Private Sub handlerInteger(ByVal n As Integer)
        If Me.InvokeRequired Then
            Me.BeginInvoke(New handler_DelegateInteger(AddressOf handlerInteger), _
                New Object() {n})
            Return
        End If
        Select Case n
            Case 0  'pas als alle initialisaties achter de rug zijn, het hoofddisplay tonen
                showMainDisplay = True
            Case Else
        End Select
    End Sub

    Private Sub handlerThreadRunning(ByVal ok As Boolean)
        If Me.InvokeRequired Then
            Me.BeginInvoke(New handler_DelegateBoolean(AddressOf handlerThreadRunning), _
                New Object() {ok})
            Return
        End If
        If ok Then
            Me.ToolStripStatuslblRunning.Text = "       Rij  mode      "
            Me.ToolStripStatuslblRunning.BackColor = Color.Green
            HerladenGegevensToolStripMenuItem.Enabled = False
        Else
            Me.ToolStripStatuslblRunning.Text = "  Power Off mode  "
            Me.ToolStripStatuslblRunning.BackColor = Color.Red
            HerladenGegevensToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub handlerBitArray(ByVal BitsArray As BitArray)
        Dim strBits As String = String.Empty
        Dim startIndex, EndIndex, blokNR As Integer
        If Me.InvokeRequired Then
            Me.BeginInvoke(New handler_DelegateBitArray(AddressOf handlerBitArray), _
                New Object() {BitsArray})
            Return
        End If
        'waarde colonnen
        Beep()
        Select Case CInt(instellingen.s88ColonRange)
            Case 1
                startIndex = 0 : EndIndex = 127
            Case 2
                startIndex = 128 : EndIndex = 255
            Case 3
                startIndex = 256 : EndIndex = 383
            Case 4
                startIndex = 284 : EndIndex = 511
            Case Else
                startIndex = 0 : EndIndex = m_MaxS88nrs - 2
        End Select

        If BitsArray.Count < EndIndex Then EndIndex = BitsArray.Count - 4
        For i = startIndex To EndIndex Step MAXS88COLONS
            For j As Integer = 0 To 3
                blokNR = (i + j) \ 2 + 1
                If BitsArray.Item(i + j) Then
                    strBits += Format((i + j), "000") + " [" + Format(blokNR, "000") + "]" + " ======> " + BitsArray.Item(i + j).ToString + " <=====" + vbTab
                Else
                    strBits += Format((i + j), "000") + "       " + BitsArray.Item(i + j).ToString + vbTab + vbTab + vbTab
                End If
            Next
            strBits += vbNewLine
        Next
        Me.DisplayS88Contacten.txts88contacts.Text = strBits
    End Sub

#End Region

End Class

