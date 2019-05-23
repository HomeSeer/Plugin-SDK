using System;

namespace HomeSeer.PluginSdk.Speech {

    public class RemoteHost {

        public string       hostname = ""; // name of host speaker client is running on
        public string       instance = ""; // instance name of speaker client on remote host
        public IFromSpeaker client;        // receiver object used to call back into speaker client
        public long         clientID;      // unique identifier of client
        public object       vr;            // vr object to handle voice recognition
        public string       ipaddr = "";   // IP address of remote host
        public int          SpeakingCount; // incremented when speaking or wave play starts, decremented when finished
        public DateTime     LastBusyCheck; // last time we polled the remote for play wave and speak status
        public bool         valid;         // true indicates valid entry
        public object       Lock = new object();

    }

}