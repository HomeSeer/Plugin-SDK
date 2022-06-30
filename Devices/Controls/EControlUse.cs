using System;
using HomeSeer.PluginSdk.Devices.Identification;

namespace HomeSeer.PluginSdk.Devices.Controls {

    /// <summary>
    /// Defines what a <see cref="StatusControl"/> is used for.
    /// </summary>
    /// <remarks>
    /// This is primarily used to improve integrations with 3rd-party systems.
    ///  It helps determine how a control is used within the context of the owning <see cref="HsFeature"/>.
    /// </remarks>
    public enum EControlUse {

        /// <summary>
        /// Default use. The control's use is not defined.
        /// </summary>
        NotSpecified    = 0,
        /// <summary>
        /// This control is used to turn something on.
        /// </summary>
        On              = 1,
        /// <summary>
        /// This control is used to turn something off.
        /// </summary>
        Off             = 2,
        /// <summary>
        /// This control is used to adjust the brightness of a light.
        /// </summary>
        Dim             = 3,
        /// <summary>
        /// This control is used as an alternative command to turn something on.
        /// </summary>
        OnAlternate     = 4,
        /// <summary>
        /// This control is used to play media.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        Play            = 5,
        /// <summary>
        /// This control is used to pause media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        Pause           = 6,
        /// <summary>
        /// This control is used to stop media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        Stop            = 7,
        /// <summary>
        /// This control is used to fast-forward media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        Forward         = 8,
        /// <summary>
        /// This control is used to rewind media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        Rewind          = 9,
        /// <summary>
        /// This control is used to repeat media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        Repeat          = 10,
        /// <summary>
        /// This control is used to shuffle the media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        Shuffle         = 11,
        /// <summary>
        /// This control is used to set the desired temperature when heating.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.HeatingSetPoint"/>
        HeatSetPoint    = 12,
        /// <summary>
        /// This control is used to set the desired temperature when cooling.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.CoolingSetPoint"/>
        CoolSetPoint    = 13,
        /// <summary>
        /// This control is used to set the thermostat operation mode to off.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.ModeSet"/>
        /// <seealso cref="EThermostatControlFeatureSubType.OperatingMode"/>
        ThermModeOff    = 14,
        /// <summary>
        /// This control is used to set the thermostat operation mode to heat.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.ModeSet"/>
        /// <seealso cref="EThermostatControlFeatureSubType.OperatingMode"/>
        ThermModeHeat   = 15,
        /// <summary>
        /// This control is used to set the thermostat operation mode to cool.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.ModeSet"/>
        /// <seealso cref="EThermostatControlFeatureSubType.OperatingMode"/>
        ThermModeCool   = 16,
        /// <summary>
        /// This control is used to set the thermostat operation mode to auto.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.ModeSet"/>
        /// <seealso cref="EThermostatControlFeatureSubType.OperatingMode"/>
        ThermModeAuto   = 17,
        /// <summary>
        /// This control is used to lock a door.
        /// </summary>
        DoorLock        = 18,
        /// <summary>
        /// This control is used to unlock a door.
        /// </summary>
        DoorUnLock      = 19,
        /// <summary>
        /// This control is used to set the fan mode for a thermostat to auto.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.FanModeSet"/>
        ThermFanAuto    = 20,
        /// <summary>
        /// This control is used to set the fan mode for a thermostat to on.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.FanModeSet"/>
        ThermFanOn      = 21,
        /// <summary>
        /// This control is used to adjust the color of a feature.
        /// </summary>
        /// <seealso cref="EControlType.ColorPicker"/>
        ColorControl    = 22,
        /// <summary>
        /// This control is used to adjust the rate for a fan.
        /// </summary>
        DimFan          = 23,
        /// <summary>
        /// Used for integrations to represent a status indicating active motion.
        /// </summary>
        /// <remarks>
        /// This is a carry over from the legacy API. It is out of place; as <see cref="EControlUse"/> defines the
        ///  use of a control and not the use of a status.
        /// </remarks>
        [Obsolete("This will be moved to a new enum type. Please prepare to change to an EStatusUse type.", false)]
        MotionActive    = 24,
        /// <summary>
        /// Used for integrations to represent a status indicating a motion sensor resetting to idle.
        /// </summary>
        /// <remarks>
        /// This is a carry over from the legacy API. It is out of place; as <see cref="EControlUse"/> defines the
        ///  use of a control and not the use of a status.
        /// </remarks>
        [Obsolete("This will be moved to a new enum type. Please prepare to change to an EStatusUse type.", false)]
        MotionInActive  = 25,
        /// <summary>
        /// Used for integrations to represent a status indicating a contact sensor triggering.
        /// </summary>
        /// <remarks>
        /// This is a carry over from the legacy API. It is out of place; as <see cref="EControlUse"/> defines the
        ///  use of a control and not the use of a status.
        /// </remarks>
        [Obsolete("This will be moved to a new enum type. Please prepare to change to an EStatusUse type.", false)]
        ContactActive   = 26,
        /// <summary>
        /// Used for integrations to represent a status indicating a contact sensor being reset.
        /// </summary>
        /// <remarks>
        /// This is a carry over from the legacy API. It is out of place; as <see cref="EControlUse"/> defines the
        ///  use of a control and not the use of a status.
        /// </remarks>
        [Obsolete("This will be moved to a new enum type. Please prepare to change to an EStatusUse type.", false)]
        ContactInActive = 27,
        /// <summary>
        /// This control is used to mute media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        Mute            = 28,
        /// <summary>
        /// This control is used to un-mute media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        UnMute          = 29,
        /// <summary>
        /// This control is used to mute or un-mute media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        MuteToggle      = 30,
        /// <summary>
        /// This control is used to advance to the next track available for playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        Next            = 31,
        /// <summary>
        /// This control is used to select the previous track for media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        Previous        = 32,
        /// <summary>
        /// This control is used to adjust the volume of media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        Volume          = 33,
        /// <summary>
        /// This control is used to increment the value of a feature.
        /// </summary>
        /// <remarks>This can be used for example for thermostat setpoints or media volume</remarks>
        IncrementValue = 34,
        /// <summary>
        /// This control is used to decrement the value of a feature.
        /// </summary>
        /// <remarks>This can be used for example for thermostat setpoints or media volume</remarks>
        DecrementValue = 35

    }

}