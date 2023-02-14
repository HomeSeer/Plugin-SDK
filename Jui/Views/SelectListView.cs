using HomeSeer.Jui.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <inheritdoc cref="AbstractView"/>
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
		/// A list of keys that corresponds to the <see cref="Options"/> list.
		/// <para>
		/// For internal use by a plugin.  This is not displayed or used in any fashion otherwise.
		/// </para>
		/// </summary>
		[JsonProperty("keys")]
		public List<string> OptionKeys { get; private set; }

		/// <summary>
		/// The display style for the list of options
		/// </summary>
		[JsonProperty("style")]
		public ESelectListType Style { get; set; }

        /// <summary>
        /// The index of the currently selected option in the list. This is the value for this view.
        /// </summary>
        /// <remarks>
        /// Set this to -1 to display a default "Select an option" text
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the selection is set to an index that does not work for the current <see cref="Options"/></exception>
        [JsonProperty("selection")]
        public int Selection { 
            get => _selection;
            set {
                if (value < -1 || value >= Options?.Count ){
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _selection = value;
            }
        }

        private int _selection = -1;

		/// <summary>
		/// The text displayed when <see cref="Selection"/> equals -1. If this property is null or empty, "Select an option" is displayed. 
		/// </summary>
        /// <remarks>This property is not used when <see cref="Style"/> is <see cref="ESelectListType.RadioList"/></remarks>
		[JsonProperty("default_selection_text")]
		public string DefaultSelectionText { get; set; }

        /// <summary>
        /// When this property is true, <see cref="GetStringValue"/>  returns the selected option key instead of the selected option index,
        /// and <see cref="UpdateValue"/> expects an option key as parameter instead of an option index.
        /// </summary>
        [JsonProperty("use_option_key_as_selection_value")]
        public bool UseOptionKeyAsSelectionValue { get; set; } = false;

		/// <inheritdoc cref="AbstractView"/>
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
		public SelectListView(string id, string name, List<string> options, ESelectListType style = ESelectListType.DropDown, int selection = -1) : base(id, name) {

			if (options == null || options.Count == 0) {
				throw new ArgumentNullException(nameof(options));
			}
            if (selection < -1 || selection >= options.Count ){
                throw new ArgumentOutOfRangeException(nameof(selection));
            }
			
			Type = EViewType.SelectList;
			Options = options;
			OptionKeys = null;
			Style = style;
			Selection = selection;
		}

		/// <inheritdoc cref="AbstractView"/>
		/// <summary>
		/// Create a new instance of a select list with the default, drop down style, an ID, Name, and the specified list of options and keys
		/// </summary>
		/// <param name="id">The unique ID for this View</param>
		/// <param name="name">The name of the view</param>
		/// <param name="options">The list of options</param>
		/// <param name="style">The display style of the select list. DEFAULT: drop down</param>
		/// <param name="selection">The index of the currently selected option in the list. DEFAULT: 0</param>
		/// <param name="optionKeys">The list of keys corresponding to the list of options</param>
		/// <exception cref="ArgumentNullException">Thrown if select list is create with an invalid list of options</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if select list is created with an invalid index for the currently selected option</exception>
		public SelectListView(string id, string name, List<string> options, List<string> optionKeys, ESelectListType style = ESelectListType.DropDown, int selection = -1) : base(id, name) {

	        if (options == null || options.Count == 0) {
		        throw new ArgumentNullException(nameof(options));
	        }
	        if (optionKeys == null || optionKeys.Count == 0) {
		        throw new ArgumentNullException(nameof(optionKeys));
	        }
	        if (options.Count != optionKeys.Count) {
		        throw new ArgumentException("Options and Option Keys must match in size.");
	        }
	        if (selection < -1 || selection >= options.Count ){
		        throw new ArgumentOutOfRangeException(nameof(selection));
	        }
			
	        Type       = EViewType.SelectList;
	        Options    = options;
	        OptionKeys = optionKeys;
	        Style      = style;
	        Selection  = selection;
        }

		/// <inheritdoc cref="AbstractView.Update"/>
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

        /// <inheritdoc cref="AbstractView.UpdateValue"/>
        /// <exception cref="FormatException">Thrown when the value is not in the correct format</exception>
        public override void UpdateValue(string value) {

            if (UseOptionKeyAsSelectionValue) {
                if (OptionKeys != null) {
                    Selection = OptionKeys.FindIndex(x => x == value);
                }
                else {
                    Selection = -1;
                }
            }
            else {
                try {
                    Selection = int.Parse(value);
                }
                catch (Exception exception) {
                    Console.WriteLine(exception);
                    throw new FormatException("Value is not in the correct format", exception);
                }
            }
        }

        /// <inheritdoc cref="AbstractView.GetStringValue"/>
        /// <remarks>
        /// Returns the selected option key if <see cref="UseOptionKeyAsSelectionValue"/> is true, else returns the selected index as a string
        /// </remarks>
        public override string GetStringValue() {
            if (UseOptionKeyAsSelectionValue) {
                return GetSelectedOptionKey();
            }
            else {
                return Selection.ToString();
            }
        }

        /// <summary>
        /// Get the currently selected option text.
        /// </summary>
        /// <returns>The text of the option at the index specified by <see cref="Selection"/>.</returns>
        public string GetSelectedOption() {
			if (Options == null || Selection >= Options.Count || Selection == -1) {
				return string.Empty;
			}
			
			return Options[Selection];
		}

		/// <summary>
		/// Get the currently selected option key
		/// </summary>
		/// <returns>The key of the option at the index specified by <see cref="Selection"/>.</returns>
		public string GetSelectedOptionKey() {
			if (Options == null || Selection >= Options.Count || Selection == -1) {
				return string.Empty;
			}
			
			if (OptionKeys == null || Selection >= OptionKeys.Count || Selection == -1) {
				return string.Empty;
			}

			return OptionKeys[Selection];
		}

		/// <inheritdoc cref="AbstractView.ToHtml"/>
		public override string ToHtml(int indent = 0) {
			
			var sb = new StringBuilder();
			sb.Append(GetIndentStringFromNumber(indent));
			//Open the containing div
			sb.Append($"<div id=\"{Id}-par\" class=\"jui-view\">");
			sb.Append(Environment.NewLine);
			//Add the select list
			switch (Style) {
				case ESelectListType.DropDown:
                case ESelectListType.SearchableDropDown:
                    string originalValue;
                    if (UseOptionKeyAsSelectionValue) {
                        if (OptionKeys != null && Selection >= 0 && Selection < OptionKeys.Count) {
                            originalValue = OptionKeys[Selection];
                        }
                        else {
                            originalValue = "";
                        }
                    }
                    else {
                        originalValue = Selection.ToString();
                    }
                    //Add the title
                    sb.Append($"<label class=\"jui-select-label\">{Name}</label>");
					sb.Append(Environment.NewLine);
					//Add the button
					sb.Append(GetIndentStringFromNumber(indent+1));
					sb.Append($"<select class=\"mdb-select md-form jui-input jui-select\" id=\"{Id}\" jui-orig-val=\"{originalValue}\" {(Style== ESelectListType.SearchableDropDown ? "searchable=\"Search...\"" : "")}>");
					sb.Append(Environment.NewLine);
					sb.Append(GetIndentStringFromNumber(indent+2));
					sb.Append($"<option value=\"\" disabled {(Selection == -1 ? "selected" : "")}>");
					sb.Append($"{(string.IsNullOrEmpty(DefaultSelectionText) ? "Select an option" : DefaultSelectionText)}");
					sb.Append("</option>");
					sb.Append(Environment.NewLine);
					for (var i = 0; i < Options.Count; i++) {
						var option = Options[i];
                        string optionValue = i.ToString();
                        if (UseOptionKeyAsSelectionValue && OptionKeys != null && i < OptionKeys.Count) {
                            optionValue = OptionKeys[i];
                        }
						sb.Append(GetIndentStringFromNumber(indent+2));
						sb.Append($"<option value=\"{optionValue}\"{(i == Selection ? " selected" : "")}>{option}</option>");
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
					sb.Append($"<div id=\"{Id}-title\" class=\"jui-title\"><small>{Name}</small></div>");
					sb.Append(Environment.NewLine);
					//Add the option items
					for (var optionNum = 0; optionNum < Options.Count; optionNum++) {
						var option = Options[optionNum];
						var optionId = $"{Id}-{optionNum}";
                        string optionValue = optionNum.ToString();
                        if (UseOptionKeyAsSelectionValue && OptionKeys != null && optionNum < OptionKeys.Count) {
                            optionValue = OptionKeys[optionNum];
                        }
                        sb.Append(GetIndentStringFromNumber(indent+2));
						sb.Append("<div class=\"jui-toggle jui-selectlist-radio-option\">");
						sb.Append(Environment.NewLine);
						sb.Append(GetIndentStringFromNumber(indent+3));
						sb.Append($"<label class=\"jui-toggle-text\" for=\"{optionId}\">{option}</label>");
						sb.Append(Environment.NewLine);
						sb.Append(GetIndentStringFromNumber(indent+3));
						sb.Append("<span class=\"form-check jui-toggle-control\">");
						sb.Append($"<input type=\"radio\" id=\"{optionId}\" par-id=\"{Id}-par\" class=\"form-check-input jui-input\" name=\"{Id}\" {(optionNum == Selection ? "checked" : "")} value=\"{optionValue}\">");
						sb.Append($"<label class=\"form-check-label jui-toggle-checkbox-label\" for=\"{optionId}\"/></span>");
						sb.Append(Environment.NewLine);
						sb.Append(GetIndentStringFromNumber(indent+2));
						sb.Append("</div>");
						sb.Append(Environment.NewLine);
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