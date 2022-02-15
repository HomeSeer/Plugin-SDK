using HomeSeer.Jui.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HomeSeer.Jui.Views {
    
    [TestFixture(
        TestOf = typeof(LabelView),
        Description = "Tests of the LabelView class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class LabelViewTests : AbstractJuiViewTestFixture {

        private static LabelView GetDefaultLabelView() {
            return new LabelView(DEFAULT_ID, DEFAULT_NAME);
        }
        
        private static IEnumerable<object[]> ValidIdNameValueCaseSource() {
            yield return new object[] {
                DEFAULT_ID,
                DEFAULT_NAME,
                string.Empty
            };
            yield return new object[] {
                DEFAULT_ID,
                DEFAULT_NAME,
                RANDOMIZER.GetString()
            };
        }
        
        private static IEnumerable<ELabelType> LabelTypesTestCaseSource() {
            yield return ELabelType.Default;
            yield return ELabelType.Preformatted;
        }
        
        private static IEnumerable<string> ValueTestCaseSource() {
            yield return RANDOMIZER.GetString();
        }

        [Test]
        [Description("Create a new instance of a LabelView using a valid ID and name and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdName_DoesNotThrow() {
            Assert.DoesNotThrow(() => {
                _ = new LabelView(DEFAULT_ID, DEFAULT_NAME);
            });
        }

        [TestCaseSource(nameof(ValidIdNameValueCaseSource))]
        [Description("Create a new instance of a LabelView using an ID, name, and value and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameValue_DoesNotThrow(string id, string name, string value) {
            Assert.DoesNotThrow(() => {
                _ = new LabelView(id, name, value);
            });
        }

        [Test]
        [Description("Create a new instance of a LabelView using a blank name and value and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameValue_NameValueNull_Throws() {
            Assert.Throws<ArgumentNullException>(() => {
                _ = new LabelView(DEFAULT_ID, string.Empty, string.Empty);
            });
        }

        [Test]
        [Description("Get the type of a LabelView and expect EViewType.Label to be returned.")]
        [Author("JLW")]
        public void Type_Get_Label() {
            LabelView labelView = GetDefaultLabelView();
            Assert.AreEqual(EViewType.Label, labelView.Type);
        }

        [Test]
        [Description("Get the label type and expect the default label type to be returned.")]
        [Author("JLW")]
        public void LabelType_Get_Default() {
            LabelView labelView = GetDefaultLabelView();
            Assert.AreEqual(ELabelType.Default, labelView.LabelType);
        }

        [TestCaseSource(nameof(LabelTypesTestCaseSource))]
        [Description("Set the label type and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void LabelType_Set_DoesNotThrow(ELabelType labelType) {
            LabelView labelView = GetDefaultLabelView();
            Assert.DoesNotThrow(() => {
                labelView.LabelType = labelType;
            });
        }

        [TestCaseSource(nameof(LabelTypesTestCaseSource))]
        [Description("Get the label type after setting it and expect the same label type to be returned.")]
        [Author("JLW")]
        public void LabelType_Get_ReturnsSame(ELabelType labelType) {
            LabelView labelView = GetDefaultLabelView();
            labelView.LabelType = labelType;
            Assert.AreEqual(labelType, labelView.LabelType);
        }

        [Test]
        [Description("Get the value and expect the default value to be returned")]
        [Author("JLW")]
        public void Value_Get_Default() {
            LabelView labelView = GetDefaultLabelView();
            Assert.AreEqual(null, labelView.Value);
        }

        [TestCaseSource(nameof(ValueTestCaseSource))]
        [Description("Set the value and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Value_Set_DoesNotThrow(string value) {
            LabelView labelView = GetDefaultLabelView();
            Assert.DoesNotThrow(() => {
                labelView.Value = value;
            });
        }
        
        [TestCaseSource(nameof(ValueTestCaseSource))]
        [Description("Get the value after setting it and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Value_Get_ReturnsSame(string value) {
            LabelView labelView = GetDefaultLabelView();
            labelView.Value = value;
            Assert.AreEqual(value, labelView.Value);
        }
        
        [Test]
        [Description("Call ToHtml and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void ToHtml_DoesNotThrow() {
            LabelView labelView = GetDefaultLabelView();
            Assert.DoesNotThrow(() => {
                _ = labelView.ToHtml();
            });
        }
        
        [Test]
        [Description("Call GetStringValue and expect null to be returned.")]
        [Author("JLW")]
        public void GetStringValue_ReturnsNull() {
            LabelView labelView = GetDefaultLabelView();
            Assert.AreEqual(null, labelView.GetStringValue());
        }
    }
}