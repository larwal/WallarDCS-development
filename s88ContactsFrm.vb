Public Class s88ContactsFrm


    'vars
    Private ColonsHeaderTextBox As TextBox
    Private s88ContactsTextBox As TextBox

    Private Sub s88ContactsFrm_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        s88contactsView = False
        Me.Dispose()
    End Sub

    Private Sub s88ContactsFrm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim basis As Integer = 0
        For i As Integer = 1 To MAXS88COLONS
            txtColons.Text += "s88 [Blok]     waarde           "
        Next
    End Sub

End Class