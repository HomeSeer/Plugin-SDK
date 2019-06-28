using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSeer.PluginSdk.Devices {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class NewDeviceData {

        public Dictionary<EDeviceProperty, object> Device = new Dictionary<EDeviceProperty, object>();
        public List<Dictionary<EDeviceProperty, object>> FeatureData = new List<Dictionary<EDeviceProperty, object>>();
        
        internal NewDeviceData(HsDevice device, List<HsDevice> features) {
            Device = device.Changes;
            FeatureData = features.Select(f => f.Changes).ToList();
        }

    }

}