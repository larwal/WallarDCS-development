Public Class VrijTreinSynopFrm
    'vars

    Private Sub VrijTreinSynopFrm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtNaarBlokNR.Text = m_maxBlok.ToString
        txtVanBlokNR.Text = SynopIsblokNR.ToString
        txtVanBlokNR.Focus()
        txtVanBlokNR.SelectAll()
    End Sub

    Private Sub btnVoeruit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVoeruit.Click
        Dim reisplan As String = String.Empty
        Dim naarBL, vanBL As Integer
        If Not IsBlokAddressOk(txtVanBlokNR.Text, m_maxBlok) Then
            txtVanBlokNR.Focus()
            txtVanBlokNR.SelectAll()
            Exit Sub
        End If
        If Not IsBlokAddressOk(txtNaarBlokNR.Text, m_maxBlok) Then
            txtNaarBlokNR.Focus()
            txtNaarBlokNR.SelectAll()
            Exit Sub
        End If
        vanBL = CInt(txtVanBlokNR.Text) : naarBL = CInt(txtNaarBlokNR.Text)
        'samenstellen reisplan "iii+00>nnnDvvv" waarin iii=ISblok, +00>=deltasnelheid, nnn=NaarBlok, D=rijrichting, vvv=vanBlok rest scheidingstekens
        reisplan = Format(SynopIsblokNR, "000") + "+00>" + Format(naarBL, "000") + "V" + Format(vanBL, "000")
        DCS.TreinInDienstName(0, reisplan)
        Me.Close()
    End Sub

End Class