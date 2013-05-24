Imports System.IO

Public Class Optionsfrm

    'options variables

#Region "Form Loading and Closing"

    Private Sub Optionsfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DisplayInstellingen()
    End Sub

    Private Sub Optionsfrm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

#End Region

#Region "Bedieningsknoppen"

    Private Sub btnLoadInstellingen_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadInstellingen.Click
        LoadInstellingen()
    End Sub

    Private Sub btnSaveInstellingen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveInstellingen.Click
        SaveInstellingen()
    End Sub

    Private Sub btnKleurSynopLaden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        LoadInstellingen()
    End Sub

    Private Sub lblBezet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblBezet.Click
        Dim kleur As Color = SetColorSynop(lblBezet.BackColor)
        instellingen.ColorBezet = kleur.ToArgb.ToString        'steek in instellingen
        lblBezet.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblAfremBezet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblAfremBezet.Click
        Dim kleur As Color = SetColorSynop(lblAfremBezet.BackColor)
        instellingen.ColorAfremBezet = kleur.ToArgb.ToString        'steek in instellingen
        lblAfremBezet.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblGereserveerd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblGereserveerd.Click
        Dim kleur As Color = SetColorSynop(lblGereserveerd.BackColor)
        instellingen.ColorGereserveerd = kleur.ToArgb.ToString        'steek in instellingen
        lblGereserveerd.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblPrereserved_Click(sender As System.Object, e As System.EventArgs) Handles lblPrereserved.Click
        Dim kleur As Color = SetColorSynop(lblPrereserved.BackColor)
        instellingen.ColorPreReserved = kleur.ToArgb.ToString        'steek in instellingen
        lblPrereserved.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblVergrendeld_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblVergrendeld.Click
        Dim kleur As Color = SetColorSynop(lblVergrendeld.BackColor)
        instellingen.ColorVergrendeld = kleur.ToArgb.ToString        'steek in instellingen
        lblVergrendeld.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblOnderhoud_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblOnderhoud.Click
        Dim kleur As Color = SetColorSynop(lblOnderhoud.BackColor)
        instellingen.ColorOnderhoud = kleur.ToArgb.ToString        'steek in instellingen
        lblOnderhoud.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblFout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblFout.Click
        Dim kleur As Color = SetColorSynop(lblFout.BackColor)
        instellingen.ColorSpook = kleur.ToArgb.ToString        'steek in instellingen
        lblFout.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblBlockPen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblBlockPen.Click
        Dim kleur As Color = SetColorSynop(lblBlockPen.BackColor)
        instellingen.colorBlockPen = kleur.ToArgb.ToString        'steek in instellingen
        lblBlockPen.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblhalt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblHalt.Click
        Dim kleur As Color = SetColorSynop(lblHalt.BackColor)
        instellingen.colorHalt = kleur.ToArgb.ToString        'steek in instellingen
        lblHalt.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblPassief_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblPassief.Click
        Dim kleur As Color = SetColorSynop(lblPassief.BackColor)
        instellingen.colorPassief = kleur.ToArgb.ToString        'steek in instellingen
        lblPassief.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblInitialisatie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblInitialisatie.Click
        Dim kleur As Color = SetColorSynop(lblInitialisatie.BackColor)
        instellingen.ColorInit = kleur.ToArgb.ToString        'steek in instellingen
        lblInitialisatie.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblInitBlok_Click(sender As System.Object, e As System.EventArgs) Handles lblInitBlok.Click
        Dim kleur As Color = SetColorSynop(lblInitBlok.BackColor)
        instellingen.ColorInitBlok = kleur.ToArgb.ToString        'steek in instellingen
        lblInitBlok.BackColor = kleur        'steek in label

    End Sub

    Private Sub lblGridLijnen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblGridLijnen.Click
        Dim kleur As Color = SetColorSynop(lblGridLijnen.BackColor)
        instellingen.ColorGridLijnen = kleur.ToArgb.ToString        'steek in instellingen
        lblGridLijnen.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblAchtergrondSynop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblAchtergrondSynop.Click
        Dim kleur As Color = SetColorSynop(lblAchtergrondSynop.BackColor)
        instellingen.ColorAchtergrondSynop = kleur.ToArgb.ToString        'steek in instellingen
        lblAchtergrondSynop.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblAchtergrondElement_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblAchtergrondElement.Click
        Dim kleur As Color = SetColorSynop(lblAchtergrondElement.BackColor)
        instellingen.ColorAchtergrondElement = kleur.ToArgb.ToString        'steek in instellingen
        lblAchtergrondElement.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblLocatieTrein_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblLocatieTrein.Click
        Dim kleur As Color = SetColorSynop(lblLocatieTrein.BackColor)
        instellingen.ColorLocatieTrein = kleur.ToArgb.ToString        'steek in instellingen
        lblLocatieTrein.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblHandbediening_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblHandbediening.Click
        Dim kleur As Color = SetColorSynop(lblHandbediening.BackColor)
        instellingen.ColorHandbediening = kleur.ToArgb.ToString        'steek in instellingen
        lblHandbediening.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblVrij_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblVrij.Click
        Dim kleur As Color = SetColorSynop(lblVrij.BackColor)
        instellingen.ColorVrij = kleur.ToArgb.ToString        'steek in instellingen
        lblVrij.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblRetrigger_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblRetrigger.Click
        Dim kleur As Color = SetColorSynop(lblRetrigger.BackColor)
        instellingen.ColorRetrigger = kleur.ToArgb.ToString        'steek in instellingen
        lblRetrigger.BackColor = kleur        'steek in label
    End Sub

    Private Sub lblTextback_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblTextback.Click
        Dim kleur As Color = SetColorSynop(lblTextback.BackColor)
        instellingen.ColorTextBack = kleur.ToArgb.ToString        'steek in instellingen
        lblTextback.BackColor = kleur        'steek in label
    End Sub

#End Region

#Region "Common subs and functions"

    Private Sub DisplayInstellingen()
        TxtHsi88Usb.Text = instellingen.LeaveHSI88USB
        txtmodLeft.Text = instellingen.s88modulesLinks
        txtmodMidden.Text = instellingen.s88modulesMidden
        txtmodRechts.Text = instellingen.s88modulesRechts
        txtTreinStartVertraging.Text = instellingen.startVertraging
        txtCargoVerlaging.Text = instellingen.GoederentreinSnelheidsverlaging
        txtAfremsnelheid.Text = instellingen.StopAfremSnelheid
        txtMacroStart.Text = instellingen.MacroStart
        txtMacroStop.Text = instellingen.MacroStop
        txtMacroFout.Text = instellingen.MacroFout
        txtHaltInStation.Text = instellingen.HaltInStation
        txtBlokkenOH.Text = instellingen.OHblokken
        txtk8055Aanwezig.Text = instellingen.K8055
        txtBoard0.Text = instellingen.k8055B0
        txtBoard1.Text = instellingen.k8055B1
        txtBoard2.Text = instellingen.k8055B2
        txtBoard3.Text = instellingen.k8055B3
        txtSchaduwUitDienstTijd.Text = instellingen.SchaduwUitDienstTijd
        txtTurnoutPulsTime.Text = instellingen.turnoutPulsTime
        txtTempoRegeling.Text = instellingen.tempoRegeling
        txtPendikte.Text = instellingen.PenWidth
        lblVrij.BackColor = Color.FromArgb(CInt(instellingen.ColorVrij))
        lblBezet.BackColor = Color.FromArgb(CInt(instellingen.ColorBezet))
        lblAfremBezet.BackColor = Color.FromArgb(CInt(instellingen.ColorAfremBezet))
        lblGereserveerd.BackColor = Color.FromArgb(CInt(instellingen.ColorGereserveerd))
        lblPrereserved.BackColor = Color.FromArgb(CInt(instellingen.ColorPreReserved))
        lblVergrendeld.BackColor = Color.FromArgb(CInt(instellingen.ColorVergrendeld))
        lblOnderhoud.BackColor = Color.FromArgb(CInt(instellingen.ColorOnderhoud))
        lblFout.BackColor = Color.FromArgb(CInt(instellingen.ColorSpook))
        lblBlockPen.BackColor = Color.FromArgb(CInt(instellingen.colorBlockPen))
        lblHalt.BackColor = Color.FromArgb(CInt(instellingen.colorHalt))
        lblPassief.BackColor = Color.FromArgb(CInt(instellingen.colorPassief))
        lblInitialisatie.BackColor = Color.FromArgb(CInt(instellingen.ColorInit))
        lblInitBlok.BackColor = Color.FromArgb(CInt(instellingen.ColorInitBlok))
        lblGridLijnen.BackColor = Color.FromArgb(CInt(instellingen.ColorGridLijnen))
        lblAchtergrondSynop.BackColor = Color.FromArgb(CInt(instellingen.ColorAchtergrondSynop))
        lblAchtergrondElement.BackColor = Color.FromArgb(CInt(instellingen.ColorAchtergrondElement))
        lblHandbediening.BackColor = Color.FromArgb(CInt(instellingen.ColorHandbediening))
        lblRetrigger.BackColor = Color.FromArgb(CInt(instellingen.ColorRetrigger))
        lblTextback.BackColor = Color.FromArgb(CInt(instellingen.ColorTextBack))
        lblLocatieTrein.BackColor = Color.FromArgb(CInt(instellingen.ColorLocatieTrein))
        txtStatusColorTurnout.Text = instellingen.StatusColorTurnout
        txtUitvoeringsColommen.Text = instellingen.s88ColonRange
        If instellingen.gridlijnenTonen Then chkGridlijnenTonen.Checked = True Else chkGridlijnenTonen.Checked = False
        If instellingen.showRailElements Then chkShowRailElements.Checked = True Else chkShowRailElements.Checked = False
        If instellingen.K83K84detectie Then chkK83K84detectie.Checked = True Else chkK83K84detectie.Checked = False
        If instellingen.uitvoeringsmode Then chkUitvoeringsmode.Checked = True Else chkUitvoeringsmode.Checked = False
    End Sub

    Private Sub LoadInstellingen()
        'ProgramSetting data from file ProgramSetting.txt
        Dim myStreamReader As StreamReader
        Try
            myStreamReader = File.OpenText(DataDir & "\instellingen.txt")
            instellingen.LeaveHSI88USB = myStreamReader.ReadLine
            instellingen.s88modulesLinks = myStreamReader.ReadLine
            instellingen.s88modulesMidden = myStreamReader.ReadLine
            instellingen.s88modulesRechts = myStreamReader.ReadLine
            instellingen.startVertraging = myStreamReader.ReadLine
            instellingen.GoederentreinSnelheidsverlaging = myStreamReader.ReadLine
            instellingen.StopAfremSnelheid = myStreamReader.ReadLine
            instellingen.MacroStart = myStreamReader.ReadLine
            instellingen.MacroStop = myStreamReader.ReadLine
            instellingen.MacroFout = myStreamReader.ReadLine
            instellingen.HaltInStation = myStreamReader.ReadLine
            instellingen.OHblokken = myStreamReader.ReadLine
            instellingen.K8055 = myStreamReader.ReadLine
            instellingen.k8055B0 = myStreamReader.ReadLine
            instellingen.k8055B1 = myStreamReader.ReadLine
            instellingen.k8055B2 = myStreamReader.ReadLine
            instellingen.k8055B3 = myStreamReader.ReadLine
            instellingen.SchaduwUitDienstTijd = myStreamReader.ReadLine
            instellingen.turnoutPulsTime = myStreamReader.ReadLine
            instellingen.tempoRegeling = myStreamReader.ReadLine
            instellingen.PenWidth = myStreamReader.ReadLine
            instellingen.ColorVrij = myStreamReader.ReadLine
            instellingen.ColorBezet = myStreamReader.ReadLine
            instellingen.ColorAfremBezet = myStreamReader.ReadLine
            instellingen.ColorGereserveerd = myStreamReader.ReadLine
            instellingen.ColorPreReserved = myStreamReader.ReadLine
            instellingen.ColorVergrendeld = myStreamReader.ReadLine
            instellingen.ColorOnderhoud = myStreamReader.ReadLine
            instellingen.ColorSpook = myStreamReader.ReadLine
            instellingen.colorBlockPen = myStreamReader.ReadLine
            instellingen.colorHalt = myStreamReader.ReadLine
            instellingen.colorPassief = myStreamReader.ReadLine
            instellingen.ColorInit = myStreamReader.ReadLine
            instellingen.ColorInitBlok = myStreamReader.ReadLine
            instellingen.ColorGridLijnen = myStreamReader.ReadLine
            instellingen.ColorAchtergrondSynop = myStreamReader.ReadLine
            instellingen.ColorAchtergrondElement = myStreamReader.ReadLine
            instellingen.ColorHandbediening = myStreamReader.ReadLine
            instellingen.ColorRetrigger = myStreamReader.ReadLine
            instellingen.ColorTextBack = myStreamReader.ReadLine
            instellingen.ColorLocatieTrein = myStreamReader.ReadLine
            instellingen.StatusColorTurnout = myStreamReader.ReadLine
            instellingen.s88ColonRange = myStreamReader.ReadLine
            instellingen.gridlijnenTonen = CBool(myStreamReader.ReadLine)
            instellingen.showRailElements = CBool(myStreamReader.ReadLine)
            instellingen.K83K84detectie = CBool(myStreamReader.ReadLine)
            instellingen.uitvoeringsmode = CBool(myStreamReader.ReadLine)
            myStreamReader.Close()

            'Put in labels backcolor
            lblVrij.BackColor = Color.FromArgb(CInt(instellingen.ColorVrij))
            lblBezet.BackColor = Color.FromArgb(CInt(instellingen.ColorBezet))
            lblAfremBezet.BackColor = Color.FromArgb(CInt(instellingen.ColorAfremBezet))
            lblGereserveerd.BackColor = Color.FromArgb(CInt(instellingen.ColorGereserveerd))
            lblPrereserved.BackColor = Color.FromArgb(CInt(instellingen.ColorPreReserved))
            lblVergrendeld.BackColor = Color.FromArgb(CInt(instellingen.ColorVergrendeld))
            lblOnderhoud.BackColor = Color.FromArgb(CInt(instellingen.ColorOnderhoud))
            lblFout.BackColor = Color.FromArgb(CInt(instellingen.ColorSpook))
            lblBlockPen.BackColor = Color.FromArgb(CInt(instellingen.colorBlockPen))
            lblHalt.BackColor = Color.FromArgb(CInt(instellingen.colorHalt))
            lblPassief.BackColor = Color.FromArgb(CInt(instellingen.colorPassief))
            lblInitialisatie.BackColor = Color.FromArgb(CInt(instellingen.ColorInit))
            lblInitBlok.BackColor = Color.FromArgb(CInt(instellingen.ColorInitBlok))
            lblGridLijnen.BackColor = Color.FromArgb(CInt(instellingen.ColorGridLijnen))
            lblAchtergrondSynop.BackColor = Color.FromArgb(CInt(instellingen.ColorAchtergrondSynop))
            lblAchtergrondElement.BackColor = Color.FromArgb(CInt(instellingen.ColorAchtergrondElement))
            lblHandbediening.BackColor = Color.FromArgb(CInt(instellingen.ColorHandbediening))
            lblRetrigger.BackColor = Color.FromArgb(CInt(instellingen.ColorRetrigger))
            lblTextback.BackColor = Color.FromArgb(CInt(instellingen.ColorTextBack))
            lblLocatieTrein.BackColor = Color.FromArgb(CInt(instellingen.ColorLocatieTrein))
            txtStatusColorTurnout.Text = instellingen.StatusColorTurnout    'I, *
            txtUitvoeringsColommen.Text = instellingen.s88ColonRange
            If instellingen.gridlijnenTonen Then chkGridlijnenTonen.Checked = True Else chkGridlijnenTonen.Checked = False
            If instellingen.showRailElements Then chkShowRailElements.Checked = True Else chkShowRailElements.Checked = False
            If instellingen.K83K84detectie Then chkK83K84detectie.Checked = True Else chkK83K84detectie.Checked = False
            If instellingen.uitvoeringsmode Then chkUitvoeringsmode.Checked = True Else chkUitvoeringsmode.Checked = False
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: Instellingen.txt " & exc.Message _
            & vbNewLine & vbNewLine & "Zie alle bestanden na in: " & DataDir, "WallarDCS Opstart", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
            Exit Sub
        End Try
    End Sub

    Private Sub SaveInstellingen()
        Dim myStreamWriter As StreamWriter
        'ProgramSetting data from file ProgramSetting.txt
        'hier moeten nog input controle op komen     '  TODO
        Try
            myStreamWriter = File.CreateText(DataDir & "\Instellingen.txt")
            myStreamWriter.WriteLine(TxtHsi88Usb.Text)
            myStreamWriter.WriteLine(txtmodLeft.Text)
            myStreamWriter.WriteLine(txtmodMidden.Text)
            myStreamWriter.WriteLine(txtmodRechts.Text)
            myStreamWriter.WriteLine(txtTreinStartVertraging.Text)
            myStreamWriter.WriteLine(txtCargoVerlaging.Text)
            myStreamWriter.WriteLine(txtAfremsnelheid.Text)
            myStreamWriter.WriteLine(txtMacroStart.Text)
            myStreamWriter.WriteLine(txtMacroStop.Text)
            myStreamWriter.WriteLine(txtMacroFout.Text)
            myStreamWriter.WriteLine(txtHaltInStation.Text)
            myStreamWriter.WriteLine(txtBlokkenOH.Text)
            myStreamWriter.WriteLine(txtk8055Aanwezig.Text)
            myStreamWriter.WriteLine(txtBoard0.Text)
            myStreamWriter.WriteLine(txtBoard1.Text)
            myStreamWriter.WriteLine(txtBoard2.Text)
            myStreamWriter.WriteLine(txtBoard3.Text)
            myStreamWriter.WriteLine(txtSchaduwUitDienstTijd.Text)
            myStreamWriter.WriteLine(txtTurnoutPulsTime.Text)
            myStreamWriter.WriteLine(txtTempoRegeling.Text)
            myStreamWriter.WriteLine(txtPendikte.Text)
            myStreamWriter.WriteLine(instellingen.ColorVrij)
            myStreamWriter.WriteLine(instellingen.ColorBezet)
            myStreamWriter.WriteLine(instellingen.ColorAfremBezet)
            myStreamWriter.WriteLine(instellingen.ColorGereserveerd)
            myStreamWriter.WriteLine(instellingen.ColorPreReserved)
            myStreamWriter.WriteLine(instellingen.ColorVergrendeld)
            myStreamWriter.WriteLine(instellingen.ColorOnderhoud)
            myStreamWriter.WriteLine(instellingen.ColorSpook)
            myStreamWriter.WriteLine(instellingen.colorBlockPen)
            myStreamWriter.WriteLine(instellingen.colorHalt)
            myStreamWriter.WriteLine(instellingen.colorPassief)
            myStreamWriter.WriteLine(instellingen.ColorInit)
            myStreamWriter.WriteLine(instellingen.ColorInitBlok)
            myStreamWriter.WriteLine(instellingen.ColorGridLijnen)
            myStreamWriter.WriteLine(instellingen.ColorAchtergrondSynop)
            myStreamWriter.WriteLine(instellingen.ColorAchtergrondElement)
            myStreamWriter.WriteLine(instellingen.ColorHandbediening)
            myStreamWriter.WriteLine(instellingen.ColorRetrigger)
            myStreamWriter.WriteLine(instellingen.ColorTextBack)
            myStreamWriter.WriteLine(instellingen.ColorLocatieTrein)
            If txtStatusColorTurnout.Text.ToUpper = "I" OrElse txtStatusColorTurnout.Text.ToUpper = "V" Then
                myStreamWriter.WriteLine(txtStatusColorTurnout.Text.ToUpper)
            Else
                myStreamWriter.WriteLine("I")
            End If
            If txtUitvoeringsColommen.Text.IndexOfAny("1234".ToCharArray) = -1 Then txtUitvoeringsColommen.Text = "1"
            myStreamWriter.WriteLine(txtUitvoeringsColommen.Text)
            myStreamWriter.WriteLine(chkGridlijnenTonen.Checked.ToString)
            myStreamWriter.WriteLine(chkShowRailElements.Checked.ToString)
            myStreamWriter.WriteLine(chkK83K84detectie.Checked.ToString)
            myStreamWriter.WriteLine(chkUitvoeringsmode.Checked.ToString)
            myStreamWriter.Close()
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: Instellingen.txt " & exc.Message _
            & vbNewLine & vbNewLine & "Zie alle bestanden na in: " & DataDir, "WallarDCS Opstart", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
            Exit Sub
        End Try
    End Sub

    Private Function SetColorSynop(ByVal colorSynop As Color) As Color
        Dim MyDialog As New ColorDialog()
        MyDialog.AllowFullOpen = True
        MyDialog.ShowHelp = True
        ' Sets the initial color select to the current text color,
        MyDialog.Color = colorSynop

        If (MyDialog.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Return MyDialog.Color
        End If
        Return Color.LightGray
    End Function

#End Region

End Class