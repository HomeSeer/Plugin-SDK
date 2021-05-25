using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NotAccessedField.Global

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// A collection of information describing a new device that needs to be created in HomeSeer.
    /// <para>
    /// Created through <see cref="DeviceFactory.PrepareForHs"/>
    /// </para>
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class NewDeviceData {

        /// <summary>
        /// A collection of properties describing the device to be created.
        /// </summary>
        public Dictionary<EProperty, object> Device;
        /// <summary>
        /// A list of collections of properties describing features associated with the device.
        /// </summary>
        public List<Dictionary<EProperty, object>> FeatureData;
        
        internal NewDeviceData(HsDevice device, List<HsFeature> features) {
            Device = device.Changes;
            FeatureData = features?.Select(f => f.Changes).ToList() ?? new List<Dictionary<EProperty, object>>();
        }

    }

}