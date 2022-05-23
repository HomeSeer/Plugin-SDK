namespace HomeSeer.PluginSdk.Devices.Identification {

    /// <summary>
    /// The specific use of a <see cref="EFeatureType.Generic"/> <see cref="HsFeature"/>
    /// </summary>
    /// <remarks>
    /// This is not stable and may see significant change.
    /// </remarks>
    /// <seealso cref="TypeInfo.SubType"/>
    /// <seealso cref="EFeatureType.Generic"/>
    public enum EGenericFeatureSubType {

        /// <summary>
        /// The <see cref="HsFeature"/> represents the status of a battery.
        /// </summary>
        /// <remarks>
        /// In the grid view on HomeSeer, this <see cref="HsFeature"/> should be displayed as a battery icon in the
        ///  status bar of the <see cref="HsDevice"/>.
        /// </remarks>
        Battery,
        /// <summary>
        /// The <see cref="HsFeature"/> represents a Z-Wave Central Scene. This is typically used to display the last
        ///  action taken on the device.
        /// </summary>
        /// <remarks>
        /// <para>In the grid view on HomeSeer, this <see cref="HsFeature"/> should be displayed as an icon in the
        ///  status bar of the <see cref="HsDevice"/>.</para>
        /// <para>An HSWD200+ will report a Central Scene of "Top Paddle Tapped" after a user
        ///  presses the top paddle. It will not change until another action is taken on the device.</para>
        /// </remarks>
        CentralScene,
        /// <summary>
        /// The <see cref="HsFeature"/> represents a binary control - something that only has on and off controls.
        /// </summary>
        BinaryControl,
        /// <summary>
        /// The <see cref="HsFeature"/> represents a binary sensor - something that only reports one of two states.
        /// </summary>
        BinarySensor

    }

}