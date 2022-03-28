using System;
using HomeSeer.PluginSdk;
using HomeSeer.PluginSdk.Devices;
using HomeSeer.PluginSdk.Devices.Controls;
using HomeSeer.PluginSdk.Devices.Identification;
using HSPI_HomeSeerSamplePlugin.Constants;
using Newtonsoft.Json;

namespace HSPI_HomeSeerSamplePlugin {

    [JsonObject]
    public class DeviceAddPostData {

        private const string IMAGES_STATUS_DIR = "images/HomeSeer/status/";
        private const double EPSILON = 0.0001;

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

            public NewDeviceData BuildDevice(string pluginId, IHsController hs) {
                var df = DeviceFactory.CreateDevice(pluginId);
                df = df.WithName(Name).WithLocation(Location1).WithLocation2(Location2);

                switch (_type) {
                    case 0:
                        df = BuildLpSwitch(pluginId, df);
                        break;
                    case 1:
                        df = BuildLpSensor(pluginId, df);
                        break;
                    case 2:
                        df = BuildBpSensor(pluginId, df);
                        break;
                    case 3:
                        df = BuildThermostat(pluginId, df, hs);
                        break;
                    default:
                        throw new Exception("Cannot create device with this configuration.");
                }

                return df.PrepareForHs();
            }

            private DeviceFactory BuildLpSwitch(string pluginId, DeviceFactory df) {
                var ff = FeatureFactory.CreateGenericBinaryControl(pluginId, $"Controls", "On", "Off", 1, 0)
                    .WithLocation(Location1).WithLocation2(Location2).WithDisplayType(EFeatureDisplayType.Important);
                df.WithFeature(ff);
                return df;
            }

            private DeviceFactory BuildLpSensor(string pluginId, DeviceFactory df) {
                var ff = FeatureFactory.CreateGenericBinarySensor(pluginId, $"Sensor State", "Sensor tripped", "No event", 1, 0)
                    .WithLocation(Location1).WithLocation2(Location2).WithDisplayType(EFeatureDisplayType.Important);
                df.WithFeature(ff);

                return df;
            }

            private DeviceFactory BuildBpSensor(string pluginId, DeviceFactory df) {
                var ff = FeatureFactory.CreateGenericBinarySensor(pluginId, $"Sensor State", "Sensor tripped", "No event", 1, 0)
                    .WithLocation(Location1).WithLocation2(Location2).WithDisplayType(EFeatureDisplayType.Important);
                var ffbat = FeatureFactory.CreateFeature(pluginId).WithName("Battery").
                    AsType(EFeatureType.Generic, (int)EGenericFeatureSubType.Battery)
                    .WithLocation(Location1).WithLocation2(Location2).WithDefaultValue(95).WithDisplayType(EFeatureDisplayType.Normal);
                var range = new ValueRange(0, 100);
                range.Suffix = "%";
                var sg = new StatusGraphic("images/HomeSeer/status/battery_100.png", range);
                ffbat.AddGraphic(sg);
                df.WithFeature(ff).WithFeature(ffbat);

                return df;
            }

            private DeviceFactory BuildThermostat(string pluginId, DeviceFactory df, IHsController hs) {
                bool useCelsius = !Convert.ToBoolean(hs.GetINISetting("Settings", "gGlobalTempScaleF", "True", "settings.ini"));
                df.AsType(EDeviceType.Thermostat, 0)
                  .WithFeature(BuildAmbientTempFeature(pluginId, useCelsius))
                  .WithFeature(BuildSetpointFeature(pluginId, false, useCelsius))
                  .WithFeature(BuildSetpointFeature(pluginId, true, useCelsius))
                  .WithFeature(BuildHvacModeFeature(pluginId))
                  .WithFeature(BuildHvacStatusFeature(pluginId))
                  .WithFeature(BuildFanModeFeature(pluginId))
                  .WithFeature(BuildFanStatusFeature(pluginId))
                  .WithFeature(BuildHumidityFeature(pluginId));

                return df;
            }

            private FeatureFactory BuildSetpointFeature(string pluginId, bool isCoolSetpoint, bool useCelsius) {

                var ff = FeatureFactory.CreateFeature(pluginId)
                    .WithName($"{(isCoolSetpoint ? "Cooling" : "Heating")} Setpoint")
                    .WithLocation(Location1)
                    .WithLocation2(Location2)
                    .AsType(EFeatureType.ThermostatControl, isCoolSetpoint ? (int)EThermostatControlFeatureSubType.CoolingSetPoint : (int)EThermostatControlFeatureSubType.HeatingSetPoint)
                    .WithDefaultValue(useCelsius ? 22 : 71);

                var range = new ValueRange(0, 100) {
                    DecimalPlaces = 1,
                    Suffix = useCelsius ? " °C" : " °F"
                };
                ff.AddValueDropDown(range, new ControlLocation(1, 1), isCoolSetpoint ? EControlUse.CoolSetPoint : EControlUse.HeatSetPoint)
                  .AddButton(1000, "-", new ControlLocation(1, 2))
                  .AddButton(1001, "+", new ControlLocation(1, 3));

                AddTemperatureRangeGraphics(ff, useCelsius);
                return ff;
            }

            private void AddTemperatureRangeGraphics(FeatureFactory ff, bool useCelsius) {
                var sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-00.png", -200, useCelsius ? -18 : 0);
                sg.TargetRange.DecimalPlaces = 1;
                sg.TargetRange.Suffix = useCelsius ? " °C" : " °F";
                ff.AddGraphic(sg);

                sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-10.png", (useCelsius ? -18 : 0) + EPSILON, useCelsius ? -12 : 10);
                sg.TargetRange.DecimalPlaces = 1;
                sg.TargetRange.Suffix = useCelsius ? " °C" : " °F";
                ff.AddGraphic(sg);

                sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-20.png", (useCelsius ? -12 : 10) + EPSILON, useCelsius ? -7 : 20);
                sg.TargetRange.DecimalPlaces = 1;
                sg.TargetRange.Suffix = useCelsius ? " °C" : " °F";
                ff.AddGraphic(sg);

                sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-30.png", (useCelsius ? -7 : 20) + EPSILON, useCelsius ? -1 : 30);
                sg.TargetRange.DecimalPlaces = 1;
                sg.TargetRange.Suffix = useCelsius ? " °C" : " °F";
                ff.AddGraphic(sg);

                sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-40.png", (useCelsius ? -1 : 30) + EPSILON, useCelsius ? 4 : 40);
                sg.TargetRange.DecimalPlaces = 1;
                sg.TargetRange.Suffix = useCelsius ? " °C" : " °F";
                ff.AddGraphic(sg);

                sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-50.png", (useCelsius ? 4 : 40) + EPSILON, useCelsius ? 10 : 50);
                sg.TargetRange.DecimalPlaces = 1;
                sg.TargetRange.Suffix = useCelsius ? " °C" : " °F";
                ff.AddGraphic(sg);

                sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-60.png", (useCelsius ? 10 : 50) + EPSILON, useCelsius ? 16 : 60);
                sg.TargetRange.DecimalPlaces = 1;
                sg.TargetRange.Suffix = useCelsius ? " °C" : " °F";
                ff.AddGraphic(sg);

                sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-70.png", (useCelsius ? 16 : 60) + EPSILON, useCelsius ? 21 : 70);
                sg.TargetRange.DecimalPlaces = 1;
                sg.TargetRange.Suffix = useCelsius ? " °C" : " °F";
                ff.AddGraphic(sg);

                sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-80.png", (useCelsius ? 21 : 70) + EPSILON, useCelsius ? 27 : 80);
                sg.TargetRange.DecimalPlaces = 1;
                sg.TargetRange.Suffix = useCelsius ? " °C" : " °F";
                ff.AddGraphic(sg);

                sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-90.png", (useCelsius ? 27 : 80) + EPSILON, useCelsius ? 32 : 90);
                sg.TargetRange.DecimalPlaces = 1;
                sg.TargetRange.Suffix = useCelsius ? " °C" : " °F";
                ff.AddGraphic(sg);

                sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-100.png", (useCelsius ? 32 : 90) + EPSILON, useCelsius ? 38 : 100);
                sg.TargetRange.DecimalPlaces = 1;
                sg.TargetRange.Suffix = useCelsius ? " °C" : " °F";
                ff.AddGraphic(sg);

                sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-110.png", (useCelsius ? 38 : 100) + EPSILON, 200);
                sg.TargetRange.DecimalPlaces = 1;
                sg.TargetRange.Suffix = useCelsius ? " °C" : " °F";
                ff.AddGraphic(sg);
            }

            private FeatureFactory BuildAmbientTempFeature(string pluginId, bool useCelsius) {
                var ff = FeatureFactory.CreateFeature(pluginId)
                    .WithName("Ambient Temperature")
                    .WithLocation(Location1)
                    .WithLocation2(Location2)
                    .AsType(EFeatureType.ThermostatStatus, (int)EThermostatStatusFeatureSubType.Temperature)
                    .WithDefaultValue(useCelsius ? 22 : 71);

                AddTemperatureRangeGraphics(ff, useCelsius);

                return ff;
            }

            private FeatureFactory BuildHvacModeFeature(string pluginId) {

                var ff = FeatureFactory.CreateFeature(pluginId)
                    .WithName("HVAC Mode")
                    .WithLocation(Location1)
                    .WithLocation2(Location2)
                    .AsType(EFeatureType.ThermostatControl, (int)EThermostatControlFeatureSubType.ModeSet)
                    .AddButton(1, "Heat", new ControlLocation(1, 1), EControlUse.ThermModeHeat)
                    .AddButton(2, "Cool", new ControlLocation(1, 2), EControlUse.ThermModeCool)
                    .AddButton(3, "Auto", new ControlLocation(1, 3), EControlUse.ThermModeAuto)
                    .AddButton(4, "Aux Heat", new ControlLocation(2, 1), EControlUse.ThermModeAuto)
                    .AddButton(5, "Off", new ControlLocation(2, 2), EControlUse.ThermModeOff)
                    .AddGraphicForValue(IMAGES_STATUS_DIR + "heating.png", 1)
                    .AddGraphicForValue(IMAGES_STATUS_DIR + "cooling.png", 2)
                    .AddGraphicForValue(IMAGES_STATUS_DIR + "auto-mode.png", 3)
                    .AddGraphicForValue(IMAGES_STATUS_DIR + "AuxHeat.png", 4)
                    .AddGraphicForValue(IMAGES_STATUS_DIR + "off.gif", 5)
                    .WithDefaultValue(5);

                return ff;
            }

            private FeatureFactory BuildHvacStatusFeature(string pluginId) {

                var ff = FeatureFactory.CreateFeature(pluginId)
                    .WithName("HVAC Status")
                    .WithLocation(Location1)
                    .WithLocation2(Location2)
                    .AsType(EFeatureType.ThermostatStatus, (int)EThermostatStatusFeatureSubType.OperatingState)
                    .AddGraphicForValue(IMAGES_STATUS_DIR + "idle.png", 0, "Idle")
                    .AddGraphicForValue(IMAGES_STATUS_DIR + "heating.png", 2, "Heating")
                    .AddGraphicForValue(IMAGES_STATUS_DIR + "cooling.png", 3, "Cooling")
                    .WithDefaultValue(0);

                return ff;
            }

            private FeatureFactory BuildFanModeFeature(string pluginId) {

                var ff = FeatureFactory.CreateFeature(pluginId)
                    .WithName("Fan Mode")
                    .WithLocation(Location1)
                    .WithLocation2(Location2)
                    .AsType(EFeatureType.ThermostatControl, (int)EThermostatControlFeatureSubType.FanModeSet)
                    .AddButton(1, "Auto", new ControlLocation(1, 1), EControlUse.ThermFanAuto)
                    .AddButton(2, "On", new ControlLocation(1, 2), EControlUse.ThermFanOn)
                    .AddGraphicForValue(IMAGES_STATUS_DIR + "fan-auto.png", 1)
                    .AddGraphicForValue(IMAGES_STATUS_DIR + "fan-on.png", 2)
                    .WithDefaultValue(1);

                return ff;
            }

            private FeatureFactory BuildFanStatusFeature(string pluginId) {

                var ff = FeatureFactory.CreateFeature(pluginId)
                    .WithName("Fan Status")
                    .WithLocation(Location1)
                    .WithLocation2(Location2)
                    .AsType(EFeatureType.ThermostatStatus, (int)EThermostatStatusFeatureSubType.FanStatus)
                    .AddGraphicForValue(IMAGES_STATUS_DIR + "fan-state-off.png", 0, "Off")
                    .AddGraphicForValue(IMAGES_STATUS_DIR + "fan-state-on.png", 1, "On")
                    .WithDefaultValue(0);
                    
                return ff;
            }

            private FeatureFactory BuildHumidityFeature(string pluginId) {

                var range = new ValueRange(0, 100) {
                    DecimalPlaces = 0,
                    Prefix = "",
                    Suffix = " %"
                };
                var sg = new StatusGraphic(IMAGES_STATUS_DIR + "water.gif", range);
                var ff = FeatureFactory.CreateFeature(pluginId)
                    .WithName("Humidity")
                    .WithLocation(Location1)
                    .WithLocation2(Location2)
                    .AsType(EFeatureType.ThermostatStatus, (int)EThermostatStatusFeatureSubType.Humidity)
                    .AddGraphic(sg)
                    .WithDefaultValue(45);

                return ff;
            }

        }

    }

}