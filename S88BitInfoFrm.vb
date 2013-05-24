Public Class S88BitInfoFrm

    Private Sub S88BitInfoFrm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

    Private Sub S88BitInfoFrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'opvullen lstS88BitOLD en lstS88BitNEW
        Try
            For i As Integer = 0 To s88BitValuesOLD.Count - 1
                lstS88BitOLD.Items.Add(i.ToString + vbTab + s88BitValuesOLD.Item(i).ToString + vbTab + s88BitValuesNEW.Item(i).ToString)
            Next
        Catch ex As Exception
            'verlaat sub
        End Try
    End Sub

End Class