using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSeer.PluginSdk.Devices {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class NewDeviceData {

        private Dictionary<EDeviceProperty, object> _deviceData = new Dictionary<EDeviceProperty, object>();
        private List<Dictionary<EDeviceProperty, object>> _featureData = new List<Dictionary<EDeviceProperty, object>>();
        
        internal NewDeviceData(HsDevice device, List<HsDevice> features) {
            _deviceData = device.Changes;
            _featureData = features.Select(f => f.Changes).ToList();
        }
        
        

    }

}