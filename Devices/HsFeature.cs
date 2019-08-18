using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
// ReSharper disable MemberCanBePrivate.Global

[assembly: InternalsVisibleTo("Scheduler")]

namespace HomeSeer.PluginSdk.Devices {
    
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class HsFeature : AbstractHsDevice {
        
        #region Properties
        
        #region Public

        //TODO don't edit directly remarks
        /// <summary>
        /// A <see cref="StatusControlCollection"/> describing all of the <see cref="StatusControl"/>s associated with
        ///  this feature.
        /// </summary>
        public StatusControlCollection StatusControls {
            get {
                if (Changes.ContainsKey(EDeviceProperty.StatusControls)) {
                    return Changes[EDeviceProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
                }
                
                return _statusControls;
            }
        }

        //TODO don't edit directly remarks
        /// <summary>
        /// A <see cref="StatusGraphicCollection"/> describing all of the <see cref="StatusGraphic"/>s associated with
        ///  this feature.
        /// </summary>
        public StatusGraphicCollection StatusGraphics {
            get {
                if (Changes.ContainsKey(EDeviceProperty.StatusGraphics)) {
                    return Changes[EDeviceProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
                }
                
                return _statusGraphics;
            }
        }

        #endregion

        #region Private
        
        private StatusGraphicCollection _statusGraphics = new StatusGraphicCollection();
        private StatusControlCollection _statusControls = new StatusControlCollection();

        #endregion

        #endregion

        internal HsFeature() { }
        
        /// <summary>
        /// Create a HomeSeer feature with the specified unique ID
        /// </summary>
        /// <param name="featureRef">The unique ID associated with this feature</param>
        public HsFeature(int featureRef) : base(featureRef) { }

        /// <summary>
        /// Make a copy of the feature with a different unique ID.
        /// <para>
        /// This will not duplicate StatusControls or StatusGraphics associated with the device.
        /// </para>
        /// </summary>
        /// <param name="featureRef">The new unique ID for the copy</param>
        /// <returns>A copy of the feature with a new reference ID</returns>
        public HsFeature Duplicate(int featureRef) {
            var dev = new HsFeature(featureRef)
                      {
                          _address        = Address,
                          _assDevices     = AssociatedDevices,
                          _deviceType     = DeviceType,
                          _image          = Image,
                          _productImage   = ProductImage,
                          _interface      = Interface,
                          _lastChange     = LastChange,
                          _location       = Location,
                          _location2      = Location2,
                          _misc           = Misc,
                          _name           = Name,
                          _plugExtraData  = PlugExtraData,
                          _relationship   = Relationship,
                          _status         = Status,
                          _userAccess     = UserAccess,
                          _userNote       = UserNote,
                          _value          = Value,
                          _voiceCommand   = VoiceCommand
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

        /// <summary>
        /// Determine if the feature has a <see cref="StatusControl"/> associated with the specified value
        /// </summary>
        /// <param name="value">The value to look for</param>
        /// <returns>
        /// TRUE if the feature has a <see cref="StatusControl"/> that targets the specified value,
        ///  FALSE if it does not.
        /// </returns>
        public bool HasControlForValue(double value) {
            
            var currentStatusControls = _statusControls ?? new StatusControlCollection();
            if (Changes.ContainsKey(EDeviceProperty.StatusControls)) {
                currentStatusControls = Changes[EDeviceProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
            }

            return currentStatusControls.ContainsValue(value);
        }
        
        /// <summary>
        /// Determine if the feature has a <see cref="StatusControl"/> associated with the specified range of values
        /// </summary>
        /// <param name="range">The range of values to look for</param>
        /// <returns>
        /// TRUE if the feature has at least one <see cref="StatusControl"/> that targets any of the values in the range,
        ///  FALSE if it does not.
        /// </returns>
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
        
        /// <summary>
        /// Determine if the feature has a <see cref="StatusGraphic"/> associated with the specified value
        /// </summary>
        /// <param name="value">The value to look for</param>
        /// <returns>
        /// TRUE if the feature has a <see cref="StatusGraphic"/> that targets the specified value,
        ///  FALSE if it does not.
        /// </returns>
        public bool HasGraphicForValue(double value) {
            
            var currentStatusGraphics = _statusGraphics ?? new StatusGraphicCollection();
            if (Changes.ContainsKey(EDeviceProperty.StatusGraphics)) {
                currentStatusGraphics = Changes[EDeviceProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
            }

            return currentStatusGraphics.ContainsValue(value);
        }
        
        /// <summary>
        /// Determine if the feature has a <see cref="StatusGraphic"/> associated with the specified range of values
        /// </summary>
        /// <param name="range">The range of values to look for</param>
        /// <returns>
        /// TRUE if the feature has at least one <see cref="StatusGraphic"/> that targets any of the values in the range,
        ///  FALSE if it does not.
        /// </returns>
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