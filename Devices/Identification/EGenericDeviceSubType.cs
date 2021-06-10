namespace HomeSeer.PluginSdk.Devices.Identification {

    /// <summary>
    /// The specific use of a <see cref="EDeviceType.Generic"/> <see cref="HsDevice"/>
    /// </summary>
    /// <remarks>
    /// This is not stable and may see significant change. The description offered by <see cref="EDeviceType"/> is
    ///  usually enough.
    /// </remarks>
    /// <seealso cref="TypeInfo.SubType"/>
    /// <seealso cref="EDeviceType.Generic"/>
    public enum EGenericDeviceSubType {
        //JLW This could, theoretically, be exposed for user configuration or removed altogether.
        
        /// <summary>
        /// The <see cref="HsDevice"/> is used to control a light.
        /// </summary>
        Light,
        /// <summary>
        /// The <see cref="HsDevice"/> is used to control a fan.
        /// </summary>
        Fan,
        /// <summary>
        /// The <see cref="HsDevice"/> is used to interact with a sensor.
        /// </summary>
        Sensor,
        /// <summary>
        /// The <see cref="HsDevice"/> is used to interact with an outlet.
        /// </summary>
        Outlet,
        
        /*
        JLW
        Possible sub-types:
         
        DoorSensor,
        WindowSensor,
        MotionSensor,
        EnvironmentSensor
        */

    }

}