using System;

namespace HomeSeer.PluginSdk.Devices.Identification {

    /// <summary>
    /// A bundle of information describing the type of a <see cref="HsDevice"/> or <see cref="HsFeature"/>.
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
        /// <remarks>
        /// This can only be one of a few values. It is the strictest type.
        /// </remarks>
        /// <seealso cref="EApiType"/>
        public EApiType ApiType {
            get => (EApiType) _apiType;
            set => _apiType = (int) value;
        }

        /// <summary>
        /// The <see cref="EDeviceType"/> or <see cref="EFeatureType"/> of the <see cref="AbstractHsDevice"/>
        /// </summary>
        /// <remarks>
        /// <para>Acceptable values can be from <see cref="EDeviceType"/> or <see cref="EFeatureType"/>.
        ///  It is not as strict as <see cref="ApiType"/>, but it is stricter than <see cref="SubType"/>.</para>
        /// <para>You can extend this and define new types and subtypes as needed. It is important to note
        ///  that these new types will not be understood by HomeSeer and may cause HomeSeer to render them
        ///  as a generic <see cref="HsDevice"/> or <see cref="HsFeature"/>. If you extend these types and would like
        ///  to have HomeSeer support them you can create a pull request on GitHub.</para>
        /// </remarks>
        /// <seealso cref="EDeviceType"/>
        /// <seealso cref="EFeatureType"/>
        public int Type {
            get => _type;
            set => _type = value;
        }
        
        /// <summary>
        /// The sub-type of the <see cref="AbstractHsDevice"/>.
        /// </summary>
        /// <remarks>
        /// <para>This is the least strict type field.</para>
        /// <para>You can extend this and define new subtypes as needed. It is important to note
        ///  that these new types will not be understood by HomeSeer and may cause HomeSeer to render them
        ///  as a generic <see cref="HsDevice"/> or <see cref="HsFeature"/>. If you extend these types and would like
        ///  to have HomeSeer support them you can create a pull request on GitHub.</para>
        /// </remarks>
        /// <seealso cref="EGenericDeviceSubType"/>
        /// <seealso cref="EGenericFeatureSubType"/>
        /// <seealso cref="EEnergyFeatureSubType"/>
        /// <seealso cref="EMediaFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatStatusFeatureSubType"/>
        public int SubType {
            get => _subType;
            set => _subType = value;
        }
        
        //TODO Description util
        /// <summary>
        /// A human-readable string description of the configured <see cref="SubType"/>
        /// </summary>
        /// <remarks>This must be manually set</remarks>
        public string SubTypeDescription {
            get => _subTypeDesc ?? "";
            set => _subTypeDesc = value ?? "";
        }

        //TODO Summary util
        /// <summary>
        /// A human-readable string summary of the entire <see cref="TypeInfo"/>
        /// </summary>
        /// <remarks>This must be manually set</remarks>
        public string Summary {
            get => _summary ?? "";
            set => _summary = value ?? "";
        }

    }

}