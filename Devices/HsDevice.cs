using System;
using System.Collections.Generic;
using HomeSeer.PluginSdk.Devices.Identification;

// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HomeSeer.PluginSdk.Devices {

    //TODO HsDevice Remarks and Examples
    /// <summary>
    /// A device connected to a HomeSeer system. This is the top level item displayed to users when
    ///  they are looking at the devices connected to their system.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class HsDevice : AbstractHsDevice {
        
        #region Properties

        #region Public
        
        /// <summary>
        /// A list of <see cref="HsFeature"/>s that make up the device.
        /// </summary>
        public List<HsFeature> Features { get; } = new List<HsFeature>();

        /// <summary>
        /// The order of priority for the features of the device. This helps HomeSeer determine the "primary" feature
        ///  for a device. Order the refs for associated features according to their importance to the user.
        /// </summary>
        /// <remarks>
        /// PLACEHOLDER - This is a Work In Progress
        /// </remarks>
        [Obsolete("This is a work in progress and use may produce unexpected behavior.")]
        public List<int> FeaturePriority {
            get {
                if (Changes.ContainsKey(EProperty.FeaturePriority)) {
                    return (List<int>) Changes[EProperty.FeaturePriority];
                }
                
                return _featurePriority ?? new List<int>();
            }
            set {

                if (value == _featurePriority) {
                    Changes.Remove(EProperty.FeaturePriority);
                    return;
                }
                
                if (Changes.ContainsKey(EProperty.FeaturePriority)) {
                    Changes[EProperty.FeaturePriority] = value;
                }
                else {
                    Changes.Add(EProperty.FeaturePriority, value);
                }

                if (_cacheChanges) {
                    return;
                }
                _featurePriority = value ?? new List<int>();
            }
        }

        #endregion

        #region Private

        private List<int> _featurePriority;
        
        #endregion

        #endregion

        internal HsDevice() { }
        
        /// <summary>
        /// Create a HomeSeer device with the specified unique ID
        /// </summary>
        /// <param name="deviceRef">The unique ID associated with this device</param>
        public HsDevice(int deviceRef) : base(deviceRef) { }

        /// <summary>
        /// Make a copy of the device with a different unique ID.
        /// <para>
        /// This will not duplicate features associated with the device.
        /// </para>
        /// </summary>
        /// <param name="deviceRef">The new unique ID for the copy</param>
        /// <returns>A copy of the device with a new reference ID</returns>
        public HsDevice Duplicate(int deviceRef) {
            var dev = new HsDevice(deviceRef)
                      {
                          _address        = Address,
                          _assDevices     = AssociatedDevices,
                          _typeInfo     = TypeInfo,
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

        /// <summary>
        /// Get the first feature of the specified type associated with this device.
        /// </summary>
        /// <param name="featureType">The <see cref="TypeInfo"/> describing the desired feature</param>
        /// <returns>The feature associated with the device that matches that specified featureType</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if there are no features or if a feature with
        ///  the specified featureType was not found.
        /// </exception>
        public HsFeature GetFeatureByType(TypeInfo featureType) {

            if (Features.Count == 0) {
                throw new KeyNotFoundException("There are no features on this device");
            }

            foreach (var feature in Features) {
                var cFeatureType = feature.TypeInfo;
                if (cFeatureType.ApiType != featureType.ApiType) {
                    continue;
                }
                if (cFeatureType.Type != featureType.Type) {
                    continue;
                }
                if (cFeatureType.SubType != featureType.SubType) {
                    continue;
                }

                return feature;
            }
            
            throw new KeyNotFoundException("There are no features of that type on this device");
        }

    }

}