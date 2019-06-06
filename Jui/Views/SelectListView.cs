using HomeSeer.Jui.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <inheritdoc />
	/// <summary>
	/// A selection list allows a user to pick a value from a predefined collection.
	/// </summary>
	public sealed class SelectListView : AbstractView {

		/// <summary>
		/// A list of options that the user can select from
		/// </summary>
		[JsonProperty("options")]
		public List<string> Options { get; private set; }

		/// <summary>
		/// The display style for the list of options
		/// </summary>
		[JsonProperty("style")]
		public ESelectListType Style { get; set; }
		
		/// <summary>
		/// The index of the currently selected option in the list
		/// </summary>
		[JsonProperty("selection")]
		public int Selection { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Create a new instance of a select list with the default, drop down style, an ID, Name, and the specified list of options
        /// </summary>
        /// <param name="id">The unique ID for this View</param>
        /// <param name="name">The name of the view</param>
        /// <param name="options">The list of options</param>
        /// <param name="style">The display style of the select list. DEFAULT: drop down</param>
        /// <param name="selection">The index of the currently selected option in the list. DEFAULT: 0</param>
        /// <exception cref="ArgumentNullException">Thrown if select list is create with an invalid list of options</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if select list is created with an invalid index for the currently selected option</exception>
        [JsonConstructor]
		public SelectListView(string id, string name, List<string> options, ESelectListType style = ESelectListType.DropDown, int selection = 0) : base(id, name) {

			if (options == null || options.Count == 0) {
				throw new ArgumentNullException(nameof(options));
			}
            if (selection < 0 || selection >= options.Count ){
                throw new ArgumentOutOfRangeException(nameof(selection));
            }
			
			Type = EViewType.SelectList;
			Options = options;
			Style = style;
			Selection = selection;
		}
		
		/// <inheritdoc />
		/// <summary>
		/// Update the view to the new state.  This will change the selected option 
		/// </summary>
		/// <exception cref="ViewTypeMismatchException">Thrown when the new view's class doesn't match the calling view</exception>
		public override void Update(AbstractView newViewState) {
			base.Update(newViewState);
			
			if (!(newViewState is SelectListView updatedSelectListView)) {
				throw new ViewTypeMismatchException("The original view type does not match the type in the update");
			}

			Selection = updatedSelectListView.Selection;
		}

		/// <inheritdoc />
		/// <exception cref="FormatException">Thrown when the value is not in the correct format</exception>
		public override void UpdateValue(string value) {

			try {
				Selection = int.Parse(value);
			}
			catch (Exception exception) {
				Console.WriteLine(exception);
				throw new FormatException("Value is not in the correct format", exception);
			}
		}

		/// <inheritdoc/>
		public override string GetStringValue() {
			return Selection.ToString();
		}
		
		/// <inheritdoc/>
		internal override string ToHtml(int indent = 0) {
			
			var sb = new StringBuilder();
			sb.Append(GetIndentStringFromNumber(indent));
			//Open the containing div
			sb.Append("<div class=\"jui-view\">");
			sb.Append(Environment.NewLine);
			//Add the select list
			switch (Style) {
				case ESelectListType.DropDown:
					//Add the button
					sb.Append(GetIndentStringFromNumber(indent+1));
					sb.Append("<select class=\"mdb-select md-form jui-input\" id=\"");
					sb.Append(Id);
					sb.Append("\">");
					sb.Append(Environment.NewLine);
					sb.Append(GetIndentStringFromNumber(indent+2));
					sb.Append("<option value=\"\" disabled>");
					sb.Append(Name);
					sb.Append("</option>");
					sb.Append(Environment.NewLine);
					for (var i = 0; i < Options.Count; i++) {
						var option = Options[i];
						sb.Append(GetIndentStringFromNumber(indent+2));
						sb.Append("<option value=\"");
						sb.Append(option);
						sb.Append(i == Selection ? "\" selected>" : "\">");
						sb.Append(option);
						sb.Append("</option>");
						sb.Append(Environment.NewLine);
					}
					sb.Append(GetIndentStringFromNumber(indent+1));
					sb.Append("</select>");
					sb.Append(Environment.NewLine);
					/*sb.Append("<button class=\"btn btn-primary dropdown-toggle btn-block\" type=\"button\" id=\"");
					sb.Append(Id);
					sb.Append(".name\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">");
					sb.Append(Name);
					sb.Append("</button>");
					sb.Append(Environment.NewLine);
					//Open the option list div
					sb.Append(GetIndentStringFromNumber(indent+1));
					sb.Append("<div class=\"dropdown-menu\" aria-labelledby=\"");
					sb.Append(Id);
					sb.Append(".name\">");
					sb.Append(Environment.NewLine);
					//Add the option items
					foreach (var option in Options) {
						sb.Append(GetIndentStringFromNumber(indent+2));
						sb.Append("<button class=\"dropdown-item\" type=\"button\">");
						sb.Append(option);
						sb.Append("</button>");
						sb.Append(Environment.NewLine);
					}
					//Close the option list div
					sb.Append(GetIndentStringFromNumber(indent+1));
					sb.Append("</div>");
					sb.Append(Environment.NewLine);*/
					break;
				case ESelectListType.RadioList:
					//Add the first horizontal line
					sb.Append(GetIndentStringFromNumber(indent+1));
					sb.Append("<hr/>");
					sb.Append(Environment.NewLine);
					//Add the title
					sb.Append(GetIndentStringFromNumber(indent+1));
					sb.Append("<div id=\"");
					sb.Append(Id);
					sb.Append(".title\" class=\"jui-title\">");
					sb.Append("<small>");
					sb.Append(Name);
					sb.Append("</small>");
					sb.Append("</div>");
					sb.Append(Environment.NewLine);
					//Add the option items
					var optionCount = 0;
					foreach (var option in Options) {
						sb.Append(GetIndentStringFromNumber(indent+2));
						sb.Append("<div class=\"jui-toggle jui-selectlist-radio-option\">");
						sb.Append(Environment.NewLine);
						sb.Append(GetIndentStringFromNumber(indent+3));
						sb.Append("<label class=\"jui-toggle-text\" for=\"");
						sb.Append(Id);
						sb.Append(".");
						sb.Append(option);
						sb.Append("\">");
						sb.Append(option);
						sb.Append("</label>");
						sb.Append(Environment.NewLine);
						sb.Append(GetIndentStringFromNumber(indent+3));
						sb.Append("<span class=\"form-check jui-toggle-control\"><input type=\"radio\" id=\"");
						sb.Append(Id);
						sb.Append(".");
						sb.Append(option);
						sb.Append("\" class=\"form-check-input jui-input\" name=\"");
						sb.Append(Id);
						sb.Append("\"");
						if (optionCount == Selection) {
							sb.Append(" checked=\"true\"");
						}
						sb.Append("><span class=\"lever\"/><label class=\"form-check-label jui-toggle-checkbox-label\" for=\"");
						sb.Append(Id);
						sb.Append(".");
						sb.Append(option);
						sb.Append("\"/></span>");
						sb.Append(Environment.NewLine);
						sb.Append(GetIndentStringFromNumber(indent+2));
						sb.Append("</div>");
						sb.Append(Environment.NewLine);
						optionCount++;
					}
					//Add the last horizontal line
					sb.Append(GetIndentStringFromNumber(indent+1));
					sb.Append("<hr/>");
					sb.Append(Environment.NewLine);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			//Close the containing div
			sb.Append(GetIndentStringFromNumber(indent));
			sb.Append("</div>");
			sb.Append(Environment.NewLine);

			return sb.ToString();
		}
		
	}

}