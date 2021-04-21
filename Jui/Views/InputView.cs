using System;
using System.Text;
using HomeSeer.Jui.Types;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <inheritdoc cref="AbstractView"/>
	/// <summary>
	/// An input view is an editable text box for the user to enter strings, numbers, etc.
	/// </summary>
	public sealed class InputView : AbstractView {

		//TODO error text
		
		/// <summary>
		/// The style of input accepted by this field.
		/// This determines the keyboard the user is shown when on a mobile device.
		/// </summary>
		[JsonProperty("input_type")]
		public EInputType InputType { get; set; }
		
		/// <summary>
		/// The current value of the field
		/// </summary>
		[JsonProperty("value")]
		public string Value { get; set; }

		/// <inheritdoc cref="AbstractView"/>
		/// <summary>
		/// Create a new instance of an InputView with an ID, a Name, and the specified style.
		/// </summary>
		/// <param name="id">The unique ID for the View</param>
		/// <param name="name">The name of the View</param>
		/// <param name="type">The style of the input. DEFAULT: <see cref="EInputType.Text"/></param>
		[JsonConstructor]
		public InputView(string id, string name, EInputType type = EInputType.Text) : base(id, name) {
			
			if (string.IsNullOrWhiteSpace(name)) {
				throw new ArgumentNullException(nameof(name));
			}
			
			Type = EViewType.Input;
			InputType = type;
		}

		/// <inheritdoc cref="AbstractView"/>
		/// <summary>
		/// Create a new instance of an InputView with an ID, a Name, a Value, and the specified style.
		/// </summary>
		/// <param name="id">The unique ID for the View</param>
		/// <param name="name">The name of the View</param>
		/// <param name="value">The value inputted into the field</param>
		/// <param name="type">The style of the input. DEFAULT: <see cref="EInputType.Text"/></param>
		/// <exception cref="InvalidValueForTypeException">Thrown when the value is invalid for the input type</exception>
		public InputView(string id, string name, string value, EInputType type = EInputType.Text) : base(id, name) {
			Type      = EViewType.Input;
			InputType = type;
			if (!IsValueValidForType(value)) {
				throw new InvalidValueForTypeException("The new value is invalid for the input type");
			}
			Value = value;
		}

		/// <inheritdoc cref="AbstractView.Update"/>
		/// <summary>
		/// Update the view to the new state.  This will change the inputted value 
		/// </summary>
		/// <exception cref="ViewTypeMismatchException">Thrown when the new view's class doesn't match the calling view</exception>
		/// <exception cref="InvalidValueForTypeException">Thrown when the value is invalid for the input type</exception>
		public override void Update(AbstractView newViewState) {
			base.Update(newViewState);

			if (!(newViewState is InputView updatedInputView)) {
				throw new ViewTypeMismatchException("The original view type does not match the type in the update");
			}

			// Removing until we figure out how to handle input types in view changes from HTML -JLW
			/*if (updatedInputView.InputType != InputType) {
				throw new InvalidOperationException("The original view type does not match the type in the update");
			}*/

			if (!IsValueValidForType(updatedInputView.Value)) {
				throw new InvalidValueForTypeException("The new value is invalid for the input type");
			}
			
			Value = updatedInputView.Value;

		}

		/// <inheritdoc cref="AbstractView.UpdateValue"/>
		public override void UpdateValue(string value) {
			
			if (!IsValueValidForType(value)) {
				throw new InvalidValueForTypeException("The new value is invalid for the input type");
			}

			Value = value;
		}

		/// <summary>
		/// Check if the value is valid for the type set on the input view
		/// </summary>
		/// <param name="value">The value to check</param>
		/// <returns>TRUE if the value is valid for the type or FALSE if it is not</returns>
		public bool IsValueValidForType(string value) {

			if (string.IsNullOrWhiteSpace(value)) {
				return true;
			}

			switch (InputType) {
				case EInputType.Text:
				//TODO validate date and time input
				case EInputType.Date:
				case EInputType.Time:
				case EInputType.DateTime:
					//Anything is valid for text
					return true;
				case EInputType.Number:
				case EInputType.Decimal:
					return float.TryParse(value, out _);
				case EInputType.Email:
					try {
						var address = new System.Net.Mail.MailAddress(value);
						return address.Address == value;
					}
					catch (Exception) {
						return false;
					}
				case EInputType.Url:
					return Uri.TryCreate(value, UriKind.Absolute, out _);
				case EInputType.Password:
					//Password types merely restrict the visibility of characters not the input
					return true;
				default:
					return false;
			}
		}

		/// <inheritdoc cref="AbstractView.Value"/>
		/// <remarks>
		/// The same as <see cref="Value"/>
		/// </remarks>
		public override string GetStringValue() {
			return Value;
		}

		/// <inheritdoc cref="AbstractView.ToHtml"/>
		public override string ToHtml(int indent = 0) {

			var sb = new StringBuilder();
			sb.Append(GetIndentStringFromNumber(indent));
			//Open the form div
			sb.Append($"<div id=\"{Id}-par\" class=\"md-form md-outline jui-view\">");
			sb.Append(Environment.NewLine);
            //Add the input
            sb.Append(GetIndentStringFromNumber(indent+1));
            string typeString = null;
            switch (InputType) {
	            case EInputType.Text:
		            typeString = "text\" ";
		            break;
	            case EInputType.Number:
		            typeString = "number\" step=\"1\" pattern=\"[0-9]*\" ";
		            break;
	            case EInputType.Email:
		            typeString = "email\" ";
		            break;
	            case EInputType.Url:
		            typeString = "url\" ";
		            break;
	            case EInputType.Password:
		            typeString = "password\" ";
		            break;
	            case EInputType.Decimal:
		            typeString = "number\" step=\"0.001\" ";
		            break;
	            default:
		            throw new ArgumentOutOfRangeException();
            }
            sb.Append($"<input type=\"{typeString}id=\"{Id}\" class=\"form-control jui-input\" value=\"{Value}\">");
            sb.Append(Environment.NewLine);
            //Add the hint label
            sb.Append(GetIndentStringFromNumber(indent+1));
            sb.Append($"<label for=\"{Id}\" id=\"{Id}-hint\">{Name}</label>");
            sb.Append(Environment.NewLine);
            //Close the form div
            sb.Append(GetIndentStringFromNumber(indent));
            sb.Append("</div>");
            sb.Append(Environment.NewLine);
			
			return sb.ToString();
		}

	}

}