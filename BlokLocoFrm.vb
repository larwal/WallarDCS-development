
Public Class BlokLocoFrm
    'vars
    Private index As Integer

    Private Sub BlokLocoFrm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Integer = 0
        SyncLock syncOK1
            For i = 0 To g_MaxBLA
                lstBlokLocoData.Items.Add(g_BlokLocoArray(i))
            Next
        End SyncLock
    End Sub

    Private Sub btnVerlaat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVerlaat.Click
        Dim i As Integer = 0
        Dim status As String = String.Empty
        SyncLock syncOK1
            For i = 1 To lstBlokLocoData.Items.Count - 1
                g_BlokLocoArray(i) = CStr(lstBlokLocoData.Items.Item(i))
                m_blok(SynopIsblokNR).locoNR = CInt(g_BlokLocoArray(i).Substring(4, 3))

                'geef de status van het blok door aan de sub BlokStatus(,,) van in dienst zijnde synoptieken op het scherm
                For j As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
                    If synopMenuItemInUse(j) Then synop(j).BlokStatus(m_blok(i).statusSynop, CInt(g_BlokLocoArray(i).Substring(0, 3)), CInt(g_BlokLocoArray(i).Substring(4, 3)))
                Next
            Next
        End SyncLock
        Me.Close()
    End Sub

    Private Sub btnPasAan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPasAan.Click
        If CInt(txtBlok.Text) > 0 Then
            If Not IsBlokAddressOk(txtBlok.Text, m_maxBlok) Then
                txtBlok.Focus()
                txtBlok.SelectAll()
                Exit Sub
            End If
            If Not txtLoco.Text = "000" Then
                If Not IsLocoAddressOk(txtLoco.Text) Then
                    txtLoco.Focus()
                    txtLoco.SelectAll()
                    Exit Sub
                End If
            End If
            lstBlokLocoData.Items.Item(index) = Format(CInt(txtBlok.Text), "000") + " " + Format(CInt(txtLoco.Text), "000")
        End If
    End Sub

    Private Sub lstBlokLocoData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstBlokLocoData.Click
        index = lstBlokLocoData.SelectedIndex
        If index > 0 Then
            txtBlok.Text = CStr(lstBlokLocoData.Items.Item(index)).Substring(0, 3)
            txtLoco.Text = CStr(lstBlokLocoData.Items.Item(index)).Substring(4, 3)
            txtLoco.Focus()
            txtLoco.SelectAll()
        End If
    End Sub
End Class