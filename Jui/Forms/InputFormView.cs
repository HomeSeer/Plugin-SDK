using System;
using HomeSeer.Jui.Types;

namespace HomeSeer.Jui.Forms {

	/// <inheritdoc />
	/// <summary>
	/// A form used for collecting input from a user.
	/// <para>
	/// This form supports all view types except for buttons and view groups.
	/// </para>
	/// </summary>
	internal sealed class InputFormView : AbstractFormView {

		/// <inheritdoc />
		/// <summary>
		/// Create a new instance of an input form with an ID and Name
		/// </summary>
		/// <param name="id">The unique ID of the form</param>
		/// <param name="name">The title of the form</param>
		/// <param name="positiveActionId">The ID that will be sent to the API when the continue button is clicked</param>
		public InputFormView(string id, string name, string positiveActionId) : base(id, name) {
			if (string.IsNullOrWhiteSpace(positiveActionId)) {
				throw new ArgumentNullException(nameof(positiveActionId), "Input forms must specify a positive action ID");
			}
			
			FormType = EFormType.Input;
			PositiveActionId = positiveActionId;
		}
		
		/// <inheritdoc/>
		internal override string ToHtml(int indent = 0) {
			throw new System.NotImplementedException();
		}
		
		/// <inheritdoc/>
		public override string GetStringValue() {
			return null;
		}

	}

}