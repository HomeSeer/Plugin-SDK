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
	public sealed class MultiSelectListView : AbstractView {

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
        public List<int> Selection { 
            get => _selection;
            set {
                //Console.WriteLine("MultiSelectListView Selection set " + Id + " (" + Name + ") - count: " + value.Count + " Indexes: " + String.Join(",", value));
                for (var i = 0; i < value.Count; i++) {
                    if (value[i] < -1 || value[i] >= Options.Count) {
                        throw new ArgumentOutOfRangeException(nameof(value));
                    }
                }
                _selection = value;
            }
        }

        private List<int> _selection;

		/// <summary>
		/// The text displayed when <see cref="Selection"/> equals -1. If this property is null or empty, "Select an option" is displayed. 
		/// </summary>
        /// <remarks>This property is not used when <see cref="Style"/> is <see cref="ESelectListType.RadioList"/></remarks>
		[JsonProperty("default_selection_text")]
		public string DefaultSelectionText { get; set; }

        /// <summary>
        /// When this property is true, <see cref="GetStringValue"/>  returns the selected option keys as a comma separated list 
        /// instead of the selected option index as a comma separated list, and <see cref="UpdateValue"/> expects option keys instead of a indexes.
        /// </summary>
        [JsonProperty("use_option_key_as_selection_value")]
        public bool UseOptionKeyAsSelectionValue { get; set; } = false;

        /// <inheritdoc cref="AbstractView"/>
        /// <summary>
        /// Create a new instance of a multi select list with the default, drop down style, an ID, Name, and the specified list of options
        /// </summary>
        /// <param name="id">The unique ID for this View</param>
        /// <param name="name">The name of the view</param>
        /// <param name="options">The list of options</param>
        /// <param name="style">The display style of the select list. DEFAULT: drop down</param>
        /// <param name="selection">The list of indices of selected options in the list. DEFAULT: 0</param>
        /// <exception cref="ArgumentNullException">Thrown if select list is create with an invalid list of options</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if select list is created with an invalid index for the currently selected option</exception>
        [JsonConstructor]
        public MultiSelectListView(string id, string name, List<string> options, ESelectListType style = ESelectListType.DropDown, List<int> selection = null) : base(id, name) {

            if (options == null || options.Count == 0) {
                throw new ArgumentNullException(nameof(options));
            }

            Type = EViewType.MultiSelectList;
            Options = options;
			OptionKeys = null;
            Style = style;
			Selection = selection == null ? new List<int>() : selection;
        }

        /// <inheritdoc cref="AbstractView"/>
        /// <summary>
        /// Create a new instance of a multi select list with the default, drop down style, an ID, Name, and the specified list of options and keys
        /// </summary>
        /// <param name="id">The unique ID for this View</param>
        /// <param name="name">The name of the view</param>
        /// <param name="options">The list of options</param>
        /// <param name="style">The display style of the select list. DEFAULT: drop down</param>
        /// <param name="selection">The index of the currently selected option in the list. DEFAULT: 0</param>
        /// <param name="optionKeys">The list of keys corresponding to the list of options</param>
        /// <exception cref="ArgumentNullException">Thrown if select list is create with an invalid list of options</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if select list is created with an invalid index for the currently selected option</exception>
        public MultiSelectListView(string id, string name, List<string> options, List<string> optionKeys, ESelectListType style = ESelectListType.DropDown, List<int> selection = null) : base(id, name) {

            if (options == null || options.Count == 0) {
		        throw new ArgumentNullException(nameof(options));
	        }
	        if (optionKeys == null || optionKeys.Count == 0) {
		        throw new ArgumentNullException(nameof(optionKeys));
	        }
	        if (options.Count != optionKeys.Count) {
		        throw new ArgumentException("Options and Option Keys must match in size.");
	        };
			
	        Type       = EViewType.MultiSelectList;
            Options    = options;
	        OptionKeys = optionKeys;
	        Style      = style;
	        Selection  = selection == null ? new List<int>() : selection;
        }

        /// <summary>
        /// Create a new instance of a select list with the default, drop down style, an ID, Name, and the specified list of options and keys
        /// </summary>
        /// <param name="id">The unique ID for this View</param>
        /// <param name="name">The name of the view</param>
        /// <param name="options">The list of options</param>
        /// <param name="optionKeys">The list of keys corresponding to the list of options</param>
        /// <param name="style">The display style of the select list. DEFAULT: drop down</param>
        /// <param name="selectedKeys">List of selected keys</param>
        public MultiSelectListView(string id, string name, List<string> options, List<string> optionKeys, ESelectListType style = ESelectListType.DropDown, List<string> selectedKeys = null) : base(id, name) {

            if (options == null || options.Count == 0) {
                throw new ArgumentNullException(nameof(options));
            }
            if (optionKeys == null || optionKeys.Count == 0) {
                throw new ArgumentNullException(nameof(optionKeys));
            }
            if (options.Count != optionKeys.Count) {
                throw new ArgumentException("Options and Option Keys must match in size.");
            };

            var idxList = new List<int>();

            if (optionKeys != null && selectedKeys != null) {
                for (var i = 0; i < selectedKeys.Count; i++) {
                    var key = selectedKeys[i];
                    var idx = optionKeys.FindIndex(x => x == key);
                    idxList.Add(idx);
                }
            }

            Type = EViewType.MultiSelectList;
            Options = options;
            OptionKeys = optionKeys;
            Style = style;
            Selection = idxList;
        }

        /// <inheritdoc cref="AbstractView.Update"/>
        /// <summary>
        /// Update the view to the new state.  This will change the selected option 
        /// </summary>
        /// <exception cref="ViewTypeMismatchException">Thrown when the new view's class doesn't match the calling view</exception>
        public override void Update(AbstractView newViewState) {

            //Console.WriteLine("MultiSelectListView Update start " + Id + " (" + Name + ") - newViewState: " + newViewState);

            base.Update(newViewState);

            if (!(newViewState is MultiSelectListView updatedSelectListView)) {
				throw new ViewTypeMismatchException("The original view type does not match the type in the update");
			}

            Selection = updatedSelectListView.Selection;

            var indexes = new List<string>();
            if (Options != null && Selection != null) {
                for (var i = 0; i < Selection.Count; i++) {
                    var idx = Selection[i];
                    indexes.Add(idx.ToString());
                }
            }
            //Console.WriteLine("MultiSelectListView Update end " + Id + " (" + Name + ") - Selection count: " + Selection.Count + " Selection: " + String.Join(",", indexes));
        }

        /// <inheritdoc cref="AbstractView.UpdateValue"/>
        /// <exception cref="FormatException">Thrown when the value is not in the correct format</exception>
        public override void UpdateValue(string value) {

            bool tryAsString = false;

            //Console.WriteLine("MultiSelectListView UpdateValue " + Id + " (" + Name + ") - UseOptionKeyAsSelectionValue: " + UseOptionKeyAsSelectionValue + " value: " + value);

            // try as index first based on configuration
            if (! UseOptionKeyAsSelectionValue) {
            
                try {
                    Selection = new List<int>(Array.ConvertAll(value.Split(','), int.Parse));
                    //Console.WriteLine("1. MultiSelectListView UpdateValue " + Id + " (" + Name + ") by key indexes count: " + Selection.Count);
                }
                catch (Exception exception) {
                    Console.WriteLine(exception);
                    Selection = new List<int>();
                    tryAsString = true;
                }
            }

            if (UseOptionKeyAsSelectionValue || tryAsString) {

                if (OptionKeys != null) {
                    var aList1 = value.Split(',');
                    var aList2 = new List<int>();

                    for (var i = 0; i < aList1.Length; i++) {
                        var key = aList1[i];
                        var idx = OptionKeys.FindIndex(x => x == key);
                        aList2.Add(idx);
                    }
                    //Console.WriteLine("2. MultiSelectListView UpdateValue " + Id + " (" + Name + ") by key values. Count " + aList2.Count);
                    Selection = aList2;
                }
                else {
                    var aList1 = value.Split(',');
                    var aList2 = new List<int>();

                    for (var i = 0; i < aList1.Length; i++) {
                        var key = aList1[i];
                        var idx = Options.FindIndex(x => x == key);
                        aList2.Add(idx);
                    }
                    //Console.WriteLine("3. MultiSelectListView UpdateValue " + Id + " (" + Name + ") by values. Count " + aList2.Count);
                    Selection = aList2;
                }
            }
        }

        /// <inheritdoc cref="AbstractView.GetStringValue"/>
        /// <remarks>
        /// Returns a comma separated list of selected option keys if <see cref="UseOptionKeyAsSelectionValue"/> is true, else the comma separated list of key indexes
        /// </remarks>
        public override string GetStringValue() {

            string str;

            if (UseOptionKeyAsSelectionValue) {
                str = GetStringSelectedOptions();
            }
            else {
                str = String.Join(",", GetSelectedIndexes());
            }
            //Console.WriteLine("MultiSelectListView GetStringValue " + Id + " (" + Name + ") - UseOptionKeyAsSelectionValue: " + UseOptionKeyAsSelectionValue + " - Selection count: " + Selection.Count + " - result: " + str);
            return str;
        }

        /// <summary>
        /// Get the currently selected indicies.
        /// </summary>
        /// <returns>The list of the options at the indexes specified by <see cref="Selection"/>.</returns>
        public List<string> GetSelectedIndexes() {

            var indexes = new List<string>();

            if (Options != null && Selection != null) {
                for (var i = 0; i < Selection.Count; i++) {
                    var idx = Selection[i];
                    if (idx < -1 || idx >= Options.Count) {
                        throw new ArgumentOutOfRangeException(nameof(Selection));
                    }
                    if (idx != -1) indexes.Add(idx.ToString());
                }
            }
            //Console.WriteLine("MultiSelectListView GetSelectedIndexes " + Id + " (" + Name + ") " + " - Selection count: " + Selection.Count + " - Indexes count: " + indexes.Count);
            return indexes;
        }

        /// <summary>
        /// Returns a comma separated list of selected options
        /// </summary>
        /// <returns></returns>
        public string GetStringSelectedOptions() {

            string str = String.Join(",", GetSelectedOptionKeys());
            //Console.WriteLine("MultiSelectListView GetStringSelectedOptions " + Id + " (" + Name + ") - Selection count: " + Selection.Count + " - result: " + str);
            return str;
        }

        /// <summary>
        /// Get the currently selected options.
        /// </summary>
        /// <returns>The list of the options at the indexes specified by <see cref="Selection"/>.</returns>
        public List<string> GetSelectedOptions() {

            var options = new List<string>();

            if (Options != null && Selection != null) {
                for (var i = 0; i < Selection.Count; i++) {
                    var idx = Selection[i];
                    if (idx < -1 || idx >= Options.Count) {
                        throw new ArgumentOutOfRangeException(nameof(Selection));
                    }
                    if (idx != -1) options.Add(Options[idx]);
                }
            }
            //Console.WriteLine("MultiSelectListView GetSelectedOptions " + Id + " (" + Name + ") " + " - Selection count: " + Selection.Count + " - options count: " + options.Count);
            return options;
		}

        /// <summary>
        /// Returns a comma separated list of selected option keys
        /// </summary>
        /// <returns></returns>
        public string GetStringSelectedOptionKeys() {

            string str = String.Join(",", GetSelectedOptionKeys());
            //Console.WriteLine("MultiSelectListView GetSelectedOptionKeys " + Id + " (" + Name + ") - Selection count: " + Selection.Count + " - result: " + str);
            return str;
        }

        /// <summary>
        /// Get the currently selected option keys
        /// </summary>
        /// <returns>The list of keys of the option at the indexes specified by <see cref="Selection"/>.</returns>
        public List<string> GetSelectedOptionKeys() {

            var keys = new List<string>();

            if (OptionKeys != null && Selection != null) {
                for (var i = 0; i < Selection.Count; i++) {
                    var idx = Selection[i];
                    if (idx < -1 || idx >= OptionKeys.Count) {
                        throw new ArgumentOutOfRangeException(nameof(Selection));
                    }
                    if (idx != -1) keys.Add(OptionKeys[idx]);
                }
            } else if (Options != null && Selection != null) {
                for (var i = 0; i < Selection.Count; i++) {
                    var idx = Selection[i];
                    if (idx < -1 || idx >= Options.Count) {
                        throw new ArgumentOutOfRangeException(nameof(Selection));
                    }
                    if (idx != -1) keys.Add(Options[idx]);
                }
            }
            //Console.WriteLine("MultiSelectListView GetSelectedOptionKeys " + Id + " (" + Name + ") " + " - Selection count: " + Selection.Count + " - keys count: " + keys.Count);
            return keys;
		}

		/// <inheritdoc cref="AbstractView.ToHtml"/>
		public override string ToHtml(int indent = 0) {

            //Console.WriteLine("MultiSelectListView ToHtml start " + Id + " (" + Name + ") - UseOptionKeyAsSelectionValue: " + UseOptionKeyAsSelectionValue + " - Style: " + Style + " - Selection Count: " + Selection.Count);

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

                    originalValue = GetStringValue();
                    if (string.IsNullOrEmpty(originalValue)) originalValue = "-1";

                    //Add the title
                    sb.Append($"<label class=\"jui-select-label\">{Name}</label>");
					sb.Append(Environment.NewLine);
					//Add the button
					sb.Append(GetIndentStringFromNumber(indent+1));
					sb.Append($"<select class=\"mdb-select md-form jui-input jui-select\" multiple id=\"{Id}\" jui-orig-val=\"{originalValue}\" {(Style== ESelectListType.SearchableDropDown ? "searchable=\"Search...\"" : "")}>");
					sb.Append(Environment.NewLine);
					sb.Append(GetIndentStringFromNumber(indent+2));
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
                        sb.Append($"<option value=\"{optionValue}\"{(Selection.Contains(i) ? " selected" : "")}>{option}</option>");
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
						sb.Append($"<input type=\"radio\" id=\"{optionId}\" par-id=\"{Id}-par\" multiple class=\"form-check-input jui-input\" name=\"{Id}\" {(Selection.Contains(optionNum) ? "checked" : "")} value=\"{optionValue}\">");
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

            //Console.WriteLine("MultiSelectListView ToHtml end " + Id + " (" + Name + ") - UseOptionKeyAsSelectionValue: " + UseOptionKeyAsSelectionValue + " html: " + sb.ToString());
            return sb.ToString();
		}		
	}
}