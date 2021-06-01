using System;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using NUnit.Framework;

namespace HomeSeer.PSDKTests.Jui.Tests {

    [TestFixture]
    public class ViewTests {
        
        private const string ViewName = "TestViewName";
		private const string TestText = "TestStringText123";
		private const string ButtonActionId = "com.homeseer.plugintest.pagetest.buttonactiontest";

		#region View Checks
		
		private void CheckId(AbstractView view, string id) {
			Console.WriteLine("Checking ID");
			Assert.AreEqual(id, view.Id);
		}

		private void CheckName(AbstractView view, string name) {
			Console.WriteLine("Checking Name");
			Assert.AreEqual(name, view.Name);
		}

		private void CheckType(AbstractView view) {
			Console.WriteLine("Checking Type");
			var type = EViewType.Undefined;
			switch (view.Type) {
				case EViewType.Group:
					type = EViewType.Group;
					break;
				case EViewType.Label:
					type = EViewType.Label;
					break;
				case EViewType.SelectList:
					type = EViewType.SelectList;
					break;
				case EViewType.Input:
					type = EViewType.Input;
					break;
				case EViewType.Toggle:
					type = EViewType.Toggle;
					break;
				default:
					Assert.Fail("View type is undefined");
					break;
			}
			
			Assert.AreEqual(type, view.Type);
		}
		
		#endregion

		/*#region Button Tests
		
		[Test]
		public void ButtonViewValidTest() {
			
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test ButtonView");
			var id = Guid.NewGuid().ToString();
			var name = ViewName;
			var actionId = ButtonActionId;
			var testButton = new ButtonView(id, name, actionId);
			
			CheckId(testButton, id);
			CheckName(testButton, name);
			Console.WriteLine("Checking ActionId");
			Assert.AreEqual(actionId, testButton.ActionId);
			CheckType(testButton);
		}
		
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void ButtonViewInvalidIdTest(string id) {
			
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test ButtonView");
			var name = ViewName;
			var actionId = ButtonActionId;
			try {
				var testButton = new ButtonView(id, name, actionId);
				var failMessage = new StringBuilder("The button was created with an invalid ID: ")
					.Append(id ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e) {
				Assert.AreEqual("id", e.ParamName);
				Assert.Pass();
			}
		}
		
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void ButtonViewInvalidNameTest(string name) {
			
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test ButtonView");
			var id = Guid.NewGuid().ToString();
			var actionId = ButtonActionId;
			try {
				var testButton = new ButtonView(id, name, actionId);
				var failMessage = new StringBuilder("The button was created with an invalid Name: ")
					.Append(name ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e) {
				Assert.AreEqual("name", e.ParamName);
				Assert.Pass();
			}
		}
		
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void ButtonViewInvalidActionIdTest(string actionId) {
			
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test ButtonView");
			var id = Guid.NewGuid().ToString();
			var name = ViewName;
			try {
				var testButton = new ButtonView(id, name, actionId);
				var failMessage = new StringBuilder("The button was created with an invalid ActionId: ")
					.Append(actionId ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e) {
				Assert.AreEqual("actionId", e.ParamName);
				Assert.Pass();
			}
		}
		
		#endregion*/
		
		#region Label Tests
		
		[Test]
		public void LabelViewValidTest() {
			
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test LabelView");
			var id = Guid.NewGuid().ToString();
			var name = ViewName;
			var value = TestText;
			var testLabel = new LabelView(id, name, value);
			
			CheckId(testLabel, id);
			CheckName(testLabel, name);
			Console.WriteLine("Checking Value");
			Assert.AreEqual(value, testLabel.Value);
			CheckType(testLabel);
		}
		
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void LabelViewInvalidIdTest(string id) {
			
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test LabelView");
			var name = ViewName;
			var value = TestText;
			try {
				var testLabel = new LabelView(id, name, value);
				var failMessage = new StringBuilder("The label was created with an invalid ID: ")
					.Append(id ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e) {
				Assert.AreEqual("id", e.ParamName);
				Assert.Pass();
			}
		}
		
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void LabelViewInvalidNameOnlyTest(string name) {
			
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test LabelView");
			var id = Guid.NewGuid().ToString();
			try {
				var testLabel = new LabelView(id, name);
				var failMessage = new StringBuilder("The label was created with an invalid Name: ")
					.Append(name ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e) {
				Assert.AreEqual("name", e.ParamName);
				Assert.Pass();
			}
		}
		
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void LabelViewInvalidNameValueTest(string value) {
			
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test LabelView");
			var id = Guid.NewGuid().ToString();
			try {
				var testLabel = new LabelView(id, null, value);
				var failMessage = new StringBuilder("The label was created with an invalid Name: NULL and Value: ")
					.Append(value ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e) {
				Assert.AreEqual("value", e.ParamName);
				Assert.Pass();
			}
		}
		
		#endregion
		
		#region Input Tests
		
		[Test]
		public void InputViewValidTest() {
			
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test InputView");
			var id = Guid.NewGuid().ToString();
			var name = ViewName;
			var value = TestText;
			var testInput = new InputView(id, name, value);
			
			CheckId(testInput, id);
			CheckName(testInput, name);
			Console.WriteLine("Checking Value");
			Assert.AreEqual(value, testInput.Value);
			Assert.AreEqual(EInputType.Text, testInput.InputType);
			CheckType(testInput);
		}
		
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void InputViewInvalidIdTest(string id) {
			
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test InputView");
			var name = ViewName;
			var value = TestText;
			try {
				var inputView = new InputView(id, name, value);
				var failMessage = new StringBuilder("The input was created with an invalid ID: ")
					.Append(id ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e) {
				Assert.AreEqual("id", e.ParamName);
				Assert.Pass();
			}
		}
		
		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void InputViewInvalidNameTest(string name) {
			
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test InputView");
			var id = Guid.NewGuid().ToString();
			try {
				var inputView = new InputView(id, name);
				var failMessage = new StringBuilder("The input was created with an invalid Name: ")
					.Append(name ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e) {
				Assert.AreEqual("name", e.ParamName);
				Assert.Pass();
			}
		}
		
		#endregion
		
		#region Select List Tests
		
		#endregion
		
		#region Toggle Tests
		
		#endregion

        //Create tests
        
        //ID tests
        
        //Name tests
        
        //Type tests
        
        //GetStringValue tests
        
        //Update tests
        
        //UpdateValue tests
        
        //ToHtml tests
        

    }

}