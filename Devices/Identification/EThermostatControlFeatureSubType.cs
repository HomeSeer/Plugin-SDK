using HomeSeer.PluginSdk.Devices.Controls;

namespace HomeSeer.PluginSdk.Devices.Identification {

    /// <summary>
    /// The specific use of a <see cref="EFeatureType.ThermostatControl"/> <see cref="HsFeature"/>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is used to render Thermostat specific UI components and for integration with systems like Google Home, Amazon, and IFTTT.
    /// </para>
    /// <para>
    /// There is some overlap with <see cref="EControlUse"/>. To ensure compatibility in all contexts you should make sure
    ///  both the <see cref="HsFeature"/> and the appropriate <see cref="StatusControl"/>s of that <see cref="HsFeature"/>
    ///  are set with a matching <see cref="EThermostatControlFeatureSubType"/> and <see cref="EControlUse"/>.
    ///  Some systems analyze the <see cref="HsFeature"/> to determine how it is used and some analyze the <see cref="StatusControl"/>.
    /// </para>
    /// <para>
    /// Not all thermostats are the same and most do not utilize all of these features. Use only the ones you need.
    /// </para>
    /// </remarks>
    public enum EThermostatControlFeatureSubType {
        //JLW TODO Finish documenting EThermostatControlFeatureSubType members
        
        /// <summary>
        /// This <see cref="HsFeature"/> is used to control the desired temperature when the thermostat is heating.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A <see cref="HsFeature"/> designated with this subtype should have <see cref="StatusControl"/>s configured
        ///  with a <see cref="StatusControl.ControlUse"/> of <see cref="EControlUse.HeatSetPoint"/>
        /// </para>
        /// </remarks>
        /// <seealso cref="EControlUse.HeatSetPoint"/>
        HeatingSetPoint = 1,
        /// <summary>
        /// This <see cref="HsFeature"/> is used to control the desired temperature when the thermostat is cooling.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A <see cref="HsFeature"/> designated with this subtype should have <see cref="StatusControl"/>s configured
        ///  with a <see cref="StatusControl.ControlUse"/> of <see cref="EControlUse.CoolSetPoint"/>
        /// </para>
        /// </remarks>
        /// <seealso cref="EControlUse.CoolSetPoint"/>
        CoolingSetPoint = 2,
        /// <summary>
        /// This <see cref="HsFeature"/> is used to control the operation mode of the thermostat.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A <see cref="HsFeature"/> designated with this subtype should have <see cref="StatusControl"/>s configured
        ///  that allow users to set the thermostat mode with a <see cref="StatusControl.ControlUse"/> of
        ///  <see cref="EControlUse.ThermModeOff"/>, <see cref="EControlUse.ThermModeHeat"/>,
        ///  <see cref="EControlUse.ThermModeCool"/>, and <see cref="EControlUse.ThermModeAuto"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="OperatingMode"/>
        ModeSet = 3,
        /// <summary>
        /// This <see cref="HsFeature"/> is used to control the fan mode of the thermostat.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A <see cref="HsFeature"/> designated with this subtype should have <see cref="StatusControl"/>s configured
        ///  that allow users to set the thermostat fan mode with a <see cref="StatusControl.ControlUse"/> of
        ///  <see cref="EControlUse.ThermFanAuto"/> and <see cref="EControlUse.ThermFanOn"/>
        /// </para>
        /// </remarks>
        FanModeSet = 4,
        FurnaceSetPoint = 7,
        DryAirSetPoint = 8,
        MoistAirSetPoint = 5,
        AutoChangeoverSetPoint = 10,
        /// <summary>
        /// This <see cref="HsFeature"/> is used to report the current operating mode of the thermostat.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="OperatingMode"/> is different from <see cref="EThermostatStatusFeatureSubType.OperatingState"/>
        ///  because <see cref="OperatingMode"/> represents the chosen mode (<see cref="EControlUse.ThermModeOff"/>,
        ///  <see cref="EControlUse.ThermModeHeat"/>, <see cref="EControlUse.ThermModeCool"/>, or <see cref="EControlUse.ThermModeAuto"/>)
        ///  while <see cref="EThermostatStatusFeatureSubType.OperatingState"/> represents the active state of that mode.
        /// </para>
        /// <para>
        /// A thermostat set to <see cref="EControlUse.ThermModeAuto"/> can be idle (<see cref="EControlUse.ThermModeOff"/>),
        ///  heating (<see cref="EControlUse.ThermModeHeat"/>), or cooling (<see cref="EControlUse.ThermModeCool"/>).
        /// </para>
        /// <para>A <see cref="HsFeature"/> that is designated with this subtype should have <see cref="StatusGraphic"/>s
        ///  configured to display the different modes that the thermostat can be in.</para>
        /// </remarks>
        OperatingMode = 9, //JLW TODO OperatingMode should be a EThermostatStatusFeatureSubType
        EnergySaveHeat = 11,
        EnergySaveCool = 12,
        HeatingAwaySetPoint = 13,
        HoldMode = 15,
        Setback = 14

    }

}