using System;

namespace HomeSeer.PluginSdk.Devices {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class DeviceTypeInfo {
                
        private int _apiType = (int) EApiType.NotSpecified;
        private int _type;
        private int _subType;
        private string _subTypeDesc = "";

        public EApiType ApiType {
            get => (EApiType) _apiType;
            set => _apiType = (int) value;
        }

        public int Type {
            get => _type;
            set => _type = value;
        }
        
        public int SubType {
            get => _subType;
            set => _subType = value;
        }
        
        public string SubTypeDescription {
            get => _subTypeDesc ?? "";
            set => _subTypeDesc = value ?? "";
        }

    }

}