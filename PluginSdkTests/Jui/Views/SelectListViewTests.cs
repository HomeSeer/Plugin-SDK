using HomeSeer.Jui.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HomeSeer.Jui.Views {
    
    [TestFixture(
        TestOf = typeof(SelectListView),
        Description = "Tests of the SelectListView class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class SelectListViewTests : AbstractJuiViewTestFixture {

        private static readonly List<string> _defaultOptionKeys = new List<string> {
            "1", 
            "2", 
            "3"
        };
        
        private static readonly List<string> _defaultOptions = new List<string> {
            "option1", 
            "option2", 
            "option3"
        };
        
        private static SelectListView GetDefaultView() {
            return new SelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, _defaultOptionKeys);
        }

        private static IEnumerable<ESelectListType> StyleTestCaseSource() {
            yield return ESelectListType.DropDown;
            yield return ESelectListType.RadioList;
            yield return ESelectListType.SearchableDropDown;
        }

        private static IEnumerable<int> ValidSelectionTestCaseSource() {
            yield return 0;
            yield return 1;
            yield return 2;
        }
        
        private static IEnumerable<int> InvalidSelectionTestCaseSource() {
            yield return -2;
            yield return 3;
        }

        private static IEnumerable<object[]> ValidSelectionOptionTestCaseSource() {
            yield return new object[] {
                0,
                _defaultOptions[0]
            };
            yield return new object[] {
                1,
                _defaultOptions[1]
            };
            yield return new object[] {
                2,
                _defaultOptions[2]
            };
        }
        
        private static IEnumerable<object[]> ValidSelectionOptionKeyTestCaseSource() {
            yield return new object[] {
                0,
                _defaultOptionKeys[0]
            };
            yield return new object[] {
                1,
                _defaultOptionKeys[1]
            };
            yield return new object[] {
                2,
                _defaultOptionKeys[2]
            };
        }

        private static IEnumerable<string> InvalidValueTestCaseSource() {
            yield return RANDOMIZER.GetString();
            yield return RANDOMIZER.NextBool().ToString();
        }

        private static IEnumerable<List<string>> InvalidOptionsTestCaseSource() {
            yield return null;
            yield return new List<string>();
        }
        
        private static IEnumerable<List<string>> InvalidOptionKeysTestCaseSource() {
            yield return null;
            yield return new List<string>();
        }

        private static IEnumerable<object[]> OptionsAndOptionKeysDoNotMatchTestCaseSource() {
            yield return new object[] {
                _defaultOptions, 
                new List<string> { "1" }
            };
            yield return new object[] {
                new List<string> { "1" },
                _defaultOptionKeys
            };
        }

        [Test]
        [Description("Create a new instance of SelectListView using the IdNameOptionsStyleSelection constructor and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameOptionsStyleSelection_DoesNotThrow() {
            Assert.DoesNotThrow(() => {
                _ = new SelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions);
            });
        }

        [TestCaseSource(nameof(InvalidOptionsTestCaseSource))]
        [Description("Create a new instance of SelectListView using the IdNameOptionsStyleSelection constructor with invalid options and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameOptionsStyleSelection_InvalidOptions_Throws(List<string> options) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = new SelectListView(DEFAULT_ID, DEFAULT_NAME, options);
            });
        }
        
        [TestCaseSource(nameof(InvalidSelectionTestCaseSource))]
        [Description("Create a new instance of SelectListView using the IdNameOptionsStyleSelection constructor with invalid selection and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameOptionsStyleSelection_InvalidSelection_Throws(int selection) {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                _ = new SelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, ESelectListType.DropDown, selection);
            });
        }
        
        [Test]
        [Description("Create a new instance of SelectListView using the IdNameOptionsOptionKeysStyleSelection constructor and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameOptionsOptionKeysStyleSelection_DoesNotThrow() {
            Assert.DoesNotThrow(() => {
                _ = new SelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, _defaultOptionKeys);
            });
        }
        
        [TestCaseSource(nameof(InvalidOptionsTestCaseSource))]
        [Description("Create a new instance of SelectListView using the IdNameOptionsOptionKeysStyleSelection constructor with invalid options and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameOptionsOptionKeysStyleSelection_InvalidOptions_Throws(List<string> options) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = new SelectListView(DEFAULT_ID, DEFAULT_NAME, options, _defaultOptionKeys);
            });
        }
        
        [TestCaseSource(nameof(InvalidSelectionTestCaseSource))]
        [Description("Create a new instance of SelectListView using the IdNameOptionsOptionKeysStyleSelection constructor with invalid selection and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameOptionsOptionKeysStyleSelection_InvalidSelection_Throws(int selection) {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                _ = new SelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, _defaultOptionKeys, ESelectListType.DropDown, selection);
            });
        }
        
        [TestCaseSource(nameof(InvalidOptionKeysTestCaseSource))]
        [Description("Create a new instance of SelectListView using the IdNameOptionsOptionKeysStyleSelection constructor with invalid option keys and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameOptionsOptionKeysStyleSelection_InvalidOptionKeys_Throws(List<string> optionKeys) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = new SelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, optionKeys);
            });
        }
        
        [TestCaseSource(nameof(OptionsAndOptionKeysDoNotMatchTestCaseSource))]
        [Description("Create a new instance of SelectListView using the IdNameOptionsOptionKeysStyleSelection constructor with options and option keys that do not match and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameOptionsOptionKeysStyleSelection_OptionsAndKeysDoNotMatch_Throws(List<string> options, List<string> optionKeys) {
            Assert.Throws<ArgumentException>(() => {
                _ = new SelectListView(DEFAULT_ID, DEFAULT_NAME, options, optionKeys);
            });
        }
        
        [Test]
        [Description("Get the Options and expect the options set during initialization to be returned.")]
        [Author("JLW")]
        public void Options_Get_ReturnsExpected() {
            var view = new SelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions);
            Assert.AreEqual(_defaultOptions, view.Options);
        }
        
        [Test]
        [Description("Get the OptionKeys and expect the option keys set during initialization to be returned.")]
        [Author("JLW")]
        public void OptionKeys_Get_ReturnsExpected() {
            var view = new SelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, _defaultOptionKeys);
            Assert.AreEqual(_defaultOptionKeys, view.OptionKeys);
        }
        
        [Test]
        [Description("Get the Style and expect the default style to be returned.")]
        [Author("JLW")]
        public void Style_Get_ReturnsDefault() {
            SelectListView view = GetDefaultView();
            Assert.AreEqual(ESelectListType.DropDown, view.Style);
        }
        
        [TestCaseSource(nameof(StyleTestCaseSource))]
        [Description("Set the Style and expect it to be set correctly.")]
        [Author("JLW")]
        public void Style_Set_SetsStyle(ESelectListType style) {
            SelectListView view = GetDefaultView();
            view.Style = style;
            Assert.AreEqual(style, view.Style);
        }
        
        [Test]
        [Description("Get the Selection and expect the default Selection to be returned.")]
        [Author("JLW")]
        public void Selection_Get_ReturnsDefault() {
            SelectListView view = GetDefaultView();
            Assert.AreEqual(-1, view.Selection);
        }
        
        [TestCaseSource(nameof(InvalidSelectionTestCaseSource))]
        [Description("Set the Selection using an invalid value and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Selection_Set_Invalid_Throws(int selection) {
            SelectListView view = GetDefaultView();
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                view.Selection = selection;
            });
        }
        
        [TestCaseSource(nameof(ValidSelectionTestCaseSource))]
        [Description("Set the Selection using a valid value and expect it to be set correctly.")]
        [Author("JLW")]
        public void Selection_Set_Valid_SetsSelection(int selection) {
            SelectListView view = GetDefaultView();
            view.Selection = selection;
            Assert.AreEqual(selection, view.Selection);
        }
        
        [TestCaseSource(nameof(ValidSelectionTestCaseSource))]
        [Description("Call Update with a valid view and expect Selection to be set correctly.")]
        [Author("JLW")]
        public void Update_ValidSelection_SetsSelection(int selection) {
            SelectListView view = GetDefaultView();
            var updateView = new SelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions) {
                Selection = selection
            };
            view.Update(updateView);
            Assert.AreEqual(selection, view.Selection);
        }

        [TestCaseSource(nameof(ValidSelectionTestCaseSource))]
        [Description("Call UpdateValue with a valid value and expect Selection to be set.")]
        [Author("JLW")]
        public void UpdateValue_ValidValue_SetsSelection(int selection) {
            SelectListView view = GetDefaultView();
            view.UpdateValue(selection.ToString());
            Assert.AreEqual(selection, view.Selection);
        }
        
        [TestCaseSource(nameof(InvalidValueTestCaseSource))]
        [Description("Call UpdateValue with an invalid value and expect an exception to be thrown.")]
        [Author("JLW")]
        public void UpdateValue_InvalidValue_Throws(string value) {
            SelectListView view = GetDefaultView();
            Assert.Throws<FormatException>(() => {
                view.UpdateValue(value);
            });
        }
        
        [TestCaseSource(nameof(ValidSelectionTestCaseSource))]
        [Description("Call GetStringValue and expect Selection to be returned.")]
        [Author("JLW")]
        public void GetStringValue_ReturnsSelection(int selection) {
            SelectListView view = GetDefaultView();
            view.Selection = selection;
            Assert.AreEqual(selection.ToString(), view.GetStringValue());
        }
        
        [TestCaseSource(nameof(ValidSelectionOptionTestCaseSource))]
        [Description("Call GetSelectedOption when the selection is valid and expect the correct option to be returned.")]
        [Author("JLW")]
        public void GetSelectedOption_ValidSelection_ReturnsSelectedOption(int selection, string option) {
            SelectListView view = GetDefaultView();
            view.Selection = selection;
            Assert.AreEqual(option, view.GetSelectedOption());
        }

        [TestCaseSource(nameof(ValidSelectionOptionKeyTestCaseSource))]
        [Description("Call GetSelectedOptionKey when the selection is valid and expect the correct option key to be returned.")]
        [Author("JLW")]
        public void GetSelectedOptionKey_ValidSelection_ReturnsSelectedOptionKey(int selection, string optionKey) {
            SelectListView view = GetDefaultView();
            view.Selection = selection;
            Assert.AreEqual(optionKey, view.GetSelectedOptionKey());
        }

        [TestCaseSource(nameof(StyleTestCaseSource))]
        [Description("Call ToHtml and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void ToHtml_DoesNotThrow(ESelectListType style) {
            var view = new SelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, style);
            Assert.DoesNotThrow(() => {
                _ = view.ToHtml();
            });
        }
        
    }
}