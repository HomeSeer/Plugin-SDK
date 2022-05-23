namespace HomeSeer.PluginSdk.Devices.Identification {

    /// <summary>
    /// The general type of an <see cref="AbstractHsDevice"/>
    /// </summary>
    /// <remarks>
    /// <para>This is primarily used to determine whether an <see cref="AbstractHsDevice"/> is a
    ///  <see cref="HsDevice"/> or a <see cref="HsFeature"/></para>
    /// <para>Creating a <see cref="HsDevice"/> or <see cref="HsFeature"/> using <see cref="DeviceFactory"/>
    ///  or <see cref="FeatureFactory"/> will automatically set this for you.</para>
    /// <para>Legacy types will be readable as <see langword="int"/></para>
    /// </remarks>
    /// <seealso cref="TypeInfo"/>
    public enum EApiType {

        /// <summary>
        /// No type is set. <see cref="AbstractHsDevice"/>s are unusable in this state.
        /// </summary>
        NotSpecified     = 0,
        //LegacyPlugin     = 4,
        //LegacyThermostat = 16,
        /// <summary>
        /// A <see cref="HsDevice"/>
        /// </summary>
        Device           = 512,
        /// <summary>
        /// A <see cref="HsFeature"/>
        /// </summary>
        Feature          = 513
        
        
        /*Media        = eCapabilities.CA_Music,        // 32
        Security     = eCapabilities.CA_Security,     // 8
        SourceSwitch = eCapabilities.CA_SourceSwitch, // 64
        Script       = 128,
        Energy       = 256*/

    }

}