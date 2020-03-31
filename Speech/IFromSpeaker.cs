namespace HomeSeer.PluginSdk.Speech {

    /// <summary>
    /// PLEASE NOTE: Code related to the Speech components in HomeSeer were ported from the HS3 plugin API and
    ///  have not been fully tested to verify full functionality from the new SDK. The Speech API may undergo
    ///  significant changes in the near future. Please use with caution.
    /// </summary>
    public interface IFromSpeaker {

        void   SpeakText(string speaktxt, bool wait);
        bool   IsBusy();
        void   VRChanged();
        void   StartListen();
        void   StopListen();
        short  SetVoice(string voice);
        void   SpeakToFile(string txt, string fname, string voice = "");
        int    SendMessage(string message, bool showballoon);
        void   PlayWaveFile(string fname, int fsize);
        void   PauseAudio();
        void   UnPauseAudio();
        void   MuteAudio();
        void   UnMuteAudio();
        bool   GetMuteStatus();
        bool   GetListenStatus();
        short  GetPauseStatus();
        string GetVoiceName();
        int    MEDIAPlay(string filename, int fsize);
        string MEDIAFunction(EMediaOperation operation, string p);
        int    GetVolume();
        void   SetVolume(int vol);
        bool   Disconnect();
        void   StopSpeaking();
        int    SetListenMode(int mode);
        int    GetListenMode();
        void   SetVRState(string state);
        void   SetSpeaker(string speaker_name);
        int    SetSpeakingSpeed(int speed);

    }

}