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
            return new SelectListView(DEFAULT_ID, DEFAULT_NAME, _defaultOptions);
        }

        private static IEnumerable<ESelectListType> StyleTestCaseSource() {
            yield return ESelectListType.DropDown;
            yield return ESelectListType.RadioList;
        }

        private static IEnumerable<int> ValidSelectionTestCaseSource() {
            yield return 0;
            yield return 1;
            yield return 2;
        }
        
        private static IEnumerable<int> InvalidSelectionTestCaseSource() {
            yield return -1;
            yield return 3;
        }

        //TODO constructor id-name-options-style-selection
        
        //TODO constructor id-name-options-optionkeys-style-selection
        
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
        
        //TODO Update
        //TODO UpdateValue
        //TODO GetStringValue returns selection
        
        //TODO GetSelectedOption returns the selection when selection is set
        //TODO GetSelectedOption returns empty string when selection is not properly set
        
        //TODO GetSelectedOptionKey returns the selected option key when selection is set
        //TODO GetSelectedOptionKey returns empty string when selection is not properly set
        
        [Test]
        [Description("Call ToHtml and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void ToHtml_DoesNotThrow() {
            SelectListView view = GetDefaultView();
            Assert.DoesNotThrow(() => {
                _ = view.ToHtml();
            });
        }
        
    }
}