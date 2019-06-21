using System;
using System.Collections;
using System.Collections.Generic;

namespace HomeSeer.PluginSdk.Devices {

    public class HsDevice {
        
        #region Properties

        #region Public
        
        public string[]                AdditionalDisplayData { get; set; }
        public bool                    CanDim                { get; set; }
        public bool                    ChangedValueOrString  { get; set; }
        public string                  DevString             { get; }      = "";
        public double                  DevValue              { get; }
        public DateTime                LastChange            { get; set; } = DateTime.MinValue;
        public int                     LinkedDevice          { get; set; }
        public PlugExtraData           PlugExtraData         { get; set; }
        public int                     Ref                   { get; set; } = -1;
        public Constants.eRelationship Relationship          { get; set; } = Constants.eRelationship.Not_Set;
        public bool                    StatusSupport         { get; set; }
        
        public string Address {
            get {
                if (_code == null)
                    _code = "";
                if (_address == null)
                    _address = "";
                if (string.IsNullOrEmpty(_code.Trim()))
                    return _address;
                if (string.IsNullOrEmpty(_address.Trim()))
                    return _code;
                return _address + "-" + _code;
            }
            set => _address = value ?? "";
        }
        
        public int[] AssociatedDevices {
            get {
                if (_assDevices == null) _assDevices = new List<int>();
                return _assDevices.ToArray();
            }
        }

        public string AssociatedDevicesList {
            get {
                if (_assDevices == null)
                    return "";
                if (_assDevices.Count < 1)
                    return "";
                var s = "";
                foreach (var ass in _assDevices)
                    if (string.IsNullOrEmpty(s.Trim()))
                        s = ass.ToString();
                    else
                        s += "," + ass;
                return s;
            }
        }
        
        public string Attention {
            get => _attention ?? "";
            set => _attention = value ?? "";
        }

        public string Code {
            get => _code ?? "";
            set => _code = value ?? "";
        }
        
        public DeviceTypeInfo DeviceType {
            get => _deviceType;
            set => _deviceType = value ?? new DeviceTypeInfo();
        }
        
        public string DeviceTypeString {
            get => _devTypeString;
            set => _devTypeString = value ?? "";
        }
        
        public string Image {
            get => _image;
            set => _image = value ?? "";
        }

        public string ImageLarge {
            get => _imageLarge;
            set => _imageLarge = value ?? "";
        }
        
        public string Interface {
            get => _interface;
            set => _interface = value ?? "";
        }

        public string InterfaceInstance {
            get => _interfaceInstance;
            set => _interfaceInstance = value ?? "";
        }
        
        public string Location {
            get => _location;
            set => _location = value ?? "";
        }

        public string Location2 {
            get => _location2;
            set => _location2 = value ?? "";
        }
        
        public string Name {
            get => _name;
            set => _name = value ?? "";
        }

        public string NewDevString {
            get => _newDevString;
            set => _newDevString = value ?? "";
        }

        public string ScaleText {
            get => _scaleText;
            set => _scaleText = value ?? "";
        }
        
        public string ScriptFunc {
            get => _scriptFunc;
            set => _scriptFunc = value ?? "";
        }

        public string ScriptName {
            get => _scriptName;
            set => _scriptName = value ?? "";
        }
        
        //String or String[]
        public object StringSelected {
            get => _stringSelected;
            set => _stringSelected = value;
        }
        
        public string UserAccess {
            get => _userAccess;
            set => _userAccess = value ?? "Any";
        }

        public string UserNote {
            get => _userNote;
            set => _userNote = value ?? "";
        }
        
        public string VoiceCommand {
            get => _voiceCommand ?? "";
            set => _voiceCommand = value ?? "";
        }

        #endregion

        #region Private

        /// InvalidValue
        /// '''  Indicates when a device has an invalid value.  In Z-Wave, -1 or 17
        /// '''    (the ways we used to indicate invalid states) could correspond to
        /// '''    a real value, so this Bool will be used instead.
        /// '''
        // internal bool InvalidValue = false;
        
        private string         _address           = "";
        private List<int>      _assDevices;
        private string         _attention         = "";
        private string         _buttons           = "";
        private Hashtable      _buttonScripts     = new Hashtable();
        private string         _code              = "";
        private string         _devTypeString     = "";
        private DeviceTypeInfo _deviceType;
        private string         _image             = "";
        private string         _imageLarge        = "";
        private string         _interface         = "";
        private string         _interfaceInstance = "";
        private bool           _invalidValue      = false;
        private string         _location          = "";
        private string         _location2         = "";
        private uint           _misc              = (uint) Constants.dvMISC.SET_DOES_NOT_CHANGE_LAST_CHANGE;
        private string         _name              = "";
        private string         _newDevString      = "";
        private string         _scaleText         = "";
        private string         _scriptFunc        = "";
        private string         _scriptName        = "";
        private string         _sortCode          = "";
        private string         _sortString        = "";
        private string         _sortTime          = "";
        private string         _userAccess        = "Any";
        private string         _userNote          = "";
        private List<StatusGraphic>   _vgPairs           = new List<StatusGraphic>();
        private string         _voiceCommand      = "";
        private List<VSPair>   _vsPairs           = new List<VSPair>();
        private object         _stringSelected;

        #endregion

        #endregion

        public void AssociatedDevice_Add(int dvRef) {
            if (_assDevices == null)
                _assDevices = new List<int>();
            if (dvRef < 1)
                return;
            if (_assDevices.Count < 1) {
                _assDevices.Add(dvRef);
                return;
            }

            var found = false;
            foreach (var ass in _assDevices)
                if (ass == dvRef) {
                    found = true;
                    break;
                }

            if (found)
                return;
            _assDevices.Add(dvRef);
        }

        public void AssociatedDevice_ClearAll() {
            _assDevices = new List<int>();
        }

        public void AssociatedDevice_Remove(int dvRef) {
            if (_assDevices == null)
                _assDevices = new List<int>();
            if (dvRef < 1)
                return;
            if (_assDevices.Count < 1)
                return;
            try {
                _assDevices.Remove(dvRef);
            }
            catch (Exception ex) { }
        }

        public bool MISC_Check(Constants.dvMISC misc) {
            return (_misc & (uint) misc) != 0;
        }

        public void MISC_Clear(Constants.dvMISC misc) {
            var miscTemp                       = (_misc ^ (uint) misc);
            if (miscTemp != _misc) _misc = (_misc ^ (uint) misc);
        }

        public void MISC_Set(Constants.dvMISC misc) {
            var miscTemp                       = (_misc | (uint) misc);
            if (miscTemp != _misc) _misc = (_misc | (uint) misc);
        }

    }

}