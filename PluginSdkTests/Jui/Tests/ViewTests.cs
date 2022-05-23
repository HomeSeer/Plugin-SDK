using System;
using System.Collections.Generic;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using NUnit.Framework;

namespace HomeSeer.PluginSdkTests.Jui.Tests {

    [TestFixture]
    public class ViewTests {
        
        private const string ViewName = "TestViewName";
		private const string TestText = "TestStringText123";

		#region View Checks
		
		private void CheckId(AbstractView view, string id) {
			Console.WriteLine("Checking ID");
			Assert.AreEqual(id, view.Id);
		}

		private void CheckName(AbstractView view, string name) {
			Console.WriteLine("Checking Name");
			Assert.AreEqual(name, view.Name);
		}

		private void CheckType(AbstractView view, EViewType type) {
			Console.WriteLine("Checking Type");
			Assert.AreEqual(type, view.Type);
		}
		
		#endregion

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
			CheckType(testLabel, EViewType.Label);
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
			CheckType(testInput, EViewType.Input);
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
			}
		}

		#endregion

		#region Select List Tests

		[Test]
		public void SelectListViewValidTest()
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test SelectListView");
			var id = Guid.NewGuid().ToString();
			var name = ViewName;
			var options = new List<string> { "Option 1", "Option 2", "Option 3" };
			var optionKeys = new List<string> { "key1", "key2", "key3" };
			var testSelectList = new SelectListView(id, name, options, optionKeys);

			CheckId(testSelectList, id);
			CheckName(testSelectList, name);
			Console.WriteLine("Checking default selection");
			Assert.AreEqual(-1, testSelectList.Selection);
			Assert.AreEqual("", testSelectList.GetSelectedOption());
			Assert.AreEqual("", testSelectList.GetSelectedOptionKey());
			Assert.AreEqual(ESelectListType.DropDown, testSelectList.Style);
			CheckType(testSelectList, EViewType.SelectList);

			Console.WriteLine("Changing selection and rechecking");
			testSelectList.Selection = 2;
			Assert.AreEqual(options[2], testSelectList.GetSelectedOption());
			Assert.AreEqual(optionKeys[2], testSelectList.GetSelectedOptionKey());
		}

		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void SelectListViewInvalidIdTest(string id)
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test SelectListView");
			var name = ViewName;
			var options = new List<string> { "Option 1", "Option 2", "Option 3" };
			try
			{
				var inputView = new SelectListView(id, name, options);
				var failMessage = new StringBuilder("The select list was created with an invalid ID: ")
					.Append(id ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e)
			{
				Assert.AreEqual("id", e.ParamName);
			}
		}

		private static readonly object[] _selectListViewInvalidOptionsTestCases =
		{
			null,
			new List<string>()
		};

		[TestCaseSource("_selectListViewInvalidOptionsTestCases")]
		public void SelectListViewInvalidOptionsTest(List<string> options)
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test SelectListView");
			var id = Guid.NewGuid().ToString();
			var name = ViewName;
			var optionKeys = new List<string> { "key1", "key2", "key3" };
			try
			{
				var selectListView = new SelectListView(id, name, options);
				var failMessage = "The select list was created with an invalid options list";
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e)
			{
				Assert.AreEqual("options", e.ParamName);
			}
			try
			{
				var selectListView2 = new SelectListView(id, name, options, optionKeys);
				var failMessage = "The select list was created with an invalid options list";
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e)
			{
				Assert.AreEqual("options", e.ParamName);
			}
		}

		private static readonly object[] _selectListViewInvalidOptionKeysTestCases =
		{
			null,
			new List<string>(),
			new List<string>() { "key1"},
			new List<string>() { "key1", "key2", "key3", "key4"},
		};

		[TestCaseSource("_selectListViewInvalidOptionKeysTestCases")]
		public void SelectListViewInvalidOptionKeysTest(List<string> optionKeys)
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test SelectListView");
			var id = Guid.NewGuid().ToString();
			var options = new List<string> { "Option 1", "Option 2", "Option 3" };
			var name = ViewName;
			try
			{
				var selectListView = new SelectListView(id, name, options, optionKeys);
				var failMessage = "The select list was created with an invalid option keys list";
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e)
			{
				Assert.AreEqual("optionKeys", e.ParamName);
			}
			catch (ArgumentException)
			{
			}
		}

		[TestCase(-2)]
		[TestCase(-100)]
		[TestCase(4)]
		[TestCase(1000)]
		public void SelectListViewInvalidSelectionTest(int selection)
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test SelectListView");
			var id = Guid.NewGuid().ToString();
			var options = new List<string> { "Option 1", "Option 2", "Option 3" };
			var optionKeys = new List<string> { "key1", "key2", "key3" };
			var name = ViewName;
			try
			{
				var selectListView = new SelectListView(id, name, options, ESelectListType.DropDown, selection);
				var failMessage = $"The select list was created with an invalid selection index {selection}";
				Assert.Fail(failMessage);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Assert.AreEqual("selection", e.ParamName);
			}
			try
			{
				var selectListView2 = new SelectListView(id, name, options, optionKeys, ESelectListType.DropDown, selection);
				var failMessage = $"The select list was created with an invalid selection index {selection}";
				Assert.Fail(failMessage);
			}
			catch (ArgumentOutOfRangeException e)
			{
				Assert.AreEqual("selection", e.ParamName);
			}
		}
		#endregion

		#region Toggle Tests
		[Test]
		public void ToggleViewValidTest()
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test ToggleView");
			var id = Guid.NewGuid().ToString();
			var name = ViewName;
			var testToggle = new ToggleView(id, name, true);

			CheckId(testToggle, id);
			CheckName(testToggle, name);
			Console.WriteLine("Checking Value");
			Assert.AreEqual(true, testToggle.IsEnabled);
			CheckType(testToggle, EViewType.Toggle);
		}

		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void ToggleViewInvalidIdTest(string id)
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test ToggleView");
			var name = ViewName;
			try
			{
				var toggleView = new ToggleView(id, name);
				var failMessage = new StringBuilder("The toggle was created with an invalid ID: ")
					.Append(id ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e)
			{
				Assert.AreEqual("id", e.ParamName);
			}
		}
		#endregion

		#region Text Area Tests
		[Test]
		public void TextAreaViewValidTest()
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test TextAreaView");
			var id = Guid.NewGuid().ToString();
			var name = ViewName;
			var value = TestText;
			var testTextArea = new TextAreaView(id, name, value, 10);

			CheckId(testTextArea, id);
			CheckName(testTextArea, name);
			Console.WriteLine("Checking Value");
			Assert.AreEqual(TestText, testTextArea.Value);
			CheckType(testTextArea, EViewType.TextArea);
		}

		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void TextAreaViewInvalidIdTest(string id)
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test TextAreaView");
			var name = ViewName;
			try
			{
				var textAreaView = new TextAreaView(id, name);
				var failMessage = new StringBuilder("The text area was created with an invalid ID: ")
					.Append(id ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e)
			{
				Assert.AreEqual("id", e.ParamName);
			}
		}
		#endregion

		#region Time Span Tests
		[Test]
		public void TimeSpanViewValidTest()
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test TimeSpanView");
			var id = Guid.NewGuid().ToString();
			var name = ViewName;
			var value = TimeSpan.FromMinutes(1234);
			var testTimeSpan = new TimeSpanView(id, name, value);

			CheckId(testTimeSpan, id);
			CheckName(testTimeSpan, name);
			Console.WriteLine("Checking Value");
			Assert.AreEqual(value, testTimeSpan.Value);
			CheckType(testTimeSpan, EViewType.TimeSpan);
		}

		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void TimeSpanViewInvalidIdTest(string id)
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test TimeSpanView");
			var name = ViewName;
			try
			{
				var timeSpanView = new TimeSpanView(id, name);
				var failMessage = new StringBuilder("The time span was created with an invalid ID: ")
					.Append(id ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e)
			{
				Assert.AreEqual("id", e.ParamName);
			}
		}
		#endregion

		#region View Group Tests
		[Test]
		public void ViewGroupValidTest()
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test ViewGroup");
			var id = Guid.NewGuid().ToString();
			var name = ViewName;
			var testViewGroup = new ViewGroup(id, name);
			var labelViewId = $"{id}-lv";
			var inputViewId = $"{id}-iv";
			var timeSpanViewId = $"{id}-tsv";

			testViewGroup.AddView(new LabelView(labelViewId, "Label View"));
			testViewGroup.AddView(new InputView(inputViewId, "Input View"));
			testViewGroup.AddView(new TimeSpanView(timeSpanViewId, "TimeSpan View"));
			CheckId(testViewGroup, id);
			CheckName(testViewGroup, name);
			Console.WriteLine("Checking child views");
			Assert.AreEqual(3, testViewGroup.ViewCount);
			Assert.AreEqual(true, testViewGroup.ContainsViewWithId(labelViewId));
			Assert.AreEqual(true, testViewGroup.ContainsViewWithId(inputViewId));
			Assert.AreEqual(true, testViewGroup.ContainsViewWithId(timeSpanViewId));
			CheckType(testViewGroup, EViewType.Group);
		}

		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void ViewGroupInvalidIdTest(string id)
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test ViewGroup");
			var name = ViewName;
			try
			{
				var textAreaView = new ViewGroup(id, name);
				var failMessage = new StringBuilder("The view group was created with an invalid ID: ")
					.Append(id ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e)
			{
				Assert.AreEqual("id", e.ParamName);
			}
		}
		#endregion

		#region Grid View Tests
		[Test]
		public void GridViewValidTest()
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test GridView");
			var id = Guid.NewGuid().ToString();
			var name = ViewName;
			var testGridView = new GridView(id, name);
			var labelViewId = $"{id}-lv";
			var inputViewId = $"{id}-iv";
			var timeSpanViewId = $"{id}-tsv";
			var row = new GridRow();

			testGridView.AddView(new LabelView(labelViewId, "Label View"));
			row.AddItem(new InputView(inputViewId, "Input View"));
			row.AddItem(new TimeSpanView(timeSpanViewId, "TimeSpan View"));
			testGridView.AddRow(row);
			CheckId(testGridView, id);
			CheckName(testGridView, name);
			Console.WriteLine("Checking child views");
			Assert.AreEqual(3, testGridView.ViewCount);
			Assert.AreEqual(true, testGridView.ContainsViewWithId(labelViewId));
			Assert.AreEqual(true, testGridView.ContainsViewWithId(inputViewId));
			Assert.AreEqual(true, testGridView.ContainsViewWithId(timeSpanViewId));
			CheckType(testGridView, EViewType.Group);
		}

		[TestCase("")]
		[TestCase(" ")]
		[TestCase(null)]
		public void GridViewInvalidIdTest(string id)
		{
			Console.WriteLine("Starting test");
			Console.WriteLine("Creating test GridView");
			var name = ViewName;
			try
			{
				var textAreaView = new GridView(id, name);
				var failMessage = new StringBuilder("The grid view was created with an invalid ID: ")
					.Append(id ?? "NULL").ToString();
				Assert.Fail(failMessage);
			}
			catch (ArgumentNullException e)
			{
				Assert.AreEqual("id", e.ParamName);
			}
		}
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