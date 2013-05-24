
'Digital-S-Inside2.vb created on October, 12 2012
'(c) Wallar
'Verion 0.1
'DCSnr 335: Freischaltcode 3F88BDA7-52EE4090-00000335 
'DCSnr  28: Freischaltcode 8771A257-0C427AEA-00000028

Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text.RegularExpressions

Public Class Digital_S_Inside2

    Implements IDisposable

#Region "Variables, structures, enums,..."

    'vars
    Private IpAddress As String = "Localhost"
    Private port As Integer = 51400
    Private s As Socket
    Private ASCII As New System.Text.ASCIIEncoding()
    Private socket As Socket
    Private disposed As Boolean = False

#End Region

#Region "Public subroutines and Functions"

    Public Function CreateSocket() As String
        Dim str As String = String.Empty
        Try
            socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            socket.Connect(IpAddress, port)
            'If socket.Connected Then Debug.WriteLine(String.Format("Connected to {0}:{1}", IpAddress, port))
        Catch   'ex As Exception
            If Not socket.Connected Then
                If MsgBox("Digital-S-Inside 2: " + vbNewLine + "Start eerst DSI-2 op en druk op YES" + vbNewLine + "Stop programma en druk op NO", _
                MsgBoxStyle.YesNo, "Fout bij opstart DSI-2") = 7 Then
                    End 'program
                Else
                    Return "REDO  DSI2 wordt heropgestart."
                    Exit Function
                End If
            End If
        End Try
        Return String.Format("OK Connected to {0}:{1}", IpAddress, port)
    End Function

    Public Sub CloseSocket()
        If Not IsNothing(s) Then s.Dispose()
    End Sub

    Public Function PowerStatus() As String
        Dim str As String = String.Empty
        Dim xmlSend As String = "<Query System=""1""><Power/></Query>"      'xml string command
        Dim cmd As Byte() = ASCII.GetBytes(xmlSend + Environment.NewLine)   'omgezet in bytes array
        str = SendToDSI_2(cmd)
        Return str
    End Function

    Public Function PowerOn() As String
        Dim str As String = String.Empty
        Dim xmlSend As String = "<Set System=""1""><Power>1</Power></Set>"
        Dim cmd As Byte() = ASCII.GetBytes(xmlSend + Environment.NewLine)
        str = SendToDSI_2(cmd)
        Return str
    End Function

    Public Function PowerOFF() As String
        Dim str As String = String.Empty
        Dim xmlSend As String = "<Set System=""1""><Power>0</Power></Set>"
        Dim cmd As Byte() = ASCII.GetBytes(xmlSend + Environment.NewLine)
        str = SendToDSI_2(cmd)
        Return str
    End Function

    Public Function DSIVersion() As String
        Dim str As String = String.Empty
        Dim xmlSend As String = "<Query><Version/></Query>"      'xml string command
        Dim cmd As Byte() = ASCII.GetBytes(xmlSend + Environment.NewLine)   'omgezet in bytes array
        str = SendToDSI_2(cmd)
        Return str
    End Function

    Public Function DCSSystem() As String
        Dim str As String = String.Empty
        Dim xmlSend As String = "<Query><System/></Query>"      'xml string command
        Dim cmd As Byte() = ASCII.GetBytes(xmlSend + Environment.NewLine)   'omgezet in bytes array
        str = SendToDSI_2(cmd)
        Return str
    End Function

    '<Set System="1"><Loco Address="72" Protocol="M2"><Speed Direction="forwards" Base="14">5</Speed><Function Id="0">1</Function></Loco></Set>
    Public Function SetLocoM(ByVal data As LocoM) As String
        Dim str As String = String.Empty
        If data.speed = "0" Then
            Dim xmlSend As String = "<Set System=""1""><Loco Address=""" + data.address _
                                    + """ Protocol=""M2"">" _
                                    + "<Speed Direction=""" + data.direction _
                                    + """ Base=""14"">" + data.speed + "</Speed>" _
                                    + "<Function Id=""0"">" + data.F0 + "</Function>" _
                                    + "<Function Id=""1"">" + data.F1 + "</Function>" _
                                    + "<Function Id=""2"">" + data.F2 + "</Function>" _
                                    + "<Function Id=""3"">" + data.F3 + "</Function>" _
                                    + "<Function Id=""4"">" + data.F4 + "</Function>" _
                                    + "</Loco></Set>"
            Dim cmd As Byte() = ASCII.GetBytes(xmlSend + Environment.NewLine)
            str = SendToDSI_2(cmd)
            Return str
        Else
            Dim xmlSend As String = "<Set System=""1""><Loco Address=""" + data.address _
                                    + """ Protocol=""M2"">" _
                                    + "<Speed Direction=""" + data.direction _
                                    + """ Base=""14"">" + data.speed + "</Speed>" _
                                    + "<Function Id=""0"">" + data.F0 + "</Function>" _
                                    + "<Function Id=""1"">" + data.F1 + "</Function>" _
                                    + "<Function Id=""2"">" + data.F2 + "</Function>" _
                                    + "<Function Id=""3"">" + data.F3 + "</Function>" _
                                    + "<Function Id=""4"">" + data.F4 + "</Function>" _
                                    + "</Loco></Set>"
            Dim cmd As Byte() = ASCII.GetBytes(xmlSend + Environment.NewLine)
            str = SendToDSI_2(cmd)
            Return str
        End If
    End Function

    '<Set System="1"><Decoder Address=""" + data.address + """ Protocol="M" Port=""" + data.port + """ Timeout=""" + data.timeout + """>1</Decoder></Set>
    Public Function SetDecoderM(ByVal data As DecoderM) As String
        Dim str As String = String.Empty
        Dim xmlSend As String = "<Set System=""1""><Decoder Address=""" + data.address + """ Protocol=""M"" Port=""" + data.port + """ Timeout=""" + data.timeout + """>1</Decoder></Set>"
        Dim cmd As Byte() = ASCII.GetBytes(xmlSend + Environment.NewLine)
        str = SendToDSI_2(cmd)
        Return str
    End Function

#End Region

#Region "Private subroutines and Functions"

    Private Function SendToDSI_2(ByVal cmd As Byte()) As String
        Dim bytes(16 * 1024) As Byte    'buffer
        Dim bytesRec As Integer         'ontvangen bytes
        Dim str As String               'antwoord van DSI-2
        'Events data received from DSI-2
        Do While socket.Available > 0
            bytesRec = socket.Receive(bytes)
            str = ASCII.GetString(bytes, 0, bytesRec)
        Loop
        'send to DSI-2
        Dim bytesSent As Integer = socket.Send(cmd)
        bytesRec = socket.Receive(bytes)
        str = ASCII.GetString(bytes, 0, bytesRec)
        Return str
    End Function

#Region "IDisposable Support"

    'IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposed Then
            If disposing Then
                socket.Dispose()
            End If
        End If
        Me.disposed = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub

#End Region

#End Region

End Class
