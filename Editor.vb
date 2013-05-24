'WallarDCS   2011
'Walter Larno
' VB2010 Express edition
'Dotnet framework 4.0

Imports System.IO

Public Class Editor

    'Member variabelen
    Private m_txtchanged As Boolean
    Private m_BestandsNaam As String
    Private m_BestandsPlaats As String
    Private m_HelptextID As String
    Private m_helpteksten As String
    Private helptekst As Help

    'forms
    Private EditorHelpFrm As EditorHelpfrm

#Region "Buttons of combobox"


    Private Sub cboxBestanden_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboxBestanden.SelectedIndexChanged
        LoadBestand()
    End Sub

    Private Sub btnVerlaat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVerlaat.Click
        Dim rt As Integer = 0
        If m_txtchanged Then
            rt = MessageBox.Show("Wijzigingen bewaren", "Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If rt = 6 Then saveBestand()
        End If
        Me.Close()
    End Sub

    Private Sub btnBewaren_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBewaren.Click
        saveBestand()
    End Sub

    Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        Dim indexEnd As Integer
        Try
            Dim index As Integer = m_helpteksten.IndexOf(m_HelptextID)
            If index < 0 Then Exit Sub
            indexEnd = m_helpteksten.IndexOf("$$", index + 1)
            If indexEnd < 0 Then Exit Sub
            EditorHelpFrm = New EditorHelpfrm
            EditorHelpFrm.Name = "Invulhelp voor " & m_HelptextID.Substring(2)
            EditorHelpFrm.txtHelpEditor.Text = m_helpteksten.Substring(index, indexEnd - index)
            EditorHelpFrm.Show()
        Catch ex As Exception

            MessageBox.Show("Fout = " & ex.Message, "Editeer HelpBestanden", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnTelLijnen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTelLijnen.Click
        lblInfo.Text = CStr(txtInput.Lines.GetLength(0))
    End Sub

#End Region

#Region "Subs en functions"

    Private Sub Editor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim myStreamReader As StreamReader
        Try
            myStreamReader = File.OpenText(DataDir + "\HelpBestanden.txt")
            m_helpteksten = myStreamReader.ReadToEnd
            myStreamReader.Close()
            m_txtchanged = False
            TextFormaat()
        Catch ex As Exception
            MessageBox.Show("Fout in Bestand: HelpBestanden.txt =" & ex.Message)
        End Try
    End Sub

    Private Sub saveBestand()

        If m_BestandsNaam = "" Then
            MessageBox.Show("Welk bestand - zet combobox juist", "Editor Bewaar", MessageBoxButtons.OK, MessageBoxIcon.Question)
            Exit Sub
        End If
        Dim MyStreamWriter As StreamWriter
        Try
            MyStreamWriter = File.CreateText(m_BestandsPlaats)
            MyStreamWriter.Write(txtInput.Text)
            MyStreamWriter.Close()
            m_txtchanged = False
            'update gegevens in memory (g_xxx arrays)
            Select Case m_BestandsNaam
                Case "Bevelen"
                    LoadBevelen()
                Case "LocoData"
                    LoadLocoData()
                Case "Relaties"
                    LoadRelaties()
                Case "s88MacroNrs"
                    LoadS88MacroNRs()
                Case Else
            End Select

        Catch ex As Exception
            MessageBox.Show("Fout in Bestand: " & m_BestandsPlaats & "  " & ex.Message)
        End Try
    End Sub

    Private Sub LoadBestand()
        Dim myStreamReader As StreamReader
        If m_txtchanged Then saveBestand()
        Try
            m_BestandsNaam = CStr(cboxBestanden.SelectedItem)
            m_BestandsPlaats = DataDir & "\" & m_BestandsNaam & ".txt"
            myStreamReader = File.OpenText(m_BestandsPlaats)
            txtInput.Text = myStreamReader.ReadToEnd
            myStreamReader.Close()
            m_txtchanged = False
            TextFormaat()
        Catch ex As Exception
            MessageBox.Show("Fout in Bestand: " & m_BestandsPlaats & "  " & ex.Message)
        End Try
    End Sub

    Private Sub txtInput_TextChanged1(ByVal sender As Object, ByVal e As System.EventArgs)
        m_txtchanged = True
    End Sub

    Private Sub TextFormaat()
        Select Case m_BestandsNaam
            Case "Bevelen"
                txtFormaat.Text = "abbbcccdefff....  [Vanaf lijn 2: a=bitwaarde 0;1, b=van, c=naar d=X,Y,Z  e=g:r, f=adresk8x  lijn 1= aantal lijnen]"
                m_HelptextID = "$$Bevelen"
            Case "BlokData"
                txtFormaat.Text = "aaa> bbcc> d> efff#efff> ggg> iii...|jjj  [a=Blok, b=min, c=max, d=Soort, e=a,b,w,d,t  f=k83k84adres, g= wisselk83adres  i=cobloknrs j=vanbloknr"
                m_HelptextID = "$$BlokData"
            Case "HelpBestanden"
                txtFormaat.Text = "$$Bestandsnaam, volgende lijnen tekst, eindig tekstblok met $$Bestandnaam, laatste lijn= $$"
                m_HelptextID = "$$Nihil"
            Case "LocoData"
                txtFormaat.Text = "aa> b> c> de> fff> ggg> hhh   [aa=LocoNR, b=0|1 0=uit 1=indienst, c=soort, d=Type, e=lengte, f=vanBlok, h =eindBlok, h=ByteWaarde(=112/80)"
                m_HelptextID = "$$LocoData"
            Case "K83K84Data"
                txtFormaat.Text = "aaa> b> [aaa=WisselNR, b=K/L K=korte L=lange wissel     '>'=TAB]"
                m_HelptextID = "$$K83K84Data"
            Case "Relaties"
                txtFormaat.Text = "bbb> ddd....|fff....#hhh....|jjj.... [vb:  026    017034|018#012013|009010]"
                m_HelptextID = "$$Relaties"
            Case "s88MacroNrs"
                txtFormaat.Text = "aaa> b> c> [lijn2: aaa=s88NR, b=MacroNR up 0/1, c=MacroNR down 1\0, lijn1= aantal lijnen"
                m_HelptextID = "$$s88MacroNrs"
            Case Else
                txtFormaat.Text = "Geen positie info aanwezig, zie help"
                m_HelptextID = "$$Nihil"
        End Select
    End Sub

#End Region

End Class