using System;
using System.Collections.Generic;
using HomeSeer.PluginSdk.Devices.Controls;
using HomeSeer.PluginSdk.Devices.Identification;

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
        public Dictionary<EProperty, object> Changes { get; private set; } = new Dictionary<EProperty, object>();
        
        /// <summary>
        /// The unique identifier for this device/feature. This is the primary key for devices and features in HomeSeer.
        /// </summary>
        public int Ref { get; private set; } = -1;

        #region Public
        
        /// <summary>
        /// A physical address for the device/feature.
        /// <para>
        /// Use this to store a unique identifier for the physical device this device/feature is associated with.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// Since v1.2.5.0, this field is overloaded with the legacy Code field for backwards compatibility.
        ///  If you are accessing a device/feature that was created using this API then you can safely ignore this remark.
        ///  If you are accessing a device/feature that was created using the HS3 legacy API you may note that this field
        ///  now includes the Code value if it exists. You can get the Code directly by using the <see cref="Code"/> field.
        /// </para>
        /// <para>
        /// This table shows the return value of <see cref="Address"/> based on the value stored in the HS database.
        /// <list type="table">
        ///  <listheader>
        ///   <term>Address Value</term>
        ///   <description>Returns</description>
        ///  </listheader>
        ///  <item>
        ///   <term>Address Only</term>
        ///   <description>Address</description>
        ///  </item>
        ///  <item>
        ///   <term>Code Only</term>
        ///   <description>Code</description>
        ///  </item>
        ///  <item>
        ///   <term>Address and Code</term>
        ///   <description>Address-Code</description>
        ///  </item>
        /// </list>
        /// </para>
        /// <para>
        /// You can use <see cref="GetAddressFromAddressString"/> and <see cref="GetCodeFromAddressString"/> to pull
        ///  the address and code, respectively, from this value.
        /// </para>
        /// </remarks>
        public string Address {
            get {
                if (Changes.ContainsKey(EProperty.Address)) {
                    return (string) Changes[EProperty.Address];
                }
                
                return _address ?? "";
            }
            set {

                if (value == _address) {
                    Changes.Remove(EProperty.Address);
                    return;
                }
                
                if (Changes.ContainsKey(EProperty.Address)) {
                    Changes[EProperty.Address] = value;
                }
                else {
                    Changes.Add(EProperty.Address, value);
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
                if (Changes.ContainsKey(EProperty.AssociatedDevices)) {
                    return Changes[EProperty.AssociatedDevices] as HashSet<int> ?? new HashSet<int>();
                }
                
                return _assDevices;
            }
            set {

                if (value == _assDevices) {
                    Changes.Remove(EProperty.AssociatedDevices);
                    return;
                }
                
                if (Changes.ContainsKey(EProperty.AssociatedDevices)) {
                    Changes[EProperty.AssociatedDevices] = value ?? new HashSet<int>();
                }
                else {
                    Changes.Add(EProperty.AssociatedDevices, value ?? new HashSet<int>());
                }
                
                if (_cacheChanges) {
                    return;
                }
                _assDevices = value ?? new HashSet<int>();
            }
        }

        /// <summary>
        /// Get the code stored in the <see cref="Address"/> string.
        /// </summary>
        /// <remarks>
        /// This field is only used for legacy support and grabs a value from the <see cref="Address"/> field directly.
        ///  The code is grabbed from the <see cref="Address"/> field by using <see cref="GetCodeFromAddressString"/>
        /// </remarks>
        public string Code {
            get {
                if (Changes.ContainsKey(EProperty.Address)) {
                    var addressCode = (string) Changes[EProperty.Address];
                    return GetCodeFromAddressString(addressCode);
                }

                return GetCodeFromAddressString(_address) ?? "";
            }
        }

        /// <summary>
        /// The address of an image that represents the current status of the device/feature
        /// </summary>
        public string Image {
            get {
                if (Changes.ContainsKey(EProperty.Image)) {
                    return (string) Changes[EProperty.Image];
                }

                return _image;
            }
            set {
                if (value == _image) {
                    Changes.Remove(EProperty.Image);
                    return;
                }
                
                if (Changes.ContainsKey(EProperty.Image)) {
                    Changes[EProperty.Image] = value;
                }
                else {
                    Changes.Add(EProperty.Image, value);
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
                if (Changes.ContainsKey(EProperty.Interface)) {
                    return (string) Changes[EProperty.Interface];
                }

                return _interface;
            }
            set {
                if (value == _interface) {
                    Changes.Remove(EProperty.Interface);
                    return;
                }
                if (Changes.ContainsKey(EProperty.Interface)) {
                    Changes[EProperty.Interface] = value;
                }
                else {
                    Changes.Add(EProperty.Interface, value);
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
                if (Changes.ContainsKey(EProperty.InvalidValue)) {
                    return (bool) Changes[EProperty.InvalidValue];
                }

                return _invalidValue || !IsValueValid();
            }
            set {
                if (value == _invalidValue) {
                    Changes.Remove(EProperty.InvalidValue);
                    return;
                }
                if (Changes.ContainsKey(EProperty.InvalidValue)) {
                    Changes[EProperty.InvalidValue] = value;
                }
                else {
                    Changes.Add(EProperty.InvalidValue, value);
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
                if (Changes.ContainsKey(EProperty.LastChange)) {
                    return (DateTime) Changes[EProperty.LastChange];
                }

                return _lastChange;
            }
            set {
                if (value == _lastChange) {
                    Changes.Remove(EProperty.LastChange);
                    return;
                }
                if (Changes.ContainsKey(EProperty.LastChange)) {
                    Changes[EProperty.LastChange] = value;
                }
                else {
                    Changes.Add(EProperty.LastChange, value);
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
                if (Changes.ContainsKey(EProperty.Location)) {
                    return (string) Changes[EProperty.Location];
                }

                return _location;
            }
            set {
                if (value == _location) {
                    Changes.Remove(EProperty.Location);
                    return;
                }
                if (Changes.ContainsKey(EProperty.Location)) {
                    Changes[EProperty.Location] = value;
                }
                else {
                    Changes.Add(EProperty.Location, value);
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
                if (Changes.ContainsKey(EProperty.Location2)) {
                    return (string) Changes[EProperty.Location2];
                }

                return _location2;
            }
            set {
                if (value == _location2) {
                    Changes.Remove(EProperty.Location2);
                    return;
                }
                if (Changes.ContainsKey(EProperty.Location2)) {
                    Changes[EProperty.Location2] = value;
                }
                else {
                    Changes.Add(EProperty.Location2, value);
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
        /// <seealso cref="EMiscFlag"/>
        public uint Misc {
            get {
                if (Changes.ContainsKey(EProperty.Misc)) {
                    return (uint) Changes[EProperty.Misc];
                }

                return _misc;
            }
            set {
                if (value == _misc) {
                    Changes.Remove(EProperty.Misc);
                    return;
                }
                if (Changes.ContainsKey(EProperty.Misc)) {
                    Changes[EProperty.Misc] = value;
                }
                else {
                    Changes.Add(EProperty.Misc, value);
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
                if (Changes.ContainsKey(EProperty.Name)) {
                    return (string) Changes[EProperty.Name];
                }

                return _name;
            }
            set {
                if (value == _name) {
                    Changes.Remove(EProperty.Name);
                    return;
                }
                if (Changes.ContainsKey(EProperty.Name)) {
                    Changes[EProperty.Name] = value;
                }
                else {
                    Changes.Add(EProperty.Name, value);
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
                if (Changes.ContainsKey(EProperty.PlugExtraData)) {
                    return Changes[EProperty.PlugExtraData] as PlugExtraData ?? new PlugExtraData();
                }
                
                return _plugExtraData;
            }
            set {
                
                if (Changes.ContainsKey(EProperty.PlugExtraData)) {
                    Changes[EProperty.PlugExtraData] = value;
                }
                else {
                    Changes.Add(EProperty.PlugExtraData, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _plugExtraData = value;
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
                if (Changes.ContainsKey(EProperty.Relationship)) {
                    return (ERelationship) Changes[EProperty.Relationship];
                }
                return _relationship;
            }
            set {

                var currentAssociatedDeviceList = _assDevices ?? new HashSet<int>();
                if (Changes.ContainsKey(EProperty.AssociatedDevices)) {
                    currentAssociatedDeviceList = (HashSet<int>) Changes[EProperty.AssociatedDevices];
                }

                if (currentAssociatedDeviceList.Count > 0) {
                    throw new DeviceRelationshipException("Please clear this devices association with other devices before changing its relationship type.");
                }
                
                if (value == _relationship) {
                    Changes.Remove(EProperty.Relationship);
                    return;
                }
                if (Changes.ContainsKey(EProperty.Relationship)) {
                    Changes[EProperty.Relationship] = value;
                }
                else {
                    Changes.Add(EProperty.Relationship, value);
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
                if (Changes.ContainsKey(EProperty.Status)) {
                    return (string) Changes[EProperty.Status];
                }
                
                return _status;
            }
            set {
                if (value == _status) {
                    Changes.Remove(EProperty.Status);
                    return;
                }
                if (Changes.ContainsKey(EProperty.Status)) {
                    Changes[EProperty.Status] = value;
                }
                else {
                    Changes.Add(EProperty.Status, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _status = value ?? "";
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
        /// <seealso cref="Identification.TypeInfo"/>
        public TypeInfo TypeInfo {
            get {
                if (Changes.ContainsKey(EProperty.DeviceType)) {
                    return (TypeInfo) Changes[EProperty.DeviceType];
                }

                return _typeInfo;
            }
            set {
                if (value == _typeInfo) {
                    Changes.Remove(EProperty.DeviceType);
                    return;
                }
                
                if (Changes.ContainsKey(EProperty.DeviceType)) {
                    Changes[EProperty.DeviceType] = value;
                }
                else {
                    Changes.Add(EProperty.DeviceType, value);
                }
                
                if (_cacheChanges) {
                    return;
                }
                _typeInfo = value ?? new TypeInfo();
            }
        }

        //TODO User Access rights
        /// <summary>
        /// A string representation of the HomeSeer user access rights for this device/feature
        /// </summary>
        /// <remarks>This is typically configured by users and can be safely ignored when creating a <see cref="HsDevice"/> or <see cref="HsFeature"/></remarks>
        public string UserAccess {
            get {
                if (Changes.ContainsKey(EProperty.UserAccess)) {
                    return (string) Changes[EProperty.UserAccess];
                }

                return _userAccess;
            }
            set {
                if (value == _userAccess) {
                    Changes.Remove(EProperty.UserAccess);
                    return;
                }
                if (Changes.ContainsKey(EProperty.UserAccess)) {
                    Changes[EProperty.UserAccess] = value;
                }
                else {
                    Changes.Add(EProperty.UserAccess, value);
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
        /// <remarks>This is typically configured by users and can be safely ignored when creating a <see cref="HsDevice"/> or <see cref="HsFeature"/></remarks>
        public string UserNote {
            get {
                if (Changes.ContainsKey(EProperty.UserNote)) {
                    return (string) Changes[EProperty.UserNote];
                }

                return _userNote;
            }
            set {
                if (value == _userNote) {
                    Changes.Remove(EProperty.UserNote);
                    return;
                }
                if (Changes.ContainsKey(EProperty.UserNote)) {
                    Changes[EProperty.UserNote] = value;
                }
                else {
                    Changes.Add(EProperty.UserNote, value);
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
                if (Changes.ContainsKey(EProperty.Value)) {
                    return (double) Changes[EProperty.Value];
                }
                
                return _value;
            }
            set {
                if (Math.Abs(value - _value) < 0.001) {
                    Changes.Remove(EProperty.Value);
                    return;
                }
                if (Changes.ContainsKey(EProperty.Value)) {
                    Changes[EProperty.Value] = value;
                }
                else {
                    Changes.Add(EProperty.Value, value);
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
                if (Changes.ContainsKey(EProperty.VoiceCommand)) {
                    return (string) Changes[EProperty.VoiceCommand];
                }

                return _voiceCommand ?? "";
            }
            set {
                if (value == _voiceCommand) {
                    Changes.Remove(EProperty.VoiceCommand);
                    return;
                }
                if (Changes.ContainsKey(EProperty.VoiceCommand)) {
                    Changes[EProperty.VoiceCommand] = value;
                }
                else {
                    Changes.Add(EProperty.VoiceCommand, value);
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
        /// <summary>
        /// Flag used to indicate whether to cache changes in <see cref="Changes"/> when setting properties and not adjust
        ///  the underlying value or to cache changes in <see cref="Changes"/> and adjust the underlying value.
        /// </summary>
        /// <remarks>
        /// Setting this to TRUE allows you to quickly revert changes made by discarding <see cref="Changes"/>
        /// </remarks>
        protected bool                       _cacheChanges           = false;
        /// <inheritdoc cref="Image"/>
        protected string         _image             = "";
        /// <inheritdoc cref="Interface"/>
        protected string         _interface         = "";
        /// <inheritdoc cref="IsValueInvalid"/>
        protected bool           _invalidValue;     //= false;
        /// <inheritdoc cref="LastChange"/>
        protected DateTime       _lastChange        = DateTime.Now;
        /// <inheritdoc cref="Location"/>
        protected string         _location          = "";
        /// <inheritdoc cref="Location2"/>
        protected string         _location2         = "";
        /// <inheritdoc cref="Misc"/>
        protected uint           _misc              = (uint) EMiscFlag.ShowValues;
        /// <inheritdoc cref="Name"/>
        protected string         _name              = "";
        /// <inheritdoc cref="PlugExtraData"/>
        protected PlugExtraData  _plugExtraData     = new PlugExtraData();
        /// <inheritdoc cref="Relationship"/>
        protected ERelationship  _relationship      = ERelationship.NotSet;
        /// <inheritdoc cref="Status"/>
        protected string         _status            = "";
        /// <inheritdoc cref="TypeInfo"/>
        protected TypeInfo       _typeInfo          = new TypeInfo();
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

        /// <summary>
        /// Create a new AbstractHsDevice with default properties
        /// </summary>
        protected AbstractHsDevice() {}

        /// <summary>
        /// Create a new AbstractHsDevice with a specific uniqueRef
        /// </summary>
        /// <param name="uniqueRef">The unique ID as an integer</param>
        protected AbstractHsDevice(int uniqueRef) {
            Ref = uniqueRef;
        }

        /// <summary>
        /// Clear all changes since initialization and reset the <see cref="Changes"/> property
        /// </summary>
        public void RevertChanges() {
            Changes = new Dictionary<EProperty, object>();
        }

        /// <summary>
        /// Determine whether the current value is valid.
        /// </summary>
        /// <returns>Always TRUE when not overriden</returns>
        protected virtual bool IsValueValid() {
            return true;
        }

        /// <summary>
        /// Add the specified <see cref="EMiscFlag"/> to the device/feature
        /// </summary>
        /// <param name="misc">The <see cref="EMiscFlag"/> to add</param>
        public void AddMiscFlag(EMiscFlag misc) {
            
            var currentMisc = _misc;
            if (Changes.ContainsKey(EProperty.Misc)) {
                currentMisc = (uint) Changes[EProperty.Misc];
            }
            
            var tempMisc = currentMisc | (uint) misc;
            
            if (Changes.ContainsKey(EProperty.Misc)) {
                Changes[EProperty.Misc] = tempMisc;
            }
            else {
                Changes.Add(EProperty.Misc, tempMisc);
            }

            if (_cacheChanges) {
                return;
            }
            
            _misc = tempMisc;
        }

        /// <summary>
        /// Determine if the device/feature contains the specified <see cref="EMiscFlag"/>
        /// </summary>
        /// <param name="misc">The <see cref="EMiscFlag"/> to look for</param>
        /// <returns>
        /// TRUE if the device/feature contains the <see cref="EMiscFlag"/>,
        ///  FALSE if it does not.
        /// </returns>
        public bool ContainsMiscFlag(EMiscFlag misc) {
            var currentMisc = _misc;
            if (Changes.ContainsKey(EProperty.Misc)) {
                currentMisc = (uint) Changes[EProperty.Misc];
            }
            
            return (currentMisc & (uint) misc) != 0;
        }

        /// <summary>
        /// Remove the specified <see cref="EMiscFlag"/> from the device/feature
        /// </summary>
        /// <param name="misc">The <see cref="EMiscFlag"/> to remove</param>
        public void RemoveMiscFlag(EMiscFlag misc) {
            var currentMisc = _misc;
            if (Changes.ContainsKey(EProperty.Misc)) {
                currentMisc = (uint) Changes[EProperty.Misc];
            }
            
            var tempMisc = currentMisc ^ (uint) misc;
            
            if (Changes.ContainsKey(EProperty.Misc)) {
                Changes[EProperty.Misc] = tempMisc;
            }
            else {
                Changes.Add(EProperty.Misc, tempMisc);
            }

            if (_cacheChanges) {
                return;
            }
            
            _misc = tempMisc;
        }

        /// <summary>
        /// Clear all <see cref="EMiscFlag"/>s on the device/feature.
        /// </summary>
        public void ClearMiscFlags() {
            if (Changes.ContainsKey(EProperty.Misc)) {
                Changes[EProperty.Misc] = (uint) 0;
            }
            else {
                Changes.Add(EProperty.Misc, (uint) 0);
            }

            if (_cacheChanges) {
                return;
            }

            _misc = 0;
        }

        /// <summary>
        /// Get the value for any combination of <see cref="EMiscFlag"/>s
        /// </summary>
        /// <param name="misc"><see cref="EMiscFlag"/>s to combine</param>
        /// <returns>A uint representing the combined <see cref="EMiscFlag"/>s</returns>
        public static uint GetMiscForFlags(params EMiscFlag[] misc) {
            uint finalMisc = 0;
            foreach (var flag in misc) {
                finalMisc |= (uint) flag;
            }

            return finalMisc;
        }

        
        /// <summary>
        /// Get the address from an address-code string.
        /// </summary>
        /// <remarks>
        /// HS3 supported an Address and Code field, but the Code field has been deprecated. The Address and Code fields
        ///  used to also be combined into a single string with the format of ${ADDRESS}-${CODE}.
        ///  The pseudocode for this is "${ADDRESS}-${CODE}".Trim('-');
        ///  To maintain backwards compatibility support, the Address field will be overloaded with the Code for
        ///  devices created using HS3. Use this method to get the address from the returned address-code string.
        /// </remarks>
        /// <param name="addressString">The <see cref="Address"/>-Code value string to parse</param>
        /// <returns>The Address value from the string</returns>
        public static string GetAddressFromAddressString(string addressString) {
            if (string.IsNullOrWhiteSpace(addressString)) {
                //Return null or empty address strings as empty
                return "";
            }

            if (!addressString.Contains("-")) {
                //Return the whole address string because it is probably just an address when it doesn't contain a -
                return addressString;
            }

            var addressParts = addressString.Split('-');
            if (addressParts.Length < 2) {
                //Return the address string trimmed of - if splitting it at - does not produce 2 or more strings.
                // This means that the string likely has the - at the beginning or the end of the string.
                // We don't know if it was just an address or just a code.
                return addressString.Trim('-');
            }

            //Return the first element in the address parts because the address is the string before the -
            // IE ADDRESS-CODE
            return addressParts[0];
        }
        
        /// <summary>
        /// Get the code from an address-code string.
        /// </summary>
        /// <remarks>
        /// HS3 supported an Address and Code field, but the Code field has been deprecated. The Address and Code fields
        ///  used to also be combined into a single string with the format of ${ADDRESS}-${CODE}.
        ///  The pseudocode for this is "${ADDRESS}-${CODE}".Trim('-');
        ///  To maintain backwards compatibility support, the Address field will be overloaded with the Code for
        ///  devices created using HS3. Use this method to get the address from the returned address-code string.
        /// </remarks>
        /// <param name="addressString">The <see cref="Address"/>-Code value string to parse</param>
        /// <returns>The Code value from the string</returns>
        public static string GetCodeFromAddressString(string addressString) {
            if (string.IsNullOrWhiteSpace(addressString)) {
                //Return null or empty address strings as empty
                return "";
            }

            if (!addressString.Contains("-")) {
                //Return an empty string because it is probably just an address when it doesn't contain a -
                return "";
            }

            var addressParts = addressString.Split('-');
            if (addressParts.Length < 2) {
                //Return the address string trimmed of - if splitting it at - does not produce 2 or more strings.
                // This means that the string likely has the - at the beginning or the end of the string.
                // We don't know if it was just an address or just a code.
                return addressString.Trim('-');
            }

            //Return the second element in the address parts because the code is the string after the -
            // IE ADDRESS-CODE
            return addressParts[1];
        }

    }

}