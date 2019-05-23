using System;
using System.Text;
using HomeSeer.Jui.Types;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <inheritdoc />
	/// <summary>
	/// An input view is an editable text box for the user to enter strings, numbers, etc.
	/// </summary>
	public sealed class InputView : AbstractView {

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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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

			if (updatedInputView.InputType != InputType) {
				throw new InvalidOperationException("The original view type does not match the type in the update");
			}

			if (!IsValueValidForType(updatedInputView.Value)) {
				throw new InvalidValueForTypeException("The new value is invalid for the input type");
			}
			
			Value = updatedInputView.Value;

		}

		/// <inheritdoc />
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

			switch (InputType) {
				case EInputType.Text:
					//Anything is valid for text
					return true;
				case EInputType.Number:
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
		
		/// <inheritdoc/>
		public override string GetStringValue() {
			return Value;
		}

		/// <inheritdoc/>
		internal override string ToHtml(int indent = 0) {

			var sb = new StringBuilder();
			sb.Append(GetIndentStringFromNumber(indent));
			//Open the form div
			sb.Append("<div class=\"md-form md-outline jui-view\">");
			sb.Append(Environment.NewLine);
            //Add the input
            sb.Append(GetIndentStringFromNumber(indent+1));
            sb.Append("<input type=\"");
            switch (InputType) {
	            case EInputType.Text:
		            sb.Append("text\" ");
		            break;
	            case EInputType.Number:
		            sb.Append("number\" pattern=\"[0-9]*\"");
		            break;
	            case EInputType.Email:
		            sb.Append("email\" ");
		            break;
	            case EInputType.Url:
		            sb.Append("url\" ");
		            break;
	            case EInputType.Password:
		            sb.Append("password\" ");
		            break;
	            case EInputType.Decimal:
		            sb.Append("text\" pattern=\"[0-9.]*\" ");
		            break;
	            default:
		            throw new ArgumentOutOfRangeException();
            }
            sb.Append("id=\" ");
            sb.Append(Id);
            sb.Append("\" class=\"form-control jui-input\" value=\"");
            sb.Append(Value);
            sb.Append("\">");
            sb.Append(Environment.NewLine);
            //Add the hint label
            sb.Append(GetIndentStringFromNumber(indent+1));
            sb.Append("<label for=\"");
            sb.Append(Id);
            sb.Append("\" id=\"");
            sb.Append(Id);
            sb.Append(".hint\">");
            sb.Append(Name);
            sb.Append("</label>");
            sb.Append(Environment.NewLine);
            //Close the form div
            sb.Append(GetIndentStringFromNumber(indent));
            sb.Append("</div>");
            sb.Append(Environment.NewLine);
			
			return sb.ToString();
		}

	}

}