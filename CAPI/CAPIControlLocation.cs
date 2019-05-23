using System;

namespace HomeSeer.PluginSdk.CAPI {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public struct CAPIControlLocation {

        public int Row;
        public int Column;
        public int ColumnSpan;

    }

}