using HomeSeer.PluginSdk.Devices.Controls;

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// Various bit flags available to add to <see cref="AbstractHsDevice"/>'s <see cref="AbstractHsDevice.Misc"/> field
    ///  that control its behavior on the HomeSeer platform.
    /// </summary>
    public enum EMiscFlag : uint {
            // for device class misc flags, bits in long value
            // PRESET_DIM = 1         ' supports preset dim if set
            // EXTENDED_DIM = 2            ' extended dim command
            // SMART_LINC = 4         ' smart linc switch
            
            /// <summary>
            /// Do not log messages pertaining to this device/feature
            /// </summary>
            NoLog = 8,
            /// <summary>
            /// Indicates that the device/feature does not have any controls and is only used to present a status
            /// </summary>
            StatusOnly = 0x10,
            /// <summary>
            /// Hide the device/feature from all views
            /// </summary>
            Hidden = 0x20,
            /// <summary>
            /// Indicates that the device/feature does not have any <see cref="StatusControl"/>s or <see cref="StatusGraphic"/>s
            ///  configured that map values to status strings.
            /// </summary>
            DeviceNoStatus = 0x40,
            /// <summary>
            /// if set, device's state is restored if power fail enabled
            /// </summary>
            IncludePowerfail = 0x80,
            /// <summary>
            /// Indicates that the device/feature should show its available controls in the UI
            /// This is set by default when creating a <see cref="HsDevice"/> or <see cref="HsFeature"/>
            /// </summary>
            ShowValues = 0x100,
            /// <summary>
            /// set=create a voice command for this device
            /// </summary>
            AutoVoiceCommand = 0x200,
            /// <summary>
            /// set=confirm voice command
            /// </summary>
            VoiceCommandConfirm = 0x400,
            /// <summary>
            /// if set, a change of this device will be sent to MYHS through the tunnel
            /// </summary>
            MyhsDeviceChangeNotify = 0x800,
            /// <summary>
            /// If set, any set to a device value will not reset last change.
            /// This is set by default when creating a <see cref="HsFeature"/> using <see cref="FeatureFactory"/>
            /// </summary>
            SetDoesNotChangeLastChange = 0x1000,
            /// <summary>
            /// Device controls a lighting device (used by Alexa)
            /// </summary>
            IsLight = 0x2000,
            /// <summary>
            /// Indicates that the device/feature is a dimmer switch (used for 3rd party integration)
            /// </summary>
            CanDim = 0x4000,
            /// <summary>
            /// Indicates that the device/feature is not available for selection in any device status change
            ///  event triggers/conditions.
            /// </summary>
            NoStatusTrigger = 0x20000,
            /// <summary>
            /// Indicates that the device/feature does not display any graphics for the status even if
            ///  <see cref="StatusGraphic"/>s are configured
            /// </summary>
            NoGraphicsDisplay = 0x40000,
            /// <summary>
            /// Indicates that the device/feature does not display any status text even if it is set.
            /// </summary>
            NoStatusDisplay = 0x80000,
            
            //CONTROL_POPUP = 0x100000,   // The controls for this device should appear in a popup window on the device utility page.
            
            /// <summary>
            /// Indicates that the device/feature should not show up in responses from the JSON API
            /// </summary>
            HideInMobile = 0x200000,
            
            // MUSIC_API = &H200000
            // MULTIZONE_API = &H400000
            // SECURITY_API = &H800000
            
            /// <summary>
            /// Expose this device to Google cloud services
            /// </summary>
            GoogleDiscoveryEnabled = 0x400000,
            /// <summary>
            /// Expose this device to Amazon cloud services.
            /// </summary>
            AmazonDiscoveryEnabled = 0x800000,
            /// <summary>
            /// Place holder for future misc flags.
            /// </summary>
            MiscUnused11 = 0x1000000,
            /// <summary>
            /// Place holder for future misc flags.
            /// </summary>
            MiscUnused12 = 0x2000000,
            /// <summary>
            /// Place holder for future misc flags.
            /// </summary>
            MiscUnused13 = 0x4000000,
            /// <summary>
            /// Place holder for future misc flags.
            /// </summary>
            MiscUnused14 = 0x8000000,
            /// <summary>
            /// Place holder for future misc flags.
            /// </summary>
            MiscUnused15 = 0x10000000,
            /// <summary>
            /// Place holder for future misc flags.
            /// </summary>
            MiscUnused16 = 0x20000000,
            /// <summary>
            /// Place holder for future misc flags.
            /// </summary>
            MiscUnused17 = 0x40000000
    }

}