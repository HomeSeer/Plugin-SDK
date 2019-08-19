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
    
        public enum CAPIControlButtonImage {
            Not_Specified = 0,
            Use_Status_Value = 1,
            Use_Custom = 2
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

        public enum EOsType {
            Windows = 0,
            Linux   = 1
        }

        [Serializable]
        public enum Device_Constants {

            // MAX_VIRTUAL = 11 ' q->z
            // 65-90 = A-Z
            FIRST_HC = 65, // 35 ' "#"
            // MAX_IO_CODES = 36   ' Actually 26 after the 10 digits (0-9) are subtracted.
            MAX_DEVICE_CODES = 999,
            // FIRST_IO_CODE = 65 '&H5B ' "["
            // IO_WRAP = &H61 ' "a"   wrap to #
            MAX_HOUSE_CODES = 26 // 26 + Device_Constants.MAX_IO_CODES 

        }

    }

}