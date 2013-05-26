'Class HsiUsb to talk directly to the LDT-HSI-88-USB unit,
'or to the HSI-88-USB part in the LDT-DiCoStation unit
'v, m, s and eventdriven i cmd implemented
'i event responses runs on a separate thread
'
'(c) walter@larno.be [version 20/01/2012]

Option Compare Binary
Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Threading.Thread
Imports System.ComponentModel
Imports Microsoft.Win32.SafeHandles
Imports System.Runtime.InteropServices

Public Class HsiUsb
    Implements IDisposable

#Region "Declarations, properties, variables"

    Private device As SafeFileHandle
    Private _modules As Integer
    Private _readThread As Thread
    Private usbStream As FileStream
    'events
    Public Event s88Changed(ByVal sender As Object, ByVal e As ChangedEventArgs)


    Public ReadOnly Property Stream() As FileStream
        Get
            Return usbStream
        End Get
    End Property

    Private _version As String = String.Empty
    Public ReadOnly Property Version() As String
        Get
            Return _version
        End Get
    End Property

    Private _readLoopActive As Boolean = False
    Public ReadOnly Property ReadLoopActive() As Boolean
        Get
            Return _readLoopActive
        End Get
    End Property

    <DllImport("Kernel32", SetLastError:=True, CharSet:=CharSet.Unicode, EntryPoint:="CreateFileW", ExactSpelling:=True)> _
    Private Shared Function CreateFile(ByVal fileName As String, ByVal desiredAccess As FileAccess, _
    ByVal shareMode As FileShare, ByVal securityAttributes As IntPtr, ByVal creationDisposition As FileMode, _
    ByVal flagsAndAttributes As FileOptions, ByVal templateFile As IntPtr) As SafeFileHandle
    End Function

    <DllImport("Kernel32.dll", SetLastError:=True)> _
    Private Shared Function CloseHandle(ByVal handle As SafeFileHandle) As Boolean
    End Function

#End Region

#Region "Class methods"

    Public Sub Open()
        ' Open the  USB device with the createFile API
        device = CreateFile("\\.\HsiUsb1", _
                FileAccess.ReadWrite, _
                FileShare.None, _
                IntPtr.Zero, _
                FileMode.Open, _
                FileOptions.None, _
                IntPtr.Zero)

        ' If not existing or an other problem, read the exception
        If (device.IsInvalid) Then
            Throw New Win32Exception(Marshal.GetLastWin32Error())
        End If

        ' We can use the standard .Net FileStream class for read and write operations
        usbStream = New FileStream(device, FileAccess.ReadWrite)

        'Get the HsiUsb version
        GetVersion()
    End Sub

    Private Sub Close()
        'If _readLoopActive OrElse Not _readThread Is Nothing Then
        '    Throw New Exception("Call then StopReadLoopBegin/End functions first")
        'End If
        usbStream.Dispose()
        device.Close()
        device.Dispose()
    End Sub

    Public Function GetVersion() As String
        If String.IsNullOrEmpty(_version) Then  'process only once
            'build command buffer array
            Dim buffer(1) As Byte
            buffer(0) = 118     'v command
            buffer(1) = 13      'carriage return
            usbStream.Write(buffer, 0, buffer.Length)
            'read the version string
            Dim result As Integer = usbStream.ReadByte()
            While (result <> 13)
                _version += Chr(result)
                result = usbStream.ReadByte()
            End While
        End If
        usbStream.Flush()
        Return _version
    End Function

    Public Function InitHsiUsb(ByVal left As Byte, ByVal middle As Byte, ByVal right As Byte) As Byte()
        Dim result(99) As Byte  'maximum number of returned bytes
        Dim rt As Integer       'answerbyte of HSI-USB
        Dim maxIndex As Integer = (left + middle + right) * 3 + 4   'laatste geldige waarde

        'Initialize and start by sending a 's' command
        Dim buffer(4) As Byte
        buffer(0) = 115     's command
        buffer(1) = left    'number of left modules
        buffer(2) = middle  'numner of middle modules
        buffer(3) = right   'number of right modules
        buffer(4) = 13      'CR
        usbStream.Write(buffer, 0, buffer.Length)

        's cmd response
        For k As Integer = 0 To maxIndex
            rt = usbStream.ReadByte
            result(k) = CByte(rt)
        Next
        usbStream.Flush()
        result(99) = CByte(maxIndex)
        Return result
    End Function

    Public Function ReadS88() As Byte()
        'not frequently used
        Dim result(99) As Byte  'maximum number of byte possible in a response
        'build the m command
        Dim buffer(1) As Byte
        buffer(0) = 109     'm command
        buffer(1) = 13      'CR
        usbStream.Write(buffer, 0, 2)
        Thread.Sleep(100)

        'get the m command response
        Dim i As Integer = 0
        Dim rt As Integer = usbStream.ReadByte
        While (rt <> 13)
            result(i) = CByte(rt)
            i += 1
            rt = usbStream.ReadByte
        End While
        usbStream.Flush()
        result(99) = CByte(i - 1)   'put the valid number of response bytes in index 99
        Return result
    End Function

    Public Sub StartReadLoop()
        ' Get a new thread from the threadpool to start the HSI_listener
        ' Create our own thread to have control of stopping it
        _readThread = New Thread(AddressOf ReadLoop)
        _readThread.Start()
        _readLoopActive = True
    End Sub

    Public Sub StopReadLoopBegin()
        _readLoopActive = False 'maak einde aan de listener loop
        ' Hierna moet door DiCoStation.vb nog 1 commando gegeven worden dat de s88 module triggert
        ' (wachten op de usbStream.ReadByte() die nog moet terugkomen uit ReadLoop)
        'Debug.WriteLine("2. Hsi88Usb: _redloopActive= false")
    End Sub

    Public Sub CloseHsi88Usb()
        If Not _readThread Is Nothing Then
            _readThread.Join()  ' LaRud
            _readThread = Nothing
        End If
        Close() 'close the device file
        ' Debug.WriteLine("4. Hsi88Usb: Release Hsi88Usb")
    End Sub

#End Region

#Region "S88 Event handling send data to dicostation.vb"

    Private Sub ReadLoop()
        'implementation of the HsiUsb-event handling aanpassing vanaf 19/09/2012    ok voor visual studio 2012
        Dim i, nMod As Integer
        Dim read As Integer = 0
        Dim stateRead As New ChangedEventArgs()
        s88ChangesAllowed = True        's88module wijzigingen mogen ontvangen worden
        Do
            Do      'wachten tot s88DataProcessing klaar is om nieuwe s88module wijzigingen te verwerken
                'Application.DoEvents()
                Sleep(1)
            Loop Until s88ChangesAllowed
            '===========================================
            read = usbStream.ReadByte()     'i-cmd= 105, hier wacht het programma tot er een byte is
            If Not _readLoopActive Then
                'Application.DoEvents()
                Sleep(0)
                Exit Sub 'verlaat subroutine
            End If
            '===========================================
            nMod = usbStream.ReadByte()     'aantal modules gemeld
            For i = 1 To nMod                 'voor het aantal gewijzigde modules
                read = usbStream.ReadByte() : stateRead.Buffer(0) = CByte(read)   'modulenummer
                read = usbStream.ReadByte() : stateRead.Buffer(1) = CByte(read)   'Highbyte
                read = usbStream.ReadByte() : stateRead.Buffer(2) = CByte(read)   'Lowbyte
                'verstuur naar s88dataProcessing
                stateRead.s88ModulesChanged = True
                RaiseEvent s88Changed(Me, stateRead)  'send modNR, HB and LB to dicostation
                stateRead = New ChangedEventArgs()  'reset
            Next
            read = usbStream.ReadByte()     'verwijder cr (13) als laatste van het bericht
        Loop
    End Sub 's88modules events

#End Region

#Region " IDisposable Support and Hsi-Usb ChangedEventArgs Class "

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                usbStream.Dispose()
                device.Dispose()
            End If

            ' TODO: free shared unmanaged resources
        End If
        Me.disposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Public Class ChangedEventArgs
        Inherits EventArgs

        Private _buffer(2) As Integer  'reeks moduleNR, HB, LB
        Public Property Buffer() As Integer()
            Get
                Return _buffer
            End Get
            Set(ByVal value As Integer())
                _buffer = value
            End Set
        End Property

        Private _s88ModulesChanged As Boolean = False   'aantal gewijzigde modules
        Public Property s88ModulesChanged() As Boolean
            Get
                Return _s88ModulesChanged
            End Get
            Set(ByVal value As Boolean)
                _s88ModulesChanged = value
            End Set
        End Property
    End Class    'HSI-USB ChangeEventArgs class

#End Region

End Class       'HSI88-USB class
