'Class DiCoStation is the center class between the Windows GUI, 
'the LDT Dicostation unit, LDT-Watchdog decoder and Modellplan Digital-S-Inside2 program
'Executes all the model railway communication
'Visual basic 2010 Express edition + Git
'(c) Wallar 2012   -  version 3.06.0.0 van 25/05/2013

Imports System
Imports System.Threading
Imports System.Collections
Imports System.Threading.Thread
Imports System.ComponentModel.ByteConverter
Imports System.Collections.CollectionBase
Imports System.Collections.Queue
Imports System.Collections.Comparer
Imports System.Text
Imports System.IO
Imports System.IO.Ports
Imports System.Timers
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.DateAndTime


Public Class DiCoStation

#Region "Main variables, events and properties declarations"

    'Public class variables
    Public inRunMode As Boolean   'if set to false to stop the ThreadEntry Loop and exit the thread

    'Private class variables
    Private WithEvents DSI2 As New Digital_S_Inside2        'make reference to digital-s-inside2 class and create it with events
    Private WithEvents HSI As HsiUsb                    'make reference to HsiUsb class and create it with events

    'Private variables
    Private s88ModulesChanged As Boolean                'In ThreadEntry loop, trigger for s88module changes
    Private s88Buffer(99) As Byte                        'gewijzigde ModNR, HB en LB
    Private s88ByteValues() As Byte
    Private _stopDoloop As Boolean              'onderbreken van initialisatie blokken
    Private _RunLoopTime As Integer     'Main en mainHSI loop cyclus tijd
    Private _s88NrTime() As Double              's88nr tijd bewaren tegen dender
    Private _s88NrTimeFre() As Double           's88nr tijd bewaren voor immuuntijd vrij
    Private execMacroNR As Integer
    Private macroInt(8) As Integer              'gereserveerde indexNRs: index=0 = blokNR, index= 1 = locoNR, index 2 = s88NR
    Private macroStr(8) As String
    Private macroBool(8) As Boolean

    'Watchdog-timer
    Private WithEvents WDTimer As New System.Timers.Timer                   'Littfinski Watchdog timer 5 seconds tick rithme
    Private _WDtimerEvent(0) As Boolean                                     'komt eens kort op true om een bevel door te laten 

    'Temporegelings-timer
    Private WithEvents TempoRegelingTimer As New System.Timers.Timer        'DiCoStation, temperen uitsturen bevelen naar digitale stroom
    Private _TempoRegelingPuls(0) As Boolean                                'temporegelingPuls komt op ritme van instellingen.tempoRegeling

    'class events for communication with MDIparentFrm
    Public Event handlerStatus(ByVal statusline As String)
    Public Event handlerStopwatch(ByVal elapsedTime As Long)
    Public Event handlerChangeBlok(ByVal status As String, ByVal BlokNR As Integer, ByVal locoNR As Integer)
    Public Event handlerOK(ByVal n As Integer)
    Public Event handlerChangeTurnout(ByVal status As String, ByVal direction As String, ByVal address As String)
    Public Event handlerBlokkenInDienst(ByVal blokkenInDienst As String)
    Public Event handlerRetrigger(ByVal Retrigger As String)
    Public Event handlerHalt(ByVal halt As String)
    Public Event handlerTreinen(ByVal treinenInDienst As String)
    Public Event handlerRWdeel(ByVal loconr As Integer, ByVal RWdeel As String)
    Public Event handlerLogData(ByVal data As String)
    Public Event handlerThreadRunning(ByVal dcsOK As Boolean)
    Public Event handlerBitArray(ByVal bitsArray As BitArray)

    'not yet in service
    'Public Event handlerNoodstop(ByVal onoff As Boolean)
    'Public Event handlerStrLocoNR(ByVal locoNR As String)
    'Public Event handlerBtnHerstartNaFoutRijden(ByVal ZetActief As Boolean)
    'Public Event ErrorReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialErrorReceivedEventArgs)


    'Command queues triggered in ThreadEntry loop
    Private queueLocoSTOP As New Queue()
    Private queueVrijkomenSectie As New Queue()
    Private queueLocoGO As New Queue()
    Private queueTurnout As New Queue()
    Private queueGeneralCmd As New Queue
    Private LocoUitDienstQueue As New Queue    'nabewerking van loco uit dienst
    Private StartVertragingQueue As New Queue  'vertraging bij sein op veilig
    Private LocoNabehandelingQueue As New Queue 'loco behandeling bij uitdienstname
    Private OpkuisTreinQueue As New Queue      'om een clear trein op te kuisen
    Private MacroQueue As New Queue            'Ogenblikkelijk uitvoeren van een macro

    'Velleman K8055D.dll v5 --------------------------------------------------------------------------------------------
    Private Declare Function OpenDevice Lib "k8055d.dll" (ByVal CardAddress As Integer) As Integer
    Private Declare Sub CloseDevice Lib "k8055d.dll" ()
    Private Declare Function Version Lib "k8055d.dll" () As Integer
    Private Declare Function SearchDevices Lib "k8055d.dll" () As Integer
    Private Declare Function SetCurrentDevice Lib "k8055d.dll" (ByVal CardAddress As Integer) As Integer
    Private Declare Function ReadAnalogChannel Lib "k8055d.dll" (ByVal Channel As Integer) As Integer
    Private Declare Sub ReadAllAnalog Lib "k8055d.dll" (ByRef Data1 As Integer, ByRef Data2 As Integer)
    Private Declare Sub OutputAnalogChannel Lib "k8055d.dll" (ByVal Channel As Integer, ByVal Data As Integer)
    Private Declare Sub OutputAllAnalog Lib "k8055d.dll" (ByVal Data1 As Integer, ByVal Data2 As Integer)
    Private Declare Sub ClearAnalogChannel Lib "k8055d.dll" (ByVal Channel As Integer)
    Private Declare Sub SetAllAnalog Lib "k8055d.dll" ()
    Private Declare Sub ClearAllAnalog Lib "k8055d.dll" ()
    Private Declare Sub SetAnalogChannel Lib "k8055d.dll" (ByVal Channel As Integer)
    Private Declare Sub WriteAllDigital Lib "k8055d.dll" (ByVal Data As Integer)
    Private Declare Sub ClearDigitalChannel Lib "k8055d.dll" (ByVal Channel As Integer)
    Private Declare Sub ClearAllDigital Lib "k8055d.dll" ()
    Private Declare Sub SetDigitalChannel Lib "k8055d.dll" (ByVal Channel As Integer)
    Private Declare Sub SetAllDigital Lib "k8055d.dll" ()
    Private Declare Function ReadDigitalChannel Lib "k8055d.dll" (ByVal Channel As Integer) As Boolean
    Private Declare Function ReadAllDigital Lib "k8055d.dll" () As Integer
    Private Declare Function ReadCounter Lib "k8055d.dll" (ByVal CounterNr As Integer) As Integer
    Private Declare Sub ResetCounter Lib "k8055d.dll" (ByVal CounterNr As Integer)
    Private Declare Sub SetCounterDebounceTime Lib "k8055d.dll" (ByVal CounterNr As Integer, ByVal DebounceTime As Integer)
    Private Declare Function ReadBackDigitalOut Lib "k8055d.dll" () As Integer
    Private Declare Sub ReadBackAnalogOut Lib "k8055d.dll" (ByRef Buffer As Integer)
    'end Velleman  ---------------------------------------------------------------------------------------------------------

#Region "Class Properties"

    'property members,  ter info: in GenModule [IBBevelenTempo, IBBaseTempo]    12345
    Private _s88Modules As Integer
    Private _runLoopStatus As Boolean
    Private _LocoStartVertraging As Integer
    Private _maxK83K84 As Integer

    Public WriteOnly Property RunLoopTime() As Integer
        Set(ByVal value As Integer)
            _RunLoopTime = value
        End Set
    End Property

    Public WriteOnly Property MaxK84K83() As Integer
        Set(ByVal value As Integer)
            _maxK83K84 = value
        End Set
    End Property

    Public WriteOnly Property LocoStartVertraging() As Integer
        Set(ByVal Value As Integer)
            _LocoStartVertraging = Value
        End Set
    End Property

    Public WriteOnly Property RunLoopStatus() As Boolean
        Set(ByVal value As Boolean)
            _runLoopStatus = value
        End Set
    End Property

    Public Property S88modules() As Integer
        Get
            Return _s88Modules
        End Get
        Set(ByVal Value As Integer)
            _s88Modules = Value
        End Set
    End Property
#End Region

#End Region

#Region "Timer events"

    Private Sub WDtimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles WDTimer.Elapsed
        SyncLock syncOK1
            _WDtimerEvent(0) = True             'default  on 4000 msec
        End SyncLock
    End Sub     'Watchdog event

    Private Sub TempoRegelingTimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles TempoRegelingTimer.Elapsed
        SyncLock syncOK1
            _TempoRegelingPuls(0) = True        'default on 100 msec
        End SyncLock
    End Sub     'DCS tempo event

#End Region

#Region "Iniatialisation subs and functions: Blokken, Loco, wissels, K8055"

    Private Function InitiateBlokken() As Boolean

        'RaiseEvent handlerStatus("Blokken initialisatie deel 1  [inlezen blokData - bezetting blokken]")
        'Public Sub InitialisatieBlokken()   'Initialisatie van de blokken
        Dim s88BitValues As BitArray
        Dim receiveBuffer(999) As Byte
        Dim MyStreamReader As StreamReader
        Dim MyStreamwriter As StreamWriter
        Dim ok As Boolean = False
        Dim ok1 As Boolean = False
        Dim ok2 As Boolean = False
        Dim i As Integer
        Dim s As String
        Dim locoNR As Integer
        Dim n, j, blokNR As Integer
        Dim b1, strBlokNR As String

        'initialisatie 
        m_Modules = CInt(instellingen.s88modulesLinks) + CInt(instellingen.s88modulesMidden) + CInt(instellingen.s88modulesRechts)
        m_maxBlok = m_Modules * 8
        m_MaxS88nrs = m_maxBlok * 2
        ReDim g_s88s(m_Modules * 2 - 1)
        ReDim m_blok(m_maxBlok)
        ReDim _s88NrTime(m_MaxS88nrs - 1)
        ReDim _s88NrTimeFre(m_MaxS88nrs - 1)
        Array.Clear(g_s88s, 0, g_s88s.Length)   'reset

        'laden van blokdata
        Try
            MyStreamReader = File.OpenText(DataDir & "\BlokData.txt")
            For i = 1 To m_maxBlok
                s = MyStreamReader.ReadLine
                m_blok(i).init(s)
                _s88NrTime(i - 1) = 0
                _s88NrTime(i) = 0
            Next
            MyStreamReader.Close()
            MyStreamReader = Nothing
        Catch ex As Exception
            Beep()
            RaiseEvent handlerStatus("Geen BlokData.txt kunnen inlezen, zie bestand na= " & ex.ToString)
        End Try

        'bezette blokken
        j = 0
        s88BitValues = New BitArray(s88ByteValues)
        For i = 0 To s88BitValues.Count - 3 Step 2   'last 2 s88inputs reserved for release HSI-USB event handler
            If s88BitValues.Item(i) Or s88BitValues.Item(i + 1) Then
                j += 1
                'RaiseEvent handlerStatus("Bloc number " & ((i / 2) + 1).ToString & " is occupied") '+++ 12345 ++
            End If
        Next
        If j = m_maxBlok - 1 Then
            MsgBox("Alle blokken zijn bezet? " + vbNewLine + "Zijn de transformatoren van de boosters ingeschakeld?" + vbNewLine _
                   + "Kijk na, het programma wordt nu afgesloten", MsgBoxStyle.OkOnly, "Stroomvoorziening boosters")
            End
        End If
        'RaiseEvent handlerStatus("Blokken initialisatie deel 2 [BlokNR/locoNR volgens BlokData]")
        Do
            m_blokkenInDienst = String.Empty   'reset
            Do
                For i = 0 To m_MaxS88nrs - 3 Step 2 ' de laatste 2 s88nrs zijn voorbehouden om de HSI88-usb loop te onderbreken 
                    blokNR = (i + 2) \ 2
                    strBlokNR = Format(blokNR, "000")
                    locoNR = CInt(g_BlokLocoArray(blokNR).Substring(4, 3))  'loconr in g_blokLocoArray()
                    If s88BitValues.Item(i) = True OrElse s88BitValues.Item(i + 1) = True Then 'blok is BEZET
                        If locoNR = LOKVRIJ Then   'Er werd geen LocoNR voor deze bezette bloknr gevonden
                            Do
                                s = ""  'reset
                                s = InputBox("Geef Loconummer in voor bloknummer " & strBlokNR _
                                & vbNewLine & "LocoNR=0 enkel voor volgblokken van lange trein!" _
                                & vbNewLine & vbNewLine & " [ 0 - 80 ]", "WallarCMD:  Ingave van ontbrekende Blok - loco gegevens", "")
                                If s = "" Then
                                    _stopDoloop = True
                                    Exit Function
                                End If
                                If Not (IsNumberOK(s) AndAlso CInt(s) >= 0 AndAlso CInt(s) < 81) Then
                                    s = ""
                                Else    's bevat een locoNR
                                    SyncLock syncOK1
                                        If CInt(s) <> 0 Then
                                            If g_locoData(CInt(s)).LocoInDienst = 0 Then
                                                n = MessageBox.Show("Ingave voor bloknummer " & blokNR.ToString & vbNewLine & vbNewLine _
                                                & "Het ingegeven loconummer  " & s _
                                                & "  is niet in gebruik volgens gegevens in 'LocoData.txt'" _
                                               & vbNewLine & vbNewLine & "Rety:     Corrigeer loconummer of" & vbNewLine _
                                               & "Cancel: STOP,     Controleer instellingen, pas aan en herstart", "WallarCMD Initialisatie Blokken-Loco", _
                                               MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation)
                                                Select Case n
                                                    Case 4  'retry
                                                        s = ""
                                                    Case Else   'cancel
                                                        ok1 = True
                                                        _stopDoloop = True
                                                        Exit Function
                                                End Select
                                            End If
                                        End If
                                    End SyncLock
                                End If
                            Loop Until s <> ""


                            locoNR = CInt(s)
                            If locoNR > 0 Then  ' loconr=0 is voor tweede,... blok bij lange trein
                                SetBlokGegevens(blokNR, locoNR, 2)
                                m_blok(blokNR).locoNR = locoNR  ' deze eigenschap hier juist zetten en niet in SetBlokGegevens
                            Else
                                m_blok(blokNR).status = True
                            End If

                            'controles op consistentie van de loco-blok gegevens
                            ok1 = True
                            SyncLock syncOK1
                                If m_blok(blokNR).locoNR > 0 AndAlso m_blok(blokNR).locoNR < 81 AndAlso g_locoData(m_blok(blokNR).locoNR).LocoInDienst = 0 Then
                                    n = MessageBox.Show("Ingave voor bloknummer " & blokNR.ToString & vbNewLine & vbNewLine _
                                    & "Het ingegeven loconummer  " & Format(m_blok(blokNR).locoNR, "###") _
                                    & "  is niet in gebruik volgens gegevens in 'LocoData.txt'" _
                                   & vbNewLine & vbNewLine & "Rety:     Corrigeer loconummer of" & vbNewLine _
                                   & "Cancel: STOP,     Controleer instellingen, pas aan en herstart", "WallarCMD Initialisatie Blokken-Loco", _
                                   MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation)
                                    Select Case n
                                        Case 4  'retry
                                            ok1 = False
                                            i -= 1
                                            g_BlokLocoArray(j) = Format(j, "000") & " 000"  'reset
                                            m_blokkenInDienst = m_blokkenInDienst.Substring(0, m_blokkenInDienst.Length - 8)
                                        Case Else   'cancel
                                            ok1 = True
                                            _runLoopStatus = False
                                            Exit Function
                                    End Select
                                End If
                            End SyncLock
                            If locoNR > 0 Then  ' loconr=0 is voor tweede,... blok bij lange trein
                                'locoNR is nu OK, pas alle gegevens in m_Blok() aan
                                SetBlokGegevens(blokNR, locoNR, 2)
                                m_blok(blokNR).locoNR = locoNR  ' deze eigenschap hier juist zetten en niet in SetBlokGegevens
                            End If
                        Else   ' blok is BEZET met bekende locoNR
                            SetBlokGegevens(blokNR, locoNR, 5)
                        End If
                    Else    'blok is VRIJ
                        SetBlokGegevens(blokNR, LOKVRIJ, 1)
                    End If
                    'wanneer alle s88nrs onderzocht werden mag deze for next verlaten worden
                    If i = m_MaxS88nrs - 4 Then
                        ok1 = True
                        Exit For
                    End If
                Next
            Loop Until ok1

            'eindcontrole blokkenindienst
            ok2 = True  'veronderstel alles is ok
            If m_blokkenInDienst = String.Empty Then  'er zijn geen blokken met loco's gemeld
                MessageBox.Show("Inlezen s88 kwam NIET in orde!" & _
                 vbNewLine & vbNewLine & "1." & vbTab & "ofwel, geen enkele trein gevonden? [minimum één loconr actief (standaard locoNR=1)  - zie na in LocoData] " _
                 & vbNewLine & "2." & vbTab & "ofwel, S88 decoders naar Intellibox of naar HSI -88 ? [ zit de kabel in het juiste toestel, zie s88 kabel na]" _
                 & vbNewLine & vbNewLine & "STOP de modelbaan, controleer instellingen (F3) en indien nodig eindig het programma en herbegin ", "Initialisatie blokken: FOUT bij Instellingen of S88 kabel aansluiting")
                _stopDoloop = True
                _runLoopStatus = False
                Exit Function
                ok2 = False
            Else    'er zijn blokken met loco's, kontrole op gelijke loconrs in de blokken
                'verwijder eerst de dubbele inschrijvingen
                i = 0
                Do
                    b1 = m_blokkenInDienst.Substring(i, 8)
                    If i < m_blokkenInDienst.Length - 8 Then
                        j = m_blokkenInDienst.IndexOf(b1, i + 8)
                        If j <> -1 Then m_blokkenInDienst = m_blokkenInDienst.Remove(j, 8)
                    End If
                    i += 8
                Loop Until i = m_blokkenInDienst.Length

                s = ""
                For i = 4 To m_blokkenInDienst.Length - 4 Step 8
                    s = "(" & m_blokkenInDienst.Substring(i, 3) 'locoNR
                    j = m_blokkenInDienst.IndexOf(s, i + 4) 'zoek zelfde locoNR
                    If j > 0 Then   'locoNR meermaals aanwezig
                        MessageBox.Show("Het bloknr " & m_blokkenInDienst.Substring(i - 4, 3) & " en het bloknr " & m_blokkenInDienst.Substring(j - 3, 3) & " hebben eenzelfde loconr " & s.Substring(1) & " , zie na!" _
                            & vbNewLine & vbNewLine _
                            & vbNewLine & "Druk 'OK' en verbeter de loconrs ", "Correctie op Blok - Loco gegevens", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                        g_BlokLocoArray(CInt(m_blokkenInDienst.Substring(i - 4, 3))) = m_blokkenInDienst.Substring(i - 4, 3) & " 000"   'reset
                        g_BlokLocoArray(CInt(m_blokkenInDienst.Substring(j - 3, 3))) = m_blokkenInDienst.Substring(j - 3, 3) & " 000"   'reset
                        ok2 = False
                        Exit For
                    End If
                Next
            End If
        Loop Until ok2

        'bewaren van BlokLoco.txt
        SaveBlokLocoArray()

        'controle blokkenindienst, bewaar de juiste toestand van blokLoco
        Try
            n = CInt(instellingen.s88modulesLinks) + CInt(instellingen.s88modulesMidden) + CInt(instellingen.s88modulesRechts)
            MyStreamwriter = File.CreateText(DataDir & "\BlokLoco.txt")
            For i = 1 To n * 8
                MyStreamwriter.WriteLine(g_BlokLocoArray(i))
            Next
            MyStreamwriter.Close()
        Catch ex As Exception
            Beep()
            MessageBox.Show("Fout in Bestand: BlokLoco(copy).txt " & ex.Message, "Initialisatie blokken", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        'initialisatie van vanBlokNR naar toegangsCoBlokNR van BlokData bestand
        For i = 1 To m_maxBlok
            If m_blok(i).type = Blok.BlokType.Schaduw Then
                m_blok(blokNR).vanBlokNR = CInt(m_blok(blokNR).toegangCoBlokNR)
            End If
        Next
        Return True
    End Function

    Private Function InitialisatieLoco() As Boolean
        Dim strLocoNR As String = String.Empty
        Dim str As String = String.Empty
        Try
            For i As Integer = 1 To m_maxBlok - 1 'voor alle blokken                12345
                If (g_BlokLocoArray(i).Substring(4, 3) <> "000") Then 'trein op het blok aanwezig
                    'creëer de trein
                    m_treinen.Add(New Trein(i, CInt(g_BlokLocoArray(i).Substring(4, 3))))
                    strLocoNR = g_BlokLocoArray(i).Substring(4, 3)
                    'init loco
                    dataL.address = strLocoNR
                    dataL.direction = "forwards"
                    dataL.speed = "0"
                    dataL.F0 = "0"
                    dataL.F1 = "0"
                    dataL.F2 = "0"
                    dataL.F3 = "0"
                    dataL.F4 = "0"
                    str = DSI2.SetLocoM(dataL)
                    RaiseEvent handlerStatus("blok in dienst= " + i.ToString + "   LocoNR= " + strLocoNR)
                    'Else
                    '    strLocoNR = "   Vrij"
                    '    RaiseEvent handlerStatus("blok in dienst= " + i.ToString + "   VRIJ")
                End If
            Next
            Return True
        Catch exc As Exception
            MessageBox.Show("Fout in sub Initialisatieloco: " & vbNewLine & exc.Message, "Initialisatie loco", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Sub InitK8055()
        Try
            Dim aantalDevices, cardAddress As Integer
            'velleman K8055d-v5
            aantalDevices = SearchDevices()
            If aantalDevices = 0 Then
                MsgBox("Er werden geen K8055D  (of vm111) gevonden")
                Exit Sub
            End If
            cardAddress = OpenDevice(0)
            RaiseEvent handlerLogData("Velleman K8055 in dienst, adres=" & cardAddress.ToString)
        Catch ex As Exception
            MessageBox.Show("Controleer K8055 instellingen. [zijn de dll's aanwezig in ...\system32 ?]", "WallarCMD - Velleman K8055 startup", MessageBoxButtons.OK, MessageBoxIcon.Error)
            instellingen.K8055 = "k8055NO"
        End Try
    End Sub

    Private Sub InitSeinen()
        'alle seinen op gesloten (rood) zetten
        If instellingen.K83K84detectie Then 'enkel als er seien of andere electromagnetisch materiaal in dienst zijn
            For blokNR As Integer = 1 To m_maxBlok
                Select Case m_blok(blokNR).K83K84.Substring(0, 1)
                    Case "b"
                        dataD.address = m_blok(blokNR).K83K84.Substring(1, 3)
                        If m_blok(blokNR).status Then dataD.port = "0" Else dataD.port = "1"
                        queueTurnout.Enqueue(dataD)

                    Case "w"
                        dataD.address = m_blok(blokNR).K83K84.Substring(1, 3)
                        dataD.port = "0"
                        queueTurnout.Enqueue(dataD)

                    Case "t"
                        dataD.address = m_blok(blokNR).K83K84.Substring(1, 3)
                        dataD.port = "0"
                        queueTurnout.Enqueue(dataD)
                    Case Else
                        'do nothing
                End Select
            Next

        End If
    End Sub

#End Region

#Region "THREADEntry, Preprocessing and Postprocessing, watchdogprocessing, statusonderzoek"

    Public Sub ThreadEntry()

        '1. initiatie vars
        Dim stopwatch As New Stopwatch()
        Dim str As String = String.Empty
        Dim DCSstatus As Integer = 0
        Dim rt As Integer = ((CInt(instellingen.s88modulesLinks) + CInt(instellingen.s88modulesMidden) + CInt(instellingen.s88modulesRechts)) * 2 - 1)
        ReDim s88ByteValues(rt)
        ReDim s88ByteValuesNEW(rt)
        ReDim s88ByteValuesOLD(rt)

        '2. Voorbereiding
        If Not PreProcessing() Then
            MessageBox.Show("Modeltreinbaan voorbereidingsactiviteiten zijn niet allemaal gelukt" + vbNewLine + "Programma wordt afgesloten")
            End
        End If

        '3. Voorbereiding tot ThreadLoop
        'put s88bytes into  s88bytesValuesNEW and in s88byteValuesOLD
        s88ByteValuesOLD = CType(s88ByteValues.Clone, Byte())
        s88ByteValuesNEW = CType(s88ByteValues.Clone, Byte())

        inRunMode = True   'all conditions ok, haertbeat while loop mag beginnen
        RaiseEvent handlerStatus("KLaar om treinen te laten rijden, DCS-ThreadName= " + Thread.CurrentThread.Name)
        RaiseEvent handlerThreadRunning(True)
        RaiseEvent handlerOK(0)
        s88BitValuesNEW = New BitArray(s88ByteValuesNEW)    'plaats startwaarden in bitArray         
        Sleep(500)


        While inRunMode        ' Start of Main Railway hartbeat loop  =========================================================================================

            '1. Avoid 100% CPU
            Sleep(1)    'loop vertrager (1= 1000 loops/sec)

            stopwatch.Start()

            '3.  DiCoStation power status control
            DCSstatus += 1
            If DCSstatus = 10000 Then      'vermindert het aantal powerON/OFF controles
                DCSstatus = 0               'reset teller
                str = DSI2.PowerStatus
                If str.Substring(str.IndexOf("<Power>") + 7, 1) = "0" Then
                    RaiseEvent handlerThreadRunning(False)
                    RaiseEvent handlerStatus("DiCoStation-ThreadLoop meldt: Power OFF")
                End If
            End If

            '4.  process new s88module values and put into commands
            If s88ModulesChanged Then
                s88ChangesAllowed = False       'onderbreek hsi_88-USB ReadLoop tot deze gegevens verwerkt zijn
                s88ModulesChanged = False       'reset
                s88dataProcessing()
                s88ChangesAllowed = True        'herneem hsi_88-USB ReadLoop om nieuwe gegevens op te halen
            End If

            '5.   Tempo geregelde bevelen uitsturen naar LDT-DCS apparaat (en modelbaan)
            SyncLock syncOK1
                If _TempoRegelingPuls(0) Then
                    _TempoRegelingPuls(0) = False       'reset
                    If queueLocoSTOP.Count > 0 Then     'eerst alle treinen stoppen
                        ExecLocoStopCMD()
                    ElseIf queueTurnout.Count > 0 Then  'wissels en ander electromateriaal
                        ExecTurnoutCMD()
                    ElseIf queueLocoGO.Count > 0 Then   'rijden treinen
                        ExecLocoGoCMD()
                    End If
                End If
            End SyncLock

            '6  Watchdog trigger every 4 seconds (default value)
            SyncLock syncOK1
                If _WDtimerEvent(0) Then
                    _WDtimerEvent(0) = False    'reset
                    SetWatchdogRelais()
                End If
            End SyncLock

            '7 Behandelingsqueues
            If queueVrijkomenSectie.Count > 0 Then
                ExecVrijkomenSectie()
            elseIF StartVertragingQueue.Count > 0 Then
                ExecStartVertragingCMD()
            ElseIf MacroQueue.Count > 0 Then
                ExecMacroQueueCMD()
            ElseIf LocoNabehandelingQueue.Count > 0 Then
                ExecLocoNabehandelingCMD()
            ElseIf LocoUitDienstQueue.Count > 0 Then
                QueueLocoUitDienstBehandeling()
            ElseIf OpkuisTreinQueue.Count > 0 Then
                QueueOpkuisTreinBehandeling()
            End If

            '8.  Railway traffic follow-up
            If m_retriggerBlokken.Length > 0 Then
                Retrigger()
            End If
            If m_haltBlokken.Length > 0 Then Halt()

            '9. Stopwatch end
            RaiseEvent handlerStopwatch(stopwatch.ElapsedMilliseconds)
            stopwatch.Reset()

        End While        '====================================== End of Main Railway hartbeat loop  =================================================================
        PostProcessing()
    End Sub

    Private Function PreProcessing() As Boolean

        Dim PresetLocoStop As Boolean = False
        RaiseEvent handlerStatus(vbCrLf & "De modelspoorbaan wordt voorbereid om de treinen te laten rijden...")
        Dim str As String = String.Empty
        Dim ok As Boolean = False
        Dim rt As MsgBoxResult = MsgBox("Druk op YES om gewoon te starten" _
                                + vbNewLine + "Druk op NO om loco's op stop te zetten" _
                                + vbNewLine + vbNewLine + "Is LDT-Watchdogmodule in dienst?", MsgBoxStyle.YesNo, "WallarDCS2")
        If rt = MsgBoxResult.No Then PresetLocoStop = True

        '1.  connect Digital-s-Inside2 via Localhost:51400
        Do
            str = DSI2.CreateSocket()
            If str.Substring(0, 2) = "OK" Then
                ok = True
            Else    'REDO
                ok = False
                RaiseEvent handlerStatus(str)
            End If
        Loop Until ok
        RaiseEvent handlerStatus("Programma Digital-S-Inside2 staat klaar: " + str)
        RaiseEvent handlerStatus(DSI2.DSIVersion)
        str = DSI2.PowerOn() 'put power on the rails
        RaiseEvent handlerStatus(str)

        If PresetLocoStop Then        'zet vooraf alle loco snelheden op 0
            For i As Integer = 0 To g_locoData.GetUpperBound(0) - 1       'voor alle loco's in dienst volgens locoData bestand
                If g_locoData(i).LocoInDienst = 1 Then
                    'queryLocoSpeed
                    dataL.address = CStr(i)
                    dataL.speed = "0"
                    dataL.F0 = "0"
                    str = DSI2.SetLocoM(dataL)
                    Sleep(32)
                End If
            Next
            str = DSI2.PowerOFF() 'remove power off the rails
            DSI2.CloseSocket()
            End     'sluit het programma af, zet eerst de treinen in start positie
        End If

        '2. set secondrelais from LDT-watchdog module (purpose: exit from HSI88USB)        
        dataD.port = "1"
        dataD.address = instellingen.LeaveHSI88USB  'default on address 254
        str = DSI2.SetDecoderM(dataD)
        RaiseEvent handlerStatus("Set LDT-Watchdog relais: " + str)

        '3.  initiate HsiUsb device
        Initiate_HsiUsb()
        RaiseEvent handlerStatus("De HSI88-Usb device is geïnitialiseerd en startgegevens opgehaald")

        '4. initate blokken
        If Not InitiateBlokken() Then
            MessageBox.Show("Fout in initialisatie blokken")
            Return False
            Exit Function
        End If

        '5. Initialisatie Loco's
        RaiseEvent handlerStatus("Locomotieven worden geïnitialiseerd")
        If Not InitialisatieLoco() Then
            MessageBox.Show("Fout in initialisatie Loco's")
            Return False
            Exit Function
        End If

        '6. Initialiseer seinen
        InitSeinen()

        '7. Initialiseer Velleman K8055 device
        If instellingen.K8055 = "k8055YES" Then
            InitK8055()
            If instellingen.K8055 = "k8055YES" Then
                SetAllDigital()

                RaiseEvent handlerStatus("Velleman K8055, version " + Version.ToString + " device geïnitialiseerd (indien aanwezig)")
            End If
        End If

        '8.  timer settings for LDT-WatchDog module first relais (default on address 253)  
        WDTimer.Interval = 4000     '4 seconds maximum delay time 
        WDTimer.Enabled = True
        WDTimer.Start()
        RaiseEvent handlerStatus("Littfinski Watchdog Decoder geactiveerd op 4 seconden")

        '9. timer settings  for command actions to LDT-DiCoStation  
        TempoRegelingTimer.Interval = CInt(instellingen.tempoRegeling)      'default= 100 milliseconden (min: 40 msec)
        TempoRegelingTimer.Enabled = True
        TempoRegelingTimer.Start()
        RaiseEvent handlerStatus("DiCoStation temporegeling geactiveerd op " + instellingen.tempoRegeling.ToString + " msec")

        '10. Start HsiUsb Listener
        HSI.StartReadLoop()

        '2. run Startmacro nr=1
        execMacroNR = CInt(instellingen.MacroStart)
        ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf MacroInterpreter))

        '11.  prepare for while loop
        Sleep(500)    'don't remove,don't change below 50: give enough process time for the HSI_s88Changed event
        Return True
    End Function

    Private Sub PostProcessing()

        '1.  Set ready to close HsiUsb listener  communication: prepare to stop te listener loop
        HSI.StopReadLoopBegin() '_readloopActive= false om ReadLoop te onderbreken

        '2. run afsluitmacro nr=2
        execMacroNR = CInt(instellingen.MacroStop)
        ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf MacroInterpreter))

        '3. bewaren van deze Blok-Loco toestand
        SaveBlokLocoArray()

        '4. clear and close velleman k8055
        If instellingen.K8055 = "k8055YES" Then
            ClearAllDigital()
            CloseDevice()
        End If

        '5. Laatste cmds naar s88module via LDT-WatchDog relais op adres=254
        dataD.port = "0"
        dataD.address = instellingen.LeaveHSI88USB
        DSI2.SetDecoderM(dataD)
        ' Debug.WriteLine("2. Laatste cms naar Watchdog relais 254 gestuurd")

        '6. close the Hsi-88-USB Listener
        HSI.CloseHsi88Usb()

        '7. release LDT-WatchtDog 
        dataD.address = "253"
        dataD.port = "0"
        DSI2.SetDecoderM(dataD)
        'Debug.WriteLine("5. LDT-Watchdog module reset ")

        '8. Dicostation powerOff
        DSI2.PowerOFF()

        '9. release  timers
        WDTimer.Stop()
        WDTimer.Dispose()
        TempoRegelingTimer.Stop()
        TempoRegelingTimer.Dispose()
    End Sub

    Private Sub SetWatchdogRelais()
        'herbewapen het watchdog relais om een continue boosterstroom te voorzien (binnen de 5 seconden)
        dataD.port = "1"
        dataD.address = "253"  'default on address 253
        DSI2.SetDecoderM(dataD)
    End Sub

#End Region

#Region "PROCESSING treinverkeersregeling"

    Private Sub s88dataProcessing()

        'enkel voor display  ===== 12345 ========================================================
        'INFO: van HSI88-Usb in s88Buffer() 
        's88buffer(0)= modulenummer changed 1 -31
        's88buffer(1)= Highbyte     0 - 255
        's88buffer(2)= Lowbyte      0 - 255
        'telkens worden de gegeven voor 1 module wijziging opgestuurd aanpassing vanaf 02/11/2011
        'RaiseEvent handlerStatus("-----------------------------------")
        'RaiseEvent handlerStatus("s88dataProcessing: Lowbyte=  " + s88Buffer(2).ToString)
        'RaiseEvent handlerStatus("s88dataProcessing: Highbyte= " + s88Buffer(1).ToString)
        'RaiseEvent handlerStatus("s88dataProcessing: ModNR=    " + s88Buffer(0).ToString)
        'RaiseEvent handlerStatus("...................................................")
        'Debug.WriteLine("s88dataProcessing: ModNR=    " + s88Buffer(0).ToString)
        'Debug.WriteLine("s88dataProcessing: Highbyte= " + s88Buffer(1).ToString)
        'Debug.WriteLine("s88dataProcessing: Lowbyte=  " + s88Buffer(2).ToString + vbNewLine)
        '================== 12345 ===============================================================

        Dim TimeNow As Double = Microsoft.VisualBasic.DateAndTime.Timer()
        Dim s88nr, indexStart, indexEnd As Integer
        'Dim blokNR As Integer
        Try
            '1. plaats nieuwe modulegegevens in NEW ByteArray
            s88ByteValuesNEW(s88Buffer(0) * 2 - 2) = s88Buffer(2)       'lowByte
            s88ByteValuesNEW(s88Buffer(0) * 2 - 1) = s88Buffer(1)       'highByte

            'bytes overbrengen naar de bitArray (NEW) en bytes bewaren (in OLD)
            s88BitValuesNEW = New BitArray(s88ByteValuesNEW)    'beide nodig
            s88BitValuesOLD = New BitArray(s88ByteValuesOLD)    'beide nodig 

            '2. uitvoering: omzetting naar bevelen, onderzoek enkel de gewijzigde module !!!
            If instellingen.uitvoeringsmode Then
                indexStart = s88Buffer(0) * 16 - 16         'start bij bitnummer voor de gemelde module
                If s88Buffer(0) = m_Modules Then indexEnd = indexStart + 13 Else indexEnd = indexStart + 15
                'eindig bij bitnummer voor de gemelde module
                For s88nr = indexStart To indexEnd
                    'welke s88nrBit is van waarde veranderd? 
                    If s88BitValuesOLD.Item(s88nr) <> s88BitValuesNEW.Item(s88nr) Then
                        If _s88NrTime(s88nr) < TimeNow Then 'antidendertijd verlopen?
                            _s88NrTime(s88nr) = TimeNow + 0.5 '.5 sec primaire antidender, een s88nr kan niet sneller wisselen
                            s88ByteValuesOLD = CType(s88ByteValuesNEW.Clone, Byte())    'bewaar de nieuwe situatie
                            BlokkenBehandeling(s88nr)
                        Else
                            _s88NrTime(s88nr) = TimeNow + 0.5 '.5 sec primaire antidender, een s88nr kan niet sneller wisselen
                        End If
                    End If
                Next
            End If

            '3. display s88contacts indien actief
            If s88contactsView Then RaiseEvent handlerBitArray(s88BitValuesNEW)
        Catch ex As Exception
            Beep()
            RaiseEvent handlerStatus("TRY: s88dataProcessing exception= " + ex.ToString)
        End Try
    End Sub  's88 waarden omzetten in bevelen

    Private Sub BlokkenBehandeling(ByVal s88nr As Integer)
        Dim melding As String = String.Empty
        Dim ArrayFT(1) As Double        '(0)=s88nr en (1)=freTime
        Dim blokNR As Integer = (s88nr + 2) \ 2
        If instellingen.OHblokken.IndexOf("|" & Format(blokNR, "000")) = -1 Then        'blok is niet in onderhoud
            'welke overgang? 
            If s88BitValuesNEW.Item(s88nr) Then                                         's88=true:   overgang 0 ---> 1  BEZET komen van een sectie
                BezettenSectie(blokNR, s88nr)
            Else                                                                        's88=false,  overgang 1 ---> 0  VRIJ komen van een sectie
                '1 -> 0 overgang is niet zo stabiel (dender), s88waarde een tijdje bewaren
                ArrayFT(0) = s88nr
                ArrayFT(1) = Microsoft.VisualBasic.DateAndTime.Timer() + 1              '1 seconde vrijkomen antidender
                queueVrijkomenSectie.Enqueue(ArrayFT)                                   '(0)=s88nr, (1)= freTime double
            End If
        End If
    End Sub

    Private Sub BezettenSectie(ByVal blokNR As Integer, ByVal s88nr As Integer)
        Dim i As Integer = 0
        If m_blok(blokNR).statusK = False Then  'GEWONE actie, geen keeractie   
            If m_blok(blokNR).bewegingSequentie = 0 And m_blok(blokNR).statusNaarRIJsectie And m_blok(blokNR).controleBlokNR = blokNR Then   'een trein wordt op dit blok VERWACHT           +++   RIJ-sectie acties +++
                m_blok(blokNR).bit = (s88nr Mod 2)  '0=paar, 1=onpaar  =  bepaalt de rijrichting   (kan enkel hier)
                m_blok(blokNR).s88RijNR = s88nr 'kan enkel hier
                If m_blok(blokNR).bit = 0 Then m_blok(blokNR).s88AfremNR = s88nr + 1 Else m_blok(blokNR).s88AfremNR = s88nr - 1
                SetBlokGegevens(blokNR, m_blok(blokNR).locoNR, 2)                                       'trein op Rijsectie
                OpRijSectie(blokNR, m_blok(blokNR).locoNR)                                              'Rijsectie acties uitvoeren
                If g_s88MacroNRS(s88nr, 0) > 0 Then MacroSelection(g_s88MacroNRS(s88nr, 0)) 'uitvoering macro's
                If instellingen.K83K84detectie Then K83K84TreinBezetBlok(blokNR, m_blok(blokNR).locoNR) 'voor wissels in blok

            ElseIf m_blok(blokNR).bewegingSequentie = 1 And s88nr = m_blok(blokNR).s88AfremNR Then
                'tweede 0/1 contact met dit blok, Bezetten AFREMsectie                              +++  AFREM-sectie acties+++ 
                SetBlokGegevens(blokNR, m_blok(blokNR).locoNR, 3)
                OpAfremSectie(blokNR, m_blok(blokNR).locoNR)
                If g_locoData(m_blok(blokNR).locoNR).lengteCode <> Trein.TreinLengte.Lang AndAlso m_blok(blokNR).wissels <> String.Empty Then VrijZettenWissels(blokNR, m_blok(blokNR).locoNR)
                If g_s88MacroNRS(s88nr, 0) > 0 Then MacroSelection(g_s88MacroNRS(s88nr, 0))
                If instellingen.K83K84detectie Then K83K84TreinBezetBlok(blokNR, m_blok(blokNR).locoNR)

            Else
                ' do nothing 
                'RaiseEvent handlerStatus("Sub BezettenSectie: s88nr= " + s88nr.ToString _
                '                         + " fout, GEEN bloksectie kunnen toewijzen voor locoNR= " _
                '                         + m_blok(blokNR).locoNR.ToString + " op blokNR= " + blokNR.ToString _
                '                         + ", bewegingSequentie was= " + m_blok(blokNR).bewegingSequentie.ToString)
            End If
        Else    'KEER actie, dan geen acties uitvoeren
            If m_blok(blokNR).locoNR = LOKVRIJ Then 'controle op locoNR
                Beep()
                RaiseEvent handlerStatus("BezettenSectie bij KEERactie: fout, locoNR = 0 van blokNR= " + blokNR.ToString)
            End If
        End If
    End Sub

    Private Sub VrijkomenSectie(ByVal blokNR As Integer, ByVal s88nr As Integer)
        Dim locoNR As Integer = m_blok(blokNR).locoNR
        Dim ok As Boolean = False
        If m_blok(blokNR).statusK = True Then           'KEREN 
            If m_blok(blokNR).statusK2 = False Then     'trein verliet RIJsectie na keren
                m_blok(blokNR).statusK2 = True
                m_blok(blokNR).statusNaarRIJsectie = False
            Else 'trein verliet blok na keren
                SetBlokGegevens(blokNR, locoNR, 1) ' blok trein vrij
            End If
        ElseIf m_blok(blokNR).s88RijNR > 0 Then     'GEWOON doorrijden (geen keren) en geen nieuwe indienstname
            'Een sectie komt vrij, welke?
            If s88nr = m_blok(blokNR).s88RijNR Then 's88nr van de RIJsectie
                SetBlokGegevens(blokNR, locoNR, 4)  'Rijsectie trein vrij
                If g_locoData(locoNR).lengteCode = Trein.TreinLengte.Kort AndAlso m_blok(blokNR).wissels <> String.Empty Then VrijZettenWissels(blokNR, locoNR) 'enkel korte trein (< 50cm)
                If g_s88MacroNRS(s88nr, 1) > 0 Then MacroSelection(g_s88MacroNRS(s88nr, 1))

            ElseIf s88nr = m_blok(blokNR).s88AfremNR And s88BitValuesNEW.Item(m_blok(blokNR).s88RijNR) = False Then 's88nr van de Afremsectie Then 
                If m_blok(blokNR).wissels <> String.Empty Then VrijZettenWissels(blokNR, locoNR) 'alle treinlengtes
                If g_s88MacroNRS(s88nr, 1) > 0 Then MacroSelection(g_s88MacroNRS(s88nr, 1))
                If instellingen.K83K84detectie Then K83K84TreinVerlietBlok(blokNR, locoNR)
                SetBlokGegevens(blokNR, locoNR, 1)  'blok trein Vrij
            Else
                RaiseEvent handlerStatus("Sub VrijkomeSectie: s88nr= " + s88nr.ToString _
                                         + " met locoNR= " + m_blok(blokNR).locoNR.ToString + " op blokNR= " + blokNR.ToString _
                                         + ", en bewegingSequentie= " + m_blok(blokNR).bewegingSequentie.ToString + " klopt niet!")

            End If
        Else    'enkel bij indienstgenomen trein, zet blok vrij als beide s88nr vrij zijn
            If s88nr Mod 2 = 0 Then
                If s88BitValuesNEW.Item(s88nr + 1) = False Then ok = True
            Else
                If s88BitValuesNEW.Item(s88nr - 1) = False Then ok = True
            End If
            If ok Then
                If m_blok(blokNR).wissels <> String.Empty Then VrijZettenWissels(blokNR, locoNR) 'alle treinlengtes
                If g_s88MacroNRS(s88nr, 1) > 0 Then MacroSelection(g_s88MacroNRS(s88nr, 1))
                If instellingen.K83K84detectie Then K83K84TreinVerlietBlok(blokNR, locoNR)
                SetBlokGegevens(blokNR, locoNR, 1)  'blok trein Vrij
            End If
        End If
    End Sub

    Public Sub SetBlokGegevens(ByVal blokNR As Integer, ByVal locoNR As Integer, ByVal statusBlok As Integer)
        Dim statusSynop As String = String.Empty
        SyncLock syncOK
            Select Case statusBlok
                Case 1   'blok is VRIJ, Afremsectie vrij, , geen trein meer op blok
                    m_blok(blokNR).statusSynop = "V"
                    m_blok(blokNR).status = False
                    m_blok(blokNR).statusK = False
                    m_blok(blokNR).statusK2 = False
                    m_blok(blokNR).eindeRW = False
                    m_blok(blokNR).locoStop = False
                    m_blok(blokNR).statusPreReserved = False
                    m_blok(blokNR).statusNaarRIJsectie = False
                    m_blok(blokNR).statusOpRIJsectie = False
                    m_blok(blokNR).statusOpAFREMsectie = False
                    m_blok(blokNR).bewegingSequentie = 0            'trein niet op blok
                    m_blok(blokNR).locoNR = LOKVRIJ
                    m_blok(blokNR).locoFout = 0
                    m_blok(blokNR).wisselsein = ""
                    g_BlokLocoArray(blokNR) = Format(blokNR, "000") & " 000"
                    Dim i As Integer = m_blokkenInDienst.IndexOf(Format(blokNR, "000") & "(")
                    If i <> -1 Then
                        m_blokkenInDienst = m_blokkenInDienst.Substring(0, i) & m_blokkenInDienst.Substring(i + 8)
                    End If
                    statusSynop = "V"

                Case 2  'trein op RIJsectie, BEZETTEN van blok
                    m_blok(blokNR).statusNaarRIJsectie = False      'volgende 0--1 overgang van dit blok is afrem
                    m_blok(blokNR).status = True                    'trein effectief op blok
                    m_blok(blokNR).statusOpRIJsectie = True
                    m_blok(blokNR).controleBlokNR = 0               'reset van verwachte blok
                    m_blok(blokNR).bewegingSequentie = 1            'trein nu rijdend op RIJsectie
                    m_blok(blokNR).locoFout = 0    'reset
                    m_blok(blokNR).statusSynop = "B"
                    m_blokkenInDienst &= Format(blokNR, "000") & "(" & Format(locoNR, "000") & ")"
                    g_BlokLocoArray(blokNR) = Format(blokNR, "000") & " " & Format(locoNR, "000")
                    statusSynop = "B"

                Case 3  'trein op AFREMsectie, BEZET
                    m_blok(blokNR).statusOpAFREMsectie = True
                    m_blok(blokNR).bewegingSequentie = 2            'trein nu rijdend of tot stilstand gekomen op AFREMsectie
                    If m_blok(blokNR).statusSynop = "B" Then        'gewoon doorrijden op afremsectie
                        m_blok(blokNR).statusSynop = "A"
                        statusSynop = "A"
                    Else
                        statusSynop = m_blok(blokNR).statusSynop    'blok was in retrigger, halt
                    End If

                Case 4  'trein verliet Rijsectie
                    m_blok(blokNR).wisselsein = ""
                    If m_blok(blokNR).statusSynop = "B" Then        'gewoon doorrijden na rijsectie
                        statusSynop = "B"
                    ElseIf m_blok(blokNR).statusSynop = "C" Then
                        statusSynop = "C"
                    Else
                        statusSynop = m_blok(blokNR).statusSynop    'blok was in retrigger, halt
                    End If

                Case 5  'initfaze: Bezetten blok bij programma start 
                    m_blok(blokNR).status = True
                    m_blok(blokNR).statusK = False
                    m_blok(blokNR).statusK2 = False
                    m_blok(blokNR).statusNaarRIJsectie = False
                    m_blok(blokNR).bewegingSequentie = 2    'init: trein aanwezig op blok
                    If g_locoData(locoNR).lengteCode = Trein.TreinLengte.Kort Then
                        m_blok(blokNR).statusOpRIJsectie = False
                    Else
                        m_blok(blokNR).statusOpRIJsectie = True
                    End If
                    m_blok(blokNR).statusOpAFREMsectie = True
                    m_blok(blokNR).eindeRW = False
                    m_blok(blokNR).locoStop = False
                    m_blok(blokNR).statusPreReserved = False
                    m_blok(blokNR).statusSynop = "C"
                    m_blok(blokNR).SynopLocoNR = locoNR
                    m_blok(blokNR).locoNR = locoNR
                    m_blok(blokNR).locoFout = 0
                    m_blok(blokNR).controleBlokNR = 0
                    m_blok(blokNR).s88RijNR = 0
                    m_blok(blokNR).s88AfremNR = 0
                    m_blokkenInDienst &= Format(blokNR, "000") & "(" & Format(locoNR, "000") & ")"
                    g_BlokLocoArray(blokNR) = Format(blokNR, "000") & " " & Format(locoNR, "000")
                    statusSynop = "C"
            End Select
            If instellingen.OHblokken.IndexOf("|" + Format(blokNR, "000")) <> -1 Then statusSynop = "U"
            RaiseEvent handlerChangeBlok(statusSynop, blokNR, m_blok(blokNR).locoNR)
        End SyncLock
    End Sub

    Private Sub OpRijSectie(ByVal blokNR As Integer, ByVal locoNR As Integer)
        Dim rwDeel As String = String.Empty
        Try
            RaiseEvent handlerChangeBlok("B", blokNR, locoNR)
            'opmaak rwDeel =================================================
            If m_treinen.Item(locoNR).VrijeTrein Then   'bepaling van het rwDeel
                If m_blok(blokNR).statusPreReserved Then   'preReservatie actief  
                    If m_treinen.Item(locoNR).Reisweg = String.Empty Then
                        VrijeTreinVerwerking(blokNR, locoNR)
                        m_blok(blokNR).statusPreReserved = False
                        rwDeel = m_treinen.Item(locoNR).rwDeel
                    ElseIf m_treinen.Item(locoNR).Reisweg.Length > 7 Then
                        'volgblok
                        m_treinen.Item(locoNR).rwDeel = m_treinen.Item(locoNR).Reisweg.Substring(0, 4)
                        rwDeel = m_treinen.Item(locoNR).rwDeel
                        m_treinen.Item(locoNR).Reisweg = m_treinen.Item(locoNR).Reisweg.Substring(4)
                    Else
                        'bestemmingsblok
                        m_treinen.Item(locoNR).rwDeel = m_treinen.Item(locoNR).Reisweg
                        rwDeel = m_treinen.Item(locoNR).rwDeel
                        m_treinen.Item(locoNR).Reisweg = String.Empty
                    End If
                Else
                    VrijeTreinVerwerking(blokNR, locoNR)
                    rwDeel = m_treinen.Item(locoNR).rwDeel
                End If
            Else    'geplande trein
                If Not Doorschuiven(locoNR) Then
                    RaiseEvent handlerStatus("OpRIJsectie: doorschuiven ging niet")
                End If
                rwDeel = m_treinen.Item(locoNR).rwDeel
            End If

            If rwDeel.Substring(0, 1).ToUpper = "E" Then        'einde actuele reisweg
                EindeRWverwerking(blokNR, locoNR, rwDeel)
            Else
                'VrijeTrein: moet er halttijd toegevoegd worden?
                Select Case m_treinen.Item(locoNR).Type
                    Case Trein.TreinType.Personen
                        If (m_blok(blokNR).type = Blok.BlokType.Perron _
                        OrElse m_blok(blokNR).type = Blok.BlokType.ElecPerron) _
                        AndAlso m_treinen.Item(locoNR).HaltPassagiersTrein <> "h000" Then
                            'standaard h000 wordt vervangen door werkelijke waarde
                            rwDeel = m_treinen.Item(locoNR).HaltPassagiersTrein & rwDeel
                            m_treinen.Item(locoNR).rwDeel = rwDeel
                        End If
                    Case Trein.TreinType.HST
                        If m_blok(blokNR).type = Blok.BlokType.ElecPerron _
                        AndAlso m_treinen.Item(locoNR).HaltPassagiersTrein <> "h000" Then
                            'standaard h000 wordt vervangen door werkelijke waarde
                            rwDeel = m_treinen.Item(locoNR).HaltPassagiersTrein & rwDeel
                            m_treinen.Item(locoNR).rwDeel = rwDeel
                        End If
                    Case Trein.TreinType.Pendel
                        If m_blok(blokNR).type = Blok.BlokType.Pendel _
                        AndAlso m_treinen.Item(locoNR).HaltPassagiersTrein <> "h000" Then
                            'standaard h000 wordt vervangen door werkelijke waarde
                            rwDeel = m_treinen.Item(locoNR).HaltPassagiersTrein & rwDeel
                            m_treinen.Item(locoNR).rwDeel = rwDeel
                        End If
                    Case Else   'Trein.TreinType.Standaard, Trein.TreinType.Goederen,Trein.TreinType.Rangeer   
                        'geen halt toevoegen
                End Select

                'maak cmd selectie
                Select Case rwDeel.Substring(0, 1).ToLower
                    Case "<"    'speciaal teken van vrije trein wachten op vrije reisweg
                        RijsectieVertraging(locoNR, blokNR, rwDeel)
                        m_retriggerBlokken.Append(Format(blokNR, "000"))   'kijken of volgReisweg vrij komt                       
                        m_blok(blokNR).statusSynop = "R"
                        RaiseEvent handlerChangeBlok("R", blokNR, locoNR)
                        RaiseEvent handlerRetrigger(m_retriggerBlokken.ToString)
                    Case "j", "h"
                        OpmaakBevelen(rwDeel, locoNR, blokNR)
                    Case "c"        'conditie reisweg
                        If ConditieVerwerking(blokNR, locoNR, rwDeel) = False Then 'geen vrije reisweg voorhanden
                            RijsectieVertraging(locoNR, blokNR, rwDeel)
                            m_retriggerBlokken.Append(Format(blokNR, "000"))   'kijken of volgReisweg vrij komt
                            m_blok(blokNR).statusSynop = "R"
                            RaiseEvent handlerChangeBlok("R", blokNR, locoNR)
                            RaiseEvent handlerRetrigger(m_retriggerBlokken.ToString)
                        End If
                    Case "m"
                        If MultiBlokVerwerking(blokNR, locoNR, rwDeel, m_treinen.Item(locoNR).Reisweg) = False Then 'niet alle multiblokken zijn vrij 
                            'Retrigger en snelheid 0
                            RijsectieVertraging(locoNR, blokNR, rwDeel)
                            m_retriggerBlokken.Append(Format(blokNR, "000"))   'kijken of volgReisweg vrij komt
                            m_blok(blokNR).statusSynop = "R"
                            RaiseEvent handlerChangeBlok("R", blokNR, locoNR)
                            RaiseEvent handlerRetrigger(m_retriggerBlokken.ToString)
                        End If
                    Case "t"
                        If TerminusVerwerking(blokNR, locoNR, rwDeel) = False Then 'geen vrije reisweg voorhanden
                            RijsectieVertraging(locoNR, blokNR, rwDeel)
                            m_retriggerBlokken.Append(Format(blokNR, "000"))   'kijken of volgReisweg vrij komt
                            m_blok(blokNR).statusSynop = "R"
                            RaiseEvent handlerChangeBlok("R", blokNR, locoNR)
                            RaiseEvent handlerRetrigger(m_retriggerBlokken.ToString)
                        End If
                    Case Else   'y,§,r,g,x,s,b,n,q: enkel deze cmd waar de ReiswegVrij moet bekeken worden
                        'onderzoek reisweg tot en met de volgende blok vrij
                        If m_treinen.Item(locoNR).MultiBlokFase Then 'Multifase(procedure)
                            If m_treinen.Item(locoNR).MultiBlokNR = blokNR Then
                                m_treinen.Item(locoNR).MultiBlokFase = False
                                m_treinen.Item(locoNR).MultiBlokNR = 0
                            End If
                            'Doorschuiven(locoNR)
                            OpmaakBevelen(rwDeel, locoNR, blokNR)
                        Else    'gewoon geval ========================================================================== 
                            'Debug.WriteLine("OPRijsectie bloknr= " & blokNR.ToString & " rwDeel= " & rwDeel)
                            If ReiswegVRIJ(rwDeel, locoNR) Then 'reisweg is vrij <=========================
                                OpmaakBevelen(rwDeel, locoNR, blokNR)
                            Else    'Reisweg is bezet
                                RijsectieVertraging(locoNR, blokNR, rwDeel)
                                m_retriggerBlokken.Append(Format(blokNR, "000"))
                                m_blok(blokNR).statusSynop = "R"
                                RaiseEvent handlerChangeBlok("R", blokNR, locoNR)
                                RaiseEvent handlerRetrigger(m_retriggerBlokken.ToString)
                            End If
                        End If
                End Select
            End If
        Catch ex As Exception
            RaiseEvent handlerStatus("Try-fout in sub OpRijSectie : " & ex.Message)
        End Try
    End Sub

    Private Sub OpAfremSectie(ByVal blokNR As Integer, ByVal locoNR As Integer)
        Try
            Dim rwdeel As String = m_treinen(locoNR).rwDeel
            '1. Treinbeweging aanpassen 
            If m_blok(blokNR).locoStop Then 'true= loco moet STOPPEN
                m_blok(blokNR).locoStop = False      'reset
                m_treinen.Item(locoNR).Snelheid = 0  'stoppen
                m_ArrayLOCO(locoNR).address = locoNR.ToString
                m_ArrayLOCO(locoNR).speed = "0"
                queueLocoSTOP.Enqueue(locoNR)
                If m_treinen.Item(locoNR).rwDeel.Substring(0, 1).ToLower = "e" Then    'was tevens einde van de reisweg
                    TreinUitDienstname(locoNR, m_treinen.Item(locoNR).rwDeel)
                End If
            Else    'false= doorrijden, op aangepaste snelheid
                Dim snelheid As Integer = m_blok(blokNR).afremSnelheid
                If LocoSpeedChange(locoNR, snelheid) Then
                    m_ArrayLOCO(locoNR).address = locoNR.ToString
                    m_ArrayLOCO(locoNR).speed = snelheid.ToString
                    queueLocoGO.Enqueue(locoNR)
                End If
                ' kleur van te berijden wisselstraat van gereserveerd naar Afrembezet wijzigen
                Dim naarBlokNR As Integer = m_blok(blokNR).naarBlokNR   'staan in de naarblok
                If Not IsNothing(m_blok(naarBlokNR).wissels) AndAlso m_blok(naarBlokNR).wissels.Length > 0 Then
                    For i As Integer = 0 To m_blok(naarBlokNR).wissels.Length - 3 Step 4
                        RaiseEvent handlerChangeTurnout("A", m_blok(naarBlokNR).wissels.Substring(i, 1), CInt(m_blok(naarBlokNR).wissels.Substring(i + 1, 3)).ToString)
                    Next
                End If
            End If
        Catch ex As Exception
            RaiseEvent handlerStatus(" Try-fout in sub OpAfremSectie: " & ex.Message)
        End Try
    End Sub

    Private Sub VrijZettenWissels(ByVal blokNR As Integer, ByVal locoNR As Integer)
        Dim i, j As Integer
        'vrijzetten overgereden wissels (bezetsectie moet een volle trein kunnen bevatten)
        If m_blok(blokNR).statusK = True Then 'nog actie tgv keren richting loco 
            m_blok(blokNR).statusK = False            'terug afzetten
        ElseIf m_blok(blokNR).wissels.Length > 0 Then       'wissels vrijzetten behalve de wisselInBLOK
            For i = 0 To m_blok(blokNR).wissels.Length - 3 Step 4
                j = CInt(m_blok(blokNR).wissels.Substring(i + 1, 3))
                m_ArrayK83K84(j).status = False
                RaiseEvent handlerChangeTurnout(instellingen.StatusColorTurnout, m_blok(blokNR).wissels.Substring(i, 1), j.ToString)
            Next
            m_blok(blokNR).wissels = ""
        End If
    End Sub

    Private Sub K83K84TreinBezetBlok(ByVal blokNR As Integer, ByVal locoNR As Integer)
        'Wisselsein behandeling
        If m_blok(blokNR).wisselsein.Length = 3 Then
            dataD.address = m_blok(blokNR).wisselsein
            dataD.port = "0"
            queueTurnout.Enqueue(dataD)
        End If
        'ander K83K84 behandeling
        If m_blok(blokNR).bit = 0 Then    'bit=0 van paar s88nr naar onpaar
            Select Case m_blok(blokNR).K83K84.Substring(0, 1)
                Case "b"
                    dataD.address = m_blok(blokNR).K83K84.Substring(1, 3)
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)


                Case "t"    'accessoire
                    dataD.address = Format(CInt(m_blok(blokNR).K83K84.Substring(1, 3)) + 2, "000")
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)
                Case Else
                    ' no action
            End Select
        Else        'richting onpaar naar paar s88nr  (waarde ná #-teken)
            Select Case m_blok(blokNR).K83K84.Substring(5, 1)
                Case "b"
                    dataD.address = m_blok(blokNR).K83K84.Substring(6, 3)
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)

                Case "t"    'accessoire
                    dataD.address = Format(CInt(m_blok(blokNR).K83K84.Substring(1, 3)) + 2, "000")
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)
                Case Else
                    ' no action
            End Select
        End If
    End Sub

    Private Sub K83K84TreinVerlietBlok(ByVal blokNR As Integer, ByVal loccoNR As Integer)
        'K83K84 seinenbehandeling bij verlaten van het blok
        If m_blok(blokNR).bit = 0 Then  'richting paar naar onpaar s88nr (waarde vóór #-teken)
            Select Case m_blok(blokNR).K83K84.Substring(0, 1)
                Case "b"    'bloksein
                    dataD.address = m_blok(blokNR).K83K84.Substring(1, 3)
                    dataD.port = "1"
                    queueTurnout.Enqueue(dataD)
                Case "w"    'wisselsein
                    dataD.address = m_blok(blokNR).K83K84.Substring(1, 3)
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)
                Case "d"    'blok- en wisselsein
                    dataD.address = m_blok(blokNR).K83K84.Substring(1, 3)
                    dataD.port = "1"
                    queueTurnout.Enqueue(dataD)
                    dataD.address = Format(CInt(m_blok(blokNR).K83K84.Substring(1, 3)) + 1, "000")
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)
                Case "t"    'blok-,wisselsein en accessoire
                    dataD.address = m_blok(blokNR).K83K84.Substring(1, 3)
                    dataD.port = "1"
                    queueTurnout.Enqueue(dataD)
                    dataD.address = Format(CInt(m_blok(blokNR).K83K84.Substring(1, 3)) + 1, "000")
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)
                    dataD.address = Format(CInt(m_blok(blokNR).K83K84.Substring(1, 3)) + 2, "000")
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)
                Case "a"    'bloksein
                    dataD.address = m_blok(blokNR).K83K84.Substring(1, 3)
                    dataD.port = "1"
                    queueTurnout.Enqueue(dataD)
                Case Else
                    ' no action
            End Select
        Else        'richting onpaar naar paar s88nr  (waarde ná #-teken)
            Select Case m_blok(blokNR).K83K84.Substring(5, 1)
                Case "b"    'bloksein
                    dataD.address = m_blok(blokNR).K83K84.Substring(6, 3)
                    dataD.port = "1"
                    queueTurnout.Enqueue(dataD)
                Case "w"    'wisselsein
                    dataD.address = m_blok(blokNR).K83K84.Substring(6, 3)
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)
                Case "d"    'blok- en wisselsein
                    dataD.address = m_blok(blokNR).K83K84.Substring(6, 3)
                    dataD.port = "1"
                    queueTurnout.Enqueue(dataD)
                    dataD.address = Format(CInt(m_blok(blokNR).K83K84.Substring(6, 3)) + 1, "000")
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)
                Case "t"    'blok-,wisselsein en accessoire
                    dataD.address = m_blok(blokNR).K83K84.Substring(6, 3)
                    dataD.port = "1"
                    queueTurnout.Enqueue(dataD)
                    dataD.address = Format(CInt(m_blok(blokNR).K83K84.Substring(6, 3)) + 1, "000")
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)
                    dataD.address = Format(CInt(m_blok(blokNR).K83K84.Substring(6, 3)) + 2, "000")
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)
                Case "a"    'bloksein
                    dataD.address = m_blok(blokNR).K83K84.Substring(6, 3)
                    dataD.port = "0"
                    queueTurnout.Enqueue(dataD)
                Case Else
                    ' no action
            End Select
        End If
    End Sub

    Private Sub SpookRijdenBehandeling(ByVal s88NR As Integer)
        Dim OudeVanBlokNR, nieuweNaarBlokNR As Integer
        Dim blokNR As Integer = ((s88NR + 2) \ 2)
        If m_blok(blokNR).locoNR = 0 Then        'onderzoek
            'zie na of het een laatste dender is van de laatste wagon, welke juist het blok verliet, dan geen acties alles is ok
            If nieuweNaarBlokNR = m_blok(blokNR).naarBlokNR AndAlso OudeVanBlokNR = m_blok(nieuweNaarBlokNR).vanBlokNR Then Exit Sub 'niets aan de hand
            If s88NR = m_blok(blokNR).s88AfremNR Then Exit Sub 'nieuw voor niets aan de hand
            Beep()
            RaiseEvent handlerChangeBlok("S", blokNR, LOKVRIJ)
            RaiseEvent handlerStatus("ER KWAM EEN ONBEKENDE TREIN op blokNR= " + blokNR.ToString + " via s88NR= " + s88NR.ToString)
        End If
    End Sub

    Private Function ReiswegVRIJ(ByVal rwDeel As String, ByVal locoNr As Integer) As Boolean
        Dim sw As Boolean = True        'true= vrij , false= bezet
        Dim i, j, nr As Integer
        Dim beideS88VRIJ As Boolean = False
        If rwDeel.IndexOf("o") = 0 Then j = 5 Else j = 0
        For i = j To rwDeel.Length - 3 Step 4
            nr = CInt(Val(rwDeel.Substring(i + 1, 3)))
            Select Case rwDeel.Substring(i, 1).ToLower
                Case "0", "1", "x", "s"    'wissels en kruisingen
                    SyncLock syncOK
                        If m_ArrayK83K84(nr).status = True Then
                            sw = False
                            Exit For
                        End If
                    End SyncLock

                Case "b", "B"
                    SyncLock syncOK
                        If m_blok(nr).status OrElse m_blok(nr).statusNaarRIJsectie Then
                            sw = False
                            Exit For
                        End If
                    End SyncLock

                    SyncLock syncOK1
                        If instellingen.OHblokken.Contains(Format(nr, "000")) Then
                            sw = False
                            Exit For
                        End If
                    End SyncLock

                Case "w", "v", "W", "V"

                    SyncLock syncOK
                        If Not (m_blok(nr).statusPreReserved AndAlso locoNr = m_blok(nr).locoNR) Then
                            If m_blok(nr).status OrElse m_blok(nr).statusNaarRIJsectie Then
                                sw = False
                                Exit For
                            End If
                        End If
                    End SyncLock
                    SyncLock syncOK1
                        If instellingen.OHblokken.Contains(Format(nr, "000")) Then
                            sw = False
                            Exit For
                        End If
                    End SyncLock

                Case "n"    'track of tussenstuk in blokken of rangeerstuk
                    If g_TrackArray(nr) Then
                        sw = False
                        Exit For
                    End If
                Case Else
                    'andere cmds niet evalueren
            End Select
        Next
        Return sw
    End Function

    Private Sub OpmaakBevelen(ByVal rwDeel As String, ByVal locoNR As Integer, ByVal blokNR As Integer)
        Dim i, j, w, nr, deltaBlok As Integer
        Dim swTraag As Boolean = False
        Dim delta As Boolean = False
        Dim ReisplanOmkeerTrigger As Boolean = False
        Dim s As String = instellingen.OHblokken
        Try
            Select Case rwDeel.Substring(0, 1)
                Case "j", "h"                'nodig voor vrijeTrein
                    OpmaakExtraBevelen(rwDeel, locoNR, blokNR)
                    Exit Sub

                Case "k"   'keren
                    OpmaakExtraBevelen(rwDeel, locoNR, blokNR)
                    'aanpassing rwDeel voor verdere bewerkingen
                    rwDeel = m_treinen.Item(locoNR).rwDeel.Remove(0, 4)
                    m_treinen.Item(locoNR).rwDeel = rwDeel
                    Exit Sub

                Case "o" 'omkeren bij reisplan wissel    
                    ReisplanOmkeerTrigger = True
                    OpmaakExtraBevelen(rwDeel, locoNR, blokNR)
                    'aanpassing rwDeel voor verdere bewerkingen: verwijderen o000
                    rwDeel = m_treinen.Item(locoNR).rwDeel.Remove(0, 5)
                    m_treinen.Item(locoNR).rwDeel = rwDeel
                    m_treinen(locoNR).Reisweg = "|" + m_treinen(locoNR).Reisweg

                Case "y", "Y"   'blok met oa. wissel(s) en volgblokken
                    Dim reisweg As String = rwDeel
                    rwDeel = rwDeel.Substring(4)
                    rwDeel = rwDeel.Replace("b", "v") : rwDeel = rwDeel.Replace("B", "V")
                    If rwDeel.IndexOfAny(CType("wW", Char())) = -1 Then
                        'geen wisselblok aanwezig, de eerste "v" wordt dan "b"
                        i = rwDeel.IndexOf("v")
                        If i <> -1 Then rwDeel = rwDeel.Substring(0, i) + "b" + rwDeel.Substring(i + 1)
                        i = rwDeel.IndexOf("V")
                        If i <> -1 Then rwDeel = rwDeel.Substring(0, i) + "B" + rwDeel.Substring(i + 1)
                    Else
                        rwDeel = rwDeel.Replace("w", "b") : rwDeel = rwDeel.Replace("W", "b")
                    End If
                    m_treinen.Item(locoNR).rwDeel = rwDeel
                    i = rwDeel.IndexOfAny(CType("bB", Char()))
                    ' Dim preReservedBlok As Integer = CInt(rwDeel.Substring(i + 1, 3))
                    m_blok(CInt(rwDeel.Substring(i + 1, 3))).statusPreReserved = True

                    'reisweg juist zetten
                    Dim index As Integer = reisweg.IndexOfAny("wW".ToCharArray)
                    reisweg = reisweg.Substring(index + 4)   'start vanaf eerste volgblok tot en met bestemmingsblok
                    reisweg = reisweg.Replace("v", "b") : reisweg = reisweg.Replace("V", "B")
                    If m_treinen.Item(locoNR).VrijeTrein Then
                        m_treinen.Item(locoNR).Reisweg = reisweg    ' preReservedBlok +
                    Else        'geplande reisweg
                        'volgblokken en bestemmingsblok bewaren in reisweg voorafgegaan door "|"
                        reisweg = reisweg.Replace("b", "|b") : reisweg = reisweg.Replace("B", "|B")
                        m_treinen.Item(locoNR).Reisweg = reisweg + m_treinen.Item(locoNR).Reisweg
                    End If

                Case "l", "f", "q"  'frontlicht, F1-F4, macro
                    OpmaakExtraBevelen(rwDeel, locoNR, blokNR)
                    'aanpassing rwDeel nodig
                    rwDeel = m_treinen.Item(locoNR).rwDeel.Remove(0, 4)    'verwijder fxxx
                    m_treinen.Item(locoNR).rwDeel = rwDeel

                Case Else
                    'gewoon doorgaan
            End Select
            Dim rwDeelO As String = rwDeel
            Dim Wissels As String = String.Empty

            'verwerk rwDeel naar bevelen
            For i = 0 To rwDeel.Length - 3 Step 4
                nr = CInt(rwDeel.Substring(i + 1, 3))
                Select Case rwDeel.Substring(i, 1).ToLower
                    Case "0"   'afbuigen aan minimum snelheid
                        SyncLock syncOK
                            dataD.address = rwDeel.Substring(i + 1, 3)
                            dataD.port = "0"
                            queueTurnout.Enqueue(dataD)
                            m_ArrayK83K84(nr).port = "0"
                            m_ArrayK83K84(nr).status = True
                        End SyncLock
                        Wissels &= rwDeelO.Substring(i, 4)
                        If m_ArrayK83K84(nr).type = "K" Then swTraag = True
                    Case "1"   'rechtdoor gewone wissels aan maximum snelheid
                        SyncLock syncOK
                            dataD.address = rwDeel.Substring(i + 1, 3)
                            dataD.port = "1"
                            queueTurnout.Enqueue(dataD)
                            m_ArrayK83K84(nr).port = "1"
                            m_ArrayK83K84(nr).status = True
                        End SyncLock
                        Wissels &= rwDeelO.Substring(i, 4)
                    Case "x"        'kruisingen 
                        SyncLock syncOK
                            m_ArrayK83K84(nr).port = "x"
                            m_ArrayK83K84(nr).status = True
                        End SyncLock
                        Wissels &= rwDeelO.Substring(i, 4)
                    Case "b", "B"
                        'naarblokNR = nr   ,  huidig blok= blokNR
                        m_blok(nr).statusNaarRIJsectie = True   'trein wordt verwacht op blok
                        m_blok(nr).controleBlokNR = nr
                        m_blok(nr).locoNR = locoNR              'tijdens bedrijf enige plaats om locoNR toe te kennen!
                        VerwachteBlok(locoNR) = nr              'naarblokNR =  nr
                        m_blok(nr).SynopLocoNR = locoNR
                        m_blok(nr).vanBlokNR = blokNR
                        m_blok(nr).statusSynop = "G"
                        m_blok(nr).wissels = Wissels            'over te rijden wissels bewaren
                        RaiseEvent handlerChangeBlok("G", nr, locoNR)
                        If m_blok(blokNR).statusSynop = "R" Then    'na Retriggering, 
                            m_blok(blokNR).statusSynop = "A"
                            RaiseEvent handlerChangeBlok("A", blokNR, locoNR)
                        End If

                        'seinbehandeling
                        SyncLock syncOK1
                            If Wissels <> String.Empty Then     'er is een wisselstraat dus w= toegangssein
                                'synop wissels bezetten
                                For j = 0 To Wissels.Length - 3 Step 4
                                    w = CInt(Wissels.Substring(j + 1, 3))
                                    Select Case m_blok(blokNR).statusSynop
                                        Case "B"
                                            RaiseEvent handlerChangeTurnout("G", Wissels.Substring(j, 1), w.ToString)
                                        Case "R"
                                            RaiseEvent handlerChangeTurnout("A", Wissels.Substring(j, 1), w.ToString)
                                        Case Else
                                            RaiseEvent handlerChangeTurnout("A", Wissels.Substring(j, 1), w.ToString)
                                    End Select
                                Next
                                'seinbehandeling enkel voor "w" = toegangssein 
                                If m_blok(blokNR).bit = 0 Then  'richting paar naar onpaar s88nr (waarde vóór #-teken)
                                    Select Case m_blok(blokNR).K83K84.Substring(0, 1)
                                        Case "w"
                                            dataD.address = m_blok(blokNR).K83K84.Substring(1, 3)
                                            dataD.port = "1"
                                            queueTurnout.Enqueue(dataD)
                                            m_blok(nr).wisselsein =  m_blok(blokNR).K83K84.Substring(1, 3)
                                        Case "d"    '  w deel met adres van b +1
                                            dataD.address = Format(CInt(m_blok(blokNR).K83K84.Substring(1, 3)) + 1, "000")
                                            dataD.port = "1"
                                            queueTurnout.Enqueue(dataD)
                                            m_blok(nr).wisselsein = Format(CInt(m_blok(blokNR).K83K84.Substring(1, 3)) + 1, "000")
                                        Case Else
                                            ' no action
                                    End Select
                                Else        'richting onpaar naar paar s88nr  (waarde ná #-teken)
                                    Select Case m_blok(blokNR).K83K84.Substring(5, 1)
                                        Case "w"
                                            dataD.address = m_blok(blokNR).K83K84.Substring(6, 3)
                                            dataD.port = "1"
                                            queueTurnout.Enqueue(dataD)
                                            m_blok(nr).wisselsein = m_blok(blokNR).K83K84.Substring(6, 3)
                                        Case "d"    '  w deel met adres van b +1
                                            dataD.address = Format(CInt(m_blok(blokNR).K83K84.Substring(6, 3)) + 1, "000")
                                            dataD.port = "1"
                                            queueTurnout.Enqueue(dataD)
                                            m_blok(nr).wisselsein = Format(CInt(m_blok(blokNR).K83K84.Substring(1, 3)) + 1, "000")
                                        Case Else
                                            ' no action
                                    End Select
                                End If
                            End If
                        End SyncLock
                        'vervolledig 
                        m_blok(blokNR).naarBlokNR = nr              'naarBlokNR, waar de trein naartoe rijdt
                        m_treinen.Item(locoNR).BlokNR = nr          'naarBlokNR, waar de trein naartoe rijdt
                        m_treinen.Item(locoNR).VanBlokNR = blokNR   'blokNR waar de trein van wegrijdt
                        g_locoData(locoNR).vanBlokNR = blokNR       'blokNR waar de trein van wegrijdt
                        Snelheidsregeling(rwDeel, locoNR, blokNR, deltaBlok, swTraag)
                        If m_treinen.Item(locoNR).MultiBlokFase Then
                            swTraag = False
                            blokNR = nr
                        End If
                        'reisplan extra doorschuif actie na halt en omkeren
                        If ReisplanOmkeerTrigger Then Doorschuiven(locoNR)

                    Case "v", "v"
                        m_blok(nr).statusNaarRIJsectie = True           'bezetten van de blok
                        'preReservatie om reiswegVrij vrijstelling te genieten
                        m_blok(nr).controleBlokNR = nr
                        m_blok(nr).statusPreReserved = True
                        m_blok(nr).locoNR = locoNR
                        m_blok(nr).statusSynop = "Q"    'colorPreReserved
                        RaiseEvent handlerChangeBlok("Q", nr, locoNR)

                    Case Else
                        rwDeel = rwDeel.Remove(0, 1)
                        i -= 4
                        ' code zonder waarde
                End Select
            Next
        Catch ex As Exception

            RaiseEvent handlerStatus("Fout in OpmaakBevelen, zie reiswegen en conditieRw na!")
        End Try
    End Sub

    Private Sub OpmaakExtraBevelen(ByVal rwDeel As String, ByVal locoNR As Integer, ByVal blokNR As Integer)
        Dim nr, v As Integer
        nr = CInt(rwDeel.Substring(1, 3))
        Select Case rwDeel.Substring(0, 1).ToLower
            Case "j"        'snelheidsverlaging 
                'snelheidsverlaging
                v = CInt(instellingen.StopAfremSnelheid) - (nr \ 100)
                If v < 2 Then v = 2 'niet onder snelheid 2 gaan
                m_ArrayLOCO(locoNR).address = locoNR.ToString
                m_ArrayLOCO(locoNR).speed = v.ToString
                queueLocoGO.Enqueue(locoNR)
                'haltTijd
                m_treinen.Item(locoNR).HalteTijd = _
                Microsoft.VisualBasic.DateAndTime.Timer() + Convert.ToDouble(nr Mod 100)
                If rwDeel.Substring(4, 1).ToLower = "k" _
                Or rwDeel.Substring(4, 1).ToLower = "f" _
                Or rwDeel.Substring(4, 1).ToLower = "l" Then
                    m_treinen.Item(locoNR).rwDeel = rwDeel.Substring(4)
                Else
                    m_treinen.Item(locoNR).rwDeel = rwDeel.Substring(5)
                End If
                m_haltBlokken.Append(Format(blokNR, "000"))
                m_blok(blokNR).statusSynop = "H"
                RaiseEvent handlerChangeBlok("H", blokNR, locoNR)
                m_blok(blokNR).locoStop = True
                m_treinen.Item(locoNR).Gestopt = 2

            Case "h"        'halt
                'snelheidsverlaging
                v = CInt(instellingen.StopAfremSnelheid)
                m_ArrayLOCO(locoNR).address = locoNR.ToString
                m_ArrayLOCO(locoNR).speed = v.ToString
                queueLocoGO.Enqueue(locoNR)
                'haltTijd
                If rwDeel.Substring(4, 1).ToLower = "k" _
                    Or rwDeel.Substring(4, 1).ToLower = "o" _
                    Or rwDeel.Substring(4, 1).ToLower = "f" _
                    Or rwDeel.Substring(4, 1).ToLower = "l" Then
                    m_treinen.Item(locoNR).rwDeel = rwDeel.Substring(4) 'zonder |
                Else
                    m_treinen.Item(locoNR).rwDeel = rwDeel.Substring(5) 'met | beginnend
                End If
                m_haltBlokken.Append(Format(blokNR, "000"))    'toevoegen aan haltstring
                m_blok(blokNR).statusSynop = "H"
                RaiseEvent handlerChangeBlok("H", blokNR, locoNR)
                m_treinen.Item(locoNR).HalteTijd = _
                Microsoft.VisualBasic.DateAndTime.Timer() + Convert.ToDouble(nr Mod 100)
                m_treinen.Item(locoNR).Gestopt = 1
                m_blok(blokNR).locoStop = True

            Case "k", "o"    'keren en omkeren bij reisplanwissel
                m_blok(blokNR).statusK = True  'keren actief
                If m_blok(blokNR).bit = 1 Then m_blok(blokNR).bit = 0 Else m_blok(blokNR).bit = 1
                'omkeren van wat de rijrichting voorheen was
                If m_ArrayLOCO(locoNR).direction = "forwards" Then m_ArrayLOCO(locoNR).direction = "rearwards" Else m_ArrayLOCO(locoNR).direction = "forwards"
                m_treinen.Item(locoNR).LocoDIR = Not m_treinen.Item(locoNR).LocoDIR
                m_ArrayLOCO(locoNR).address = locoNR.ToString
                m_ArrayLOCO(locoNR).speed = "0"
                queueLocoSTOP.Enqueue(locoNR)
                'vertrekvertraging na keren
                m_treinen.Item(locoNR).rwDeel = rwDeel.Substring(4) 'verwijder kxxx cmd
                m_haltBlokken.Append(Format(blokNR, "000"))    'toevoegen aan haltstring
                m_blok(blokNR).statusSynop = "H"
                RaiseEvent handlerChangeBlok("H", blokNR, locoNR)
                m_treinen.Item(locoNR).HalteTijd = _
                Microsoft.VisualBasic.DateAndTime.Timer() + Convert.ToDouble(rwDeel.Substring(1, 3))
                m_treinen.Item(locoNR).Gestopt = 2
                m_blok(blokNR).locoStop = True


            Case "l"        'frontsein licht aanzetten
                v = m_treinen.Item(locoNR).Snelheid
                'g_locoData(locoNR).F0 = 1
                m_ArrayLOCO(locoNR).F0 = "1"
                m_ArrayLOCO(locoNR).address = locoNR.ToString
                m_ArrayLOCO(locoNR).speed = v.ToString
                queueLocoGO.Enqueue(locoNR)

            Case "f"   'functies F1-F4 aanzetten
                'v = m_treinen.Item(locoNR).Snelheid
                'Select Case CInt(rwDeel.Substring(1, 3))      'hieruit moet fe F1-F4 waarden gehaald worden   12345
                '    Case 1
                '        m_ArrayLOCO(locoNR).F1 = "1"

                'End Select

                'm_ArrayLOCO(locoNR).address = locoNR.ToString
                'm_ArrayLOCO(locoNR).speed = v.ToString
                'queueLocoGO.Enqueue(locoNR)
            Case "q"    'play macro
                RunMacro(CInt(rwDeel.Substring(1, 3)))
            Case Else
                'geen geldige reden hier te komen
        End Select
    End Sub

    Public Function Doorschuiven(ByVal locoNR As Integer) As Boolean
        Dim i As Integer
        Dim s, RWdeel, reisweg As String
        Try
            reisweg = m_treinen.Item(locoNR).Reisweg
            i = reisweg.IndexOfAny("bB".ToCharArray) + 3
            s = reisweg.Substring(0, 1)
            Select Case s
                Case "|"
                    RWdeel = reisweg.Substring(1, i)
                    reisweg = reisweg.Substring(i + 1) & s & RWdeel
                Case "j", "h", "l", "f", "p"
                    RWdeel = reisweg.Substring(0, i + 1)
                    reisweg = reisweg.Substring(i + 1) & RWdeel
                Case "E", "e"
                    RWdeel = reisweg.Substring(0, i + 1)
                Case "c", "p", "t"
                    RWdeel = reisweg.Substring(0, i + 1)
                Case "w"
                    RWdeel = reisweg
                Case Else
                    Return False
                    Exit Function
            End Select
            'actualiseren
            m_treinen.Item(locoNR).rwDeel = RWdeel
            m_treinen.Item(locoNR).Reisweg = reisweg
            Return True
        Catch
            Return False
        End Try
    End Function

    Private Sub RijsectieVertraging(ByVal locoNR As Integer, ByVal blokNR As Integer, ByVal rwDeel As String)
        'vertraag alvast op Rijsectie (voorseinBevel) 
        Dim snelheid As Integer = CInt(instellingen.StopAfremSnelheid)
        If snelheid < 3 Then snelheid = 3
        If snelheid > 14 Then snelheid = 14
        If LocoSpeedChange(locoNR, snelheid) Then
            m_ArrayLOCO(locoNR).address = locoNR.ToString
            m_ArrayLOCO(locoNR).speed = snelheid.ToString
            queueLocoGO.Enqueue(locoNR)
            m_treinen.Item(locoNR).Snelheid = snelheid
        End If
        'voorbereiding tot stoppen op Afremsectie
        m_treinen.Item(locoNR).Gestopt = 1   '1= Rijvertraging
        m_blok(blokNR).locoStop = True   'stoppen op Afremsectie
        'zie na of het rwDeel geen fxxx bevat, deze eerst verwijderen
        Do While m_treinen.Item(locoNR).rwDeel.StartsWith("f")
            m_treinen.Item(locoNR).rwDeel = m_treinen.Item(locoNR).rwDeel.Substring(4)
            RaiseEvent handlerRWdeel(locoNR, m_treinen.Item(locoNR).rwDeel & "[" & m_treinen.Item(locoNR).EindBlok & "]")
        Loop
    End Sub

    Private Sub EindeRWverwerking(ByVal blokNR As Integer, ByVal locoNR As Integer, ByVal rwDeel As String)
        Dim volgRW, nr As Integer
        Dim halt As String = String.Empty
        Dim header As String = String.Empty
        If m_treinen.Item(locoNR).Reiswegen.Length > 3 Then   'nog een andere reisweg uitvoeren
            If m_treinen.Item(locoNR).Reiswegen.Substring(3, 3) = "ppp" Then 'perp.Mobile 
                'wijzig ppp voor het in rw+ppp
                m_treinen.Item(locoNR).Reiswegen = m_treinen.Item(locoNR).Reiswegen.Substring(0, 3) & "ppp" 'mag zo blijven, stoort niet
            Else
                'is er een tussen halt voorzien?
                If m_treinen.Item(locoNR).Reiswegen.Substring(3, 1) = "h" Then
                    halt = m_treinen.Item(locoNR).Reiswegen.Substring(3, 4)
                    'shift xxxhxxx, volgende reisweg vooraan zetten
                    m_treinen.Item(locoNR).Reiswegen = m_treinen.Item(locoNR).Reiswegen.Substring(7)
                Else
                    'shift xxx, volgende reisweg vooraan zetten
                    m_treinen.Item(locoNR).Reiswegen = m_treinen.Item(locoNR).Reiswegen.Substring(3)
                End If
            End If
            volgRW = CInt(m_treinen.Item(locoNR).Reiswegen.Substring(0, 3))
            m_treinen.Item(locoNR).Reisweg = g_ReisWegenArray(volgRW).Substring(5) ' zonder "|" teken 12345  ++++++
            'haal start reiswegdeel op
            rwDeel = m_treinen.Item(locoNR).Reisweg.Substring(0, m_treinen.Item(locoNR).Reisweg.IndexOfAny("bB".ToCharArray) + 4)
            'onderzoek of er moet gekeerd worden, onderzoek reisweg header
            header = g_ReisWegenArray(volgRW).Substring(0, 4)
            If header.Substring(1, 1) = "2" OrElse header.Substring(1, 1) = "3" Then
                'rijrichting omkeren, voeg omkeerbevel o000| toe in rwdeel, | niet vergeten nodig voor normale doorschuiven
                rwDeel = "o000|" & rwDeel
            End If
            'toevoegen van het halte bevel indien aanwezig
            rwDeel = halt & rwDeel
            m_treinen.Item(locoNR).rwDeel = rwDeel     'nieuw reiswegdeel aanpassen
            If halt <> String.Empty Then                'Is er een halt bevel?
                OpmaakBevelen(rwDeel, locoNR, blokNR)   'voer eerst halt bevel uit
            Else
                'onderzoek reisweg tot en met de volgende blok
                If ReiswegVRIJ(rwDeel, locoNR) = True Then 'Reisweg is vrij, trein mag doorrijden
                    OpmaakBevelen(rwDeel, locoNR, blokNR)
                    m_treinen.Item(locoNR).Reisweg = g_ReisWegenArray(volgRW).Substring(4) ' met "|" teken 12345  ++++++
                    Doorschuiven(locoNR)
                Else        'reisweg is bezet, trein moet stoppen en geRetriggerd worden
                    Dim strBlok As String = Format(blokNR, "000")
                    SyncLock syncOK1
                        m_retriggerBlokken.Append(strBlok)
                        m_blok(blokNR).statusSynop = "R"
                        RaiseEvent handlerChangeBlok("R", blokNR, locoNR)
                    End SyncLock
                    m_treinen.Item(locoNR).Gestopt = 0
                    m_blok(blokNR).locoStop = True
                    'snelheid voor rijsectie aanpassen
                    nr = CInt(instellingen.StopAfremSnelheid)
                    If nr < 2 Then nr = 2
                    If nr > 14 Then nr = 14
                    If LocoSpeedChange(locoNR, nr) Then
                        m_ArrayLOCO(locoNR).address = locoNR.ToString
                        m_ArrayLOCO(locoNR).speed = nr.ToString
                        queueLocoGO.Enqueue(locoNR)
                        m_treinen.Item(locoNR).Snelheid = nr
                    End If
                    If Doorschuiven(locoNR) Then
                        'extra doorschuiven reisweg na RW wissel
                    Else
                        RaiseEvent handlerStatus("EindeRWVerwerking: doorschuiven niet gelukt")
                    End If
                    RaiseEvent handlerRWdeel(locoNR, m_treinen.Item(locoNR).rwDeel & "[" & m_treinen.Item(locoNR).EindBlok & "]")
                End If
            End If
        Else    'reisweg beëindigen
            'loco stoppen op Afremsectie
            m_blok(blokNR).locoStop = True
            'snelheid voor rijsectie aanpassen
            nr = CInt(instellingen.StopAfremSnelheid)
            If nr < 2 Then nr = 2
            If nr > 14 Then nr = 14
            If LocoSpeedChange(locoNR, nr) Then
                m_ArrayLOCO(locoNR).address = locoNR.ToString
                m_ArrayLOCO(locoNR).speed = nr.ToString
                queueLocoGO.Enqueue(locoNR)
                m_treinen.Item(locoNR).Snelheid = nr
            End If
            m_treinen.Item(locoNR).Gestopt = 0
        End If
    End Sub

    Private Sub Snelheidsregeling(ByVal rwDeel As String, ByVal locoNR As Integer, ByVal blokNR As Integer, ByVal deltaBlok As Integer, ByVal swTraag As Boolean)
        'AfremsectieSnelheid bepalen
        Dim timeNow As Double
        Dim s As String
        Dim v As Integer    'snelheid
        Dim vAfrem As Integer
        If swTraag = True Then
            v = m_blok(blokNR).minSnelheid + deltaBlok
        Else
            v = m_blok(blokNR).maxSnelheid + deltaBlok
        End If
        If rwDeel.IndexOf("B") > 0 Then v += m_treinen(locoNR).DeltaSnelheid
        'snelheid limiten
        If v < 3 Then v = 3
        If v > 14 Then v = 14
        m_blok(blokNR).afremSnelheid = v    'bewaar in blok property
        vAfrem = v  'tijdelijk bewaren

        'RijsectieSnelheid bepalen
        v = m_blok(blokNR).maxSnelheid + deltaBlok  'altijd maximum
        If rwDeel.IndexOf("B") > 0 Then v += m_treinen(locoNR).DeltaSnelheid
        'snelheid limieten
        If v < 3 Then v = 3
        If v > 14 Then v = 14
        If m_treinen.Item(locoNR).Hertriggering Then 'opgeroepen vanaf sub Retrigger
            m_treinen.Item(locoNR).Hertriggering = False 'reset
            If LocoSpeedChange(locoNR, vAfrem) Then
                m_ArrayLOCO(locoNR).address = locoNR.ToString
                m_ArrayLOCO(locoNR).speed = vAfrem.ToString
                If m_treinen.Item(locoNR).Gestopt = 1 Then  '0 en 2= geen startvertraging, 1= startvertraging
                    'startvertraging inlassen
                    timeNow = Microsoft.VisualBasic.DateAndTime.Timer() + CInt(instellingen.startVertraging)
                    s = Format(locoNR, "000") & Format(timeNow, "00000") & Format(v, "00")
                    StartVertragingQueue.Enqueue(s)
                Else
                    m_ArrayLOCO(locoNR).address = locoNR.ToString
                    m_ArrayLOCO(locoNR).speed = v.ToString
                    queueLocoGO.Enqueue(locoNR)
                End If
                m_treinen.Item(locoNR).Snelheid = vAfrem
            End If
        Else    'normale uitvoering
            If LocoSpeedChange(locoNR, v) Then
                m_ArrayLOCO(locoNR).address = locoNR.ToString
                m_ArrayLOCO(locoNR).speed = v.ToString
                If m_treinen.Item(locoNR).Gestopt = 1 Then  '0 en 2= geen startvertraging, 1= startvertraging
                    'startvertraging inlassen
                    timeNow = Microsoft.VisualBasic.DateAndTime.Timer() + CInt(instellingen.startVertraging)
                    s = Format(locoNR, "000") & Format(timeNow, "00000") & Format(v, "00")
                    StartVertragingQueue.Enqueue(s)
                Else
                    m_ArrayLOCO(locoNR).address = locoNR.ToString
                    m_ArrayLOCO(locoNR).speed = v.ToString
                    queueLocoGO.Enqueue(locoNR)
                End If
                m_treinen.Item(locoNR).Snelheid = v
            End If
        End If
    End Sub

    Private Function LocoSpeedChange(ByVal locoNR As Integer, ByVal snelheid As Integer) As Boolean
        Try
            If m_treinen.Item(locoNR).Snelheid = snelheid Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Return True
        End Try
    End Function

    Private Function MultiBlokVerwerking(ByVal blokNR As Integer, ByVal locoNR As Integer, ByVal rwDeel As String, ByVal reisweg As String) As Boolean

        Dim rwVOORmb As String = ""     'voor reiswegVRIJ onderzoek
        Dim rwNAmb As String = reisweg  'herstel van treinen.item(LocoNR).Reisweg
        Dim i, j, k As Integer
        Dim strBlokNR As String = "|" & rwDeel.Substring(rwDeel.IndexOfAny("bB".ToCharArray), 4)
        Dim aantalMultiBlok As Integer = CInt(rwDeel.Substring(1, 3)) - 1 'aantal blokken in multiblok
        m_treinen.Item(locoNR).MultiBlokFase = True

        'zoek begin reisweg na de multiblok

        For i = 0 To reisweg.Length - 3 Step 4
            j = reisweg.IndexOfAny("bB".ToCharArray, j + 1)
            k += 1
            If k = aantalMultiBlok Then
                m_treinen.Item(locoNR).MultiBlokNR = CInt(reisweg.Substring(j + 1, 3))
                Exit For
            End If
        Next
        rwDeel = rwDeel.Substring(4)        ' mxxx verwijderen
        rwVOORmb = rwDeel
        ' aanvullen van de bevelen voor de bijkomende multieblokken te vinden in reisweg
        reisweg = reisweg.Substring(1)        'aanpassen zonder eerste |
        For i = 0 To k - 1        'bijkomende blokken
            For j = 0 To reisweg.IndexOfAny("bB".ToCharArray) Step 4
                rwVOORmb &= reisweg.Substring(j, 4)
            Next
            reisweg = reisweg.Substring(5)
        Next
        m_treinen.Item(locoNR).rwDeel = m_treinen.Item(locoNR).rwDeel.Remove(0, 4)
        If ReiswegVRIJ(rwVOORmb, locoNR) Then
            OpmaakBevelen(rwVOORmb, locoNR, blokNR)   'startfase: enkel alle blokken, wisselstraten bezetten
            'hertstel van de reisweg
            m_treinen.Item(locoNR).Reisweg = rwNAmb    'strBlokNR &
            Return True
        Else
            Return False
        End If
    End Function

    Private Function ConditieVerwerking(ByVal blokNR As Integer, ByVal locoNR As Integer, ByVal s As String) As Boolean
        Dim i, j As Integer
        Dim nr As Integer = CInt(s.Substring(1, 3))   'nr van de conditie reisweg
        Dim conditieRW As String = g_ConditieRWArray(nr)
        Dim rwDeel As String
        Do
            i = conditieRW.IndexOfAny("bB".ToCharArray) + 4 'eerste b
            rwDeel = conditieRW.Substring(1, i - 1)
            If ReiswegVRIJ(rwDeel, locoNR) = True Then  'een vrije reisweg gevonden
                OpmaakBevelen(rwDeel, locoNR, blokNR)
                m_blok(blokNR).locoStop = False  '  er moet niet meer gestopt worden
                'aanpassen reisweg
                j = conditieRW.IndexOfAny("bB".ToCharArray, i) + 4  'tweede b
                rwDeel = conditieRW.Substring(i, j - i)
                rwDeel &= m_treinen.Item(locoNR).Reisweg.Substring(8)
                m_treinen.Item(locoNR).Reisweg = conditieRW.Substring(i, j - i) & m_treinen.Item(locoNR).Reisweg.Substring(8)
                RaiseEvent handlerRWdeel(locoNR, m_treinen.Item(locoNR).rwDeel & "[" & m_treinen.Item(locoNR).EindBlok & "]")
                Return True
                Exit Function
            Else
                i = conditieRW.IndexOfAny("bB".ToCharArray, i) + 4
                conditieRW = conditieRW.Substring(i)
            End If
        Loop Until conditieRW = ""
        Return False
    End Function

    Private Function TerminusVerwerking(ByVal blokNR As Integer, ByVal locoNR As Integer, ByVal s As String) As Boolean
        Dim i As Integer
        Dim nr As Integer = CInt(s.Substring(1, 3))   'nr terminusreisweg
        Dim terminusRW As String = g_ConditieRWArray(nr)
        Dim rwDeel As String
        Do
            i = terminusRW.IndexOfAny("bB".ToCharArray) + 4
            rwDeel = terminusRW.Substring(1, i - 1)
            If ReiswegVRIJ(rwDeel, locoNR) Then  'een vrije reisweg gevonden
                OpmaakBevelen(rwDeel, locoNR, blokNR)
                'aanpassen reisweg
                m_treinen.Item(locoNR).Reisweg = terminusRW.Substring(0, i) & s.Substring(4)
                If Doorschuiven(locoNR) Then
                    'doorschuiven 
                Else

                    RaiseEvent handlerStatus("TerminusVerwerking: doorschuiven niet gelukt")
                End If
                RaiseEvent handlerRWdeel(locoNR, m_treinen.Item(locoNR).rwDeel & "[" & m_treinen.Item(locoNR).EindBlok & "]")
                Return True
                Exit Function
            Else
                terminusRW = terminusRW.Substring(i)
            End If
        Loop Until terminusRW = ""
        Return False
    End Function

    Private Sub StopAlleTreinen(ByVal foutBlokNR As Integer, ByVal s88NR As Integer)
        Dim locoNR As Integer = 0
        Dim i As Integer
        'stop alle treinen
        For i = 0 To m_treinenInDienst.Length - 1 Step 4
            locoNR = CInt(m_treinenInDienst.Substring(i + 1, 3))
            m_ArrayLOCO(locoNR).address = locoNR.ToString
            m_ArrayLOCO(locoNR).speed = "0"
            queueLocoSTOP.Enqueue(locoNR)
        Next
        Beep()
        RaiseEvent handlerLogData("Alle treinen gestopt, ONDERZOEK gegevens voor blokNR= " & (s88NR \ 2 + 1).ToString + " s88nr= " + s88NR.ToString)
    End Sub

    Private Function VerwerkBlok(ByVal param2 As String, ByVal param3 As String, ByVal blNR As Integer, ByVal blokNR As Integer) As Integer
        Dim ok, boolWaarde As Boolean
        Dim tab1, tab2, tab3 As Integer
        Dim intWaarde As Integer = 0
        Dim eigenschap, strWaarde As String

        Do     'param2 kan meerdere testen bevatten
            tab1 = param2.IndexOfAny("=><".ToCharArray) ' vergelijkteken
            tab2 = param2.IndexOf("&")   'and clausule
            tab3 = param2.IndexOf("|")   'or clausule
            eigenschap = param2.Substring(0, tab1)
            strWaarde = param2.Substring(tab1 + 1)
            'zet strWaarde in het juiste formaat
            If IsNumberOK(strWaarde) Then
                intWaarde = CInt(strWaarde)
            ElseIf strWaarde.ToLower.StartsWith("w") Then
                boolWaarde = True
            ElseIf strWaarde.ToLower.StartsWith("f") Then
                boolWaarde = False
            End If

            'vergelijkteken select
            Select Case param2.Substring(tab1, 1)
                Case "="
                    Select Case eigenschap
                        Case "loconr"
                            If m_blok(blNR).locoNR = intWaarde Then ok = True Else ok = False
                        Case "status"
                            If m_blok(blNR).status = boolWaarde Then ok = True Else ok = False
                        Case "wissels"
                            If m_blok(blNR).wissels = strWaarde Then ok = True Else ok = False
                        Case Else
                            ok = False
                    End Select

                Case ">"
                    Select Case eigenschap
                        Case "loconr"
                            If m_blok(blNR).locoNR > intWaarde Then ok = True Else ok = False
                        Case Else
                            ok = False
                    End Select

                Case "<"
                    Select Case eigenschap
                        Case "loconr"
                            If m_blok(blNR).locoNR < intWaarde Then ok = True Else ok = False
                        Case "status"
                        Case Else
                            ok = False
                    End Select
                Case Else
                    MessageBox.Show("Verkeerd vergelijkteken, enkel =, > of < gebruiken")
            End Select
            If tab2 > 0 Then
                param2 = param2.Substring(tab2 + 1)
            ElseIf tab3 > 0 Then
                param2 = param2.Substring(tab3 + 1)
            Else
                param2 = ""
            End If
        Loop Until param2 = ""
        'param3
        If ok Then
            Return CInt(param3.Substring(0, 3))
        ElseIf param3.IndexOf(".") < 0 Then
            Return -1
        Else
            Return CInt(param3.Substring(4, 3))
        End If
    End Function

    Private Function Verwerkloco(ByVal param2 As String, ByVal param3 As String, ByVal blNR As Integer, ByVal blokNR As Integer) As Boolean
        Dim ok, boolWaarde As Boolean
        Dim i, tab1, tab2, tab3 As Integer
        Dim intWaarde As Integer = 0
        Dim eigenschap, strWaarde As String

        Do     'param2 kan meerdere testen bevatten
            tab1 = param2.IndexOfAny("=><".ToCharArray) ' vergelijkteken
            tab2 = param2.IndexOf("&")   'and clausule
            tab3 = param2.IndexOf("|")   'or clausule
            eigenschap = param2.Substring(0, tab1)
            strWaarde = param2.Substring(tab1 + 1)
            'zet strWaarde in het juiste formaat
            If IsNumberOK(strWaarde) Then
                intWaarde = CInt(strWaarde)
            ElseIf strWaarde = "True" Then
                boolWaarde = True
            ElseIf strWaarde = "False" Then
                boolWaarde = False
            End If

            'vergelijkteken select
            Select Case param2.Substring(tab1, 1)
                Case "="
                    SyncLock syncOK1
                        Select Case eigenschap
                            Case "inDienst"
                                If g_locoData(i).locoInDienst = intWaarde Then ok = True Else ok = False
                            Case "codeSoort"
                                If g_locoData(i).soortCode = intWaarde Then ok = True Else ok = False
                            Case "codeLengte"
                                If g_locoData(i).lengteCode = intWaarde Then ok = True Else ok = False
                            Case Else
                        End Select
                    End SyncLock
                Case ">"
                    SyncLock syncOK1
                        Select Case eigenschap
                            Case "inDienst"
                                If g_locoData(i).locoInDienst > intWaarde Then ok = True Else ok = False
                            Case "codeSoort"
                                If g_locoData(i).soortCode > intWaarde Then ok = True Else ok = False
                            Case "codeLengte"
                                If g_locoData(i).lengteCode > intWaarde Then ok = True Else ok = False
                            Case Else
                        End Select
                    End SyncLock
                Case "<"
                    SyncLock syncOK1
                        Select Case eigenschap
                            Case "inDienst"
                                If g_locoData(i).locoInDienst < intWaarde Then ok = True Else ok = False
                            Case "codeSoort"
                                If g_locoData(i).soortCode < intWaarde Then ok = True Else ok = False
                            Case "codeLengte"
                                If g_locoData(i).lengteCode < intWaarde Then ok = True Else ok = False
                            Case Else
                        End Select
                    End SyncLock
                Case Else
                    MessageBox.Show("Verkeerd vergelijkteken, enkel =, > of < gebruiken")
            End Select
            If tab2 > 0 Then
                param2 = param2.Substring(tab2 + 1)
            ElseIf tab3 > 0 Then
                param2 = param2.Substring(tab3 + 1)
            Else
                param2 = ""
            End If
        Loop Until param2 = ""
        'param3
        If param3.IndexOf(".") < 0 Then Return CBool(-1) : Exit Function 'verlaat macro
        If ok Then Return CBool(CInt(param3.Substring(0, 3))) Else Return CBool(CInt(param3.Substring(4, 3)))
    End Function

    Private Sub Retrigger()
        Try
            Dim strBlokNR As String
            Dim rwdeel As String = String.Empty
            Dim i, blokNR, locoNR As Integer
            Dim ok As Boolean = False

            'onderzoek of er een personentrein wacht: deze krijgt voorrang
            For i = 0 To m_retriggerBlokken.Length - 3 Step 3
                strBlokNR = m_retriggerBlokken.ToString.Substring(i, 3)
                blokNR = CInt(strBlokNR)
                locoNR = m_blok(blokNR).locoNR
                If locoNR = 0 Then
                    Beep()
                    RaiseEvent handlerStatus("Retrigger: blokNR= " + blokNR.ToString + " heeft locoNR= " + locoNR.ToString + " DIT IS FOUT!!!")
                    m_retriggerBlokken.Remove(i, 3)     'verwijder strBlokNR uit stringbuilder
                    SETPowerOnOff(False)
                    Exit Sub
                End If
                If g_locoData(locoNR).soortCode = 1 _
                    OrElse g_locoData(locoNR).soortCode = 2 _
                    OrElse g_locoData(locoNR).soortCode = 4 _
                    OrElse g_locoData(locoNR).soortCode = 6 _
                    OrElse g_locoData(locoNR).soortCode = 7 _
                    OrElse g_locoData(locoNR).soortCode = 9 Then
                    ok = True
                    m_retriggerBlokken.Remove(i, 3)     'verwijder strBlokNR uit stringbuilder
                    Exit For
                End If
            Next
            If ok Then      'personentrein aanwezig
                If locoNR > 0 Then
                    If m_treinen.Item(locoNR).VrijeTrein Then VrijeTreinProcessing(blokNR, locoNR)
                    rwdeel = m_treinen.Item(locoNR).rwDeel
                    If rwdeel = "" Then Exit Sub 'tgv uitdienstname()
                End If

                'Acties voor VrijeTrein: moet er keren toegevoegd worden?
                Select Case m_treinen.Item(locoNR).Type
                    Case Trein.TreinType.Pendel
                        If m_blok(blokNR).type = Blok.BlokType.Pendel _
                        AndAlso m_treinen.Item(locoNR).HaltPassagiersTrein <> "h000" Then
                            'k000 toevoegen vooraan rwDeel
                            rwdeel = "k000" & rwdeel
                            m_treinen.Item(locoNR).rwDeel = rwdeel
                        End If
                    Case Else   'Trein.TreinType: Standaard, Goederen, Personen, HST   
                        'geen keren toevoegen
                End Select

                'Acties voor elke trein
                Select Case rwdeel.Substring(0, 1).ToLower
                    Case "<"
                        'er is bij VrijeTrein geen vrije weg gevonden, terug Retriggeren
                        m_retriggerBlokken.Append(Format(blokNR, "000"))
                    Case "c"
                        If ConditieVerwerking(blokNR, locoNR, rwdeel) = False Then 'geen vrije reisweg
                            m_retriggerBlokken.Append(Format(blokNR, "000"))
                        End If
                    Case "t"
                        If TerminusVerwerking(blokNR, locoNR, rwdeel) = False Then 'geen vrije reisweg
                            m_retriggerBlokken.Append(Format(blokNR, "000"))
                        End If
                    Case "m"
                        If MultiBlokVerwerking(blokNR, locoNR, rwdeel, m_treinen.Item(locoNR).Reisweg) = False Then 'niet alle multiblokken zijn vrij 
                            'Retrigger
                            m_retriggerBlokken.Append(Format(blokNR, "000"))
                        End If
                    Case "j", "h"
                        OpmaakBevelen(rwdeel, locoNR, blokNR)
                        m_treinen.Item(locoNR).Hertriggering = True

                    Case Else   'gewoon geval voor wissel en blok sturing ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        If ReiswegVRIJ(rwdeel, locoNR) Then 'is reisweg vrij?
                            'Reisweg is vrij loco mag doorrrijden
                            Dim volgBlokNR As Integer = CInt(rwdeel.Substring(rwdeel.IndexOfAny("bB".ToCharArray) + 1, 3))
                            m_blok(volgBlokNR).locoNR = locoNR   'ophalen locoNR
                            m_treinen.Item(locoNR).Hertriggering = True  'acties vanaf Retrigger
                            OpmaakBevelen(rwdeel, locoNR, blokNR)
                            'vrijgave van afremsectie stop als trein rijdt op Rijsectie
                            m_blok(blokNR).locoStop = False  '
                        Else    'reisweg is NIET vrij, blok moet Retriggerd worden
                            m_retriggerBlokken.Append(Format(blokNR, "000"))
                        End If
                End Select
            End If

            'onderzoek eerste blok in Retriggering
            'Debug.WriteLine("In Retrigger eerste blok m_retriggerBlokken.length= " & m_retriggerBlokken.Length)
            If m_retriggerBlokken.Length > 2 Then
                strBlokNR = m_retriggerBlokken.ToString.Substring(0, 3)
                m_retriggerBlokken.Remove(0, 3)     'verwijder strBlokNR uit stringbuilder
                blokNR = CInt(strBlokNR)
                locoNR = m_blok(blokNR).locoNR
                If locoNR = 0 Then
                    Beep()
                    RaiseEvent handlerStatus("Retrigger: blokNR= " + blokNR.ToString + " heeft locoNR= " + locoNR.ToString + " DIT IS FOUT!!!")
                    m_retriggerBlokken.Remove(0, 3)     'verwijder strBlokNR uit stringbuilder
                    SETPowerOnOff(False)
                    Exit Sub
                End If
            Else
                Exit Sub
            End If
            If locoNR > 0 Then
                If m_treinen.Item(locoNR).VrijeTrein Then VrijeTreinProcessing(blokNR, locoNR)
                rwdeel = m_treinen.Item(locoNR).rwDeel
                If rwdeel = "" Then Exit Sub 'tgv uitdienstname()
            Else
                RaiseEvent handlerStatus("Retrigger: g_BlokLocoArray(" + blokNR.ToString + ") bevat het locoNR= " _
                                         + g_BlokLocoArray(blokNR) + " ,veroorzaakt geen storing")
                Exit Sub
            End If

            'Acties voor geplande trein: moet er keren toegevoegd worden?
            Select Case m_treinen.Item(locoNR).Type
                Case Trein.TreinType.Pendel
                    If m_blok(blokNR).type = Blok.BlokType.Pendel _
                    AndAlso m_treinen.Item(locoNR).HaltPassagiersTrein <> "h000" Then
                        'k000 toevoegen vooraan rwDeel
                        rwdeel = "k000" & rwdeel
                        m_treinen.Item(locoNR).rwDeel = rwdeel
                    End If
                Case Else   'Trein.TreinType: Standaard, Goederen, Personen, HST   
                    'geen keren toevoegen
            End Select
            Select Case rwdeel.Substring(0, 1).ToLower
                Case "<"
                    'er is bij VrijeTrein geen vrije weg gevonden, terug Retriggeren
                    m_retriggerBlokken.Append(Format(blokNR, "000"))
                Case "c"
                    If ConditieVerwerking(blokNR, locoNR, rwdeel) = False Then 'geen vrije reisweg
                        m_retriggerBlokken.Append(Format(blokNR, "000"))
                    End If
                Case "t"
                    If TerminusVerwerking(blokNR, locoNR, rwdeel) = False Then 'geen vrije reisweg
                        m_retriggerBlokken.Append(Format(blokNR, "000"))
                    End If
                Case "m"
                    If MultiBlokVerwerking(blokNR, locoNR, rwdeel, m_treinen.Item(locoNR).Reisweg) = False Then 'niet alle multiblokken zijn vrij 
                        'Retrigger
                        m_retriggerBlokken.Append(Format(blokNR, "000"))
                    End If
                Case "j", "h"
                    OpmaakBevelen(rwdeel, locoNR, blokNR)
                    m_treinen.Item(locoNR).Hertriggering = True
                Case Else   'gewoon geval voor wissel en blok sturing =============================================================================
                    If ReiswegVRIJ(rwdeel, locoNR) Then 'is reisweg vrij?
                        'Reisweg is vrij loco mag doorrrijden
                        Dim volgBlokNR As Integer = CInt(rwdeel.Substring(rwdeel.IndexOfAny("bB".ToCharArray) + 1, 3))
                        m_blok(volgBlokNR).locoNR = locoNR   'ophalen locoNR
                        m_treinen.Item(locoNR).Hertriggering = True  'acties vanaf Retrigger
                        OpmaakBevelen(rwdeel, locoNR, blokNR)
                        'vrijgave van afremsectie stop als trein rijdt op Rijsectie
                        m_blok(blokNR).locoStop = False  '
                        Exit Sub
                    Else    'reisweg is NIET vrij, blok moet Retriggerd worden
                        m_retriggerBlokken.Append(Format(blokNR, "000"))
                    End If
            End Select
        Catch ex As Exception
            RaiseEvent handlerStatus("Sub Retrigger: exception= " + ex.ToString)
        End Try
    End Sub

    Private Sub Halt()
        Dim strBlokNr As String = m_haltBlokken.ToString.Substring(0, 3)
        Dim blokNR As Integer = CInt(strBlokNr)
        Dim locoNR As Integer = m_blok(blokNR).locoNR
        m_haltBlokken.Remove(0, 3)
        If m_blokkenInDienst.IndexOf(strBlokNr & "(") >= 0 Then
            If Microsoft.VisualBasic.DateAndTime.Timer() >= m_treinen.Item(m_blok(blokNR).locoNR).HalteTijd Then        'tijd verstreken
                'zet blok in Retrigger
                SyncLock syncOK1
                    m_retriggerBlokken.Append(strBlokNr)
                    m_blok(blokNR).statusSynop = "R"
                    RaiseEvent handlerChangeBlok("R", blokNR, locoNR)
                    RaiseEvent handlerHalt(m_haltBlokken.ToString)  'pas halt status aan
                End SyncLock
            Else    'tijd nog niet verstreken
                'shift bloknr
                m_haltBlokken.Append(strBlokNr)
                RaiseEvent handlerHalt(m_haltBlokken.ToString)
            End If
        End If
    End Sub

#End Region

#Region "TREINVERKEER In en UIT dienstname"

    Public Function TreinInDienstName(ByVal lokNR As Integer, ByVal ReisPlan As String) As Boolean
        If instellingen.uitvoeringsmode Then
            Try
                Dim vrijeTrein, LocoDIR As Boolean
                Dim i, j, LocoNR, index, startBlokNR, reiswegNR As Integer
                Dim reisWeg, reiswegen, eindBlok, naarBlokken, strNaarBlok, haltPassagier As String
                Dim alleBlokken As String = ""
                startBlokNR = CInt(ReisPlan.Substring(0, 3))
                If lokNR > 0 Then
                    LocoNR = lokNR
                Else    'blok(blokNR).loconr gebruiken, VrijeTrein start
                    If m_blok(startBlokNR).locoNR = 0 Then

                        RaiseEvent handlerStatus("Het locoNR was 0, voor bloknr=" & startBlokNR.ToString & " --> Combinatie ongeldig, zie na en corrigeer.")
                        Return False
                        Exit Function
                    Else
                        LocoNR = m_blok(startBlokNR).locoNR
                    End If
                End If
                index = m_treinenInDienst.IndexOf(" " & Format(LocoNR, "000"))
                'is locoNR nog vrij?
                If index = -1 Then
                    If ReisPlan.Length = 14 Then   'VrijeTrein behandeling ====================================================================
                        'samenstellen reisplan "iii+00>nnnDvvv" waarin iii=ISblok, +00>=deltasnelheid, nnn=NaarBlok, D=rijrichting, vvv=vanBlok
                        startBlokNR = CInt(ReisPlan.Substring(0, 3))
                        Dim strVanBlok As String = ReisPlan.Substring(11)
                        m_blok(startBlokNR).vanBlokNR = CInt(strVanBlok)
                        'm_blok(startBlokNR).naarBlokNR = CInt(ReisPlan.Substring(7, 3))
                        g_locoData(LocoNR).eindBlokNR = CInt(ReisPlan.Substring(7, 3))
                        'bitwaarde juist zetten= zoeken naar een reisweg waar de Bit kan uit gehaald worden
                        SyncLock syncOK1
                            alleBlokken = g_Relaties(startBlokNR).Substring(g_Relaties(startBlokNR).IndexOf(vbTab) + 1) 'xxx|xxx#xxx....
                            If alleBlokken = "" Then
                                RaiseEvent handlerStatus("TreinIndienstname:Vrije trein, geen gegevens in relatiestabel" _
                                & vbNewLine & "AlleBlokken voor startBlokNR= " & startBlokNR.ToString & " is leeg, controleer relaties.txt")
                                Return False
                                Exit Function
                            End If
                        End SyncLock
                        'selecteer uit de alleBlokken enkel de naarblokken
                        i = alleBlokken.IndexOf(strVanBlok)
                        j = alleBlokken.IndexOf("#")
                        If i >= 0 Then
                            If j > i Then   'xxxxxx....
                                naarBlokken = alleBlokken.Substring(j + 1)
                            Else
                                naarBlokken = alleBlokken.Substring(0, j)
                            End If
                        Else
                            naarBlokken = alleBlokken.Substring(0, j)
                        End If
                        strNaarBlok = naarBlokken.Substring(0, 3)  'xxx
                        For i = 1 To g_MaxBevelen
                            If g_Bevelen(i).Substring(1, 3) = Format(startBlokNR, "000") _
                            AndAlso g_Bevelen(i).Substring(4, 3) = strNaarBlok Then
                                m_blok(startBlokNR).bit = CInt(g_Bevelen(i).Substring(0, 1)) '0=paar, 1=onpaar (rijrichting)
                                Exit For
                            End If
                        Next
                        reisWeg = "" : reiswegen = ""
                        vrijeTrein = True
                        haltPassagier = "j" & instellingen.HaltInStation
                        eindBlok = ReisPlan.Substring(7, 3)
                        If ReisPlan.Substring(10, 1) = "V" Then     'forwards + frontlicht aan
                            LocoDIR = True
                            m_ArrayLOCO(LocoNR).direction = "forwards"
                            m_ArrayLOCO(LocoNR).F0 = "1"
                        Else            'rearwards + frontlicht aan
                            LocoDIR = False
                            m_ArrayLOCO(LocoNR).direction = "rearwards"
                            m_ArrayLOCO(LocoNR).F0 = "1"
                        End If
                    Else    'gewone, geprogrammeerde trein behandeling  - eerste reisweg =====================================================
                        vrijeTrein = False
                        eindBlok = ""   'verplicht
                        reiswegen = ReisPlan.Substring(14)
                        reiswegNR = CInt(ReisPlan.Substring(14, 3))
                        If Not g_ReisWegenArray(reiswegNR) Is Nothing Then
                            reisWeg = g_ReisWegenArray(reiswegNR)
                            haltPassagier = ReisPlan.Substring(6, 4)
                            'blok waar trein van start initialiseren
                            m_blok(startBlokNR).bit = CInt(reisWeg.Substring(3, 1)) '0=paar, 1=onpaar (rijrichting)

                            'initialisatie locoM gegevens
                            m_ArrayLOCO(LocoNR).address = LocoNR.ToString
                            m_ArrayLOCO(LocoNR).speed = "0"
                            Select Case reisWeg.Substring(1, 1)
                                Case "0" 'dir= forwards , F0= off
                                    LocoDIR = True
                                    m_ArrayLOCO(LocoNR).F0 = "0"
                                    m_ArrayLOCO(LocoNR).direction = "forwards"

                                Case "1" 'dir= forwards , F0= on
                                    LocoDIR = True
                                    m_ArrayLOCO(LocoNR).F0 = "1"
                                    m_ArrayLOCO(LocoNR).direction = "forwards"

                                Case "2" 'dir= rearwards , F0= off
                                    LocoDIR = False
                                    m_ArrayLOCO(LocoNR).F0 = "0"
                                    m_ArrayLOCO(LocoNR).direction = "rearwards"
                                    m_blok(startBlokNR).statusK = True

                                Case "3" 'dir= rearwards , F0= on
                                    LocoDIR = False
                                    m_ArrayLOCO(LocoNR).F0 = "1"
                                    m_ArrayLOCO(LocoNR).direction = "rearwards"
                                    m_blok(startBlokNR).statusK = True
                                Case Else
                            End Select
                            'Functies F1-F4
                            m_ArrayLOCO(LocoNR).F1 = CStr(g_locoData(LocoNR).F1)
                            m_ArrayLOCO(LocoNR).F2 = CStr(g_locoData(LocoNR).F2)
                            m_ArrayLOCO(LocoNR).F3 = CStr(g_locoData(LocoNR).F3)
                            m_ArrayLOCO(LocoNR).F4 = CStr(g_locoData(LocoNR).F4)
                            reisWeg = reisWeg.Substring(4)  'verwijder de header
                        Else
                            RaiseEvent handlerStatus("Indienstname: Reisweg is nothing")
                            Return False
                            Exit Function
                        End If
                    End If
                    If IsNothing(m_treinen.Item(LocoNR)) Then
                        MessageBox.Show("Er kunnen geen (nieuwe) treinen in dienst genomen worden tijdens de initialisatiefase en tijdens de actieve (run) fase", _
                        "InDienstName trein", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                        Exit Function
                    End If
                    'juist zetten van de blokgegevens en locogegevens
                    m_blok(startBlokNR).controleBlokNR = startBlokNR
                    m_blok(startBlokNR).statusOpRIJsectie = True
                    m_blok(startBlokNR).statusOpAFREMsectie = True
                    m_treinen.Item(LocoNR).StatusInDienst = True
                    m_treinen.Item(LocoNR).BlokNR = startBlokNR
                    m_treinen.Item(LocoNR).Reisweg = reisWeg
                    m_treinen.Item(LocoNR).Reiswegen = reiswegen
                    m_treinen.Item(LocoNR).ReisPlan = ReisPlan
                    m_treinen.Item(LocoNR).LocoDIR = LocoDIR
                    m_treinen.Item(LocoNR).DeltaSnelheid = CInt(ReisPlan.Substring(3, 3))
                    m_treinen.Item(LocoNR).FrontSein = True
                    m_treinen.Item(LocoNR).HaltPassagiersTrein = haltPassagier
                    m_treinen.Item(LocoNR).EindBlok = eindBlok
                    m_treinen.Item(LocoNR).VrijeTrein = vrijeTrein
                    m_treinen.Item(LocoNR).Snelheid = 0
                    m_treinen.Item(LocoNR).Gestopt = 1       'nodig voor startvertraging
                    m_treinenInDienst = m_treinenInDienst + " " + Format(LocoNR, "000")
                    RaiseEvent handlerTreinen(m_treinenInDienst)
                    If ReisPlan.Length <> 14 Then   'geen VrijeTrein
                        If Not Doorschuiven(LocoNR) Then
                            RaiseEvent handlerStatus("TreinInDienstName: Er kon niet doorgeschoven worden")
                        End If
                    End If
                    'loco moet rechtstreeks aangestuurd worden, niet mogelijk via queue
                    dataL.address = LocoNR.ToString
                    dataL.speed = "0"
                    If g_locoData(LocoNR).standaardRichting = 0 Then    'standaardrijrichting van de loco is normale vooruit (schouw vooraan of nr1 kant)
                        dataL.direction = m_ArrayLOCO(LocoNR).direction
                    Else   'standaardrijrichting is loco normale achteruit
                        If m_ArrayLOCO(LocoNR).direction = "forwards" Then
                            dataL.direction = "rearwards"
                        Else
                            dataL.direction = "forwards"
                        End If
                    End If
                    dataL.F0 = m_ArrayLOCO(LocoNR).F0
                    DSI2.SetLocoM(dataL)

                    If m_treinen.Item(LocoNR).rwDeel <> "" Then
                        RaiseEvent handlerRWdeel(LocoNR, m_treinen.Item(LocoNR).rwDeel & "[" & m_treinen.Item(LocoNR).EindBlok & "]")
                    End If
                    'Blok triggeren
                    m_treinen.Item(LocoNR).Gestopt = 1
                    m_retriggerBlokken.Append(Format(startBlokNR, "000"))
                    RaiseEvent handlerChangeBlok("R", startBlokNR, LocoNR)
                    RaiseEvent handlerRetrigger(m_retriggerBlokken.ToString)
                Else

                    RaiseEvent handlerStatus("Trein met loconr " & LocoNR.ToString & " is reeds in dienst")
                    Return False
                End If
                Return True
            Catch ex As Exception

                RaiseEvent handlerStatus("Fout bij TreinInDienstname,= " & ex.Message)
                Exit Function
            End Try
        Else
            Beep()
            MsgBox("Modelbaan staat in Inactief mode, en kan geen bevelen uitvoeren" + vbNewLine + "Plaats modelbaan eerst in Rij mode", MsgBoxStyle.Critical, "Modelbaan mode status")
        End If
    End Function

    Public Sub TreinUitDienstname(ByVal LocoNR As Integer, ByVal rwDeel As String)
        Dim blokNR As Integer
        Try
            'vertaagtijd om frontsein en andere functies uit te voeren
            If rwDeel.StartsWith("<") Then
                Dim i As Integer = rwDeel.LastIndexOf(">") - 4
                blokNR = CInt(rwDeel.Substring(i, 3))
                m_blok(blokNR).wissels = String.Empty
                m_blok(blokNR).statusK = False
                m_blok(blokNR).statusK2 = False
                m_blok(blokNR).eindeRW = False
                m_blok(blokNR).controleBlokNR = 0
                If m_retriggerBlokken.Length > 0 AndAlso m_retriggerBlokken.ToString.Contains(Format(blokNR, "000")) Then
                    m_retriggerBlokken.Replace(Format(blokNR, "000"), "")
                End If
                'trein vrij zetten
                m_treinen.Item(LocoNR).StatusInDienst = False
                m_treinen.Item(LocoNR).Reisweg = ""
                m_treinen.Item(LocoNR).rwDeel = ""
                m_treinen.Item(LocoNR).Reiswegen = ""
                m_treinen.Item(LocoNR).ReisPlan = ""
                m_treinen.Item(LocoNR).EindBlok = ""
                m_treinenInDienst = m_treinenInDienst.Replace(" " + Format(LocoNR, "000"), "")

            ElseIf rwDeel.Substring(3, 1).ToLower <> "a" Then   'er is nabewerkingen
                Dim t As String = Format(Convert.ToDouble(rwDeel.Substring(1, 2)) + _
                Microsoft.VisualBasic.DateAndTime.Timer(), "00000")
                LocoUitDienstQueue.Enqueue(Format(LocoNR, "000") & t & rwDeel.Substring(3, 1))
                RaiseEvent handlerTreinen(m_treinenInDienst)
            Else
                'trein vrijzetten
                m_treinen.Item(LocoNR).StatusInDienst = False
                m_treinen.Item(LocoNR).Reisweg = ""
                m_treinen.Item(LocoNR).rwDeel = ""
                m_treinen.Item(LocoNR).Reiswegen = ""
                m_treinen.Item(LocoNR).ReisPlan = ""
                m_treinen.Item(LocoNR).EindBlok = ""
                m_treinenInDienst = m_treinenInDienst.Replace(" " + Format(LocoNR, "000"), "")

                'gereserveerde blok vrijzetten
                Dim i As Integer = rwDeel.IndexOf("b", 0) + 1
                blokNR = CInt(rwDeel.Substring(i, 3))
                m_blok(blokNR).status = False
                m_blok(blokNR).wissels = String.Empty
                m_blok(blokNR).vanBlokNR = 0
                m_blok(blokNR).naarBlokNR = 0
                m_blok(blokNR).statusSynop = "V"
                m_blok(blokNR).statusK = False
                m_blok(blokNR).statusK2 = False
                m_blok(blokNR).locoNR = LOKVRIJ
                m_blok(blokNR).eindeRW = False
                m_blok(blokNR).controleBlokNR = 0
                m_blok(blokNR).coBlokken = String.Empty
                m_blok(blokNR).statusNaarRIJsectie = False
            End If
        Catch ex As Exception
            RaiseEvent handlerStatus("Fout in TreinUitDienstName = " & ex.Message)
        End Try
    End Sub

    Public Sub VrijeTreinUitDienstname(ByVal isBlokNR As Integer, ByVal LocoNR As Integer, ByVal rwDeel As String)
        Dim blokNR As Integer
        Dim adres, i As Integer
        Dim strBlokNR As String = String.Empty
        Try
            If rwDeel.StartsWith("<") Then  'vrije trein staat retrigger
                blokNR = CInt(rwDeel.Substring(rwDeel.IndexOf(">", 0) - 3, 3))
                ' 1. trein vrijzetten
                m_treinen.Item(LocoNR).StatusInDienst = False
                m_treinen.Item(LocoNR).Reisweg = ""
                m_treinen.Item(LocoNR).rwDeel = ""
                m_treinen.Item(LocoNR).Reiswegen = ""
                m_treinen.Item(LocoNR).ReisPlan = ""
                m_treinen.Item(LocoNR).EindBlok = ""

                '2. trein verwijderen uit m_treinenInDienst =" xxx yyy zzz" (spatie + loconr)
                m_treinenInDienst = m_treinenInDienst.Remove(m_treinenInDienst.IndexOf(" " + Format(LocoNR, "000")), 4)
                '3. verwijderen uit hertrigger
                For i = 0 To m_retriggerBlokken.Length - 3 Step 3
                    strBlokNR = m_retriggerBlokken.ToString.Substring(i, 3)
                    blokNR = CInt(strBlokNR)
                    If isBlokNR = blokNR Then m_retriggerBlokken.Remove(i, 3) 'verwijder strBlokNR uit stringbuilder
                    Exit For
                Next
            Else
                blokNR = CInt(rwDeel.Substring(rwDeel.IndexOf("b", 0) + 1, 3))
                ' 1.trein vrijzetten
                m_treinen.Item(LocoNR).StatusInDienst = False
                m_treinen.Item(LocoNR).Reisweg = ""
                m_treinen.Item(LocoNR).rwDeel = ""
                m_treinen.Item(LocoNR).Reiswegen = ""
                m_treinen.Item(LocoNR).ReisPlan = ""
                m_treinen.Item(LocoNR).EindBlok = ""
                ' trein verwijderen uit m_treinenInDienst =" xxx yyy zzz" (spatie + loconr)
                m_treinenInDienst = m_treinenInDienst.Remove(m_treinenInDienst.IndexOf(" " + Format(LocoNR, "000")), 4)

                '2. 'wissels vrijzetten
                For i = 0 To rwDeel.Length - 3 Step 4
                    adres = CInt(rwDeel.Substring(i + 1, 3))
                    m_ArrayK83K84(adres).status = False
                    RaiseEvent handlerChangeTurnout("I", rwDeel.Substring(i, 1), adres.ToString)
                Next
                '3. gereserveerde blok vrijzetten
                m_blok(blokNR).wissels = String.Empty
                m_blok(blokNR).status = False
                m_blok(blokNR).vanBlokNR = 0
                m_blok(blokNR).naarBlokNR = 0
                m_blok(blokNR).statusSynop = "V"
                m_blok(blokNR).statusK = False
                m_blok(blokNR).statusK2 = False
                m_blok(blokNR).locoNR = LOKVRIJ
                m_blok(blokNR).afremSnelheid = 0
                m_blok(blokNR).eindeRW = False
                m_blok(blokNR).controleBlokNR = 0
                m_blok(blokNR).coBlokken = String.Empty

                '4. naarBlok verwijderen uit m_blokkenInDienst
                m_blokkenInDienst = m_blokkenInDienst.Replace(Format(blokNR, "000") + "(" + Format(LocoNR, "000") + ")", "")
            End If

        Catch ex As Exception
            RaiseEvent handlerStatus("Fout in VrijeTreinUitDienstName = " & ex.Message)
        End Try
    End Sub

#End Region

#Region "VrijeTrein verwerking"

    Private Sub VrijeTreinVerwerking(ByVal blokNR As Integer, ByVal locoNR As Integer)
        Dim i, n, IsBlokNR, naarBlokNR As Integer
        Dim vanBlokNR, reisPlan, coblokken, rwDeel As String
        Dim blokType As Blok.BlokType = m_blok(blokNR).type

        'handel volgens bloktype dat bereden wordt
        Select Case blokType
            Case Blok.BlokType.Schaduw
                vanBlokNR = Format(m_blok(blokNR).vanBlokNR, "000")
                If CInt(vanBlokNR) = m_blok(blokNR).vanBlokNR Then  'trein komt van de toegangsSchaduwblok
                    coblokken = m_blok(blokNR).coBlokken
                    n = CInt(coblokken.Length / 3)    'aantal co-SBlokken
                    'staat er een trein op een indienstzijnde co-Sblok?
                    For i = 1 To n
                        IsBlokNR = CInt(coblokken.Substring(i * 3 - 3, 3))
                        If m_blok(IsBlokNR).status _
                        AndAlso m_blok(IsBlokNR).locoNR > 0 _
                        AndAlso m_treinen.Item(m_blok(IsBlokNR).locoNR).StatusInDienst = False _
                        AndAlso instellingen.OHblokken.IndexOf("|" & Format(IsBlokNR, "000")) = -1 Then Exit For 'hier staat een trein
                    Next
                    If i <= n Then  'er is trein op een coblok in dienst te nemen
                        'a) neemt de toegekomen loco UIT dienst
                        If Not IsNothing(m_treinen.Item(locoNR)) Then
                            m_treinen.Item(locoNR).rwDeel = "E" & instellingen.SchaduwUitDienstTijd & "Bv00" & instellingen.StopAfremSnelheid   'Einde rwDeel met "B" opmaken
                        End If
                        'b) Neem de coblok in dienst 
                        naarBlokNR = g_locoData(m_blok(IsBlokNR).locoNR).eindBlokNR 'ok
                        If naarBlokNR = 0 Then naarBlokNR = 8 * (CInt(instellingen.s88modulesLinks) + CInt(instellingen.s88modulesMidden) + CInt(instellingen.s88modulesRechts))
                        'en vanblokNR= ISblokNR
                        vanBlokNR = Format(IsBlokNR, "000")
                        'csamenstellen reisplan "iii+00>nnnDvvv" waarin iii=ISblok, +00>=deltasnelheid, nnn=NaarBlok, D=rijrichting, vvv=vanBlok
                        reisPlan = Format(IsBlokNR, "000") + "+00>" + Format(naarBlokNR, "000") + "V" + vanBlokNR
                        TreinInDienstName(0, reisPlan) 'locoNR wordt ingevuld in sub TreinIndienstname 
                    Else          'er is geen co-Bblok trein aanwezig, trein rijdt zelf door
                        If blokType = Blok.BlokType.Pendel Then
                            'trein moet halt, keren en wegrijden
                            rwDeel = m_treinen.Item(locoNR).rwDeel
                            m_treinen.Item(locoNR).rwDeel = "j330k000" & rwDeel 'toevoegen halt en keren
                        End If
                        VrijeTreinProcessing(blokNR, locoNR)
                    End If
                Else    'trein kwam NIET van de toegangsSchaduwblok
                    Beep()
                    RaiseEvent handlerStatus("Fout in VrijeTreinVerwerking: toegekomen trein kwam niet van toegangsblok ( " + m_blok(blokNR).toegangCoBlokNR _
                                             + ", maar van " + vanBlokNR + "  ? - onderzoek")
                End If
            Case Else 'alle andere soort blokken
                VrijeTreinProcessing(blokNR, locoNR)
        End Select
    End Sub

    Private Sub VrijeTreinProcessing(ByVal blokNR As Integer, ByVal locoNR As Integer)
        'gewone processing
        Dim ok As Boolean = False
        Dim i, j, k, x, index, naarBlokNR As Integer
        Dim alleBlokken As String = ""
        Dim s, naarBlokken, strVanBlok, GeldigeNaarBlokken, strNaarBlokNR As String
        Dim strIsBlokNR As String = Format(blokNR, "000")
        Try
            alleBlokken = GetAlleBlokken(strIsBlokNR, index)
            If alleBlokken = "" Then

                RaiseEvent handlerStatus("Fout in sub 'VrijeTreinProcessing' = Alleblokken is LEEG!")
                'stop deze trein
                m_ArrayLOCO(locoNR).address = locoNR.ToString
                m_ArrayLOCO(locoNR).speed = "0"
                queueLocoSTOP.Enqueue(locoNR)
            End If
            strVanBlok = Format(m_blok(blokNR).vanBlokNR, "000") 'Axxx
            If alleBlokken Is Nothing Then  'als er geen alleBlokken zijn kan de VrijeTrein NIET werken!!
                m_ArrayLOCO(locoNR).address = locoNR.ToString
                m_ArrayLOCO(locoNR).speed = "0"
                queueLocoGO.Enqueue(locoNR)
                m_treinen.Item(locoNR).Snelheid = 0

                RaiseEvent handlerStatus(" Er werd geen alleBlokken gevonden voor locoNR: " & locoNR.ToString)
                Haltbevel()
            End If

            'speciale maatregelen voor pendeltreinregeling
            If m_blok(blokNR).type = Blok.BlokType.Pendel And m_treinen.Item(locoNR).Type = Trein.TreinType.Pendel Then  'wijzig de vanBlok
                'kies een fictieve vanBLok uit alleblokken
                i = alleBlokken.IndexOf(strVanBlok)  'vanblok
                j = alleBlokken.IndexOf("#")  'het # scheidingsteken
                If j > i Then
                    strVanBlok = alleBlokken.Substring(j + 1, 4)
                Else
                    strVanBlok = alleBlokken.Substring(0, 4)
                End If
            End If

            'Normale VrijeTrein regeling, selecteer uit alleBlokken enkel de naarblok
            naarBlokken = GetNaarBlokken(alleBlokken, strVanBlok)
            'is de eindblok aanwezig in naarblokken, dan VT eindigen?
            s = naarBlokken.Replace("|", "")
            For x = 0 To s.Length - 2 Step 3
                If s.Substring(x, 3) = m_treinen.Item(locoNR).EindBlok Then
                    'eindblok aanwezig
                    VrijeTreinRWdeelVorming(locoNR, strIsBlokNR, m_treinen.Item(locoNR).EindBlok.Substring(0))
                    m_treinen.Item(locoNR).Reisweg = "E" & instellingen.SchaduwUitDienstTijd & "Uv00" & instellingen.StopAfremSnelheid & "|" & m_treinen.Item(locoNR).rwDeel
                    m_treinen.Item(locoNR).VrijeTrein = False
                    Exit Sub
                End If
            Next

            'Normale VT regeling, bepaal eerst alle GeldigeNaarBlokken
            i = naarBlokken.IndexOf("|")
            If i > 0 Then
                GeldigeNaarBlokken = naarBlokken.Substring(0, i)
            Else
                GeldigeNaarBlokken = naarBlokken
            End If
            For i = 0 To GeldigeNaarBlokken.Length - 3 Step 3
                strNaarBlokNR = GeldigeNaarBlokken.Substring(i, 3)
                'If instellingen.OHblokken.IndexOf("|" + strNaarBlokNR) = -1 Then     'blok is niet in Onderhoud
                naarBlokNR = CInt(strNaarBlokNR)
                VrijeTreinRWdeelVorming(locoNR, strIsBlokNR, strNaarBlokNR)
                'ReiswegVrij testen
                If ReiswegVRIJ(m_treinen.Item(locoNR).rwDeel, locoNR) Then  'Reisweg is VRIJ
                    'testen of er een juist blokType aanwesig is voor de aangekomen trein
                    Select Case m_treinen.Item(locoNR).Type

                        Case Trein.TreinType.Standaard
                            ok = True : Exit For 'geldige keuze

                        Case Trein.TreinType.Personen
                            If m_blok(naarBlokNR).type = Blok.BlokType.Standaard Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Schaduw Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Personen Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Elec Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Perron Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.ElecPerron Then
                                ok = True : Exit For 'geldige keuze
                            End If

                        Case Trein.TreinType.HST
                            If m_blok(naarBlokNR).type = Blok.BlokType.Standaard Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Schaduw Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Personen Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Elec Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.ElecPerron Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Hst Then
                                ok = True : Exit For 'geldige keuze
                            End If

                        Case Trein.TreinType.Goederen
                            If m_blok(naarBlokNR).type = Blok.BlokType.Standaard Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Schaduw Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Goederen Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Elec Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.ElecGoederen Then
                                ok = True : Exit For 'geldige keuze
                            End If

                        Case Trein.TreinType.Pendel
                            If m_blok(naarBlokNR).type = Blok.BlokType.Standaard Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Schaduw Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Personen Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Elec Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Perron Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.ElecPerron Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Pendel Then
                                ok = True : Exit For 'geldige keuze
                            End If

                        Case Trein.TreinType.Elec   'mag op alle electrische blokken rijden
                            If m_blok(naarBlokNR).type = Blok.BlokType.Standaard Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Schaduw Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Elec Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.ElecPerron Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.ElecGoederen Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Personen Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Goederen Then
                                ok = True : Exit For 'geldige keuze
                            End If

                        Case Trein.TreinType.ElecPersonen
                            If m_blok(naarBlokNR).type = Blok.BlokType.Standaard Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Schaduw Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Elec Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.ElecPerron Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Personen Then
                                ok = True : Exit For 'geldige keuze
                            End If

                        Case Trein.TreinType.ElecHST
                            If m_blok(naarBlokNR).type = Blok.BlokType.Standaard Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Schaduw Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Elec Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.ElecPerron Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Hst Then
                                ok = True : Exit For 'geldige keuze
                            End If

                        Case Trein.TreinType.ElecGoederen
                            If m_blok(naarBlokNR).type = Blok.BlokType.Standaard Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Schaduw Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Elec Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Goederen Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.ElecGoederen Then
                                ok = True : Exit For 'geldige keuze
                            End If

                        Case Trein.TreinType.ElecPendel
                            If m_blok(naarBlokNR).type = Blok.BlokType.Standaard Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Schaduw Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Personen Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Elec Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.ElecPerron Then
                                ok = True : Exit For 'geldige keuze
                            ElseIf m_blok(naarBlokNR).type = Blok.BlokType.Pendel Then
                                ok = True : Exit For 'geldige keuze
                            End If
                    End Select
                End If
            Next
            If ok Then
                'verder gaan met doorschuiven - round robin
                i = alleBlokken.IndexOf(naarBlokken)
                j = alleBlokken.IndexOf("#")
                s = naarBlokken.Substring(0, 3)
                k = naarBlokken.IndexOfAny("|#".ToCharArray)
                If k = -1 Then k = naarBlokken.Length
                If j > i Then
                    If k > 4 Then
                        g_Relaties(index) = g_Relaties(index).Substring(0, 4) & naarBlokken.Substring(3, k - 3) & s & naarBlokken.Substring(k) & alleBlokken.Substring(j)
                    End If
                Else
                    If k > 4 Then
                        g_Relaties(index) = g_Relaties(index).Substring(0, 4) & alleBlokken.Substring(0, j + 1) & naarBlokken.Substring(3, k - 3) & s & naarBlokken.Substring(k)
                    End If
                End If
            Else
                m_treinen.Item(locoNR).rwDeel = "< VrijeTrein op blok " & strIsBlokNR & " >"  ' "<" = nodig om het Retriggeren vrijeTrein te herstarten
            End If
            If Not m_treinen.Item(locoNR).rwDeel.StartsWith("<") Then
                RaiseEvent handlerRWdeel(locoNR, m_treinen.Item(locoNR).rwDeel _
                & "[" & m_treinen.Item(locoNR).EindBlok & "]")
            End If
        Catch exc As Exception

            RaiseEvent handlerStatus("Fout in sub 'VrijeTreinverwerking' exc= " & exc.Message)
        End Try
    End Sub

    Private Sub VrijeTreinRWdeelVorming(ByVal locoNR As Integer, ByVal strIsBlokNR As String, _
    ByVal strNaarBlokNR As String)
        Dim i As Integer
        For i = 1 To g_MaxBevelen
            If g_Bevelen(i).Substring(1, 3) = strIsBlokNR _
            AndAlso g_Bevelen(i).Substring(4, 3) = strNaarBlokNR Then
                m_treinen.Item(locoNR).rwDeel = g_Bevelen(i).Substring(8) & "b" & strNaarBlokNR
                Exit For
            End If
        Next
    End Sub

    Private Function GetAlleBlokken(ByVal strIsBlokNR As String, ByRef index As Integer) As String
        Dim alleBlokken As String = ""
        index = CInt(strIsBlokNR)
        If index > 0 Then alleBlokken = g_Relaties(index).Substring(g_Relaties(index).IndexOf(vbTab) + 1) 'AxxxAxxx#Axxx....
        Return alleBlokken
    End Function

    Private Function GetNaarBlokken(ByVal alleblokken As String, ByVal strVanBlok As String) As String
        Dim naarblokken As String = ""
        Dim i, j, k As Integer
        'zoek de strVanBlok
        k = alleblokken.IndexOfAny("|#".ToCharArray)
        j = alleblokken.IndexOf("#")
        For i = 0 To k - 2 Step 3
            If strVanBlok = alleblokken.Substring(i, 3) Then Exit For
        Next
        If i = k Then i = -1 'niet gevonden
        If k < j Then
            For i = k + 1 To j - 2 Step 3
                If strVanBlok = alleblokken.Substring(i, 3) Then Exit For
            Next
            If i = j Then i = -1 'niet gevonden
        End If
        If i >= 0 Then
            If j > i Then
                naarblokken = alleblokken.Substring(j + 1)
            Else
                naarblokken = alleblokken.Substring(0, j)
            End If
        Else
            naarblokken = alleblokken.Substring(0, j)
        End If
        Return naarblokken
    End Function

#End Region

#Region "Queues"

    Private Sub ExecVrijkomenSectie()
        Dim ArrayFT() As Double = CType(queueVrijkomenSectie.Dequeue, Double())

        If Microsoft.VisualBasic.DateAndTime.Timer() > ArrayFT(1) Then     'tijd is verstreken
            Dim s88nr As Integer = CInt(ArrayFT(0))
            s88BitValuesNEW = New BitArray(s88ByteValuesNEW)    'actuele waarden
            If s88BitValuesNEW.Item(s88nr) = False Then    's88nr nog steeds VRIJ
                VrijkomenSectie((s88nr + 2) \ 2, s88nr)
            End If
        Else 'tijd nog niet verstreken
            queueVrijkomenSectie.Enqueue(ArrayFT)     'steek gegeven terug bovenaan in queue
        End If
    End Sub

    Private Sub QueueLocoUitDienstBehandeling()
        Dim s As String = CStr(LocoUitDienstQueue.Dequeue)
        If Microsoft.VisualBasic.DateAndTime.Timer() > CDbl(s.Substring(3, 5)) Then
            Dim blokNR As Integer
            Dim StrLocoNR As String = s.Substring(0, 3)
            Dim locoNR As Integer = CInt(s.Substring(0, 3))
            blokNR = m_treinen.Item(locoNR).BlokNR
            If m_blok(blokNR).type <> Blok.BlokType.Schaduw And g_locoData(locoNR).lengteCode = Trein.TreinLengte.Lang Then
                VrijZettenWissels(blokNR, locoNR)
            End If
            LocoNabehandelingQueue.Enqueue("#" & s.Substring(8, 1).ToUpper & StrLocoNR & Format(blokNR, "000"))
            'bewaren van deze Blok-Loco toestand
            SaveBlokLocoArray()
            'opkuis
            m_treinen.Item(locoNR).StatusInDienst = False
            m_treinen.Item(locoNR).Reisweg = ""
            m_treinen.Item(locoNR).rwDeel = ""
            m_treinen.Item(locoNR).Reiswegen = ""
            m_treinen.Item(locoNR).ReisPlan = ""
            m_treinen.Item(locoNR).EindBlok = ""
            m_treinenInDienst = m_treinenInDienst.Replace(" " + Format(locoNR, "000"), "")
            RaiseEvent handlerTreinen(m_treinenInDienst)
            RaiseEvent handlerRWdeel(CInt(s.Substring(0, 3)), "")
        Else
            LocoUitDienstQueue.Enqueue(s)
        End If
    End Sub

    Public Sub QueueOpkuisTreinBehandeling()
        Try
            Dim locoNR As Integer = CInt(OpkuisTreinQueue.Dequeue)
            m_treinen.Item(locoNR).StatusInDienst = False
            m_treinen.Item(locoNR).Reisweg = ""
            m_treinen.Item(locoNR).rwDeel = ""
            m_treinen.Item(locoNR).Reiswegen = ""
            m_treinen.Item(locoNR).ReisPlan = ""
            m_treinen.Item(locoNR).EindBlok = ""
            If m_treinenInDienst.IndexOf(" " & Format(locoNR, "000")) <> -1 Then
                m_treinenInDienst = m_treinenInDienst.Remove(m_treinenInDienst.IndexOf(" " + Format(locoNR, "000")), 4)
                RaiseEvent handlerTreinen(m_treinenInDienst)
            End If
            RaiseEvent handlerRWdeel(locoNR, "")
        Catch ex As Exception

            RaiseEvent handlerStatus("QueueOpkuisTreinBehandeling meldt: " & ex.Message)
        End Try
    End Sub

#End Region

#Region "Macro Interpreter"

    Private Sub ExecMacroQueueCMD()
        'queue the task
        execMacroNR = CInt(MacroQueue.Dequeue)
        ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf MacroInterpreter))
    End Sub

    Private Sub MacroSelection(ByVal macroNR As Integer)
        execMacroNR = macroNR
        ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf MacroInterpreter))
    End Sub

    Private Sub MacroInterpreter(state As Object)
        'wordt in eigen thread uitgevoerd van ThreadPool
        Dim blokNR As Integer = macroInt(0)
        Dim locoNR As Integer = macroInt(1)
        Dim s88Nr As Integer = macroInt(2)

        Dim i, index, returnIndex, nextlineNR, aantalLijnen, tab1, blNR, lokNR As Integer
        Dim key, reisplan As String
        Dim lijn As String = ""
        Dim param1 As String = ""
        Dim param2 As String = ""
        Dim param3 As String = ""
        Dim strWaarde As String = ""
        Dim vanBlokNR As String = ""
        Dim intWaarde As Integer = 0
        Dim boolWaarde As Boolean = False
        Dim sVar As String = ""    'interne string variable
        Dim iVar As Integer = 0    'interne integer variable
        Dim bVar As Boolean = False
        Dim sVarP(9) As String
        Dim iVarP(9) As Integer
        Dim bVarP(9) As Boolean
        Try
            'JIT-compile: verwijder lijnNR = index van lines(index), alles in lowercase zonder spaties
            Dim program As String = g_macroArray(execMacroNR)
            If IsNothing(program) Then Exit Sub
            Dim lines As String() = program.Split(New [Char]() {Chr(13)})
            'Dim lines() As String = program.Split(vbCr)
            aantalLijnen = lines.GetUpperBound(0)
            For i = 0 To aantalLijnen
                tab1 = lines(i).IndexOf(vbTab) + 1
                lines(i) = lines(i).Substring(tab1).ToLower
                Sleep(1)            'ruimte laten voor ThreadEntry
            Next

            'execute macroprogramma
            Do Until lines(index).StartsWith("end")
                Sleep(1)        'tijd laten voor ThreadEntry
                lijn = lines(index)
                If lijn.StartsWith("'") Then
                    index += 1
                Else
                    tab1 = lijn.IndexOf(vbTab)
                    If tab1 = -1 Then
                        key = lijn
                    Else
                        key = lijn.Substring(0, tab1)
                    End If

                    'keyword verwerking
                    Select Case key
                        Case "nop"
                            index += 1 'volgende lijn in macro

                        Case "end", "stop"
                            Exit Do

                        Case "goto"
                            index = CInt(lijn.Substring(lijn.IndexOf(vbTab) + 1)) - 1

                        Case "gosub"
                            nextlineNR = index + 1
                            index = CInt(lijn.Substring(lijn.IndexOf(vbTab) + 1)) - 1

                        Case "return"
                            index = nextlineNR

                        Case "say"
                            param1 = lijn.Substring(lijn.IndexOf(vbTab) + 1)
                            Select Case param1
                                Case "svar"
                                    param1 = sVar   'geef de waarde van sVar
                                Case "iVar"
                                    param1 = iVar.ToString  'geef de waarde van iVar
                                Case Else
                            End Select
                            'raiseEvent handlerlogData(param1)
                            RaiseEvent handlerLogData(param1)
                            index += 1 'volgende lijn in macro

                        Case "beep"
                            Beep()
                            index += 1 'volgende lijn in macro

                        Case "run"
                            parameters2(lijn, tab1, param1, param2)
                            Select Case param1
                                Case "opafrem"
                                    OpAfremSectie(CInt(param1), CInt(param2))
                                Case "oprij"
                                    OpAfremSectie(CInt(param1), CInt(param2))
                                Case Else
                            End Select
                            index += 1 'volgende lijn in macro

                        Case "ifi"
                            'param1= =>< <= <= <>, param2=value, param3=else index
                            parameters3(lijn, tab1, param1, param2, param3)
                            Select Case param1
                                Case "="
                                    If iVar = CInt(param2) Then index += 1 Else index = CInt(param3) - 1
                                Case "<>"
                                    If iVar <> CInt(param2) Then index += 1 Else index = CInt(param3) - 1
                                Case ">"
                                    If iVar > CInt(param2) Then index += 1 Else index = CInt(param3) - 1
                                Case "<"
                                    If iVar < CInt(param2) Then index += 1 Else index = CInt(param3) - 1
                                Case ">="
                                    If iVar >= CInt(param2) Then index += 1 Else index = CInt(param3) - 1
                                Case "<="
                                    If iVar <= CInt(param2) Then index += 1 Else index = CInt(param3) - 1
                                Case Else
                            End Select

                        Case "ifs"
                            'param1= =>< <= <= <>, param2=value, param3=else index
                            parameters3(lijn, tab1, param1, param2, param3)
                            Select Case param1
                                Case "="
                                    If sVar = param2 Then index += 1 Else index = CInt(param3) - 1
                                Case "<>"
                                    If sVar <> param2 Then index += 1 Else index = CInt(param3) - 1
                                Case ">"
                                    If sVar > param2 Then index += 1 Else index = CInt(param3) - 1
                                Case "<"
                                    If sVar < param2 Then index += 1 Else index = CInt(param3) - 1
                                Case Else
                            End Select

                        Case "ifb"
                            'param1= = , param2=value, param3=else index
                            parameters3(lijn, tab1, param1, param2, param3)
                            Select Case param1
                                Case "="
                                    If bVar = CBool(param2) Then index += 1 Else index = CInt(param3) - 1
                                Case Else
                            End Select

                        Case "runk83", "runk84"
                            If Not IsNothing(m_ArrayK83K84) Then
                                param1 = lijn.Substring(lijn.IndexOf(vbTab) + 1)
                                intWaarde = CInt(param1.Substring(1))
                                'steek bevel in wissel queue formaat= g054
                                queueTurnout.Enqueue(param1)
                                If param1.Substring(0, 1) = "g" Then
                                    m_ArrayK83K84(intWaarde).port = "0"
                                Else
                                    m_ArrayK83K84(intWaarde).port = "1"
                                End If
                                index += 1 'volgende lijn in macro
                            End If

                        Case "trein"    'VrijeTrein in dienst nemen
                            'param1=vanBlokNR, param2=isBlokNR, param3=naarBlokNR => strwaarden!  "027" 
                            parameters3(lijn, tab1, param1, param2, param3)
                            reisplan = param2 & "+00" & "|" & param3 & "1" & param1
                            TreinInDienstName(0, reisplan)   'locoNR wordt ingevuld in sub TreinIndienstname
                            index += 1 'volgende lijn in macro

                        Case "k8055"
                            If instellingen.K8055 = "k8055YES" Then 'enkel als K8055 indienst is uitvoeren
                                'param1= k8055 proc- of subnaam, param2= waarde, param3= diverse
                                parameters3(lijn, tab1, param1, param2, param3)
                                Select Case param1
                                    Case "opendevice"
                                        iVar = OpenDevice(CInt(param2))
                                    Case "closedevice"
                                        CloseDevice()
                                    Case "readanalogchannel"
                                        iVar = ReadAnalogChannel(CInt(param2))
                                    Case "readallanalog"
                                        ReadAllAnalog(CInt(param2), CInt(param3))
                                    Case "outputanalogchannel"
                                        OutputAnalogChannel(CInt(param2), CInt(param3))
                                    Case "outputallanalog"
                                        OutputAllAnalog(CInt(param2), CInt(param3))
                                    Case "clearanalogchannel"
                                        ClearAnalogChannel(CInt(param2))
                                    Case "clearallanalog"
                                        ClearAllAnalog()
                                    Case "setanalogchannel"
                                        SetAnalogChannel(CInt(param2))
                                    Case "setallanalog"
                                        SetAllAnalog()
                                    Case "writealldigital"
                                        WriteAllDigital(CInt(param2))
                                    Case "cleardigitalchannel"
                                        ClearDigitalChannel(CInt(param2))
                                    Case "clearalldigital"
                                        ClearAllDigital()
                                    Case "setdigitalchannel"
                                        SetDigitalChannel(CInt(param2))
                                    Case "setalldigital"
                                        SetAllDigital()
                                    Case "readdigitalchannel"
                                        bVar = ReadDigitalChannel(CInt(param2))
                                    Case "readalldigital"
                                        iVar = ReadAllDigital()
                                    Case "resetcounter"
                                        ResetCounter(CInt(param2))
                                    Case "readcounter"
                                        iVar = ReadCounter(CInt(param2))
                                    Case "setcounterdebouncetime"
                                        SetCounterDebounceTime(CInt(param2), CInt(param3))
                                    Case Else
                                        RaiseEvent handlerLogData("K8055 - foutieve instructie ")
                                End Select
                            End If
                            index += 1 'volgende lijn in macro

                        Case "if"
                            '==================================================================
                            'param1 bxxx of lxxx blokNR of locoNR 
                            'param2 eigenschap = waarde; naast = ook > en < 
                            '       & and | of indien meerdere waarden te onderzoeken
                            'param3 xxx.yyy   xxx=true conditie, yyy=false conditie, gescheiden door . puntteken
                            '       xxx       enkel true conditie, false doet programma eindigen
                            'vb:  if    locoNR=20 & statusK=true | locoNR=34    012.018
                            '==================================================================
                            parameters3(lijn, tab1, param1, param2, param3)

                            'param1 verwerking
                            blNR = CInt(param1.Substring(1, 3))    'blok- of locoNummer

                            'ondervraag blok of loco parameters
                            If param1.StartsWith("b") Then
                                'blok verwerking
                                returnIndex = VerwerkBlok(param2, param3, blNR, blokNR)
                                If returnIndex = -1 Then
                                    index = aantalLijnen  'ga naar laatste lijn
                                Else
                                    index = returnIndex - 1  'volgende lijn in macro
                                    If index > aantalLijnen Then index = aantalLijnen
                                End If
                            Else
                                'loco verwerking
                                returnIndex = CInt(Verwerkloco(param2, param3, blNR, blokNR))
                                If returnIndex = -1 Then
                                    index = aantalLijnen 'ga naar laatste lijn
                                Else
                                    index += 1 'volgende lijn in macro
                                    If index > aantalLijnen Then index = aantalLijnen
                                End If
                            End If

                        Case "setbl"
                            'param1=bloknummer, param2=eigenschap, param3=waarde
                            parameters3(lijn, tab1, param1, param2, param3)
                            blNR = CInt(param1)
                            'zet param3 in het juiste formaat en voer uit 
                            If IsNumberOK(param3) Then
                                intWaarde = CInt(param3)
                            ElseIf param3.ToLower.StartsWith("w") Then
                                boolWaarde = True
                            ElseIf param3.ToLower.StartsWith("f") Then
                                boolWaarde = False
                            End If
                            Select Case param2
                                Case "status"
                                    m_blok(blNR).status = boolWaarde
                                Case "loconr"
                                    m_blok(blNR).locoNR = intWaarde
                                Case "K83K84"
                                    m_blok(blNR).K83K84 = strWaarde
                                Case "maxsnelheid"
                                    m_blok(blNR).maxSnelheid = intWaarde
                                Case "minsnelheid"
                                    m_blok(blNR).minSnelheid = intWaarde
                                Case "bit"
                                    m_blok(blNR).bit = intWaarde
                                Case "wissels"
                                    m_blok(blNR).wissels = param3
                                Case "coBlokken"
                                    m_blok(blNR).coBlokken = param3
                                Case "statusk"
                                    m_blok(blNR).statusK = boolWaarde
                                Case "statusk2"
                                    m_blok(blNR).statusK2 = boolWaarde
                                Case "statusrijsectie"
                                    m_blok(blNR).statusNaarRIJsectie = boolWaarde
                                Case "statusoprijsectie"
                                    m_blok(blNR).statusOpRIJsectie = boolWaarde
                                Case "locostop"
                                    m_blok(blNR).locoStop = boolWaarde
                                Case "einderw"
                                    m_blok(blNR).eindeRW = boolWaarde
                                Case "controlebloknr"
                                    m_blok(blNR).controleBlokNR = intWaarde
                                Case "vanbloknr"
                                    m_blok(blNR).vanBlokNR = intWaarde
                                    m_blok(blNR).afremSnelheid = intWaarde
                                Case "type"
                                    m_blok(blNR).type = CType(intWaarde, Blok.BlokType)
                                Case Else
                            End Select
                            index += 1 'volgende lijn in macro

                        Case "getbl"
                            'param1=bloknummer, param2=eigenschap
                            parameters2(lijn, tab1, param1, param2)
                            blNR = CInt(param1)
                            Select Case param2
                                Case "loconr"
                                    sVar = m_blok(blNR).locoNR.ToString
                                Case "status"
                                    sVar = m_blok(blNR).status.ToString
                                Case "maxsnelheid"
                                    sVar = m_blok(blNR).maxSnelheid.ToString
                                Case "minsnelheid"
                                    sVar = m_blok(blNR).minSnelheid.ToString
                                Case "bit"
                                    sVar = m_blok(blNR).bit.ToString
                                Case "afremsnelheid"
                                    sVar = m_blok(blNR).afremSnelheid.ToString
                                Case "K83K84"
                                    sVar = m_blok(blNR).K83K84
                                Case "type"
                                    sVar = m_blok(blNR).type.ToString
                                Case "wissels"
                                    sVar = m_blok(blNR).wissels
                                Case "coBlokken"
                                    sVar = m_blok(blNR).coBlokken
                                Case "statusk"
                                    sVar = m_blok(blNR).statusK.ToString
                                Case "statusk2"
                                    sVar = m_blok(blNR).statusK2.ToString
                                Case "statusrijsectie"
                                    sVar = m_blok(blNR).statusNaarRIJsectie.ToString
                                Case "statusoprijsectie"
                                    sVar = m_blok(blNR).statusOpRIJsectie.ToString
                                Case "locostop"
                                    sVar = m_blok(blNR).locoStop.ToString
                                Case "einderw"
                                    sVar = m_blok(blNR).eindeRW.ToString
                                Case "controlebloknr"
                                    sVar = m_blok(blNR).controleBlokNR.ToString
                                Case "vanbloknr"
                                    sVar = m_blok(blNR).vanBlokNR.ToString
                                Case Else
                            End Select
                            RaiseEvent handlerLogData("Opgevraagde waarde " & param2 & " van " & param1 & " =" & sVar)
                            index += 1 'volgende lijn in macro

                        Case "setlok"
                            'param1=bloknummer, param2=eigenschap, param3=waarde
                            parameters3(lijn, tab1, param1, param2, param3)
                            lokNR = CInt(param1)
                            'zet param3 in het juiste formaat en voer uit 
                            If IsNumberOK(param3) Then
                                intWaarde = CInt(param3)
                            ElseIf param3.ToLower.StartsWith("w") Then
                                boolWaarde = True
                            ElseIf param3.ToLower.StartsWith("f") Then
                                boolWaarde = False
                            End If
                            SyncLock syncOK1
                                Select Case param2
                                    Case "locoindienst"
                                        g_locoData(lokNR).locoInDienst = intWaarde
                                    Case "soortcode"
                                        g_locoData(lokNR).soortCode = intWaarde
                                    Case "lengtecode"
                                        g_locoData(lokNR).lengteCode = intWaarde
                                    Case "naarBlokNR"
                                        g_locoData(lokNR).eindBlokNR = intWaarde
                                    Case "vanBlokNR"
                                        g_locoData(lokNR).vanBlokNR = intWaarde
                                    Case Else
                                End Select
                            End SyncLock
                            index += 1 'volgende lijn in macro

                        Case "getlok"
                            'param1=bloknummer, param2=eigenschap
                            parameters2(lijn, tab1, param1, param2)
                            lokNR = CInt(param1)
                            SyncLock syncOK1
                                Select Case param2
                                    Case "locoindienst"
                                        sVar = CStr(CInt(g_locoData(lokNR).locoInDienst))
                                    Case "soortcode"
                                        sVar = CStr(CInt(g_locoData(lokNR).soortCode))
                                    Case "lengtecode"
                                        sVar = CStr(CInt(g_locoData(lokNR).lengteCode))
                                    Case "naarblokNR"
                                        sVar = CStr(CInt(g_locoData(lokNR).eindBlokNR))
                                    Case "vanBlokNR"
                                        sVar = CStr(CInt(g_locoData(lokNR).vanBlokNR))
                                    Case Else
                                End Select
                            End SyncLock
                            RaiseEvent handlerLogData("Opgevraagde waarde " & param2 & " van " & param1 & " =" & sVar)
                            index += 1 'volgende lijn in macro

                        Case "ifip"
                            'param1= =>< <= <= <>, param2=value, param3=else index
                            parameters3(lijn, tab1, param1, param2, param3)
                            Select Case param1
                                Case "="
                                    If iVarP(CInt(param2.Substring(0))) = CInt(param2.Substring(2)) Then index += 1 Else index = CInt(param3) - 1
                                Case "<>"
                                    If iVarP(CInt(param2.Substring(0))) <> CInt(param2.Substring(2)) Then index += 1 Else index = CInt(param3) - 1
                                Case ">"
                                    If iVarP(CInt(param2.Substring(0))) > CInt(param2.Substring(2)) Then index += 1 Else index = CInt(param3) - 1
                                Case "<"
                                    If iVarP(CInt(param2.Substring(0))) < CInt(param2.Substring(2)) Then index += 1 Else index = CInt(param3) - 1
                                Case ">="
                                    If iVarP(CInt(param2.Substring(0))) >= CInt(param2.Substring(2)) Then index += 1 Else index = CInt(param3) - 1
                                Case "<="
                                    If iVarP(CInt(param2.Substring(0))) <= CInt(param2.Substring(2)) Then index += 1 Else index = CInt(param3) - 1
                                Case Else
                            End Select

                        Case "ifsp"
                            'param1= =>< <= <= <>, param2=value, param3=else index
                            parameters3(lijn, tab1, param1, param2, param3)
                            Select Case param1
                                Case "="
                                    If sVarP(CInt(param2.Substring(0))) = param2.Substring(2) Then index += 1 Else index = CInt(param3) - 1
                                Case "<>"
                                    If sVarP(CInt(param2.Substring(0))) <> param2.Substring(2) Then index += 1 Else index = CInt(param3) - 1
                                Case ">"
                                    If sVarP(CInt(param2.Substring(0))) > param2.Substring(2) Then index += 1 Else index = CInt(param3) - 1
                                Case "<"
                                    If sVarP(CInt(param2.Substring(0))) < param2.Substring(2) Then index += 1 Else index = CInt(param3) - 1
                                Case Else
                            End Select

                        Case "ifbp"
                            'param1= = , param2=value, param3=else index
                            parameters3(lijn, tab1, param1, param2, param3)
                            Select Case param1
                                Case "="
                                    If bVarP(CInt(param2.Substring(0))) = CBool(param2.Substring(2).ToLower) Then index += 1 Else index = CInt(param3) - 1
                                Case Else
                            End Select

                        Case "sblok"    'schaduwblok
                            'coBlokken verwerking
                            vanBlokNR = m_blok(blokNR).coBlokken.Substring(0, 3)
                            Dim coblokken As String = m_blok(blokNR).coBlokken.Substring(4)
                            iVar = CInt(coblokken.Length / 3)    'aantal co-SBlokken
                            'welke Sblok is vrij
                            For i = 1 To iVar
                                blNR = CInt(coblokken.Substring(i * 3 - 3, 3))
                                If Not m_blok(blNR).status Then Exit For
                            Next
                            If i <= iVar Then  'er is een vrije Sblok
                                reisplan = vanBlokNR & "+00" & "|" & Format(blNR, "000") & "1" & Format(blNR, "000")
                                RaiseEvent handlerStatus("Macro uitgevoerd, zie melding in onderhoudstab")
                                RaiseEvent handlerStatus("Trein  Indienstname=  TreinInDienstname(0, reisplan = " & reisplan & ")" _
                                & vbNewLine & "Trein Uitdienstname= " & m_treinen.Item(m_blok(blokNR).locoNR).rwDeel & vbNewLine)

                                'neem een co-loco indienst
                                TreinInDienstName(0, reisplan)   'locoNR wordt ingevuld in sub TreinIndienstname
                                'neemt de eigen loco uitdienst
                                m_treinen.Item(m_blok(blokNR).locoNR).rwDeel = "E" & instellingen.SchaduwUitDienstTijd & "Uv00" & instellingen.StopAfremSnelheid   'rwDeel opmaken
                            End If
                            index += 1 'volgende lijn in macro

                        Case "titel"
                            'voorlopig niets uitvoeren
                            index += 1
                        Case Else

                            RaiseEvent handlerStatus("Fout in programmalijn " & (index + 1).ToString & "  van marcoNR=" & execMacroNR.ToString)
                            Exit Do
                    End Select
                End If
            Loop
        Catch ex As Exception

            RaiseEvent handlerStatus("Algemene verwerkingsfout in macro Interpreter=  " & ex.Message)
        End Try
    End Sub

    Private Sub parameters2(ByVal lijn As String, ByVal tab1 As Integer, ByRef param1 As String, ByRef param2 As String)
        'bepaal parameters
        Dim tab2 As Integer = lijn.IndexOf(vbTab, tab1 + 1)
        param1 = lijn.Substring(tab1 + 1, tab2 - tab1 - 1)
        param2 = lijn.Substring(tab2 + 1)
    End Sub

    Private Sub parameters3(ByVal lijn As String, ByVal tab1 As Integer, ByRef param1 As String, ByRef param2 As String, ByRef param3 As String)
        'bepaal parameters
        Dim tab2, tab3 As Integer
        tab2 = lijn.IndexOf(vbTab, tab1 + 1)
        If tab2 > 0 Then
            param1 = lijn.Substring(tab1 + 1, tab2 - tab1 - 1)
            tab3 = lijn.IndexOf(vbTab, tab2 + 1)

            If tab3 > 0 Then
                param2 = lijn.Substring(tab2 + 1, tab3 - tab2 - 1)
                param3 = lijn.Substring(tab3 + 1)
            Else
                param2 = lijn.Substring(tab2 + 1)
                param3 = ""
            End If
        Else
            param1 = lijn.Substring(tab1 + 1)
            Exit Sub
        End If
    End Sub

    Private Sub RunMacro(ByVal p1 As Integer)
        Throw New NotImplementedException
    End Sub

    Private Sub Haltbevel()
        Throw New NotImplementedException
    End Sub

#End Region

#Region "Digital-S-Inside-2 and HSI88-USB processing"

#Region "Communication with Digital-S-Inside-2"

    Private Sub WatchDogEvent(ByVal sender As Object, ByVal e As System.EventArgs)
        'send cmd address=253, port=1 to the LDT WatchDog decoder every 3 seconds to keep the rails under current
        dataD.address = "253"
        dataD.port = "1"
        DSI2.SetDecoderM(dataD)
    End Sub

    Private Sub ExecLocoStopCMD()
        'RaiseEvent handlerStatus("STOPcmd queueCMD= " + queueLocoSTOP.Count.ToString)
        Dim str As String = String.Empty
        Try
            Dim locoNR As Integer = CInt(queueLocoSTOP.Dequeue)
            If Not IsNothing(m_treinen.Item(locoNR)) Then
                m_treinen.Item(locoNR).Snelheid = 0
                dataL.address = m_ArrayLOCO(locoNR).address
                dataL.speed = m_ArrayLOCO(locoNR).speed
                dataL.F0 = m_ArrayLOCO(locoNR).F0
                dataL.F1 = m_ArrayLOCO(locoNR).F1
                dataL.F2 = m_ArrayLOCO(locoNR).F2
                dataL.F3 = m_ArrayLOCO(locoNR).F3
                dataL.F4 = m_ArrayLOCO(locoNR).F4
                If g_locoData(locoNR).standaardRichting = 0 Then    'standaardrijrichting is loco normale vooruit
                    dataL.direction = m_ArrayLOCO(locoNR).direction
                Else   'standaardrijrichting is loco normale achteruit
                    If m_ArrayLOCO(locoNR).direction = "forwards" Then
                        dataL.direction = "rearwards"
                    Else
                        dataL.direction = "forwards"
                    End If
                End If
                str = DSI2.SetLocoM(dataL)
                'Debug.WriteLine("STOPcmd speed " + dataL.speed + " > " + dataL.direction)
            End If
        Catch ex As Exception
            Beep()
            RaiseEvent handlerStatus(" fout in ExecLocoStopCMD  ex= " + ex.ToString)
        End Try
    End Sub

    Private Sub ExecLocoGoCMD()
        'RaiseEvent handlerStatus("GOcmd queueCMD= " + queueLocoGO.Count.ToString)
        Dim str As String = String.Empty
        Dim locoNR As Integer = CInt(queueLocoGO.Dequeue)
        Try
            dataL.address = m_ArrayLOCO(locoNR).address
            dataL.speed = m_ArrayLOCO(locoNR).speed
            dataL.F0 = m_ArrayLOCO(locoNR).F0
            dataL.F1 = m_ArrayLOCO(locoNR).F1
            dataL.F2 = m_ArrayLOCO(locoNR).F2
            dataL.F3 = m_ArrayLOCO(locoNR).F3
            dataL.F4 = m_ArrayLOCO(locoNR).F4
            If g_locoData(locoNR).standaardRichting = 0 Then    'standaardrijrichting is loco normale vooruit
                dataL.direction = m_ArrayLOCO(locoNR).direction
            Else   'standaardrijrichting is loco normale achteruit
                If m_ArrayLOCO(locoNR).direction = "forwards" Then
                    dataL.direction = "rearwards"
                Else
                    dataL.direction = "forwards"
                End If
            End If
            If Not IsNothing(m_treinen.Item(locoNR)) Then
                m_treinen.Item(locoNR).Snelheid = CInt(dataL.speed)
                m_treinen.Item(locoNR).FrontSein = CBool(CInt(dataL.F0))
                str = DSI2.SetLocoM(dataL)
                'Debug.WriteLine("Gocmd speed " + dataL.speed + " > " + dataL.direction)
            Else
                Beep()
                RaiseEvent handlerStatus("Loconr= " + locoNR.ToString + " kon niet indienstgenomen worden, pas bij restart programma")
            End If

        Catch ex As Exception
            RaiseEvent handlerStatus(" fout in ExecLocoGoCMD  ex= " + ex.ToString)
        End Try

    End Sub

    Private Sub ExecTurnoutCMD()
        ' RaiseEvent handlerStatus("Turnoutcmd queueCMD= " + queueTurnout.Count.ToString)
        Dim str As String = String.Empty
        dataD = CType(queueTurnout.Dequeue, DecoderM)
        dataD.timeout = instellingen.turnoutPulsTime
        str = DSI2.SetDecoderM(dataD)
        'Debug.WriteLine("wissel of sein= " + dataD.address + "  port= " + dataD.port)
    End Sub

    Private Sub ExecStartVertragingCMD()
        Try
            Dim s As String = CStr(StartVertragingQueue.Dequeue)
            'is tijd verstreken?
            If Microsoft.VisualBasic.DateAndTime.Timer() > CDbl(s.Substring(3, 5)) Then
                Dim locoNR As Integer = CInt(s.Substring(0, 3))
                If Not IsNothing(m_treinen.Item(locoNR)) Then
                    queueLocoGO.Enqueue(locoNR) 'voer loco bevel uit
                End If
            Else        'steek terug in queue
                StartVertragingQueue.Enqueue(s)
            End If
        Catch ex As Exception
            Beep()
            RaiseEvent handlerStatus("Fout in ExecStartVertragingCMD")
        End Try
    End Sub

    Private Sub ExecLocoNabehandelingCMD()
        Try
            Dim s As String = CStr(LocoNabehandelingQueue.Dequeue) '#Uzzzbbb
            Dim locoNR As Integer = CInt(s.Substring(2, 3))     'zzz = locoNR
            Dim blokNR As Integer = CInt(s.Substring(5))        'bbb = blokNR
            Select Case s.Substring(0, 2)
                Case "#U", "#B"
                    m_ArrayLOCO(locoNR).direction = "forwards"
                    m_ArrayLOCO(locoNR).F0 = "0"
                    m_ArrayLOCO(locoNR).F1 = "0"
                    m_ArrayLOCO(locoNR).F2 = "0"
                    m_ArrayLOCO(locoNR).F3 = "0"
                    m_ArrayLOCO(locoNR).F4 = "0"
                Case "#A", "#G"
                    m_ArrayLOCO(locoNR).direction = "rearwards"
                    m_ArrayLOCO(locoNR).F0 = "1"
                    m_ArrayLOCO(locoNR).F1 = "0"
                    m_ArrayLOCO(locoNR).F2 = "0"
                    m_ArrayLOCO(locoNR).F3 = "0"
                    m_ArrayLOCO(locoNR).F4 = "0"

                Case "#N", "#L"
                    m_ArrayLOCO(locoNR).direction = "forwards"
                    m_ArrayLOCO(locoNR).F0 = "1"
                    m_ArrayLOCO(locoNR).F1 = "0"
                    m_ArrayLOCO(locoNR).F2 = "0"
                    m_ArrayLOCO(locoNR).F3 = "0"
                    m_ArrayLOCO(locoNR).F4 = "0"
                Case Else
            End Select
            m_ArrayLOCO(locoNR).address = s.Substring(2, 3)
            m_ArrayLOCO(locoNR).speed = "0"
            queueLocoGO.Enqueue(locoNR)
        Catch ex As Exception
            RaiseEvent handlerStatus("Fout in verwerking EcecLocoNabehandelingCMD")
        End Try
    End Sub

#End Region

#Region " Communication with the HsiUsb and Listener"

    Private Sub HSI_s88Changed(ByVal sender As Object, ByVal e As HsiUsb.ChangedEventArgs) Handles HSI.s88Changed
        'This is the event fired by HsiUsb sub and received in DiCoStation, for s88modules changes
        s88ModulesChanged = e.s88ModulesChanged  'used in Threadloop to trigger evaluate subs
        For i As Integer = 0 To 2   'every event fires 1 module change modNR, Hb, Lb
            s88Buffer(i) = CByte(e.Buffer(i))  'contains the bytevalues for examination and processing
        Next
    End Sub 'Event Listener received from HsiUsb

    Private Sub Initiate_HsiUsb()
        HSI = New HsiUsb()  'make instance of the class HsiUsb


        'Open de device file for reading from s88modules
        HSI.Open()
        RaiseEvent handlerStatus("HSI-88-USB version= " & HSI.Version)

        'execute s-cmd
        Dim s_result() As Byte
        s_result = HSI.InitHsiUsb(CByte(instellingen.s88modulesLinks), CByte(instellingen.s88modulesMidden), CByte(instellingen.s88modulesRechts))

        'put in return buffer s88ByteValues()
        Array.Clear(s88ByteValues, 0, s88ByteValues.Length) 'reset the byteArray
        Dim k As Integer = 0
        Dim j As Integer = s_result(99)     'maxIndex of Last actual byteValue in s_result()
        For i As Integer = 6 To j Step 3
            s88ByteValues(k) = s_result(i + 1)  'lowByte
            s88ByteValues(k + 1) = s_result(i)  'highByte
            k += 2  'incredement of s88ByteValues index
        Next
    End Sub

#End Region

#End Region

#Region "Communication subs for execute cmds from user interface MDIparentfrm and childfrms"

    Public Sub SetLoco(ByVal locoNR As Integer)
        If CInt(m_ArrayLOCO(locoNR).speed) < 2 Then
            queueLocoSTOP.Enqueue(locoNR)
        Else
            queueLocoGO.Enqueue(locoNR)
        End If
    End Sub

    Public Sub SetMacro(ByVal macroNR As Integer)
        MacroQueue.Enqueue(macroNR)
    End Sub

    Public Sub SetTurnout(ByVal data As DecoderM)
        queueTurnout.Enqueue(data)
    End Sub

    Public Sub SetGeneralCMD(ByVal cmd As String)
        queueGeneralCmd.Enqueue(cmd)
    End Sub

    Public Sub SETPowerOnOff(ByVal p As Boolean)
        Dim str As String = String.Empty
        If p Then
            'powerON
            str = DSI2.PowerOn() 'put power on the rails
            RaiseEvent handlerThreadRunning(True)
            RaiseEvent handlerStatus("Digital-S-Inside-2: Power ON command " + str)
            s88ChangesAllowed = True
        Else
            'PowerOFF
            s88ChangesAllowed = False
            RaiseEvent handlerThreadRunning(False)
            str = DSI2.PowerOFF() 'put power on the rails
            RaiseEvent handlerStatus("Digital-S-Inside-2: Power OFF command " + str)
        End If
    End Sub

    Public Sub SetTreinInDienst(ByVal locoNR As Integer, ByVal reisPlan As String)
        TreinInDienstName(locoNR, reisPlan)
    End Sub

    Public Sub SetTreinUitDienst(ByVal locoNR As Integer, ByVal rwDeel As String)
        TreinUitDienstname(locoNR, rwDeel)
    End Sub

    Private Sub SetBlok(ByVal p1 As Integer, ByVal LOKVRIJ As Integer, ByVal p3 As Boolean)
        Throw New NotImplementedException
    End Sub

    Public Sub StartTreinenOnderweg()
        Dim relatie As String = String.Empty
        Dim Reisplan As String = String.Empty
        Dim locoNR, naarBlokNR As Integer
        For i As Integer = 1 To m_maxBlok - 1    'enkel voor treinen welke niet in stations of schaduwstations staan
            If m_blok(i).locoNR > 0 _
            AndAlso (m_blok(i).type = Blok.BlokType.Standaard _
                    OrElse m_blok(i).type = Blok.BlokType.Personen _
                    OrElse m_blok(i).type = Blok.BlokType.Goederen _
                    OrElse m_blok(i).type = Blok.BlokType.Elec _
                    OrElse m_blok(i).type = Blok.BlokType.Hst _
                    ) Then

                locoNR = m_blok(i).locoNR
                If Not m_treinenInDienst.Contains(" " & Format(locoNR, "000")) Then
                    naarBlokNR = m_maxBlok 'laatste blokNR mogelijk bij s88
                    'bepaal reisPlan  "xxxyyyzzzzdvvv" xxx=isBloknr, yyy=deltasnelheid, zzzz naarBloknr, d=richting steeds standaard vooruit =V, vvv=vanblok 
                    Reisplan = Format(i, "000") & "+00|" + Format(naarBlokNR, "000") + "V" + Format(i, "000")
                    'trein in dienst brengen
                    DCS.TreinInDienstName(0, Reisplan)   'locoNR wordt ingevuld in sub TreinIndienstname
                    Sleep(500)      'temporiseren anders loopt het fout
                End If
            End If
        Next
    End Sub

    Public Sub DisplayBlokData(ByVal keuze As Integer, ByVal address As Integer)
        Dim display As String = String.Empty
        Select Case keuze
            Case 1  'bloknr
                SyncLock syncOK
                    display = "Informatie blok " + address.ToString _
                                + vbNewLine + "--------------------------------" _
                                + vbNewLine + "Status= " + m_blok(address).status.ToString _
                                + vbNewLine + "Status NaarRijsectie= " + m_blok(address).statusNaarRIJsectie.ToString _
                                + vbNewLine + "Status OpRijsectie= " + m_blok(address).statusOpRIJsectie.ToString _
                                + vbNewLine + "Status OpAFREMsectie= " + m_blok(address).statusOpAFREMsectie.ToString _
                                + vbNewLine + "bewegingSequentie " + m_blok(address).bewegingSequentie.ToString _
                                + vbNewLine + vbNewLine + "Status K= " + m_blok(address).statusK.ToString _
                                + vbNewLine + "Status K2= " + m_blok(address).statusK2.ToString _
                                + vbNewLine + "Status Synoptiek= " + m_blok(address).statusSynop.ToString _
                                + vbNewLine + "Status PreReserved= " + m_blok(address).statusPreReserved.ToString _
                                + vbNewLine + vbNewLine + "Type= " + m_blok(address).type.ToString _
                                + vbNewLine + "Einde Reisweg= " + m_blok(address).eindeRW.ToString _
                                + vbNewLine + "Bit= " + m_blok(address).bit.ToString _
                                + vbNewLine + "s88nr RIJsectie= " + m_blok(address).s88RijNR.ToString _
                                + vbNewLine + "s88nr AFREMsectie= " + m_blok(address).s88AfremNR.ToString _
                                + vbNewLine + "ControleBlokNR= " + m_blok(address).controleBlokNR.ToString _
                                + vbNewLine + "VanBlokNR= " + m_blok(address).vanBlokNR.ToString _
                                + vbNewLine + "NaarBlokNR= " + m_blok(address).naarBlokNR.ToString _
                                + vbNewLine + vbNewLine + "LocoNR= " + m_blok(address).locoNR.ToString _
                                + vbNewLine + "SynopLocoNR= " + m_blok(address).SynopLocoNR.ToString _
                                + vbNewLine + "LocoStop= " + m_blok(address).locoStop.ToString _
                                + vbNewLine + "LocoFout= " + m_blok(address).locoFout.ToString _
                                + vbNewLine + "Min snelheid= " + m_blok(address).minSnelheid.ToString _
                                + vbNewLine + "Max snelheid= " + m_blok(address).maxSnelheid.ToString _
                                + vbNewLine + "Afremsnelheid= " + m_blok(address).afremSnelheid.ToString _
                                + vbNewLine + vbNewLine + "CoBlokken= " + m_blok(address).coBlokken _
                                + vbNewLine + "ToegangCoBlokNR= " + m_blok(address).toegangCoBlokNR _
                                + vbNewLine + "K83K84= " + m_blok(address).K83K84.ToString _
                                + vbNewLine + "Wisselsein= " + m_blok(address).wisselsein _
                                + vbNewLine + "Wissels= " + m_blok(address).wissels _
                                + vbNewLine + "Cmd in blok= " + m_blok(address).cmdInBlok
                End SyncLock
            Case 2
                SyncLock syncOK2
                    If Not IsNothing(m_treinen.Item(address)) Then
                        display = "Informatie trein " + address.ToString + vbNewLine _
                                    + vbNewLine + "LocoNR= " + m_treinen.Item(address).locoNR.ToString _
                                    + vbNewLine + "Kwam van  blokNR= " + m_treinen.Item(address).VanBlokNR.ToString _
                                    + vbNewLine + "Gaat of staat op blokNR= " + m_treinen.Item(address).BlokNR.ToString _
                                    + vbNewLine + vbNewLine + "Snelheid= " + m_treinen.Item(address).Snelheid.ToString _
                                    + vbNewLine + "Delta snelheid= " + m_treinen.Item(address).DeltaSnelheid.ToString _
                                    + vbNewLine + "StatusInDienst= " + m_treinen.Item(address).StatusInDienst.ToString _
                                    + vbNewLine + "Gestopt= " + m_treinen.Item(address).Gestopt.ToString _
                                    + vbNewLine + "Eindblok= " + m_treinen.Item(address).EindBlok _
                                    + vbNewLine + "Haltetijd= " + m_treinen.Item(address).HalteTijd.ToString _
                                    + vbNewLine + "Startvertraging= " + m_treinen.Item(address).StartVertraging.ToString _
                                    + vbNewLine + vbNewLine + "Vrijetrein= " + m_treinen.Item(address).VrijeTrein.ToString _
                                    + vbNewLine + "Loco richting= " + m_treinen.Item(address).LocoDIR.ToString _
                                    + vbNewLine + "Frontsein= " + m_treinen.Item(address).FrontSein.ToString _
                                    + vbNewLine + "Type= " + m_treinen.Item(address).Type.ToString _
                                    + vbNewLine + vbNewLine + "Reisweg= " + m_treinen.Item(address).Reisweg _
                                    + vbNewLine + "Reisweg deel= " + m_treinen.Item(address).rwDeel _
                                    + vbNewLine + "Reiswegen= " + m_treinen.Item(address).Reiswegen _
                                    + vbNewLine + "Reisplan= " + m_treinen.Item(address).ReisPlan _
                                    + vbNewLine + "Halte passagierstrein= " + m_treinen.Item(address).HaltPassagiersTrein.ToString _
                                    + vbNewLine + "Multiblokfase= " + m_treinen.Item(address).MultiBlokFase.ToString _
                                    + vbNewLine + "MultiblokNR= " + m_treinen.Item(address).MultiBlokNR.ToString _
                                    + vbNewLine + vbNewLine + "Hertriggering= " + m_treinen.Item(address).Hertriggering.ToString _
                                    + vbNewLine + "Halt= " + m_treinen.Item(address).Halt.ToString
                    End If
                End SyncLock
            Case 3
                SyncLock syncOK1
                    display = "Informatie loco " + address.ToString _
                                + vbNewLine + "F1= " + g_locoData(address).F1.ToString _
                                + vbNewLine + "F2= " + g_locoData(address).F2.ToString _
                                + vbNewLine + "F3= " + g_locoData(address).F3.ToString _
                                + vbNewLine + "F4= " + g_locoData(address).F4.ToString _
                                + vbNewLine + "Lengte code= " + g_locoData(address).lengteCode.ToString _
                                + vbNewLine + "Loco in dienst= " + g_locoData(address).locoInDienst.ToString _
                                + vbNewLine + "EindblokNR= " + g_locoData(address).eindBlokNR.ToString _
                                + vbNewLine + "Soort code= " + g_locoData(address).soortCode.ToString _
                                + vbNewLine + "Standaard rijrichting= " + g_locoData(address).standaardRichting.ToString _
                                + vbNewLine + "vanBlokNR= " + g_locoData(address).vanBlokNR.ToString
                End SyncLock
            Case 4
                SyncLock syncOK
                    If address > 0 AndAlso address < m_ArrayK83K84.GetUpperBound(0) Then
                        display = "Informatie wissel " + address.ToString _
                                        + vbNewLine + "Adres= " + m_ArrayK83K84(address).address.ToString _
                                        + vbNewLine + "Status= " + m_ArrayK83K84(address).status.ToString _
                                        + vbNewLine + "StatusSynop= " + m_ArrayK83K84(address).statusSynop.ToString _
                                        + vbNewLine + "Port= " + m_ArrayK83K84(address).port.ToString _
                                        + vbNewLine + "Actief in dienst= " + m_ArrayK83K84(address).active.ToString _
                                        + vbNewLine + "Type= " + m_ArrayK83K84(address).type.ToString _
                                        + vbNewLine + "Timeout= " + m_ArrayK83K84(address).timeout.ToString
                    End If
                End SyncLock

            Case 5
                SyncLock syncOK2
                    If Not IsNothing(m_treinen.Item(address)) Then
                        display = "Informatie reisweg deel " _
                            + vbNewLine + "Trein=  " + m_treinen.Item(address).locoNR.ToString _
                            + vbNewLine + "Blok=   " + m_treinen.Item(address).BlokNR.ToString _
                            + vbNewLine + vbNewLine + "rwDeel= " + m_treinen.Item(address).rwDeel
                    End If
                End SyncLock

            Case Else
        End Select
        RaiseEvent handlerLogData(display)
    End Sub

    Public Sub NieuweLocoInDienstName(ByVal locoNR As Integer, ByVal blokNR As Integer, ByVal paarBit As Boolean)
        Dim strLocoNR As String = ""
        Dim str As String = ""
        If IsNothing(m_treinen.Item(locoNR)) Then
            SetBlokGegevens(blokNR, locoNR, 5)  'initialisatie trein op blok
            If Not IsNothing(s88BitValuesOLD) Then
                If paarBit Then s88BitValuesOLD.Item(blokNR * 2 - 2) = True Else s88BitValuesOLD.Item(blokNR * 2 - 1) = True
            End If
            instellingen.OHblokken = instellingen.OHblokken.Replace("|" + Format(blokNR, "000"), "")
            'creëer de trein
            m_treinen.Add(New Trein(blokNR, CInt(g_BlokLocoArray(blokNR).Substring(4, 3))))
            strLocoNR = g_BlokLocoArray(blokNR).Substring(4, 3)
            'geef de status van het blok door aan de sub BlokStatus(,,) van in dienst zijnde synoptieken op het scherm
            For i As Integer = 0 To uBoundSynopGroup    'voor alle synoptieken
                If synopMenuItemInUse(i) Then synop(i).BlokStatus("B", blokNR, locoNR)
            Next
        End If
    End Sub

#End Region

End Class

