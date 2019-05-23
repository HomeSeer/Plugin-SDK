using System;

namespace HomeSeer.PluginSdk {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable()]
    public class DeviceTypeInfo {
                
        private int mvarAPI = (int) Constants.eDeviceAPI.No_API;
        private int mvarType; // = eDeviceType.No_API
        private int mvarSubType;
        private string mvarSubTypeDesc = "";

        public Constants.eDeviceAPI Device_API {
            get => (Constants.eDeviceAPI) mvarAPI;
            set => mvarAPI = (int) value;
        }
        public string Device_API_Description {
            get {
                switch ((Constants.eDeviceAPI) mvarAPI) {
                    case Constants.eDeviceAPI.No_API:
                        return "No API";
                    case Constants.eDeviceAPI.Media:
                        return "Music API";
                    case Constants.eDeviceAPI.Plug_In:
                        return "Plug-In API";
                    case Constants.eDeviceAPI.Security:
                        return "Security API";
                    case Constants.eDeviceAPI.Thermostat:
                        return "Thermostat API";
                    case Constants.eDeviceAPI.SourceSwitch:
                        return "Source Switch";
                    case Constants.eDeviceAPI.Script:
                        return "Run Script";
                    case Constants.eDeviceAPI.Energy:
                        return "Energy API";
                    default:
                        return "Undefined API=" + Convert.ToInt32(mvarAPI);
                }
            }
        }

        public int Device_Type {
            get => mvarType;
            set => mvarType = value;
        }
        public string Device_Type_Description {
            get {
                switch ((Constants.eDeviceAPI) mvarAPI) {
                    case Constants.eDeviceAPI.Energy:
                        switch ((Constants.eDeviceType_Energy) mvarType) {
                            case Constants.eDeviceType_Energy.Amps:
                                return "Amps";
                            case Constants.eDeviceType_Energy.Graphing:
                                return "Graphing";
                            case Constants.eDeviceType_Energy.KWH:
                                return "KWH";
                            case Constants.eDeviceType_Energy.Volts:
                                return "Volts";
                            case Constants.eDeviceType_Energy.Watts:
                                return "Watts";
                        }
                        break;
                    case Constants.eDeviceAPI.Media:
                        switch ((Constants.eDeviceType_Media) mvarType) {
                            case Constants.eDeviceType_Media.Media_Album:
                                return "Album";
                            case Constants.eDeviceType_Media.Media_Artist:
                                return "Artist";
                            case Constants.eDeviceType_Media.Media_Genre:
                                return "Genre";
                            case Constants.eDeviceType_Media.Media_Playlist:
                                return "Playlist";
                            case Constants.eDeviceType_Media.Media_Selector_Control:
                                return "Music Selector";
                            case Constants.eDeviceType_Media.Media_Track:
                                return "Track";
                            case Constants.eDeviceType_Media.Player_Control:
                                return "Player Control";
                            case Constants.eDeviceType_Media.Player_Repeat:
                                return "Player Repeat";
                            case Constants.eDeviceType_Media.Player_Shuffle:
                                return "Player Shuffle";
                            case Constants.eDeviceType_Media.Player_Status:
                                return "Player Status";
                            case Constants.eDeviceType_Media.Player_Status_Additional:
                                return "Player Additional Status";
                            case Constants.eDeviceType_Media.Player_Volume:
                                return "Player Volume";
                            case Constants.eDeviceType_Media.Media_Type:
                                return "Media Type";
                            case Constants.eDeviceType_Media.Root:
                                return "Music Root Device";
                            default:
                                return "Undefined Music Type " + mvarType.ToString();
                        }
                        break;
                    case Constants.eDeviceAPI.No_API:
                        return "Type " + mvarType.ToString();
                    case Constants.eDeviceAPI.Plug_In:
                        if (mvarType == (int) Constants.eDeviceType_Plugin.Root) {
                            return "Plug-In Root Device";
                        }
                        else {
                            return "Plug-In Type " + mvarType.ToString();
                        }
                        break;
                    case Constants.eDeviceAPI.Security:
                        switch ((Constants.eDeviceType_Security) mvarType) {
                            case Constants.eDeviceType_Security.Alarm:
                                return "Alarm";
                            case Constants.eDeviceType_Security.Arming:
                                return "Arming";
                            case Constants.eDeviceType_Security.Communicator:
                                return "Communicator";
                            case Constants.eDeviceType_Security.Keypad:
                                return "Keypad";
                            case Constants.eDeviceType_Security.Zone_Perimeter:
                                return "Perimeter Zone";
                            case Constants.eDeviceType_Security.Zone_Perimeter_Delay:
                                return "Perimeter Zone (D)";
                            case Constants.eDeviceType_Security.Zone_Interior:
                                return "Interior Zone";
                            case Constants.eDeviceType_Security.Zone_Interior_Delay:
                                return "Interior Zone (D)";
                            case Constants.eDeviceType_Security.Zone_Auxiliary:
                                return "Auxiliary Zone";
                            case Constants.eDeviceType_Security.Zone_Other:
                                return "Other Zone";
                            case Constants.eDeviceType_Security.Zone_Safety_Smoke:
                                return "Smoke Detector";
                            case Constants.eDeviceType_Security.Zone_Safety_CO:
                                return "CO Detector";
                            case Constants.eDeviceType_Security.Zone_Safety_CO2:
                                return "CO2 Detector";
                            case Constants.eDeviceType_Security.Zone_Safety_Other:
                                return "Other Safety Detector";
                            case Constants.eDeviceType_Security.Output_Relay:
                                return "Relay";
                            case Constants.eDeviceType_Security.Output_Other:
                                return "Other Output";
                            case Constants.eDeviceType_Security.Siren:
                                return "Siren";
                            case Constants.eDeviceType_Security.Root:
                                return "Security Root Device";
                            default:
                                return "Undefined Security Type " + mvarType.ToString();
                        }
                        break;
                    case Constants.eDeviceAPI.Thermostat:
                        switch ((Constants.eDeviceType_Thermostat) mvarType) {
                            case Constants.eDeviceType_Thermostat.Additional_Temperature:
                                return "Thermostat Additional Temperature";
                            case Constants.eDeviceType_Thermostat.Fan_Mode_Set:
                                return "Thermostat Fan Mode";
                            case Constants.eDeviceType_Thermostat.Fan_Status:
                                return "Thermostat Fan Status";
                            case Constants.eDeviceType_Thermostat.Filter_Remind:
                                return "Thermostat Filter Reminder";
                            case Constants.eDeviceType_Thermostat.Hold_Mode:
                                return "Thermostat Hold Mode";
                            case Constants.eDeviceType_Thermostat.Mode_Set:
                                return "Thermostat Mode Setting";
                            case Constants.eDeviceType_Thermostat.Operating_Mode:
                                return "Thermostat Operating Mode";
                            case Constants.eDeviceType_Thermostat.Operating_State:
                                return "Thermostat Operating State";
                            case Constants.eDeviceType_Thermostat.RunTime:
                                return "Thermostat Runtime";
                            case Constants.eDeviceType_Thermostat.Setback:
                                return "Thermostat Setback";
                            case Constants.eDeviceType_Thermostat.Setpoint:
                                return "Thermostat Setpoint";
                            case Constants.eDeviceType_Thermostat.Temperature:
                                return "Thermostat Temperature";
                            case Constants.eDeviceType_Thermostat.Root:
                                return "Thermostat Root Device";
                            default:
                                return "Undefined Thermostat Type " + mvarType.ToString();
                        }
                        break;
                    case Constants.eDeviceAPI.Script:
                        switch ((Constants.eDeviceType_Script) mvarType) {
                            case Constants.eDeviceType_Script.Disabled:
                                return "Run Script: Disabled";
                            case Constants.eDeviceType_Script.Run_On_Any_Change:
                                return "Run Script: On Any Change";
                            case Constants.eDeviceType_Script.Run_On_String_Change:
                                return "Run Script: On String Change";
                            case Constants.eDeviceType_Script.Run_On_Value_Change:
                                return "Run Script: On Value Change";
                            default:
                                return "Run Script: (Type Invalid)";
                        }
                        break;
                    case Constants.eDeviceAPI.SourceSwitch:
                        switch ((Constants.eDeviceType_SourceSwitch) mvarType) {
                            case Constants.eDeviceType_SourceSwitch.Root:
                                return "Source Switch Root";
                            case Constants.eDeviceType_SourceSwitch.Source:
                                return "Source Switch Source";
                            case Constants.eDeviceType_SourceSwitch.Source_Extended:
                                return "Source Switch Extended Source";
                            case Constants.eDeviceType_SourceSwitch.System:
                                return "Source Switch System";
                            case Constants.eDeviceType_SourceSwitch.Zone:
                                return "Source Switch Zone";
                            case Constants.eDeviceType_SourceSwitch.Zone_Extended:
                                return "Source Switch Extended Zone";
                            default:
                                return "Source Switch Unknown=" + mvarType.ToString();
                        }
                        break;
                    default:
                        if (mvarType == Constants.eDeviceType_GenericRoot) {
                            if (mvarSubType > 0) {
                                return "Generic Root Device, SubType " + mvarSubType.ToString();
                            }
                            
                            return "Generic Root Device";
                        }
                        
                        return "Undefined API " + Convert.ToInt32(mvarAPI).ToString() + ", Type " + mvarType.ToString() + ", SubType " + mvarSubType.ToString();
                }

                //TODO throw an exception here?
                return "";
            }
        }

        // for internal use by a plugin
        public int Device_SubType {
            get => mvarSubType;
            set => mvarSubType = value;
        }
        public string Device_SubType_Description {
            get => mvarSubTypeDesc ?? (mvarSubTypeDesc = "");
            set => mvarSubTypeDesc = value ?? "";
        }

    }

}