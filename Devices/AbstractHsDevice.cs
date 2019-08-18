using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// The base implementation of a HomeSeer device.
    /// <para>
    /// Used to represent devices and features as either a <see cref="HsDevice"/> or <see cref="HsFeature"/>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Due to the fact that both <see cref="HsDevice"/>s and <see cref="HsFeature"/>s derive from this class
    ///  some documentation may refer to either as a device. Be careful to ensure you know which one you are working
    ///  with at all times to avoid unexpected InvalidOperationExceptions.
    /// </remarks>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public abstract class AbstractHsDevice {
        
        #region Properties
        
        /// <summary>
        /// A collection of changes to the device/feature since its initialization
        /// </summary>
        /// <remarks>
        /// This acts as a local cache and makes it easier to change multiple things about a device/feature and then
        ///  send all of the changes to HomeSeer as a bundle via <see cref="IHsController.UpdateDeviceByRef"/>
        ///  or <see cref="IHsController.UpdateFeatureByRef"/>
        /// </remarks>
        public Dictionary<EDeviceProperty, object> Changes { get; private set; } = new Dictionary<EDeviceProperty, object>();
        
        /// <summary>
        /// The unique identifier for this device/feature. This is the primary key for devices and features in HomeSeer.
        /// </summary>
        public int Ref { get; } = -1;

        #region Public
        
        /// <summary>
        /// A physical address for the device/feature.
        /// <para>
        /// Use this to store a unique identifier for the physical device this device/feature is associated with.
        /// </para>
        /// </summary>
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
        
        /// <summary>
        /// A set of unique IDs that represent the devices/features that are associated with this device/feature
        /// </summary>
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

        /// <summary>
        /// Type info for this device/feature
        /// </summary>
        /// <remarks>
        /// This is used to describe this device/feature in a manner that is easily understood by UI generation engines
        ///  and other smart home platforms. When these systems can understand what this device/feature is, they are
        ///  better able to tailor the experience of the user to their expectations.
        /// </remarks>
        /// <seealso cref="DeviceTypeInfo"/>
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
        
        /// <summary>
        /// The address of an image that represents the current status of the device/feature
        /// </summary>
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

        /// <summary>
        /// The ID of the interface that is responsible for processing interactions with this device/feature
        /// </summary>
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
        
        /// <summary>
        /// Whether the device/feature is in an invalid state and should display as such to users.
        /// </summary>
        /// <remarks>
        /// Use this as a manual way to flag the device/feature as invalid when the automatic check through
        ///  <see cref="IsValueValid"/> will not produce the desired result. Setting this to TRUE will force the
        ///  device/feature's state to invalid.
        /// </remarks>
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
        
        /// <summary>
        /// The date and time that this device/feature was last updated or changed
        /// </summary>
        public DateTime LastChange {
            get {
                if (Changes.ContainsKey(EDeviceProperty.LastChange)) {
                    return (DateTime) Changes[EDeviceProperty.LastChange];
                }

                return _lastChange;
            }
            set {
                if (value == _lastChange) {
                    Changes.Remove(EDeviceProperty.LastChange);
                    return;
                }
                if (Changes.ContainsKey(EDeviceProperty.LastChange)) {
                    Changes[EDeviceProperty.LastChange] = value;
                }
                else {
                    Changes.Add(EDeviceProperty.LastChange, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _lastChange = value;
            }
        }

        /// <summary>
        /// The primary location of the device/feature according to the locations configured on the user's system
        /// </summary>
        /// <remarks>
        /// Do not set this directly on features. It will be ignored/overwritten in favor of the location set
        ///  on the owning device.
        /// <para>
        /// To optimize the user experience, it is recommended to ask the user which location they wish to assign
        ///  to a device before finishing the inclusion process.
        /// </para>
        /// </remarks>
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

        /// <summary>
        /// The secondary location of the device/feature according to the locations configured on the user's system
        /// </summary>
        /// <remarks>
        /// Do not set this directly on features. It will be ignored/overwritten in favor of the location2 set
        ///  on the owning device.
        /// <para>
        /// To optimize the user experience, it is recommended to ask the user which location they wish to assign
        ///  to a device before finishing the inclusion process.
        /// </para>
        /// </remarks>
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

        /// <summary>
        /// A collection of bit flags used to represent various configuration options for a device/feature.
        /// <para>
        /// It is not recommended to set this directly. Instead, use <see cref="AddMiscFlag"/>,
        ///  <see cref="RemoveMiscFlag"/>, and <see cref="ContainsMiscFlag"/> to interface with this property
        /// </para>
        /// </summary>
        /// <seealso cref="EDeviceMiscFlag"/>
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

        //TODO Name guidelines
        /// <summary>
        /// The name of the device/feature
        /// </summary>
        /// <remarks>
        /// <para>
        /// To optimize the user experience, it is recommended to ask the user what name they wish to assign
        ///  to the device before finishing the inclusion process.
        /// </para>
        /// </remarks>
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
        
        /// <summary>
        /// A memory space available for plugins to store keyed and non-keyed data associated with the device/feature
        /// </summary>
        /// <remarks>
        /// Use this to store device/feature specific configuration options accessed via the DeviceConfig page
        /// </remarks>
        /// <seealso cref="PlugExtraData"/>
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
        
        /// <summary>
        /// The address of an image of the physical device this HomeSeer device/feature is associated with
        /// </summary>
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

        /// <summary>
        /// The type of relationship this device/feature has with other devices/features.
        ///  See <see cref="ERelationship"/> for valid types and more details.
        /// </summary>
        /// <exception cref="DeviceRelationshipException">
        /// Thrown when setting the relationship while there are
        ///  listed devices/features still associated with this device/feature
        /// </exception>
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

        /// <summary>
        /// The current status of the device/feature.
        /// <para>
        /// This is the primary piece of information that users will look at.
        /// </para>
        /// </summary>
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

        //TODO User Access rights
        /// <summary>
        /// A string representation of the HomeSeer user access rights for this device/feature
        /// </summary>
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

        /// <summary>
        /// Notes attached to this device/feature by users
        /// </summary>
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

        /// <summary>
        /// A numeric value representing the current state of the device/feature.
        /// </summary>
        /// <remarks>
        /// Although this is available on devices it should not be set directly. This should only be set on features;
        ///  as the information of the identified primary feature will be automatically pushed to the device to better
        ///  adhere to user experience expectations.
        /// <para>
        /// This value relates to the TargetValue and TargetRange properties on the <see cref="StatusControl"/>
        ///  and <see cref="StatusGraphic"/> classes.
        /// </para>
        /// </remarks>
        public double Value {
            get {
                if (Changes.ContainsKey(EDeviceProperty.Value)) {
                    return (double) Changes[EDeviceProperty.Value];
                }
                
                return _value;
            }
            set {
                if (Math.Abs(value - _value) < 0.001) {
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

        //TODO voice friendly strings documentation
        /// <summary>
        /// A voice friendly command string used to identify this device/feature
        /// </summary>
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
        
        /// <inheritdoc cref="Address"/>
        protected string         _address           = "";
        /// <inheritdoc cref="AssociatedDevices"/>
        protected HashSet<int>   _assDevices        = new HashSet<int>();
        protected bool           _cacheChanges      = false;
        /// <inheritdoc cref="DeviceType"/>
        protected DeviceTypeInfo _deviceType        = new DeviceTypeInfo();
        /// <inheritdoc cref="Image"/>
        protected string         _image             = "";
        /// <inheritdoc cref="Interface"/>
        protected string         _interface         = "";
        /// <inheritdoc cref="IsValueInvalid"/>
        protected bool           _invalidValue;     //= false;
        /// <inheritdoc cref="LastChange"/>
        protected DateTime       _lastChange        = DateTime.Now;
        /// <inheritdoc cref="Location"/>
        protected string         _location          = "Unknown";
        /// <inheritdoc cref="Location2"/>
        protected string         _location2         = "Unknown";
        /// <inheritdoc cref="Misc"/>
        protected uint           _misc              = (uint) EDeviceMiscFlag.SHOW_VALUES;
        /// <inheritdoc cref="Name"/>
        protected string         _name              = "";
        /// <inheritdoc cref="PlugExtraData"/>
        protected PlugExtraData  _plugExtraData     = new PlugExtraData();
        /// <inheritdoc cref="ProductImage"/>
        protected string         _productImage      = "";
        /// <inheritdoc cref="Relationship"/>
        protected ERelationship  _relationship      = ERelationship.NotSet;
        /// <inheritdoc cref="Status"/>
        protected string         _status            = "";
        /// <inheritdoc cref="UserAccess"/>
        protected string         _userAccess        = "Any";
        /// <inheritdoc cref="UserNote"/>
        protected string         _userNote          = "";
        /// <inheritdoc cref="Value"/>
        protected double         _value;            //= 0D;
        /// <inheritdoc cref="VoiceCommand"/>
        protected string         _voiceCommand      = "";
        
        #endregion

        #endregion

        internal AbstractHsDevice() {}

        internal AbstractHsDevice(int featureRef) {
            Ref = featureRef;
        }

        /// <summary>
        /// Clear all changes since initialization and reset the <see cref="Changes"/> property
        /// </summary>
        public void RevertChanges() {
            Changes = new Dictionary<EDeviceProperty, object>();
        }

        /// <summary>
        /// Determine whether the current value is valid.
        /// </summary>
        /// <returns>Always TRUE when not overriden</returns>
        protected virtual bool IsValueValid() {
            return true;
        }

        /// <summary>
        /// Add the specified <see cref="EDeviceMiscFlag"/> to the device/feature
        /// </summary>
        /// <param name="misc">The <see cref="EDeviceMiscFlag"/> to add</param>
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

        /// <summary>
        /// Determine if the device/feature contains the specified <see cref="EDeviceMiscFlag"/>
        /// </summary>
        /// <param name="misc">The <see cref="EDeviceMiscFlag"/> to look for</param>
        /// <returns>
        /// TRUE if the device/feature contains the <see cref="EDeviceMiscFlag"/>,
        ///  FALSE if it does not.
        /// </returns>
        public bool ContainsMiscFlag(EDeviceMiscFlag misc) {
            var currentMisc = _misc;
            if (Changes.ContainsKey(EDeviceProperty.Misc)) {
                currentMisc = (uint) Changes[EDeviceProperty.Misc];
            }
            
            return (currentMisc & (uint) misc) != 0;
        }

        /// <summary>
        /// Remove the specified <see cref="EDeviceMiscFlag"/> from the device/feature
        /// </summary>
        /// <param name="misc">The <see cref="EDeviceMiscFlag"/> to remove</param>
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

        /// <summary>
        /// Clear all <see cref="EDeviceMiscFlag"/>s on the device/feature.
        /// </summary>
        public void ClearMiscFlags() {
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