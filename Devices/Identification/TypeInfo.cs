using System;

namespace HomeSeer.PluginSdk.Devices.Identification {

    /// <summary>
    /// A bundle of information describing the type of a device or feature.
    /// <para>
    /// This is used to describe the device/feature in a manner that is easily understood by UI generation engines
    ///  and other smart home platforms. When these systems can understand what this device/feature is, they are
    ///  better able to tailor the experience of the user to their expectations.
    /// </para>
    /// </summary>
    /// <seealso cref="EApiType"/>
    /// <seealso cref="EDeviceType"/>
    /// <seealso cref="EFeatureType"/>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class TypeInfo {
                
        private int _apiType = (int) EApiType.NotSpecified;
        private int _type;
        private int _subType;
        private string _subTypeDesc = "";
        private string _summary = "";

        /// <summary>
        /// The primary type of the device/feature.
        ///  This should always represent whether the item is a device or feature.
        /// <para> <see cref="EApiType.Device"/> for devices and <see cref="EApiType.Feature"/> for features </para>
        /// </summary>
        /// <seealso cref="EApiType"/>
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

        public string Summary {
            get => _summary ?? "";
            set => _summary = value ?? "";
        }

    }

}