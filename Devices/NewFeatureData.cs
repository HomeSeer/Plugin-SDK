using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// A collection of information describing a new feature that needs to be created in HomeSeer.
    /// <para>
    /// Created through <see cref="FeatureFactory.PrepareforHs()"/>
    /// </para>
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class NewFeatureData {

        /// <summary>
        /// A collection of properties describing the feature to be created.
        /// </summary>
        public Dictionary<EDeviceProperty, object> Feature;
        
        internal NewFeatureData(int deviceRef, HsFeature feature) {
            if (feature == null) {
                throw new ArgumentNullException(nameof(feature));
            }
            
            if (deviceRef <= 0) {
                throw new ArgumentOutOfRangeException(nameof(deviceRef));
            }
            
            Feature = feature.Changes;
            Feature[EDeviceProperty.Relationship] = ERelationship.Feature;
            Feature[EDeviceProperty.AssociatedDevices] = new HashSet<int>() {deviceRef};
        }
        
        internal NewFeatureData(HsFeature feature) {
            if (feature == null) {
                throw new ArgumentNullException(nameof(feature));
            }

            Feature = feature.Changes;

            if (!(Feature[EDeviceProperty.AssociatedDevices] is HashSet<int> assDevices) || 
                assDevices.Count == 0 || assDevices.Count >= 2 || assDevices.First() <= 0) {
                throw new ArgumentOutOfRangeException(nameof(feature), 
                                                      "Invalid associated device ref");
            }

            Feature[EDeviceProperty.Relationship] = ERelationship.Feature;
        }

    }

}