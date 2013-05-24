Public Class QueryBlockLocofrm

    Private Sub QueryBlockLocofrm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

    Private Sub QueryBlockLoco_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim blokNR As Integer = 0
        Dim s As String = String.Empty
        SyncLock syncOK1
            For i As Integer = 0 To g_BlokLocoArray.GetUpperBound(0) - 1
                If g_BlokLocoArray(i).Substring(4, 3) <> "000" Then
                    s = "  " + g_BlokLocoArray(i).Substring(4, 3) + "   ===>  " + g_BlokLocoArray(i).Substring(0, 3)
                    lstLocoBlok.Items.Add(s)
                End If
            Next
        End SyncLock
    End Sub

    Private Sub btnShowOnSynop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowOnSynop.Click
        Dim item As String = CStr(lstLocoBlok.SelectedItem)
        If Not IsNothing(item) Then
            Dim blokNR As Integer = CInt(item.Substring(14, 3))
            Dim locoNR As Integer = CInt(item.Substring(2, 3))
            'geef de status van het blok door aan de sub BlokStatus(,,) van in dienst zijnde synoptieken op het scherm
            For i As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
                If synopMenuItemInUse(i) Then synop(i).BlokStatus("§", blokNR, locoNR)
            Next
        End If
    End Sub

End Class