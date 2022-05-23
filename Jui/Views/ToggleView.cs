using System;
using System.Text;
using HomeSeer.Jui.Types;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <inheritdoc cref="AbstractView"/>
	/// <summary>
	/// A toggle switch is used to indicate a control that has only two possible operational states.
	/// </summary>
	public sealed class ToggleView : AbstractView {

		/*[JsonProperty("off")]
		public string OffText { get; set; }
		
		[JsonProperty("on")]
		public string OnText { get; set; }*/
		
		/// <summary>
		/// The current state of the toggle switch
		/// </summary>
		[JsonProperty("enabled")]
		public bool IsEnabled { get; set; }
		
		/// <summary>
		/// The style of toggle
		/// </summary>
		[JsonProperty("toggle_style")]
		public EToggleType ToggleType { get; set; }

		/// <inheritdoc cref="AbstractView"/>
		/// <summary>
		/// Create a new instance of a toggle with an ID and Name
		/// </summary>
		/// <param name="id">The unique ID for the view</param>
		/// <param name="name">The name of the view</param>
		/// <param name="isEnabled">The state of the toggle. DEFAULT: false</param>
		[JsonConstructor]
		public ToggleView(string id, string name, bool isEnabled = false) : base(id, name) {
			Type = EViewType.Toggle;
			IsEnabled = isEnabled;
			ToggleType = EToggleType.Switch;
		}

		/// <inheritdoc cref="AbstractView.Update"/>
		/// <summary>
		/// Update the view to the new state.  This will change the enabled state
		/// </summary>
		/// <exception cref="ViewTypeMismatchException">Thrown when the new view's class doesn't match the calling view</exception>
		public override void Update(AbstractView newViewState) {
			base.Update(newViewState);
			
			if (!(newViewState is ToggleView updatedToggleView)) {
				throw new ViewTypeMismatchException("The original view type does not match the type in the update");
			}

			IsEnabled = updatedToggleView.IsEnabled;
		}

		/// <inheritdoc cref="AbstractView.UpdateValue"/>
		/// <exception cref="FormatException">Thrown when the value is not in the correct format</exception>
		public override void UpdateValue(string value) {

			try {
				IsEnabled = bool.Parse(value);
			}
			catch (Exception exception) {
				Console.WriteLine(exception);
				throw new FormatException("Value is not in the correct format", exception);
			}
		}

		/// <inheritdoc cref="AbstractView.GetStringValue"/>
		public override string GetStringValue() {
			return IsEnabled.ToString();
		}

		/// <inheritdoc cref="AbstractView.ToHtml"/>
		public override string ToHtml(int indent = 0) {
			
			var sb = new StringBuilder();
			sb.Append(GetIndentStringFromNumber(indent));
			//Open the containing div
			sb.Append($"<div id=\"{Id}-par\" class=\"jui-view jui-toggle\">");
			sb.Append(Environment.NewLine);
			//Add the text
			sb.Append(GetIndentStringFromNumber(indent+1));
			sb.Append($"<label class=\"jui-toggle-text\" for=\"{Id}\">{Name}</label>");
			sb.Append(Environment.NewLine);
			//Add the toggle
			sb.Append(GetIndentStringFromNumber(indent+1));
			switch (ToggleType) {
				case EToggleType.Switch:
					sb.Append("<span class=\"switch jui-toggle-control\"><label>");
					sb.Append($"<input type=\"checkbox\" class=\"jui-input\" id=\"{Id}\" {(IsEnabled ? "checked" : "")}>");
					sb.Append("<span class=\"lever\"/></label></span>");
					break;
				case EToggleType.Checkbox:
					sb.Append("<span class=\"form-check jui-toggle-control\">");
					sb.Append("<span class=\"form-check form-check-inline jui-toggle-checkbox\">");
					sb.Append($"<input type=\"checkbox\" class=\"form-check-input jui-input\" id=\"{Id}\" {(IsEnabled ? "checked" : "")}>");
					sb.Append($"<label class=\"form-check-label jui-toggle-checkbox-label\" for=\"{Id}\"></label></span></span>");
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