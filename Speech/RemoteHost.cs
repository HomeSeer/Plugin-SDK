using System;

namespace HomeSeer.PluginSdk.Speech {

    /// <summary>
    /// PLEASE NOTE: Code related to the Speech components in HomeSeer were ported from the HS3 plugin API and
    ///  have not been fully tested to verify full functionality from the new SDK. The Speech API may undergo
    ///  significant changes in the near future. Please use with caution.
    /// </summary>
    public class RemoteHost {

        /// <summary>
        /// name of host speaker client is running on
        /// </summary>
        public string       hostname = ""; 
        /// <summary>
        /// instance name of speaker client on remote host
        /// </summary>
        public string       instance = "";
        /// <summary>
        /// receiver object used to call back into speaker client
        /// </summary>
        public IFromSpeaker client;
        /// <summary>
        /// unique identifier of client
        /// </summary>
        public long         clientID;
        /// <summary>
        /// vr object to handle voice recognition
        /// </summary>
        public object       vr;
        /// <summary>
        /// IP address of remote host
        /// </summary>
        public string       ipaddr = "";
        /// <summary>
        /// incremented when speaking or wave play starts, decremented when finished
        /// </summary>
        public int          SpeakingCount;
        /// <summary>
        /// last time we polled the remote for play wave and speak status
        /// </summary>
        public DateTime     LastBusyCheck;
        /// <summary>
        /// true indicates valid entry
        /// </summary>
        public bool         valid;
        public object       Lock = new object();

    }

}