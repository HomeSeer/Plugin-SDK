using System;

namespace HomeSeer.PluginSdk.Logging {

    public struct LogEntry {

        public DateTime LogTime;
        public string   LogType;
        public string   LogText;
        public string   LogStyleColor;
        public int      LogPriority;
        public string   LogFrom;
        public int      LogErrorCode;
        public int      LogLength;
        public int      LogID;

    }

}