'WallarDCS 2011 (c)

Public Class NewSynopPropertiesFrm

    Private Sub NewSynopPropertiesFrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not wijzigAppearance Then lstAppearance.SetSelected(1, True) : wijzigAppearance = False 'nieuwe synoptiek (geen wijziging bestaande)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Dispose()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'collect choosen values and put in canvasProperty
        If rb12.Checked Then
            canvasProperty.gridStep = 12
        ElseIf rb16.Checked Then
            canvasProperty.gridStep = 16
        ElseIf rb24.Checked Then
            canvasProperty.gridStep = 24
        ElseIf rb28.Checked Then
            canvasProperty.gridStep = 28
        ElseIf rb32.Checked Then
            canvasProperty.gridStep = 32
        Else
            canvasProperty.gridStep = 20
        End If
        canvasProperty.labelText = txtLabel.Text
        canvasProperty.tittle = txtTitle.Text
        Select Case lstAppearance.SelectedIndices(0)
            Case 0
                canvasProperty.appearance = CanvasAppearances.Fitscreen
            Case 1
                canvasProperty.appearance = CanvasAppearances.Standaard
            Case 2
                canvasProperty.appearance = CanvasAppearances.Maximized
            Case Else
                canvasProperty.appearance = CanvasAppearances.Standaard
        End Select
    End Sub

End Class