using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSeer.PluginSdk.Devices {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public abstract class AbstractHsDevice {
        
        #region Properties
        
        public Dictionary<EDeviceProperty, object> Changes { get; protected set; } = new Dictionary<EDeviceProperty, object>();
        public DateTime LastChange { get; protected set; } = DateTime.MinValue;
        public int      Ref        { get; protected set; } = -1;

        #region Public
        
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
        
        public HashSet<int> AssociatedDevices {
            get {
                if (Changes.ContainsKey(EDeviceProperty.AssociatedDevices)) {
                    return Changes[EDeviceProperty.AssociatedDevices] as HashSet<int> ?? new HashSet<int>();
                }
                
                return _assDevices;
            }
            set {

                if (value == _assDevices) {
                    Changes.Remove(EDeviceProperty.AssociatedDevices);
                    return;
                }
                
                if (Changes.ContainsKey(EDeviceProperty.AssociatedDevices)) {
                    Changes[EDeviceProperty.AssociatedDevices] = value ?? new HashSet<int>();
                }
                else {
                    Changes.Add(EDeviceProperty.AssociatedDevices, value ?? new HashSet<int>());
                }
                
                if (_cacheChanges) {
                    return;
                }
                _assDevices = value ?? new HashSet<int>();
            }
        }

        public string AssociatedDevicesList {
            get {
                if (Changes.ContainsKey(EDeviceProperty.AssociatedDevices)) {
                    if (!(Changes[EDeviceProperty.AssociatedDevices] is HashSet<int> cachedAssDevices) || cachedAssDevices.Count == 0) {
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

        //public string FullAddress => $"{_address?.Trim()}{((string.IsNullOrWhiteSpace(_address) || string.IsNullOrWhiteSpace(_code)) ? "" : "-")}{_code?.Trim()}";
        
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
        
        public bool IsValueInvalid {
            get {
                if (Changes.ContainsKey(EDeviceProperty.InvalidValue)) {
                    return (bool) Changes[EDeviceProperty.InvalidValue];
                }

                return _invalidValue || !IsValueValid();
            }
            set {
                if (value == _invalidValue) {
                    Changes.Remove(EDeviceProperty.InvalidValue);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.InvalidValue)) {
                    Changes[EDeviceProperty.InvalidValue] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.InvalidValue, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _invalidValue = value;
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
            set {
                if (value == _misc) {
                    Changes.Remove(EDeviceProperty.Misc);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.Misc)) {
                    Changes[EDeviceProperty.Misc] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.Misc, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _misc = value;
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
        
        public string ProductImage {
            get {
                if (Changes.ContainsKey(EDeviceProperty.ProductImage)) {
                    return (string) Changes[EDeviceProperty.ProductImage];
                }

                return _productImage;
            }
            set {
                if (value == _productImage) {
                    Changes.Remove(EDeviceProperty.ProductImage);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.ProductImage)) {
                    Changes[EDeviceProperty.ProductImage] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.ProductImage, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _productImage = value ?? "";
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

                var currentAssociatedDeviceList = _assDevices ?? new HashSet<int>();
                if (Changes.ContainsKey(EDeviceProperty.AssociatedDevices)) {
                    currentAssociatedDeviceList = (HashSet<int>) Changes[EDeviceProperty.AssociatedDevices];
                }

                if (currentAssociatedDeviceList.Count > 0) {
                    throw new DeviceRelationshipException("Please clear this devices association with other devices before changing its relationship type.");
                }
                
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
            get {
                if (Changes.ContainsKey(EDeviceProperty.Status)) {
                    return (string) Changes[EDeviceProperty.Status];
                }
                
                return _status;
            }
            set {
                if (value == _status) {
                    Changes.Remove(EDeviceProperty.Status);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.Status)) {
                    Changes[EDeviceProperty.Status] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.Status, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _status = value ?? "";
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
            get {
                if (Changes.ContainsKey(EDeviceProperty.Value)) {
                    return (double) Changes[EDeviceProperty.Value];
                }
                
                return _value;
            }
            set {
                if (value == _value) {
                    Changes.Remove(EDeviceProperty.Value);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.Value)) {
                    Changes[EDeviceProperty.Value] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.Value, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                
                _value = value;
            }
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
        
        protected string         _address           = "";
        protected HashSet<int>   _assDevices        = new HashSet<int>();
        //private string         _attention         = "";
        //private string         _buttons           = "";
        //private Hashtable      _buttonScripts     = new Hashtable();
        protected bool           _cacheChanges      = false;
        //private bool           _canDim            = false;
        //private string         _code              = "";
        //private string         _devTypeString     = "";
        protected DeviceTypeInfo _deviceType; //TODO device type
        protected string         _image             = "";
        protected string         _interface         = "";
        protected bool           _invalidValue      = false;
        //private string         _interfaceInstance = "";
        protected string         _location          = "Home";
        protected string         _location2         = "Home";
        protected uint           _misc              = (uint) EDeviceMiscFlag.SHOW_VALUES;
        protected string         _name              = "";
        protected PlugExtraData  _plugExtraData     = new PlugExtraData();
        protected string         _productImage      = "";
        protected ERelationship  _relationship      = ERelationship.NotSet;
        protected string         _status            = "";
        //private string         _scaleText         = "";
        //private string         _scriptFunc        = "";
        //private string         _scriptName        = "";
        protected string         _userAccess        = "Any";
        protected string         _userNote          = "";
        protected double         _value             = 0D;
        protected string         _voiceCommand      = "";
        //private object         _stringSelected;
        
        #endregion

        #endregion

        internal AbstractHsDevice() {}

        internal AbstractHsDevice(int deviceRef) {
            Ref = deviceRef;
        }

        internal AbstractHsDevice(int deviceRef, DateTime lastChange) {
            Ref = deviceRef;
            LastChange = lastChange;
        }

        public void RevertChanges() {
            Changes = new Dictionary<EDeviceProperty, object>();
        }

        protected virtual bool IsValueValid() {
            return true;
        }

        public void SetParentDevice(int deviceRef) {

            var associatedDevices = _assDevices ?? new HashSet<int>();

            if (Changes.ContainsKey(EDeviceProperty.AssociatedDevices)) {
                associatedDevices = Changes[EDeviceProperty.AssociatedDevices] as HashSet<int> ?? new HashSet<int>();
            }

            if (associatedDevices.Count > 0) {
                if (Changes.ContainsKey(EDeviceProperty.Relationship)) {
                    var cachedRelationshipChange = (ERelationship) Changes[EDeviceProperty.Relationship];
                    if (cachedRelationshipChange == ERelationship.Device) {
                        throw new DeviceRelationshipException("This device is already a parent device with children.  Remove its associations before converting it to a child device.");
                    }
                }
                else if (_relationship == ERelationship.Device) {
                    throw new DeviceRelationshipException("This device is already a parent device with children.  Remove its associations before converting it to a child device.");
                }
            }
            
            var updatedDeviceList = new HashSet<int> {deviceRef};

            if (Changes.ContainsKey(EDeviceProperty.Relationship)) {
                Changes[EDeviceProperty.Relationship] = ERelationship.Feature;
            }
            else {
                Changes.Add(EDeviceProperty.Relationship, ERelationship.Feature);
            }

            if (Changes.ContainsKey(EDeviceProperty.AssociatedDevices)) {
                Changes[EDeviceProperty.AssociatedDevices] = updatedDeviceList;
            }
            else {
                Changes.Add(EDeviceProperty.AssociatedDevices, updatedDeviceList);
            }

            if (_cacheChanges) {
                return;
            }

            _assDevices = updatedDeviceList;
            _relationship = ERelationship.Feature;
        }
        
        public void AddMiscFlag(EDeviceMiscFlag misc) {
            
            var currentMisc = _misc;
            if (Changes.ContainsKey(EDeviceProperty.Misc)) {
                currentMisc = (uint) Changes[EDeviceProperty.Misc];
            }
            
            var tempMisc = currentMisc | (uint) misc;
            
            if (Changes.ContainsKey(EDeviceProperty.Misc)) {
                Changes[EDeviceProperty.Misc] = tempMisc;
            }
            else {
                Changes.Add(EDeviceProperty.Misc, tempMisc);
            }

            if (_cacheChanges) {
                return;
            }
            
            _misc = tempMisc;
        }

        public bool ContainsMiscFlag(EDeviceMiscFlag misc) {
            var currentMisc = _misc;
            if (Changes.ContainsKey(EDeviceProperty.Misc)) {
                currentMisc = (uint) Changes[EDeviceProperty.Misc];
            }
            
            return (currentMisc & (uint) misc) != 0;
        }

        public void RemoveMiscFlag(EDeviceMiscFlag misc) {
            var currentMisc = _misc;
            if (Changes.ContainsKey(EDeviceProperty.Misc)) {
                currentMisc = (uint) Changes[EDeviceProperty.Misc];
            }
            
            var tempMisc = currentMisc ^ (uint) misc;
            
            if (Changes.ContainsKey(EDeviceProperty.Misc)) {
                Changes[EDeviceProperty.Misc] = tempMisc;
            }
            else {
                Changes.Add(EDeviceProperty.Misc, tempMisc);
            }

            if (_cacheChanges) {
                return;
            }
            
            _misc = tempMisc;
        }

        public void ClearMiscFlag() {
            if (Changes.ContainsKey(EDeviceProperty.Misc)) {
                Changes[EDeviceProperty.Misc] = (uint) 0;
            }
            else {
                Changes.Add(EDeviceProperty.Misc, (uint) 0);
            }

            if (_cacheChanges) {
                return;
            }

            _misc = 0;
        }

    }

}