using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HomeSeer.Jui.Views {

    [TestFixture(
        TestOf = typeof(NavigateButtonView),
        Description = "Tests of the NavigateButtonView class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class NavigateButtonViewTests : AbstractJuiViewTestFixture {

        private const string _invalidNameCharacters = "<>\n\r";
        private const string _defaultHomeSeerUrl = "/devices.html";
        
        private static NavigateButtonView GetDefaultView() {
            return new NavigateButtonView(DEFAULT_ID, DEFAULT_NAME, _defaultHomeSeerUrl);
        }

        private static IEnumerable<string> ValidNameTestCaseSource() {
            yield return RANDOMIZER.GetString();
        }

        private static IEnumerable<string> InvalidNameTestCaseSource() {
            yield return null;
            yield return string.Empty;
            yield return " ";
            foreach (var c in _invalidNameCharacters.ToCharArray()) {
                yield return c.ToString();
            }
        }

        private static IEnumerable<string> ValidUrlTestCaseSource() {
            yield return "/devices.html";
        }

        private static IEnumerable<string> InvalidUrlTestCaseSource() {
            yield return null;
            yield return string.Empty;
            yield return RANDOMIZER.GetString();
            yield return "www.homeseer.com";
            yield return "https://www.homeseer.com";
            yield return "192.168.1.1";
        }

        [TestCaseSource(nameof(ValidNameTestCaseSource))]
        [Description("Create a new instance of NavigateButtonView using an Id, valid Name, and Url and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameUrl_ValidName_DoesNotThrow(string name) {
            Assert.DoesNotThrow(() => {
                _ = new NavigateButtonView(DEFAULT_ID, name, _defaultHomeSeerUrl);
            });
        }
        
        [TestCaseSource(nameof(InvalidNameTestCaseSource))]
        [Description("Create a new instance of NavigateButtonView using an Id, invalid Name, and Url and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameUrl_InvalidName_Throws(string name) {
            Assert.Throws<ArgumentException>(() => {
                _ = new NavigateButtonView(DEFAULT_ID, name, _defaultHomeSeerUrl);
            });
        }
        
        [TestCaseSource(nameof(ValidUrlTestCaseSource))]
        [Description("Create a new instance of NavigateButtonView using an Id, Name, and valid Url and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameUrl_ValidUrl_DoesNotThrow(string url) {
            Assert.DoesNotThrow(() => {
                _ = new NavigateButtonView(DEFAULT_ID, DEFAULT_NAME, url);
            });
        }
        
        [TestCaseSource(nameof(InvalidUrlTestCaseSource))]
        [Description("Create a new instance of NavigateButtonView using an Id, Name, and invalid Url and expect an exception to be thrown.")]
        [Author("JLW")]
        public void Constructor_IdNameUrl_InvalidUrl_Throws(string url) {
            Assert.Throws<ArgumentException>(() => {
                _ = new NavigateButtonView(DEFAULT_ID, DEFAULT_NAME, url);
            });
        }
        
        [TestCaseSource(nameof(ValidUrlTestCaseSource))]
        [Description("Get the HomeSeerUrl and expect the correct value to be returned.")]
        [Author("JLW")]
        public void HomeSeerUrl_Get_ReturnsExpected(string url) {
            var view = new NavigateButtonView(DEFAULT_ID, DEFAULT_NAME, url);
            Assert.AreEqual(url, view.HomeSeerUrl);
        }
        
        [TestCaseSource(nameof(ValidUrlTestCaseSource))]
        [Description("Call Update and expect that the HomeSeerUrl is set correctly.")]
        [Author("JLW")]
        public void Update_SetsHomeSeerUrl(string url) {
            NavigateButtonView view = GetDefaultView();
            var updateView = new NavigateButtonView(DEFAULT_ID, DEFAULT_NAME, url);
            view.Update(updateView);
            Assert.AreEqual(url, view.HomeSeerUrl);
        }
        
        [Test]
        [Description("Call GetStringValue and expect null to be returned.")]
        [Author("JLW")]
        public void GetStringValue_ReturnsNull() {
            NavigateButtonView view = GetDefaultView();
            Assert.AreEqual(null, view.GetStringValue());
        }
        
        [Test]
        [Description("Call ToHtml and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void ToHtml_DoesNotThrow() {
            NavigateButtonView view = GetDefaultView();
            Assert.DoesNotThrow(() => {
                _ = view.ToHtml();
            });
        }
        

    }

}