using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HomeSeer.Jui.Views {

    [TestFixture(
        TestOf = typeof(TextAreaView),
        Description = "Tests of the TextAreaView class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class TextAreaViewTests : AbstractJuiViewTestFixture {

        private static TextAreaView GetDefaultView() {
            return new TextAreaView(DEFAULT_ID, DEFAULT_NAME);
        }
        
        private static IEnumerable<string> ValueTestCaseSource() {
            yield return RANDOMIZER.GetString();
        }

        private static IEnumerable<int> ValidRowsTestCaseSource() {
            yield return 1;
            yield return RANDOMIZER.NextShort(1, 512);
            yield return 512;
        }
        
        private static IEnumerable<int> InvalidRowsTestCaseSource() {
            yield return -1;
            yield return 0;
        }

        [Test]
        [Description("Create a new instance of TextAreaView using the ID, Name, and Rows constructor and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameRows_DoesNotThrow() {
            Assert.DoesNotThrow(() => {
                _ = new TextAreaView(DEFAULT_ID, DEFAULT_NAME);
            });
        }
        
        [TestCaseSource(nameof(InvalidRowsTestCaseSource))]
        [Description("Create a new instance of TextAreaView using the ID, Name, and Rows constructor with invalid rows and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameRows_InvalidRows_Throws(int rows) {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                _ = new TextAreaView(DEFAULT_ID, DEFAULT_NAME, rows);
            });
        }
        
        [Test]
        [Description("Create a new instance of TextAreaView using the ID, Name, Value, and Rows constructor and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameValueRows_DoesNotThrow() {
            Assert.DoesNotThrow(() => {
                _ = new TextAreaView(DEFAULT_ID, DEFAULT_NAME, RANDOMIZER.GetString());
            });
        }
        
        [TestCaseSource(nameof(InvalidRowsTestCaseSource))]
        [Description("Create a new instance of TextAreaView using the ID, Name, Value, and Rows constructor with invalid rows and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameValueRows_InvalidRows_Throws(int rows) {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                _ = new TextAreaView(DEFAULT_ID, DEFAULT_NAME, RANDOMIZER.GetString(), rows);
            });
        }
        
        [Test]
        [Description("Get the Rows from a new TextAreaView and expect the default Rows to be returned")]
        [Author("JLW")]
        public void Rows_Get_Default() {
            TextAreaView view = GetDefaultView();
            Assert.AreEqual(5, view.Rows);
        }
        
        [TestCaseSource(nameof(ValidRowsTestCaseSource))]
        [Description("Set the Rows using a valid value and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Rows_Set_Valid_DoesNotThrow(int rows) {
            TextAreaView view = GetDefaultView();
            Assert.DoesNotThrow(() => {
                view.Rows = rows;
            });
        }
        
        [TestCaseSource(nameof(InvalidRowsTestCaseSource))]
        [Description("Set the Rows using an invalid value and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Rows_Set_Invalid_Throws(int rows) {
            TextAreaView view = GetDefaultView();
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                view.Rows = rows;
            });
        }
        
        [TestCaseSource(nameof(ValidRowsTestCaseSource))]
        [Description("Set the Rows using a valid value and expect Rows to be set correctly.")]
        [Author("JLW")]
        public void Rows_Set_Valid_SetsRows(int rows) {
            TextAreaView view = GetDefaultView();
            view.Rows = rows;
            Assert.AreEqual(rows, view.Rows);
        }
        
        [Test]
        [Description("Get the value from a new TextAreaView and expect the default value to be returned")]
        [Author("JLW")]
        public void Value_Get_Default() {
            TextAreaView view = GetDefaultView();
            Assert.AreEqual(string.Empty, view.Value);
        }

        [TestCaseSource(nameof(ValueTestCaseSource))]
        [Description("Set the value for a default TextAreaView and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Value_Set_DoesNotThrow(string value) {
            TextAreaView view = GetDefaultView();
            Assert.DoesNotThrow(() => {
                view.Value = value;
            });
        }
        
        [TestCaseSource(nameof(ValueTestCaseSource))]
        [Description("Get the value after setting it and expect the correct value to be returned.")]
        [Author("JLW")]
        public void Value_Set_SetsValue(string value) {
            TextAreaView view = GetDefaultView();
            view.Value = value;
            Assert.AreEqual(value, view.Value);
        }
        
        [TestCaseSource(nameof(ValueTestCaseSource))]
        [Description("Call Update with a valid view and expect the Value to be set correctly.")]
        [Author("JLW")]
        public void Update_Valid_SetsValue(string value) {
            TextAreaView view = GetDefaultView();
            var updateView = new TextAreaView(DEFAULT_ID, DEFAULT_NAME) {
                Value = value
            };
            view.Update(updateView);
            Assert.AreEqual(value, view.Value);
        }
        
        [TestCaseSource(nameof(ValueTestCaseSource))]
        [Description("Call UpdateValue and expect the Value to be set correctly.")]
        [Author("JLW")]
        public void UpdateValue_SetsValue(string value) {
            TextAreaView view = GetDefaultView();
            view.UpdateValue(value);
            Assert.AreEqual(value, view.Value);
        }
        
        [TestCaseSource(nameof(ValueTestCaseSource))]
        [Description("Call GetStringValue and expect the Value to be returned.")]
        [Author("JLW")]
        public void GetStringValue_ReturnsValue(string value) {
            TextAreaView view = GetDefaultView();
            view.Value = value;
            Assert.AreEqual(value, view.GetStringValue());
        }
        
        [Test]
        [Description("Call ToHtml and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void ToHtml_DoesNotThrow() {
            TextAreaView view = GetDefaultView();
            Assert.DoesNotThrow(() => {
                _ = view.ToHtml();
            });
        }

    }

}