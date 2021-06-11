namespace HomeSeer.PluginSdk.Devices.Identification {

    /// <summary>
    /// The primary use of a <see cref="HsFeature"/>. This describes a function of a piece of hardware.
    /// </summary>
    /// <seealso cref="TypeInfo.Type"/>
    public enum EFeatureType {

        /// <summary>
        /// A generic <see cref="HsFeature"/>. There is no specific use for it.
        /// </summary>
        Generic = 0,
        /// <summary>
        /// The <see cref="HsFeature"/> is used for security purposes.
        /// </summary>
        Security = 8,
        /// <summary>
        /// The <see cref="HsFeature"/> is used to show the status of a thermostat.
        /// </summary>
        /// <seealso cref="EThermostatStatusFeatureSubType"/>
        ThermostatStatus = 16,
        /// <summary>
        /// The <see cref="HsFeature"/> is used to control a thermostat.
        /// </summary>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        ThermostatControl = 17,
        /// <summary>
        /// The <see cref="HsFeature"/> is used to interact with a media playback device.
        /// </summary>
        /// <seealso cref="EMediaFeatureSubType"/>
        Media = 32,
        /// <summary>
        /// The <see cref="HsFeature"/> is used to interact with energy management devices.
        /// </summary>
        /// <seealso cref="EEnergyFeatureSubType"/>
        Energy = 256

    }

}