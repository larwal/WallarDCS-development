Imports System.IO
Imports Microsoft.VisualBasic.Devices


Public Class ReisplanFrm

    Private Sub ReisplanFrm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Dispose()
    End Sub

    Private Sub ReisplanFrm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'reisplannen laden in listbox
        For i = 1 To g_ReisplannenText.Length - 1
            chkLstReisplannen.Items.Add(g_ReisplannenText(i))
        Next
        chkLstReisplannen.Focus()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Dispose()
    End Sub

    Private Sub btnOK_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        For i As Integer = 0 To chkLstReisplannen.Items.Count - 1
            If CBool(chkLstReisplannen.GetItemCheckState(i)) Then
                DCS.TreinInDienstName(0, g_ReisPlannenArray(i + 1))
            End If
        Next
        Me.Dispose()

    End Sub

End Class