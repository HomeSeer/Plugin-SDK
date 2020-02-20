using System;
using System.Text;
using HomeSeer.Jui.Types;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <inheritdoc cref="AbstractView"/>
	/// <summary>
	/// A button is used to execute an action with the plugin.
	/// A plugin will then instruct the client on how to proceed by either responding
	/// with a message or a form to display to the user.  
	/// </summary>
	internal sealed class ButtonView : AbstractView {

		/// <summary>
		/// The ID that will be sent to the API when this button is clicked
		/// </summary>
		[JsonProperty("action_id")]
		public string ActionId { get; set; }

		/*[JsonProperty("params")]
		public List<string> Parameters { get; set; }*/

		/// <inheritdoc cref="AbstractView"/>
		/// <summary>
		/// Create an instance of a ButtonView with an ID, Name, and ActionID
		/// </summary>
		/// <param name="id">The unique ID for the View</param>
		/// <param name="name">The name of the View</param>
		/// <param name="actionId">The ID of the action sent when this button is clicked</param>
		/// <exception cref="ArgumentNullException">Thrown if a view is created with an invalid Name or ActionId</exception>
		[JsonConstructor]
		public ButtonView(string id, string name, string actionId) : base(id, name) {
			
			if (string.IsNullOrWhiteSpace(name)) {
				throw new ArgumentNullException(nameof(name));
			}
			
			if (string.IsNullOrWhiteSpace(actionId)) {
				throw new ArgumentNullException(nameof(actionId));
			}
			
			Type = EViewType.Button;
			ActionId = actionId;
		}

		/// <inheritdoc cref="AbstractView.GetStringValue"/>
		public override string GetStringValue() {
			return null;
		}

		/// <inheritdoc cref="AbstractView.ToHtml"/>
		public override string ToHtml(int indent = 0) {
			//TODO attach to button click event
			var sb = new StringBuilder();
			sb.Append(GetIndentStringFromNumber(indent));
			sb.Append("<button class=\"btn btn-primary btn-block\" type=\"button\" style=\"margin-top: 16px;\" id=\"");
			sb.Append(Id);
			sb.Append("\">");
			sb.Append(Name);
			sb.Append("</button>");
			sb.Append(Environment.NewLine);

			return sb.ToString();
		}

	}

}