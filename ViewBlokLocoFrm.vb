Public Class ViewBlokLocoFrm

    Private Sub ViewBlokLocoFrm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

    Private Sub ViewBlokLocoFrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim s As String = String.Empty
        lstBlokLoco.Items.Add("Blok ===> Loco")
        SyncLock syncOK1
            For i As Integer = 1 To g_BlokLocoArray.GetUpperBound(0) - 1
                If g_BlokLocoArray(i).Substring(4, 3) <> "000" Then
                    s = g_BlokLocoArray(i).Substring(0, 3) + "  ===> " + g_BlokLocoArray(i).Substring(4, 3)
                    lstBlokLoco.Items.Add(s)
                End If
            Next
        End SyncLock
    End Sub
End Class