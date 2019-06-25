using System;
using System.Collections.Generic;
using System.Linq;
using Classes;

namespace HomeSeer.PluginSdk.Devices {

    public class HsDevice {
        
        #region Properties

        #region Public
        
        public DateTime LastChange { get; private set; } = DateTime.MinValue;
        public int      Ref        { get; private set; } = -1;

        public Dictionary<EDeviceProperty, object> Changes { get; private set; } = new Dictionary<EDeviceProperty, object>();
        
        public string Address {
            get {
                if (Changes.ContainsKey(EDeviceProperty.Address)) {
                    return (string) Changes[EDeviceProperty.Address];
                }
                
                return _address ?? "";
            }
            set {

                if (value == _address) {
                    Changes.Remove(EDeviceProperty.Address);
                    return;
                }
                
                if (Changes.ContainsKey(EDeviceProperty.Address)) {
                    Changes[EDeviceProperty.Address] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.Address, value);
                }

                if (_cacheChanges) {
                    return;
                }
                _address = value ?? "";
            }
        }

        public int[] AssociatedDevices {
            get {
                if (Changes.ContainsKey(EDeviceProperty.AssociatedDevices)) {
                    return (Changes[EDeviceProperty.AssociatedDevices] as HashSet<int>)?.ToArray() ?? new int[0];
                }
                
                return _assDevices.ToArray();
            }
            set {

                var uniqueDevices = value != null ? new HashSet<int>(value.Distinct()) : new HashSet<int>();
                if (uniqueDevices == _assDevices) {
                    Changes.Remove(EDeviceProperty.AssociatedDevices);
                    return;
                }
                
                if (Changes.ContainsKey(EDeviceProperty.AssociatedDevices)) {
                    Changes[EDeviceProperty.AssociatedDevices] = uniqueDevices;
                }
                else {
                    Changes.Add(EDeviceProperty.AssociatedDevices, uniqueDevices);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _assDevices = uniqueDevices;
            }
        }

        public string AssociatedDevicesList {
            get {
                if (Changes.ContainsKey(EDeviceProperty.AssociatedDevices)) {
                    var cachedAssDevices = (Changes[EDeviceProperty.AssociatedDevices] as HashSet<int>)?.ToArray();
                    if (cachedAssDevices == null || cachedAssDevices.Length == 0) {
                        return "";
                    }

                    return string.Join(",", cachedAssDevices.Select(i => i.ToString()));
                }
                
                if (_assDevices == null || _assDevices.Count == 0) {
                    return "";
                }

                return string.Join(",", _assDevices.Select(i => i.ToString()));
            }
        }
        
        public string Attention {
            get {
                if (Changes.ContainsKey(EDeviceProperty.Attention)) {
                    return (string) Changes[EDeviceProperty.Attention];
                }
                
                return _attention ?? "";
            }
            set {
                if (value == _attention) {
                    Changes.Remove(EDeviceProperty.Attention);
                    return;
                }
                
                if (Changes.ContainsKey(EDeviceProperty.Attention)) {
                    Changes[EDeviceProperty.Attention] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.Attention, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _attention = value ?? "";
            }
        }
        
        public bool CanDim {
            get {
                if (Changes.ContainsKey(EDeviceProperty.CanDim)) {
                    return (bool) Changes[EDeviceProperty.CanDim];
                }
                
                return _canDim;
            }
            set {
                if (value == _canDim) {
                    Changes.Remove(EDeviceProperty.CanDim);
                    return;
                }
                
                if (Changes.ContainsKey(EDeviceProperty.CanDim)) {
                    Changes[EDeviceProperty.CanDim] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.CanDim, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _canDim = value;
            }
        }

        public string Code {
            get {
                if (Changes.ContainsKey(EDeviceProperty.Code)) {
                    return (string) Changes[EDeviceProperty.Code];
                }

                return _code ?? "";
            }
            set {
                if (value == _code) {
                    Changes.Remove(EDeviceProperty.Code);
                    return;
                }
                
                if (Changes.ContainsKey(EDeviceProperty.Code)) {
                    Changes[EDeviceProperty.Code] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.Code, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _code = value ?? "";
            }
        }

        public DeviceTypeInfo DeviceType {
            get {
                if (Changes.ContainsKey(EDeviceProperty.DeviceType)) {
                    return (DeviceTypeInfo) Changes[EDeviceProperty.DeviceType];
                }

                return _deviceType;
            }
            set {
                if (value == _deviceType) {
                    Changes.Remove(EDeviceProperty.DeviceType);
                    return;
                }
                
                if (Changes.ContainsKey(EDeviceProperty.DeviceType)) {
                    Changes[EDeviceProperty.DeviceType] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.DeviceType, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _deviceType = value ?? new DeviceTypeInfo();
            }
        }

        public string DeviceTypeString {
            get {
                if (Changes.ContainsKey(EDeviceProperty.DeviceTypeString)) {
                    return (string) Changes[EDeviceProperty.DeviceTypeString];
                }

                return _devTypeString;
            }
            set {
                if (value == _devTypeString) {
                    Changes.Remove(EDeviceProperty.DeviceTypeString);
                    return;
                }
                
                if (Changes.ContainsKey(EDeviceProperty.DeviceTypeString)) {
                    Changes[EDeviceProperty.DeviceTypeString] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.DeviceTypeString, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _devTypeString = value ?? "";
            }
        }

        public string FullAddress => 
            $"{_address?.Trim()}{((string.IsNullOrWhiteSpace(_address) || string.IsNullOrWhiteSpace(_code)) ? "" : "-")}{_code?.Trim()}";
        
        public string Image {
            get {
                if (Changes.ContainsKey(EDeviceProperty.Image)) {
                    return (string) Changes[EDeviceProperty.Image];
                }

                return _image;
            }
            set {
                if (value == _image) {
                    Changes.Remove(EDeviceProperty.Image);
                    return;
                }
                
                if (Changes.ContainsKey(EDeviceProperty.Image)) {
                    Changes[EDeviceProperty.Image] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.Image, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _image = value ?? "";
            }
        }

        public string ImageLarge {
            get {
                if (Changes.ContainsKey(EDeviceProperty.ImageLarge)) {
                    return (string) Changes[EDeviceProperty.ImageLarge];
                }

                return _imageLarge;
            }
            set {
                if (value == _imageLarge) {
                    Changes.Remove(EDeviceProperty.ImageLarge);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.ImageLarge)) {
                    Changes[EDeviceProperty.ImageLarge] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.ImageLarge, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _imageLarge = value ?? "";
            }
        }

        public string Interface {
            get {
                if (Changes.ContainsKey(EDeviceProperty.Interface)) {
                    return (string) Changes[EDeviceProperty.Interface];
                }

                return _interface;
            }
            set {
                if (value == _interface) {
                    Changes.Remove(EDeviceProperty.Interface);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.Interface)) {
                    Changes[EDeviceProperty.Interface] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.Interface, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _interface = value ?? "";
            }
        }

        /*public string InterfaceInstance {
            get => _interfaceInstance;
            set => _interfaceInstance = value ?? "";
        }*/
        
        public string Location {
            get {
                if (Changes.ContainsKey(EDeviceProperty.Location)) {
                    return (string) Changes[EDeviceProperty.Location];
                }

                return _location;
            }
            set {
                if (value == _location) {
                    Changes.Remove(EDeviceProperty.Location);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.Location)) {
                    Changes[EDeviceProperty.Location] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.Location, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _location = value ?? "";
            }
        }

        public string Location2 {
            get {
                if (Changes.ContainsKey(EDeviceProperty.Location2)) {
                    return (string) Changes[EDeviceProperty.Location2];
                }

                return _location2;
            }
            set {
                if (value == _location2) {
                    Changes.Remove(EDeviceProperty.Location2);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.Location2)) {
                    Changes[EDeviceProperty.Location2] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.Location2, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _location2 = value ?? "";
            }
        }

        public uint Misc {
            get {
                if (Changes.ContainsKey(EDeviceProperty.Misc)) {
                    return (uint) Changes[EDeviceProperty.Misc];
                }

                return _misc;
            }
        }

        public string Name {
            get {
                if (Changes.ContainsKey(EDeviceProperty.Name)) {
                    return (string) Changes[EDeviceProperty.Name];
                }

                return _name;
            }
            set {
                if (value == _name) {
                    Changes.Remove(EDeviceProperty.Name);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.Name)) {
                    Changes[EDeviceProperty.Name] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.Name, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _name = value ?? "";
            }
        }
        
        public PlugExtraData PlugExtraData {
            get {
                if (Changes.ContainsKey(EDeviceProperty.PlugExtraData)) {
                    return Changes[EDeviceProperty.PlugExtraData] as PlugExtraData ?? new PlugExtraData();
                }
                
                return _plugExtraData;
            }
            set {
                if (value == _plugExtraData) {
                    Changes.Remove(EDeviceProperty.PlugExtraData);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.PlugExtraData)) {
                    Changes[EDeviceProperty.PlugExtraData] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.PlugExtraData, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _plugExtraData = value;
            }
        }

        public ERelationship Relationship {
            get {
                if (Changes.ContainsKey(EDeviceProperty.Relationship)) {
                    return (ERelationship) Changes[EDeviceProperty.Relationship];
                }
                return _relationship;
            }
            set {
                if (value == _relationship) {
                    Changes.Remove(EDeviceProperty.Relationship);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.Relationship)) {
                    Changes[EDeviceProperty.Relationship] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.Relationship, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _relationship = value;
            }
        }

        public string Status {
            get => _status;
            set => _status = value ?? "";
        }

        public string ScaleText {
            get {
                if (Changes.ContainsKey(EDeviceProperty.ScaleText)) {
                    return (string) Changes[EDeviceProperty.ScaleText];
                }

                return _scaleText;
            }
            set {
                if (value == _scaleText) {
                    Changes.Remove(EDeviceProperty.ScaleText);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.ScaleText)) {
                    Changes[EDeviceProperty.ScaleText] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.ScaleText, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _scaleText = value ?? "";
            }
        }

        public string ScriptFunc {
            get {
                if (Changes.ContainsKey(EDeviceProperty.ScriptFunc)) {
                    return (string) Changes[EDeviceProperty.ScriptFunc];
                }

                return _scriptFunc;
            }
            set {
                if (value == _scriptFunc) {
                    Changes.Remove(EDeviceProperty.ScriptFunc);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.ScriptFunc)) {
                    Changes[EDeviceProperty.ScriptFunc] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.ScriptFunc, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _scriptFunc = value ?? "";
            }
        }

        public string ScriptName {
            get {
                if (Changes.ContainsKey(EDeviceProperty.ScriptName)) {
                    return (string) Changes[EDeviceProperty.ScriptName];
                }

                return _scriptName;
            }
            set {
                if (value == _scriptName) {
                    Changes.Remove(EDeviceProperty.ScriptName);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.ScriptName)) {
                    Changes[EDeviceProperty.ScriptName] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.ScriptName, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _scriptName = value ?? "";
            }
        }

        public StatusControlCollection StatusControls {
            get => _statusControls;
            set => _statusControls = value ?? new StatusControlCollection();
        }

        public StatusGraphicCollection StatusGraphics {
            get => _statusGraphics;
            set => _statusGraphics = value ?? new StatusGraphicCollection();
        }

        //String or String[]
        public object StringSelected {
            get {
                if (Changes.ContainsKey(EDeviceProperty.StringSelected)) {
                    return (string) Changes[EDeviceProperty.StringSelected];
                }

                return _stringSelected;
            }
            set {
                if (value == _stringSelected) {
                    Changes.Remove(EDeviceProperty.StringSelected);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.StringSelected)) {
                    Changes[EDeviceProperty.StringSelected] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.StringSelected, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _stringSelected = value;
            }
        }

        public string UserAccess {
            get {
                if (Changes.ContainsKey(EDeviceProperty.UserAccess)) {
                    return (string) Changes[EDeviceProperty.UserAccess];
                }

                return _userAccess;
            }
            set {
                if (value == _userAccess) {
                    Changes.Remove(EDeviceProperty.UserAccess);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.UserAccess)) {
                    Changes[EDeviceProperty.UserAccess] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.UserAccess, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _userAccess = value ?? "Any";
            }
        }

        public string UserNote {
            get {
                if (Changes.ContainsKey(EDeviceProperty.UserNote)) {
                    return (string) Changes[EDeviceProperty.UserNote];
                }

                return _userNote;
            }
            set {
                if (value == _userNote) {
                    Changes.Remove(EDeviceProperty.UserNote);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.UserNote)) {
                    Changes[EDeviceProperty.UserNote] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.UserNote, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _userNote = value ?? "";
            }
        }

        public double Value {
            get => _value;
            set => _value = value;
        }

        public string VoiceCommand {
            get {
                if (Changes.ContainsKey(EDeviceProperty.VoiceCommand)) {
                    return (string) Changes[EDeviceProperty.VoiceCommand];
                }

                return _voiceCommand ?? "";
            }
            set {
                if (value == _voiceCommand) {
                    Changes.Remove(EDeviceProperty.VoiceCommand);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.VoiceCommand)) {
                    Changes[EDeviceProperty.VoiceCommand] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.VoiceCommand, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _voiceCommand = value ?? "";
            }
        }

        #endregion

        #region Private

        // InvalidValue
        // '''  Indicates when a device has an invalid value.  In Z-Wave, -1 or 17
        // '''    (the ways we used to indicate invalid states) could correspond to
        // '''    a real value, so this Bool will be used instead.
        // '''
        // internal bool InvalidValue = false;
        
        private string         _address           = "";
        private HashSet<int>   _assDevices        = new HashSet<int>();
        private string         _attention         = "";
        //private string         _buttons           = "";
        //private Hashtable      _buttonScripts     = new Hashtable();
        private bool           _cacheChanges      = false;
        private bool           _canDim            = false;
        private string         _code              = "";
        private string         _devTypeString     = "";
        private DeviceTypeInfo _deviceType;
        private string         _image             = "";
        private string         _imageLarge        = "";
        private string         _interface         = "";
        //private string         _interfaceInstance = "";
        //private bool           _invalidValue      = false;
        private string         _location          = "";
        private string         _location2         = "";
        private uint           _misc;             //= (uint) Constants.dvMISC.SET_DOES_NOT_CHANGE_LAST_CHANGE;
        private string         _name              = "";
        private PlugExtraData  _plugExtraData     = new PlugExtraData();
        private ERelationship  _relationship      = ERelationship.Not_Set;
        private string         _status            = "";
        private string         _scaleText         = "";
        private string         _scriptFunc        = "";
        private string         _scriptName        = "";
        private string         _userAccess        = "Any";
        private string         _userNote          = "";
        private double         _value             = 0D;
        private string         _voiceCommand      = "";
        private object         _stringSelected;
        
        private StatusGraphicCollection _statusGraphics = new StatusGraphicCollection();
        private StatusControlCollection _statusControls = new StatusControlCollection();

        #endregion

        #endregion

        public HsDevice() {
            
        }

        public HsDevice(int deviceRef) {
            Ref = deviceRef;
        }

        public HsDevice(int deviceRef, DateTime lastChange) {
            Ref = deviceRef;
            LastChange = lastChange;
        }

        public HsDevice Duplicate(int deviceRef, HsDevice source) {
            var dev = new HsDevice(deviceRef, source.LastChange);
            dev._address = source.Address;
            dev._assDevices = new HashSet<int>(source.AssociatedDevices.Distinct());
            dev._attention = source.Attention;
            dev._canDim = source.CanDim;
            dev._code = source.Code;
            dev._deviceType = source.DeviceType;
            dev._devTypeString = source.DeviceTypeString;
            dev._image = source.Image;
            dev._imageLarge = source.ImageLarge;
            dev._interface = source.Interface;
            dev._location = source.Location;
            dev._location2 = source.Location2;
            dev._misc = source.Misc;
            dev._name = source.Name;
            dev._plugExtraData = source.PlugExtraData;
            dev._relationship = source.Relationship;
            dev._status = source.Status;
            dev._scaleText = source.ScaleText;
            dev._scriptFunc = source.ScriptFunc;
            dev._scriptName = source.ScriptName;
            dev._statusControls = source.StatusControls;
            dev._statusGraphics = source.StatusGraphics;
            dev._stringSelected = source.StringSelected;
            dev._userAccess = source.UserAccess;
            dev._userNote = source.UserNote;
            dev._value = source.Value;
            dev._voiceCommand = source.VoiceCommand;
            return dev;
        }
        
        public void AddMiscFlag(EDeviceMiscFlag misc) {
            var miscTemp = _misc | (uint) misc;
            if (miscTemp != _misc) {
                _misc = _misc | (uint) misc;
            }
        }

        public bool ContainsMiscFlag(EDeviceMiscFlag misc) {
            return (_misc & (uint) misc) != 0;
        }

        public void ClearMiscFlag(EDeviceMiscFlag misc) {
            var miscTemp = _misc ^ (uint) misc;
            if (miscTemp != _misc) {
                _misc = _misc ^ (uint) misc;
            }
        }

    }

}