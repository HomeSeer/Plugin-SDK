using System;
using HomeSeer.PluginSdk.Devices;
using HSPI_HomeSeerSamplePlugin.Constants;
using Newtonsoft.Json;

namespace HSPI_HomeSeerSamplePlugin {

    [JsonObject]
    public class DeviceAddPostData {

        [JsonProperty("action")]
        public string Action;
        
        [JsonProperty("device")]
        public DeviceAddData Device;
        
        [JsonObject]
        public class DeviceAddData {

            [JsonProperty("ref")]
            public int Ref;
            
            [JsonProperty("type")]
            public string Type {
                get => _type.ToString();
                set {
                    if (!int.TryParse(value, out var index)) {
                        return;
                    }

                    _type = index;
                    TypeDescription = Devices.SampleDeviceTypeList[index];
                    Features = Devices.SampleDeviceTypeFeatures[index];
                }
            }

            [JsonIgnore]
            private int _type;
            
            [JsonProperty("typeDesc")]
            public string TypeDescription;
            
            [JsonProperty("name")]
            public string Name;
            
            [JsonProperty("location")]
            public string Location1;
            
            [JsonProperty("location2")]
            public string Location2;
            
            [JsonProperty("features")]
            public string[] Features;

            public NewDeviceData BuildDevice(string pluginId) {
                var df = DeviceFactory.CreateDevice(pluginId);
                df = df.WithName(Name).WithLocation(Location1).WithLocation2(Location2);

                switch (_type) {
                    case 0:
                        df = BuildLpSwitch(pluginId, df);
                        break;
                    case 1:
                        df = BuildLpSensor(pluginId, df);
                        break;
                    default:
                        throw new Exception("Cannot create device with this configuration.");
                }

                return df.PrepareForHs();
            }

            private DeviceFactory BuildLpSwitch(string pluginId, DeviceFactory df) {
                var ff = FeatureFactory.CreateGenericBinaryControl(pluginId, $"Controls", "On", "Off", 1, 0)
                    .WithLocation(Location1).WithLocation2(Location2);
                df.WithFeature(ff);
                return df;
            }
            
            private DeviceFactory BuildLpSensor(string pluginId, DeviceFactory df) {
                var ff = FeatureFactory.CreateGenericBinarySensor(pluginId, $"Sensor State", "Sensor tripped", "No event", 1, 0)
                    .WithLocation(Location1).WithLocation2(Location2);
                df.WithFeature(ff);
                return df;
            }

        }

    }

}