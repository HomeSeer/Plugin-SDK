using HomeSeer.Jui.Types;

namespace HomeSeer.Jui.Forms {

	/// <inheritdoc />
	/// <summary>
	/// A form used for communicating information, like instructions, to the user.
	/// <para>This form only supports the use of label and image views.</para>
	/// </summary>
	internal sealed class InfoFormView : AbstractFormView {

		/// <inheritdoc />
		/// <summary>
		/// Create a new instance of an info form with an ID and Name
		/// </summary>
		/// <param name="id">The unique ID of the form</param>
		/// <param name="name">The title of the form</param>
		public InfoFormView(string id, string name) : base(id, name) {
			FormType = EFormType.Info;
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