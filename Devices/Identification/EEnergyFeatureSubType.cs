using System;

namespace HomeSeer.PluginSdk.Devices {

    /// <inheritdoc cref="Identification.EEnergyFeatureSubType"/>
    [Obsolete("Please use HomeSeer.PluginSdk.Devices.Identification.EEnergyFeatureSubType", false)]
    public enum EEnergyFeatureSubType {

        /// <inheritdoc cref="Identification.EEnergyFeatureSubType.Watts"/>
        [Obsolete("Please use HomeSeer.PluginSdk.Devices.Identification.EEnergyFeatureSubType", false)]
        Watts    = 1,
        /// <inheritdoc cref="Identification.EEnergyFeatureSubType.Amps"/>
        [Obsolete("Please use HomeSeer.PluginSdk.Devices.Identification.EEnergyFeatureSubType", false)]
        Amps     = 2,
        /// <inheritdoc cref="Identification.EEnergyFeatureSubType.Volts"/>
        [Obsolete("Please use HomeSeer.PluginSdk.Devices.Identification.EEnergyFeatureSubType", false)]
        Volts    = 3,
        /// <inheritdoc cref="Identification.EEnergyFeatureSubType.Kwh"/>
        [Obsolete("Please use HomeSeer.PluginSdk.Devices.Identification.EEnergyFeatureSubType", false)]
        KWH      = 4,         
        /// <inheritdoc cref="Identification.EEnergyFeatureSubType.Graphing"/>
        [Obsolete("Please use HomeSeer.PluginSdk.Devices.Identification.EEnergyFeatureSubType", false)]
        Graphing = 5        

    }

}

namespace HomeSeer.PluginSdk.Devices.Identification {
    
    /// <summary>
    /// The specific use of an <see cref="EFeatureType.Energy"/> type <see cref="HsFeature"/>
    /// </summary>
    /// <remarks>This has not been fully migrated from the legacy API. Expect future changes.</remarks>
    public enum EEnergyFeatureSubType {

        /// <summary>
        /// The <see cref="HsFeature"/> is used to track Watts.
        /// </summary>
        Watts    = 1,
        /// <summary>
        /// The <see cref="HsFeature"/> is used to track Amps.
        /// </summary>
        Amps     = 2,
        /// <summary>
        /// The <see cref="HsFeature"/> is used to track Volts.
        /// </summary>
        Volts    = 3,
        /// <summary>
        /// The <see cref="HsFeature"/> is used to track Kilowatt-hours.
        /// </summary>
        Kwh      = 4,    
        /// <summary>
        /// The <see cref="HsFeature"/> is used for graphing.
        /// </summary>
        Graphing = 5        

    }
}