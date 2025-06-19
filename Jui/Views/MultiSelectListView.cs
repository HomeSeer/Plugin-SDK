using HomeSeer.Jui.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <inheritdoc cref="AbstractView"/>
	/// <summary>
	/// A selection list allows a user to pick multiple values from a predefined collection.
	/// </summary>
	public sealed class MultiSelectListView : AbstractView {

		/// <summary>
		/// A list of options that the user can select from
		/// </summary>
		[JsonProperty("options")]
		public List<string> Options { get; private set; }
		
		/// <summary>
		/// A list of keys that corresponds to the <see cref="Options"/> list.
		/// </summary>
		[JsonProperty("keys")]
		public List<string> OptionKeys { get; private set; }

		/// <summary>
		/// The display style for the list of options
		/// </summary>
		[JsonProperty("style")]
		public EMultiSelectListType Style { get; set; }

        /// <summary>
        /// The indexes of the currently selected options in the list. This is the value for this view.
        /// </summary>
        /// <remarks>
        /// Set this to null or an emptyl List to display a default "Select an option" text
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the selection is set to an index that does not work for the current <see cref="Options"/></exception>
        [JsonProperty("selection")]
        public List<int> Selection { 
            get => _selection;
            set {
                if (value != null) {
                    for (var i = 0; i < value.Count; i++) {
                        if (value[i] < 0 || value[i] >= Options.Count) {
                            throw new ArgumentOutOfRangeException(nameof(value));
                        }
                    }
                }
                _selection = value;
            }
        }

        private List<int> _selection;

        /// <summary>
        /// The text displayed when <see cref="Selection"/> equals null or an emptyl List. If this property is null or empty, "Select options" is displayed. 
        /// </summary>
        [JsonProperty("default_selection_text")]
		public string DefaultSelectionText { get; set; }

        /// <inheritdoc cref="AbstractView"/>
        /// <summary>
        /// Create a new instance of a multi select list with the default, drop down style, an ID, Name, and the specified list of options and keys
        /// </summary>
        /// <param name="id">The unique ID for this View</param>
        /// <param name="name">The name of the view</param>
        /// <param name="options">The list of options</param>
        /// <param name="optionKeys">The list of keys corresponding to the list of options</param>
        /// <param name="style">The display style of the select list. DEFAULT: drop down</param>
        /// <param name="selection">The list of indexes of the currently selected options in the list. DEFAULT: null</param>
        /// <exception cref="ArgumentNullException">Thrown if multi select list is create with an invalid list of options or option keys</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if multi select list is created with invalid indexes for the currently selected options</exception>
        [JsonConstructor]
        public MultiSelectListView(string id, string name, List<string> options, List<string> optionKeys, EMultiSelectListType style = EMultiSelectListType.DropDown, List<int> selection = null) : base(id, name) {

	        if (options == null || options.Count == 0) {
		        throw new ArgumentNullException(nameof(options));
	        }
	        if (optionKeys == null || optionKeys.Count == 0) {
		        throw new ArgumentNullException(nameof(optionKeys));
	        }
	        if (options.Count != optionKeys.Count) {
		        throw new ArgumentException("Options and Option Keys must match in size.");
	        }
	        if (selection != null){
                for (var i = 0; i < selection.Count; i++) {
                    if (selection[i] < 0 || selection[i] >= options.Count) {
                        throw new ArgumentOutOfRangeException(nameof(selection));
                    }
                }
	        }
			
	        Type       = EViewType.MultiSelectList;
	        Options    = options;
	        OptionKeys = optionKeys;
	        Style      = style;
	        Selection  = selection;
        }

        /// <inheritdoc cref="AbstractView"/>
        /// <summary>
        /// Create a new instance of a multi select list with the default, drop down style, an ID, Name, and the specified list of options and keys
        /// </summary>
        /// <param name="id">The unique ID for this View</param>
        /// <param name="name">The name of the view</param>
        /// <param name="options">The list of options</param>
        /// <param name="optionKeys">The list of keys corresponding to the list of options</param>
        /// <param name="style">The display style of the select list. DEFAULT: drop down</param>
        /// <param name="selectedKeys">The list of selected option keys</param>
        /// <exception cref="ArgumentNullException">Thrown if multi select list is create with an invalid list of options or option keys</exception>
        public MultiSelectListView(string id, string name, List<string> options, List<string> optionKeys, EMultiSelectListType style = EMultiSelectListType.DropDown, List<string> selectedKeys = null) : base(id, name) {

            if (options == null || options.Count == 0) {
                throw new ArgumentNullException(nameof(options));
            }
            if (optionKeys == null || optionKeys.Count == 0) {
                throw new ArgumentNullException(nameof(optionKeys));
            }
            if (options.Count != optionKeys.Count) {
                throw new ArgumentException("Options and Option Keys must match in size.");
            }

            var idxList = new List<int>();

            if (selectedKeys != null) {
                foreach(var key in selectedKeys) {
                    var idx = optionKeys.FindIndex(x => x == key);
                    if(idx > -1) {
                        idxList.Add(idx);
                    }
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
        /// Update the view to the new state.  This will change the selected options 
        /// </summary>
        /// <exception cref="ViewTypeMismatchException">Thrown when the new view's class doesn't match the calling view</exception>
        public override void Update(AbstractView newViewState) {
			base.Update(newViewState);
			
			if (!(newViewState is MultiSelectListView updatedSelectListView)) {
				throw new ViewTypeMismatchException("The original view type does not match the type in the update");
			}

			Selection = updatedSelectListView.Selection;
		}

        /// <inheritdoc cref="AbstractView.UpdateValue"/>
        public override void UpdateValue(string value) {

            var aList1 = value.Split(',');
            var aList2 = new List<int>();

            foreach (var key in aList1) {
                var idx = OptionKeys.FindIndex(x => x == key);
                if (idx > -1) {
                    aList2.Add(idx);
                }
            }
            Selection = aList2;
        }

        /// <inheritdoc cref="AbstractView.GetStringValue"/>
        /// <remarks>
        /// Returns a comma separated list of selected option keys
        /// </remarks>
        public override string GetStringValue() {
            string value = "";
            List<string> keys = GetSelectedOptionKeys();

            if(keys != null) {
                value = string.Join(",", keys);
            }
            return value;
        }

        /// <summary>
        /// Get the currently selected options.
        /// </summary>
        /// <returns>The list of the options at the indexes specified by <see cref="Selection"/>.</returns>
        public List<string> GetSelectedOptions() {
			var selectedOptions = new List<string>();

            if (Selection != null) {
                foreach (var idx in Selection) {
                    if (idx >= 0 && idx < Options.Count) {
                        selectedOptions.Add(Options[idx]);
                    }
                }
            }
            return selectedOptions;
		}

		/// <summary>
		/// Get the currently selected option keys
		/// </summary>
		/// <returns>The list of keys of the option at the indexes specified by <see cref="Selection"/>.</returns>
		public List<string> GetSelectedOptionKeys() {
            var keys = new List<string>();

            if (Selection != null) {
                foreach (var idx in Selection) {
                    if (idx >= 0 && idx < OptionKeys.Count) {
                        keys.Add(OptionKeys[idx]);
                    }
                }
            }
            return keys;
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
				case EMultiSelectListType.DropDown:
                case EMultiSelectListType.SearchableDropDown:
                    string originalValue = GetStringValue();
                    
                    //Add the title
                    sb.Append($"<label class=\"jui-select-label\">{Name}</label>");
					sb.Append(Environment.NewLine);
					//Add the button
					sb.Append(GetIndentStringFromNumber(indent+1));
					sb.Append($"<select class=\"mdb-select md-form jui-input jui-select\" multiple id=\"{Id}\" jui-orig-val=\"{originalValue}\" {(Style == EMultiSelectListType.SearchableDropDown ? "searchable=\"Search...\"" : "")}>");
					sb.Append(Environment.NewLine);
					sb.Append(GetIndentStringFromNumber(indent+2));
					sb.Append($"<option value=\"\" disabled>");
					sb.Append($"{(string.IsNullOrEmpty(DefaultSelectionText) ? "Select options" : DefaultSelectionText)}");
					sb.Append("</option>");
					sb.Append(Environment.NewLine);
					for (var i = 0; i < Options.Count; i++) {
						var option = Options[i];
                        string optionValue = OptionKeys[i];
                        
						sb.Append(GetIndentStringFromNumber(indent+2));
						sb.Append($"<option value=\"{optionValue}\"{((Selection != null && Selection.Contains(i)) ? " selected" : "")}>{option}</option>");
						sb.Append(Environment.NewLine);
					}
					sb.Append(GetIndentStringFromNumber(indent+1));
					sb.Append("</select>");
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