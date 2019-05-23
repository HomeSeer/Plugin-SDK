using System;
using System.Collections.ObjectModel;

namespace HomeSeer.PluginSdk.Speech {

    public interface ISpeechAPI {

        // Function GetHSPIInterface() As Object
    int GetTestNumber();
    int version();
    int RemoteHostsListCount();
    void RemoveHost(string host, string instance);
    bool IsHostConnected(string host, string instance);
    void VRChanged(RemoteHost rh);
    Collection<object> GetHostsList(string host_instance);
    void SetSpeaker(string speaker_name, string host_instance);
    int SetSpeakingSpeed(int speed, string host_instance);
    void PauseAudio(string host_instance);
    void UnPauseAudio(string host_instance);
    void MuteAudio(string host_instance);
    void UnMuteAudio(string host_instance);
    void SetVRState(string state, string host_instance);
    Collection<object> GetRemoteHost(string host, string instance);
    void SpeakFromClient(string txt, string host);
    Constants.speak_error_values Speak(string txt, bool wait, string host_Specified, Constants.speak_type_values mtype, bool showballoon, int fsize);
    void WaveDone(string host, string instance);
    void SpeakDone(string host, string instance);
    bool IsSpeaking(string host);
    void StopSpeaking(string host_instance);
    string GetAttentionMenu(string host, string instance);
    string CreateGrammar(string host, string instance, string AttentionWord, string YesWord, string NoWord, string IgnoreWord);
    void Reset_mDoIt(string host, string instance);
    string GetCommandMenu(string host, string instance);
    void SetAttentionPhrases(string attstr, string attackstr, string ignorestr, string ignoreackstr, string host, string instance);
    void SetLastVR(string raw_phrase, string parsed_phrase, int id, string host, string instance);
    string Recognize(string raw_phrase, string parsed_phrase, int id, string host, string instance);
    DateTime GetFileDate(string fname);
    string GetFilePart(string fname, int start, int fsize);
    void MEDIAPlay(string filename, string host_instance);
    string MEDIAFunction(Constants.media_operation_values operation, string p, string host_instance);
    int GetVolume(string host_instance);
    bool GetMuteStatus(string host_instance);
    bool GetListenStatus(string host_instance);
    int GetPauseStatus(string host_instance);
    string GetVoiceName(string host_instance);
    void SetVolume(int vol, string host_instance);
    void StartListening(string host_instance);
    void StopListening(string host_instance);
    int SetVoice(string voice, string host_instance);
    int SetListenMode(int mode, string host_instance);
    int GetListenMode(string host_instance);

    string Connect(string hostname, string instance, string ipaddr, string user, string pw);

    // new
    string RecognizeRaw(string phrase, string host, string instance);
    string ConnectLocal(string hostname, string instance, string ipaddr, string user, string pw, IFromSpeaker hostapi);
    void DisconnectLocal(long client_id);

    }

}