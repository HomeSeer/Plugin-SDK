using System;

namespace HomeSeer.PluginSdk {

    public class Constants {
        
        [Obsolete("Do not use this Enum. This is only available to aid in upgrading legacy plugins. Use Devices.Identification instead", false)]
        public const int eDeviceType_GenericRoot = 999;
        
        /// <summary>
        /// editions is the type of software features enabled. license ID determines which edition we are running
        /// </summary>
        public enum editions {
            HS3STANDARD = 1,
            HS3PRO = 2,
            HS3BASIC = 3,
            HS3ZEE = 4,
            HS3ZEEASCII = 5,
            HS3ZEES2 = 6
        }
    
        [Obsolete("This will be removed in a future release.  Please use Types.ERegistrationMode as of v1.0.8.1", false)]
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

        [Obsolete("Do not use this Enum. This is only available to aid in upgrading legacy plugins. Use Devices.Identification instead", false)]
        public enum eDeviceType_Energy {
            Watts = 1,
            Amps = 2,
            Volts = 3,
            /// <summary>
            /// kwh used
            /// </summary>
            KWH = 4,
            /// <summary>
            /// HS graphing device
            /// </summary>
            Graphing = 5
        }

        [Obsolete("Do not use this Enum. This is only available to aid in upgrading legacy plugins. Use Devices.Identification instead", false)]
        public enum eDeviceType_Security {
            /// <summary>
            /// Alarm status & control (shows alarms that have occurred and can also invoke an alarm - e.g. Duress)
            /// </summary>
            Alarm = 1,
            /// <summary>
            /// Arming status & control (shows the state of the security arming and can set arming state)
            /// </summary>
            Arming = 10,
            /// <summary>
            /// Keypad status & control
            /// </summary>
            Keypad = 20,
            /// <summary>
            /// A perimeter zone
            /// </summary>
            Zone_Perimeter = 30,
            /// <summary>
            /// A perimeter zone with a violation alarm delay
            /// </summary>
            Zone_Perimeter_Delay = 31,
            /// <summary>
            /// An interior zone (not normally armed in stay mode)
            /// </summary>
            Zone_Interior = 32,
            /// <summary>
            /// An interior zone (with a violation alarm delay when armed)
            /// </summary>
            Zone_Interior_Delay = 33,
            /// <summary>
            /// An aux zone, not usually included in any arming mode
            /// </summary>
            Zone_Auxiliary = 34,
            /// <summary>
            /// A zone that does not fit any other zone description
            /// </summary>
            Zone_Other = 35,
            /// <summary>
            /// A smoke detector zone (not allowed to be bypassed)
            /// </summary>
            Zone_Safety_Smoke = 40,
            /// <summary>
            /// A Carbon Monoxide zone (not allowed to be bypassed)
            /// </summary>
            Zone_Safety_CO = 41,
            /// <summary>
            /// A Carbon Dioxide zone (not allowed to be bypassed)
            /// </summary>
            Zone_Safety_CO2 = 42,
            /// <summary>
            /// A zone for some other safety sensor that cannot be bypassed
            /// </summary>
            Zone_Safety_Other = 43,
            /// <summary>
            /// A general purpose output relay
            /// </summary>
            Output_Relay = 50,
            /// <summary>
            /// A general purpose output (could be virtual as in a 'flag' output)
            /// </summary>
            Output_Other = 51,
            /// <summary>
            /// Communicator status and (if available) control
            /// </summary>
            Communicator = 60,
            /// <summary>
            /// Siren output - status usually - control follows alarm state.
            /// </summary>
            Siren = 70,
            /// <summary>
            /// Indicates a root device of a root/child grouping.
            /// </summary>
            Root = 99,
        }
        
        [Obsolete("Do not use this Enum. This is only available to aid in upgrading legacy plugins. Use Devices.Identification instead", false)]
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
        
        [Obsolete("Do not use this Enum. This is only available to aid in upgrading legacy plugins. Use Devices.Identification instead", false)]
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
            Root = 99
        }
        
        [Obsolete("Do not use this Enum. This is only available to aid in upgrading legacy plugins. Use Devices.Identification instead", false)]
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
        
        [Obsolete("Do not use this Enum. This is only available to aid in upgrading legacy plugins. Use Devices.Identification instead", false)]
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
            /// <summary>
            /// data contains the error message
            /// </summary>
            ErrorMessage           = 10,
            ErrorMessageHSTouch = 11,
            /// <summary>
            /// create a user/pass that is the MyHS user pass so HSTouch allows connections with these credentials
            /// </summary>
            CreateHSTouchUser   = 12,
            /// <summary>
            /// tunnel requests license ID from HS after HS connects
            /// </summary>
            RequestLicID = 13,
            /// <summary>
            /// HS device has changed value, new value sent to tunned (mainly for IFTTT)
            /// </summary>
            DeviceChange = 14,
            /// <summary>
            /// use compact serialization
            /// </summary>
            UseBOISSerialization = 15,
            DeviceChangeAll = 16
        }
        
        public enum CAPIControlResponse {
            Indeterminate = 0,
            All_Success   = 1,
            Some_Failed   = 2,
            All_Failed    = 3
        }

        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        public enum enumEnergyDirection {
            Consumed = 1,
            Produced = 2
        }
        
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        public enum enumEnergyDevice {
            /// <summary>
            /// Not defined
            /// </summary>
            _Undefined_ = 0,
            /// <summary>
            /// A small light
            /// </summary>
            Light_Small = 1,
            /// <summary>
            /// A large light or several lights
            /// </summary>
            Light_Large = 2,
            /// <summary>
            /// Any appliance
            /// </summary>
            Appliance = 10,
            /// <summary>
            /// A small appliance such as a toaster
            /// </summary>
            Appliance_Small = 11,                       
            /// <summary>
            /// A large appliance such as an oven
            /// </summary>
            Appliance_Large = 12,                       
            /// <summary>
            /// A utility device
            /// </summary>
            Utility = 20,
            /// <summary>
            /// A small utility device such as a water filter
            /// </summary>
            Utility_Small = 21,
            /// <summary>
            /// A large utility device such as a well pump
            /// </summary>
            Utility_Large = 22,
            /// <summary>
            /// An entertainment device
            /// </summary>
            Entertainment = 30,
            /// <summary>
            /// A small entertainment device such as a radio
            /// </summary>
            Entertainment_Small = 31,
            /// <summary>
            /// A large entertainment device such as a home theatre system
            /// </summary>
            Entertainment_Large = 32,
            /// <summary>
            /// An HVAC device
            /// </summary>
            HVAC = 40,
            /// <summary>
            /// An Air Conditioning device
            /// </summary>
            Electric_AC = 41,
            /// <summary>
            /// An electric heating device
            /// </summary>
            Electric_Heat = 42,
            /// <summary>
            /// An electrical panel providing several branches of electrical service to the home.
            /// </summary>
            Panel = 51,
            /// <summary>
            /// An electrical panel providing several branches of electrical service to the home.
            /// </summary>
            Panel_A = 52,
            /// <summary>
            /// An electrical panel providing several branches of electrical service to the home.
            /// </summary>
            Panel_B = 53,
            /// <summary>
            /// An electrical panel providing several branches of electrical service to the home.
            /// </summary>
            Panel_C = 54,
            /// <summary>
            /// An electrical panel providing several branches of electrical service to the home.
            /// </summary>
            Panel_D = 55,
            /// <summary>
            /// An electrical panel providing several branches of electrical service to the home.
            /// </summary>
            Panel_E = 56,
            /// <summary>
            /// An electrical panel providing several branches of electrical service to the home.
            /// </summary>
            Panel_F = 57,
            /// <summary>
            /// An electric meter measuring usage for an unspecified or general purpose.
            /// </summary>
            Meter = 61,
            /// <summary>
            /// An electric meter measuring usage for electrical service such as a house service entrance.
            /// </summary>
            Meter_Service = 62,
            /// <summary>
            /// An electric meter measuring usage for a single device.
            /// </summary>
            Meter_Device = 63,
            /// <summary>
            /// An electricity producing generator.
            /// </summary>
            Generator = 71,
            /// <summary>
            /// An electricity producing solar panel.
            /// </summary>
            Solar_Panel = 72,
            /// <summary>
            /// An electricity producing wind turbine.
            /// </summary>
            Wind_Turbine = 73,
            /// <summary>
            /// An electricity producing water (wave) turbine.
            /// </summary>
            Water_Turbine = 74,
            /// <summary>
            /// The root device for child energy devices.
            /// </summary>
            Root = 98,
            /// <summary>
            /// A device (consumer or producer) that does not fit any other device type.
            /// </summary>
            Other = 99
        }
        
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        public enum eGraphType {
            Line,
            Bar,
            Pie
        }
        
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        public enum eGraphInterval {
            Minute,
            Hour,
            Day,
            Week,
            Month,
            Quarter,
            Year
        }
        
        /// <summary>
        /// PLEASE NOTE: Code related to the Speech components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Speech API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        [Obsolete("This will be removed in a future release. Please use Speech.ESpeakErrorValue as of v1.0.8.1", false)]
        public enum speak_error_values {
            SPEAK_NO_ERROR      = 1,
            SPEAK_NO_CLIENTS    = 2,
            SPEAK_ERROR_SENDING = 3
        }

        /// <summary>
        /// PLEASE NOTE: Code related to the Speech components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Speech API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        [Obsolete("This will be removed in a future release. Please use Speech.ESpeakTypeValue as of v1.0.8.1", false)]
        public enum speak_type_values {
            /// <summary>
            /// TTS voice
            /// </summary>
            SPEAK_TTS = 1,
            /// <summary>
            /// Text message
            /// </summary>
            SPEAK_MESSAGE = 2,
            /// <summary>
            /// Play a wave file
            /// </summary>
            SPEAK_WAVEFILE = 3
        }

        [Obsolete("This will be removed in a future release.  Please use Types.EOsType as of v1.0.8.1", false)]
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