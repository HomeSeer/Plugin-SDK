using HomeSeer.Jui.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HomeSeer.Jui.Views {
    
    [TestFixture(
        TestOf = typeof(InputView),
        Description = "Tests of the InputView class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class InputViewTests : AbstractJuiViewTestFixture {

        private static InputView GetDefaultInputView() {
            return new InputView(DEFAULT_ID, DEFAULT_NAME);
        }
        
        private static IEnumerable<EInputType> InputTypesTestCaseSource() {
            yield return EInputType.Text;
            yield return EInputType.Number;
            yield return EInputType.Email;
            yield return EInputType.Url;
            yield return EInputType.Password;
            yield return EInputType.Decimal;
            yield return EInputType.Date;
            yield return EInputType.Time;
            //TODO DateTime
        }
        
        private static IEnumerable<object[]> ValidValueForTypeTestCaseSource() {
            yield return new object[] {
                EInputType.Text, 
                RANDOMIZER.GetString()
            };
            yield return new object[] {
                EInputType.Number, 
                RANDOMIZER.NextShort().ToString()
            };
            yield return new object[] {
                EInputType.Email, 
                "support@homeseer.com"
            };
            yield return new object[] {
                EInputType.Url, 
                "https://homeseer.com/"
            };
            yield return new object[] {
                EInputType.Password, 
                RANDOMIZER.GetString()
            };
            yield return new object[] {
                EInputType.Decimal, 
                RANDOMIZER.NextFloat().ToString(CultureInfo.CurrentCulture)
            };
            yield return new object[] {
                EInputType.Date,
                "2/22/2022"
            };
            yield return new object[] {
                EInputType.Time,
                "12:30PM"
            };
            yield return new object[] {
                EInputType.Time,
                "22:15"
            };
            //TODO DateTime
        }
        
        private static IEnumerable<string> ValueTestCaseSource() {
            yield return RANDOMIZER.GetString();
        }
        
        private static IEnumerable<object[]> InvalidValueForTypeTestCaseSource() {
            yield return new object[] {
                EInputType.Email, 
                RANDOMIZER.GetString()
            };
            yield return new object[] {
                EInputType.Url, 
                RANDOMIZER.GetString()
            };
            yield return new object[] {
                EInputType.Date, 
                RANDOMIZER.GetString()
            };
            yield return new object[] {
                EInputType.Time, 
                RANDOMIZER.GetString()
            };
            //TODO DateTime
        }

        private static IEnumerable<string> InvalidNameTestCaseSource() {
            yield return null;
            yield return string.Empty;
            yield return " ";
            yield return "\r";
            yield return "\n";
        }

        [TestCaseSource(nameof(InputTypesTestCaseSource))]
        [Description("Create a new instance of InputView using an Id, Name, and Type and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameType_DoesNotThrow(EInputType type) {
            Assert.DoesNotThrow(() => {
                _ = new InputView(DEFAULT_ID, DEFAULT_NAME, type);
            });
        }

        [TestCaseSource(nameof(InvalidNameTestCaseSource))]
        [Description("Create a new instance of InputView using an Id and invalid Name and expect an exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameType_InvalidName_Throws(string name) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = new InputView(DEFAULT_ID, name);
            });
        }
        
        [TestCaseSource(nameof(ValidValueForTypeTestCaseSource))]
        [Description("Create a new instance of InputView using an Id, Name, Value, and Type and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameValueType_DoesNotThrow(EInputType type, string value) {
            Assert.DoesNotThrow(() => {
                _ = new InputView(DEFAULT_ID, DEFAULT_NAME, value, type);
            });
        }
        
        [TestCaseSource(nameof(InvalidNameTestCaseSource))]
        [Description("Create a new instance of InputView using an Id and invalid Name and expect an exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameValueType_InvalidName_Throws(string name) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = new InputView(DEFAULT_ID, name, "");
            });
        }
        
        [TestCaseSource(nameof(InvalidValueForTypeTestCaseSource))]
        [Description("Create a new instance of InputView using an Id, Name, and invalid value for type and expect an exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameValueType_InvalidValueForType_Throws(EInputType type, string value) {
            Assert.Throws<InvalidValueForTypeException>(() => {
                _ = new InputView(DEFAULT_ID, DEFAULT_NAME, value, type);
            });
        }
        
        [TestCaseSource(nameof(ValidValueForTypeTestCaseSource))]
        [Description("Create a new instance of InputView using an Id, Name, Value, and Type and expect the Value to be set.")]
        [Author("JLW")]
        public void Constructor_IdNameValueType_SetsValue(EInputType type, string value) {
            var view = new InputView(DEFAULT_ID, DEFAULT_NAME, value, type);
            Assert.AreEqual(value, view.Value);
        }
        
        [Test]
        [Description("Get the input type and expect the default input type to be returned.")]
        [Author("JLW")]
        public void InputType_Get_Default() {
            InputView view = GetDefaultInputView();
            Assert.AreEqual(EInputType.Text, view.InputType);
        }
        
        [TestCaseSource(nameof(InputTypesTestCaseSource))]
        [Description("Set the input type and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void InputType_Set_DoesNotThrow(EInputType inputType) {
            InputView view = GetDefaultInputView();
            Assert.DoesNotThrow(() => {
                view.InputType = inputType;
            });
        }
        
        //TODO InputType set clears value

        [TestCaseSource(nameof(InputTypesTestCaseSource))]
        [Description("Get the input type after settings it and expect the same input type to be returned.")]
        [Author("JLW")]
        public void InputType_Get_ReturnsSame(EInputType inputType) {
            InputView view = GetDefaultInputView();
            view.InputType = inputType;
            Assert.AreEqual(inputType, view.InputType);
            
        }

        [Test]
        [Description("Get the value from a new InputView and expect the default value to be returned")]
        [Author("JLW")]
        public void Value_Get_Default() {
            InputView view = GetDefaultInputView();
            Assert.AreEqual(string.Empty, view.Value);
        }

        [TestCaseSource(nameof(ValueTestCaseSource))]
        [Description("Set the value for a default InputView and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Value_Set_DoesNotThrow(string value) {
            InputView view = GetDefaultInputView();
            Assert.DoesNotThrow(() => {
                view.Value = value;
            });
        }
        
        [TestCaseSource(nameof(ValueTestCaseSource))]
        [Description("Get the value after setting it and expect the correct value to be returned.")]
        [Author("JLW")]
        public void Value_Set_SetsValue(string value) {
            InputView view = GetDefaultInputView();
            view.Value = value;
            Assert.AreEqual(value, view.Value);
        }
        
        [TestCaseSource(nameof(ValidValueForTypeTestCaseSource))]
        [Description("Set the value using a valid value for the input type and expect no exception to be thrown.")]
        [Author("JLW")]
        public void Value_SetValidForType_DoesNotThrow(EInputType inputType, string value) {
            InputView view = GetDefaultInputView();
            view.InputType = inputType;
            Assert.DoesNotThrow(() => {
                view.Value = value;
            });
        }
        
        [TestCaseSource(nameof(InvalidValueForTypeTestCaseSource))]
        [Description("Set the value using an invalid value for the input type and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Value_SetInvalidForType_Throws(EInputType inputType, string value) {
            InputView view = GetDefaultInputView();
            view.InputType = inputType;
            Assert.Throws<InvalidValueForTypeException>(() => {
                view.Value = value;
            });
        }
        
        [TestCaseSource(nameof(ValueTestCaseSource))]
        [Description("Call GetStringValue and expect the Value to be returned.")]
        [Author("JLW")]
        public void GetStringValue_ReturnsValue(string value) {
            InputView view = GetDefaultInputView();
            view.Value = value;
            Assert.AreEqual(value, view.GetStringValue());
        }

        [TestCaseSource(nameof(ValidValueForTypeTestCaseSource))]
        [Description("Call IsValueValidForType using a valid value for an input type and expect true to be returned.")]
        [Author("JLW")]
        public void IsValueValidForType_Valid_ReturnsTrue(EInputType inputType, string value) {
            InputView view = GetDefaultInputView();
            view.InputType = inputType;
            Assert.IsTrue(view.IsValueValidForType(value));
        }
        
        [TestCaseSource(nameof(InvalidValueForTypeTestCaseSource))]
        [Description("Call IsValueValidForType using an invalid value for an input type and expect false to be returned.")]
        [Author("JLW")]
        public void IsValueValidForType_Invalid_ReturnsFalse(EInputType inputType, string value) {
            InputView view = GetDefaultInputView();
            view.InputType = inputType;
            Assert.IsFalse(view.IsValueValidForType(value));
        }

        [TestCaseSource(nameof(ValidValueForTypeTestCaseSource))]
        [Description("Update an InputView using a valid value for the input type and expect no exception to be thrown.")]
        [Author("JLW")]
        public void Update_Valid_DoesNotThrow(EInputType inputType, string value) {
            InputView view = GetDefaultInputView();
            view.InputType = inputType;
            var updateView = new InputView(DEFAULT_ID, DEFAULT_NAME, value, inputType);
            Assert.DoesNotThrow(() => {
                view.Update(updateView);
            });
        }
        
        [TestCaseSource(nameof(ValidValueForTypeTestCaseSource))]
        [Description("Call UpdateValue using a valid value for the input type and expect no exception to be thrown.")]
        [Author("JLW")]
        public void UpdateValue_Valid_DoesNotThrow(EInputType inputType, string value) {
            InputView view = GetDefaultInputView();
            view.InputType = inputType;
            Assert.DoesNotThrow(() => {
                view.UpdateValue(value);
            });
        }
        
        [TestCaseSource(nameof(ValidValueForTypeTestCaseSource))]
        [Description("Call UpdateValue using a valid value for the input type and expect the Value to be set correctly.")]
        [Author("JLW")]
        public void UpdateValue_Valid_SetsValue(EInputType inputType, string value) {
            InputView view = GetDefaultInputView();
            view.InputType = inputType;
            view.UpdateValue(value);
            Assert.AreEqual(value, view.Value);
        }

        [TestCaseSource(nameof(InvalidValueForTypeTestCaseSource))]
        [Description("Call UpdateValue using an invalid value for the input type and expect an exception to be thrown.")]
        [Author("JLW")]
        public void UpdateValue_Invalid_Throws(EInputType inputType, string value) {
            InputView view = GetDefaultInputView();
            view.InputType = inputType;
            Assert.Throws<InvalidValueForTypeException>(() => {
                view.UpdateValue(value);
            });
        }

        [Test]
        [Description("Call ToHtml and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void ToHtml_DoesNotThrow() {
            InputView view = GetDefaultInputView();
            Assert.DoesNotThrow(() => {
                _ = view.ToHtml();
            });
        }

    }
}