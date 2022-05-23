using System;

namespace HomeSeer.PluginSdk.Speech {

    /// <summary>
    /// PLEASE NOTE: Code related to the Speech components in HomeSeer were ported from the HS3 plugin API and
    ///  have not been fully tested to verify full functionality from the new SDK. The Speech API may undergo
    ///  significant changes in the near future. Please use with caution.
    /// </summary>
    public class clsLastVR {

        public string   Raw      = "";
        public string   Parsed   = "";
        public string   Host     = "";
        public string   Instance = "";
        public int      ID       = -1;
        public DateTime Time     = DateTime.MinValue;

    }

}