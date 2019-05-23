namespace HomeSeer.PluginSdk.Speech {

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
        string MEDIAFunction(Constants.media_operation_values operation, string p);
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