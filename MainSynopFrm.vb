'Wallar (c) 2011
'20111104

Imports System.Threading.Thread
Imports System.Drawing.Drawing2D
Imports System.Drawing.SolidBrush
Imports System.ComponentModel

Public Class MainSynopFrm

#Region "Initial Class Vars"

    'Initial values
    Private synopNR As Integer          'wordt uit de titel van de active form gehaald
    Private gridStep As Integer = 24    'determines the space between lines  
    Private gridUnit As Integer
    Private gridText As Integer
    Private cv As CanvasCells
    Private data As DecoderM
    'colors
    Private colorFre As Color = Color.FromArgb(CInt(instellingen.ColorVrij))
    Private colorInit As Color = Color.FromArgb(CInt(instellingen.ColorInit))
    Private colorInitBlok As Color = Color.FromArgb(CInt(instellingen.ColorInitBlok))
    Private colorReserved As Color = Color.FromArgb(CInt(instellingen.ColorGereserveerd))
    Private colorPreReserved As Color = Color.FromArgb(CInt(instellingen.ColorPreReserved))
    Private colorOccupied As Color = Color.FromArgb(CInt(instellingen.ColorBezet))
    Private colorAfremOccupied As Color = Color.FromArgb(CInt(instellingen.ColorAfremBezet))
    Private colorRetrigger As Color = Color.FromArgb(CInt(instellingen.ColorRetrigger))
    Private colorHalt As Color = Color.FromArgb(CInt(instellingen.colorHalt))
    Private colorLocked As Color = Color.FromArgb(CInt(instellingen.ColorVergrendeld))
    Private colormaintenance As Color = Color.FromArgb(CInt(instellingen.ColorOnderhoud))
    Private colorError As Color = Color.FromArgb(CInt(instellingen.ColorSpook))
    Private colorTextBack As Color = Color.FromArgb(CInt(instellingen.ColorTextBack))
    Private colorlocatieTrein As Color = Color.FromArgb(CInt(instellingen.ColorLocatieTrein))
    Private colorPassief As Color = Color.FromArgb(CInt(instellingen.colorPassief))
    Private colorHandbediening As Color = Color.FromArgb(CInt(instellingen.ColorHandbediening))
    Private colorElementBackground As Color = Color.FromArgb(CInt(instellingen.ColorAchtergrondElement))
    'pens
    Private gridPen As New Pen(Color.FromArgb(CInt(instellingen.ColorGridLijnen)), 1)
    Private elementPen As New Pen(Color.FromArgb(CInt(instellingen.ColorInit)), 1)
    Private elementPenFre As New Pen(Color.FromArgb(CInt(instellingen.ColorVrij)), 1)
    Private elementPenStatus As New Pen(Color.Black, 1) 'kleur wordt dynamisch aangepast
    Private blockPen As New Pen(Color.FromArgb(CInt(instellingen.colorBlockPen)), 1)
    Private passiefPen As New Pen(Color.FromArgb(CInt(instellingen.colorPassief)), 2)
    Private balPen As New Pen(Color.FromArgb(CInt(instellingen.ColorAchtergrondSynop)), 1)
    Private textPen As New Pen(Color.FromArgb(CInt(instellingen.ColorBezet)), 1)
    'fonts
    Private blockFont As Font
    Private textFont As Font

    'solidbrushes
    Private blockBrush As New SolidBrush(Color.FromArgb(CInt(instellingen.ColorGridLijnen)))
    Private blockFillBrush As New SolidBrush(Color.FromArgb(CInt(instellingen.ColorGridLijnen)))
    Private TextFillBrush As New SolidBrush(Color.FromArgb(CInt(instellingen.ColorTextBack)))
    Private textBrush As New SolidBrush(Color.FromArgb(CInt(instellingen.ColorGridLijnen)))
    Private blancoBrush As New SolidBrush(Color.FromArgb(CInt(instellingen.ColorGridLijnen)))
    Private blockBackgroundBrush As New SolidBrush(Color.FromArgb(CInt(instellingen.ColorAchtergrondElement)))

    'stringformat
    Private blockStrFormat As New StringFormat
    'draw coordinaten in DrawElements
    Private X1, Y1, XU, XS, YU, YS, X1H, XSH, Y1H, YSH As Integer   'used only in Sub DrawElement
    Private cm() As ContextMenuStrip
    Private indexCanvas As Integer
#End Region

#Region "Subs bij laden en afsluiten MainSynopFrm class  "

    Private Sub MainSynopFrm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Dim synopNR As Integer = CInt(Me.Text.Substring(0, 3))
            synopMenuItemInUse(synopNR) = False
            If Not IsNothing(cm(synopNR)) Then cm(synopNR).Dispose()
            Me.Dispose()
        Catch ex As Exception
            MessageBox.Show("Fout in afsluiten context menu 'cm' in MainSynopFrm_Closing", "Fout in MainSynopFrm" & vbNewLine & " Synoptics.txt bestaat is fout", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub MainSynopFrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        synopNR = CInt(Me.Text.Substring(0, 3))     'welke synop wordt geladen: in titel nummer gevormd uit de 3 eerste tekens
        EnableDoubleBuffering()     'verbetert de teken snelheid
        SetCanvasVariables()        'pas de variabelen aan aan de instellingen en opties van het lopende programma
    End Sub

    Public Sub EnableDoubleBuffering()
        ' Set the value of the double-buffering style bits to true.
        Me.SetStyle(ControlStyles.DoubleBuffer _
          Or ControlStyles.UserPaint _
          Or ControlStyles.AllPaintingInWmPaint, _
          True)
        Me.UpdateStyles()
    End Sub

#End Region

#Region "Sub en functions voor Synopbewerkingen en GeneralContextMenu"

    Private Sub SetCanvasVariables()
        'Setting variabelen voor synoptic grid  ===================
        If synopGroupActive(synopNR) Then
            Me.canvas.BackColor = Color.FromArgb(CInt(instellingen.ColorAchtergrondSynop))
            gridStep = synopGroup(synopNR).Prop.gridStep
            gridUnit = gridStep \ canvasValue.setGridUnit
            gridText = gridStep \ canvasValue.setGridText
            gridPen.Color = Color.FromArgb(CInt(instellingen.ColorGridLijnen))
            gridPen.Width = 1
            'diktes pennen
            elementPen.Width = gridStep \ canvasValue.setPenWidth
            elementPenFre.Width = gridStep \ canvasValue.setPenWidth
            elementPenStatus.Width = gridStep \ canvasValue.setPenWidth
            elementPenStatus.LineJoin = Drawing2D.LineJoin.Round
            balPen.Width = gridStep \ canvasValue.setPenWidth
            passiefPen.Width = gridStep \ canvasValue.setPenWidth

            'block properties
            blockFont = New Font("Arial", gridUnit)
            blockBrush.Color = Color.FromArgb(CInt(instellingen.colorBlockPen))
            blockFillBrush.Color = Color.FromArgb(CInt(instellingen.ColorBezet))
            blockStrFormat.FormatFlags = StringFormatFlags.DirectionVertical
            'text
            textBrush.Color = Color.FromArgb(CInt(instellingen.colorBlockPen))
            textFont = New Font("Verdana", gridUnit)
            'blanco
            blancoBrush.Color = Color.White

            'contextMenuStrip cm
            ReDim cm(uBoundSynopGroup)
            cm(synopNR) = New ContextMenuStrip
            canvas.ContextMenuStrip = cm(synopNR)
            'zie ook sub GeneralContextMenuMenu om rode string teksten te synchroniseren voor oproepen van de gepaste cm (contextmenu)
            cm(synopNR).Items.Add("Blok in onderhoud zetten", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))        '0
            cm(synopNR).Items.Add("Blok in dienst nemen", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))            '1
            cm(synopNR).Items.Add("Blok vrij zetten", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))                '2
            cm(synopNR).Items.Add("-------------------", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))             '3
            cm(synopNR).Items.Add("Blok-loco herstellen", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))            '4
            cm(synopNR).Items.Add("-------------------", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))             '5
            cm(synopNR).Items.Add("Trein uit dienst Nemen", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))          '6
            cm(synopNR).Items.Add("Vrije Trein in dienst nemen", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))     '7
            cm(synopNR).Items.Add("Nieuwe loco in dienst nemen", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))     '8
            cm(synopNR).Items.Add("-------------------", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))             '9
            cm(synopNR).Items.Add("Wissel in onderhoud zetten", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))      '10
            cm(synopNR).Items.Add("Wissel vrij zetten", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))              '11
            cm(synopNR).Items.Add("-------------------", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))             '12
            cm(synopNR).Items.Add("Informatie blok", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))                 '13
            cm(synopNR).Items.Add("Informatie trein", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))                '14
            cm(synopNR).Items.Add("Informatie locomotief", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))           '15
            cm(synopNR).Items.Add("Informatie reisweg Deel", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))         '16
            cm(synopNR).Items.Add("Informatie wissel", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))               '17
            cm(synopNR).Items.Add("Informatie Synoptiek", Nothing, New EventHandler(AddressOf GeneralContextMenuMenu_Click))            '18
        End If
    End Sub

    Private Sub GeneralContextMenuMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim s As Integer = 0
        Dim b As Boolean = False
        Dim display As String = String.Empty
        Select Case sender.ToString
            Case "Trein uit dienst Nemen"
                SynopIsblokNR = CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR)
                SynopIsLocoNR = CInt(synopGroup(synopNR).canvas(indexCanvas).locoNR)
                If m_treinenInDienst = String.Empty Then Exit Sub
                Try
                    If Not IsNothing(m_treinen.Item(SynopIsLocoNR)) Then

                        s = m_treinenInDienst.IndexOf(Format(SynopIsLocoNR, "000"))
                        If s > 0 Then   'deze trein is in dienst
                            If m_treinen.Item(SynopIsLocoNR).Snelheid > 0 Then
                                MsgBox("De snelheid van locoNR " + SynopIsLocoNR.ToString + " is groter dan 0" + vbNewLine + "Trein kan niet uit dienst genomen worden", MsgBoxStyle.Critical, "Vrije trein uitdienstname")
                                Exit Sub
                            Else
                                'Gewone of vrijetrein?
                                Dim rwDeel As String = m_treinen(SynopIsLocoNR).rwDeel
                                If rwDeel.Length > 0 Then
                                    If m_treinen.Item(SynopIsLocoNR).VrijeTrein Then
                                        'synop gereserveerd blok vrij zetten
                                        Dim reservedBlokNR As Integer
                                        If Not rwDeel.StartsWith("<") Then
                                            SyncLock syncOK
                                                DCS.VrijeTreinUitDienstname(SynopIsblokNR, SynopIsLocoNR, rwDeel)
                                                'reservedblokgegevens aanpassen
                                                reservedBlokNR = CInt(rwDeel.Substring(rwDeel.IndexOf("b", 0) + 1, 3))
                                                DCS.SetBlokGegevens(SynopIsblokNR, SynopIsLocoNR, 1)        'Blok VRIJ
                                            End SyncLock
                                            'geef de status van het blok door aan de sub BlokStatus(status,blokNR,locoNR) van in dienst zijnde synoptieken op het scherm
                                            For j As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken 
                                                If synopMenuItemInUse(j) Then synop(j).BlokStatus(m_blok(reservedBlokNR).statusSynop, reservedBlokNR, 0)
                                            Next
                                        Else 'gewone trein
                                            DCS.TreinUitDienstname(SynopIsLocoNR, rwDeel)
                                        End If
                                    Else
                                        DCS.TreinUitDienstname(SynopIsLocoNR, rwDeel)
                                    End If
                                End If
                                m_treinen.Item(SynopIsLocoNR).FrontSein = False
                                m_treinen.Item(SynopIsLocoNR).ReisPlan = ""
                                m_treinen.Item(SynopIsLocoNR).HaltPassagiersTrein = ""
                                m_treinen.Item(SynopIsLocoNR).VrijeTrein = False
                                m_treinen.Item(SynopIsLocoNR).StatusInDienst = False
                                'zet loco gegevens juist
                                m_ArrayLOCO(SynopIsLocoNR).address = SynopIsLocoNR.ToString
                                m_ArrayLOCO(SynopIsLocoNR).speed = "0"
                                m_ArrayLOCO(SynopIsLocoNR).F0 = "0"
                                m_ArrayLOCO(SynopIsLocoNR).F1 = "0"
                                m_ArrayLOCO(SynopIsLocoNR).F2 = "0"
                                m_ArrayLOCO(SynopIsLocoNR).F3 = "0"
                                m_ArrayLOCO(SynopIsLocoNR).F4 = "0"
                                DCS.SetLoco(SynopIsLocoNR)
                                'geef de status van het blok door aan de sub BlokStatus(status,blokNR,locoNR) van in dienst zijnde synoptieken op het scherm
                                For j As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken 
                                    If synopMenuItemInUse(j) Then synop(j).BlokStatus("A", SynopIsblokNR, SynopIsLocoNR)
                                Next
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Beep()
                    MsgBox("Fout Trein uit dienst nemen, ex= " + ex.ToString, MsgBoxStyle.Critical, "GeneralContexMenu")
                End Try

            Case "Vrije Trein in dienst nemen"
                SynopIsblokNR = CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR)
                SynopIsLocoNR = CInt(synopGroup(synopNR).canvas(indexCanvas).locoNR)
                If SynopIsLocoNR <> 0 Then
                    If instellingen.OHblokken.IndexOf("|" + Format(SynopIsblokNR, "000")) <> -1 Then
                        MsgBox("BlokNr " + SynopIsblokNR.ToString + " staat in onderhoud of retrigger, trein kan niet indienst worden genomen", MsgBoxStyle.Information, "Vrije trein indienstname")
                    Else
                        VrijTreinSynopFrm.Show()
                    End If
                End If

            Case "Nieuwe loco in dienst nemen"
                SynopIsblokNR = CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR)
                instellingen.OHblokken += "|" + Format(SynopIsblokNR, "000")
                NieuweLocoIndienstFrm.Show()

            Case "Blok-loco herstellen"
                SynopIsblokNR = CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR)
                SynopIsLocoNR = CInt(synopGroup(synopNR).canvas(indexCanvas).locoNR)
                SynopBlokLocoFrm.Show()

            Case "Blok in onderhoud zetten"
                SynopIsblokNR = CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR)
                SynopIsLocoNR = CInt(synopGroup(synopNR).canvas(indexCanvas).locoNR)
                SyncLock syncOK
                    b = (m_blok(SynopIsblokNR).status)
                End SyncLock
                If (b = False AndAlso m_blok(SynopIsblokNR).statusNaarRIJsectie = False) _
                    Or ((Not IsNothing(m_treinen.Item(SynopIsLocoNR)) AndAlso m_treinen.Item(SynopIsLocoNR).Snelheid = 0)) Then
                    instellingen.OHblokken = instellingen.OHblokken + "|" + Format(SynopIsblokNR, "000")      'toevoegen
                    instellingen.OHblokken = instellingen.OHblokken  'aanpassen
                    m_blok(SynopIsblokNR).statusSynop = "U"
                    'geef de status van het blok door aan de sub BlokStatus(,,) van in dienst zijnde synoptieken op het scherm
                    For j As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
                        If synopMenuItemInUse(j) Then synop(j).BlokStatus("U", SynopIsblokNR, SynopIsLocoNR)
                    Next
                Else
                    MsgBox("BlokNr " + SynopIsblokNR.ToString + " is bezet, blok kan nu niet in onderhoud gezet worden", _
                           MsgBoxStyle.Information, "Blok in onderhoud zetten")
                End If

            Case "Blok in dienst nemen"
                Dim status As String = String.Empty
                SynopIsblokNR = CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR)
                SynopIsLocoNR = CInt(synopGroup(synopNR).canvas(indexCanvas).locoNR)
                Dim index As Integer = 0
                Dim strBlokNR As String = "|" + Format(SynopIsblokNR, "000")
                For i = 1 To instellingen.OHblokken.Length - 3 Step 4
                    index = instellingen.OHblokken.IndexOf(strBlokNR)
                    If index <> -1 Then
                        instellingen.OHblokken = instellingen.OHblokken.Replace(strBlokNR, "")   'verwijder uit blokken in onderhoud
                        Exit For
                    End If
                Next
                instellingen.OHblokken = instellingen.OHblokken  'aanpassen
                SyncLock syncOK
                    If SynopIsLocoNR > 0 Then
                        status = "B"
                        m_blok(SynopIsblokNR).locoNR = SynopIsLocoNR
                        m_blok(SynopIsblokNR).status = True
                        m_blok(SynopIsblokNR).statusSynop = "B"
                    Else
                        status = "V"
                        m_blok(SynopIsblokNR).statusSynop = "V"
                    End If
                End SyncLock
                'geef de status van het blok door aan de sub BlokStatus(,,) van in dienst zijnde synoptieken op het scherm
                For j As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
                    If synopMenuItemInUse(j) Then synop(j).BlokStatus(status, SynopIsblokNR, SynopIsLocoNR)
                Next

            Case "Wissel in onderhoud zetten"
                Dim wisselNR As Integer = CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR)
                SyncLock syncOK
                    m_ArrayK83K84(wisselNR).status = True
                    wisselsInOH += "|" + Format(wisselNR, "000")
                    m_ArrayK83K84(wisselNR).statusSynop = "U"
                    'geef de status van het blok door aan de sub BlokStatus(,,) van in dienst zijnde synoptieken op het scherm
                    For j As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
                        If synopMenuItemInUse(j) Then synop(j).TurnoutStatus("U", synopGroup(synopNR).canvas(indexCanvas).direction, wisselNR.ToString)
                    Next
                End SyncLock

            Case "Wissel vrij zetten"
                Dim wisselNR As Integer = CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR)
                SyncLock syncOK
                    m_ArrayK83K84(wisselNR).status = False
                    m_ArrayK83K84(wisselNR).statusSynop = "I"
                    If wisselsInOH <> String.Empty Then
                        wisselsInOH = wisselsInOH.Replace("|" + Format(wisselNR, "000"), "")
                    End If
                    'geef de status van het blok door aan de sub BlokStatus(,,) van in dienst zijnde synoptieken op het scherm
                    For j As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
                        If synopMenuItemInUse(j) Then synop(j).TurnoutStatus("I", synopGroup(synopNR).canvas(indexCanvas).direction, wisselNR.ToString)
                    Next
                End SyncLock

            Case "Blok vrij zetten"
                SyncLock syncOK
                    SynopIsblokNR = CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR)
                    SynopIsLocoNR = CInt(synopGroup(synopNR).canvas(indexCanvas).locoNR)
                    'blokgegevens aanpassen
                    DCS.SetBlokGegevens(SynopIsblokNR, SynopIsLocoNR, 1)        'Blok VRIJ zetten
                End SyncLock
                'geef de status van het blok door aan de sub BlokStatus(,,) van in dienst zijnde synoptieken op het scherm
                For j As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
                    If synopMenuItemInUse(j) Then synop(j).BlokStatus("V", SynopIsblokNR, 0)
                Next

            Case "Informatie blok"  'ContextMenuItem 01
                Dim SynopIsBlokNR As Integer = CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR)
                If SynopIsBlokNR > 0 Then DCS.DisplayBlokData(1, SynopIsBlokNR) 'via raiseEvent HandlerLogData

            Case "Informatie trein"
                Dim locoNR As Integer = CInt(synopGroup(synopNR).canvas(indexCanvas).locoNR)
                If locoNR > 0 Then DCS.DisplayBlokData(2, locoNR) 'via raiseEvent HandlerLogData

            Case "Informatie reisweg Deel"
                Dim locoNR As Integer = CInt(synopGroup(synopNR).canvas(indexCanvas).locoNR)
                If locoNR > 0 Then DCS.DisplayBlokData(5, locoNR) 'via raiseEvent HandlerLogData

            Case "Informatie locomotief"
                Dim locoNR As Integer = CInt(synopGroup(synopNR).canvas(indexCanvas).locoNR)
                If locoNR > 0 Then DCS.DisplayBlokData(3, locoNR) 'via raiseEvent HandlerLogData

            Case "Informatie wissel"
                Dim wisselNR As Integer = CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR)
                If wisselNR > 0 Then DCS.DisplayBlokData(4, wisselNR) 'via raiseEvent HandlerLogDat

            Case "Informatie Synoptiek"
                display = "Informatie Synoptiek:" _
                                + vbNewLine + "strInfo= " + synopGroup(synopNR).canvas(indexCanvas).strInfo _
                                + vbNewLine + "itemNR= " + synopGroup(synopNR).canvas(indexCanvas).itemNR _
                                + vbNewLine + "elementNR= " + synopGroup(synopNR).canvas(indexCanvas).elementNR.ToString _
                                + vbNewLine + "Element Lengte= " + synopGroup(synopNR).canvas(indexCanvas).elementLenght.ToString _
                                + vbNewLine + "locoNr= " + synopGroup(synopNR).canvas(indexCanvas).locoNR _
                                + vbNewLine + "status= " + synopGroup(synopNR).canvas(indexCanvas).status _
                                + vbNewLine + "direction= " + synopGroup(synopNR).canvas(indexCanvas).direction _
                                + vbNewLine + "Pos X= " + synopGroup(synopNR).canvas(indexCanvas).posX.ToString _
                                + vbNewLine + "Pos Y= " + synopGroup(synopNR).canvas(indexCanvas).posY.ToString _
                                + vbNewLine + "Actief= " + synopGroup(synopNR).canvas(indexCanvas).active.ToString _
                                + vbNewLine + vbNewLine + "Synoptiek eigenschappen: " _
                                + vbNewLine + "SynoptiekNR= " + synopGroup(synopNR).Prop.synopNR.ToString _
                                + vbNewLine + "Grid maat= " + synopGroup(synopNR).Prop.gridStep.ToString _
                                + vbNewLine + "Voorstelling= " + synopGroup(synopNR).Prop.appearance.ToString _
                                + vbNewLine + "Titel= " + synopGroup(synopNR).Prop.tittle _
                                + vbNewLine + "Beschrijving= " + synopGroup(synopNR).Prop.labelText _
                                + vbNewLine + vbNewLine + "notes: " + vbNewLine + synopGroup(synopNR).canvas(indexCanvas).notes
                MessageBox.Show(display)
            Case Else
                'separatie items, geen actie
        End Select
    End Sub     ' events for all instances

#End Region

#Region "Subs and functions Toolstrip buttons"

    Private Sub ToolStripButton1_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripRepaint.Click
        Dim gr As Graphics = canvas.CreateGraphics
        RepaintCanvas(gr)
    End Sub

#End Region

#Region "Canvas synoptic paint en mouse Up en Down subs"

    Public Sub canvas_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles canvas.Paint
        If instellingen.gridlijnenTonen Then DrawGrid(e.Graphics) 'teken de grid
        DrawSynoptic(e.Graphics)        'teken de synoptiek
    End Sub

    Private Sub canvas_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles canvas.MouseDown
        'Eerst steeds alle contectmenu's onzichtbaar maken = hier de enige plaats waar het werkt
        For j As Integer = 0 To cm(synopNR).Items.Count - 1
            cm(synopNR).Items.Item(j).Visible = False
        Next
    End Sub

    Private Sub canvas_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles canvas.MouseUp
        ' Dim s As String = String.Empty
        Dim i, d1, d2, cX, cY As Integer
        cX = e.X \ gridStep
        cY = e.Y \ gridStep
        Dim xy As String = "#" & cX.ToString & "." & cY.ToString

        'onderzoek mouse click
        i = synopIndexStr(synopNR).IndexOf(xy)
        If i >= 0 Then   'enkel bij active cellen
            d1 = synopIndexStr(synopNR).IndexOf("|", i) + 1 : d2 = synopIndexStr(synopNR).IndexOf("|", d1)
            indexCanvas = CInt(synopIndexStr(synopNR).Substring(d1, d2 - d1))
            If synopGroup(synopNR).canvas(indexCanvas).elementNR < 100 Then        'only active cells
                Select Case e.Button
                    Case Windows.Forms.MouseButtons.Left
                        'enkel voor magnetische artikelen omschakelen
                        If synopGroup(synopNR).canvas(indexCanvas).elementNR > 29 AndAlso synopGroup(synopNR).canvas(indexCanvas).elementNR < 100 Then  'wissels
                            If CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR) > 0 Then
                                SyncLock syncOK
                                    If MDIParentfrm.VergrendeldeWisselsToolStripMenuItem.Checked Then
                                        If m_ArrayK83K84(CInt(synopGroup(synopNR).canvas(indexCanvas).itemNR)).status = False Then
                                            If synopGroup(synopNR).canvas(indexCanvas).direction = "1" Or synopGroup(synopNR).canvas(indexCanvas).direction = "*" Then
                                                synopGroup(synopNR).canvas(indexCanvas).direction = "0"
                                                data.address = synopGroup(synopNR).canvas(indexCanvas).itemNR
                                                data.port = "0"
                                            Else    'is = 0, zet nu op 1
                                                synopGroup(synopNR).canvas(indexCanvas).direction = "1"
                                                data.address = synopGroup(synopNR).canvas(indexCanvas).itemNR
                                                data.port = "1"
                                            End If
                                            DCS.SetTurnout(data)
                                            m_ArrayK83K84(CInt(data.address)).port = data.port
                                            m_ArrayK83K84(CInt(data.address)).status = True
                                            For j As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
                                                If synopMenuItemInUse(j) Then
                                                    synop(j).TurnoutStatus("M", synopGroup(synopNR).canvas(indexCanvas).direction, synopGroup(synopNR).canvas(indexCanvas).itemNR)
                                                End If
                                            Next
                                        Else
                                            Beep()
                                        End If
                                    Else
                                        If synopGroup(synopNR).canvas(indexCanvas).direction = "1" Or synopGroup(synopNR).canvas(indexCanvas).direction = "*" Then
                                            synopGroup(synopNR).canvas(indexCanvas).direction = "0"
                                            data.address = synopGroup(synopNR).canvas(indexCanvas).itemNR
                                            data.port = "0"
                                        Else    'is = 0, zet nu op 1
                                            synopGroup(synopNR).canvas(indexCanvas).direction = "1"
                                            data.address = synopGroup(synopNR).canvas(indexCanvas).itemNR
                                            data.port = "1"
                                        End If
                                        DCS.SetTurnout(data)
                                        m_ArrayK83K84(CInt(data.address)).port = data.port
                                        m_ArrayK83K84(CInt(data.address)).status = True
                                        For j As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
                                            If synopMenuItemInUse(j) Then
                                                synop(j).TurnoutStatus("M", synopGroup(synopNR).canvas(indexCanvas).direction, synopGroup(synopNR).canvas(indexCanvas).itemNR)
                                            End If
                                        Next
                                    End If
                                End SyncLock
                            End If
                        End If
                    Case Windows.Forms.MouseButtons.Right       ' synoptiek rechtermuis click acties ==============================

                        'enkel de zinvolle context menu's zichtbaar maken
                        Select Case synopGroup(synopNR).canvas(indexCanvas).elementNR
                            Case 1 To 2     'blokken
                                cm(synopNR).Items.Item(0).Visible = True
                                cm(synopNR).Items.Item(1).Visible = True
                                cm(synopNR).Items.Item(2).Visible = True
                                cm(synopNR).Items.Item(3).Visible = True
                                cm(synopNR).Items.Item(4).Visible = True
                                cm(synopNR).Items.Item(5).Visible = True
                                cm(synopNR).Items.Item(6).Visible = True
                                cm(synopNR).Items.Item(7).Visible = True
                                cm(synopNR).Items.Item(8).Visible = True
                                cm(synopNR).Items.Item(9).Visible = True
                                cm(synopNR).Items.Item(13).Visible = True
                                cm(synopNR).Items.Item(14).Visible = True
                                cm(synopNR).Items.Item(15).Visible = True
                                cm(synopNR).Items.Item(16).Visible = True
                                cm(synopNR).Items.Item(18).Visible = True
                            Case 29 To 100  'wissels
                                cm(synopNR).Items.Item(10).Visible = True
                                cm(synopNR).Items.Item(11).Visible = True
                                cm(synopNR).Items.Item(12).Visible = True
                                cm(synopNR).Items.Item(17).Visible = True
                                cm(synopNR).Items.Item(18).Visible = True
                            Case 23 To 24       'tekstvelden
                                cm(synopNR).Items.Item(18).Visible = True
                            Case Else
                                'niets zichtbaar maken
                        End Select
                        cm(synopNR).Show()
                    Case Windows.Forms.MouseButtons.Middle
                End Select
            End If
        End If

    End Sub

#End Region

#Region "Drawing and canvas setting subs"

    Private Sub DrawGrid(ByVal gr As Graphics)
        Dim nooflines As Integer
        Dim pbwidth As Integer
        Dim pbheight As Integer
        Dim distance As Integer = 0
        canvas.BackColor = Color.FromArgb(CInt(instellingen.ColorAchtergrondSynop))
        nooflines = CInt(canvas.Height / gridStep) 'calculating lines needed according to height  
        pbwidth = canvas.Width
        For i = 0 To nooflines  'not deceresing the lines to make it to the bottom  
            gr.DrawLine(gridPen, 0, distance, pbwidth, distance)
            distance += gridStep
        Next
        nooflines = CInt(canvas.Width / gridStep) 'calculating lines needed according to width  
        pbheight = canvas.Height
        distance = 0
        For i = 0 To nooflines  'not deceresing the lines to make it to the bottom  
            gr.DrawLine(gridPen, distance, 0, distance, pbheight)
            distance += gridStep
        Next
    End Sub

    Private Sub DrawSynoptic(ByVal gr As Graphics)
        'draw the synoptic elements on the canvas
        Dim nElements As Integer = synopGroup(synopNR).canvas.GetUpperBound(0)
        For i As Integer = 0 To nElements
            'draw element on canvas
            cv.status = synopGroup(synopNR).canvas(i).status
            cv.direction = synopGroup(synopNR).canvas(i).direction
            cv.gridStep = synopGroup(synopNR).Prop.gridStep
            cv.elementNR = synopGroup(synopNR).canvas(i).elementNR
            cv.elementLenght = synopGroup(synopNR).canvas(i).elementLenght
            cv.posX = synopGroup(synopNR).canvas(i).posX
            cv.posY = synopGroup(synopNR).canvas(i).posY
            cv.itemNR = synopGroup(synopNR).canvas(i).itemNR
            cv.locoNR = synopGroup(synopNR).canvas(i).locoNR
            cv.strInfo = synopGroup(synopNR).canvas(i).strInfo
            DrawElement(cv, gr)
        Next
    End Sub

    Private Sub DrawElement(ByVal cv As CanvasCells, ByVal gr As Graphics)

        'ter info
        'Public Structure CanvasCells
        '    Public posX As Integer
        '    Public posY As Integer
        '    Public active As Boolean       'voor rechtermuisclick= true
        '    Public gridStep As Integer
        '    Public elementNR As Integer
        '    Public blokNR As String
        '    Public locoNR As String
        '    Public elementLenght As Integer
        '    Public status As String
        '    Public direction As String
        '    Public strInfo As String
        'End Structure

        'frequently used combinations
        Dim gridUnit As Integer = cv.gridStep \ 2
        Dim HGridunit As Integer = (10 * cv.gridStep) \ 23
        X1 = cv.posX * cv.gridStep
        Y1 = cv.posY * cv.gridStep
        XU = X1 + gridUnit
        XS = X1 + cv.gridStep
        YU = Y1 + gridUnit
        YS = Y1 + gridStep
        X1H = X1 + HGridunit
        XSH = XS - HGridunit
        Y1H = Y1 + HGridunit
        YSH = YS - HGridunit

        Select Case cv.status
            Case "V"      'Vrij
                elementPenStatus.Color = colorFre
                blockFillBrush.Color = colorFre
            Case "G"     'Gereserveerd
                elementPenStatus.Color = colorReserved
                blockFillBrush.Color = colorReserved
            Case "B"     'Bezet
                elementPenStatus.Color = colorOccupied
                blockFillBrush.Color = colorOccupied
            Case "A"     'AfremBezet
                elementPenStatus.Color = colorAfremOccupied
                blockFillBrush.Color = colorAfremOccupied
            Case "R"     'Retrigger
                elementPenStatus.Color = colorRetrigger
                blockFillBrush.Color = colorRetrigger
            Case "H"     'Halt
                elementPenStatus.Color = colorHalt
                blockFillBrush.Color = colorHalt
            Case "U"     'Onderhoud
                elementPenStatus.Color = colormaintenance
                blockFillBrush.Color = colormaintenance
            Case "L"     'Locked = vergrendeld
                elementPenStatus.Color = colorLocked
                blockFillBrush.Color = colorLocked
            Case "Q"     'Prereserved
                elementPenStatus.Color = colorPreReserved
                blockFillBrush.Color = colorPreReserved
            Case "S"      'Spook
                elementPenStatus.Color = colorError
                blockFillBrush.Color = colorError
            Case "§"     'HighLite
                elementPenStatus.Color = colorlocatieTrein
                blockFillBrush.Color = colorlocatieTrein
            Case "M"     'Handbediening
                elementPenStatus.Color = colorHandbediening
                blockFillBrush.Color = colorHandbediening
            Case "P"     'Passief
                elementPenStatus.Color = colorPassief
                blockFillBrush.Color = colorPassief
            Case "C"     'Initblok
                elementPenStatus.Color = colorInitBlok
                blockFillBrush.Color = colorInitBlok
            Case Else   ' *
                If cv.elementNR > 100 And cv.elementNR < 200 Then   'passieve railelementen
                    elementPenStatus.Color = colorPassief
                    blockFillBrush.Color = colorPassief
                Else
                    elementPenStatus.Color = colorInit
                    blockFillBrush.Color = colorInit
                End If
        End Select

        Select Case cv.elementNR
            'block elements
            Case 1
                If cv.locoNR = "0" Then cv.locoNR = String.Empty
                gr.FillRectangle(blockBackgroundBrush, X1, Y1, gridStep * cv.elementLenght, gridStep)
                gr.FillRectangle(blockFillBrush, X1, Y1, gridStep, gridStep)
                gr.DrawRectangle(blockPen, X1, Y1, gridStep * cv.elementLenght, gridStep)
                gr.DrawLine(blockPen, XS, Y1, XS, YS)
                gr.DrawString(cv.itemNR + "." + cv.locoNR, blockFont, blockBrush, XS, Y1 + gridText)

            Case 2  'vertical block
                If cv.locoNR = "0" Then cv.locoNR = String.Empty
                gr.FillRectangle(blockBackgroundBrush, X1, Y1, gridStep, gridStep * cv.elementLenght)
                gr.FillRectangle(blockFillBrush, X1, Y1, gridStep, gridStep)
                gr.DrawRectangle(blockPen, X1, Y1, gridStep, gridStep * cv.elementLenght)
                gr.DrawLine(blockPen, X1, YS, XS, YS)
                gr.DrawString(cv.itemNR + "." + cv.locoNR, blockFont, blockBrush, X1, YS, blockStrFormat)

                'text elements
            Case 23  'horizontal text
                gr.FillRectangle(blockBackgroundBrush, X1, Y1, gridStep * cv.elementLenght, gridStep)
                If cv.strInfo.Substring(0, 1) = "$" Then
                    gr.FillRectangle(TextFillBrush, X1, Y1, gridStep, gridStep)
                Else
                    gr.FillRectangle(Brushes.Yellow, X1, Y1, gridStep, gridStep)
                End If
                gr.DrawRectangle(textPen, X1, Y1, gridStep * cv.elementLenght, gridStep)
                gr.DrawString(cv.strInfo.Substring(1), textFont, textBrush, X1, Y1 + gridText)
            Case 24  'vertical text
                gr.FillRectangle(blockBackgroundBrush, X1, Y1, gridStep, gridStep * cv.elementLenght)
                If cv.strInfo.Substring(0, 1) = "$" Then
                    gr.FillRectangle(TextFillBrush, X1, Y1, gridStep, gridStep)
                Else
                    gr.FillRectangle(Brushes.Yellow, X1, Y1, gridStep, gridStep)
                End If
                gr.DrawRectangle(textPen, X1, Y1, gridStep, gridStep * cv.elementLenght)
                gr.DrawString(cv.strInfo.Substring(1), textFont, textBrush, X1, Y1, blockStrFormat)

                'engelse wissels
            Case 33  'kruising vertikaal, links boven naar rechts onder
                Select Case cv.direction
                    Case "0"
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                        gr.DrawLine(balPen, X1H, Y1H, XSH, YSH)

                    Case "1"
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case Else
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                End Select
            Case 34  'kruising vertikaal, links onder naar rechts boven
                Select Case cv.direction
                    Case "0"
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                        gr.DrawLine(balPen, X1H, Y1H, XSH, YSH)
                    Case "1"
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case Else
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                End Select
            Case 35  'kruising horizontaal, links onder naar rechts boven
                Select Case cv.direction
                    Case "0"
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                        gr.DrawLine(balPen, X1H, Y1H, XSH, YSH)

                    Case "1"
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                    Case Else
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                End Select
            Case 36  'kruising horizontaal, links boven naar rechts onder
                Select Case cv.direction
                    Case "0"
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                        gr.DrawLine(balPen, X1H, Y1H, XSH, YSH)
                    Case "1"
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                    Case Else
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                End Select

                'Wissels
            Case 42  'wissel kort horizontaal arm links naar boven
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, X1, YU, XU, Y1)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "0"
                        gr.DrawLine(passiefPen, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, YU, XU, Y1)
                    Case Else
                        Dim points As Point() = {New Point(XS, YU), New Point(X1, YU), _
                      New Point(XU, Y1)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 43  'wissel kort horizontaal arm links naar beneden
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, X1, YU, XU, YS)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "0"
                        gr.DrawLine(passiefPen, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, YU, XU, YS)
                    Case Else
                        Dim points As Point() = {New Point(XS, YU), New Point(X1, YU), _
                      New Point(XU, YS)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 44  'wissel kort horizontaal arm rechts naar boven
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, XU, Y1, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "0"
                        gr.DrawLine(passiefPen, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, XU, Y1, XS, YU)
                    Case Else
                        Dim points As Point() = {New Point(X1, YU), New Point(XS, YU), _
                      New Point(XU, Y1)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 45  'wissel kort horizontaal arm rechts naar beneden
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, XS, YU, XU, YS)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "0"
                        gr.DrawLine(passiefPen, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, XS, YU, XU, YS)
                    Case Else
                        Dim points As Point() = {New Point(X1, YU), New Point(XS, YU), _
                      New Point(XU, YS)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 46  'wissel kort vertikaal arm links onderaan
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, X1, YU, XU, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)

                    Case "0"
                        gr.DrawLine(passiefPen, XU, Y1, XU, YS)
                        gr.DrawLine(elementPenStatus, X1, YU, XU, YS)
                    Case Else
                        Dim points As Point() = {New Point(XU, Y1), New Point(XU, YS), _
                      New Point(X1, YU)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 47  'wissel kort vertikaal arm rechts onderaan
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, XS, YU, XU, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)

                    Case "0"
                        gr.DrawLine(passiefPen, XU, Y1, XU, YS)
                        gr.DrawLine(elementPenStatus, XS, YU, XU, YS)
                    Case Else
                        Dim points As Point() = {New Point(XU, Y1), New Point(XU, YS), _
                      New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 48  'wissel kort vertikaal arm links onderaan
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, X1, YU, XU, Y1)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)

                    Case "0"
                        gr.DrawLine(passiefPen, XU, Y1, XU, YS)
                        gr.DrawLine(elementPenStatus, X1, YU, XU, Y1)
                    Case Else
                        Dim points As Point() = {New Point(X1, YU), New Point(XU, Y1), _
                      New Point(XU, YS)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 49  'wissel kort vertikaal arm rechts bovenaan
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, XU, Y1, XS, YU)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)

                    Case "0"
                        gr.DrawLine(passiefPen, XU, Y1, XU, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XS, YU)
                    Case Else
                        Dim points As Point() = {New Point(XU, YS), New Point(XU, Y1), _
                      New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)
                End Select

            Case 64  'wissel lang horizontaal arm links naar boven
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, XU, YU, XS, Y1)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "0"
                        gr.DrawLine(passiefPen, XU, YU, XS, YU)
                        Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                        New Point(XS, Y1)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(XS, Y1), New Point(XU, YU), _
                        New Point(X1, YU), New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 65  'wissel lang horizontaal arm links naar beneden
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, XU, YU, XS, YS)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                    Case "0"
                        gr.DrawLine(passiefPen, XU, YU, XS, YU)
                        Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                         New Point(XS, YS)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(XS, YS), New Point(XU, YU), _
                        New Point(X1, YU), New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 66  'wissel lang horizontaal arm links naar boven
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, X1, Y1, XU, YU)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "0"
                        gr.DrawLine(passiefPen, X1, YU, XU, YU)
                        Dim points As Point() = {New Point(X1, Y1), New Point(XU, YU), _
                        New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, Y1), New Point(XU, YU), _
                        New Point(X1, YU), New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 67  'wissel lang horizontaal arm links naar beneden
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, X1, YS, XU, YU)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                    Case "0"
                        gr.DrawLine(passiefPen, X1, YU, XU, YU)
                        Dim points As Point() = {New Point(X1, YS), New Point(XU, YU), _
                         New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, YS), New Point(XU, YU), _
                        New Point(X1, YU), New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 68  'wissel lang vertikaal arm links naar boven
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, X1, Y1, XU, YU)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case "0"
                        gr.DrawLine(passiefPen, XU, Y1, XU, YU)
                        Dim points As Point() = {New Point(X1, Y1), New Point(XU, YU), _
                         New Point(XU, YS)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, Y1), New Point(XU, YU), _
                        New Point(XU, Y1), New Point(XU, YS)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 69  'wissel lang vertikaal arm rechts naar boven
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, XS, Y1, XU, YU)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case "0"
                        gr.DrawLine(passiefPen, XU, Y1, XU, YU)
                        Dim points As Point() = {New Point(XS, Y1), New Point(XU, YU), _
                         New Point(XU, YS)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(XS, Y1), New Point(XU, YU), _
                        New Point(XU, Y1), New Point(XU, YS)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 70  'wissel lang vertikaal arm rechts naar boven
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, X1, YS, XU, YU)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case "0"
                        gr.DrawLine(passiefPen, XU, YU, XU, YS)
                        Dim points As Point() = {New Point(XU, Y1), New Point(XU, YU), _
                         New Point(X1, YS)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, YS), New Point(XU, YU), _
                        New Point(XU, Y1), New Point(XU, YS)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 71  'wissel lang vertikaal arm rechts naar beneden
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, XU, YU, XS, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case "0"
                        gr.DrawLine(passiefPen, XU, YU, XU, YS)
                        Dim points As Point() = {New Point(XU, Y1), New Point(XU, YU), _
                         New Point(XS, YS)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(XS, YS), New Point(XU, YU), _
                        New Point(XU, Y1), New Point(XU, YS)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 72  'wissel lang diagonaal arm rechts naar midden
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, XU, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                    Case "0"
                        gr.DrawLine(passiefPen, XU, YU, XS, Y1)
                        Dim points As Point() = {New Point(X1, YS), New Point(XU, YU), _
                         New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, YS), New Point(XS, Y1), _
                        New Point(XU, YU), New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 73  'wissel lang diagonaal arm links naar midden
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, X1, YU, XU, YU)
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                    Case "0"
                        gr.DrawLine(passiefPen, X1, Y1, XU, YU)
                        Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                         New Point(XS, YS)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                        New Point(X1, Y1), New Point(XS, YS)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 74  'wissel lang diagonaal arm rechts naar midden
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, XU, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                    Case "0"
                        gr.DrawLine(passiefPen, XU, YU, XS, YS)
                        Dim points As Point() = {New Point(X1, Y1), New Point(XU, YU), _
                         New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, Y1), New Point(XS, YS), _
                        New Point(XU, YU), New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)
                End Select
            Case 75  'wissel lang diagonaal arm rechts naar midden
                Select Case cv.direction
                    Case "1"
                        gr.DrawLine(passiefPen, X1, YU, XU, YU)
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                    Case "0"
                        gr.DrawLine(passiefPen, X1, YS, XU, YU)
                        Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                         New Point(XS, Y1)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, YS), New Point(XS, Y1), _
                        New Point(XU, YU), New Point(X1, YU)}
                        gr.DrawLines(elementPenStatus, points)
                End Select

                'blanco
            Case 100      'simply blanc
                gr.FillRectangle(blancoBrush, X1 + 1, Y1 + 1, gridStep - 1, gridStep - 1)
                gr.DrawRectangle(gridPen, X1, Y1, gridStep, gridStep)

                'rechte sporen
            Case 111  'rechtspoor horizontaal
                gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
            Case 112  'rechtspoor vertikaal
                gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
            Case 113  'rechtspoor diagonaal van linksonder naar rechtsboven
                gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
            Case 114  'rechtspoor diagonaal van linksboven naar rechtsonder
                gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
            Case 115  'rechtspoor schuine hoek links boven
                elementPenStatus.EndCap = LineCap.Round
                elementPenStatus.StartCap = LineCap.Round
                gr.DrawLine(elementPenStatus, X1, YU, XU, Y1)
                elementPenStatus.StartCap = LineCap.Flat    'undo penchanges
                elementPenStatus.EndCap = LineCap.Flat      'undo penchanges
            Case 116  'rechtspoor schuine hoek rechts boven
                elementPenStatus.EndCap = LineCap.Round
                elementPenStatus.StartCap = LineCap.Round
                gr.DrawLine(elementPenStatus, XU, Y1, XS, YU)
                elementPenStatus.StartCap = LineCap.Flat    'undo penchanges
                elementPenStatus.EndCap = LineCap.Flat      'undo penchanges
            Case 117  'rechtspoor schuine hoek rechts onder
                elementPenStatus.EndCap = LineCap.Round
                elementPenStatus.StartCap = LineCap.Round
                gr.DrawLine(elementPenStatus, XU, YS, XS, YU)
                elementPenStatus.StartCap = LineCap.Flat    'undo penchanges
                elementPenStatus.EndCap = LineCap.Flat      'undo penchanges
            Case 118  'rechtspoor schuine hoek links onder
                elementPenStatus.EndCap = LineCap.Round
                elementPenStatus.StartCap = LineCap.Round
                gr.DrawLine(elementPenStatus, X1, YU, XU, YS)
                elementPenStatus.StartCap = LineCap.Flat    'undo penchanges
                elementPenStatus.EndCap = LineCap.Flat      'undo penchanges

                'Gebogen sporen
            Case 119  'gebogenspoor vertikaal naar boven links
                Dim points As Point() = {New Point(XU, YS), New Point(XU, YU), _
                New Point(X1, Y1)}
                gr.DrawLines(elementPenStatus, points)
            Case 120  'gebogenspoor vertikaal naar boven rechts
                Dim points As Point() = {New Point(XU, YS), New Point(XU, YU), _
                New Point(XS, Y1)}
                gr.DrawLines(elementPenStatus, points)
            Case 121  'gebogenspoor vertikaal naar beneden links
                Dim points As Point() = {New Point(XU, Y1), New Point(XU, YU), _
                New Point(X1, YS)}
                gr.DrawLines(elementPenStatus, points)
            Case 122  'gebogenspoor vertikaal naar beneden rechts
                Dim points As Point() = {New Point(XU, Y1), New Point(XU, YU), _
                New Point(XS, YS)}
                gr.DrawLines(elementPenStatus, points)
            Case 123  'gebogenspoor horizontaal links onder naar  midden
                Dim points As Point() = {New Point(X1, YS), New Point(XU, YU), _
                New Point(XS, YU)}
                gr.DrawLines(elementPenStatus, points)
            Case 124  'gebogenspoor horizontaal links boven naar midden
                Dim points As Point() = {New Point(X1, Y1), New Point(XU, YU), _
                New Point(XS, YU)}
                gr.DrawLines(elementPenStatus, points)
            Case 125  'gebogenspoor links horizontaal naar recht onder
                Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                New Point(XS, YS)}
                gr.DrawLines(elementPenStatus, points)
            Case 126  'gebogenspoor links horizontaal naar recht boven
                Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                New Point(XS, Y1)}
                gr.DrawLines(elementPenStatus, points)
            Case 127  'gebogenspoor horizontaal rechts naar vertikaal beneden
                Dim points As Point() = {New Point(XS, YU), New Point(XU, YU), _
                New Point(XU, YS)}
                gr.DrawLines(elementPenStatus, points)
            Case 128  'gebogenspoor horizontaal rechts naar vertikaal boven
                Dim points As Point() = {New Point(XU, Y1), New Point(XU, YU), _
                New Point(XS, YU)}
                gr.DrawLines(elementPenStatus, points)
            Case 129  'gebogenspoor horizontaal links naar vertikaal beneden
                Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                New Point(XU, Y1)}
                gr.DrawLines(elementPenStatus, points)
            Case 130  'gebogenspoor horizontaal links naar vertikaal boven
                Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                New Point(XU, YS)}
                gr.DrawLines(elementPenStatus, points)

                'Buffers
            Case 131  'buffer horizontal links
                gr.DrawLine(elementPenStatus, X1, YU, XU, YU)
                gr.DrawLine(elementPen, XU, YU - gridUnit \ 2, XU, YU + gridUnit \ 2)
            Case 132  'buffer horizontal links
                gr.DrawLine(elementPenStatus, XU, Y1, XU, YU)
                gr.DrawLine(elementPen, XU - gridUnit \ 2, YU, XU + gridUnit \ 2, YU)
            Case 133  'buffer horizontal links
                gr.DrawLine(elementPenStatus, XU, YU, XS, YU)
                gr.DrawLine(elementPen, XU, YU - gridUnit \ 2, XU, YU + gridUnit \ 2)
            Case 134  'buffer horizontal links
                gr.DrawLine(elementPenStatus, XU, YS, XU, YU)
                gr.DrawLine(elementPen, XU - gridUnit \ 2, YU, XU + gridUnit \ 2, YU)

                'Special building elements
            Case 180     'horizontal under bridge
                gr.DrawLine(passiefPen, XU, Y1, XU, YS)
                gr.DrawLine(elementPenStatus, X1, YU, XU - gridUnit \ 3, YU)
                gr.DrawLine(elementPenStatus, XU + gridUnit \ 3, YU, XS, YU)
            Case 181     'vertical under bridge
                gr.DrawLine(passiefPen, X1, YU, XS, YU)
                gr.DrawLine(elementPenStatus, XU, Y1, XU, YU - gridUnit \ 3)
                gr.DrawLine(elementPenStatus, XU, YU + gridUnit \ 3, XU, YS)
            Case 182     'left to right diagonal under bridge
                gr.DrawLine(passiefPen, X1, YS, XS, Y1)
                gr.DrawLine(elementPenStatus, X1, Y1, XU - gridUnit \ 3, YU - gridUnit \ 3)
                gr.DrawLine(elementPenStatus, XU + gridUnit \ 3, YU + gridUnit \ 3, XS, YS)
            Case 183     'right to left diagonal under bridge
                gr.DrawLine(passiefPen, X1, Y1, XS, YS)
                gr.DrawLine(elementPenStatus, X1, YS, XU - gridUnit \ 3, YU + gridUnit \ 3)
                gr.DrawLine(elementPenStatus, XU + gridUnit \ 3, YU - gridUnit \ 3, XS, Y1)
            Case 184  'kruising andreus
                gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
            Case 185  'kruising horizontaal, vertikaal
                gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
            Case Else
                ' number not in use
        End Select
    End Sub

    Private Sub RepaintCanvas(ByVal gr As Graphics)
        Dim i, blokNR, locoNR, wisselNR As Integer
        Dim strBlokNR As String = String.Empty
        Dim strWisselNR As String = String.Empty
        'repaint
        gr.Clear(Color.White)
        If instellingen.gridlijnenTonen Then DrawGrid(gr) 'teken de grid
        'draw the synoptic elements on the canvas
        Dim nElements As Integer = synopGroup(synopNR).canvas.GetUpperBound(0)
        For i = 0 To nElements
            Select Case synopGroup(synopNR).canvas(i).elementNR

                Case Is < 3        'blok element
                    locoNR = CInt(synopGroup(synopNR).canvas(i).locoNR)
                    blokNR = CInt(synopGroup(synopNR).canvas(i).itemNR)
                    strBlokNR = (blokNR + 1000).ToString.Substring(1)
                    If instellingen.OHblokken.Contains(strBlokNR) Then
                        synopGroup(synopNR).canvas(i).status = "U"
                        cv.status = synopGroup(synopNR).canvas(i).status
                    ElseIf locoNR > 0 Then
                        synopGroup(synopNR).canvas(i).status = m_blok(blokNR).statusSynop
                        cv.status = synopGroup(synopNR).canvas(i).status
                    Else
                        synopGroup(synopNR).canvas(i).status = "V"
                        cv.status = synopGroup(synopNR).canvas(i).status
                    End If
                Case 30 To 99    ' k83K84 elementen zoals wissels
                    wisselNR = CInt(synopGroup(synopNR).canvas(i).itemNR)
                    strWisselNR = (wisselNR + 1000).ToString.Substring(1)
                    If wisselsInOH.Contains(strWisselNR) Then
                        synopGroup(synopNR).canvas(i).status = "U"
                        cv.status = synopGroup(synopNR).canvas(i).status
                    Else
                        cv.status = synopGroup(synopNR).canvas(i).status
                    End If
                Case Else   'neutrale elementen
                    cv.status = synopGroup(synopNR).canvas(i).status
            End Select
            cv.direction = synopGroup(synopNR).canvas(i).direction
            cv.gridStep = synopGroup(synopNR).Prop.gridStep
            cv.elementNR = synopGroup(synopNR).canvas(i).elementNR
            cv.elementLenght = synopGroup(synopNR).canvas(i).elementLenght
            cv.posX = synopGroup(synopNR).canvas(i).posX
            cv.posY = synopGroup(synopNR).canvas(i).posY
            cv.itemNR = synopGroup(synopNR).canvas(i).itemNR
            cv.locoNR = synopGroup(synopNR).canvas(i).locoNR
            cv.strInfo = synopGroup(synopNR).canvas(i).strInfo      '  <12345>
            DrawElement(cv, gr)
        Next
    End Sub

#End Region

#Region "Blokstatus gegevens afkomstig van dicostation via MdiParentFrm handlerChangeBlok"

    Public Sub BlokStatus(ByVal status As String, ByVal blokNR As Integer, ByVal locoNR As Integer)
        Dim cv As New CanvasCells
        Dim gr As Graphics = canvas.CreateGraphics
        'voor alle bestaande synop canvassen update er de nieuwe status en locoNR
        For i As Integer = 0 To synopGroup(synopNR).canvas.GetUpperBound(0) 'voor alle elementen
            'is blokNR aanwezig?
            If CInt(synopGroup(synopNR).canvas(i).itemNR) = blokNR Then
                If synopGroup(synopNR).canvas(i).elementNR > 0 And synopGroup(synopNR).canvas(i).elementNR < 3 OrElse _
                   (instellingen.showRailElements AndAlso (synopGroup(synopNR).canvas(i).elementNR > 100 And synopGroup(synopNR).canvas(i).elementNR < 200)) Then
                    synopGroup(synopNR).canvas(i).status = status
                    synopGroup(synopNR).canvas(i).locoNR = CStr(locoNR)
                    'teken de gewijzigde blokstatus op de zichtbare canvas
                    If Me.canvas.Visible Then
                        'voorbereiding DrawElement
                        cv.status = status : cv.itemNR = CStr(blokNR) : cv.locoNR = CStr(locoNR)
                        cv.gridStep = synopGroup(synopNR).Prop.gridStep
                        cv.direction = synopGroup(synopNR).canvas(i).direction
                        cv.elementLenght = synopGroup(synopNR).canvas(i).elementLenght
                        cv.elementNR = synopGroup(synopNR).canvas(i).elementNR
                        cv.strInfo = synopGroup(synopNR).canvas(i).strInfo
                        cv.posX = synopGroup(synopNR).canvas(i).posX
                        cv.posY = synopGroup(synopNR).canvas(i).posY
                        DrawElement(cv, gr)
                    End If
                End If
            End If
        Next
        gr.Dispose()
    End Sub

    Public Sub TurnoutStatus(ByVal status As String, ByVal direction As String, ByVal address As String)
        Dim cv As New CanvasCells
        Dim gr As Graphics = canvas.CreateGraphics
        'voor alle bestaande synop canvassen update er de nieuwe status en locoNR
        For i As Integer = 0 To synopGroup(synopNR).canvas.GetUpperBound(0) 'voor alle elementen
            'is wissel aanwezig?
            If synopGroup(synopNR).canvas(i).itemNR = address Then
                If synopGroup(synopNR).canvas(i).elementNR > 29 And synopGroup(synopNR).canvas(i).elementNR < 100 Then
                    synopGroup(synopNR).canvas(i).status = status
                    synopGroup(synopNR).canvas(i).direction = direction
                    'teken de gewijzigde wisselstatus op de zichtbare canvas
                    If Me.canvas.Visible Then
                        'voorbereiding DrawElement
                        cv.status = status : cv.itemNR = address : cv.direction = direction
                        cv.gridStep = synopGroup(synopNR).Prop.gridStep
                        cv.elementLenght = synopGroup(synopNR).canvas(i).elementLenght
                        cv.elementNR = synopGroup(synopNR).canvas(i).elementNR
                        cv.posX = synopGroup(synopNR).canvas(i).posX
                        cv.posY = synopGroup(synopNR).canvas(i).posY
                        DrawElement(cv, gr)
                    End If
                End If
            End If
        Next
        gr.Dispose()
    End Sub

    Public Sub SetGridStep(ByVal NR As Integer)
        If Me.Text.Length <> 0 Then
            If synopNR = NR Then
                'If CInt(Me.Text.Substring(0, 3)) = NR Then
                synopNR = NR
                Dim gr As Graphics = canvas.CreateGraphics
                SetCanvasVariables()
                DrawGrid(gr)
                canvas.Refresh()
            End If
        End If
    End Sub

#End Region

End Class
