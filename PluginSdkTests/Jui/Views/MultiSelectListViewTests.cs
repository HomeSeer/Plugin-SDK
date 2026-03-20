using HomeSeer.Jui.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HomeSeer.Jui.Views {
    
    [TestFixture(
        TestOf = typeof(MultiSelectListView),
        Description = "Tests of the MultiSelectListView class to ensure it behaves as expected under normal conditions.")]
    public class MultiSelectListViewTests : AbstractJuiViewTestFixture {

        private static readonly List<string> _defaultOptionKeys = new List<string> {
            "k1", 
            "k2", 
            "k3"
        };
        
        private static readonly List<string> _defaultOptions = new List<string> {
            "option1", 
            "option2", 
            "option3"
        };
        
        private static MultiSelectListView GetDefaultView() {
            return new MultiSelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, _defaultOptionKeys, EMultiSelectListType.DropDown, new List<int>());
        }

        private static IEnumerable<EMultiSelectListType> StyleTestCaseSource() {
            yield return EMultiSelectListType.DropDown;
            yield return EMultiSelectListType.SearchableDropDown;
        }

        private static IEnumerable<List<int>> ValidSelectionTestCaseSource() {
            yield return new List<int>() { 0 };
            yield return new List<int>() { 1 };
            yield return new List<int>() { 2 };
            yield return new List<int>() { 1, 2 };
        }

        private static IEnumerable<string> ValidOptionKeysTestCaseSource() {
            yield return _defaultOptionKeys[0];
            yield return _defaultOptionKeys[1];
            yield return _defaultOptionKeys[2];
        }

        private static IEnumerable<List<int>> InvalidSelectionTestCaseSource() {
            yield return new List<int>() { -2 };
            yield return new List<int>() { 3 };
        }

        private static IEnumerable<object[]> ValidSelectionOptionTestCaseSource() {
            yield return new object[] {
                new List<int> { 0 },
                new List<string> {_defaultOptions[0] }
            };
            yield return new object[] {
                new List<int> { 1 },
                new List<string> {_defaultOptions[1] }
            };
            yield return new object[] {
                new List<int> { 2 },
                new List<string> {_defaultOptions[2] }
            };
            yield return new object[] {
                new List<int> { 0, 2 },
                new List<string> {_defaultOptions[0], _defaultOptions[2] }
            };
            yield return new object[] {
                new List<int> { 0, 1, 2 },
                new List<string> {_defaultOptions[0], _defaultOptions[1], _defaultOptions[2] }
            };
        }

        private static IEnumerable<object[]> ValidSelectionOptionKeyTestCaseSource() {
            yield return new object[] {
                new List<int> { 0 },
                new List<string> { _defaultOptionKeys[0] }
            };
            yield return new object[] {
                new List<int> { 1 },
                new List<string> { _defaultOptionKeys[1] }
            };
            yield return new object[] {
                new List<int> { 2 },
                new List<string> { _defaultOptionKeys[2] }
            };
            yield return new object[] {
                new List<int> { 0, 2 },
                new List<string> { _defaultOptionKeys[0], _defaultOptionKeys[2] }
            };
        }

        private static IEnumerable<string> ValidValueTestCaseSource() {
            yield return _defaultOptionKeys[0];
            yield return _defaultOptionKeys[1];
            yield return $"{_defaultOptionKeys[0]},{_defaultOptionKeys[1]}";
            yield return $"{_defaultOptionKeys[0]},{_defaultOptionKeys[1]},{_defaultOptionKeys[2]}";
        }

        private static IEnumerable<string> InvalidValueTestCaseSource() {
            yield return RANDOMIZER.GetString();
            yield return RANDOMIZER.NextBool().ToString();
            yield return "1,2,3";
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
        [Description("Create a new instance of MultiSelectListView using the IdNameOptionsOptionKeysStyleSelection constructor and expect no exceptions to be thrown.")]
        public void Constructor_IdNameOptionsOptionKeysStyleSelection_DoesNotThrow() {
            Assert.DoesNotThrow(() => {
                _ = new MultiSelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, _defaultOptionKeys, EMultiSelectListType.DropDown, new List<int>());
            });
        }
        
        [TestCaseSource(nameof(InvalidOptionsTestCaseSource))]
        [Description("Create a new instance of MultiSelectListView using the IdNameOptionsOptionKeysStyleSelection constructor with invalid options and expect an exception to be thrown.")]
        public void Constructor_IdNameOptionsOptionKeysStyleSelection_InvalidOptions_Throws(List<string> options) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = new MultiSelectListView(DEFAULT_ID, DEFAULT_NAME, options, _defaultOptionKeys, EMultiSelectListType.DropDown, new List<int>());
            });
        }
        
        [TestCaseSource(nameof(InvalidSelectionTestCaseSource))]
        [Description("Create a new instance of MultiSelectListView using the IdNameOptionsOptionKeysStyleSelection constructor with invalid selection and expect an exception to be thrown.")]
        public void Constructor_IdNameOptionsOptionKeysStyleSelection_InvalidSelection_Throws(List<int> selection) {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                _ = new MultiSelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, _defaultOptionKeys, EMultiSelectListType.DropDown, selection);
            });
        }
        
        [TestCaseSource(nameof(InvalidOptionKeysTestCaseSource))]
        [Description("Create a new instance of MultiSelectListView using the IdNameOptionsOptionKeysStyleSelection constructor with invalid option keys and expect an exception to be thrown.")]
        public void Constructor_IdNameOptionsOptionKeysStyleSelection_InvalidOptionKeys_Throws(List<string> optionKeys) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = new MultiSelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, optionKeys, EMultiSelectListType.DropDown, new List<int>());
            });
        }
        
        [TestCaseSource(nameof(OptionsAndOptionKeysDoNotMatchTestCaseSource))]
        [Description("Create a new instance of MultiSelectListView using the IdNameOptionsOptionKeysStyleSelection constructor with options and option keys that do not match and expect an exception to be thrown.")]
        public void Constructor_IdNameOptionsOptionKeysStyleSelection_OptionsAndKeysDoNotMatch_Throws(List<string> options, List<string> optionKeys) {
            Assert.Throws<ArgumentException>(() => {
                _ = new MultiSelectListView(DEFAULT_ID, DEFAULT_NAME, options, optionKeys, EMultiSelectListType.DropDown, new List<int>());
            });
        }
        
        [Test]
        [Description("Get the Options and expect the options set during initialization to be returned.")]
        public void Options_Get_ReturnsExpected() {
            var view = new MultiSelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, _defaultOptionKeys, EMultiSelectListType.DropDown, new List<int>());
            Assert.AreEqual(_defaultOptions, view.Options);
        }
        
        [Test]
        [Description("Get the OptionKeys and expect the option keys set during initialization to be returned.")]
        public void OptionKeys_Get_ReturnsExpected() {
            var view = new MultiSelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, _defaultOptionKeys, EMultiSelectListType.DropDown, new List<int>());
            Assert.AreEqual(_defaultOptionKeys, view.OptionKeys);
        }
        
        [Test]
        [Description("Get the Style and expect the default style to be returned.")]
        public void Style_Get_ReturnsDefault() {
            MultiSelectListView view = GetDefaultView();
            Assert.AreEqual(EMultiSelectListType.DropDown, view.Style);
        }
        
        [TestCaseSource(nameof(StyleTestCaseSource))]
        [Description("Set the Style and expect it to be set correctly.")]
        public void Style_Set_SetsStyle(EMultiSelectListType style) {
            MultiSelectListView view = GetDefaultView();
            view.Style = style;
            Assert.AreEqual(style, view.Style);
        }
        
        [Test]
        [Description("Get the Selection and expect the default Selection to be returned.")]
        public void Selection_Get_ReturnsDefault() {
            MultiSelectListView view = GetDefaultView();
            Assert.AreEqual(new List<int>(), view.Selection);
        }
        
        [TestCaseSource(nameof(InvalidSelectionTestCaseSource))]
        [Description("Set the Selection using an invalid value and expect an exception to be thrown.")]
        public void Selection_Set_Invalid_Throws(List<int> selection) {
            MultiSelectListView view = GetDefaultView();
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                view.Selection = selection;
            });
        }
        
        [TestCaseSource(nameof(ValidSelectionTestCaseSource))]
        [Description("Set the Selection using a valid value and expect it to be set correctly.")]
        public void Selection_Set_Valid_SetsSelection(List<int> selection) {
            MultiSelectListView view = GetDefaultView();
            view.Selection = selection;
            Assert.AreEqual(selection, view.Selection);
        }
        
        [TestCaseSource(nameof(ValidSelectionTestCaseSource))]
        [Description("Call Update with a valid view and expect Selection to be set correctly.")]
        public void Update_ValidSelection_SetsSelection(List<int> selection) {
            MultiSelectListView view = GetDefaultView();
            var updateView = new MultiSelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, _defaultOptionKeys, EMultiSelectListType.DropDown, selection);
            view.Update(updateView);
            Assert.AreEqual(selection, view.Selection);
        }

        [TestCaseSource(nameof(ValidValueTestCaseSource))]
        [Description("Call UpdateValue  with a valid value and expect GetStringValue to be correct.")]
        public void UpdateValue_ValidValue_ReturnsValue(string value) {
            MultiSelectListView view = GetDefaultView();
            view.UpdateValue(value);
            Assert.AreEqual(value, view.GetStringValue());
        }

        [TestCaseSource(nameof(InvalidValueTestCaseSource))]
        [Description("Call UpdateValue with an invalid value and expect GetStringValue to be empty.")]
        public void UpdateValue_InvalidValue_ReturnsEmptyValue(string value) {
            MultiSelectListView view = GetDefaultView();
            view.UpdateValue(value);
            Assert.AreEqual("", view.GetStringValue());
        }

        [TestCaseSource(nameof(ValidSelectionOptionTestCaseSource))]
        [Description("Call GetSelectedOptions when the selection is valid and expect the correct option to be returned.")]
        public void GetSelectedOption_ValidSelection_ReturnsSelectedOption(List<int> selection, List<string> option) {
            MultiSelectListView view = GetDefaultView();
            view.Selection = selection;
            Assert.AreEqual(option, view.GetSelectedOptions());
        }

        [TestCaseSource(nameof(ValidSelectionOptionKeyTestCaseSource))]
        [Description("Call GetSelectedOptionKeys when the selection is valid and expect the correct option key to be returned.")]
        public void GetSelectedOptionKey_ValidSelection_ReturnsSelectedOptionKey(List<int> selection, List<string> optionKeys) {
            MultiSelectListView view = GetDefaultView();
            view.Selection = selection;
            Assert.AreEqual(optionKeys, view.GetSelectedOptionKeys());
        }

        [TestCaseSource(nameof(StyleTestCaseSource))]
        [Description("Call ToHtml and expect no exceptions to be thrown.")]
        public void ToHtml_DoesNotThrow(EMultiSelectListType style) {
            var view = new MultiSelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions, _defaultOptionKeys, style, new List<int>());
            Assert.DoesNotThrow(() => {
                _ = view.ToHtml();
            });
        }
        
    }
}