using NUnit.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using HomeSeer.PluginSdk.Devices.Controls;

namespace HomeSeer.PluginSdk.Devices {
    
    [TestFixture(Description = "Tests of the FeatureFactory class to ensure it behaves as expected under normal conditions.",
        TestOf = typeof(FeatureFactory))]
    public class FeatureFactoryTests {

        private const string IMAGES_STATUS_DIR = "images/HomeSeer/status/";
        private const double EPSILON = 0.0001;
        private FeatureFactory _ff;

        [SetUp]
        public void InitFeatureFactory() {
            _ff = FeatureFactory.CreateFeature("TestPluginId");
        }

        private static IEnumerable<StatusGraphic> StatusGraphicTestCaseSource() {
            var sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-00.png", -100, -18);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-10.png", -18 + EPSILON, -12);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-20.png", -12 + EPSILON, -7);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-30.png", -7 + EPSILON, -1);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-40.png", -1 + EPSILON, 4);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-50.png", 4 + EPSILON, 10);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-60.png", 10 + EPSILON, 16);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-70.png", 16 + EPSILON, 21);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-80.png", 21 + EPSILON, 27);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-90.png", 27 + EPSILON, 32);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-100.png", 32 + EPSILON, 38);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-110.png", 38 + EPSILON, 100);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "heating.png", 0, "Heat");
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "cooling.png", 1, "Cool");
            yield return sg;

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "auto-mode.png", 2, "Auto");
            yield return sg;
            
            sg = new StatusGraphic(IMAGES_STATUS_DIR + "off.gif", 3, "Off");
            yield return sg;
        }

        [TestCaseSource(nameof(StatusGraphicTestCaseSource))]
        [Description("Add a Single Valid StatusGraphic")]
        public void AddGraphic_Single_Valid(StatusGraphic sg) {
            Assert.DoesNotThrow(() => {
                _ff.AddGraphic(sg);
            });
        }

        [Test]
        [Description("Add Multiple Valid StatusGraphics that target range of values")]
        public void AddGraphic_Multiple_Valid_Ranges() {

            var sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-00.png", -100, -18);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            _ff.AddGraphic(sg);

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-10.png", -18 + EPSILON, -12);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            _ff.AddGraphic(sg);

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-20.png", -12 + EPSILON, -7);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            _ff.AddGraphic(sg);

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-30.png", -7 + EPSILON, -1);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            _ff.AddGraphic(sg);

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-40.png", -1 + EPSILON, 4);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            _ff.AddGraphic(sg);

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-50.png", 4 + EPSILON, 10);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            _ff.AddGraphic(sg);

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-60.png", 10 + EPSILON, 16);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            _ff.AddGraphic(sg);

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-70.png", 16 + EPSILON, 21);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            _ff.AddGraphic(sg);

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-80.png", 21 + EPSILON, 27);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            _ff.AddGraphic(sg);

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-90.png", 27 + EPSILON, 32);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            _ff.AddGraphic(sg);

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-100.png", 32 + EPSILON, 38 );
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            _ff.AddGraphic(sg);

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-110.png", 38  + EPSILON, 100);
            sg.TargetRange.DecimalPlaces = 1;
            sg.TargetRange.Suffix = " °C";
            _ff.AddGraphic(sg);
        }

        [Test]
        [Description("Add Multiple Valid StatusGraphics that target single values")]
        public void AddGraphic_Multiple_Valid_SingleValues() {

            var sg = new StatusGraphic(IMAGES_STATUS_DIR + "heating.png", 0, "Heat");
            _ff.AddGraphic(sg);
            sg = new StatusGraphic(IMAGES_STATUS_DIR + "cooling.png", 1, "Cool");
            _ff.AddGraphic(sg);
            sg = new StatusGraphic(IMAGES_STATUS_DIR + "auto-mode.png", 2, "Auto");
            _ff.AddGraphic(sg);
            sg = new StatusGraphic(IMAGES_STATUS_DIR + "off.gif", 3, "Off");
            _ff.AddGraphic(sg);
        }

        [Test]
        [Description("Add overlapping StatusGraphics that target range of values and expect an exception to be thrown.")]
        public void AddGraphic_Multiple_Invalid_Ranges_Throws() {

            var sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-00.png", -100, 50);
            _ff.AddGraphic(sg);

            sg = new StatusGraphic(IMAGES_STATUS_DIR + "Thermometer-10.png", 0, 100);
            Assert.Throws<ArgumentException>(() => {
                _ff.AddGraphic(sg);
            }); 
        }

        [Test]
        [Description("Add multiple StatusGraphics that target the same single value and expect an exception to be thrown.")]
        public void AddGraphic_Multiple_Invalid_SingleValues_Throws() {

            var sg = new StatusGraphic(IMAGES_STATUS_DIR + "heating.png", 0, "Heat");
            _ff.AddGraphic(sg);
            sg = new StatusGraphic(IMAGES_STATUS_DIR + "cooling.png", 1, "Cool");
            _ff.AddGraphic(sg);
            sg = new StatusGraphic(IMAGES_STATUS_DIR + "auto-mode.png", 2, "Auto");
            _ff.AddGraphic(sg);
            sg = new StatusGraphic(IMAGES_STATUS_DIR + "off.gif", 2, "Off");
            Assert.Throws<ArgumentException>(() => {
                _ff.AddGraphic(sg);
            });
        }

        [Test]
        [Description("Add null StatusGraphic and expect an exception to be thrown.")]
        public void AddGraphic_Null_Throws() {

            Assert.Throws<ArgumentNullException>(() => {
                _ff.AddGraphic(null);
            });
        }
		
		[Test]
        [Description("Add Valid StatusControls that target range of values")]
        public void AddControl_Valid_Ranges() {

            var sc = new StatusControl(EControlType.ValueRangeSlider);
            sc.TargetRange = new ValueRange(-100, 0);
            _ff.AddControl(sc);

            sc = new StatusControl(EControlType.ValueRangeSlider);
            sc.TargetRange = new ValueRange(EPSILON, 100);
            _ff.AddControl(sc);

            sc = new StatusControl(EControlType.ValueRangeSlider);
            sc.TargetRange = new ValueRange(100+EPSILON, 200);
            _ff.AddControl(sc);
        }

        [Test]
        [Description("Add Valid StatusControls that target single values")]
        public void AddControl_Valid_SingleValues() {

            var sc = new StatusControl(EControlType.Button);
            sc.TargetValue = 0;
            sc.Label = "Heat";
            _ff.AddControl(sc);
            sc = new StatusControl(EControlType.Button);
            sc.TargetValue = 1;
            sc.Label = "Cool";
            _ff.AddControl(sc);
            sc = new StatusControl(EControlType.Button);
            sc.TargetValue = 2;
            sc.Label = "Auto";
            _ff.AddControl(sc);
            sc = new StatusControl(EControlType.Button);
            sc.TargetValue = 3;
            sc.Label = "Off";
            _ff.AddControl(sc);
        }

        [Test]
        [Description("Add overlapping StatusControls that target range of values and expect an exception to be thrown.")]
        public void AddControl_Invalid_Ranges_Throws() {

            var sc = new StatusControl(EControlType.ValueRangeSlider);
            sc.TargetRange = new ValueRange(-100, 0);
            _ff.AddControl(sc);

            sc = new StatusControl(EControlType.ValueRangeSlider);
            sc.TargetRange = new ValueRange(0, 100);

            Assert.Throws<ArgumentException>(() => {
                _ff.AddControl(sc);
            }); 
        }

        [Test]
        [Description("Add multiple StatusControls that target the same single value and expect an exception to be thrown.")]
        public void AddControl_Invalid_SingleValues_Throws() {

            var sc = new StatusControl(EControlType.Button);
            sc.TargetValue = 0;
            sc.Label = "Heat";
            _ff.AddControl(sc);
            sc = new StatusControl(EControlType.Button);
            sc.TargetValue = 1;
            sc.Label = "Cool";
            _ff.AddControl(sc);
            sc = new StatusControl(EControlType.Button);
            sc.TargetValue = 2;
            sc.Label = "Auto";
            _ff.AddControl(sc);
            sc = new StatusControl(EControlType.Button);
            sc.TargetValue = 2;
            sc.Label = "Off";
            Assert.Throws<ArgumentException>(() => {
                _ff.AddControl(sc);
            });
        }

        [Test]
        [Description("Add null StatusControl and expect an exception to be thrown.")]
        public void AddControl_Null_Throws() {

            Assert.Throws<ArgumentNullException>(() => {
                _ff.AddControl(null);
            });
        }
    }
}