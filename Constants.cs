using System;

namespace HomeSeer.PluginSdk {

    public class Constants {
        
        public const int eDeviceType_GenericRoot = 999;

        // editions is the type of software features enabled
        // license ID determines which edition we are running
        public enum editions {
            HS3STANDARD = 1,
            HS3PRO = 2,
            HS3BASIC = 3,
            HS3ZEE = 4,
            HS3ZEEASCII = 5,
            HS3ZEES2 = 6
        }
    
        public enum REGISTRATION_MODES {
            REG_UNKNOWN = 0,
            REG_UNREG = 1,
            REG_TRIAL = 2,
            REG_REGISTERED = 3,
            REG_READY_TO_REGISTER = 4       // for HSPRO, plugin does not have a license and needs to be enabled to get one
        }
    
        public enum eCapabilities {
            // CA_IR = 2
            CA_IO = 4,
            CA_Security = 8,
            CA_Thermostat = 0x10,
            CA_Music = 0x20,
            CA_SourceSwitch = 0x40,
            CA_V2PLUGIN_API = 0x80
        }
    
        public enum eRelationship {
            Not_Set = 0,
            Indeterminate = 1,   // Could not be determined
            Parent_Root = 2,
            Standalone = 3,
            Child = 4
        }
    
        public enum ConfigDevicePostReturn {
            DoneAndSave = 1,
            DoneAndCancel = 2,
            DoneAndCancelAndStay = 3,
            CallbackOnce = 4,
            CallbackTimer = 5
        }
    
        public enum DeviceScriptChange {
            DevValue = 1,
            DevString = 2,
            Both = 3
        }
    
        public enum enumVCMDType {
            Disabled = 0,
            Microphone = 1,
            Telephone = 2,
            Both = 3
        }
    
        public enum dvMISC : uint {
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
    
        public enum eDeviceProperty {
            Ref,
            AdditionalDisplayData,
            Address,
            AssociatedDevices_Add,
            AssociatedDevices_Remove,
            AssociatedDevices_ClearAll,
            Attention,
            Buttons,
            Can_Dim,
            Code,
            Device_Type,
            Device_Type_String,
            Image,
            ImageLarge,
            Interface,
            InterfaceInstance,
            Last_Change,
            Location,
            Location2,
            MISC_Clear,
            MISC_Set,
            Name,
            OLD_Values,
            PlugExtraData,
            Relationship,
            ScriptName,
            ScriptFunc,
            ScaleText,
            Status_Support,
            StringSelected,
            UserNote,
            UserAccess,
            LinkedDevice,
            VoiceCommand
        }
    
        // For HSEvent callbacks
        public enum HSEvent {
            // X10 = 1
            LOG = 2,
            // STATUS_CHANGE = 4       ' a device changed status or value
            AUDIO = 8,
            // X10_TRANSMIT = &H10S
            CONFIG_CHANGE = 0x20,
            STRING_CHANGE = 0x40,
            SPEAKER_CONNECT = 0x80,
            CALLER_ID = 0x100,
            // ZWAVE = &H200
            VALUE_CHANGE = 0x400,      // rjh added in 2.2.0.3
            VALUE_SET = 0x800,           // rjh added in 3.0.0.292
            // STATUS_ONLY_CHANGE = &H800 ' rjh added in 2.2.0.10
            VOICE_REC = 0x1000,
            SETUP_CHANGE = 0x2000,
            RUN_SCRIPT_SPECIAL = 0x4000,
            GENERIC = 0x8000        // RVCT added 3/10/08, 2.2.0.76ish
        }
    
    
        public enum HSDAY {
            Monday = 0x1,
            Tuesday = 0x2,
            Wednesday = 0x4,
            Thursday = 0x8,
            Friday = 0x10,
            Saturday = 0x20,
            Sunday = 0x40,
            All_Days = 0x7F,
            Weekends = 0x60,
            Weekdays = 0x1F
        }
    
        public enum CAPIControlType {
            Not_Specified = 1,
            Values = 2,                          // This is the default to use if one of the others is not specified.
            Single_Text_from_List = 3,
            List_Text_from_List = 4,
            Button = 5,
            ValuesRange = 6,                 // Rendered as a drop-list by default.
            ValuesRangeSlider = 7,
            TextList = 8,
            TextBox_Number = 9,
            TextBox_String = 10,
            Radio_Option = 11,
            Button_Script = 12,      // Rendered as a button, executes a script when activated.
            Color_Picker = 13
        }
    
        public enum CAPIControlButtonImage {
            Not_Specified = 0,
            Use_Status_Value = 1,
            Use_Custom = 2
        }
        
        public enum eDeviceAPI {
            No_API = 0,
            Plug_In = eCapabilities.CA_IO,                    // 4
            Thermostat = eCapabilities.CA_Thermostat,         // 16
            Media = eCapabilities.CA_Music,                   // 32
            Security = eCapabilities.CA_Security,             // 8
            SourceSwitch = eCapabilities.CA_SourceSwitch,     // 64
            Script = 128,
            Energy = 256
        }
        
        public enum eDeviceType_Plugin {
            Root = 999
        }

        public enum eDeviceType_Energy {
            Watts = 1,
            Amps = 2,
            Volts = 3,
            KWH = 4,             // kwh used
            Graphing = 5        // HS graphing device
        }

        public enum eDeviceType_Security {
            Alarm = 1,                           // Alarm status & control (shows alarms that have occurred and can also invoke an alarm - e.g. Duress)
            Arming = 10,                        // Arming status & control (shows the state of the security arming and can set arming state)
            Keypad = 20,                       // Keypad status & control
            Zone_Perimeter = 30,            // A perimeter zone
            Zone_Perimeter_Delay = 31,   // A perimeter zone with a violation alarm delay
            Zone_Interior = 32,               // An interior zone (not normally armed in stay mode)
            Zone_Interior_Delay = 33,      // An interior zone (with a violation alarm delay when armed)
            Zone_Auxiliary = 34,              // An aux zone, not usually included in any arming mode
            Zone_Other = 35,                 // A zone that does not fit any other zone description
            Zone_Safety_Smoke = 40,     // A smoke detector zone (not allowed to be bypassed)
            Zone_Safety_CO = 41,          // A Carbon Monoxide zone (not allowed to be bypassed)
            Zone_Safety_CO2 = 42,         // A Carbon Dioxide zone (not allowed to be bypassed)
            Zone_Safety_Other = 43,       // A zone for some other safety sensor that cannot be bypassed
            Output_Relay = 50,               // A general purpose output relay
            Output_Other = 51,               // A general purpose output (could be virtual as in a 'flag' output)
            Communicator = 60,              // Communicator status and (if available) control
            Siren = 70,                          // Siren output - status usually - control follows alarm state.
            Root = 99,                           // Indicates a root device of a root/child grouping.
        }
        public enum eDeviceType_Media {
            Player_Status = 1,
            Player_Status_Additional = 2,
            Player_Control = 3,
            Player_Volume = 4,
            Player_Shuffle = 5,
            Player_Repeat = 6,
            Media_Genre = 7,
            Media_Album = 8,
            Media_Artist = 9,
            Media_Track = 10,
            Media_Playlist = 11,
            Media_Type = 12,
            Media_Selector_Control = 20, // Used to track which instance of MusicAPI and selection mode (e.g. album, artists, playlists)
            Root = 99                           // Indicates a root device of a root/child grouping.
        }
        public enum eDeviceType_SourceSwitch {
            Invalid = 0,
            System = 1,                  // Indicates system status and/or contains system control capabilities.
            Source = 10,                 // Indicates source status information and/or contains source control capabilities.
            Source_Extended = 15,   // An extension to Source, can be used for less common status or control features.
            Zone = 20,                   // Indicates zone status information and/or contains zone control capabilities.
            Zone_Extended = 25,     // An extension to Zone, can be used for less common status or control features.
            Root = 99                   // The root device of a root/child grouping.
        }
        public enum eDeviceSubType_SecurityArea {
            Invalid = 0,
            PRIMARY = 1,
            Area_Partition_2 = 2,
            Area_Partition_3 = 3,
            Area_Partition_4 = 4,
            Area_Partition_5 = 5,
            Area_Partition_6 = 6,
            Area_Partition_7 = 7,
            Area_Partition_8 = 8,
            Area_Partition_9 = 9
        }
        public enum eDeviceType_Thermostat {
            Operating_State = 1,
            Temperature = 2,
            Mode_Set = 3,
            Fan_Mode_Set = 4,
            Fan_Status = 5,
            Setpoint = 6,
            RunTime = 7,
            Hold_Mode = 8,
            Operating_Mode = 9,
            Additional_Temperature = 10,
            Setback = 11,
            Filter_Remind = 12,
            Root = 99                           // Indicates a root device of a root/child grouping.
        }
        public enum eDeviceSubType_Setpoint {
            Invalid = 0,
            Heating_1 = 1,
            Cooling_1 = 2,
            Furnace = 7,
            Dry_Air = 8,
            Moist_Air = 9,
            Auto_Changeover = 10,
            Energy_Save_Heat = 11,
            Energy_Save_Cool = 12,
            Away_Heating = 13
        }
        public enum eDeviceSubType_Temperature {
            Temperature = 0,
            Temperature_1 = 1,
            Other_Temperature = 2,
            _Unused_3 = 3,
            _Unused_4 = 4,
            Humidity = 5
        }
        public enum eDeviceType_Script {
            Disabled = 0,
            Run_On_Any_Change = 1,
            Run_On_Value_Change = 2,
            Run_On_String_Change = 3
        }
        
        public enum VSVGPairType {
            SingleValue = 1,
            Range       = 2
        }
        
        public enum TunnelCommand {
            Unknown                = 0,
            OpenConnection         = 1,
            CloseConnection        = 2,
            Data                   = 3,
            Ack                    = 4,
            RegisterHS             = 5,
            NULL                   = 6,
            OpenConnectionHStouch  = 7,
            DataHSTouch            = 8,
            CloseConnectionHSTouch = 9,
            ErrorMessage           = 10, // data contains the error message
            ErrorMessageHSTouch = 11,
            CreateHSTouchUser   = 12, // create a user/pass that is the MyHS user pass so HSTouch allows connections with these credentials
            RequestLicID = 13, // tunnel requests license ID from HS after HS connects
            DeviceChange = 14, // HS device has changed value, new value sent to tunned (mainly for IFTTT)
            UseBOISSerialization = 15, // use compact serialization
            DeviceChangeAll = 16
        }
        
        public enum CAPIControlResponse {
            Indeterminate = 0,
            All_Success   = 1,
            Some_Failed   = 2,
            All_Failed    = 3
        }

        public enum enumEnergyDirection {
            Consumed = 1,
            Produced = 2
        }
        
        public enum enumEnergyDevice {
            _Undefined_ = 0,                   // Not defined
            Light_Small = 1,                    // A small light
            Light_Large = 2,                    // A large light or several lights
            Appliance = 10,                     // Any appliance
            Appliance_Small = 11,             // A small appliance such as a toaster
            Appliance_Large = 12,            // A large appliance such as an oven
            Utility = 20,                          // A utility device
            Utility_Small = 21,                  // A small utility device such as a water filter
            Utility_Large = 22,                  // A large utility device such as a well pump
            Entertainment = 30,                // An entertainment device
            Entertainment_Small = 31,       // A small entertainment device such as a radio
            Entertainment_Large = 32,       // A large entertainment device such as a home theatre system
            HVAC = 40,                           // An HVAC device
            Electric_AC = 41,                   // An Air Conditioning device
            Electric_Heat = 42,                 // An electric heating device
            Panel = 51,                           // An electrical panel providing several branches of electrical service to the home.
            Panel_A = 52,                        // 
            Panel_B = 53,                        // 
            Panel_C = 54,                        // 
            Panel_D = 55,                        // 
            Panel_E = 56,                        // 
            Panel_F = 57,                        // 
            Meter = 61,                           // An electric meter measuring usage for an unspecified or general purpose.
            Meter_Service = 62,               // An electric meter measuring usage for electrical service such as a house service entrance.
            Meter_Device = 63,                // An electric meter measuring usage for a single device.
            Generator = 71,                     // An electricity producing generator.
            Solar_Panel = 72,                   // An electricity producing solar panel.
            Wind_Turbine = 73,                // An electricity producing wind turbine.
            Water_Turbine = 74,              // An electricity producing water (wave) turbine.
            Root = 98,                       // The root device for child energy devices.
            Other = 99                          // A device (consumer or producer) that does not fit any other device type.
        }
        
        public enum eGraphType {
            Line,
            Bar,
            Pie
        }
        
        public enum eGraphInterval {
            Minute,
            Hour,
            Day,
            Week,
            Month,
            Quarter,
            Year
        }
        
        public enum speak_error_values {
            SPEAK_NO_ERROR      = 1,
            SPEAK_NO_CLIENTS    = 2,
            SPEAK_ERROR_SENDING = 3
        }

        public enum media_operation_values {
            media_stop       = 1,
            media_pause      = 2,
            media_mute       = 3,
            media_is_playing = 4, // returns true if media player is currently playing
            media_set_volume = 5, // 0 -> 100  100=FULL
            media_get_volume   = 6,
            media_get_filename = 7,
            media_set_filename = 8,
            media_unpause      = 9,
            media_unmute       = 10
        }

        public enum speak_type_values {
            SPEAK_TTS = 1, // TTS voice
            SPEAK_MESSAGE = 2, // Text message
            SPEAK_WAVEFILE = 3 // Play a wave file
        }
        
        public enum CD_DAY_EvenOdd {
            Even = 0,
            Odd  = 1
        }

        public enum CD_DAY_IS_TYPE {
            FIRST  = 1,
            SECOND = 2,
            THIRD  = 3,
            FOURTH = 4,
            LAST   = 5,
            FIFTH  = 6
        }

        public enum ePairStatusControl {
            Status  = 1,
            Control = 2,
            Both    = 3
        }

        public enum ePairControlUse {
            Not_Specified = 0,
            _On           = 1,
            _Off          = 2,
            _Dim          = 3,
            _On_Alternate = 4,
            _Play         = 5, // media control devices
            _Pause        = 6,
            _Stop         = 7,
            _Forward      = 8,
            _Rewind       = 9,
            _Repeat       = 10,
            _Shuffle      = 11,
            _HeatSetPoint = 12, // these are added so we can support IFTTT for now until they change their API
            _CoolSetPoint    = 13,
            _ThermModeOff    = 14,
            _ThermModeHeat   = 15,
            _ThermModeCool   = 16,
            _ThermModeAuto   = 17,
            _DoorLock        = 18,
            _DoorUnLock      = 19,
            _ThermFanAuto    = 20,
            _ThermFanOn      = 21,
            _ColorControl    = 22,
            _DimFan          = 23,
            _MotionActive    = 24,
            _MotionInActive  = 25,
            _ContactActive   = 26,
            _ContactInActive = 27
        }

        public enum eSearchReturn {
            r_String_Other = 0,
            r_URL          = 1,
            r_Object       = 2
        }

        public enum eOSType {
            windows = 0,
            linux   = 1
        }
        
        public enum enumPollResult {
            OK                     = 1,
            Device_Not_Found       = 2,
            Error_Getting_Status   = 3,
            Could_Not_Reach_Plugin = 4,
            Unknown                = 5,
            Timeout_OK             = 6,
            Other_Error            = 7,
            Status_Not_Supported   = 8
        }

        public struct SearchReturn {
            public eSearchReturn RType;
            public string        RDescription;
            public string        RValue;
            public object        RObject;
        }

        public struct Pair {
            public string Name;
            public string Value;
        }

    }

}