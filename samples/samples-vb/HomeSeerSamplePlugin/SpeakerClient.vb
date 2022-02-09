Imports HomeSeer.PluginSdk
Imports HomeSeer.PluginSdk.Logging
Imports HomeSeer.PluginSdk.Speech
Imports HSCF.Communication.Scs.Communication.EndPoints.Tcp
Imports HSCF.Communication.ScsServices.Client
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports HomeSeer.PluginSdk.Constants

Class SpeakerClient
    Implements IFromSpeaker

    Private _speakHost As ISpeechAPI
    Private _client As IScsServiceClient(Of ISpeechAPI)
    Private Const SPEAKER_INTERFACE_VERSION As Integer = 10
    Private _clientName As String = ""
    Public Shared objlock As Object = New Object()


    Public Sub New(ByVal name As String)
        _clientName = name
    End Sub

    Public Function Connect(ByVal username As String, ByVal password As String) As Boolean
        SyncLock objlock
            Dim ipAddress As String = "127.0.0.1"
            Logger.LogInfo("Connecting speaker client {0} to HomeSeer IP {1}", _clientName, ipAddress)

            Try
                _client = ScsServiceClientBuilder.CreateClient(Of ISpeechAPI)(New ScsTcpEndPoint(ipAddress, 10401), Me)
                AddHandler _client.Disconnected, New EventHandler(AddressOf ClientDisconnected)
                AddHandler _client.Connected, New EventHandler(AddressOf ClientConnected)
            Catch e As Exception
                Logger.LogError(e.ToString())
                Return False
            End Try

            Try
                _client.Connect()
                _speakHost = _client.ServiceProxy
                Dim v As Integer

                Try
                    v = _speakHost.version()
                Catch e As Exception
                    Logger.LogError("Error attempting to connect to server, please check your connection options: {0}", e.Message)
                    Return False
                End Try

                If v <> SPEAKER_INTERFACE_VERSION Then
                    Logger.LogError("Speaker API version mismatch")
                    Return False
                End If

                Dim rval As String = _speakHost.Connect("Sample Plugin", _clientName, "127.0.0.1", username, password)

                If Not String.IsNullOrEmpty(rval) Then
                    Logger.LogError("Error, Unable to connect speaker client interface: {0}", rval)
                    DisconnectNow()
                    Return False
                End If

            Catch e As Exception
                Logger.LogError("Error while connecting speaker client: {0}", e.ToString())
                Return False
            End Try
        End SyncLock

        Return True
    End Function

    Private Sub ClientDisconnected(ByVal sender As Object, ByVal e As EventArgs)
        Logger.LogInfo("Speaker client {0} has disconnected", _clientName)
    End Sub

    Private Sub ClientConnected(ByVal sender As Object, ByVal e As EventArgs)
        Logger.LogInfo("Speaker client {0} has connected", _clientName)
    End Sub

    Private Sub DisconnectNow()
        SyncLock objlock

            Try

                If _client IsNot Nothing Then

                    Try

                        If _speakHost IsNot Nothing Then
                            _speakHost = Nothing
                        End If

                        _client.Disconnect()
                        _client.Dispose()
                    Catch __unusedException1__ As Exception
                    End Try
                End If

            Catch __unusedException1__ As Exception
            End Try
        End SyncLock
    End Sub

    Public Function Disconnect() As Boolean Implements IFromSpeaker.Disconnect
        DisconnectNow()
        Return True
    End Function

    Public Sub SpeakText(ByVal speaktxt As String, ByVal wait As Boolean) Implements IFromSpeaker.SpeakText
        Logger.LogInfo("Speaking from Sample Speaker Client: " & speaktxt)
    End Sub

    Public Sub PlayWaveFile(ByVal fname As String, ByVal fsize As Integer) Implements IFromSpeaker.PlayWaveFile
        Logger.LogInfo("Playing wave file from Sample Speaker Client, file: " & fname)
    End Sub

    Public Function IsBusy() As Boolean Implements IFromSpeaker.IsBusy
        Return False
    End Function

    Public Sub VRChanged() Implements IFromSpeaker.VRChanged
    End Sub

    Public Sub StartListen() Implements IFromSpeaker.StartListen
    End Sub

    Public Sub StopListen() Implements IFromSpeaker.StopListen
    End Sub

    Public Function SetVoice(ByVal voice As String) As Short Implements IFromSpeaker.SetVoice
        Return 0
    End Function

    Public Sub SpeakToFile(ByVal txt As String, ByVal fname As String, ByVal Optional voice As String = "") Implements IFromSpeaker.SpeakToFile
    End Sub

    Public Function SendMessage(ByVal message As String, ByVal showballoon As Boolean) As Integer Implements IFromSpeaker.SendMessage
        Return 0
    End Function

    Public Sub PauseAudio() Implements IFromSpeaker.PauseAudio
    End Sub

    Public Sub UnPauseAudio() Implements IFromSpeaker.UnPauseAudio
    End Sub

    Public Sub MuteAudio() Implements IFromSpeaker.MuteAudio
    End Sub

    Public Sub UnMuteAudio() Implements IFromSpeaker.UnMuteAudio
    End Sub

    Public Function GetMuteStatus() As Boolean Implements IFromSpeaker.GetMuteStatus
        Return False
    End Function

    Public Function GetListenStatus() As Boolean Implements IFromSpeaker.GetListenStatus
        Return False
    End Function

    Public Function GetPauseStatus() As Short Implements IFromSpeaker.GetPauseStatus
        Return 0
    End Function

    Public Function GetVoiceName() As String Implements IFromSpeaker.GetVoiceName
        Return String.Empty
    End Function

    Public Function MEDIAPlay(ByVal filename As String, ByVal fsize As Integer) As Integer Implements IFromSpeaker.MEDIAPlay
        Return 0
    End Function

    Public Function MEDIAFunction(ByVal operation As EMediaOperation, ByVal p As String) As String Implements IFromSpeaker.MEDIAFunction
        Return String.Empty
    End Function

    Public Function GetVolume() As Integer Implements IFromSpeaker.GetVolume
        Return 0
    End Function

    Public Sub SetVolume(ByVal vol As Integer) Implements IFromSpeaker.SetVolume
    End Sub

    Public Sub StopSpeaking() Implements IFromSpeaker.StopSpeaking
    End Sub

    Public Function SetListenMode(ByVal mode As Integer) As Integer Implements IFromSpeaker.SetListenMode
        Return 0
    End Function

    Public Function GetListenMode() As Integer Implements IFromSpeaker.GetListenMode
        Return 0
    End Function

    Public Sub SetVRState(ByVal state As String) Implements IFromSpeaker.SetVRState
    End Sub

    Public Sub SetSpeaker(ByVal speaker_name As String) Implements IFromSpeaker.SetSpeaker
    End Sub

    Public Function SetSpeakingSpeed(ByVal speed As Integer) As Integer Implements IFromSpeaker.SetSpeakingSpeed
        Return 0
    End Function
End Class

Friend Module Logger
        Sub Log(ByVal line As String, ByVal level As ELogType)
            Try
                Program._plugin.WriteLog(level, line)
            Catch ex As Exception
                Console.WriteLine(ex.ToString())
            End Try
        End Sub

        Sub LogDebug(ByVal line As String)
            Log(line, ELogType.Debug)
        End Sub

        Sub LogDebug(ByVal format As String, ParamArray args As Object())
            LogDebug(String.Format(format, args))
        End Sub

        Sub LogInfo(ByVal line As String)
            Log(line, ELogType.Info)
        End Sub

        Sub LogInfo(ByVal format As String, ParamArray args As Object())
            LogInfo(String.Format(format, args))
        End Sub

        Sub LogWarning(ByVal line As String)
            Log(line, ELogType.Warning)
        End Sub

        Sub LogWarning(ByVal format As String, ParamArray args As Object())
            LogWarning(String.Format(format, args))
        End Sub

        Sub LogError(ByVal line As String)
            Log(line, ELogType.[Error])
        End Sub

        Sub LogError(ByVal format As String, ParamArray args As Object())
            LogError(String.Format(format, args))
        End Sub
    End Module
