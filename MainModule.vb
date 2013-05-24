'wallar(c) 2010

Imports System.IO
Imports System.Text

Public Module MainModule

#Region "Variables, constants, declarations"

    'constanten
    Public Const MAX As Integer = 512
    Public Const LOKVRIJ As Integer = 0             'Blok vrij Loconummer
    Public Const LOKBEZET As Integer = 256          'Blok Bezet loconummer
    Public Const ANTIDENDERPRESET As Double = 50    'minimum denderfactor
    Public Const ANTIDENDERFREEBLOK As Double = 1   'na vrijkomen blok minimum nadendertijd in seconden
    Public Const WDTRIGGERTIME As Integer = 3000    '3000 is optimum maar 5000 msec is maximum
    Public Const HSIEVENTPRESET As Integer = 8      '8 msec
    Public Const MAXK83K84 As Integer = 254         'maximum toegelaten k83 en k84 adressen
    Public Const MAXS88COLONS As Integer = 4        'aantal colommen per rij ins88contactsFrm
    Public Const MACRODELIMITER As String = "@"      'delemiter om macrorecords van elkaar te scheiden 

    'visible in the whole program
    Public WithEvents DCS As New DiCoStation        'must be visible in MdiParent and MdiChild forms
    Public ProgSettings As ProgramSettings
    Public dataL As LocoM
    Public dataD As DecoderM
    Public s88ChangesAllowed As Boolean             'Gegevensverwerking klaar voor volgende s88 module wijzigingen
    Public ClipboardTrains(80) As LocoM             'gegevens van rijdende treinen tijdelijk bewaren
    Public syncOK As String = "syncOK"
    Public syncOK1 As String = "syncOK1"
    Public syncOK2 As String = "syncOK2"
    Public s88ColonRange As Integer = 4
    Public wisselsInOH As String = String.Empty     '"|xxx|yyy|zzz..."
    Public VerwachteBlok(80) As Integer             'bevat blokNR waar trein op moet komen

    'clipboard
    Public SynopClipboard(,) As CanvasCells
    Public UndoClipboard(,) As CanvasCells
    Public redoClipboard(,) As CanvasCells
    Public ClipboardActive As Boolean

    'speciale  triggers
    Public testRun As Boolean                           'om terzelfdertijd op vaste en portable te kunnen ontwikkelen 
    Public applExit As Boolean

    Public s88contactsView As Boolean                   ' true om actuele toestand van de s88contacten te zien
    Public FoutrijdenDataBtnHerstart As String          'bbblllvvv foutblok loco vanblok
    Public wijzigAppearance As Boolean
    Public s88ByteValuesNEW() As Byte                   'Nieuwe s88module bytes
    Public s88ByteValuesOLD() As Byte                   'Vorige ThreadEntry run waarden
    Public s88BitValuesNEW As Collections.BitArray      'Nieuwe s88module bitwaarden
    Public s88BitValuesOLD As Collections.BitArray      'Vorige ThreadEntry bitwaarden


    'synoptic arrays en variabelen
    Public element(11, 5) As Integer            'bij NewSynoptic keuze van het te tekenen element
    Public xE As Integer                        'bij NewSynoptic keuze van het te tekenen element
    Public yE As Integer                        'bij NewSynoptic keuze van het te tekenen element
    Public blokXY() As BlokkenXY                'used in MainSynopFrm, sub canvas_mouseDown gegevens snel uit gridcel halen
    Public blockLenght As Integer               'bloklengte
    Public TekstLenght As Integer               'tekstlengte
    Public canvasProperty As CanvasProperties   'Used in NewSynopProperiesFrm and NewSynopticFrm
    Public synopMenuItemInUse() As Boolean      'used for indicating that the Menu item is already clicked (single instance)
    Public synopIndexStr() As String                 'used in mousedown event, looks if x,y grid position has a element
    Public strSynopGroup() As String           'verzameling van alle bestaande synoptieken in string uitvoering voor op disk
    Public synopGroup() As canvasGroups        'verzameling van alle bestaande synoptieken voor gebruik in programma
    Public synopGroupActive() As Boolean       'used for dynamic drawing of the synoptics
    Public uBoundSynopGroup As Integer          'Ubound van synopGroup en synopGroupActive()
    Public SynopIsblokNR As Integer             'gebruikt via synoptiek context menu Vrije trein indienstname, blok-loco
    Public SynopIsLocoNR As Integer             'gebruikt via synoptiek context menu Vrije trein indienstname, blok-loco

    'newsynop
    Public synopActiveNR As Integer = -1        ' -1 = synop new

    'Synoptic childforms en helpvariables
    Public synop(0) As MainSynopFrm             'Array om dynamisch MainSynopFrm class variabelen op te maken 
    Public synopName(0) As String

    'Instellingen
    Public instellingen As ProgramSettings
    Public canvasValue As CanvasSetValues

    'gemeenschappelijke programma gegevens
    Public g_ActiveCom As String = ""
    Public g_LocoSpeed As Integer = 7
    Public g_HSILinks As Integer = 0
    Public g_HSIMidden As Integer = 0
    Public g_HSIRechts As Integer = 0

    'Bevelen
    Public g_Bevelen() As String
    Public g_MaxBevelen As Integer
    'transitdata
    Public g_TransitArray(9) As String
    'reisplannen
    Public g_ReisPlannenArray() As String
    Public g_ReisplannenText() As String
    'reiswegen array
    Public g_ReisWegenArray(MAX) As String
    Public g_ConditieRWArray(MAX) As String
    'blokdata
    Public g_s88s() As Byte
    'LocoData
    Public g_locoData(80) As LocoData 'maximum 80 loco's !!!!
    'BlokLoc
    Public g_BlokLocoArray(248) As String
    Public g_MaxBLA As Integer
    'BlokRelatie
    Public g_Relaties(MAX) As String
    Public g_MaxRelaties As Integer
    'MacroInterpreter
    Public g_macroArray(MAX) As String
    'tracks of stukken spoor tussen rij en Afrem of in rangeerplaatsen.
    Public g_TrackArray(MAX) As Boolean
    's88MacroNRs
    Public g_s88MacroNRS(MAX, 1) As Integer
    Public g_MaxS88MacroNRS As Integer

    'Actuele opvolging trein verkeer
    Public m_blok() As Blok                      'blokgegevens
    Public m_ArrayK83K84(MAXK83K84) As DecoderM  'wissel/sein gegevens
    Public m_ArrayLOCO(80) As LocoM              'loco gegevens
    Public m_treinen As New Treinen              'collection dictionary

    'events from HSI-USB
    Public Event MessageChanged(ByVal sender As Object, ByVal e As DCS_EventArgs)

    'allerlei  public class variables for Class DiCoStation2
    Public m_LocoInGebruik As String             ' "002 016 054 078" loco's in gebruik
    Public m_Modules As Integer                       'aantal modules in gebruik
    Public m_maxBlok As Integer                  'maximum aantal blokken
    Public m_MaxS88nrs As Integer                'hoogste s88nr van de laatste s88module
    Public m_MaxWissels As Integer              ' Ubound van wissels array
    Public m_MaxK83K84 As Integer
    Public m_blokkenInDienst As String = ""     'info voor op hoofdscherm
    Public m_treinenInDienst As String = ""     'info voor hoofdscherm
    Public m_plcCmds As Integer
    Public m_FoutWisselstraat As String
    Public m_retriggerBlokken As New StringBuilder    'blokken in Retriggering
    Public m_haltBlokken As New StringBuilder          'blokken in halt

#End Region

#Region "Digital-S-Inside-2 Structures en eventArgs"

    Public Structure LocoData
        Public locoInDienst As Integer
        Public standaardRichting As Integer
        Public soortCode As Integer
        Public lengteCode As Integer
        Public vanBlokNR As Integer
        Public eindBlokNR As Integer
        Public F1 As Integer
        Public F2 As Integer
        Public F3 As Integer
        Public F4 As Integer
    End Structure           'voor treinverkeerregeling

    Public Structure LocoM
        Public address As String
        Public speed As String
        Public direction As String
        Public F0 As String
        Public F1 As String
        Public F2 As String
        Public F3 As String
        Public F4 As String
    End Structure   'DSI2

    Public Structure DecoderM
        Public active As String
        Public address As String
        Public port As String
        Public timeout As String
        Public status As Boolean
        Public statusSynop As String
        Public type As String
    End Structure   'DSI2

    Public Class DCS_EventArgs
        Inherits EventArgs

        Private _strMessage As String
        Public Property STR_message() As String
            Get
                Return _strMessage
            End Get
            Set(ByVal value As String)
                _strMessage = value
            End Set
        End Property

        Private _intMessage As Integer = 0
        Public Property Int_Message() As Integer
            Get
                Return _intMessage
            End Get
            Set(ByVal value As Integer)
                _intMessage = value
            End Set
        End Property
    End Class

#End Region

#Region " Drawing enums, structures"

    Public Enum CanvasAppearances
        Fitscreen = 1
        Standaard = 2
        Maximized = 3
    End Enum

    Public Enum EditModes
        selekt = 0
        Delete = 1
        Cut = 2
        Copy = 3
        Paste = 4
        Export = 5
        Import = 6
    End Enum

    Public Structure SelectXY
        Public x As Integer
        Public y As Integer
    End Structure

    Public Structure BlokkenXY
        Public blokNR As Integer
        Public locoNR As Integer
        Public elementNR As Integer
        Public x As Integer
        Public y As Integer
    End Structure

    Public Structure canvasGroups
        Public canvas() As CanvasCells           'de synoptiek
        Public Prop As CanvasProperties         'synoptiek eigenschappen
    End Structure

    Public Structure CanvasProperties
        Public synopNR As Integer
        Public gridStep As Integer
        Public labelText As String
        Public tittle As String
        Public appearance As CanvasAppearances
    End Structure

    Public Structure CanvasCells    'for run synop gebruiken
        Public posX As Integer
        Public posY As Integer
        Public active As Boolean
        Public gridStep As Integer
        Public elementNR As Integer
        Public itemNR As String
        Public locoNR As String
        Public elementLenght As Integer
        Public status As String
        Public direction As String
        Public strInfo As String
        Public notes As String
    End Structure

    Public Structure CanvasSetValues
        Public setGridUnit As Integer
        Public setGridText As Integer
        Public setPenWidth As Integer
    End Structure

#End Region

#Region "Structures and  classes: Programsettings, Blok,  Trein, Class Treinen"

    Public Structure ProgramSettings
        Public LeaveHSI88USB As String
        Public s88modulesLinks As String
        Public s88modulesMidden As String
        Public s88modulesRechts As String
        Public startVertraging As String
        Public GoederentreinSnelheidsverlaging As String
        Public StopAfremSnelheid As String
        Public MacroStart As String
        Public MacroStop As String
        Public MacroFout As String
        Public HaltInStation As String
        Public OHblokken As String
        Public K8055 As String
        Public k8055B0 As String
        Public k8055B1 As String
        Public k8055B2 As String
        Public k8055B3 As String
        Public SchaduwUitDienstTijd As String
        Public turnoutPulsTime As String
        Public tempoRegeling As String
        Public PenWidth As String
        Public ColorVrij As String
        Public ColorBezet As String
        Public ColorAfremBezet As String
        Public ColorGereserveerd As String
        Public ColorPreReserved As String
        Public ColorVergrendeld As String
        Public ColorOnderhoud As String
        Public ColorSpook As String
        Public colorBlockPen As String
        Public colorHalt As String
        Public colorPassief As String
        Public ColorInit As String
        Public ColorInitBlok As String
        Public ColorGridLijnen As String
        Public ColorAchtergrondSynop As String
        Public ColorAchtergrondElement As String
        Public ColorHandbediening As String
        Public ColorRetrigger As String
        Public ColorTextBack As String
        Public ColorLocatieTrein As String
        Public StatusColorTurnout As String
        Public s88ColonRange As String
        Public gridlijnenTonen As Boolean
        Public showRailElements As Boolean
        Public K83K84detectie As Boolean
        Public uitvoeringsmode As Boolean
    End Structure

    Public Structure Blok
        Friend status As Boolean                'false=vrij ; true=bezet
        Friend statusK As Boolean               'statuskeren 
        Friend statusK2 As Boolean              'statuskeren2
        Friend statusNaarRIJsectie As Boolean   'NaarRIJsectie: true= trein verwacht, false geen trein verwacht op de sectie
        Friend statusOpRIJsectie As Boolean     'True: zolang trein RijsectieBezet, false= trein weg 
        Friend statusOpAFREMsectie As Boolean   'True: zolang als een trein op deze sectie aanwezig is
        Friend statusPreReserved As Boolean     'True indien een wisselblokbehandeling actief is
        Friend statusSynop As String            'Ohblok terug in dienst na foutrijden
        Friend bewegingSequentie As Integer     'bloksesquentie: 0= geen trein beweging, 1= rijsectie bezet, 2= afremsectie bezet
        Friend locoFout As Integer              '0 en 1 = geen actie, >1 actie
        Friend locoStop As Boolean              'Loco op afresectie moet stoppen
        Friend eindeRW As Boolean               'einde reisweg bij verlaten IB
        Friend bit As Integer                   '1=onpaar, 0=paar
        Friend s88RijNR As Integer              's88nr van de Rijsectie
        Friend s88AfremNR As Integer            's88nr van de Afremsectie
        Friend locoNR As Integer                'nummer loco welke blok bezet
        Friend SynopLocoNR As Integer          'laatst aanwezig locoNR bij toekenning ControleBlokNR 
        Friend controleBlokNR As Integer        'de verwachte bloknr voor controle
        Friend vanBlokNR As Integer             'blok welke trein verliet 
        Friend naarBlokNR As Integer             'blok welke trein verliet 
        Friend afremSnelheid As Integer         'te nemen snelheid op afremsectie
        Friend maxSnelheid As Integer           'maximum doorrij bloksnelheid
        Friend minSnelheid As Integer           'minimum doorrij bloksnelheid
        Friend wissels As String                'Vrij te zetten overgereden wissels
        Friend type As BlokType                 'type van blok
        Friend K83K84 As String                 'txxxyyy of dxxxyyy soortsein en volgens bit richting
        Friend wisselsein As String             'bevelcmd van een wisselsein op de naarBlok
        Friend coBlokken As String              'van|coBlokken reeks van een  TrekDuw- of schaduwstation
        Friend toegangCoBlokNR As String        'van|coBlokken reeks van een  TrekDuw- of schaduwstation
        Friend cmdInBlok As String          'Blok bevat een wissel indien waarde >0    waarde= k83adres

        Public Sub init(ByVal s As String)
            Try
                Dim i, d1, d2, d3, d4, d5 As Integer
                Dim str As String = ""
                status = False                  'blokbezet
                statusNaarRIJsectie = False         'Rijsectiebezet
                statusK = False                 'keren
                statusK2 = False                'deel 2 van keren
                eindeRW = False                 'eindereisweg
                locoStop = False
                statusSynop = "*"
                locoFout = 0
                locoNR = 0
                controleBlokNR = 0
                afremSnelheid = 0
                bit = 0
                wissels = ""                    'bezette wissels tijdens treinrit
                d1 = s.IndexOf(vbTab) + 1
                d2 = s.IndexOf(vbTab, d1) + 1
                d3 = s.IndexOf(vbTab, d2) + 1
                d4 = s.IndexOf(vbTab, d3) + 1
                d5 = s.IndexOf(vbTab, d4) + 1
                maxSnelheid = CInt(s.Substring(d1, 2))          'maximum snelheid
                minSnelheid = CInt(s.Substring(d1 + 2, 2))      'minimum snelheid
               Select Case s.Substring(d2, 1)                  'blokType
                    Case "0"
                        type = BlokType.Standaard
                    Case "1"
                        type = BlokType.Personen
                    Case "2"
                        type = BlokType.Goederen
                    Case "3"
                        type = BlokType.Schaduw
                    Case "4"
                        type = BlokType.Pendel
                    Case "5"
                        type = BlokType.Elec
                    Case "6"
                        type = BlokType.Hst
                    Case "7"
                        type = BlokType.Perron
                    Case "8"
                        type = BlokType.ElecPerron
                    Case "9"
                        type = BlokType.ElecGoederen
                    Case Else
                        type = BlokType.Standaard
                End Select
                K83K84 = s.Substring(d3, d4 - d3 - 1)               'seinen of andere k83K84 sturingen
                cmdInBlok = s.Substring(d4, d5 - d4 - 1)            'wisseladres in Blok
                str = s.Substring(d5)                               'schaduwblokken
                i = str.IndexOf("|")                                'formaat  xxxyyyzzz...|ttt   of xxxyyyzzz...
                If i <> -1 Then
                    coBlokken = str.Substring(0, i)                 'xxxyyyzzz
                    toegangCoBlokNR = str.Substring(i + 1)          'ttt
                Else
                    coBlokken = str
                End If
            Catch ex As Exception

                MessageBox.Show("Fout in bestand: blokdata.txt, struktuur niet OK" _
                & vbNewLine & "Herstart programma en zie bestand na via editor [F5]!", "Initialisatie blokken", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End
            End Try
        End Sub

        Public Enum BlokType
            Standaard = 0
            Personen = 1
            Goederen = 2
            Schaduw = 3
            Pendel = 4
            Elec = 5
            Hst = 6
            Perron = 7
            ElecPerron = 8
            ElecGoederen = 9
        End Enum

    End Structure

    Public Class Trein

        Private _statusInDienst As Boolean  'Trein gereserveerd om te rijden
        Private _LocoNR As Integer          'adres van de trekkende loco
        Private _ReisWeg As String          'gehele reisweg
        Private _RWdeel As String           'actief deel reisweg
        Private _Reiswegen As String        'gehele Reiswegen
        Private _ReisPlan As String         'gehele Reisplan
        Private _DeltaSnelheid As Integer   'snelheidsvermindering of vermeerdering
        Private _Frontsein As Boolean       'frontsein  0=uit  1=aan
        Private _halteTijd As Double        'stilstand wachttijd van een trein op een blok
        Private _Snelheid As Integer        'actuele snelheid van de trein
        Private _Gestopt As Integer         'Gestopt voor sein onveilig
        Private _StartVertraging As Integer 'Seconden vertraging bij omwchakeling sein op veilig
        Private _multiBlokFase As Boolean   'true=start multiblok in OpmaakBevelen, False= tijdens de loop van multiblok
        Private _multiBlokNR As Integer     'laatste blokNR in een multifase rit
        Private _Hertriggering As Boolean   'true= Opmaak bevelen van af hertrigger aangestuurd
        Private _Halt As Boolean            'true= Opmaak bevelen van af hertrigger aangestuurd
        Private _vanBlokNR As Integer       'blokNR waar trein vandaan kwam
        Private _blokNR As Integer          'rijdend= naarBlokNR anders isBlokNR
        Private _HaltPassagier As String    'hxxx, waar xxx = haltttijd in seconden van 000 tot 999
        Private _LocoDIR As Boolean = True  'de rijrichting van de loco  true= vooruit
        Private _VrijeTrein As Boolean      'true= Trein zonder gedefinieerde reisweg, false= gewone reisweg
        Private _EindBlok As String         'geeft de einde reisweg blok aan voor vtijeTrein
        Private _type As TreinType          'enum van treintypes


        Public Sub New(ByVal blokNR As Integer, ByVal locoNR As Integer)
            _blokNR = blokNR
            _LocoNR = locoNR
            _statusInDienst = False
            _ReisWeg = ""
            _RWdeel = ""
            _Reiswegen = ""
            _ReisPlan = ""
            _DeltaSnelheid = 0
            _Frontsein = False
            _halteTijd = CInt(instellingen.HaltInStation)
            _Snelheid = 0
            _Gestopt = 0
            _StartVertraging = CInt(instellingen.startVertraging)
            _multiBlokFase = False
            _multiBlokNR = 0
            _Hertriggering = False
            _Halt = False
            _vanBlokNR = 0
            _HaltPassagier = instellingen.HaltInStation
            _LocoDIR = True
            _VrijeTrein = False
            _EindBlok = "000"
            _type = CType(g_locoData(locoNR).SoortCode, TreinType)
        End Sub

        Public ReadOnly Property locoNR() As Integer
            Get
                Return _LocoNR
            End Get
        End Property

        Public Property StatusInDienst() As Boolean
            Get
                Return _statusInDienst
            End Get
            Set(ByVal value As Boolean)
                _statusInDienst = value
            End Set
        End Property

        Public Property VrijeTrein() As Boolean
            Get
                Return _VrijeTrein
            End Get
            Set(ByVal value As Boolean)
                _VrijeTrein = value
            End Set
        End Property

        Public Property Reiswegen() As String
            Get
                Return _Reiswegen
            End Get
            Set(ByVal value As String)
                _Reiswegen = value
            End Set
        End Property

        Public Property ReisPlan() As String
            Get
                Return _ReisPlan
            End Get
            Set(ByVal value As String)
                _ReisPlan = value
            End Set
        End Property

        Public Property Reisweg() As String
            Get
                Return _ReisWeg
            End Get
            Set(ByVal value As String)
                _ReisWeg = value
            End Set
        End Property

        Public Property rwDeel() As String
            Get
                Return _RWdeel
            End Get
            Set(ByVal value As String)
                _RWdeel = value
            End Set
        End Property

        Public Property VanBlokNR() As Integer
            Get
                Return _vanBlokNR
            End Get
            Set(ByVal value As Integer)
                _vanBlokNR = value
            End Set
        End Property

        Public Property BlokNR() As Integer
            Get
                Return _blokNR
            End Get
            Set(ByVal value As Integer)
                _blokNR = value
            End Set
        End Property

        Public Property EindBlok() As String
            Get
                Return _EindBlok
            End Get
            Set(ByVal value As String)
                _EindBlok = value
            End Set
        End Property

        Public Property StartVertraging() As Integer
            Get
                Return _StartVertraging
            End Get
            Set(ByVal value As Integer)
                _StartVertraging = value
            End Set
        End Property

        Public Property Snelheid() As Integer
            Get
                Return _Snelheid
            End Get
            Set(ByVal value As Integer)
                _Snelheid = value
            End Set
        End Property

        Public Property MultiBlokFase() As Boolean
            Get
                Return _multiBlokFase
            End Get
            Set(ByVal value As Boolean)
                _multiBlokFase = value
            End Set
        End Property

        Public Property MultiBlokNR() As Integer
            Get
                Return _multiBlokNR
            End Get
            Set(ByVal value As Integer)
                _multiBlokNR = value
            End Set
        End Property

        Public Property Hertriggering() As Boolean
            Get
                Return _Hertriggering
            End Get
            Set(ByVal value As Boolean)
                _Hertriggering = value
            End Set
        End Property

        Public Property Halt() As Boolean
            Get
                Return _Halt
            End Get
            Set(ByVal value As Boolean)
                _Halt = value
            End Set
        End Property

        Public Property Gestopt() As Integer
            Get
                Return _Gestopt
            End Get
            Set(ByVal value As Integer)
                _Gestopt = value
            End Set
        End Property

        Public Property DeltaSnelheid() As Integer
            Get
                Return _DeltaSnelheid
            End Get
            Set(ByVal value As Integer)
                _DeltaSnelheid = value
            End Set

        End Property

        Public Property HaltPassagiersTrein() As String
            Get
                Return _HaltPassagier
            End Get
            Set(ByVal value As String)
                _HaltPassagier = value
            End Set
        End Property

        Public Property FrontSein() As Boolean
            Get
                Return _Frontsein
            End Get
            Set(ByVal value As Boolean)
                _Frontsein = value
            End Set
        End Property

        Public Property HalteTijd() As Double
            Get
                Return _halteTijd
            End Get
            Set(ByVal value As Double)
                _halteTijd = value
            End Set
        End Property

        Public Property Type() As TreinType
            Get
                Return _type
            End Get
            Set(ByVal value As TreinType)
                _type = value
            End Set
        End Property

        Public Property LocoDIR() As Boolean
            Get
                Return _LocoDIR
            End Get
            Set(ByVal value As Boolean)
                _LocoDIR = value
            End Set
        End Property

        Public Structure TreinData
            Friend vrijeTrein As Boolean
            Friend frontSein As Boolean
            Friend startBlokNR As Integer
            Friend deltaSnelheid As Integer
            Friend perronHalt As String
            Friend grensPlanBlok As String
            Friend reisWegen As String
        End Structure

        Public Enum TreinType
            Standaard = 0
            Personen = 1
            HST = 2
            Goederen = 3
            Pendel = 4
            Elec = 5
            ElecPersonen = 6
            ElecHST = 7
            ElecGoederen = 8
            ElecPendel = 9
        End Enum

        Public Enum TreinLengte
            Standaard = 0
            Kort = 1
            Lang = 2
        End Enum

    End Class

    Public Class Treinen
        Inherits System.Collections.DictionaryBase

        Public Sub Add(ByVal Trein As Trein)
            Dictionary.Add(Trein.locoNR, Trein)
        End Sub

        Public Sub Remove(ByVal LocoNR As Integer)
            Dictionary.Remove(LocoNR)
        End Sub

        Public Sub RemoveAllTrains()
            Dictionary.Clear()
        End Sub

        Default Public ReadOnly Property Item(ByVal LocoNR As Integer) As Trein
            Get
                Return CType(Dictionary.Item(LocoNR), Trein)
            End Get
        End Property

    End Class

#End Region

#Region "Properties, Sub and Functions"

    Public Function IsNumberOK(ByVal s As String) As Boolean
        If s.IndexOfAny("&@#'(§^!{|})-[]$*:/;.,?´`°¨".ToCharArray) >= 0 Then
            Return False
            Exit Function
        ElseIf IsNumeric(s) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function IsStrSpeedOK(ByVal str As String) As Boolean
        If str.Length = 0 Then
            Return False
        Else
            For i As Integer = 0 To str.Length - 1
                If Not Char.IsNumber(str, i) Then
                    Return False
                    Exit Function
                End If
            Next
            If CInt(str) >= 0 And CInt(str) < 15 Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Function IsStrK8xOK(ByVal str As String) As Boolean
        If str.Length = 0 Then
            Return False
        Else
            For i As Integer = 0 To str.Length - 1
                If Not Char.IsNumber(str, i) Then
                    Return False
                    Exit Function
                End If
            Next
            If CInt(str) > 0 And CInt(str) < 257 Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Function IsDecoderNrOk(ByVal s As String) As Boolean
        If s.IndexOfAny("&@#'(§^!{|})-[]$*:/;.,?´`°¨".ToCharArray) >= 0 Then
            Return False
            Exit Function
        ElseIf IsNumeric(s) Then
            If CInt(s) > 0 AndAlso CInt(s) < 255 Then
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Public Function IsLocoAddressOk(ByVal s As String) As Boolean
        If s.IndexOfAny("&@#'(§^!{|})-[]$*:/;.,?´`°¨".ToCharArray) >= 0 Then
            Return False
            Exit Function
        ElseIf IsNumeric(s) Then
            If CInt(s) > 0 AndAlso CInt(s) < 81 Then
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Public Function IsBlokAddressOk(ByVal s As String, ByVal maxBlok As Integer) As Boolean
        If s.IndexOfAny("&@#'(§^!{|})-[]$*:/;.,?´`°¨".ToCharArray) >= 0 Then
            Return False
            Exit Function
        ElseIf IsNumeric(s) Then
            If CInt(s) > 0 AndAlso CInt(s) < maxBlok + 1 Then
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Public Function IsLocoSpeedOk(ByVal s As String) As Boolean
        If s.IndexOfAny("&@#'(§^!{|})-[]$*:/;.,?´`°¨".ToCharArray) >= 0 Then
            Return False
            Exit Function
        ElseIf IsNumeric(s) Then
            If CInt(s) >= 0 AndAlso CInt(s) < 15 Then
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Public Function ParsedXlmString(ByVal str As String) As String
        Dim strParsed As String = String.Empty
        Dim iB, iE As Integer
        Do
            iE = str.IndexOf(">", iB)
            If iE = -1 Then Exit Do
            strParsed += str.Substring(iB, iE - iB + 1) + Environment.NewLine
            iB = iE + 1
        Loop
        Return strParsed
        'Debug.WriteLine(strParsed)
    End Function

    Public ReadOnly Property DataDir() As String
        Get
            Return My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\WallarDCS"
        End Get
    End Property

#End Region

#Region "Load modelrailway data"

    Public Sub LoadBevelen()
        Dim i As Integer
        Dim data As String
        Dim myStreamReader As StreamReader
        Try
            myStreamReader = File.OpenText(DataDir & "\Bevelen.txt")
            data = myStreamReader.ReadLine
            g_MaxBevelen = CInt(data)
            ReDim g_Bevelen(g_MaxBevelen)
            For i = 1 To CInt(data)
                data = myStreamReader.ReadLine
                g_Bevelen(i) = data
            Next
            myStreamReader.Close()
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: : Bevelen.txt " & exc.Message)
        End Try
    End Sub

    Public Sub LoadRelaties()
        Dim i As Integer
        Dim data As String
        Dim myStreamReader As StreamReader
        Try
            myStreamReader = File.OpenText(DataDir & "\Relaties.txt")
            data = myStreamReader.ReadLine
            ReDim g_Relaties(CInt(data))
            For i = 1 To CInt(data)
                data = myStreamReader.ReadLine
                g_Relaties(i) = data
            Next
            myStreamReader.Close()
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: : Relaties.txt " & exc.Message)
        End Try
    End Sub

    Public Sub loadBlokData()
        Dim data As String
        Dim myStreamReader As StreamReader
        Try
            myStreamReader = File.OpenText(DataDir & "\BlokData.txt")
            For i As Integer = 1 To m_maxBlok
                data = myStreamReader.ReadLine
                m_blok(i).init(data)
            Next
            myStreamReader.Close()
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: : BlokData.txt " & exc.Message)
        End Try

    End Sub

    Public Sub LoadLocoData()
        Dim myStreamReader As StreamReader
        Dim s, data As String
        Dim i, locoNR, displacement As Integer
        Try
            myStreamReader = File.OpenText(DataDir & "\LocoData.txt")
            For i = 1 To 80
                displacement = 0
                data = myStreamReader.ReadLine
                locoNR = CInt(data.Substring(0, data.IndexOf(vbTab)))
                displacement = data.IndexOf(vbTab, displacement) + 1
                g_locoData(locoNR).LocoInDienst = CInt(data.Substring(displacement, 1))
                displacement = data.IndexOf(vbTab, displacement) + 1
                g_locoData(locoNR).StandaardRichting = CInt(data.Substring(displacement, 1))
                displacement = data.IndexOf(vbTab, displacement) + 1
                g_locoData(locoNR).SoortCode = CInt(data.Substring(displacement, 1))
                g_locoData(locoNR).LengteCode = CInt(data.Substring(displacement + 1, 1))
                displacement = data.IndexOf(vbTab, displacement) + 1
                g_locoData(locoNR).vanBlokNR = CInt(data.Substring(displacement, 3))
                displacement = data.IndexOf(vbTab, displacement) + 1
                g_locoData(locoNR).eindBlokNR = CInt(data.Substring(displacement, 3))
                displacement = data.IndexOf(vbTab, displacement) + 1
                s = data.Substring(displacement)
                g_locoData(locoNR).F1 = CInt(s.Substring(0, 1))
                g_locoData(locoNR).F2 = CInt(s.Substring(1, 1))
                g_locoData(locoNR).F3 = CInt(s.Substring(2, 1))
                g_locoData(locoNR).F4 = CInt(s.Substring(3, 1))
            Next
            myStreamReader.Close()
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: LocoData.txt " & exc.Message)
        End Try
    End Sub

    Public Sub LoadK83K84()
        Dim data As String = String.Empty
        Dim MyStreamReader As StreamReader
        Dim i As Integer = 0
        Dim index As Integer = 0
        Try
            MyStreamReader = File.OpenText(DataDir & "\K83K84data.txt")
            Do
                i += 1
                data = MyStreamReader.ReadLine
                index = data.IndexOf(vbTab) + 1
                m_ArrayK83K84(i).type = data.Substring(index, 1)        'lang of kort
                index = data.IndexOf(vbTab, index) + 1
                m_ArrayK83K84(i).active = data.Substring(index, 1)      'wissel aanwezig=1 afwezig=0
                m_ArrayK83K84(i).status = False                         'bezet of vrij om over te rijden
                m_ArrayK83K84(i).port = "*"                             '*= nog niet aangesproken
                m_ArrayK83K84(i).statusSynop = "*"                      '*= nog niet aangesproken
                m_ArrayK83K84(i).address = i.ToString                   'itemNR
                m_ArrayK83K84(i).timeout = instellingen.turnoutPulsTime 'basiswaarde voor aanstuurtijd in msec
            Loop Until MyStreamReader.EndOfStream
            MyStreamReader.Close()
            MyStreamReader = Nothing
            m_MaxK83K84 = i
            ReDim Preserve m_ArrayK83K84(m_MaxK83K84)   'Ubound aangepast
        Catch ex As Exception
            MessageBox.Show("Fout in K83K84data.txt: " & ex.Message, "Initialisatie K83K84", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub LoadBlokLocoArray()
        Dim myStreamReader As StreamReader
        Dim data As String
        Dim i As Integer = 0
        Try
            g_BlokLocoArray(0) = "000 000 Dummy"
            myStreamReader = File.OpenText(DataDir & "\BlokLoco.txt")
            Do
                i += 1
                data = myStreamReader.ReadLine
                g_BlokLocoArray(i) = data
            Loop Until myStreamReader.EndOfStream
            myStreamReader.Close()
            myStreamReader = Nothing
            g_MaxBLA = i
            ReDim Preserve g_BlokLocoArray(g_MaxBLA)   'Ubound aangepast
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: : BlokLoco.txt " & exc.Message)
        End Try
    End Sub

    Public Sub LoadReisplannen()
        Dim MyStreamReader As StreamReader
        Dim Data As String
        Dim i As Integer
        'ophalen Reisplannen
        Try
            MyStreamReader = File.OpenText(DataDir & "\Reisplannen.txt")
            Data = MyStreamReader.ReadLine
            ReDim g_ReisPlannenArray(CInt(Data))
            ReDim g_ReisplannenText(CInt(Data))
            For i = 1 To CInt(Data)
                Data = MyStreamReader.ReadLine
                g_ReisplannenText(i) = Data.Substring(0, Data.LastIndexOf(vbTab))
                g_ReisPlannenArray(i) = Data.Substring(Data.LastIndexOf(vbTab) + 1)
            Next
            MyStreamReader.Close()
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: : Reisplannen" & exc.Message)
        End Try

    End Sub

    Public Sub LoadReiswegenConditiereiswegen()
        Dim MyStreamReader As StreamReader
        Dim Data As String
        Dim i As Integer
        'ophalen Reiswegen
        Try
            MyStreamReader = File.OpenText(DataDir & "\Reiswegen.txt")
            Data = MyStreamReader.ReadLine
            ReDim g_ReisWegenArray(CInt(Data))
            For i = 1 To CInt(Data)
                Data = MyStreamReader.ReadLine
                g_ReisWegenArray(i) = Data.Substring(Data.IndexOf(vbTab) + 1)
            Next
            MyStreamReader.Close()
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: : ReisWegen" & exc.Message)
        End Try

        'ophalen ConditieRW
        Try
            MyStreamReader = File.OpenText(DataDir & "\ConditieRW.txt")
            Data = MyStreamReader.ReadLine
            If Not Data Is Nothing Then
                ReDim g_ConditieRWArray(CInt(Data))
                For i = 1 To CInt(Data)
                    Data = MyStreamReader.ReadLine
                    g_ConditieRWArray(i) = Data.Substring(Data.IndexOf(vbTab) + 1)
                Next
            End If
            MyStreamReader.Close()
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: : ConditieRW.txt " & exc.Message)
        End Try
    End Sub

    Public Sub LoadMacro()
        Try
            Dim mystreamreader As StreamReader
            Dim i, d1, d2 As Integer
            Dim data, macro As String
            mystreamreader = File.OpenText(DataDir & "\MacroData.txt")
            data = mystreamreader.ReadToEnd
            d1 = data.IndexOf(MACRODELIMITER)                   'begin delimiterteken @, start van een macro
            g_macroArray(0) = data.Substring(0, d1 - 1)         'index=0: aantal aanwezige macro's in array (crlf aftrekken)
            For i = 1 To CInt(g_macroArray(0))                  'lees alle macro's en plaats in opeenvolgend records. Eerste macroNR=1
                d2 = data.IndexOf(MACRODELIMITER, d1 + 1)
                macro = data.Substring(d1 + 2, d2 - d1 - 3)     'crlf verwijderen
                g_macroArray(i) = macro
                d1 = d2
            Next
            mystreamreader.Close()

        Catch ex As Exception

        End Try
    End Sub

    Public Sub LoadS88MacroNRs()
        Dim mystreamreader As StreamReader
        Dim i, tab1, tab2 As Integer
        Dim data As String
        mystreamreader = File.OpenText(DataDir & "\s88MacroNrs.txt")
        data = mystreamreader.ReadLine
        g_MaxS88MacroNRS = CInt(data)
        For i = 0 To g_MaxS88MacroNRS
            data = mystreamreader.ReadLine
            tab1 = data.IndexOf(vbTab) + 1
            tab2 = data.IndexOf(vbTab, tab1)
            g_s88MacroNRS(i, 0) = CInt(data.Substring(tab1, tab2 - tab1))
            g_s88MacroNRS(i, 1) = CInt(data.Substring(tab2 + 1))
        Next
        mystreamreader.Close()
    End Sub

#End Region

#Region "Save Modelrailway data"

    Public Sub SaveBevelen()
        Dim myStreamWriter As StreamWriter
        Try
            myStreamWriter = File.CreateText(DataDir & "\Bevelen.txt")
            myStreamWriter.WriteLine(g_Bevelen.GetUpperBound(0))
            For i As Integer = 1 To g_Bevelen.GetUpperBound(0)
                myStreamWriter.WriteLine(g_Bevelen(i))
            Next
            myStreamWriter.Close()
            myStreamWriter = Nothing
        Catch exc As Exception
            MessageBox.Show("Fout in wegschrijven van Bevelen.txt " & exc.Message)
        End Try
    End Sub

    Public Sub SaveRelaties()
        Dim myStreamWriter As StreamWriter
        Try
            myStreamWriter = File.CreateText(DataDir & "\Relaties.txt")
            myStreamWriter.WriteLine(g_Relaties.GetUpperBound(0))
            For i As Integer = 1 To g_Relaties.GetUpperBound(0)
                myStreamWriter.WriteLine(g_Relaties(i))
            Next
            myStreamWriter.Close()
            myStreamWriter = Nothing
            myStreamWriter = Nothing
        Catch exc As Exception
            MessageBox.Show("Fout in wegschrijven van Relaties.txt " & exc.Message)
        End Try

    End Sub

    Public Sub SaveLocoData()
        Dim i As Integer
        Dim myStreamWriter As StreamWriter
        Try
            myStreamWriter = File.CreateText(DataDir & "\LocoData.txt")
            For i = 1 To 80
                myStreamWriter.WriteLine(i.ToString _
                    & vbTab & g_locoData(i).locoInDienst.ToString _
                    & vbTab & g_locoData(i).standaardRichting.ToString _
                    & vbTab & g_locoData(i).soortCode.ToString & g_locoData(i).lengteCode.ToString _
                    & vbTab & Format(g_locoData(i).vanBlokNR, "000") _
                    & vbTab & Format(g_locoData(i).eindBlokNR, "000") _
                    & vbTab & g_locoData(i).F1.ToString + g_locoData(i).F1.ToString + g_locoData(i).F2.ToString + g_locoData(i).F3.ToString + g_locoData(i).F4.ToString)
            Next
            myStreamWriter.Close()
            myStreamWriter = Nothing
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: LocoData.txt " & exc.Message)
        End Try
    End Sub

    Public Sub SaveMacro()
        Try
            Dim s As String = g_macroArray(0) + vbCrLf + MACRODELIMITER
            For i As Integer = 1 To CInt(g_macroArray(0))
                s += g_macroArray(i) + vbCrLf + MACRODELIMITER
            Next
            Dim mystreamwriter As StreamWriter
            mystreamwriter = File.CreateText(DataDir & "\MacroData.txt")
            mystreamwriter.Write(s)
            mystreamwriter.Close()
            mystreamwriter = Nothing
        Catch ex As Exception
            MessageBox.Show("Fout in Bestand: MacroData.txt " & ex.Message)
        End Try
    End Sub

    Public Sub SaveS88MacroNRs()
        Dim s As String = String.Empty
        Dim myStreamwriter As StreamWriter
        Try
            myStreamwriter = File.CreateText(DataDir & " \s88MacroNrs.txt")
            myStreamwriter.WriteLine(g_MaxS88MacroNRS.ToString)
            For i As Integer = 0 To g_MaxS88MacroNRS
                myStreamwriter.WriteLine(Format(i, "000") & vbTab & g_s88MacroNRS(i, 0) & vbTab & g_s88MacroNRS(i, 1))
            Next
            myStreamwriter.Close()
            myStreamwriter = Nothing
            myStreamwriter = Nothing
        Catch ex As Exception
            MessageBox.Show("Fout in wegschrijven van s88MacroNrs.txt " & ex.Message)
        End Try
    End Sub

    Public Sub SaveBlokLocoArray()
        Dim i, n As Integer
        Dim myStreamWriter As StreamWriter
        Try
            n = CInt(instellingen.s88modulesLinks) + CInt(instellingen.s88modulesMidden) + CInt(instellingen.s88modulesRechts)
            myStreamWriter = File.CreateText(DataDir & "\BlokLoco.txt")
            For i = 1 To n * 8
                myStreamWriter.WriteLine(g_BlokLocoArray(i))
            Next
            myStreamWriter.Close()
            myStreamWriter = Nothing
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: BlokLoco.txt " & exc.Message)
        End Try
    End Sub

    Public Sub SaveBlokLoco(ByVal copy As Boolean)
        'bewaart de laatste gekende situatie van Blok -loco 
        Dim i As Integer
        Dim ok As Boolean = False
        Dim myStreamWriter As StreamWriter

        Try
            If copy Then
                myStreamWriter = File.CreateText(DataDir & "\BlokLocoCopy.txt")
            Else
                myStreamWriter = File.CreateText(DataDir & "\BlokLoco.txt")
            End If
            For i = 1 To g_MaxBLA
                myStreamWriter.WriteLine(g_BlokLocoArray(i))
            Next
            myStreamWriter.Close()
            myStreamWriter = Nothing
        Catch exc As Exception
            MessageBox.Show("Fout in Bestand: : BlokLoco(copy).txt " & exc.Message)
        End Try

    End Sub

    Public Sub SaveReisplannen()
        Dim data As String
        Dim myStreamWriter As StreamWriter
        Try
            myStreamWriter = File.CreateText(DataDir & "\Reisplannen.txt")
            myStreamWriter.WriteLine(g_ReisPlannenArray.GetUpperBound(0))
            For i As Integer = 1 To g_ReisPlannenArray.GetUpperBound(0)
                data = g_ReisplannenText(i) & vbTab & g_ReisPlannenArray(i)
                myStreamWriter.WriteLine(data)
            Next
            myStreamWriter.Close()
            myStreamWriter = Nothing
        Catch exc As Exception
            MessageBox.Show("Fout in wegschrijven van Reisplannen.txt " & exc.Message)
        End Try

    End Sub

    Public Sub SaveReiswegenConditiereiswegen()
        Dim myStreamWriter As StreamWriter
        Try
            myStreamWriter = File.CreateText(DataDir & "\Reiswegen.txt")
            myStreamWriter.WriteLine(g_ReisWegenArray.GetUpperBound(0))
            For i As Integer = 1 To g_ReisWegenArray.GetUpperBound(0)
                myStreamWriter.WriteLine(g_ReisWegenArray(i))
            Next
            myStreamWriter.Close()
        Catch exc As Exception
            MessageBox.Show("Fout in wegschrijven van Reiswegen.txt " & exc.Message)
        End Try

        Try
            myStreamWriter = File.CreateText(DataDir & "\ConditieRW.txt")
            myStreamWriter.WriteLine(g_ConditieRWArray.GetUpperBound(0))
            For i As Integer = 1 To g_ConditieRWArray.GetUpperBound(0)
                myStreamWriter.WriteLine(g_ConditieRWArray(i))
            Next
            myStreamWriter.Close()
            myStreamWriter = Nothing
        Catch exc As Exception
            MessageBox.Show("Fout in wegschrijven van ConditieRW.txt " & exc.Message)
        End Try

    End Sub

#End Region

#Region "General subs and functions"
    Public Sub Directstart()
        'breng een trein voor één reiwweg tijdelijk in dienst"
        DCS.SetTreinInDienst(CInt(g_TransitArray(8)), g_TransitArray(9))
    End Sub
#End Region

#Region "TE bewaren reserve sub en functions"

    Private Sub MakeNewMenu()

        'Static x As Integer
        'x += 1
        'Dim NewMenu As New ToolStripMenuItem("Show synoptic nr=" & x.ToString, Nothing, New EventHandler(AddressOf GeneralMenu_Click))    '1. nieuw menu item

        'SynopticsToolStripMenuItem.DropDownItems.Add(NewMenu)           '2. toevoegen net onder bestaand menu

        'mnMenu.Items.Add(winmenu)                                       '2a. toevoegen aan bestaand hoofdmenu
        'winmenu.DropDownItems.Add(NewMenu)                              '3a. eerst toevoegen aan nieuw hoofdmenu
        'SynopticsToolStripMenuItem.DropDownItems.Add(winmenu)           '3b. toevoegen aan nieuw menu en nieuw submenu 

        ' keuze: 1 en 2 nodig om item toe te voegen net onder bestaand hoofdmenu item (hier synop)
        ' keuze: 1,2a,3a en 3b om item aan een bestaande hoofdmenu toe te voegen aan een nieuwe submen
    End Sub 'create a new dynamic menu item

#End Region

End Module
