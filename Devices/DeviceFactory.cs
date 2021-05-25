using System;
using System.Collections.Generic;
using System.Linq;
using HomeSeer.PluginSdk.Devices.Identification;

namespace HomeSeer.PluginSdk.Devices {

    public class DeviceFactory {

        private HsDevice _device;
        private List<HsFeature> _features;
        
        #region Create
        
        //Create
        public static DeviceFactory CreateDevice(string pluginId) {
            var df = new DeviceFactory();
            var device = new HsDevice
                         {
                             Relationship = ERelationship.Device,
                             Interface = pluginId
                         };
            device.Changes.Add(EProperty.Misc, (uint) EMiscFlag.ShowValues);
            device.Changes.Add(EProperty.UserAccess, "Any");
            device.Changes.Add(EProperty.Location2, "Plugin");
            device.Changes.Add(EProperty.Location, pluginId);
            df._device = device;

            return df;
        }
        
        #endregion
                
        //Add Features
        public DeviceFactory WithFeature(FeatureFactory feature) {

            if (feature?.Feature == null) {
                throw new ArgumentNullException(nameof(feature));
            }

            if (_features == null) {
                _features = new List<HsFeature>();
            }

            _features.Add(feature.Feature);

            return this;
        }
        
        #region Device Properties

        /// <summary>
        /// Add an <see cref="AbstractHsDevice.Address"/> to the device
        /// </summary>
        /// <param name="address">The string to set the address to</param>
        /// <returns>The <see cref="DeviceFactory"/> with the updated address value</returns>
        public DeviceFactory WithAddress(string address) {
            _device.Address = address;
            return this;
        }

        public DeviceFactory WithName(string name) {

            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentNullException(nameof(name));
            }

            _device.Name = name;

            return this;
        }
        
        public DeviceFactory WithExtraData(PlugExtraData extraData) {

            if (extraData == null) {
                throw new ArgumentNullException(nameof(extraData));
            }

            _device.PlugExtraData = extraData;

            return this;
        }
        
        public DeviceFactory WithMiscFlags(params EMiscFlag[] miscFlags) {

            if (miscFlags == null || miscFlags.Length == 0) {
                throw new ArgumentNullException(nameof(miscFlags));
            }

            foreach (var miscFlag in miscFlags.Distinct()) {
                _device.AddMiscFlag(miscFlag);
            }
            
            return this;
        }
        
        /// <summary>
        /// Remove a <see cref="EMiscFlag"/> from the device
        /// </summary>
        /// <param name="miscFlags"><see cref="EMiscFlag"/>(s) to remove</param>
        /// <returns>The DeviceFactory updated by removing the specified <see cref="EMiscFlag"/>(s)</returns>
        /// <exception cref="ArgumentNullException">Thrown when no <paramref name="miscFlags"/> are specified</exception>
        public DeviceFactory WithoutMiscFlags(params EMiscFlag[] miscFlags)
        {
            if (miscFlags == null || miscFlags.Length == 0)
            {
                throw new ArgumentNullException(nameof(miscFlags));
            }

            foreach (var miscFlag in miscFlags.Distinct())
            {
                _device.RemoveMiscFlag(miscFlag);
            }

            return this;
        }

        /// <summary>
        /// Set the Location property on the device.
        /// </summary>
        /// <param name="location">The location to set on the device</param>
        /// <returns>The DeviceFactory updated with the specified location</returns>
        /// <remarks>Null or whitespace strings will be converted to empty strings ""</remarks>
        public DeviceFactory WithLocation(string location) {
            // 09-15-2020 JLW - Default null or whitespace strings to empty string "" instead of throwing an exception PSDK-98
            _device.Location = string.IsNullOrWhiteSpace(location) ? "" : location;

            return this;
        }

        /// <summary>
        /// Set the Location2 property on the device.
        /// </summary>
        /// <param name="location2">The location2 to set on the device</param>
        /// <returns>The DeviceFactory updated with the specified location2</returns>
        /// <remarks>Null or whitespace strings will be converted to empty strings ""</remarks>
        public DeviceFactory WithLocation2(string location2) {
            // 09-15-2020 JLW - Default null or whitespace strings to empty string "" instead of throwing an exception PSDK-98
            _device.Location2 = string.IsNullOrWhiteSpace(location2) ? "" : location2;

            return this;
        }

        #endregion

        public DeviceFactory AsType(EDeviceType deviceType, int deviceSubType) {

            _device.TypeInfo = new TypeInfo()
                                 {
                                     ApiType = EApiType.Device,
                                     Type    = (int) deviceType,
                                     SubType = deviceSubType
                                 };

            return this;
        }

        public NewDeviceData PrepareForHs() {
            return new NewDeviceData(_device, _features);
        }
    }

}