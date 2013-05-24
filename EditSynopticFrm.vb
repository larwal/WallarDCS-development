Imports System.Windows.Forms

Public Class EditSynopticFrm

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Dim nsProp As New NewSynopPropertiesFrm
        wijzigAppearance = True       'wijziging appearance
        'read actual data uit gekozen strSynopgroup(x)
        Dim str As String = CStr(lstBox.SelectedItem)
        If str = Nothing Then Exit Sub
        Select Case CType(str.Substring(0, 1), CanvasAppearances)
            Case CanvasAppearances.Fitscreen
                nsProp.lstAppearance.SetSelected(0, True)
            Case CanvasAppearances.Standaard
                nsProp.lstAppearance.SetSelected(1, True)
            Case CanvasAppearances.Maximized
                nsProp.lstAppearance.SetSelected(2, True)
            Case Else
        End Select
        Select Case str.Substring(2, 2)
            Case "12"
                nsProp.rb12.Checked = True
            Case "16"
                nsProp.rb16.Checked = True
            Case "20"
                nsProp.rb20.Checked = True
            Case "24"
                nsProp.rb24.Checked = True
            Case "28"
                nsProp.rb28.Checked = True
            Case "32"
                nsProp.rb32.Checked = True
        End Select
        Dim i As Integer = str.IndexOf("|", 5)
        'enkel de titel niet de 3 eerste "index" cijfers mogen veranderd worden
        Dim index As Integer = CInt(str.Substring(5, 3))
        nsProp.txtTitle.Text = str.Substring(9, i - 9)
        nsProp.txtLabel.Text = str.Substring(i + 1, str.Length - i - 2)
        If nsProp.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            str = (CInt(nsProp.lstAppearance.SelectedIndex) + 1).ToString & "|"
            If nsProp.rb12.Checked Then str &= "12|"
            If nsProp.rb16.Checked Then str &= "16|"
            If nsProp.rb20.Checked Then str &= "20|"
            If nsProp.rb24.Checked Then str &= "24|"
            If nsProp.rb28.Checked Then str &= "28|"
            If nsProp.rb32.Checked Then str &= "32|"
            str &= Format(index, "000") & " " & nsProp.txtTitle.Text & "|"
            str &= nsProp.txtLabel.Text & "|"
            strSynopGroup(index + 1) = str
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class
