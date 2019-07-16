using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSeer.PluginSdk.Devices {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class NewFeatureData {
        
        public Dictionary<EDeviceProperty, object> Feature = new Dictionary<EDeviceProperty, object>();
        
        internal NewFeatureData(int deviceRef, HsDevice feature) {
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