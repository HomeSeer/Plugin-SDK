namespace HomeSeer.PluginSdk.Devices {

    public enum EMiscFlag : uint {
            // for device class misc flags, bits in long value
            // PRESET_DIM = 1         ' supports preset dim if set
            // EXTENDED_DIM = 2            ' extended dim command
            // SMART_LINC = 4         ' smart linc switch
            NO_LOG = 8,             // no logging to event log for this device
            STATUS_ONLY = 0x10,     // device cannot be controlled
            HIDDEN = 0x20,          // device is hidden from views
            DEVICE_NO_STATUS = 0x40,           // device does not have any value pairs that support status
            INCLUDE_POWERFAIL = 0x80,      // if set, device's state is restored if power fail enabled
            SHOW_VALUES = 0x100,    // set=display value options in win gui and web status
            AUTO_VOICE_COMMAND = 0x200,        // set=create a voice command for this device
            VOICE_COMMAND_CONFIRM = 0x400,     // set=confirm voice command
            MYHS_DEVICE_CHANGE_NOTIFY = 0x800,        // if set, a change of this device will be sent to MYHS through the tunnel
            SET_DOES_NOT_CHANGE_LAST_CHANGE = 0x1000, // if set, any set to a device value will not reset last change, this is not set by default for backward compatibility
            IS_LIGHT = 0x2000,       // Device controls a lighting device (used by Alexa)
            CanDim = 0x4000,
            // for compatibility with 1.7, the following 2 bits are 0 by default which disables SetDeviceStatus notify and enables SetDeviceValue notify
            // rjh added 1967
            // SETSTATUS_NOTIFY = &H4000  ' if set, SetDeviceStatus calls plugin SetIO (default is 0 or not to notify)
            // SETVALUE_NOTIFY = &H8000   ' if set, SetDeviceValue calls plugin SetIO (default is 0 or to not notify)
            // ON_OFF_ONLY = &H10000      ' if set, device actions are ON and OFF only
            NO_STATUS_TRIGGER = 0x20000,   // If set, device will not appear in the device status change trigger or conditions lists.
            NO_GRAPHICS_DISPLAY = 0x40000,    // this device will not display any graphics for its status, graphics pairs are ignored
            NO_STATUS_DISPLAY = 0x80000,     // if set, no status text will be displayed for a device, will still display any graphic from graphic pairs
            CONTROL_POPUP = 0x100000,   // The controls for this device should appear in a popup window on the device utility page.
            HIDE_IN_MOBILE = 0x200000,
            // MUSIC_API = &H200000
            // MULTIZONE_API = &H400000
            // SECURITY_API = &H800000
            MISC_UNUSED_09 = 0x400000,
            MISC_UNUSED_10 = 0x800000,
            MISC_UNUSED_11 = 0x1000000,
            MISC_UNUSED_12 = 0x2000000,
            MISC_UNUSED_13 = 0x4000000,
            MISC_UNUSED_14 = 0x8000000,
            MISC_UNUSED_15 = 0x10000000,
            MISC_UNUSED_16 = 0x20000000,
            MISC_UNUSED_17 = 0x40000000
        }

}