using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Scheduler")]

namespace HomeSeer.PluginSdk.Devices {
    
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class HsFeature : AbstractHsDevice {
        
        #region Properties
        
        #region Public

        public StatusControlCollection StatusControls {
            get {
                if (Changes.ContainsKey(EDeviceProperty.StatusControls)) {
                    return Changes[EDeviceProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
                }
                
                return _statusControls;
            }
            /*set {
                var currentRelationship = _relationship;
                
                if (Changes.ContainsKey(EDeviceProperty.Relationship)) {
                    currentRelationship = (ERelationship) Changes[EDeviceProperty.Relationship];
                }

                if (currentRelationship != ERelationship.Child || currentRelationship != ERelationship.Standalone) {
                    throw new DeviceRelationshipException("Only child and standalone devices can have status controls");
                }
                
                _statusControls = value ?? new StatusControlCollection();
            }*/
        }

        public StatusGraphicCollection StatusGraphics {
            get {
                if (Changes.ContainsKey(EDeviceProperty.StatusGraphics)) {
                    return Changes[EDeviceProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
                }
                
                return _statusGraphics;
            }
            /*set {
                var currentRelationship = _relationship;
                
                if (Changes.ContainsKey(EDeviceProperty.Relationship)) {
                    currentRelationship = (ERelationship) Changes[EDeviceProperty.Relationship];
                }

                if (currentRelationship != ERelationship.Child || currentRelationship != ERelationship.Standalone) {
                    throw new DeviceRelationshipException("Only child and standalone devices can have status graphics");
                }
                
                _statusGraphics = value ?? new StatusGraphicCollection();
            }*/
        }

        #endregion

        #region Private
        
        private StatusGraphicCollection _statusGraphics = new StatusGraphicCollection();
        private StatusControlCollection _statusControls = new StatusControlCollection();

        #endregion

        #endregion

        public HsFeature() { }
        public HsFeature(int deviceRef) : base(deviceRef) { }
        public HsFeature(int deviceRef, DateTime lastChange) : base(deviceRef, lastChange) { }

        public HsFeature Duplicate(int deviceRef, HsFeature source) {
            var dev = new HsFeature(deviceRef, source.LastChange)
                      {
                          _address        = source.Address,
                          _assDevices     = source.AssociatedDevices,
                          _deviceType     = source.DeviceType,
                          _image          = source.Image,
                          _productImage   = source.ProductImage,
                          _interface      = source.Interface,
                          _location       = source.Location,
                          _location2      = source.Location2,
                          _misc           = source.Misc,
                          _name           = source.Name,
                          _plugExtraData  = source.PlugExtraData,
                          _relationship   = source.Relationship,
                          _status         = source.Status,
                          _statusControls = source.StatusControls,
                          _statusGraphics = source.StatusGraphics,
                          _userAccess     = source.UserAccess,
                          _userNote       = source.UserNote,
                          _value          = source.Value,
                          _voiceCommand   = source.VoiceCommand
                      };
            return dev;
        }

        /// <inheritdoc />
        protected override bool IsValueValid() {
            try {
                return HasControlForValue(_value) || HasGraphicForValue(_value);
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                return false;
            }            
        }

        internal void SetParentDevice(int deviceRef) {

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

        /*private bool AddAssociatedDevice(int deviceRef) {
            if (_relationship != ERelationship.Parent_Root || _relationship != ERelationship.Child) {
                throw new DeviceRelationshipException("Cannot add an association to a device that is not a parent or child.");
            }

            var wasDeviceAdded = false;
            var updatedDeviceList = _assDevices ?? new HashSet<int>();
            
            if (Changes.ContainsKey(EDeviceProperty.AssociatedDevices)) {

                updatedDeviceList = Changes[EDeviceProperty.AssociatedDevices] as HashSet<int> ?? new HashSet<int>();
                wasDeviceAdded = updatedDeviceList.Add(deviceRef);
                Changes[EDeviceProperty.AssociatedDevices] = updatedDeviceList;
            }
            else {
                wasDeviceAdded = updatedDeviceList.Add(deviceRef);
                Changes.Add(EDeviceProperty.AssociatedDevices, updatedDeviceList);
            }

            if (!_cacheChanges) {
                _assDevices = updatedDeviceList;
            }

            return wasDeviceAdded;
        }

        private bool RemoveAssociatedDevice(int deviceRef) {
            
            var wasDeviceRemoved = false;
            var updatedDeviceList = _assDevices ?? new HashSet<int>();
            
            if (Changes.ContainsKey(EDeviceProperty.AssociatedDevices)) {

                updatedDeviceList = Changes[EDeviceProperty.AssociatedDevices] as HashSet<int> ?? new HashSet<int>();
                wasDeviceRemoved = updatedDeviceList.Remove(deviceRef);
                Changes[EDeviceProperty.AssociatedDevices] = updatedDeviceList;
            }
            else {
                wasDeviceRemoved = updatedDeviceList.Remove(deviceRef);
                Changes.Add(EDeviceProperty.AssociatedDevices, updatedDeviceList);
            }

            if (!_cacheChanges) {
                _assDevices = updatedDeviceList;
            }

            return wasDeviceRemoved;
        }*/

        internal void AddStatusControl(StatusControl statusControl) {

            var currentStatusControls = _statusControls ?? new StatusControlCollection();
            if (Changes.ContainsKey(EDeviceProperty.StatusControls)) {
                currentStatusControls = Changes[EDeviceProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
            }
            
            currentStatusControls.Add(statusControl);
            
            if (Changes.ContainsKey(EDeviceProperty.StatusControls)) {
                Changes[EDeviceProperty.StatusControls] = currentStatusControls;
            }
            else {
                Changes.Add(EDeviceProperty.StatusControls, currentStatusControls);
            }

            if (_cacheChanges) {
                return;
            }

            _statusControls = currentStatusControls;
        }

        internal void AddStatusControls(List<StatusControl> statusControls) {
            var currentStatusControls = _statusControls ?? new StatusControlCollection();
            if (Changes.ContainsKey(EDeviceProperty.StatusControls)) {
                currentStatusControls = Changes[EDeviceProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
            }
            
            currentStatusControls.AddRange(statusControls);
            
            if (Changes.ContainsKey(EDeviceProperty.StatusControls)) {
                Changes[EDeviceProperty.StatusControls] = currentStatusControls;
            }
            else {
                Changes.Add(EDeviceProperty.StatusControls, currentStatusControls);
            }

            if (_cacheChanges) {
                return;
            }

            _statusControls = currentStatusControls;
        }

        public bool HasControlForValue(double value) {
            
            var currentStatusControls = _statusControls ?? new StatusControlCollection();
            if (Changes.ContainsKey(EDeviceProperty.StatusControls)) {
                currentStatusControls = Changes[EDeviceProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
            }

            return currentStatusControls.ContainsValue(value);
        }
        
        public bool HasControlForRange(ValueRange range) {
            
            var currentStatusControls = _statusControls ?? new StatusControlCollection();
            if (Changes.ContainsKey(EDeviceProperty.StatusControls)) {
                currentStatusControls = Changes[EDeviceProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
            }

            return currentStatusControls.ContainsValue(range.Min) || currentStatusControls.ContainsValue(range.Max);
        }

        internal void RemoveStatusControl(StatusControl statusControl) {
            
            var currentStatusControls = _statusControls ?? new StatusControlCollection();
            if (Changes.ContainsKey(EDeviceProperty.StatusControls)) {
                currentStatusControls = Changes[EDeviceProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
            }
            
            currentStatusControls.Remove(statusControl);
            
            if (Changes.ContainsKey(EDeviceProperty.StatusControls)) {
                Changes[EDeviceProperty.StatusControls] = currentStatusControls;
            }
            else {
                Changes.Add(EDeviceProperty.StatusControls, currentStatusControls);
            }

            if (_cacheChanges) {
                return;
            }

            _statusControls = currentStatusControls;
        }

        internal void ClearStatusControls() {
            
            var currentStatusControls = new StatusControlCollection();
            
            if (Changes.ContainsKey(EDeviceProperty.StatusControls)) {
                Changes[EDeviceProperty.StatusControls] = currentStatusControls;
            }
            else {
                Changes.Add(EDeviceProperty.StatusControls, currentStatusControls);
            }

            if (_cacheChanges) {
                return;
            }

            _statusControls = currentStatusControls;
        }

        internal void AddStatusGraphic(StatusGraphic statusGraphic) {
            
            var currentStatusGraphics = _statusGraphics ?? new StatusGraphicCollection();
            if (Changes.ContainsKey(EDeviceProperty.StatusGraphics)) {
                currentStatusGraphics = Changes[EDeviceProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
            }
            
            currentStatusGraphics.Add(statusGraphic);
            
            if (Changes.ContainsKey(EDeviceProperty.StatusGraphics)) {
                Changes[EDeviceProperty.StatusGraphics] = currentStatusGraphics;
            }
            else {
                Changes.Add(EDeviceProperty.StatusGraphics, currentStatusGraphics);
            }

            if (_cacheChanges) {
                return;
            }

            _statusGraphics = currentStatusGraphics;
        }
        
        internal void AddStatusGraphics(List<StatusGraphic> statusGraphics) {
            var currentStatusGraphics = _statusGraphics ?? new StatusGraphicCollection();
            if (Changes.ContainsKey(EDeviceProperty.StatusGraphics)) {
                currentStatusGraphics = Changes[EDeviceProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
            }
            
            currentStatusGraphics.AddRange(statusGraphics);
            
            if (Changes.ContainsKey(EDeviceProperty.StatusGraphics)) {
                Changes[EDeviceProperty.StatusGraphics] = currentStatusGraphics;
            }
            else {
                Changes.Add(EDeviceProperty.StatusGraphics, currentStatusGraphics);
            }

            if (_cacheChanges) {
                return;
            }

            _statusGraphics = currentStatusGraphics;
        }
        
        public bool HasGraphicForValue(double value) {
            
            var currentStatusGraphics = _statusGraphics ?? new StatusGraphicCollection();
            if (Changes.ContainsKey(EDeviceProperty.StatusGraphics)) {
                currentStatusGraphics = Changes[EDeviceProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
            }

            return currentStatusGraphics.ContainsValue(value);
        }
        
        public bool HasGraphicForRange(ValueRange range) {
            
            var currentStatusGraphics = _statusGraphics ?? new StatusGraphicCollection();
            if (Changes.ContainsKey(EDeviceProperty.StatusGraphics)) {
                currentStatusGraphics = Changes[EDeviceProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
            }

            return currentStatusGraphics.ContainsValue(range.Min) || currentStatusGraphics.ContainsValue(range.Max);
        }

        internal void RemoveStatusGraphic(StatusGraphic statusGraphic) {
            
            var currentStatusGraphics = _statusGraphics ?? new StatusGraphicCollection();
            if (Changes.ContainsKey(EDeviceProperty.StatusGraphics)) {
                currentStatusGraphics = Changes[EDeviceProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
            }
            
            currentStatusGraphics.Remove(statusGraphic);
            
            if (Changes.ContainsKey(EDeviceProperty.StatusGraphics)) {
                Changes[EDeviceProperty.StatusGraphics] = currentStatusGraphics;
            }
            else {
                Changes.Add(EDeviceProperty.StatusGraphics, currentStatusGraphics);
            }

            if (_cacheChanges) {
                return;
            }

            _statusGraphics = currentStatusGraphics;
        }

        internal void ClearStatusGraphics() {
            
            var currentStatusGraphics = new StatusGraphicCollection();
            
            if (Changes.ContainsKey(EDeviceProperty.StatusGraphics)) {
                Changes[EDeviceProperty.StatusGraphics] = currentStatusGraphics;
            }
            else {
                Changes.Add(EDeviceProperty.StatusGraphics, currentStatusGraphics);
            }

            if (_cacheChanges) {
                return;
            }

            _statusGraphics = currentStatusGraphics;
        }

    }

}