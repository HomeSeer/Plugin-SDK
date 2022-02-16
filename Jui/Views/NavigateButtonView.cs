using System;
using System.Text;
using HomeSeer.Jui.Types;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <inheritdoc cref="AbstractView"/>
	/// <summary>
	/// A button is used to navigate the UI to another HomeSeer page.
	/// </summary>
	/// <remarks>
	/// This type of view is not recommended for event actions and triggers because it will force the user away from
	///  the page they are on. It disrupts the workflow for editing and creating actions and triggers.
	/// </remarks>
	public sealed class NavigateButtonView : AbstractView {
        
        private const string _invalidNameCharacters = "<>\n\r";
        
        /// <summary>
        /// The HomeSeer URL this button navigates to. This must be a location relative to the hostname of the system.
        ///  It must start with a /
        /// <para>
        /// For example: "/devices.html"
        /// </para>
        /// </summary>
        public string HomeSeerUrl { get; private set; }
        
        //TODO Button Color Styles

        /// <inheritdoc cref="AbstractView"/>
        /// <summary>
        /// Create an instance of a ButtonView with an ID and Name
        /// </summary>
        /// <param name="id">The unique ID for the View</param>
        /// <param name="name">The name of the View. Must not be blank</param>
        /// <param name="homeSeerUrl">The HomeSeer URL this button navigates to. See <see cref="HomeSeerUrl"/> for the correct format.</param>
        /// <exception cref="ArgumentException">Thrown if a view is created with an invalid Name or HomeSeerUrl</exception>
        [JsonConstructor]
		public NavigateButtonView(string id, string name, string homeSeerUrl) : base(id, name) {
			
			if (string.IsNullOrWhiteSpace(name)) {
				throw new ArgumentException("Name cannot be null or blank", nameof(name));
			}
            if (name.IndexOfAny(_invalidNameCharacters.ToCharArray()) != -1) {
                throw new ArgumentException("The name cannot contain any of the following characters: <>\\n\\r", nameof(name));
            }
            if (string.IsNullOrWhiteSpace(homeSeerUrl)) {
                throw new ArgumentException("homeSeerUrl cannot be null or blank", nameof(homeSeerUrl));
            }
            if (!homeSeerUrl.StartsWith("/") || !Uri.TryCreate(homeSeerUrl, UriKind.Relative, out _)) {
                throw new ArgumentException("The homeSeerUrl must be in a relative format like this /devices.html", nameof(homeSeerUrl));
            }
            Type = EViewType.Button;
            HomeSeerUrl = homeSeerUrl;
        }

		/// <inheritdoc cref="AbstractView.GetStringValue"/>
		/// <returns>Returns null</returns>
		public override string GetStringValue() {
			return null;
		}

        /// <inheritdoc cref="AbstractView.Update"/>
        /// <summary>
        /// Update the view to the new state. This will change the <see cref="HomeSeerUrl"/> 
        /// </summary>
        /// <exception cref="ViewTypeMismatchException">Thrown when the new view's class doesn't match the calling view</exception>
        public override void Update(AbstractView newViewState) {
            base.Update(newViewState);
            if (!(newViewState is NavigateButtonView updatedButtonView)) {
                throw new ViewTypeMismatchException("The original view type does not match the type in the update");
            }
            HomeSeerUrl = updatedButtonView.HomeSeerUrl;
        }

        /// <inheritdoc cref="AbstractView.ToHtml"/>
		public override string ToHtml(int indent = 0) {
			var sb = new StringBuilder();
			sb.Append(GetIndentStringFromNumber(indent));
			sb.Append($"<button class=\"btn btn-primary mt-2\" type=\"button\" onclick=\"jui_navigate({HomeSeerUrl});\" id=\"{Id}\">");
            sb.Append(Name);
			sb.Append("</button>");
			sb.Append(Environment.NewLine);
            return sb.ToString();
		}

	}

}