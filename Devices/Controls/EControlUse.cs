using System;
using HomeSeer.PluginSdk.Devices.Identification;

namespace HomeSeer.PluginSdk.Devices.Controls {
    /// <summary>
    /// ScaleAttribute applies a string field Label
    /// to Enums.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ScaleAttribute : Attribute {
        /// <summary>
        /// Add the Scale Attribute as a String
        /// </summary>
        public string Scale { get; }
        /// <summary>
        /// The ScaleAttribute using Scale
        /// </summary>
        /// <param name="scale"></param>
        public ScaleAttribute(string scale) {
            Scale = scale;
        }
    }
    /// <summary>
    /// ScaleHtmlAttribute applies a string field Label
    /// to Enums.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ScaleHtmlAttribute : Attribute {
        /// <summary>
        /// Add the ScaleHtml Attribute as a String
        /// </summary>
        public string ScaleHtml { get; }
        /// <summary>
        /// The ScaleAttribute using Scale
        /// </summary>
        /// <param name="scalehtml"></param>
        public ScaleHtmlAttribute(string scalehtml) {
            ScaleHtml = scalehtml;
        }
    }

    /// <summary>
    /// LabelAttribute applies a string field Label
    /// to Enums.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelAttribute : Attribute {
        /// <summary>
        /// Add the Label Attribute as a String
        /// </summary>
        public string Label { get; }
        /// <summary>
        /// The LabelAttribute using Label
        /// </summary>
        /// <param name="label"></param>
        public LabelAttribute(string label) {
            Label = label;
        }
    }

    /// <summary>
    /// Extensions which can be used to retrieve property values
    /// encoded into Enums
    /// </summary>
    public static class EnumExtensionsControlUse {
        /// <summary>
        /// GetLabel retrieves the value of the Label property
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetLabel(this Enum enumValue) {
            try {
                var type = enumValue.GetType();
                var fieldInfo = type.GetField(enumValue.ToString());
                var attributes = (LabelAttribute[])fieldInfo.GetCustomAttributes(typeof(LabelAttribute), false);
                if (attributes != null && attributes.Length > 0) {
                    return attributes[0].Label;
                }
                return "";
            }
            catch (Exception) {
                return "";
            }
        }
        /// <summary>
        /// GetScale retrieves the value of the Scale property
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetScale(this Enum enumValue) {
            try {
                var type = enumValue.GetType();
                var fieldInfo = type.GetField(enumValue.ToString());
                var attributes = (ScaleAttribute[])fieldInfo.GetCustomAttributes(typeof(ScaleAttribute), false);
                if (attributes != null && attributes.Length > 0) {
                    return attributes[0].Scale;
                }
                return "";
            }
            catch (Exception) {
                return "";
            }
        }
        /// <summary>
        /// GetScaleHTML retrieves the value of the ScaleHTML property
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetScaleHTML(this Enum enumValue) {
            try {
                var type = enumValue.GetType();
                var fieldInfo = type.GetField(enumValue.ToString());
                var attributes = (ScaleHtmlAttribute[])fieldInfo.GetCustomAttributes(typeof(ScaleHtmlAttribute), false);
                if (attributes != null && attributes.Length > 0) {
                    return attributes[0].ScaleHtml;
                }
                return "";
            }
            catch (Exception) {
                return "";
            }
        }
    }

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
        [Label("(Use Not Specified)")]
        [Scale("")]
        [ScaleHtml("")]
        NotSpecified = 0,
        /// <summary>
        /// This control is used to turn something on.
        /// </summary>
        [Label("On")]
        [Scale("")]
        [ScaleHtml("")]
        On = 1,
        /// <summary>
        /// This control is used to turn something off.
        /// </summary>
        [Label("Off")]
        [Scale("")]
        [ScaleHtml("")]
        Off = 2,
        /// <summary>
        /// This control is used to adjust the brightness of a light.
        /// </summary>
        [Label("Dim")]
        [Scale("")]
        [ScaleHtml("")]
        Dim = 3,
        /// <summary>
        /// This control is used as an alternative command to turn something on.
        /// </summary>
        [Label("On Alternate")]
        [Scale("")]
        [ScaleHtml("")]
        OnAlternate = 4,
        /// <summary>
        /// This control is used to play media.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Play")]
        [Scale("")]
        [ScaleHtml("")]
        Play = 5,
        /// <summary>
        /// This control is used to pause media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Pause")]
        [Scale("")]
        [ScaleHtml("")]
        Pause = 6,
        /// <summary>
        /// This control is used to stop media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Stop")]
        [Scale("")]
        [ScaleHtml("")]
        Stop = 7,
        /// <summary>
        /// This control is used to fast-forward media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Forward")]
        [Scale("")]
        [ScaleHtml("")]
        Forward = 8,
        /// <summary>
        /// This control is used to rewind media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Rewind")]
        [Scale("")]
        [ScaleHtml("")]
        Rewind = 9,
        /// <summary>
        /// This control is used to repeat media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Repeat")]
        [Scale("")]
        [ScaleHtml("")]
        Repeat = 10,
        /// <summary>
        /// This control is used to shuffle the media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Shuffle")]
        [Scale("")]
        [ScaleHtml("")]
        Shuffle = 11,
        /// <summary>
        /// This control is used to set the desired temperature when heating.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.HeatingSetPoint"/>
        [Label("Heat Setpoint")]
        [Scale("")]
        [ScaleHtml("")]
        HeatSetPoint = 12,
        /// <summary>
        /// This control is used to set the desired temperature when cooling.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.CoolingSetPoint"/>
        [Label("Cool Setpoint")]
        [Scale("")]
        [ScaleHtml("")]
        CoolSetPoint = 13,
        /// <summary>
        /// This control is used to set the thermostat operation mode to off.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.ModeSet"/>
        /// <seealso cref="EThermostatControlFeatureSubType.OperatingMode"/>
        [Label("Thermostat Off")]
        [Scale("")]
        [ScaleHtml("")]
        ThermModeOff = 14,
        /// <summary>
        /// This control is used to set the thermostat operation mode to heat.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.ModeSet"/>
        /// <seealso cref="EThermostatControlFeatureSubType.OperatingMode"/>
        [Label("Thermostat Heat")]
        [Scale("")]
        [ScaleHtml("")]
        ThermModeHeat = 15,
        /// <summary>
        /// This control is used to set the thermostat operation mode to cool.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.ModeSet"/>
        /// <seealso cref="EThermostatControlFeatureSubType.OperatingMode"/>
        [Label("Thermostat Cool")]
        [Scale("")]
        [ScaleHtml("")]
        ThermModeCool = 16,
        /// <summary>
        /// This control is used to set the thermostat operation mode to auto.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.ModeSet"/>
        /// <seealso cref="EThermostatControlFeatureSubType.OperatingMode"/>
        [Label("Thermostat Auto")]
        [Scale("")]
        [ScaleHtml("")]
        ThermModeAuto = 17,
        /// <summary>
        /// This control is used to lock a door.
        /// </summary>
        [Label("Door Lock")]
        [Scale("")]
        [ScaleHtml("")]
        DoorLock = 18,
        /// <summary>
        /// This control is used to unlock a door.
        /// </summary>
        [Label("Door Unlock")]
        [Scale("")]
        [ScaleHtml("")]
        DoorUnLock = 19,
        /// <summary>
        /// This control is used to set the fan mode for a thermostat to auto.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.FanModeSet"/>
        [Label("Thermostat Fan Auto")]
        [Scale("")]
        [ScaleHtml("")]
        ThermFanAuto = 20,
        /// <summary>
        /// This control is used to set the fan mode for a thermostat to on.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.FanModeSet"/>
        [Label("Thermostat Fan On")]
        [Scale("")]
        [ScaleHtml("")]
        ThermFanOn = 21,
        /// <summary>
        /// This control is used to adjust the color of a feature.
        /// </summary>
        /// <seealso cref="EControlType.ColorPicker"/>
        [Label("Color Control")]
        [Scale("")]
        [ScaleHtml("")]
        ColorControl = 22,
        /// <summary>
        /// This control is used to adjust the rate for a fan.
        /// </summary>
        [Label("Fan Speed")]
        [Scale("")]
        [ScaleHtml("")]
        DimFan = 23,
        /// <summary>
        /// This control is used to set playback to Normal (no more shuffle).
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Shuffle Off")]
        [Scale("")]
        [ScaleHtml("")]
        ShuffleOff = 24,
        /// <summary>
        /// This control is used to set playback to Normal (no more repeat).
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Repeat Off")]
        [Scale("")]
        [ScaleHtml("")]
        RepeatOff = 25,
        /// <summary>
        /// This control is used to play media from the beginning.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Start Over")]
        [Scale("")]
        [ScaleHtml("")]
        StartOver = 26,
        /// <summary>
        /// This control is used to mute media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Mute")]
        [Scale("")]
        [ScaleHtml("")]
        Mute = 28,
        /// <summary>
        /// This control is used to un-mute media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Unmute")]
        [Scale("")]
        [ScaleHtml("")]
        UnMute = 29,
        /// <summary>
        /// This control is used to mute or un-mute media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Mute Toggle")]
        [Scale("")]
        [ScaleHtml("")]
        MuteToggle = 30,
        /// <summary>
        /// This control is used to advance to the next track available for playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Next")]
        [Scale("")]
        [ScaleHtml("")]
        Next = 31,
        /// <summary>
        /// This control is used to select the previous track for media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Previous")]
        [Scale("")]
        [ScaleHtml("")]
        Previous = 32,
        /// <summary>
        /// This control is used to adjust the volume of media playback.
        /// </summary>
        /// <remarks>This is for media devices</remarks>
        /// <seealso cref="EMediaFeatureSubType"/>
        [Label("Volume")]
        [Scale("")]
        [ScaleHtml("")]
        Volume = 33,        
        /// <summary>
        /// This control is used to increment the value of a feature.
        /// </summary>
        /// <remarks>This can be used for example for thermostat setpoints or media volume</remarks>
        [Label("Increment")]
        [Scale("")]
        [ScaleHtml("")]
        IncrementValue = 34,
        /// <summary>
        /// This control is used to decrement the value of a feature.
        /// </summary>
        /// <remarks>This can be used for example for thermostat setpoints or media volume</remarks>
        [Label("Decrement")]
        [Scale("")]
        [ScaleHtml("")]
        DecrementValue = 35,  
        /// <summary>
        /// This control is used to open a window covering such as a curtain, blind, or shade.
        /// </summary>
        /// <remarks>This can be used for anything that you prefer to say open/close instead of on/off with.</remarks>
        [Label("Window Covering Open")]
        [Scale("")]
        [ScaleHtml("")]
        WindowCoverOpen = 36,
        /// <summary>
        /// This control is used to close a window covering such as a curtain, blind, or shade.
        /// </summary>
        /// <remarks>This can be used for anything that you prefer to say open/close instead of on/off with.</remarks>
        [Label("Window Covering Close")]
        [Scale("")]
        [ScaleHtml("")]
        WindowCoverClose = 37,
        /// <summary>
        /// This control is used to stop movement on a window covering such as a curtain, blind, or shade.
        /// </summary>
        /// <remarks>This can be used for anything that you set in motion with one command and then wish to stop it.</remarks>
        [Label("Window Covering Stop")]
        [Scale("")]
        [ScaleHtml("")]
        WindowCoverStopHold = 38,
        /// <summary>
        /// This control is used to set a window covering such as a curtain, blind, or shade to a preset position.
        /// </summary>
        /// <remarks>This can be used for anything that you wish to set a preset on. The Label property on the 
        /// control set will be used for the preset name in Alexa and (if supported) in Google.</remarks>
        [Label("Window Covering Preset")]
        [Scale("")]
        [ScaleHtml("")]
        WindowCoverPreset = 39,
        /// <summary>
        /// This control is used to set a window covering such as a curtain, blind, or shade to a specific position.
        /// (Like in dimming, this should be the range of values exclusive of open/close, etc. that are defined elsewhere)
        /// </summary>
        /// <remarks>Example: Set the master bedroom shade to 67%</remarks>
        [Label("Window Covering Move To")]
        [Scale("")]
        [ScaleHtml("")]
        WindowCoverMoveTo = 40,
        /// <summary>
        /// This control is used to tilt a window blind to a specific position.
        /// (Like in dimming, this should be the range of values 1 to 99 percent.)
        /// </summary>
        /// <remarks>Example: Tilt the master bedroom shade to 67%</remarks>
        [Label("Window Covering Tilt")]
        [Scale("")]
        [ScaleHtml("")]
        WindowCoverTiltValue = 41,
        /// <summary>
        /// This control is used to tilt a window blind to a specific angular position.
        /// </summary>
        /// <remarks>Example: Tilt the master bedroom blind to 30 degrees. (Or -30 degrees)</remarks>
        [Label("Window Covering Tilt Angle")]
        [Scale("")]
        [ScaleHtml("")]
        WindowCoverTiltAngle = 42,
        /// <summary>
        /// This control is used to Open a Garage Door.
        /// This is different from a generic Open/Close because a garage door may have security.
        /// </summary>
        [Label("Garage Door Open")]
        [Scale("")]
        [ScaleHtml("")]
        GarageDoorOpen = 43,
        /// <summary>
        /// This control is used to Close a Garage Door.
        /// This is different from a generic Open/Close because a garage door may have security.
        /// </summary>
        [Label("Garage Door Close")]
        [Scale("")]
        [ScaleHtml("")]
        GarageDoorClose = 44,

        /// <summary>
        /// This control is used to Open a Garage Door.
        /// This is different from a generic Open/Close because a garage door may have security.
        /// </summary>
        [Label("Generic Device Open")]
        [Scale("")]
        [ScaleHtml("")]
        GenericOpen = 45,
        /// <summary>
        /// This control is used to Close a Garage Door.
        /// This is different from a generic Open/Close because a garage door may have security.
        /// </summary>
        [Label("Generic Device Close")]
        [Scale("")]
        [ScaleHtml("")]
        GenericClose = 46,

        /// <summary>
        /// This control is used to set the color temperature value.
        /// </summary>
        /// <remarks>This is for color temperature devices</remarks>
        /// <seealso cref="EControlUse.ColorControl"/>
        [Label("Color Temp")]
        [Scale("K")]
        [ScaleHtml("K")]
        ColorTemp = 50,

        /// <summary>
        /// This control is used to set the color temperature to a preset value.
        /// </summary>
        /// <remarks>This is for color temperature devices</remarks>
        /// <seealso cref="EControlUse.ColorControl"/>
        [Label("Color Temp Preset")]
        [Scale("K")]
        [ScaleHtml("K")]
        ColorTempPreset = 51,


        /// <summary>
        /// This control is used to set the fan mode for a thermostat to Circulate.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.FanModeSet"/>
        [Label("Thermostat Fan Circulate")]
        [Scale("")]
        [ScaleHtml("")]
        ThermFanCirculate = 55,

        // 56 Open

        /// <summary>
        /// This control is used to set the thermostat operation mode to economy.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.ModeSet"/>
        /// <seealso cref="EThermostatControlFeatureSubType.OperatingMode"/>
        [Label("Thermostat Economy")]
        [Scale("")]
        [ScaleHtml("")]
        ThermModeEco = 57,

        /// <summary>
        /// This control is used to set the thermostat operation mode to emergency heat.
        /// </summary>
        /// <remarks>This is for thermostat devices</remarks>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType.ModeSet"/>
        /// <seealso cref="EThermostatControlFeatureSubType.OperatingMode"/>
        [Label("Thermostat EM Heat")]
        [Scale("")]
        [ScaleHtml("")]
        ThermModeEMHeat = 58,

        /// <summary>
        /// Used to indicate an "active" state for a sensor without any other
        /// descriptive unit of measure or identifying state name.
        /// </summary>
        /// <remarks>
        /// Note: Should be represented as just "Active"
        /// </remarks>
        [Label("Binary Active")]
        [Scale("")]
        [ScaleHtml("")]
        GenericBinaryActive = 60,

        /// <summary>
        /// Used to indicate an "inactive" state for a sensor without any other
        /// descriptive unit of measure or identifying state name.
        /// </summary>
        /// <remarks>
        /// Note: Should be represented as just "Inactive"
        /// </remarks>
        [Label("Binary Inactive")]
        [Scale("")]
        [ScaleHtml("")]
        GenericBinaryInactive = 61,


        ///
        /// =========================================================================================================================
        /// =========================================================================================================================
        /// 
        ///        Status Use types start here
        ///        
        /// =========================================================================================================================
        /// =========================================================================================================================



        /// =======================================================================================================================
        ///
        ///  Following are some multilevel (e.g. range) sensor types.
        ///

        /// <summary>
        /// A multi-level sensor (e.g. range of values)
        /// </summary>
        /// <remarks>
        /// There is no set unit of measure associated with this value - it is represented as the number value only.
        /// </remarks>
        [Label("General Range")]
        [Scale("")]
        [ScaleHtml("")]
        GeneralNoUnit = 1001,

        /// <summary>
        /// A multi-level value that is represented as a percent. 
        /// </summary>
        /// <remarks>
        /// Generally, but not always, this would be in the 0-100 range. (e.g. you
        /// can give 110%!)
        /// </remarks>
        [Label("Percent")]
        [Scale("%")]
        [ScaleHtml("&#37;")]
        GeneralPercent = 1002,

        /// <summary>
        /// Used by Alexa in routines for things like central scene 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        [Label("Central Scene Action")]
        [Scale("")]
        [ScaleHtml("")]
        SimpleEventSource = 1003,


        /// =======================================================================================================================
        /// 
        /// The following 4 are binary types moved over from EControlUse
        /// 

        /// <summary>
        /// Used for integrations to represent a status indicating active motion.
        /// </summary>
        /// <remarks>
        /// This is a carry over from the legacy API. It is out of place; as <see cref="EControlUse"/> defines the
        ///  use of a control and not the use of a status.
        ///  MAINTAINING VALUE OF 24 FROM WHEN IT WAS PART OF EControlUse!
        /// </remarks>
        [Label("Motion")]
        [Scale("")]
        [ScaleHtml("")]
        MotionActive = 1024,

        /// <summary>
        /// Used for integrations to represent a status indicating a motion sensor resetting to idle.
        /// </summary>
        /// <remarks>
        /// This is a carry over from the legacy API. It is out of place; as <see cref="EControlUse"/> defines the
        ///  use of a control and not the use of a status.
        ///  MAINTAINING VALUE OF 25 FROM WHEN IT WAS PART OF EControlUse!
        /// </remarks>
        [Label("No Motion")]
        [Scale("")]
        [ScaleHtml("")]
        MotionInActive = 1025,

        /// <summary>
        /// Used for integrations to represent a status indicating a contact sensor triggering.
        /// </summary>
        /// <remarks>
        /// This is a carry over from the legacy API. It is out of place; as <see cref="EControlUse"/> defines the
        ///  use of a control and not the use of a status.
        ///  MAINTAINING VALUE OF 26 FROM WHEN IT WAS PART OF EControlUse!
        /// </remarks>
        [Label("Contact")]
        [Scale("")]
        [ScaleHtml("")]
        ContactActive = 1026,

        /// <summary>
        /// Used for integrations to represent a status indicating a contact sensor being reset.
        /// </summary>
        /// <remarks>
        /// This is a carry over from the legacy API. It is out of place; as <see cref="EControlUse"/> defines the
        ///  use of a control and not the use of a status.
        ///  MAINTAINING VALUE OF 27 FROM WHEN IT WAS PART OF EControlUse!
        /// </remarks>
        [Label("No Contact")]
        [Scale("")]
        [ScaleHtml("")]
        ContactInActive = 1027,

        /// =======================================================================================================================

        /// <summary>
        /// Used for integrations to represent a status indicating a Temperature sensor. This does not imply a
        /// unit of measure (F/C/K) so in most cases it will be reported as just degrees.
        /// </summary>
        /// <remarks>
        /// When a status needs to be represented in a temperature scale (e.g. degrees) but cannot be controlled, use this.
        /// </remarks>
        [Label("Temperature")]
        [Scale("°")]
        [ScaleHtml("&deg;")]
        Temperature = 1150,

        /// <summary>
        /// Uses to indicate temperature using the Fahrenheit scale.
        /// </summary>
        /// <remarks>
        /// In Z-Wave, there are separate sensor types for air or water and other elements, but 
        /// these are not individually represented because the HomeSeer device can indicate what
        /// is being measured. 
        /// </remarks>
        [Label("Temperature Fahrenheit")]
        [Scale("°F")]
        [ScaleHtml("&deg;F")]
        TemperatureF = 1151,

        /// <summary>
        /// Uses to indicate temperature using the Celsius scale.
        /// </summary>
        /// <remarks>
        /// In Z-Wave, there are separate sensor types for air or water and other elements, but 
        /// these are not individually represented because the HomeSeer device can indicate what
        /// is being measured. 
        /// </remarks>
        [Label("Temperature Celsius")]
        [Scale("°C")]
        [ScaleHtml("&deg;C")]
        TemperatureC = 1152,

        /// <summary>
        /// Uses to indicate temperature using the Kelvin scale.
        /// </summary>
        /// <remarks>
        /// In Z-Wave, there are separate sensor types for air or water and other elements, but 
        /// these are not individually represented because the HomeSeer device can indicate what
        /// is being measured. This scale is rarely used in the home, but it completes the 
        /// trifecta and allows for scientific instruments.
        /// </remarks>
        [Label("Temperature Kelvin")]
        [Scale("°K")]
        [ScaleHtml("&deg;K")]
        TemperatureK = 1153,

        /// <summary>
        /// Uses to indicate relative humidity in percent.
        /// </summary>
        [Label("Humidity Percent")]
        [Scale("%")]
        [ScaleHtml("&#37;")]
        PercentHumidity = 1154,

        /// =======================================================================================================================

        /// <summary>
        /// A sensor state indicating active.
        /// </summary>
        [Label("Active")]
        [Scale("")]
        [ScaleHtml("")]
        SensorBinaryActive = 1602,

        /// <summary>
        /// A sensor state indicating that Carbon Monoxide is no longer/not present.
        /// </summary>
        /// <remarks>
        /// Note: Suggested representation as: "CO" (or Carbon Monoxide) "Not Detected" (or "No Longer Detected")
        /// </remarks>
        [Label("Inactive")]
        [Scale("")]
        [ScaleHtml("")]
        SensorBinaryInactive = 1603,

        /// <summary>
        /// A media playing state.
        /// </summary>
        /// <remarks>
        /// Note: One of 3 states supported by Alexa")
        /// </remarks>
        [Label("Playing")]
        [Scale("")]
        [ScaleHtml("")]
        MediaPlaying = 1700,
        /// <summary>
        /// A media playing state.
        /// </summary>
        /// <remarks>
        /// Note: One of 3 states supported by Alexa")
        /// </remarks>
        [Label("Paused")]
        [Scale("")]
        [ScaleHtml("")]
        MediaPaused = 1705,
        /// <summary>
        /// A media playing state.
        /// </summary>
        /// <remarks>
        /// Note: One of 3 states supported by Alexa")
        /// </remarks>
        [Label("Stopped")]
        [Scale("")]
        [ScaleHtml("")]
        MediaStopped = 1710,

        /// <summary>
        /// A volume muted state.
        /// </summary>
        [Label("Muted")]
        [Scale("")]
        [ScaleHtml("")]
        MediaMuted = 1715,

        /// <summary>
        /// A volume unmuted state.
        /// </summary>
        [Label("Unmuted")]
        [Scale("")]
        [ScaleHtml("")]
        MediaUnmuted = 1720

    }
}