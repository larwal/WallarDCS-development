'WallarDCS (c) wallar 2011

Imports System.Drawing.Drawing2D

Public Class ElementsFrm

#Region "Initial Vars"

    'Initial values
    Private gridStep As Integer   'determines the space between lines  
    Private gridUnit As Integer
    Private gridText As Integer

    Private colorFre As Color = Color.LightGreen
    Private colorReserved As Color = Color.Orange
    Private colorOccupied As Color = Color.Blue
    Private colormaintenance As Color = Color.Magenta
    Private colorError As Color = Color.Red

    Private gridPen As New Pen(Color.LightBlue, 1)
    Private elementPen As New Pen(Color.DarkGray, 1)
    Private elementPenFre As New Pen(colorFre, 1)
    Private elementPenStatus As New Pen(Color.Black, 1)
    Private blockFont As Font
    Private blockPen As Pen
    Private textFont As Font
    Private blockBrush As Brush
    Private blockFillBrush As Brush
    Private blockStrFormat As New StringFormat
    Private blockMultiply As Integer = 3
    Private textBrush As Brush
    Private textPen As New Pen(Color.Black, 1)
    Private blancoBrush As Brush

    'draw coordinaten in DrawElements
    Private X1, Y1, XU, XS, YU, YS As Integer   'used only in Sub DrawElement
    Private cv As CanvasCells
#End Region

#Region "Load, mouse and paint"

    Private Sub ElementsFrm_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        elementPen.Dispose()
        elementPenFre.Dispose()
        elementPenStatus.Dispose()
    End Sub

    Private Sub ElementsFrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gridStep = 24           'determines the space between lines 
        'gridmaten  ===================
        gridUnit = gridStep \ canvasValue.setGridUnit
        gridText = gridStep \ canvasValue.setGridText
        gridPen.Color = Color.LightBlue
        gridPen.Width = 1

        'diktes pennen
        elementPen.Width = gridStep \ canvasValue.setPenWidth
        elementPenFre.Width = gridStep \ canvasValue.setPenWidth
        elementPenStatus.Width = gridStep \ canvasValue.setPenWidth
        elementPenStatus.LineJoin = Drawing2D.LineJoin.Round

        'block properties
        blockMultiply = 4
        blockFont = New Font("Arial", gridUnit)
        textFont = New Font("Verdana", gridUnit)
        blockBrush = Brushes.Red
        blockFillBrush = Brushes.LightGray
        blockPen = Pens.Black
        blockStrFormat.FormatFlags = StringFormatFlags.DirectionVertical
        textBrush = Brushes.Blue
        textPen = Pens.DarkBlue
        blancoBrush = Brushes.White

        'opvullen van element(,)
        element(0, 0) = 111 : element(1, 0) = 112 : element(2, 0) = 113 : element(3, 0) = 114 : element(4, 0) = 115 : element(5, 0) = 116
        element(6, 0) = 117 : element(7, 0) = 118 : element(8, 0) = 119 : element(9, 0) = 120 : element(10, 0) = 121 : element(11, 0) = 122

        element(0, 1) = 123 : element(1, 1) = 124 : element(2, 1) = 125 : element(3, 1) = 126 : element(4, 1) = 127 : element(5, 1) = 128
        element(6, 1) = 129 : element(7, 1) = 130 : element(8, 1) = 131 : element(9, 1) = 132 : element(10, 1) = 133 : element(11, 1) = 134

        element(0, 2) = 35 : element(1, 2) = 36 : element(2, 2) = 184 : element(3, 2) = 185 : element(4, 2) = 42 : element(5, 2) = 43
        element(6, 2) = 44 : element(7, 2) = 45 : element(8, 2) = 46 : element(9, 2) = 47 : element(10, 2) = 48 : element(11, 2) = 49

        element(0, 3) = 64 : element(1, 3) = 65 : element(2, 3) = 66 : element(3, 3) = 67 : element(4, 3) = 68 : element(5, 3) = 69
        element(6, 3) = 70 : element(7, 3) = 71 : element(8, 3) = 72 : element(9, 3) = 73 : element(10, 3) = 74 : element(11, 3) = 75

        element(0, 4) = 180 : element(1, 4) = 181 : element(2, 4) = 182 : element(3, 4) = 183 : element(4, 4) = 33 : element(5, 4) = 34
        element(6, 4) = 0 : element(7, 4) = 0 : element(8, 4) = 0 : element(9, 4) = 0 : element(10, 4) = 0 : element(11, 4) = 0

        element(0, 5) = 0 : element(1, 5) = 0 : element(2, 5) = 0 : element(3, 5) = 0 : element(4, 5) = 0 : element(5, 5) = 0
        element(6, 5) = 0 : element(7, 5) = 0 : element(8, 5) = 0 '

        'reserved for tekst horizontal and vertical, blanco, horizontal and vertical block !!! must be the 5 last elements !!!
        element(7, 5) = 23 : element(8, 5) = 24 : element(9, 5) = 100 : element(10, 5) = 1 : element(11, 5) = 2

    End Sub

    Private Sub pE_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pE.MouseDown

        Dim g As Graphics = pbActive.CreateGraphics()
        g.Clear(Color.White)
        Dim x As Integer = e.X \ gridStep
        Dim y As Integer = e.Y \ gridStep
        Dim s As String = String.Empty
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                'plaats in actieve cell
                DrawElement(cv, g)
                xE = x : yE = y     'bewaar voor overdracht naar neuwe canvas
                s = "Left"
            Case Windows.Forms.MouseButtons.Right
                s = "right"
            Case Windows.Forms.MouseButtons.Middle
                s = "middle"
        End Select
        MDIParentfrm.mdiParentStatus.Text = "GridCell x = " & x & ",  y = " & y & "   Muisknop gedrukt: " & s

    End Sub

    Private Sub pE_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pE.Paint

        For j As Integer = 0 To 5    'draw all elements
            For i As Integer = 0 To 11
                If j = 5 And (i = 10 Or i = 11) Then Exit For
                If element(i, j) > 0 Then
                    cv.posX = i : cv.posY = j : cv.gridStep = gridStep
                    cv.status = "*" : cv.strInfo = "*" : cv.direction = "*"
                    cv.elementNR = element(i, j) : cv.elementLenght = 1
                    cv.itemNR = 0.ToString : cv.locoNR = 0.ToString
                    cv.active = False
                    DrawElement(cv, e.Graphics)
                End If
            Next
        Next
        DrawGrid(e.Graphics)
    End Sub

#End Region

#Region "Buttons click"

    Private Sub btnHorizontaal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHorizontaal.Click
        Dim g As Graphics = pbActive.CreateGraphics()
        g.Clear(Color.LightGray)
        g.DrawString(" H", blockFont, blockBrush, 0, gridText)
        xE = 10 : yE = 5     'bewaar voor overdracht naar neuwe canvas
        blockLenght = CInt(txtMaat.Text)
    End Sub

    Private Sub btnVertikaal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVertikaal.Click
        Dim g As Graphics = pbActive.CreateGraphics()
        g.Clear(Color.LightGray)
        g.DrawString(" V", blockFont, blockBrush, 0, gridText)
        xE = 11 : yE = 5     'bewaar voor overdracht naar neuwe canvas
        blockLenght = CInt(txtMaat.Text)
    End Sub

    Private Sub btnTekstH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTekstH.Click
        Dim g As Graphics = pbtekst.CreateGraphics()
        g.Clear(Color.LightGray)
        g.DrawString("TH", blockFont, blockBrush, 0, gridText)
        xE = 7 : yE = 5     'bewaar voor overdracht naar neuwe canvas
        TekstLenght = CInt(txtLengte.Text)
    End Sub

    Private Sub btnTekstV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTekstV.Click
        Dim g As Graphics = pbtekst.CreateGraphics()
        g.Clear(Color.LightGray)
        g.DrawString("TV", blockFont, blockBrush, 0, gridText)
        xE = 8 : yE = 5     'bewaar voor overdracht naar neuwe canvas
        TekstLenght = CInt(txtLengte.Text)
    End Sub

#End Region

#Region "DrawElements"

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
        '    Public cv.strInfo As String
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
            Case "V"      'fre Vrij
                elementPenStatus.Color = colorFre
                blockFillBrush = Brushes.LightGreen
            Case "G"     'reserved Gereserveerd
                elementPenStatus.Color = colorReserved
                blockFillBrush = Brushes.Orange
            Case "B"     'occupied Bezet
                elementPenStatus.Color = colorOccupied
                blockFillBrush = Brushes.Blue
            Case "U"     'maintenance Onderhoud
                elementPenStatus.Color = colormaintenance
                blockFillBrush = Brushes.Magenta
            Case "S"      'error Spook
                elementPenStatus.Color = colorError
                blockFillBrush = Brushes.Red
            Case "I"      'error Onbepaald
                elementPenStatus.Color = Color.BlueViolet
                blockFillBrush = Brushes.BlueViolet
            Case "A"      'Active 
                elementPenStatus.Color = Color.DarkRed
                blockFillBrush = Brushes.LightGreen
            Case Else   'init gebruik *
                elementPenStatus.Color = Color.DarkGray
                blockFillBrush = Brushes.DarkGray
        End Select

        Select Case cv.elementNR
            'block elements
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
                'Case 23  'horizontal text
                '    gr.FillRectangle(Brushes.LightYellow, X1, Y1, gridStep * cv.elementLenght, gridStep)
                '    gr.FillRectangle(Brushes.Yellow, X1, Y1, gridStep, gridStep)
                '    gr.DrawRectangle(textPen, X1, Y1, gridStep * cv.elementLenght, gridStep)
                '    gr.DrawString(cv.strInfo, textFont, textBrush, X1, Y1 + gridText)
                'Case 24  'vertical text
                '    gr.FillRectangle(Brushes.LightYellow, X1, Y1, gridStep, gridStep * cv.elementLenght)
                '    gr.FillRectangle(Brushes.Yellow, X1, Y1, gridStep, gridStep)
                '    gr.DrawRectangle(textPen, X1, Y1, gridStep, gridStep * cv.elementLenght)
                '    gr.DrawString(cv.strInfo, textFont, textBrush, X1, Y1, blockStrFormat)

                'text elements
            Case 23  'horizontal text
                gr.FillRectangle(Brushes.LightYellow, X1, Y1, gridStep * cv.elementLenght, gridStep)
                gr.FillRectangle(Brushes.LightGreen, X1, Y1, gridStep, gridStep)
                gr.DrawRectangle(textPen, X1, Y1, gridStep * cv.elementLenght, gridStep)
                gr.DrawString(cv.strInfo, textFont, textBrush, XS, Y1 + gridText)
            Case 24  'vertical text
                gr.FillRectangle(Brushes.LightYellow, X1, Y1, gridStep * cv.elementLenght, gridStep)
                gr.FillRectangle(Brushes.LightGreen, X1, Y1, gridStep, gridStep)
                gr.DrawRectangle(textPen, X1, Y1, gridStep, gridStep * cv.elementLenght)
                gr.DrawString(cv.strInfo, textFont, textBrush, X1, YS, blockStrFormat)

                'engelse wissels
            Case 33  'kruising vertikaal, links boven naar rechts onder
                Select Case cv.direction
                    Case "v"
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                        'gr.DrawLine(elementPenFre, X1H, Y1H, XSH, YSH)
                    Case "h"
                        gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                    Case Else
                        gr.DrawLine(elementPen, X1, Y1, XS, YS)
                        gr.DrawLine(elementPen, XU, Y1, XU, YS)
                End Select
            Case 34  'kruising vertikaal, links onder naar rechts boven
                Select Case cv.direction
                    Case "v"
                        gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                        gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)
                        'gr.DrawLine(elementPenFre, X1H, Y1H, XSH, YSH)
                    Case "h"
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
            Case 184    'Andreus kruising
                gr.DrawLine(elementPenStatus, X1, YS, XS, Y1)
                gr.DrawLine(elementPenStatus, X1, Y1, XS, YS)
            Case 185  'kruising horizontaal, vertikaal
                gr.DrawLine(elementPenStatus, X1, YU, XS, YU)
                gr.DrawLine(elementPenStatus, XU, Y1, XU, YS)

            Case Else
                ' number not in use
        End Select
    End Sub

    Private Sub DrawGrid(ByVal gr As Graphics)
        Dim Xmax As Integer = gridStep * 12
        Dim Ymax As Integer = gridStep * 6
        Dim distance As Integer = 0
        For i = 0 To 6
            gr.DrawLine(gridPen, 0, distance, Xmax, distance)
            distance += gridStep
        Next
        distance = 0
        For i = 0 To 12
            gr.DrawLine(gridPen, distance, 0, distance, Ymax)
            distance += gridStep
        Next
    End Sub     'paint the grid

#End Region

End Class