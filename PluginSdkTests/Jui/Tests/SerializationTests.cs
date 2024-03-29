using System;
using System.Collections.Generic;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using NUnit.Framework;

namespace HomeSeer.PluginSdkTests.Jui.Tests {

    /// <summary>
    /// A collection of tests for JUI serialization. These tests do not adhere to the new test format, but are kept here for reference for now.
    /// </summary>
    [TestFixture]
    public class SerializationTests {

        private static Page MakeTestSettingsPage() {

			var pageId = new StringBuilder("com-homeseer-juisampleplugin-settings");
			var sampleSettingsPage = PageFactory.CreateSettingsPage(pageId.ToString(), "Settings");
			sampleSettingsPage.WithToggle(new StringBuilder(pageId.ToString()).Append("logtoggle").ToString(), "Enable Logging");

			var logLevelOptions = new List<string> {"Minimal", "Normal", "Detailed"};
            var groupViews = new List<AbstractView>();
            groupViews.Add(new SelectListView(new StringBuilder(pageId.ToString()).Append("loglevel").ToString(), "Log Level", logLevelOptions));

            sampleSettingsPage.WithGroup(new StringBuilder(pageId.ToString()).Append("loggroup").ToString(), "", groupViews);

            return sampleSettingsPage.Page;
		}

        private static Page MakeTestEventActionPage() {

            var pageId = new StringBuilder("com-homeseer-pluginsdktests-eventaction");
            var actionEventPage = PageFactory.CreateEventActionPage(pageId.ToString(), "Event Action");

            List<string> options = new List<string>() { "option1", "option2", "option3" };

            GridView gridView = new GridView("gridview");
            GridRow gridRow = new GridRow();
            gridRow.AddItem(new SelectListView("sel1", "Sel1", options));
            gridRow.AddItem(new SelectListView("sel2", "Sel2", options));
            gridRow.AddItem(new SelectListView("sel3", "Sel3", options));
            gridView.AddRow(gridRow);

            actionEventPage.WithView(gridView);

            return actionEventPage.Page;
        }

        [Test]
		public void ViewTypeSerializationTest() {

			Console.WriteLine("Starting test");
			var testPage = MakeTestSettingsPage();

			Console.WriteLine("Serializing");
			var serializedPage = testPage.ToJsonString();
			Console.WriteLine(serializedPage);
			Console.WriteLine("Deserializing");
			var deserializedPage = Page.FromJsonString(serializedPage);
			
			Assert.IsTrue(deserializedPage.Equals(testPage));
		}

        [Test]
        [Description("Test that serialization/deserialization of a page preserves the object references when the same object is referenced more than once like for GridView")]
        public void PreservingObjectReferencesTest() {

            Console.WriteLine("Starting test");
            var testPage = MakeTestEventActionPage();

            Console.WriteLine("Serializing");
            var serializedPage = testPage.ToJsonString();
            Console.WriteLine(serializedPage);
            Console.WriteLine("Deserializing");
            var deserializedPage = Page.FromJsonString(serializedPage);

            Assert.IsTrue(deserializedPage.Equals(testPage));

            //Update a view value in both the original page and the deserialized page
            testPage.UpdateViewValueById("sel1", "2");
            deserializedPage.UpdateViewValueById("sel1", "2");
            //Serialize both
            var serializedOriginalPage = testPage.ToJsonString();
            var serializedDeserializedPage = deserializedPage.ToJsonString();

            Assert.AreEqual(serializedOriginalPage, serializedDeserializedPage);
        }

        [Test]
		public void HtmlTest() {
			
			Console.WriteLine("Starting test");
			var pageId = "sample-page1";
            var samplePage = PageFactory.CreateSettingsPage(pageId, "Page 1");
            var sampleToggle1 = new ToggleView(new StringBuilder(pageId).Append(".sampletoggle1").ToString(),
                                              "Sample Toggle 1");
            var sampleLabel1 = new LabelView(new StringBuilder(pageId).Append(".samplelabel1").ToString(),
                                             "Sample Label 1",
                                             "This is a sample label with a title");
            var sampleSelectListOptions = new List<string> {"Option 1", "Option 2", "Option 3"};
            var sampleSelectList1 =
                new SelectListView(new StringBuilder(pageId).Append(".sampleselectlist1").ToString(),
                                   "Sample Select List 1",
                                   sampleSelectListOptions);
            
            samplePage.WithView(sampleLabel1);
            samplePage.WithView(sampleToggle1);
            samplePage.WithView(sampleSelectList1);
            var sampleToggle2 = new ToggleView(new StringBuilder(pageId).Append(".sampletoggle2").ToString(),
                                               "Sample Toggle 2");
            var sampleToggle3 = new ToggleView(new StringBuilder(pageId).Append(".sampletoggle3").ToString(),
                                               "Sample Toggle 3");
            var sampleToggle4 = new ToggleView(new StringBuilder(pageId).Append(".sampletoggle4").ToString(),
                                               "Sample Toggle 4");
            var sampleViewGroup1 = new ViewGroup(new StringBuilder(pageId).Append(".sampleviewgroup1").ToString(),
                                                 "Sample View Group 1");
            sampleViewGroup1.AddView(sampleToggle2);
            sampleViewGroup1.AddView(sampleToggle3);
            sampleViewGroup1.AddView(sampleToggle4);
            var sampleLabel2 = new LabelView(new StringBuilder(pageId).Append(".samplelabel2").ToString(),
                                             null,
                                             "This is a sample label without a title");
            var sampleSelectList2 =
                new SelectListView(new StringBuilder(pageId).Append(".sampleselectlist2").ToString(),
                                   "Sample Select List 2",
                                   sampleSelectListOptions, ESelectListType.RadioList, 0);
            
            samplePage.WithView(sampleViewGroup1);
            samplePage.WithView(sampleLabel2);
            samplePage.WithView(sampleSelectList2);
            var sampleInput1 = new InputView(new StringBuilder(pageId).Append(".sampleinput1").ToString(),
                                             "Sample Input 1");
            var sampleInput2 = new InputView(new StringBuilder(pageId).Append(".sampleinput2").ToString(),
                                             "Sample Input 2", EInputType.Number);
            var sampleInput3 = new InputView(new StringBuilder(pageId).Append(".sampleinput3").ToString(),
                                             "Sample Input 3", EInputType.Email);
            var sampleInput4 = new InputView(new StringBuilder(pageId).Append(".sampleinput4").ToString(),
                                             "Sample Input 4", EInputType.Url);
            var sampleInput5 = new InputView(new StringBuilder(pageId).Append(".sampleinput5").ToString(),
                                             "Sample Input 5", EInputType.Password);
            var sampleInput6 = new InputView(new StringBuilder(pageId).Append(".sampleinput6").ToString(),
                                             "Sample Input 6", EInputType.Decimal);
            
            samplePage.WithView(sampleInput1);
            samplePage.WithView(sampleInput2);
            samplePage.WithView(sampleInput3);
            samplePage.WithView(sampleInput4);
            samplePage.WithView(sampleInput5);
            samplePage.WithView(sampleInput6);
			var html = samplePage.Page.ToHtml();
			Console.WriteLine(html);
		}
		
		[Test]
		public void MultiPageHtmlTest() {
			
			Console.WriteLine("Starting test");
			var settings = new SettingsCollection();
			//Build Settings Page 1
            var pageId = "settings-page1";
            var settingsPage1 = PageFactory.CreateSettingsPage(pageId, "Page 1");
            var sampleToggle1 = new ToggleView(new StringBuilder(pageId).Append(".sampletoggle1").ToString(),
                                              "Sample Toggle 1");
            var sampleLabel1 = new LabelView(new StringBuilder(pageId).Append(".samplelabel1").ToString(),
                                             "Sample Label 1",
                                             "This is a sample label with a title");
            var sampleSelectListOptions = new List<string> { "Option 1", "Option 2", "Option 3" };
            var sampleSelectList1 =
                new SelectListView(new StringBuilder(pageId).Append(".sampleselectlist1").ToString(),
                                   "Sample Select List 1",
                                   sampleSelectListOptions);

            settingsPage1.WithView(sampleLabel1);
            settingsPage1.WithView(sampleToggle1);
            settingsPage1.WithView(sampleSelectList1);
            settings.Add(settingsPage1.Page);
            //Build Settings Page 2
            pageId = "settings-page2";
            var settingsPage2 = PageFactory.CreateSettingsPage(pageId, "Page 2");
            var sampleLabel2 = new LabelView(new StringBuilder(pageId).Append(".colorlabel").ToString(),
                                             null,
                                             "These control the list of colors presented for selection in the Sample Guided Process feature page.");
            var redToggle = new ToggleView(new StringBuilder(pageId).Append(".red").ToString(),
                                               "Red", true);
            var orangeToggle = new ToggleView(new StringBuilder(pageId).Append(".orange").ToString(),
                                               "Orange", true);
            var yellowToggle = new ToggleView(new StringBuilder(pageId).Append(".yellow").ToString(),
                                               "Yellow", true);
            var greenToggle = new ToggleView(new StringBuilder(pageId).Append(".green").ToString(),
                                              "Green", true);
            var blueToggle = new ToggleView(new StringBuilder(pageId).Append(".blue").ToString(),
                                              "Blue", true);
            var indigoToggle = new ToggleView(new StringBuilder(pageId).Append(".indigo").ToString(),
                                              "Indigo", true);
            var violetToggle = new ToggleView(new StringBuilder(pageId).Append(".violet").ToString(),
                                              "Violet", true);
            var sampleViewGroup1 = new ViewGroup(new StringBuilder(pageId).Append(".colorgroup").ToString(),
                                                 "Available colors");
            sampleViewGroup1.AddView(sampleLabel2);
            sampleViewGroup1.AddView(redToggle);
            sampleViewGroup1.AddView(orangeToggle);
            sampleViewGroup1.AddView(yellowToggle);
            sampleViewGroup1.AddView(greenToggle);
            sampleViewGroup1.AddView(blueToggle);
            sampleViewGroup1.AddView(indigoToggle);
            sampleViewGroup1.AddView(violetToggle);
            var sampleLabel3 = new LabelView(new StringBuilder(pageId).Append(".samplelabel3").ToString(),
                                             null,
                                             "This is a sample label without a title");
            var sampleSelectList2 =
                new SelectListView(new StringBuilder(pageId).Append(".sampleselectlist2").ToString(),
                                   "Sample Select List 2",
                                   sampleSelectListOptions, ESelectListType.RadioList);

            settingsPage2.WithView(sampleViewGroup1);
            settingsPage2.WithView(sampleLabel3);
            settingsPage2.WithView(sampleSelectList2);
            settings.Add(settingsPage2.Page);
            //Build Settings Page 3
            pageId = "settings-page3";
            var settingsPage3 = PageFactory.CreateSettingsPage(pageId, "Page 3");
            var sampleInput1 = new InputView(new StringBuilder(pageId).Append(".sampleinput1").ToString(),
                                             "Sample Text Input");
            var sampleInput2 = new InputView(new StringBuilder(pageId).Append(".sampleinput2").ToString(),
                                             "Sample Number Input", EInputType.Number);
            var sampleInput3 = new InputView(new StringBuilder(pageId).Append(".sampleinput3").ToString(),
                                             "Sample Email Input", EInputType.Email);
            var sampleInput4 = new InputView(new StringBuilder(pageId).Append(".sampleinput4").ToString(),
                                             "Sample URL Input", EInputType.Url);
            var sampleInput5 = new InputView(new StringBuilder(pageId).Append(".sampleinput5").ToString(),
                                             "Sample Password Input", EInputType.Password);
            var sampleInput6 = new InputView(new StringBuilder(pageId).Append(".sampleinput6").ToString(),
                                             "Sample Decimal Input", EInputType.Decimal);

            settingsPage3.WithView(sampleInput1);
            settingsPage3.WithView(sampleInput2);
            settingsPage3.WithView(sampleInput3);
            settingsPage3.WithView(sampleInput4);
            settingsPage3.WithView(sampleInput5);
            settingsPage3.WithView(sampleInput6);
            settings.Add(settingsPage3.Page);
			var html = settings.ToHtml();
			Console.WriteLine(html);
		}

    }

}