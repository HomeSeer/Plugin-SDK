using System;
using System.Text;
using HomeSeer.Jui.Types;
using Newtonsoft.Json;
using System.Globalization;

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
		/// <exception cref="InvalidValueForTypeException">Thrown when the value is invalid for the input type</exception>
		[JsonProperty("value")]
		public string Value { get => _value;
            set {
                if (!IsValueValidForType(value)) {
                    throw new InvalidValueForTypeException("The new value is invalid for the input type");
                }
                _value = value;
            }
        }

        private string _value = string.Empty;
        
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
            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentNullException(nameof(name));
            }
			Type      = EViewType.Input;
			InputType = type;
            Value = value ?? string.Empty;
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

            Value = updatedInputView.Value ?? string.Empty;

		}

		/// <inheritdoc cref="AbstractView.UpdateValue"/>
		public override void UpdateValue(string value) {
            Value = value ?? string.Empty;
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
                case EInputType.Date:
                    try {
                        _ = DateTime.Parse(value, CultureInfo.CurrentCulture);
                        return true;
                    }
                    catch (Exception) {
                        return false;
                    }
                case EInputType.Time:
                    try {
                        _ = DateTime.Parse(value);
                        return true;
                    }
                    catch (Exception) {
                        return false;
                    }
				default:
					return false;
			}
		}

		/// <inheritdoc cref="AbstractView.GetStringValue"/>
		/// <remarks>
		/// The same as <see cref="Value"/>
		/// </remarks>
		public override string GetStringValue() {
			return Value;
		}

		/// <inheritdoc cref="AbstractView.ToHtml"/>
		public override string ToHtml(int indent = 0) {
            var value = $"{Value.Replace("\"", "&quot;")}";
            var id = $"{Id}-par";
            var sb = new StringBuilder();
			sb.Append(GetIndentStringFromNumber(indent));
            switch (InputType) {
                case EInputType.Date:
                case EInputType.Time:
                    sb.Append("<div class=\"md-form\">");
                    sb.AppendLine(GetIndentStringFromNumber(indent+1));
                    sb.Append($"<input type=\"text\" id=\"{id}\" class=\"form-control ");
                    sb.Append(InputType == EInputType.Date ? "datepicker jui-date" : "timepicker jui-time");
                    sb.Append($"\" value=\"{value}\">");
                    sb.AppendLine(GetIndentStringFromNumber(indent+1));
                    sb.Append($"<label for=\"{id}\">{Name}</label>");
                    break;
                default:
                    //Open the form div
                    sb.Append($"<div id=\"{Id}-par\" class=\"md-form md-outline jui-view\">");
                    //Add the input
                    sb.AppendLine(GetIndentStringFromNumber(indent+1));
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
                        case EInputType.DateTime:
                            //not supported yet
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    sb.Append($"<input type=\"{typeString}id=\"{Id}\" class=\"form-control jui-input\" value=\"{Value.Replace("\"", "&quot;")}\">");
                    //Add the hint label
                    sb.AppendLine(GetIndentStringFromNumber(indent+1));
                    sb.Append($"<label for=\"{Id}\" id=\"{Id}-hint\">{Name}</label>");
                    break;
            }
            //Close the form div
            sb.AppendLine(GetIndentStringFromNumber(indent));
            sb.Append("</div>");
            sb.Append(Environment.NewLine);
			
			return sb.ToString();
		}

	}

}