using System;
using System.Text;
using HomeSeer.Jui.Types;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <summary>
	/// The base implementation of a JUI view
	/// </summary>
	public abstract class AbstractView {

		/// <summary>
		/// A unique identifier for this view.  You will need to use this to identify the view when HomeSeer 
		/// communicates changes to its values from a client.
		/// <para>
		/// For consistency and readability it is advised to use the format of COMPANY.PLUGIN.PAGE.VIEW
		/// </para>
		/// <para>
		/// For example: a LabelView on the first settings page in the Z-Wave Plugin
		/// might have an id of HomeSeer.ZWave.Settings1.InterfaceName
		/// </para>
		/// </summary>
		[JsonProperty("id", Required = Required.Always)]
		public string Id { get; protected set; }
		
		/// <summary>
		/// The name/title of this view
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; protected set; }
		
		/// <summary>
		/// The type of this view.
		/// <para>
		/// This is automatically configured
		/// </para>
		/// </summary>
		[JsonProperty("type", Required = Required.Always)]
		public EViewType Type { get; protected set; }

		/// <summary>
		/// Represents a tab/indent for formatting HTML
		/// </summary>
		[JsonIgnore] private const string HtmlIndent = "    ";
		
		/// <summary>
		/// Create an instance of an AbstractView with an ID
		/// </summary>
		/// <param name="id">The unique ID for the AbstractView</param>
		/// <exception cref="ArgumentNullException">Thrown if a view is created with an invalid ID</exception>
		protected AbstractView(string id) {
			if (string.IsNullOrWhiteSpace(id)) {
				throw new ArgumentNullException(nameof(id));
			}
			
			Id = id;
		}
		
		/// <summary>
		/// Create an instance of an AbstractView with an ID and Name
		/// </summary>
		/// <param name="id">The unique ID for the AbstractView</param>
		/// <param name="name">The name for the AbstractView</param>
		/// <exception cref="ArgumentNullException">Thrown if a view is created with an invalid ID</exception>
		protected AbstractView(string id, string name) {
			if (string.IsNullOrWhiteSpace(id)) {
				throw new ArgumentNullException(nameof(id));
			}
			
			Id = id;
			Name = name;
		}

		/// <summary>
		/// Get the value associated with this view as a string if there is one.
		/// </summary>
		/// <returns>
		/// The value stored in this view as a string or NULL if there is no value stored.
		/// </returns>
		public abstract string GetStringValue();

		/// <summary>
		/// Update the the user editable properties from a new version of the same view
		/// </summary>
		/// <param name="newViewState">
		/// The new state of the view being updated.
		/// This view's ID and Type must match the calling view exactly
		/// </param>
		/// <exception cref="ArgumentNullException">Thrown when the new state of the view is null</exception>
		/// <exception cref="InvalidOperationException">Thrown when the new view's ID or Type don't match the calling view</exception>
		public virtual void Update(AbstractView newViewState) {

			if (newViewState == null) {
				throw new ArgumentNullException(nameof(newViewState));
			}

			if (newViewState.Id != Id) {
				throw new InvalidOperationException("The ID of the update does not match the ID of the original view");
			}

			if (newViewState.Type != Type) {
				throw new InvalidOperationException("The original view type does not match the type in the update");
			}

		}

		/// <summary>
		/// Update the value of the view
		/// </summary>
		/// <param name="value">The new value</param>
		public virtual void UpdateValue(string value) {
			
		}

		/// <summary>
		/// Get a string representation of this view converted into HTML
		/// </summary>
		/// <returns>An HTML representation of the view as a string</returns>
		internal abstract string ToHtml(int indent = 0);

		internal static string GetIndentStringFromNumber(int indent) {	
			
			var indentString = new StringBuilder("");
			
			for (var i = 0; i < indent; i++) {
				indentString.Append(HtmlIndent);
			}

			return indentString.ToString();
		}

	}

}