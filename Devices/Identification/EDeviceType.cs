namespace HomeSeer.PluginSdk.Devices.Identification {

    /// <summary>
    /// The primary use of a <see cref="HsDevice"/>. This describes the physical hardware.
    /// </summary>
    /// <remarks>
    /// Where <see cref="EFeatureType"/> describes the function of a specific <see cref="HsFeature"/> of a
    ///  <see cref="HsDevice"/>, <see cref="EDeviceType"/> describes the overall use of the hardware.
    ///  IE an in-wall paddle switch is described as a <see cref="Switch"/> <see cref="HsDevice"/> with a
    ///  <see cref="EFeatureType.Generic"/> <see cref="HsFeature"/>.
    /// </remarks>
    /// <seealso cref="TypeInfo.Type"/>
    public enum EDeviceType {

        /// <summary>
        /// A generic <see cref="HsDevice"/>. There is no specific use for it.
        /// </summary>
        Generic = 0,
        /// <summary>
        /// The <see cref="HsDevice"/> represents a door.
        /// </summary>
        Door = 1,
        /// <summary>
        /// The <see cref="HsDevice"/> represents a fan.
        /// </summary>
        Fan = 2,
        /// <summary>
        /// The <see cref="HsDevice"/> represents a light.
        /// </summary>
        Light = 3,
        /// <summary>
        /// The <see cref="HsDevice"/> represents a lock.
        /// </summary>
        Lock = 4,
        /// <summary>
        /// The <see cref="HsDevice"/> represents an outlet.
        /// </summary>
        Outlet = 5,
        /// <summary>
        /// The <see cref="HsDevice"/> represents an external controller or remote like a hub.
        /// </summary>
        RemoteControl = 6,
        /// <summary>
        /// The <see cref="HsDevice"/> represents a switch.
        /// </summary>
        /// <remarks>
        /// This should be used instead of <see cref="Light"/> or <see cref="Fan"/> when a more generic type is needed.
        /// </remarks>
        Switch = 7,
        /// <summary>
        /// The <see cref="HsDevice"/> represents a thermostat.
        /// </summary>
        Thermostat,
        /// <summary>
        /// The <see cref="HsDevice"/> represents a window.
        /// </summary>
        Window,
        /// <summary>
        /// The <see cref="HsDevice"/> represents a media device.
        /// </summary>
        Media

    }

}