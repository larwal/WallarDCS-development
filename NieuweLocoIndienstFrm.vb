Public Class NieuweLocoIndienstFrm

    Private Sub NieuweLocoIndienstname_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

    Private Sub NieuweLocoIndienstname_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        txtBlokNR.Text = SynopIsblokNR.ToString
        If SynopIsblokNR <> 0 Then
            txtBlokNR.Enabled = False
        End If
        txtLocoNR.Focus()
        txtLocoNR.SelectAll()
    End Sub

    Private Sub btnIndienstname_Click(sender As System.Object, e As System.EventArgs) Handles btnIndienstname.Click
        'controleer geldigheid ingaves
        Try
            If Not IsLocoAddressOk(txtLocoNR.Text) Then
                txtLocoNR.Focus()
                txtLocoNR.SelectAll()
                Exit Sub
            End If

            SyncLock syncOK
                If g_locoData(CInt(txtLocoNR.Text)).LocoInDienst = 0 Then
                    txtLocoNR.Focus()
                    txtLocoNR.SelectAll()
                    Exit Sub
                End If

            End SyncLock
            If Not IsBlokAddressOk(txtBlokNR.Text, m_maxBlok) Then
                txtBlokNR.Focus()
                txtBlokNR.SelectAll()
                Exit Sub
            End If
            SyncLock syncOK
                instellingen.OHblokken += "|" + Format(CInt(txtBlokNR.Text), "000")
            End SyncLock
            DCS.NieuweLocoInDienstName(CInt(txtLocoNR.Text), CInt(txtBlokNR.Text), radPaar.Checked)
            Me.Close()

        Catch ex As Exception
            MsgBox("Ex: " & ex.ToString)
        End Try

    End Sub
End Class