using System;
using System.Collections.Generic;
using HomeSeer.PluginSdk.Devices;

namespace HomeSeer.PluginSdk {

    public class HsInterfaceConnection {

        private IHsController _hsController;
        private string _pluginId;
        private string _pluginName;
        
        private SortedDictionary<int, HsDevice> _devices = new SortedDictionary<int, HsDevice>();
        private SortedDictionary<int, HsDevice> _features = new SortedDictionary<int, HsDevice>();
        private HashSet<string> _interfaceKeys = new HashSet<string>();

        public HsInterfaceConnection(IHsController hsController, IPlugin plugin) {
            _hsController = hsController;
            _pluginId = plugin.Id;
            _pluginName = plugin.Name;
            _interfaceKeys.Add(_pluginId);
            _interfaceKeys.Add(_pluginName);
        }
        
        #region Interface Keys

        public bool AddInterfaceKey(string key) {
            
            if (string.IsNullOrWhiteSpace(key)) {
                throw new ArgumentNullException(nameof(key));
            }
            
            return _interfaceKeys.Add(key);
        }

        public void AddInterfaceKeys(params string[] keys) {

            if (keys == null || keys.Length == 0) {
                throw new ArgumentNullException(nameof(keys));
            }

            var ks = new HashSet<string>(_interfaceKeys);
                
            foreach (var key in keys) {
                if (string.IsNullOrWhiteSpace(key)) {
                    continue;
                }
                    
                ks.Add(key);
            }

            _interfaceKeys = ks;
        }

        public bool RemoveInterfaceKey(string key) {
            
            if (string.IsNullOrWhiteSpace(key)) {
                throw new ArgumentNullException(nameof(key));
            }
            
            return _interfaceKeys.Remove(key);
        }

        public void RemoveInterfaceKeys(params string[] keys) {
            
            if (keys == null || keys.Length == 0) {
                throw new ArgumentNullException(nameof(keys));
            }

            var ks = new HashSet<string>(_interfaceKeys);
                
            foreach (var key in keys) {
                if (string.IsNullOrWhiteSpace(key)) {
                    continue;
                }
                    
                ks.Remove(key);
            }

            _interfaceKeys = ks;
        }
        
        #endregion

        public void Initialize() {
            
            //Get all devices that are owned by this plugin (WHERE Interface = _pluginId || Interface = _pluginName)
            foreach (var interfaceKey in _interfaceKeys) {
                //Get all the device marked with the interface key
                var devicesForInterface = new List<HsDevice>();//_hsController.GetDevicesByInterface(interfaceKey);
                if (devicesForInterface == null) {
                    continue;
                }
                foreach (var device in devicesForInterface) {
                    //Sort all of the devices returned
                    switch (device.Relationship) {
                        case ERelationship.Feature:
                            try {
                                _features.Add(device.Ref, device);
                                break;
                            }
                            catch (Exception) {
                                break;
                            }
                        default:
                            try {
                                _devices.Add(device.Ref, device);
                                break;
                            }
                            catch (Exception) {
                                break;
                            }
                    }
                    
                    //TODO listen for device updates
                }
            }
        }
                
        //Get Device

        public HsDevice GetDeviceByRef(int devRef) {

            if (devRef <= 0) {
                throw new ArgumentOutOfRangeException(nameof(devRef));
            }

            if (_features.ContainsKey(devRef)) {
                throw new ArgumentException("This device is a feature.  Please use GetDeviceFeature() instead.");
            }

            var device = _hsController.GetDeviceByRef(devRef);
            
            if (device == null) {
                throw new KeyNotFoundException("No device with the reference exists in HomeSeer");
            }
            
            //Not excluding features from this method for now -JLW
            /*if (device.Relationship == ERelationship.Feature) {
                _features.Add(device.Ref, device);
                throw new DeviceRelationshipException("This device is a feature.  Please use GetDeviceFeature() instead.");
            }*/

            if (_devices.ContainsKey(devRef)) {

                
            }

            if (device.AssociatedDevices.Count == 0) {
                return device;
            }

            foreach (var assDeviceRef in device.AssociatedDevices) {
                //TODO get latest
                if (_features.ContainsKey(assDeviceRef)) {
                    device.Features.Add(_features[assDeviceRef]);
                    continue;
                }

                if (_devices.ContainsKey(assDeviceRef)) {
                    device.Features.Add(_devices[assDeviceRef]);
                    continue;
                }
                    
                var assDevice = _hsController.GetDeviceByRef(assDeviceRef);
                if (assDevice == null) {
                    Console.WriteLine($"No device found with ref : {assDeviceRef}");
                    continue;
                }
                switch (assDevice.Relationship) {
                    case ERelationship.Feature:
                        try {
                            _features.Add(assDevice.Ref, assDevice);
                            break;
                        }
                        catch (Exception) {
                            break;
                        }
                    default:
                        try {
                            _devices.Add(assDevice.Ref, assDevice);
                            break;
                        }
                        catch (Exception) {
                            break;
                        }
                }
                
                device.Features.Add(assDevice);
            }

            return device;
        }
        
        //Get Device Feature by type

        /*public HsDevice GetDeviceFeatureByType() {
            
        }*/
        
        //Get Device feature by address?
        
        //Control Device Feature By Ref and Type - Double || String
        
        //Update Device by Ref
        
        //Delete Device by Ref
        
        

    }

}