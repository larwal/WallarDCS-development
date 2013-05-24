'(c)    Wallar  2011-08-28 
Imports System.Threading.Thread

Public Class CommandoChildfrm

    Private Sub CommandoChildfrm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'laadt alle macro's in lstMacro
        Dim data() As String
        Dim s As String
        Dim cr As Integer

        For i As Integer = 1 To CInt(g_macroArray(0))
            data = g_macroArray(i).Split(CChar(vbTab))
            'analiseer
            s = data(2)
            cr = s.LastIndexOf(vbCr)
            lstMacro.Items.Add((i).ToString + vbTab + s.Substring(0, cr))
        Next
    End Sub

    Private Sub btnGO_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGO.Click
        VoeruitLocoCMD(UpDownSpeed.Value.ToString)
    End Sub     ' ToDo

    Private Sub btnSTOP_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSTOP.Click
        VoeruitLocoCMD("0")
    End Sub     ' ToDo

    Private Sub btnStraightTurnout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStraightTurnout.Click
        If IsDecoderNrOk(txtAddressTournout.Text) Then
            dataD.address = txtAddressTournout.Text
            dataD.port = "1"
            DCS.SetTurnout(dataD)
        End If
        txtAddressTournout.Focus()
        txtAddressTournout.SelectAll()
        MDIParentfrm.AdaptTurnoutStatus("M", "1", dataD.address)
    End Sub

    Private Sub btnBendingTurnout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBendingTurnout.Click
        If IsDecoderNrOk(txtAddressTournout.Text) Then
            dataD.address = txtAddressTournout.Text
            dataD.port = "0"
            DCS.SetTurnout(dataD)
        End If
        txtAddressTournout.Focus()
        txtAddressTournout.SelectAll()
        MDIParentfrm.AdaptTurnoutStatus("M", "0", dataD.address)
    End Sub

    Private Sub TabControl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl.Click
        txtLocoAddress.Focus()
        txtLocoAddress.SelectAll()
        txtAddressTournout.Focus()
        txtAddressTournout.SelectAll()
        txtVan.Focus()
        txtVan.SelectAll()
        txtMacroNR.Focus()
        txtMacroNR.SelectAll()
    End Sub

    Private Sub TabPage1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage1.Enter
        txtLocoAddress.Focus()
        txtLocoAddress.SelectAll()
    End Sub

    Private Sub TabPage2_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage2.Enter
        txtAddressTournout.Focus()
        txtAddressTournout.SelectAll()
    End Sub

    Private Sub TabPage3_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage3.Enter
        txtNaar.Text = m_maxBlok.ToString
        txtVan.Focus()
        txtVan.SelectAll()
    End Sub

    Private Sub btnVrijGO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVrijGO.Click
        Dim vanBL, isBL, naarBL As Integer
        Dim reisplan, D As String

        If IsBlokAddressOk(txtVan.Text, m_maxBlok) Then
            vanBL = CInt(txtVan.Text)
        Else
            txtVan.Focus()
            txtVan.SelectAll()
            Beep()
            Exit Sub
        End If

        If IsBlokAddressOk(txtIS.Text, m_maxBlok) Then
            isBL = CInt(txtIS.Text)
        Else
            txtIS.Focus()
            txtIS.SelectAll()
            Beep()
            Exit Sub
        End If

        If IsBlokAddressOk(txtNaar.Text, m_maxBlok) Then
            naarBL = CInt(txtNaar.Text)
        Else
            txtNaar.Focus()
            txtNaar.SelectAll()
            Beep()
            Exit Sub
        End If
        If radVooruit.Checked Then D = "V" Else D = "A"

        'samenstellen reisplan "iii+00>nnnDvvv" waarin iii=ISblok, +00>=deltasnelheid, nnn=NaarBlok, D=rijrichting, vvv=vanBlok rest scheidingstekens
        reisplan = Format(isBL, "000") + "+00>" + Format(naarBL, "000") + D + Format(vanBL, "000")
        DCS.TreinInDienstName(0, reisplan)

    End Sub

    Private Sub VoeruitLocoCMD(ByVal speed As String)
        Dim locoNR As Integer = 0
        If IsLocoAddressOk(txtLocoAddress.Text) Then
            locoNR = CInt(txtLocoAddress.Text)
            m_ArrayLOCO(locoNR).address = txtLocoAddress.Text
        Else
            txtLocoAddress.Focus()
            txtLocoAddress.SelectAll()
            Exit Sub
        End If
        m_ArrayLOCO(locoNR).speed = speed
        If rBtnVooruit.Checked Then m_ArrayLOCO(locoNR).direction = "forwards" Else m_ArrayLOCO(locoNR).direction = "rearwards"
        If chkF0.Checked Then m_ArrayLOCO(locoNR).F0 = "1" Else m_ArrayLOCO(locoNR).F0 = "0"
        If chkF1.Checked Then m_ArrayLOCO(locoNR).F1 = "1" Else m_ArrayLOCO(locoNR).F1 = "0"
        If chkF2.Checked Then m_ArrayLOCO(locoNR).F2 = "1" Else m_ArrayLOCO(locoNR).F2 = "0"
        If chkF3.Checked Then m_ArrayLOCO(locoNR).F3 = "1" Else m_ArrayLOCO(locoNR).F3 = "0"
        If chkF4.Checked Then m_ArrayLOCO(locoNR).F4 = "1" Else m_ArrayLOCO(locoNR).F4 = "0"
        DCS.SetLoco(locoNR)
        txtLocoAddress.Focus()
        txtLocoAddress.SelectAll()
    End Sub

    Private Sub rBtnAchteruit_Click(sender As Object, e As System.EventArgs) Handles rBtnAchteruit.Click
        rBtnVooruit.Checked = False
        txtLocoAddress.Focus()
        txtLocoAddress.SelectAll()
        VoeruitLocoCMD("0")
    End Sub

    Private Sub rBtnVooruit_Click(sender As Object, e As System.EventArgs) Handles rBtnVooruit.Click
        rBtnVooruit.Checked = True
        txtLocoAddress.Focus()
        txtLocoAddress.SelectAll()
        VoeruitLocoCMD("0")
    End Sub

    Private Sub btnVoeruitMacro_Click(sender As System.Object, e As System.EventArgs) Handles btnVoeruitMacro.Click
        If CInt(txtMacroNR.Text) > CInt(g_macroArray(0)) OrElse CInt(txtMacroNR.Text) < 1 Then
            txtMacroNR.Focus()
            txtMacroNR.SelectAll()
            Exit Sub
        End If
        VoerMacroUit(CInt(txtMacroNR.Text))
    End Sub

    Private Sub VoerMacroUit(ByVal macroNR As Integer)
        'voer macro uit
        DCS.SetMacro(macroNR)               'eerste macro staat op index+1
    End Sub

    Private Sub LlstMacro_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstMacro.SelectedIndexChanged
        Dim index As Integer = lstMacro.SelectedIndex + 1    'eerste macro
        txtMacroNR.Text = index.ToString
        VoerMacroUit(index)
    End Sub

    Private Sub chkF0_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkF0.CheckedChanged
        VoeruitLocoCMD("0")
    End Sub

End Class