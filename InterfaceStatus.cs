using System;

namespace HomeSeer.PluginSdk {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public struct InterfaceStatus {

        public Constants.enumInterfaceStatus intStatus;
        public string              sStatus;

    }

}