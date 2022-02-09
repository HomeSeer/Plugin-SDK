using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <para> NOTE - THIS IS PREVIEW MATERIAL AND WILL NOT FUNCTION UNTIL HS v4.2.0.0 </para>
        /// <para>
        /// A list of <see cref="AbstractHsDevice.Ref"/>s indicating the order of importance for the features of a
        ///  device where 1 is the most important. This helps HomeSeer determine how to display features in the UI.
        /// </para>
        /// <para>
        /// You must call <see cref="IHsController.GetDeviceWithFeaturesByRef"/> to fill this property, otherwise
        ///  this will be an empty list.
        /// </para>
        /// </summary>
        /// <remarks>
        /// This should typically be left to the user to configure, but it is recommended that you pre-configure it
        ///  for users with the best setup possible by adding the <see cref="HsFeature"/>s in the desired order
        ///  when using <see cref="DeviceFactory"/> to create a <see cref="HsDevice"/>.
        /// <para>See <see cref="EFeatureDisplayType"/> for additional info on controlling the way features are displayed.</para>
        /// </remarks>
        public List<int> FeatureDisplayPriority {
            get {
                if (Changes.ContainsKey(EProperty.FeatureDisplayPriority)) {
                    return (List<int>) Changes[EProperty.FeatureDisplayPriority];
                }
                
                return _featureDisplayPriority ?? new List<int>();
            }
            set {

                if (value?.Count != value?.Distinct().Count()) {
                    throw new ArgumentException("List contains duplicate values. Remove the duplicates and try again.");
                }

                if (value?.Count != _assDevices.Count) {
                    throw new ArgumentException($"Number of elements in list does not match the number of associated features for this device - Expected {_assDevices?.Count} : Found {value?.Count}");
                }

                if (! _assDevices.SetEquals(value)) {
                    throw new ArgumentException("Elements in the list do not match the associated features for this device.");
                }

                if (_cacheChanges && value == _featureDisplayPriority) {
                    Changes.Remove(EProperty.FeatureDisplayPriority);
                    return;
                }
                
                if (Changes.ContainsKey(EProperty.FeatureDisplayPriority)) {
                    Changes[EProperty.FeatureDisplayPriority] = value;
                }
                else {
                    Changes.Add(EProperty.FeatureDisplayPriority, value);
                }

                if (_cacheChanges) {
                    return;
                }

                _featureDisplayPriority = value;
            }
        }

        #endregion

        #region Private

        private List<int> _featureDisplayPriority = new List<int>();
        
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

        /// <summary>
        /// Get a collection of <see cref="HsFeature"/>s in the same order defined by <see cref="FeatureDisplayPriority"/>
        /// </summary>
        /// <returns>A List of <see cref="HsFeature"/>s ordered by <see cref="HsFeature.DisplayPriority"/></returns>
        public List<HsFeature> GetFeaturesInDisplayOrder() {
            if (Features.Count == 0 || FeatureDisplayPriority.Count == 0) {
                return new List<HsFeature>();
            }

            var priorityMap = new Dictionary<int, int>();
            for (var i = 0; i < _featureDisplayPriority.Count; i++) {
                var featRef = _featureDisplayPriority[i];
                priorityMap.Add(featRef, i);
            }

            var orderedFeatures = new SortedDictionary<int, HsFeature>();
            foreach (var feature in Features) {
                orderedFeatures.Add(priorityMap[feature.Ref], feature);
            }

            return orderedFeatures.Values.ToList();
        }

    }

}