using HomeSeer.Jui.Types;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Forms {

	/// <inheritdoc />
	/// <summary>
	/// A form used for indicating to the user that a task is running on the HomeSeer system
	/// and they should wait until it completes before performing any other tasks
	/// <para>
	/// This form only supports the use of label and image views
	/// </para>
	/// </summary>
	public class ProgressFormView : AbstractFormView {

		/// <summary>
		/// The number of milliseconds to wait between calls to the API with the positive action id.
		/// The minimum allowed interval is 250ms and the maximum is 30000ms.
		/// </summary>
		[JsonProperty("interval")]
		public int UpdateInterval { get; set; }
		
		/// <summary>
		/// The number of times to call the API with the positive action id before canceling the process.
		/// A value of 0 indicates that the process should never time out.
		/// </summary>
		[JsonProperty("retry")]
		public int RetryCount { get; set; }

		
		/// <inheritdoc />
		/// <summary>
		/// Create a new instance of a progress form with an ID and Name
		/// </summary>
		/// <param name="id">The ID of the form</param>
		/// <param name="name">The title of the form</param>
		public ProgressFormView(string id, string name) : base(id, name) {
			FormType = EFormType.Progress;
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