Public Class SynopBlokLocoFrm

    Private Sub SynopBlokLocoFrm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtBlokNR.Text = SynopIsblokNR.ToString
        txtLocoNR.Text = SynopIsLocoNR.ToString
    End Sub

    Private Sub btnVoeruit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVoeruit.Click
        If Not CInt(txtLocoNR.Text) = 0 Then
            If Not IsLocoAddressOk(txtLocoNR.Text) Then
                txtLocoNR.Focus()
                txtLocoNR.SelectAll()
                Exit Sub
            End If
        End If
        'is de loco indienst
        Dim LocoNR As Integer = CInt(txtLocoNR.Text)
        If g_locoData(LocoNR).locoInDienst = 1 Then     'loco bestaat
            g_BlokLocoArray(SynopIsblokNR) = Format(SynopIsblokNR, "000") + " " + Format(CInt(txtLocoNR.Text), "000")
            m_blok(SynopIsblokNR).locoNR = CInt(txtLocoNR.Text)
            'geef de status van het blok door aan de sub BlokStatus(,,) van in dienst zijnde synoptieken op het scherm
            DCS.SetBlokGegevens(SynopIsblokNR, CInt(txtLocoNR.Text), 2)
            For j As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
                If synopMenuItemInUse(j) Then synop(j).BlokStatus(m_blok(SynopIsblokNR).statusSynop, SynopIsblokNR, CInt(txtLocoNR.Text))
            Next
        End If
        Me.Close()
    End Sub

End Class