using HomeSeer.PluginSdk.Devices.Controls;

namespace HomeSeer.PluginSdk.Devices.Identification {

    /// <summary>
    /// The specific use of a <see cref="EFeatureType.ThermostatStatus"/> <see cref="HsFeature"/>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is used to render Thermostat specific UI components and for integration with systems like Google Home, Amazon, and IFTTT.
    /// </para>
    /// <para>
    /// Not all thermostats are the same and most do not utilize all of these features. Use only the ones you need.
    /// </para>
    /// </remarks>
    public enum EThermostatStatusFeatureSubType {

        /// <summary>
        /// This <see cref="HsFeature"/> is used to display the primary temperature being reported by a thermostat
        /// </summary>
        Temperature = 0,
        /// <summary>
        /// This <see cref="HsFeature"/> is used to display a secondary temperature being reported by a thermostat
        /// </summary>
        Temperature2 = 1,
        /// <summary>
        /// This <see cref="HsFeature"/> is used to display additional temperatures being reported by a thermostat
        /// </summary>
        TemperatureOther = 2,
        /// <summary>
        /// This <see cref="HsFeature"/> is used to display the humidity levels being reported by a thermostat
        /// </summary>
        Humidity = 5,
        /// <summary>
        /// This <see cref="HsFeature"/> is used to display the current operating state a thermostat is in
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="EThermostatControlFeatureSubType.OperatingMode"/> is different from <see cref="OperatingState"/>
        ///  because <see cref="EThermostatControlFeatureSubType.OperatingMode"/> represents the chosen mode
        ///  (<see cref="EControlUse.ThermModeOff"/>, <see cref="EControlUse.ThermModeHeat"/>,
        ///  <see cref="EControlUse.ThermModeCool"/>, or <see cref="EControlUse.ThermModeAuto"/>) while
        ///  <see cref="OperatingState"/> represents the active state of that mode.
        /// </para>
        /// <para>
        /// A thermostat set to <see cref="EControlUse.ThermModeAuto"/> can be idle (<see cref="EControlUse.ThermModeOff"/>),
        ///  heating (<see cref="EControlUse.ThermModeHeat"/>), or cooling (<see cref="EControlUse.ThermModeCool"/>).
        /// </para>
        /// </remarks>
        OperatingState = 6,
        /// <summary>
        /// This <see cref="HsFeature"/> is used to display the current state of the fan reported by a thermostat
        /// </summary>
        /// <remarks>
        /// <para>This is typically either OFF or ON</para>
        /// </remarks>
        FanStatus = 7
    }
    
    /*
      Temperature = 0,
      Temperature_1 = 1,
      Other_Temperature = 2,
      _Unused_3 = 3,
      _Unused_4 = 4,
      Humidity = 5
      Operating_State = 1,
      Fan_Status = 5,
      RunTime = 7,
      Additional_Temperature = 10,
      Setback = 11,
      Filter_Remind = 12,
    */

}