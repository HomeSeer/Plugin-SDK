using System;
using System.Text;
using HomeSeer.Jui.Types;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <inheritdoc cref="AbstractView"/>
	/// <summary>
	/// Labels define static text displayed to the user.
	/// This can either be as a single set of text or as a combination of a name and value to indicate
	/// that a particular property is set to a given value.
	/// </summary>
	public sealed class LabelView : AbstractView {

        /// <summary>
        /// The type of the label
        /// </summary>
        [JsonProperty("label_type")]
        public ELabelType LabelType { get; set; }

        /// <summary>
        /// The value displayed; leave blank to just show the name
        /// </summary>
        [JsonProperty("value")]
		public string Value { get; set; }

        /// <inheritdoc cref="AbstractView"/>
		/// <summary>
		/// Create a new instance of a Label with an ID and text value
		/// </summary>
		/// <param name="id">The unique ID for the View</param>
		/// <param name="name">The name of the View; the text displayed by the label</param>
		[JsonConstructor]
		public LabelView(string id, string name) : base(id, name ?? "") {
			
			Type = EViewType.Label;
            LabelType = ELabelType.Default;
        }

        /// <inheritdoc cref="AbstractView"/>
		/// <summary>
		/// Create a new instance of a Label with an ID, Name, and text value
		/// </summary>
		/// <param name="id">The unique ID for the View</param>
		/// <param name="name">The name of the View; the title of the label</param>
		/// <param name="value">The text displayed by the label</param>
		/// <exception cref="ArgumentNullException">Thrown if both the name and value are empty</exception>
		public LabelView(string id, string name, string value) : base(id, name) {

			if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(value)) {
				throw new ArgumentNullException(nameof(value), "Both the name and value cannot be empty.");				
			}
			
			Type = EViewType.Label;
			Value = value;
            LabelType = ELabelType.Default;
		}

		/// <inheritdoc cref="AbstractView.GetStringValue"/>
		public override string GetStringValue() {
			return null;
		}

		/// <inheritdoc cref="AbstractView.ToHtml"/>
		public override string ToHtml(int indent = 0) {
			
			var sb = new StringBuilder();
			sb.Append(GetIndentStringFromNumber(indent));
			//Open the containing div
			sb.Append($"<div id=\"{Id}\" class=\"jui-view\">");
			sb.Append(Environment.NewLine);
			//Add the title
			sb.Append(GetIndentStringFromNumber(indent+1));
			sb.Append($"<div id=\"{Id}-title\" class=\"jui-title\"><small>{Name}</small></div>");
			sb.Append(Environment.NewLine);
			//Add the label text
			sb.Append(GetIndentStringFromNumber(indent+1));
            switch (LabelType) {
                case ELabelType.Default:
                    sb.Append($"<p id=\"{Id}-value\">{Value}</p> ");
                    break;
                case ELabelType.Preformatted:
                    sb.Append($"<pre id=\"{Id}-value\">{Value}</pre> ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            sb.Append(Environment.NewLine);
			//Close the containing div
			sb.Append(GetIndentStringFromNumber(indent));
			sb.Append("</div>");
			sb.Append(Environment.NewLine);

			return sb.ToString();
		}

	}

}