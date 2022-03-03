using HomeSeer.Jui.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HomeSeer.Jui.Views {
    
    [TestFixture(
        TestOf = typeof(ToggleView),
        Description = "Tests of the ToggleView class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class ToggleViewTests : AbstractJuiViewTestFixture {
        
        private static ToggleView GetDefaultToggleView() {
            return new ToggleView(DEFAULT_ID, DEFAULT_NAME);
        }

        private static IEnumerable<EToggleType> ToggleTypeTestCaseSource() {
            yield return EToggleType.Switch;
            yield return EToggleType.Checkbox;
        }

        private static IEnumerable<string> InvalidValueTestCaseSource() {
            yield return RANDOMIZER.GetString();
            yield return RANDOMIZER.NextShort().ToString();
        }

        [TestCaseSource(nameof(BoolTestCaseSource))]
        [Description("Create a new instance of ToggleView and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameIsEnabled_DoesNotThrow(bool isEnabled) {
            Assert.DoesNotThrow(() => {
                _ = new ToggleView(DEFAULT_ID, DEFAULT_NAME, isEnabled);
            });
        }
        
        [Test]
        [Description("Get IsEnabled and expect the default IsEnabled state - False.")]
        [Author("JLW")]
        public void IsEnabled_Get_ReturnsDefault() {
            ToggleView view = GetDefaultToggleView();
            Assert.IsFalse(view.IsEnabled);
        }

        [TestCaseSource(nameof(BoolTestCaseSource))]
        [Description("Set IsEnabled and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void IsEnabled_Set_DoesNotThrow(bool isEnabled) {
            ToggleView view = GetDefaultToggleView();
            Assert.DoesNotThrow(() => {
                view.IsEnabled = isEnabled;
            });
        }

        [TestCaseSource(nameof(BoolTestCaseSource))]
        [Description("Set IsEnabled and expect IsEnabled to be set correctly.")]
        [Author("JLW")]
        public void IsEnabled_Set_SetsIsEnabled(bool isEnabled) {
            ToggleView view = GetDefaultToggleView();
            view.IsEnabled = isEnabled;
            Assert.AreEqual(isEnabled, view.IsEnabled);
        }
        
        [Test]
        [Description("Get ToggleType and expect the default ToggleType - Switch.")]
        [Author("JLW")]
        public void ToggleType_Get_ReturnsDefault() {
            ToggleView view = GetDefaultToggleView();
            Assert.AreEqual(EToggleType.Switch, view.ToggleType);
        }

        [TestCaseSource(nameof(ToggleTypeTestCaseSource))]
        [Description("Set ToggleType and expect ToggleType to be set correctly.")]
        [Author("JLW")]
        public void ToggleType_Set_SetsToggleType(EToggleType type) {
            ToggleView view = GetDefaultToggleView();
            view.ToggleType = type;
            Assert.AreEqual(type, view.ToggleType);
        }
        
        [Test]
        [Description("Update a ToggleView and expect no exception to be thrown.")]
        [Author("JLW")]
        public void Update_Valid_DoesNotThrow() {
            ToggleView view = GetDefaultToggleView();
            var updateView = new ToggleView(DEFAULT_ID, DEFAULT_NAME, true);
            Assert.DoesNotThrow(() => {
                view.Update(updateView);
            });
        }
        
        [TestCaseSource(nameof(BoolTestCaseSource))]
        [Description("Call UpdateValue and expect IsEnabled to be set correctly.")]
        [Author("JLW")]
        public void UpdateValue_Valid_SetsIsEnabled(bool isEnabled) {
            ToggleView view = GetDefaultToggleView();
            view.UpdateValue(isEnabled.ToString());
            Assert.AreEqual(isEnabled, view.IsEnabled);
        }
        
        [TestCaseSource(nameof(InvalidValueTestCaseSource))]
        [Description("Call UpdateValue with an invalid value and expect an exception to be thrown.")]
        [Author("JLW")]
        public void UpdateValue_Invalid_Throws(string isEnabled) {
            ToggleView view = GetDefaultToggleView();
            Assert.Throws<FormatException>(() => {
                view.UpdateValue(isEnabled);
            });
        }
        
        [Test]
        [Description("Call ToHtml and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void ToHtml_DoesNotThrow() {
            ToggleView view = GetDefaultToggleView();
            Assert.DoesNotThrow(() => {
                _ = view.ToHtml();
            });
        }
        
        [TestCaseSource(nameof(BoolTestCaseSource))]
        [Description("Call GetStringValue and expect IsEnabled to be returned.")]
        [Author("JLW")]
        public void GetStringValue_ReturnsIsEnabled(bool isEnabled) {
            ToggleView view = GetDefaultToggleView();
            view.IsEnabled = isEnabled;
            Assert.AreEqual(isEnabled.ToString(), view.GetStringValue());
        }
        
    }
}