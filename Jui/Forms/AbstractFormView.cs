using System;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Forms {

	/// <inheritdoc />
	/// <summary>
	/// The base implementation for a JUI form
	/// </summary>
	internal abstract class AbstractFormView : ViewGroup {

		/// <summary>
		/// The form's type and style.  See <see cref="EFormType"/> for more information about each type
		/// </summary>
		[JsonProperty("form_type")]
		public EFormType FormType { get; set; }
		
		/// <summary>
		/// The ID that will be sent to the API when the continue button is clicked.
		///<para>
		/// On an input form, all of the views on the form will be sent as id-value pairs.
		/// </para>
		/// </summary>
		[JsonProperty("positive_id")]
		public string PositiveActionId { get; set; }
		
		/// <summary>
		/// The ID that will be sent to the API when the cancel/back button is clicked
		/// </summary>
		[JsonProperty("negative_id")]
		public string NegativeActionId { get; set; }
		
		/// <summary>
		/// The number of the step in the wizard that this form represents
		/// <para>
		/// This is ignored when the form is presented by a button being clicked.
		/// </para>
		/// </summary>
		[JsonProperty("step_current")]
		public int StepNumber { get; set; }
		
		/// <summary>
		/// The total number of steps in the wizard
		/// <para>
		/// This is ignored when the form is presented by a button being clicked.
		/// </para>
		/// </summary>
		[JsonProperty("step_total")]
		public int TotalSteps { get; set; }

		/// <inheritdoc />
		/// <summary>
		/// Create a new instance of a form with an ID
		/// </summary>
		/// <param name="id">The unique ID for the form</param>
		/// <param name="name">The title of the form</param>
		/// <exception cref="ArgumentNullException">Thrown if a view is created with an invalid Name</exception>
		protected AbstractFormView(string id, string name) : base(id, name) {
			
			if (string.IsNullOrWhiteSpace(name)) {
				throw new ArgumentNullException(nameof(name));
			}
			
			Type = EViewType.Form;
		}

	}

}