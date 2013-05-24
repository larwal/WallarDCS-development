Imports System.Windows.Forms

Public Class NewSynopticContext

    Private Sub NewSynopticContext_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.txtNR.Focus()
        Me.txtNR.SelectAll()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        'bewaar alle data
        If Not IsNumberOK(txtNR.Text) Then txtNR.Focus() : txtNR.SelectAll() : Exit Sub
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
