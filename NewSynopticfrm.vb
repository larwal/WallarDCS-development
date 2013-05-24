
Imports System.Threading.Thread
Imports System.Drawing.Drawing2D
Imports System.IO


Public Class NewSynopticfrm

#Region "Initial Vars"

    'class variables
    'zie ook mainmodule clipboard
    Private switch As Boolean
    Private selectON As Boolean = False
    Private selectActive As Boolean = False
    Private editMode As EditModes
    Private selectStart As SelectXY
    Private selectEnd As SelectXY
    Private gridStartX As Integer
    Private gridStartY As Integer

    'Class variables
    Private canvasNew(,) As CanvasCells
    Private synopActiveDone As Boolean
    Private firstEntry As Boolean
    Private canvasH As Integer
    Private canvasW As Integer
    Private gridStep As Integer = 20  'determines the space between lines  
    Private gridUnit As Integer
    Private gridText As Integer

    Private colorFre As Color = Color.LightGreen
    Private colorReserved As Color = Color.Orange
    Private colorOccupied As Color = Color.Blue
    Private colormaintenance As Color = Color.Magenta
    Private colorError As Color = Color.Red

    Private gridPen As New Pen(Color.LightBlue, 1)
    Private elementPen As New Pen(Color.Gray, 1)
    Private elementPenFre As New Pen(colorFre, 1)
    Private elementPenStatus As New Pen(Color.Black, 1)
    Private selectPen As New Pen(Color.Black, 2)
    Private blockPen As Pen
    Private blockFont As Font
    Private textFont As Font
    Private blockBrush As Brush
    Private textBrush As Brush
    Private textPen As New Pen(Color.DarkBlue, 1)
    Private blancoBrush As Brush
    Private blockFillBrush As Brush
    Private blockStrFormat As New StringFormat

    'dialog forms
    Private cm As NewSynopticContext

    'draw coordinaten in DrawElements
    Private X1, Y1, XU, XS, YU, YS As Integer   'used only in Sub DrawElement
    Private canvasArray(8192) As CanvasCells    'bevat alle synops
    Private cv As CanvasCells
    Private ImportNewBaseX As Integer = 0
    Private importNewBaseY As Integer = 0
#End Region

#Region "form Load & close , var.settings, Open and save data"

    Private Sub NewSynopticfrm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        MDIParentfrm.EditMenu.Visible = False
        Me.Dispose()
    End Sub

    Private Sub NewSynopticfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.ResizeRedraw = True
        EnableDoubleBuffering()
        SetVariabelen()
        If ClipboardActive Then tsBtnImport.Enabled = True
    End Sub

    Public Sub EnableDoubleBuffering()
        ' Set the value of the double-buffering style bits to true.
        Me.SetStyle(ControlStyles.DoubleBuffer _
          Or ControlStyles.UserPaint _
          Or ControlStyles.AllPaintingInWmPaint, _
          True)
        Me.UpdateStyles()
    End Sub

    Private Sub SetVariabelen()

        'Setting variabelen voor synoptic grid  ===================
        gridStep = canvasProperty.gridStep
        gridUnit = gridStep \ canvasValue.setGridUnit
        gridText = gridStep \ canvasValue.setGridText
        gridPen.Color = Color.LightBlue
        gridPen.Width = 1

        'diktes pennen
        elementPen.Width = gridStep \ canvasValue.setPenWidth
        elementPenFre.Width = gridStep \ canvasValue.setPenWidth
        elementPenStatus.Width = gridStep \ canvasValue.setPenWidth
        elementPenStatus.LineJoin = Drawing2D.LineJoin.Round
        selectPen.DashStyle = DashStyle.Dash
        'block properties
        blockLenght = 3
        blockFont = New Font("Arial", gridUnit)
        textFont = New Font("Verdana", gridUnit)
        blockBrush = Brushes.Red
        blockFillBrush = Brushes.LightGray
        blockPen = Pens.Red
        blockStrFormat.FormatFlags = StringFormatFlags.DirectionVertical
        'blanco
        textBrush = Brushes.Blue
        textPen = Pens.DarkBlue
        blancoBrush = Brushes.White

        'set other vars
        If synopActiveNR = -1 Then
            Me.Text = "Nieuwe synoptiek "
        Else
            Me.Text = "Te wijzigen synoptiek " & canvasProperty.tittle
            switch = True
        End If
        Me.ToolStripLabel.Text = canvasProperty.labelText
        ' enable and disable buttons
        tsbtnSelect.Enabled = True : tsbtnCopy.Enabled = False : tsbtnCut.Enabled = False : tsbtnDelete.Enabled = False
        tsbtnUndo.Enabled = False : tsbtnRedo.Enabled = False : tsbtnPaste.Enabled = False

    End Sub

    Public Sub SaveSynoptic(ByVal strProp As String, ByVal strCanvas As String)
        Try
            Dim uBound As Integer = strSynopGroup.GetUpperBound(0) + 1
            If synopActiveDone Then  'edited synop
                strSynopGroup(synopActiveNR + 1) = strProp & "#" & strCanvas      'aanpassen aan stringcanvasGroup() index
                uBound -= 1  'terugstellen ubound
            Else    'new synop
                Dim str As String = String.Empty
                'wat is er reeds aanwezig in strcanvasGroup()
                'de eerste lijn ( index =0) bevat het aantal synoplijnen, verhoog met 1
                ReDim Preserve strSynopGroup(uBound)   'pas array grootte aan
                strSynopGroup(0) = (uBound).ToString 'aantal synoptics= uBound min eerste lijn
                strSynopGroup(uBound) = strProp & "#" & strCanvas
            End If
            Dim myStreamWriter As StreamWriter
            myStreamWriter = File.CreateText(DataDir & "\Synoptics.txt")
            For i As Integer = 0 To uBound
                myStreamWriter.WriteLine(strSynopGroup(i))
            Next
            myStreamWriter.Close()
            myStreamWriter = Nothing
            myStreamWriter = Nothing
        Catch exc As Exception
            MessageBox.Show("Fout in wegschrijven van Synoptics.txt " & exc.Message)
        End Try
    End Sub

#End Region

#Region "Paint, mouse, tooolstrip buttons actions "

    Private Sub pnlCanvas_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pnlCanvas.MouseDown

        Dim cm = New NewSynopticContext
        Dim g As Graphics = pnlCanvas.CreateGraphics()
        Dim i, j As Integer
        Dim x As Integer = e.X \ gridStep
        If x > canvasW Then Exit Sub
        Dim y As Integer = e.Y \ gridStep
        If y > canvasH Then Exit Sub
        ' Dim rt 
        Try
            If tsBtnImport.Enabled Then
                editMode = EditModes.Import
                selectActive = True
                tsBtnImport.Enabled = False
            End If

            If selectActive Then    'selectmode
                Select Case e.Button
                    Case Windows.Forms.MouseButtons.Left
                        Select Case editMode
                            Case EditModes.selekt '========================================= select ===================
                                If Not selectON Then
                                    selectON = True
                                    gridStartX = x
                                    gridStartY = y
                                    selectStart.x = x * gridStep
                                    selectStart.y = y * gridStep
                                    g.DrawRectangle(selectPen, selectStart.x, selectStart.y, gridStep, gridStep)
                                Else    'select end 
                                    'Cursor = Cursors.Hand
                                    selectON = False
                                    selectEnd.x = x * gridStep + gridStep
                                    selectEnd.y = y * gridStep + gridStep
                                    g.DrawRectangle(selectPen, selectStart.x, selectStart.y, selectEnd.x - selectStart.x, selectEnd.y - selectStart.y)

                                    'opvullen van SynopClipboard enkel met elementcellen
                                    Dim coordX As Integer = ((selectEnd.x - selectStart.x) \ gridStep) - 1
                                    Dim coordY As Integer = ((selectEnd.y - selectStart.y) \ gridStep) - 1
                                    If coordX < 0 Or coordY < 0 Then Exit Select
                                    ReDim SynopClipboard(coordX, coordY)
                                    ReDim UndoClipboard(coordX, coordY)
                                    ReDim redoClipboard(coordX, coordY)
                                    For j = 0 To coordY
                                        For i = 0 To coordX
                                            SynopClipboard(i, j).posX = gridStartX + i
                                            SynopClipboard(i, j).posY = gridStartY + j
                                            SynopClipboard(i, j).itemNR = canvasNew(i + gridStartX, j + gridStartY).itemNR
                                            SynopClipboard(i, j).direction = canvasNew(i + gridStartX, j + gridStartY).direction
                                            SynopClipboard(i, j).elementNR = canvasNew(i + gridStartX, j + gridStartY).elementNR
                                            SynopClipboard(i, j).elementLenght = canvasNew(i + gridStartX, j + gridStartY).elementLenght
                                            SynopClipboard(i, j).strInfo = canvasNew(i + gridStartX, j + gridStartY).strInfo
                                            SynopClipboard(i, j).notes = canvasNew(i + gridStartX, j + gridStartY).notes
                                            SynopClipboard(i, j).status = canvasNew(i + gridStartX, j + gridStartY).status
                                        Next
                                    Next
                                    Array.Copy(SynopClipboard, UndoClipboard, SynopClipboard.Length)

                                    'enable cut, copy,undo,redo, disable select, paste
                                    tsbtnCut.Enabled = True : tsbtnCopy.Enabled = True : tsbtnDelete.Enabled = True
                                    tsBtnExport.Enabled = True : tsBtnImport.Enabled = False
                                End If
                            Case EditModes.Cut
                                CutElements()
                            Case EditModes.Copy
                                CopyElements()
                            Case EditModes.Paste
                                PasteElements(x, y)
                            Case EditModes.Delete
                                DeleteElements()
                            Case EditModes.Import
                                gridStartX = x
                                gridStartY = y
                                selectStart.x = x * gridStep
                                selectStart.y = y * gridStep
                                g.DrawRectangle(selectPen, selectStart.x, selectStart.y, gridStep, gridStep)
                                ImportClipBoard(x, y)

                                'Case EditModes.Export
                                '    sub nog aanmaken
                            Case Else   'selectMode
                        End Select
                    Case Windows.Forms.MouseButtons.Right
                    Case Windows.Forms.MouseButtons.Middle
                End Select
            Else            '========== Draw mode Draw mode Draw mode Draw mode Draw mode Draw mode Draw mode ===================
                ClipboardActive = False
                tsBtnExport.Enabled = False
                Select Case e.Button
                    Case Windows.Forms.MouseButtons.Left
                        'plaats element op scherm
                        'bewaar in canvasNew(,)
                        canvasNew(x, y).active = True
                        canvasNew(x, y).elementNR = element(xE, yE)
                        Select Case element(xE, yE)
                            Case 1, 2        'lengte blokken
                                canvasNew(x, y).elementLenght = CInt(blockLenght.ToString)
                                cv.active = False : cv.itemNR = "0" : cv.locoNR = String.Empty
                                cv.posX = x : cv.posY = y : cv.gridStep = gridStep
                                cv.elementNR = element(xE, yE) : cv.elementLenght = CInt(blockLenght.ToString)
                                cv.strInfo = cm.txtInfo.Text
                                cv.notes = cm.txtNotes.Text
                                cv.status = "*" : cv.direction = "*"
                                DrawElement(cv, g)
                            Case 23, 24        'lengte tekst
                                canvasNew(x, y).elementLenght = CInt(TekstLenght.ToString)
                                cv.active = False : cv.itemNR = "0" : cv.locoNR = String.Empty
                                cv.posX = x : cv.posY = y : cv.gridStep = gridStep
                                cv.elementNR = element(xE, yE) : cv.elementLenght = CInt(TekstLenght.ToString)
                                cv.strInfo = cm.txtInfo.Text
                                cv.notes = cm.txtNotes.Text
                                cv.status = "*" : cv.direction = "*"
                                DrawElement(cv, g)
                            Case Else        'lengte van alle andere elmementen = 1
                                canvasNew(x, y).elementLenght = 1
                                cv.active = False : cv.itemNR = "0" : cv.locoNR = String.Empty
                                cv.posX = x : cv.posY = y : cv.gridStep = gridStep
                                cv.elementNR = element(xE, yE) : cv.elementLenght = 1
                                cv.strInfo = cm.txtInfo.Text
                                cv.notes = cm.txtNotes.Text
                                cv.status = "*" : cv.direction = "*"
                                DrawElement(cv, g)
                        End Select
                        canvasNew(x, y).direction = "*"
                        canvasNew(x, y).status = "*"
                        canvasNew(x, y).strInfo = "$"
                        canvasNew(x, y).notes = "@"
                        canvasNew(x, y).itemNR = "0"
                        canvasNew(x, y).locoNR = "0"
                        MDIParentfrm.mdiParentStatus.Text = "GridCell x = " & x & ",  y = " & y

                    Case Windows.Forms.MouseButtons.Right   'contextmenu oproepen
                        Dim Result As DialogResult

                        If canvasNew(x, y).active Then
                            cm.txtNR.Text = canvasNew(x, y).itemNR.ToString
                            cm.txtInfo.Text = canvasNew(x, y).strInfo
                            cm.txtNotes.Text = canvasNew(x, y).notes
                            Result = cm.ShowDialog()
                            If Result = System.Windows.Forms.DialogResult.OK Then
                                'Als result ok dan gegevens bewaren
                                If cm.txtNR.Text <> "" AndAlso Not (String.IsNullOrWhiteSpace(cm.txtNR.Text)) Then
                                    canvasNew(x, y).itemNR = CStr(CInt(cm.txtNR.Text))
                                    canvasNew(x, y).strInfo = cm.txtInfo.Text
                                    canvasNew(x, y).notes = cm.txtNotes.Text
                                    cv.active = False : cv.itemNR = "0" : cv.locoNR = String.Empty
                                    cv.posX = x : cv.posY = y : cv.gridStep = gridStep
                                    cv.elementNR = canvasNew(x, y).elementNR : cv.elementLenght = canvasNew(x, y).elementLenght
                                    cv.strInfo = cm.txtInfo.Text.Substring(1)
                                    cv.notes = cm.txtNotes.Text
                                    cv.status = "*" : cv.direction = "*"
                                    DrawElement(cv, g)
                                End If
                            End If
                        End If
                    Case Windows.Forms.MouseButtons.Middle
                        'clear cell
                        cv.active = False : cv.itemNR = "0" : cv.locoNR = String.Empty
                        cv.posX = x : cv.posY = y : cv.gridStep = gridStep
                        cv.elementNR = 100 : cv.elementLenght = 1
                        cv.strInfo = cm.txtInfo.Text
                        cv.notes = cm.txtNotes.Text
                        cv.status = "*" : cv.direction = "*"
                        DrawElement(cv, g)
                        'bewaar in canvasNew(,)
                        canvasNew(x, y).active = False
                        canvasNew(x, y).itemNR = "0"
                        canvasNew(x, y).elementNR = 0
                        canvasNew(x, y).elementLenght = 1
                        canvasNew(x, y).direction = "*"
                        canvasNew(x, y).status = "*"
                        canvasNew(x, y).strInfo = "$"
                        canvasNew(x, y).notes = "@"
                End Select
            End If
            g.Dispose()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub pnlCanvas_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlCanvas.Paint
        DrawGrid(e.Graphics)        'draw the grid
        'New or Edit?
        If synopActiveNR >= 0 Then  'editeer bestaande synoptiek
            If switch Then InitCanvasNew() : switch = False
            synopActiveDone = True     'niet meer uitvoeren
        End If
        RepaintCanvas(e.Graphics)
    End Sub

    Private Sub tsbtnDrawElements_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnDrawElements.Click
        Dim ElementsFrm As New ElementsFrm
        ElementsFrm.Show()
    End Sub

    Private Sub ToolStripCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripCancel.Click
        MDIParentfrm.EditMenu.Visible = False
        Me.Dispose()
    End Sub

    Private Sub tsRepaint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsRepaint.Click
        Dim gr As Graphics = pnlCanvas.CreateGraphics
        RepaintCanvas(gr)
    End Sub

    Private Sub tsClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsClear.Click

        If MessageBox.Show("Wilt U alle elementen wegvegen?", "Nieuwe synoptiek", _
         MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim g As Graphics = pnlCanvas.CreateGraphics()
            g.Clear(Color.White)
            DrawGrid(g)
            For x As Integer = 0 To canvasW
                For y As Integer = 0 To canvasH
                    canvasNew(x, y).active = False
                Next
            Next
        End If
    End Sub

    Private Sub ToolStripSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripSave.Click

        Dim ok As Boolean = False
        Dim d As String = "|"       'item separator delimiter used in strProp and strCanvas and canvasgroup()
        Dim strCanvas As String = ""
        Dim strProp As String = ""
        Dim i As Integer = 0
        Dim t As Integer = 0
        Dim index As Integer = -1

        'canvas deel: extract active cells from canvasNew() and put in canvasArray()
        For y As Integer = 0 To canvasNew.GetUpperBound(1)             'y
            For x As Integer = 0 To canvasNew.GetUpperBound(0)         'x
                i = x + y * canvasNew.GetUpperBound(0) + t
                If canvasNew(x, y).active Then
                    ok = True   ' zie verder canvas is niet leeg

                    'put in string volgorde: x.y, elementNR, lenght, blokNR, naam v/h blok, status, direction
                    If canvasNew(x, y).elementNR > 0 Then
                        strCanvas += x.ToString & d & y.ToString & d & canvasNew(x, y).elementNR.ToString & d
                        strCanvas += canvasNew(x, y).elementLenght.ToString & d
                        strCanvas += canvasNew(x, y).itemNR.ToString & d
                        If IsNothing(canvasNew(x, y).strInfo) Then canvasNew(x, y).strInfo = "$"
                        strCanvas += canvasNew(x, y).strInfo & d
                        If IsNothing(canvasNew(x, y).notes) Then canvasNew(x, y).notes = "@"
                        strCanvas += canvasNew(x, y).notes & d
                        strCanvas += canvasNew(x, y).status & d
                        If IsNothing(canvasNew(x, y).direction) Then canvasNew(x, y).direction = "*"
                        strCanvas += canvasNew(x, y).direction & d
                    End If

                    'put in new canvasNew().canvas,   canvasCells with addition of the x, y coordinates
                    index += 1  'verhoog index

                    '+++++++++++++++++++++++++++++++++++++++++++++++++++++++ 12345 ==================
                    If index > canvasArray.GetUpperBound(0) Then
                        Dim result = MessageBox.Show("index= " & index.ToString & "   Ubound canvasArray= " & canvasArray.GetUpperBound(0).ToString, "Fout bij wegschrijven nieuwe synoptiek", _
                                  MessageBoxButtons.YesNo, _
                                  MessageBoxIcon.Question)
                        If (result = DialogResult.Yes) Then
                            Exit Sub
                        End If
                    End If
                    '=================================================== 12345 =================


                    canvasArray(index).posX = x : canvasArray(index).posY = y
                    canvasArray(index).elementNR = canvasNew(x, y).elementNR
                    canvasArray(index).elementLenght = canvasNew(x, y).elementLenght       'elementlengte meestal v/e blok
                    canvasArray(index).itemNR = canvasNew(x, y).itemNR
                    canvasArray(index).strInfo = canvasNew(x, y).strInfo 'bv de bloknaam
                    canvasArray(index).notes = canvasNew(x, y).notes 'bv omschrijving
                    canvasArray(index).direction = canvasNew(x, y).direction ' g= gerade, r= rund, *= onbepaald
                End If
            Next
            t += 1
        Next
        If ok Then       'indien niet leeg
            'canvas property deel in string format: appearance, gridstep, tittle, txtlabel
            If Not synopActiveDone Then  'New synop
                'pas de titel aan met prefix "000 " nummer
                canvasProperty.tittle = Format(strSynopGroup.GetUpperBound(0), "000") + " " + canvasProperty.tittle
            End If
            strProp = canvasProperty.appearance & d & canvasProperty.gridStep & d & _
                canvasProperty.tittle & d & canvasProperty.labelText & d
            SaveSynoptic(strProp, strCanvas)            'save on disk
        Else
            canvasProperty.tittle = Format(strSynopGroup.GetUpperBound(0), "000") + " " + canvasProperty.tittle
            strProp = canvasProperty.appearance & d & canvasProperty.gridStep & d & _
                canvasProperty.tittle & d & "PLaatshouder voor lege synoptiek !!!!!!! " + d
            SaveSynoptic(strProp, "0|0|0|1|0|$|@|*|*|")            'save on disk 
        End If
        MDIParentfrm.EditMenu.Visible = False
        Me.Dispose()
    End Sub

    Private Sub tsDrawMode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsDrawMode.Click
        Dim gr As Graphics = pnlCanvas.CreateGraphics
        RepaintCanvas(gr)

        'enable and disable buttons       
        selectActive = False
        selectON = False
        editMode = EditModes.selekt
        Me.Cursor = Cursors.Arrow

        ' enable and disable buttons
        tsbtnSelect.Enabled = True : tsbtnCopy.Enabled = False : tsbtnCut.Enabled = False : tsbtnDelete.Enabled = False
        tsbtnUndo.Enabled = False : tsbtnRedo.Enabled = False : tsbtnPaste.Enabled = False

    End Sub

    Private Sub tsbtnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnSelect.Click
        selectActive = True
        Me.Cursor = Cursors.Hand
        ' enable and disable buttons
        tsbtnSelect.Enabled = False : tsbtnCopy.Enabled = False : tsbtnCut.Enabled = False : tsbtnDelete.Enabled = False
        tsbtnUndo.Enabled = False : tsbtnRedo.Enabled = False : tsbtnPaste.Enabled = False
        editMode = EditModes.selekt        'naar volgende editMode
    End Sub

    Private Sub tsbtnCut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnCut.Click
        'pas de canvasNew(i,j) aan en maak ReduClipboard(i,j) aan
        CutElements()
        'aanpassen canvas
        Dim g As Graphics = pnlCanvas.CreateGraphics
        RepaintCanvas(g)
        g.Dispose()
        ' enable and disable buttons
        tsbtnSelect.Enabled = False : tsbtnCopy.Enabled = False : tsbtnCut.Enabled = False : tsbtnDelete.Enabled = False
        tsbtnUndo.Enabled = True : tsbtnRedo.Enabled = False : tsbtnPaste.Enabled = True
        editMode = EditModes.Cut        'naar volgende editMode
    End Sub

    Private Sub tsbtnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnCopy.Click
        CopyElements()  'maak ReduClipboard(,) aan
        ' enable and disable buttons
        tsbtnSelect.Enabled = False : tsbtnCopy.Enabled = False : tsbtnCut.Enabled = False : tsbtnDelete.Enabled = False
        tsbtnUndo.Enabled = True : tsbtnRedo.Enabled = False : tsbtnPaste.Enabled = True
        editMode = EditModes.Paste        'naar volgende editMode

    End Sub

    Private Sub tsbtnPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnPaste.Click
        ' enable and disable buttons
        tsbtnSelect.Enabled = True : tsbtnCopy.Enabled = False : tsbtnCut.Enabled = False : tsbtnDelete.Enabled = True
        tsbtnUndo.Enabled = True : tsbtnRedo.Enabled = True
        editMode = EditModes.Paste
    End Sub

    Private Sub tsbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnDelete.Click
        DeleteElements()
        ' enable and disable buttons
        tsbtnSelect.Enabled = True : tsbtnCopy.Enabled = False : tsbtnCut.Enabled = False : tsbtnDelete.Enabled = False
        tsbtnUndo.Enabled = True : tsbtnRedo.Enabled = False : tsbtnPaste.Enabled = False
        editMode = EditModes.Delete
    End Sub

    Private Sub tsbtnUndo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnUndo.Click
        'Herstel de gridcellen uit synopClipboard(,)
        Dim i, j As Integer
        For j = 0 To UndoClipboard.GetUpperBound(1)
            For i = 0 To UndoClipboard.GetUpperBound(0)
                canvasNew(UndoClipboard(i, j).posX, UndoClipboard(i, j).posY).active = True
                canvasNew(UndoClipboard(i, j).posX, UndoClipboard(i, j).posY).itemNR = UndoClipboard(i, j).itemNR
                canvasNew(UndoClipboard(i, j).posX, UndoClipboard(i, j).posY).direction = UndoClipboard(i, j).direction
                canvasNew(UndoClipboard(i, j).posX, UndoClipboard(i, j).posY).elementNR = UndoClipboard(i, j).elementNR
                canvasNew(UndoClipboard(i, j).posX, UndoClipboard(i, j).posY).elementLenght = UndoClipboard(i, j).elementLenght
                canvasNew(UndoClipboard(i, j).posX, UndoClipboard(i, j).posY).status = UndoClipboard(i, j).status
                canvasNew(UndoClipboard(i, j).posX, UndoClipboard(i, j).posY).strInfo = UndoClipboard(i, j).strInfo
                canvasNew(UndoClipboard(i, j).posX, UndoClipboard(i, j).posY).notes = UndoClipboard(i, j).notes
            Next
        Next
        Dim g As Graphics = pnlCanvas.CreateGraphics
        RepaintCanvas(g)
        g.Dispose()
        ' enable and disable buttons
        tsbtnSelect.Enabled = True : tsbtnCopy.Enabled = False : tsbtnCut.Enabled = False : tsbtnDelete.Enabled = True
        tsbtnUndo.Enabled = False : tsbtnRedo.Enabled = True : tsbtnPaste.Enabled = False
    End Sub

    Private Sub tsbtnRedo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnRedo.Click
        'Herstel de gridcellen uit synopClipboard(,)
        Dim i, j As Integer
        For j = 0 To redoClipboard.GetUpperBound(1)
            For i = 0 To redoClipboard.GetUpperBound(0)
                canvasNew(redoClipboard(i, j).posX, redoClipboard(i, j).posY).active = True
                canvasNew(redoClipboard(i, j).posX, redoClipboard(i, j).posY).itemNR = redoClipboard(i, j).itemNR
                canvasNew(redoClipboard(i, j).posX, redoClipboard(i, j).posY).direction = redoClipboard(i, j).direction
                canvasNew(redoClipboard(i, j).posX, redoClipboard(i, j).posY).elementNR = redoClipboard(i, j).elementNR
                canvasNew(redoClipboard(i, j).posX, redoClipboard(i, j).posY).elementLenght = redoClipboard(i, j).elementLenght
                canvasNew(redoClipboard(i, j).posX, redoClipboard(i, j).posY).status = redoClipboard(i, j).status
                canvasNew(redoClipboard(i, j).posX, redoClipboard(i, j).posY).strInfo = redoClipboard(i, j).strInfo
                canvasNew(redoClipboard(i, j).posX, redoClipboard(i, j).posY).notes = redoClipboard(i, j).notes
            Next
        Next
        Dim g As Graphics = pnlCanvas.CreateGraphics
        RepaintCanvas(g)
        g.Dispose()
        ' enable and disable buttons
        tsbtnSelect.Enabled = True : tsbtnCopy.Enabled = False : tsbtnCut.Enabled = False : tsbtnDelete.Enabled = False
        tsbtnUndo.Enabled = True : tsbtnRedo.Enabled = True : tsbtnPaste.Enabled = True
    End Sub

    Private Sub tsbtnProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnProperties.Click
        If synopActiveNR < 0 Then Exit Sub
        Dim prop As New NewSynopPropertiesFrm
        prop.txtTitle.Text = synopGroup(synopActiveNR).Prop.tittle
        prop.txtLabel.Text = synopGroup(synopActiveNR).Prop.labelText
        prop.GroupBox1.Enabled = False
        prop.lstAppearance.Enabled = False
        If prop.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            ' Read the contents of testDialog's TextBox.
            synopGroup(synopActiveNR).Prop.tittle = prop.txtTitle.Text
            synopGroup(synopActiveNR).Prop.labelText = prop.txtLabel.Text
        End If
        prop.Dispose()
    End Sub

#End Region

#Region " Help subs for Menu items"

    Private Sub DeleteElements()
        'maak ReduClipboard(,) aan en reset canvasNew(,) naar init waarden
        Dim i, j As Integer
        Try
            For j = 0 To SynopClipboard.GetUpperBound(1)
                For i = 0 To SynopClipboard.GetUpperBound(0)
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).active = False
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).itemNR = "0"
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).direction = "*"
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).elementNR = 0
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).elementLenght = 1
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).strInfo = "$"
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).notes = "@"
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).status = "*"
                    UndoClipboard(i, j).posX = SynopClipboard(i, j).posX
                    UndoClipboard(i, j).posY = SynopClipboard(i, j).posY
                    UndoClipboard(i, j).active = True
                    UndoClipboard(i, j).itemNR = SynopClipboard(i, j).itemNR
                    UndoClipboard(i, j).direction = SynopClipboard(i, j).direction
                    UndoClipboard(i, j).elementNR = SynopClipboard(i, j).elementNR
                    UndoClipboard(i, j).elementLenght = SynopClipboard(i, j).elementLenght
                    UndoClipboard(i, j).status = SynopClipboard(i, j).status
                    UndoClipboard(i, j).strInfo = SynopClipboard(i, j).strInfo
                    UndoClipboard(i, j).notes = SynopClipboard(i, j).notes
                Next
            Next
            Dim g As Graphics = pnlCanvas.CreateGraphics
            RepaintCanvas(g)
            g.Dispose()
            ' enable and disable buttons
            tsbtnSelect.Enabled = True : tsbtnCopy.Enabled = False : tsbtnCut.Enabled = False : tsbtnDelete.Enabled = False
            tsbtnUndo.Enabled = True : tsbtnRedo.Enabled = False : tsbtnPaste.Enabled = False
        Catch ex As Exception
            Beep()
        End Try
    End Sub

    Private Sub CutElements()
        'maak ReduClipboard(,) aan en reset canvasNew(,) naar init waarden
        Dim i, j As Integer
        Try
            For j = 0 To SynopClipboard.GetUpperBound(1)
                For i = 0 To SynopClipboard.GetUpperBound(0)
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).active = False
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).itemNR = "0"
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).direction = "*"
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).elementNR = 0
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).elementLenght = 1
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).strInfo = "$"
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).notes = "$"
                    canvasNew(SynopClipboard(i, j).posX, SynopClipboard(i, j).posY).status = "*"
                    redoClipboard(i, j).posX = SynopClipboard(i, j).posX
                    redoClipboard(i, j).posY = SynopClipboard(i, j).posY
                    redoClipboard(i, j).active = False
                    redoClipboard(i, j).itemNR = "0"
                    redoClipboard(i, j).direction = "*"
                    redoClipboard(i, j).elementNR = 0
                    redoClipboard(i, j).elementLenght = 1
                    redoClipboard(i, j).strInfo = "$"
                    redoClipboard(i, j).notes = "@"
                    redoClipboard(i, j).status = "*"
                Next
            Next
        Catch ex As Exception
            Beep()
        End Try
    End Sub

    Private Sub CopyElements()
        'maak ReduClipboard(,) aan uit synopClipboard(,)
        Dim i, j As Integer
        Try
            For j = 0 To SynopClipboard.GetUpperBound(1)
                For i = 0 To SynopClipboard.GetUpperBound(0)
                    redoClipboard(i, j).posX = SynopClipboard(i, j).posX
                    redoClipboard(i, j).posY = SynopClipboard(i, j).posY
                    redoClipboard(i, j).active = True
                    redoClipboard(i, j).itemNR = SynopClipboard(i, j).itemNR
                    redoClipboard(i, j).direction = SynopClipboard(i, j).direction
                    redoClipboard(i, j).elementNR = SynopClipboard(i, j).elementNR
                    redoClipboard(i, j).elementLenght = SynopClipboard(i, j).elementLenght
                    redoClipboard(i, j).status = SynopClipboard(i, j).status
                    redoClipboard(i, j).strInfo = SynopClipboard(i, j).strInfo
                    redoClipboard(i, j).notes = SynopClipboard(i, j).notes
                Next
            Next
        Catch ex As Exception
            Beep()
        End Try
    End Sub

    Private Sub PasteElements(ByVal newBaseX As Integer, ByVal newBaseY As Integer)
        'Copy de gridcellen uit synopClipboard(,)
        Dim i, j, indexX, indexY, deltaX, deltaY As Integer
        Dim oldBaseX As Integer = SynopClipboard(0, 0).posX
        Dim oldBaseY As Integer = SynopClipboard(0, 0).posY
        Try
            deltaX = newBaseX - oldBaseX
            deltaY = newBaseY - oldBaseY
            For j = 0 To SynopClipboard.GetUpperBound(1)
                For i = 0 To SynopClipboard.GetUpperBound(0)
                    If SynopClipboard(i, j).posX > 0 OrElse SynopClipboard(i, j).posY > 0 Then  'blancocellen uitfilteren
                        indexX = SynopClipboard(i, j).posX + deltaX
                        indexY = SynopClipboard(i, j).posY + deltaY
                        canvasNew(indexX, indexY).active = True
                        canvasNew(indexX, indexY).itemNR = SynopClipboard(i, j).itemNR
                        canvasNew(indexX, indexY).direction = SynopClipboard(i, j).direction
                        canvasNew(indexX, indexY).elementNR = SynopClipboard(i, j).elementNR
                        canvasNew(indexX, indexY).elementLenght = SynopClipboard(i, j).elementLenght
                        canvasNew(indexX, indexY).status = SynopClipboard(i, j).status
                        canvasNew(indexX, indexY).strInfo = SynopClipboard(i, j).strInfo
                        canvasNew(indexX, indexY).notes = SynopClipboard(i, j).notes
                    End If
                Next
            Next
            Dim g As Graphics = pnlCanvas.CreateGraphics
            RepaintCanvas(g)
            g.Dispose()
            'enable and disable buttons
            'tsbtnCopy.Enabled = False : tsbtnCut.Enabled = False
            tsbtnSelect.Enabled = True : tsbtnUndo.Enabled = True : tsbtnRedo.Enabled = False
        Catch ex As Exception
            Beep()
        End Try
    End Sub

    Private Sub ImportClipBoard(ByVal newBaseX As Integer, ByVal newBaseY As Integer)
        tsbtnRedo.Enabled = False
        ClipboardActive = False
        'Copy de gridcellen uit synopClipboard(,)
        Dim i, j, indexX, indexY, deltaX, deltaY As Integer
        Dim oldBaseX As Integer = SynopClipboard(0, 0).posX
        Dim oldBaseY As Integer = SynopClipboard(0, 0).posY
        Try
            deltaX = newBaseX - oldBaseX
            deltaY = newBaseY - oldBaseY
            For j = 0 To SynopClipboard.GetUpperBound(1)
                For i = 0 To SynopClipboard.GetUpperBound(0)
                    If SynopClipboard(i, j).posX > 0 OrElse SynopClipboard(i, j).posY > 0 Then  'blancocellen uitfilteren
                        indexX = SynopClipboard(i, j).posX + deltaX
                        indexY = SynopClipboard(i, j).posY + deltaY
                        canvasNew(indexX, indexY).active = True
                        canvasNew(indexX, indexY).itemNR = SynopClipboard(i, j).itemNR
                        canvasNew(indexX, indexY).direction = SynopClipboard(i, j).direction
                        canvasNew(indexX, indexY).elementNR = SynopClipboard(i, j).elementNR
                        canvasNew(indexX, indexY).elementLenght = SynopClipboard(i, j).elementLenght
                        canvasNew(indexX, indexY).status = SynopClipboard(i, j).status
                        canvasNew(indexX, indexY).strInfo = SynopClipboard(i, j).strInfo
                        canvasNew(indexX, indexY).notes = SynopClipboard(i, j).notes
                    End If
                Next
            Next
            Dim g As Graphics = pnlCanvas.CreateGraphics
            RepaintCanvas(g)
            g.Dispose()
        Catch ex As Exception
            Beep()
        End Try

    End Sub

    Private Sub tsBtnExport_Click(sender As System.Object, e As System.EventArgs) Handles tsBtnExport.Click
        ClipboardActive = True
        tsBtnExport.Enabled = False
    End Sub

    Private Sub tsBtnImport_Click(sender As System.Object, e As System.EventArgs) Handles tsBtnImport.Click
    End Sub

#End Region

#Region "Canvas Drawing subroutines"

    Private Sub DrawGrid(ByVal gr As Graphics)

        Dim nooflines As Integer
        Dim pbwidth As Integer
        Dim pbheight As Integer
        Dim distance As Integer = 0
        nooflines = CInt(MdiParent.Height / gridStep) 'calculating lines needed according to height  
        canvasH = nooflines
        pbwidth = MdiParent.Width
        For i = 0 To nooflines  'not deceresing the lines to make it to the bottom  
            gr.DrawLine(gridPen, 0, distance, pbwidth, distance)
            distance += gridStep
        Next
        nooflines = CInt(MdiParent.Width / gridStep) 'calculating lines needed according to width 
        canvasW = nooflines
        pbheight = MdiParent.Height
        distance = 0
        For i = 0 To nooflines  'not deceresing the lines to make it to the bottom  
            gr.DrawLine(gridPen, distance, 0, distance, pbheight)
            distance += gridStep
        Next
        If firstEntry = False Then
            ReDim canvasNew(512, 384)   'grootst mogelijke array
            firstEntry = True
        End If
    End Sub

    Private Sub DrawElement(ByVal cv As CanvasCells, ByVal gr As Graphics)
        'ter info
        'Public Structure CanvasCells 
        '    Public posX As Integer
        '    Public posY As Integer
        '    public gridStep As Integer
        '    Public elementNR As Integer
        '    Public blokNR As Integer
        '    Public elementLenght As Integer
        '    Public status As String
        '    Public direction As String
        '    Public strInfo As String
        '    Public notes AS string
        'End Structure

        'frequently used combinations
        Dim gridUnit As Integer = cv.gridStep \ 2
        X1 = cv.posX * cv.gridStep
        Y1 = cv.posY * cv.gridStep
        XU = X1 + gridUnit
        XS = X1 + cv.gridStep
        YU = Y1 + gridUnit
        YS = Y1 + gridStep

        Select Case cv.status
            Case "A"      'Active 
                elementPenStatus.Color = Color.DarkRed
                blockFillBrush = Brushes.LightGreen
            Case Else   'init gebruik *
                elementPenStatus.Color = Color.DarkGray
                blockFillBrush = Brushes.DarkGray
        End Select

        Select Case cv.elementNR
            'numbers below 100 are active elements

            '   block elements
            Case 1  'horizontal block
                gr.FillRectangle(Brushes.Beige, X1, Y1, gridStep * cv.elementLenght, gridStep)
                gr.FillRectangle(blockFillBrush, X1, Y1, gridStep, gridStep)
                gr.DrawRectangle(blockPen, X1, Y1, gridStep * cv.elementLenght, gridStep)
                gr.DrawLine(blockPen, XS, Y1, XS, YS)
                'gr.DrawLine(blockPen, X1 + gridStep * cv.elementLenght - gridStep, Y1, X1 + gridStep * cv.elementLenght - gridStep, YS)
                gr.DrawString(cv.strInfo, blockFont, blockBrush, XS, Y1 + gridText)
            Case 2  'vertical block
                gr.FillRectangle(Brushes.Beige, X1, Y1, gridStep, gridStep * cv.elementLenght)
                gr.FillRectangle(blockFillBrush, X1, Y1, gridStep, gridStep)
                gr.DrawRectangle(blockPen, X1, Y1, gridStep, gridStep * cv.elementLenght)
                gr.DrawLine(blockPen, X1, YS, XS, YS)
                'gr.DrawLine(blockPen, X1, Y1 + gridStep * cv.elementLenght - gridStep, XS, Y1 + gridStep * cv.elementLenght - gridStep)

                gr.DrawString(cv.strInfo, blockFont, blockBrush, X1, YS, blockStrFormat)

                'text elements
            Case 23  'horizontal text
                gr.FillRectangle(Brushes.LightYellow, X1, Y1, gridStep * cv.elementLenght, gridStep)
                gr.FillRectangle(Brushes.Yellow, X1, Y1, gridStep, gridStep)
                gr.DrawRectangle(textPen, X1, Y1, gridStep * cv.elementLenght, gridStep)
                gr.DrawString(cv.strInfo, textFont, textBrush, X1, Y1 + gridText)
            Case 24  'vertical text
                gr.FillRectangle(Brushes.LightYellow, X1, Y1, gridStep, gridStep * cv.elementLenght)
                gr.FillRectangle(Brushes.Yellow, X1, Y1, gridStep, gridStep)
                gr.DrawRectangle(textPen, X1, Y1, gridStep, gridStep * cv.elementLenght)
                gr.DrawString(cv.strInfo, textFont, textBrush, X1, Y1, blockStrFormat)

                'Engelse wissels
            Case 33  'kruising vertikaal, links boven naar rechts onder
                Select Case cv.direction
                    Case "0"
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                        'gr.DrawLine(balPen, X1H, Y1H, XSH, YSH)

                    Case "1"
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case Else
                        gr.DrawLine(elementPen, X1, Y1, XS, YS)
                        gr.DrawLine(elementPen, XU, Y1, XU, YS)
                End Select
            Case 34  'kruising vertikaal, links onder naar rechts boven
                Select Case cv.direction
                    Case "0"
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                        'gr.DrawLine(balPen, X1H, Y1H, XSH, YSH)
                    Case "1"
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case Else
                        gr.DrawLine(elementPen, X1, YS, XS, Y1)
                        gr.DrawLine(elementPen, XU, Y1, XU, YS)
                End Select
            Case 35  'kruising links onder rechts boven
                Select Case cv.direction
                    Case "h"
                        gr.DrawLine(elementPenFre, X1, YS, XS, Y1)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "v"
                        gr.DrawLine(elementPenFre, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                    Case Else
                        gr.DrawLine(elementPen, X1, YS, XS, Y1)
                        gr.DrawLine(elementPen, X1, YU, XS, YU)
                End Select
            Case 36  'kruising links boven rechts onder
                Select Case cv.direction
                    Case "h"
                        gr.DrawLine(elementPenFre, X1, Y1, XS, YS)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                    Case "v"
                        gr.DrawLine(elementPenFre, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                    Case Else
                        gr.DrawLine(elementPen, X1, YU, XS, YU)
                        gr.DrawLine(elementPen, X1, Y1, XS, YS)
                End Select

                'Wissels
            Case 42  'wissel kort horizontaal arm links naar boven
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, X1, YU, XU, Y1)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "r"
                        gr.DrawLine(elementPenFre, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, YU, XU, Y1)
                    Case Else
                        Dim points As Point() = {New Point(XS, YU), New Point(X1, YU), _
                      New Point(XU, Y1)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 43  'wissel kort horizontaal arm links naar beneden
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, X1, YU, XU, YS)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "r"
                        gr.DrawLine(elementPenFre, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, YU, XU, YS)
                    Case Else
                        Dim points As Point() = {New Point(XS, YU), New Point(X1, YU), _
                      New Point(XU, YS)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 44  'wissel kort horizontaal arm rechts naar boven
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, XU, Y1, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "r"
                        gr.DrawLine(elementPenFre, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, XU, Y1, XS, YU)
                    Case Else
                        Dim points As Point() = {New Point(X1, YU), New Point(XS, YU), _
                      New Point(XU, Y1)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 45  'wissel kort horizontaal arm rechts naar beneden
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, XS, YU, XU, YS)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "r"
                        gr.DrawLine(elementPenFre, X1, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, XS, YU, XU, YS)
                    Case Else
                        Dim points As Point() = {New Point(X1, YU), New Point(XS, YU), _
                      New Point(XU, YS)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 46  'wissel kort vertikaal arm links onderaan
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, X1, YU, XU, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)

                    Case "r"
                        gr.DrawLine(elementPenFre, XU, Y1, XU, YS)
                        gr.DrawLine(elementPenStatus, X1, YU, XU, YS)
                    Case Else
                        Dim points As Point() = {New Point(XU, Y1), New Point(XU, YS), _
                      New Point(X1, YU)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 47  'wissel kort vertikaal arm rechts onderaan
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, XS, YU, XU, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)

                    Case "r"
                        gr.DrawLine(elementPenFre, XU, Y1, XU, YS)
                        gr.DrawLine(elementPenStatus, XS, YU, XU, YS)
                    Case Else
                        Dim points As Point() = {New Point(XU, Y1), New Point(XU, YS), _
                      New Point(XS, YU)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 48  'wissel kort vertikaal arm links onderaan
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, X1, YU, XU, Y1)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)

                    Case "r"
                        gr.DrawLine(elementPenFre, XU, Y1, XU, YS)
                        gr.DrawLine(elementPenStatus, X1, YU, XU, Y1)
                    Case Else
                        Dim points As Point() = {New Point(X1, YU), New Point(XU, Y1), _
                      New Point(XU, YS)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 49  'wissel kort vertikaal arm rechts bovenaan
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, XU, Y1, XS, YU)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)

                    Case "r"
                        gr.DrawLine(elementPenFre, XU, Y1, XU, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XS, YU)
                    Case Else
                        Dim points As Point() = {New Point(XU, YS), New Point(XU, Y1), _
                      New Point(XS, YU)}
                        gr.DrawLines(elementPen, points)
                End Select

            Case 64  'wissel lang horizontaal arm links naar boven
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, XU, YU, XS, YS)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "r"
                        gr.DrawLine(elementPenFre, XU, YU, XS, YU)
                        Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                        New Point(XS, Y1)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(XS, Y1), New Point(XU, YU), _
                        New Point(X1, YU), New Point(XS, YU)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 65  'wissel lang horizontaal arm links naar beneden
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, XU, YU, XS, YS)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                    Case "r"
                        gr.DrawLine(elementPenFre, XU, YU, XS, YU)
                        Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                         New Point(XS, YS)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(XS, YS), New Point(XU, YU), _
                        New Point(X1, YU), New Point(XS, YU)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 66  'wissel lang horizontaal arm links naar boven
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, X1, Y1, XU, YU)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)

                    Case "r"
                        gr.DrawLine(elementPenFre, X1, YU, XU, YU)
                        Dim points As Point() = {New Point(X1, Y1), New Point(XU, YU), _
                        New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, Y1), New Point(XU, YU), _
                        New Point(X1, YU), New Point(XS, YU)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 67  'wissel lang horizontaal arm links naar beneden
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, X1, YS, XU, YU)
                        gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                    Case "r"
                        gr.DrawLine(elementPenFre, X1, YU, XU, YU)
                        Dim points As Point() = {New Point(X1, YS), New Point(XU, YU), _
                         New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, YS), New Point(XU, YU), _
                        New Point(X1, YU), New Point(XS, YU)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 68  'wissel lang vertikaal arm links naar boven
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, X1, Y1, XU, YU)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case "r"
                        gr.DrawLine(elementPenFre, XU, Y1, XU, YU)
                        Dim points As Point() = {New Point(X1, Y1), New Point(XU, YU), _
                         New Point(XU, YS)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, Y1), New Point(XU, YU), _
                        New Point(XU, Y1), New Point(XU, YS)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 69  'wissel lang vertikaal arm rechts naar boven
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, XS, Y1, XU, YU)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case "r"
                        gr.DrawLine(elementPenFre, XU, Y1, XU, YU)
                        Dim points As Point() = {New Point(XS, Y1), New Point(XU, YU), _
                         New Point(XU, YS)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(XS, Y1), New Point(XU, YU), _
                        New Point(XU, Y1), New Point(XU, YS)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 70  'wissel lang vertikaal arm rechts naar boven
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, X1, YS, XU, YU)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case "r"
                        gr.DrawLine(elementPenFre, XU, YU, XU, YS)
                        Dim points As Point() = {New Point(XU, Y1), New Point(XU, YU), _
                         New Point(X1, YS)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, YS), New Point(XU, YU), _
                        New Point(XU, Y1), New Point(XU, YS)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 71  'wissel lang vertikaal arm rechts naar beneden
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, XU, YU, XS, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case "r"
                        gr.DrawLine(elementPenFre, XU, YU, XU, YS)
                        Dim points As Point() = {New Point(XU, Y1), New Point(XU, YU), _
                         New Point(XS, YS)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(XS, YS), New Point(XU, YU), _
                        New Point(XU, Y1), New Point(XU, YS)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 72  'wissel lang diagonaal arm rechts naar midden
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, XU, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                    Case "r"
                        gr.DrawLine(elementPenFre, XU, YU, XS, Y1)
                        Dim points As Point() = {New Point(X1, YS), New Point(XU, YU), _
                         New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, YS), New Point(XS, Y1), _
                        New Point(XU, YU), New Point(XS, YU)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 73  'wissel lang diagonaal arm links naar midden
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, X1, YU, XU, YU)
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                    Case "r"
                        gr.DrawLine(elementPenFre, X1, Y1, XU, YU)
                        Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                         New Point(XS, YS)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                        New Point(X1, Y1), New Point(XS, YS)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 74  'wissel lang diagonaal arm rechts naar midden
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, XU, YU, XS, YU)
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                    Case "r"
                        gr.DrawLine(elementPenFre, XU, YU, XS, YS)
                        Dim points As Point() = {New Point(X1, Y1), New Point(XU, YU), _
                         New Point(XS, YU)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, Y1), New Point(XS, YS), _
                        New Point(XU, YU), New Point(XS, YU)}
                        gr.DrawLines(elementPen, points)
                End Select
            Case 75  'wissel lang diagonaal arm rechts naar midden
                Select Case cv.direction
                    Case "g"
                        gr.DrawLine(elementPenFre, X1, YU, XU, YU)
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                    Case "r"
                        gr.DrawLine(elementPenFre, X1, YS, XU, YU)
                        Dim points As Point() = {New Point(X1, YU), New Point(XU, YU), _
                         New Point(XS, Y1)}
                        gr.DrawLines(elementPenStatus, points)

                    Case Else
                        Dim points As Point() = {New Point(X1, YS), New Point(XS, Y1), _
                        New Point(XU, YU), New Point(X1, YU)}
                        gr.DrawLines(elementPen, points)
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
                gr.DrawLine(elementPenFre, XU, Y1, XU, YS)
                gr.DrawLine(elementPenStatus, X1, YU, XU - gridUnit \ 3, YU)
                gr.DrawLine(elementPenStatus, XU + gridUnit \ 3, YU, XS, YU)
            Case 181     'vertical under bridge
                gr.DrawLine(elementPenFre, X1, YU, XS, YU)
                gr.DrawLine(elementPenStatus, XU, Y1, XU, YU - gridUnit \ 3)
                gr.DrawLine(elementPenStatus, XU, YU + gridUnit \ 3, XU, YS)
            Case 182     'left to right diagonal under bridge
                gr.DrawLine(elementPenFre, X1, YS, XS, Y1)
                gr.DrawLine(elementPenStatus, X1, Y1, XU - gridUnit \ 3, YU - gridUnit \ 3)
                gr.DrawLine(elementPenStatus, XU + gridUnit \ 3, YU + gridUnit \ 3, XS, YS)
            Case 183     'right to left diagonal under bridge
                gr.DrawLine(elementPenFre, X1, Y1, XS, YS)
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

    Private Sub DrawCellElements(ByVal g As Graphics)
        'draw the synoptic elements on the specific g canvas
        Dim nElements As Integer = synopGroup(synopActiveNR).canvas.GetUpperBound(0)
        For k As Integer = 0 To nElements
            'draw element on canvas
            canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).active = True
            canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).status = "*"
            canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).direction = synopGroup(synopActiveNR).canvas(k).direction
            canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).elementNR = synopGroup(synopActiveNR).canvas(k).elementNR
            canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).elementLenght = synopGroup(synopActiveNR).canvas(k).elementLenght
            canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).itemNR = synopGroup(synopActiveNR).canvas(k).itemNR
            canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).strInfo = synopGroup(synopActiveNR).canvas(k).strInfo
            canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).notes = synopGroup(synopActiveNR).canvas(k).notes
            cv.active = False : cv.itemNR = "0" : cv.locoNR = String.Empty
            cv.posX = synopGroup(synopActiveNR).canvas(k).posX : cv.posY = synopGroup(synopActiveNR).canvas(k).posY : cv.gridStep = gridStep
            cv.elementNR = synopGroup(synopActiveNR).canvas(k).elementNR : cv.elementLenght = synopGroup(synopActiveNR).canvas(k).elementLenght
            cv.strInfo = synopGroup(synopActiveNR).canvas(k).strInfo
            cv.notes = synopGroup(synopActiveNR).canvas(k).notes
            cv.status = "*" : cv.direction = "*"
            DrawElement(cv, g)
        Next
    End Sub

    Private Sub InitCanvasNew()
        'draw the synoptic elements on the specific g canvas
        Try

            Dim nElements As Integer = synopGroup(synopActiveNR).canvas.GetUpperBound(0)
            For k As Integer = 0 To nElements
                'draw element on canvas
                canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).active = True
                canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).status = "*"
                canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).direction = synopGroup(synopActiveNR).canvas(k).direction
                canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).elementNR = synopGroup(synopActiveNR).canvas(k).elementNR
                canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).elementLenght = synopGroup(synopActiveNR).canvas(k).elementLenght
                canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).itemNR = synopGroup(synopActiveNR).canvas(k).itemNR
                canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).strInfo = synopGroup(synopActiveNR).canvas(k).strInfo
                canvasNew(synopGroup(synopActiveNR).canvas(k).posX, synopGroup(synopActiveNR).canvas(k).posY).notes = synopGroup(synopActiveNR).canvas(k).notes
            Next
        Catch ex As Exception
            MsgBox("Elementen passen niet meer in Canvas: " + ex.ToString, MsgBoxStyle.Information, "InitnewCanvas")
        End Try
    End Sub

    Private Sub RepaintCanvas(ByVal g As Graphics)
        'repaint
        g.Clear(Color.White)
        DrawGrid(g)
        For x As Integer = 0 To canvasW
            For y As Integer = 0 To canvasH
                If canvasNew(x, y).active Then
                    cv.active = False : cv.itemNR = "0" : cv.locoNR = String.Empty
                    cv.posX = x : cv.posY = y : cv.gridStep = gridStep
                    cv.elementNR = canvasNew(x, y).elementNR : cv.elementLenght = canvasNew(x, y).elementLenght
                    If Not IsNothing(canvasNew(x, y).strInfo) AndAlso canvasNew(x, y).strInfo.StartsWith("$") Then
                        cv.strInfo = canvasNew(x, y).strInfo.Substring(1)
                    Else
                        cv.strInfo = canvasNew(x, y).strInfo
                    End If
                    cv.notes = canvasNew(x, y).notes
                    cv.status = "*" : cv.direction = "*"
                    DrawElement(cv, g)
                End If
            Next
        Next
    End Sub

#End Region
End Class