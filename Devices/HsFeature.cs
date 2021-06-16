using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HomeSeer.PluginSdk.Devices.Controls;
using HomeSeer.PluginSdk.Devices.Identification;

// ReSharper disable MemberCanBePrivate.Global

[assembly: InternalsVisibleTo("Scheduler")]

namespace HomeSeer.PluginSdk.Devices {
    
    //TODO HsFeature example and remarks
    /// <summary>
    /// A feature of a device connected to a HomeSeer system. It encapsulates a specific component of functionality
    ///  available to the device it is associated with.
    /// <para>
    /// For example: A battery powered device should have a feature associated with it that handles only battery
    ///  related information and controls. This feature should not contain anything having to do with any other
    ///  component of that device.
    /// </para>
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class HsFeature : AbstractHsDevice {
        
        #region Properties
        
        #region Public

        /// <summary>
        /// A list of strings used in the displayed status for the <see cref="HsFeature"/>. The strings in this list
        ///  are used to replace the replacement tokens corresponding to each index in the status - @%INDEX@
        /// </summary>
        public List<string> AdditionalStatusData {
            get {
                if (Changes.ContainsKey(EProperty.AdditionalStatusData)) {
                    return Changes[EProperty.AdditionalStatusData] as List<string> ?? new List<string>();
                }
                
                return _additionalStatusData ?? new List<string>();
            }
            set {

                if (value == _additionalStatusData) {
                    Changes.Remove(EProperty.AdditionalStatusData);
                    return;
                }
                
                if (Changes.ContainsKey(EProperty.AdditionalStatusData)) {
                    Changes[EProperty.AdditionalStatusData] = value;
                }
                else {
                    Changes.Add(EProperty.AdditionalStatusData, value);
                }

                if (_cacheChanges) {
                    return;
                }
                _additionalStatusData = value ?? new List<string>();
            }
        }

        /// <summary>
        /// <para> NOTE - THIS IS PREVIEW MATERIAL AND WILL NOT FUNCTION UNTIL HS v4.2.0.0 </para>
        /// <para>
        /// The priority of the feature when being considered for display where 1 is the most important.
        /// </para>
        /// </summary>
        /// <remarks>
        /// This is property is read-only.
        /// Modify <see cref="HsDevice.FeatureDisplayPriority"/> to change this property on HS.
        /// </remarks>
        public int DisplayPriority => _displayPriority;

        /// <summary>
        /// <para> NOTE - THIS IS PREVIEW MATERIAL AND WILL NOT FUNCTION UNTIL HS v4.2.0.0 </para>
        /// The <see cref="EFeatureDisplayType"/> for a feature.
        /// </summary>
        /// <remarks>This is used to help HS determine how it should be displayed to the user</remarks>
        public EFeatureDisplayType DisplayType {
            get {
                if (Changes.ContainsKey(EProperty.FeatureDisplayType)) {
                    try {
                        return (EFeatureDisplayType) Changes[EProperty.FeatureDisplayType];
                    }
                    catch (InvalidCastException) {
                        return EFeatureDisplayType.Normal;
                    }
                    
                }
                
                return _displayType < 0 ? EFeatureDisplayType.Normal : (EFeatureDisplayType) _displayType;
            }
            set {
                if (value == (EFeatureDisplayType) _displayType) {
                    Changes.Remove(EProperty.FeatureDisplayType);
                    return;
                }
                
                if (Changes.ContainsKey(EProperty.FeatureDisplayType)) {
                    Changes[EProperty.FeatureDisplayType] = (int) value;
                }
                else {
                    Changes.Add(EProperty.FeatureDisplayType, (int) value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _displayType = (int) value;
            }
        }

        /// <summary>
        /// A <see cref="StatusControlCollection"/> describing all of the <see cref="StatusControl"/>s associated with
        ///  this feature.
        /// </summary>
        /// <remarks>
        /// This is read only because HomeSeer needs to index <see cref="StatusControls"/> for events and other automated behaviors.
        ///  To edit these use methods in <see cref="IHsController"/>
        /// </remarks>
        public StatusControlCollection StatusControls {
            get {
                if (Changes.ContainsKey(EProperty.StatusControls)) {
                    return Changes[EProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
                }
                
                return _statusControls;
            }
        }

        /// <summary>
        /// A <see cref="StatusGraphicCollection"/> describing all of the <see cref="StatusGraphic"/>s associated with
        ///  this feature.
        /// </summary>
        /// <remarks>
        /// This is read only because HomeSeer needs to index <see cref="StatusGraphics"/> for events and other automated behaviors.
        ///  To edit these use methods in <see cref="IHsController"/>
        /// </remarks>
        public StatusGraphicCollection StatusGraphics {
            get {
                if (Changes.ContainsKey(EProperty.StatusGraphics)) {
                    return Changes[EProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
                }
                
                return _statusGraphics;
            }
        }

        #endregion

        #region Private
        
        private List<string> _additionalStatusData = new List<string>();

        private int _displayPriority = 0;
        private int _displayType;

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
                          _typeInfo     = TypeInfo,
                          _image          = Image,
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

        /// <summary>
        /// Get the additional data token corresponding to the specified index $%tokenIndex$
        /// </summary>
        /// <remarks>
        /// This is used as a replacement token for the status of a feature. Tokens are replaced with data from
        ///  <see cref="AdditionalStatusData"/>
        /// </remarks>
        /// <param name="tokenIndex">The index for the token</param>
        /// <returns>An additional data token used in the status eg for a tokenIndex of 0 returns $%0$</returns>
        public static string GetAdditionalDataToken(int tokenIndex) {
            return $"$%{tokenIndex}$";
        }
        
        /// <summary>
        /// Create a <see cref="ControlEvent"/> for the given value based on this features defined controls
        /// </summary>
        /// <param name="value">The value to set the feature to</param>
        /// <returns>A <see cref="ControlEvent"/> with info from associated <see cref="StatusControl"/> and <see cref="StatusGraphic"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when there is no <see cref="StatusControl"/> associated with the specified value</exception>
        public ControlEvent CreateControlEvent(double value) {
            if (!HasControlForValue(value)) {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            var sc = StatusControls[value];
            var dce = sc.CreateControlEvent(Ref, value);
            if (!HasGraphicForValue(value)) {
                return dce;
            }
            
            var sg = StatusGraphics[value];
            if (string.IsNullOrWhiteSpace(sg.Label)) {
                return dce;
            }

            dce.Label = sg.GetLabelForValue(value);
            
            return dce;
        }

        /// <inheritdoc cref="AbstractHsDevice.IsValueValid"/>
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

            if (Changes.ContainsKey(EProperty.AssociatedDevices)) {
                associatedDevices = Changes[EProperty.AssociatedDevices] as HashSet<int> ?? new HashSet<int>();
            }

            if (associatedDevices.Count > 0) {
                if (Changes.ContainsKey(EProperty.Relationship)) {
                    var cachedRelationshipChange = (ERelationship) Changes[EProperty.Relationship];
                    if (cachedRelationshipChange == ERelationship.Device) {
                        throw new DeviceRelationshipException("This device is already a parent device with children.  Remove its associations before converting it to a child device.");
                    }
                }
                else if (_relationship == ERelationship.Device) {
                    throw new DeviceRelationshipException("This device is already a parent device with children.  Remove its associations before converting it to a child device.");
                }
            }
            
            var updatedDeviceList = new HashSet<int> {deviceRef};

            if (Changes.ContainsKey(EProperty.Relationship)) {
                Changes[EProperty.Relationship] = ERelationship.Feature;
            }
            else {
                Changes.Add(EProperty.Relationship, ERelationship.Feature);
            }

            if (Changes.ContainsKey(EProperty.AssociatedDevices)) {
                Changes[EProperty.AssociatedDevices] = updatedDeviceList;
            }
            else {
                Changes.Add(EProperty.AssociatedDevices, updatedDeviceList);
            }

            if (_cacheChanges) {
                return;
            }

            _assDevices = updatedDeviceList;
            _relationship = ERelationship.Feature;
        }

        internal void AddStatusControl(StatusControl statusControl) {

            var currentStatusControls = _statusControls ?? new StatusControlCollection();
            if (Changes.ContainsKey(EProperty.StatusControls)) {
                currentStatusControls = Changes[EProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
            }
            
            currentStatusControls.Add(statusControl);
            
            if (Changes.ContainsKey(EProperty.StatusControls)) {
                Changes[EProperty.StatusControls] = currentStatusControls;
            }
            else {
                Changes.Add(EProperty.StatusControls, currentStatusControls);
            }

            if (_cacheChanges) {
                return;
            }

            _statusControls = currentStatusControls;
        }

        internal void AddStatusControls(List<StatusControl> statusControls) {
            var currentStatusControls = _statusControls ?? new StatusControlCollection();
            if (Changes.ContainsKey(EProperty.StatusControls)) {
                currentStatusControls = Changes[EProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
            }
            
            currentStatusControls.AddRange(statusControls);
            
            if (Changes.ContainsKey(EProperty.StatusControls)) {
                Changes[EProperty.StatusControls] = currentStatusControls;
            }
            else {
                Changes.Add(EProperty.StatusControls, currentStatusControls);
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
            if (Changes.ContainsKey(EProperty.StatusControls)) {
                currentStatusControls = Changes[EProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
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
            if (Changes.ContainsKey(EProperty.StatusControls)) {
                currentStatusControls = Changes[EProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
            }

            return currentStatusControls.ContainsValue(range.Min) || currentStatusControls.ContainsValue(range.Max);
        }

        internal void RemoveStatusControl(StatusControl statusControl) {
            
            var currentStatusControls = _statusControls ?? new StatusControlCollection();
            if (Changes.ContainsKey(EProperty.StatusControls)) {
                currentStatusControls = Changes[EProperty.StatusControls] as StatusControlCollection ?? new StatusControlCollection();
            }
            
            currentStatusControls.Remove(statusControl);
            
            if (Changes.ContainsKey(EProperty.StatusControls)) {
                Changes[EProperty.StatusControls] = currentStatusControls;
            }
            else {
                Changes.Add(EProperty.StatusControls, currentStatusControls);
            }

            if (_cacheChanges) {
                return;
            }

            _statusControls = currentStatusControls;
        }

        internal void ClearStatusControls() {
            
            var currentStatusControls = new StatusControlCollection();
            
            if (Changes.ContainsKey(EProperty.StatusControls)) {
                Changes[EProperty.StatusControls] = currentStatusControls;
            }
            else {
                Changes.Add(EProperty.StatusControls, currentStatusControls);
            }

            if (_cacheChanges) {
                return;
            }

            _statusControls = currentStatusControls;
        }

        internal void AddStatusGraphic(StatusGraphic statusGraphic) {
            
            var currentStatusGraphics = _statusGraphics ?? new StatusGraphicCollection();
            if (Changes.ContainsKey(EProperty.StatusGraphics)) {
                currentStatusGraphics = Changes[EProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
            }
            
            currentStatusGraphics.Add(statusGraphic);
            
            if (Changes.ContainsKey(EProperty.StatusGraphics)) {
                Changes[EProperty.StatusGraphics] = currentStatusGraphics;
            }
            else {
                Changes.Add(EProperty.StatusGraphics, currentStatusGraphics);
            }

            if (_cacheChanges) {
                return;
            }

            _statusGraphics = currentStatusGraphics;
        }
        
        internal void AddStatusGraphics(List<StatusGraphic> statusGraphics) {
            var currentStatusGraphics = _statusGraphics ?? new StatusGraphicCollection();
            if (Changes.ContainsKey(EProperty.StatusGraphics)) {
                currentStatusGraphics = Changes[EProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
            }
            
            currentStatusGraphics.AddRange(statusGraphics);
            
            if (Changes.ContainsKey(EProperty.StatusGraphics)) {
                Changes[EProperty.StatusGraphics] = currentStatusGraphics;
            }
            else {
                Changes.Add(EProperty.StatusGraphics, currentStatusGraphics);
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
            if (Changes.ContainsKey(EProperty.StatusGraphics)) {
                currentStatusGraphics = Changes[EProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
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
            if (Changes.ContainsKey(EProperty.StatusGraphics)) {
                currentStatusGraphics = Changes[EProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
            }

            return currentStatusGraphics.ContainsValue(range.Min) || currentStatusGraphics.ContainsValue(range.Max);
        }

        internal void RemoveStatusGraphic(StatusGraphic statusGraphic) {
            
            var currentStatusGraphics = _statusGraphics ?? new StatusGraphicCollection();
            if (Changes.ContainsKey(EProperty.StatusGraphics)) {
                currentStatusGraphics = Changes[EProperty.StatusGraphics] as StatusGraphicCollection ?? new StatusGraphicCollection();
            }
            
            currentStatusGraphics.Remove(statusGraphic);
            
            if (Changes.ContainsKey(EProperty.StatusGraphics)) {
                Changes[EProperty.StatusGraphics] = currentStatusGraphics;
            }
            else {
                Changes.Add(EProperty.StatusGraphics, currentStatusGraphics);
            }

            if (_cacheChanges) {
                return;
            }

            _statusGraphics = currentStatusGraphics;
        }

        internal void ClearStatusGraphics() {
            
            var currentStatusGraphics = new StatusGraphicCollection();
            
            if (Changes.ContainsKey(EProperty.StatusGraphics)) {
                Changes[EProperty.StatusGraphics] = currentStatusGraphics;
            }
            else {
                Changes.Add(EProperty.StatusGraphics, currentStatusGraphics);
            }

            if (_cacheChanges) {
                return;
            }

            _statusGraphics = currentStatusGraphics;
        }

    }

}