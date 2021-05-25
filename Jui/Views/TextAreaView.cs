using HomeSeer.Jui.Types;
using Newtonsoft.Json;
using System;
using System.Text;

namespace HomeSeer.Jui.Views
{

    /// <inheritdoc cref="AbstractView"/>
    /// <summary>
    /// A text area view is an editable text box for the user to enter a large volume of text
    /// </summary>
    public sealed class TextAreaView : AbstractView 
	{	
		/// <summary>
		/// The number of text raws displayed
		/// </summary>
		[JsonProperty("rows")]
		public int Rows { get; set; }

		/// <summary>
		/// The current value of the field
		/// </summary>
		[JsonProperty("value")]
		public string Value { get; set; } = "";

		/// <inheritdoc cref="AbstractView"/>
		/// <summary>
		/// Create a new instance of an TextAreaView with an ID, a Name, and the number of rows.
		/// </summary>
		/// <param name="id">The unique ID for the View</param>
		/// <param name="name">The name of the View</param>
		/// <param name="rows">The number of text rows in the text area. DEFAULT: 5</param>
		[JsonConstructor]
		public TextAreaView(string id, string name, int rows = 5) : base(id, name) 
		{	
			Type = EViewType.TextArea;
			Rows = rows;
		}

		/// <inheritdoc cref="AbstractView"/>
		/// <summary>
		/// Create a new instance of an TextAreaView with an ID, a Name, a Value, and the number of rows.
		/// </summary>
		/// <param name="id">The unique ID for the View</param>
		/// <param name="name">The name of the View</param>
		/// <param name="value">The value inputted into the field</param>
		/// <param name="rows">The number of text rows in the text area. DEFAULT: 5</param>
		public TextAreaView(string id, string name, string value, int rows = 5) : base(id, name) 
		{
			Type = EViewType.TextArea;
			Rows = rows;
			Value = value ?? "";
		}

		/// <inheritdoc cref="AbstractView.Update"/>
		/// <summary>
		/// Update the view to the new state.  This will change the inputted value 
		/// </summary>
		/// <exception cref="ViewTypeMismatchException">Thrown when the new view's class doesn't match the calling view</exception>
		public override void Update(AbstractView newViewState) 
		{
			base.Update(newViewState);

			if (!(newViewState is TextAreaView updatedTextAreaView)) 
			{
				throw new ViewTypeMismatchException("The original view type does not match the type in the update");
			}			
			Value = updatedTextAreaView.Value ?? "";
		}

		/// <inheritdoc cref="AbstractView.UpdateValue"/>
		public override void UpdateValue(string value) 
		{	
			Value = value ?? "";
		}

		/// <inheritdoc cref="AbstractView.GetStringValue"/>
		/// <remarks>
		/// The same as <see cref="Value"/>
		/// </remarks>
		public override string GetStringValue() 
		{
			return Value;
		}

		/// <inheritdoc cref="AbstractView.ToHtml"/>
		public override string ToHtml(int indent = 0) {

			var sb = new StringBuilder();
			sb.Append(GetIndentStringFromNumber(indent));
			//Open the form div
			sb.Append($"<div id=\"{Id}-par\" class=\"md-form md-outline jui-view\">");
			sb.Append(Environment.NewLine);
            //Add the text area
            sb.Append(GetIndentStringFromNumber(indent+1));
            sb.Append($"<textarea id=\"{Id}\" class=\"md-textarea form-control jui-input\" rows=\"{Rows}\">");
			sb.Append(Value);
			sb.Append("</textarea>");
			sb.Append(Environment.NewLine);
			//Add the hint label
			sb.Append(GetIndentStringFromNumber(indent+1));
            sb.Append($"<label for=\"{Id}\" id=\"{Id}-hint\">{Name}</label>");
            sb.Append(Environment.NewLine);
            //Close the form div
            sb.Append(GetIndentStringFromNumber(indent));
            sb.Append("</div>");
            sb.Append(Environment.NewLine);
			
			return sb.ToString();
		}

	}

}