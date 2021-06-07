using System;
using System.Collections.Generic;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using NUnit.Framework;

namespace HomeSeer.PSDKTests.Jui.Tests {

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
		
		/*[Test]
		public void PageDeltaTest() {

			Console.WriteLine("Starting test");
			var testPage = Page.Factory.CreateSettingPage("testPage", "page1");
			testPage.AddViewDelta("id1",5, true);
			testPage.AddViewDelta("id2",3, 2);
			testPage.AddViewDelta("id3",5, false);
			
			var testPage2 = Page.Factory.CreateSettingPage("testPage2", "page2");
			testPage.AddViewDelta("id4",5, true);
			testPage.AddViewDelta("id5",3, 2);
			testPage.AddViewDelta("id6",5, false);

			var pageList = new List<Page>();
			pageList.Add(testPage);
			pageList.Add(testPage2);
			Console.WriteLine("Serializing");
			var serializedPageList = Page.JsonFromList(pageList);
			Console.WriteLine(serializedPageList);
			
			Console.WriteLine("Deserializing");
			var deserializedPageList = Page.ListFromJson(serializedPageList);
			Assert.IsTrue(deserializedPageList.Equals(pageList));
		}*/

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
            //var sampleButton1 = new ButtonView(new StringBuilder(pageId).Append(".samplebutton1").ToString(), "Sample Button 1", "samplebutton1");
            
            samplePage.WithView(sampleLabel1);
            samplePage.WithView(sampleToggle1);
            samplePage.WithView(sampleSelectList1);
            //samplePage.WithView(sampleButton1);
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
            //var sampleButton1 = new ButtonView(new StringBuilder(pageId).Append(".samplebutton1").ToString(), "Sample Button 1", "samplebutton1");

            settingsPage1.WithView(sampleLabel1);
            settingsPage1.WithView(sampleToggle1);
            settingsPage1.WithView(sampleSelectList1);
            //settingsPage1.WithView(sampleButton1);
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
		
		/*public static string Encrypt(string input, string key) {  
			var inputArray = UTF8Encoding.UTF8.GetBytes(input);  
			var tripleDES  = new TripleDESCryptoServiceProvider();
			var keyBytes = UTF8Encoding.UTF8.GetBytes(key);
			var trimmedKey = new byte[16];
			for (var i = 0; i < 16; i++) {
				trimmedKey[i] = keyBytes[i];
			}
			tripleDES.Key = trimmedKey;
			tripleDES.Mode = CipherMode.ECB;  
			tripleDES.Padding = PaddingMode.PKCS7;  
			var cTransform  = tripleDES.CreateEncryptor();  
			var resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);  
			tripleDES.Clear();  
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);  
		}  */

    }

}