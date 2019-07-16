using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSeer.PluginSdk.Devices {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class HsDevice : AbstractHsDevice {
        
        #region Properties

        #region Public
        
        public List<HsDevice> Features { get; internal set; } = new List<HsDevice>();

        #endregion

        #region Private
        
        #endregion

        #endregion

        public HsDevice() { }
        public HsDevice(int deviceRef) : base(deviceRef) { }
        public HsDevice(int deviceRef, DateTime lastChange) : base(deviceRef, lastChange) { }

        public HsDevice Duplicate(int deviceRef, HsDevice source) {
            var dev = new HsDevice(deviceRef, source.LastChange)
                      {
                          _address        = source.Address,
                          _assDevices     = source.AssociatedDevices,
                          _deviceType     = source.DeviceType,
                          _image          = source.Image,
                          _productImage   = source.ProductImage,
                          _interface      = source.Interface,
                          _location       = source.Location,
                          _location2      = source.Location2,
                          _misc           = source.Misc,
                          _name           = source.Name,
                          _plugExtraData  = source.PlugExtraData,
                          _relationship   = source.Relationship,
                          _status         = source.Status,
                          _userAccess     = source.UserAccess,
                          _userNote       = source.UserNote,
                          _value          = source.Value,
                          _voiceCommand   = source.VoiceCommand
                      };
            return dev;
        }

        public HsDevice GetFeatureByType(DeviceTypeInfo featureType) {

            if (Features.Count == 0) {
                throw new KeyNotFoundException("There are no features on this device");
            }

            foreach (var feature in Features) {
                var cFeatureType = feature.DeviceType;
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