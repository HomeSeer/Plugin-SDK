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

        private const double EPSILON = 0.0001;
        private FeatureFactory _ff;

        [SetUp]
        public void InitFeatureFactory() {
            _ff = FeatureFactory.CreateFeature("TestPluginId");
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