using HomeSeer.Jui.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <summary>
	/// A page is the primary container used to define a view for the user interface.
	/// </summary>
	public class Page {
		
		#region Properties
		
		/// <summary>
		/// A unique identifier for the page
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; protected set; }
		
		/// <summary>
		/// The name that is displayed to the user for this page
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; protected set; }
		
		/// <summary>
		/// The page's type; this determines where this page will present itself to the user.
		/// </summary>
		[JsonProperty("type")]
		public EPageType Type { get; protected set; }
		
		/// <summary>
		/// A Base64 encoded image used as an icon for this page
		/// </summary>
		[JsonProperty("image")]
		public string Image { get; set; }

		/// <summary>
		/// The collection of views that are on this page.   This is for access only.
		/// Use <see cref="AddView"/> and <see cref="UpdateViewById"/> for setting views/updating them
		/// </summary>
		[JsonIgnore]
		public ReadOnlyCollection<AbstractView> Views => (_views ?? (_views = new List<AbstractView>())).AsReadOnly();

		/// <summary>
		/// A list of the IDs of the views in this group
		/// </summary>
		[JsonIgnore]
		public ReadOnlyCollection<string> ViewIds => (_viewIds?.Keys ?? (_viewIds = new Dictionary<string, int>()).Keys)
		                                             .ToList().AsReadOnly();
		
		/// <summary>
		/// The number of views that are on this page.
		/// </summary>
		[JsonIgnore]
		public int ViewCount => _viewIds?.Count ?? 0;

		/// <summary>
		/// The collection of views that are on this page.
		/// </summary>
		[JsonProperty("views")] 
		private List<AbstractView> _views;
		
		/// <summary>
		/// A set of IDs for the views on this page.  This is used to ensure that there are no duplicate IDs used.
		/// </summary>
		[JsonIgnore]
		private Dictionary<string, int> _viewIds = new Dictionary<string, int>();
		
		#endregion
		
		#region Constructors
		
		/// <summary>
		/// Create a new instance of a Page of the specified type with an ID and Name
		/// </summary>
		/// <param name="id">The unique ID of the page</param>
		/// <param name="name">The name of the page; the title shown to the user</param>
		/// <param name="type">The type of the page</param>
		/// <exception cref="ArgumentNullException">Thrown if a Page is created with an invalid ID or Name</exception>
		[JsonConstructor]
		protected Page(string id, string name, EPageType type) {
			if (string.IsNullOrWhiteSpace(id)) {
				throw new ArgumentNullException(nameof(id), "You must specify an ID");
			}

			if (string.IsNullOrWhiteSpace(name)) {
				throw new ArgumentNullException(nameof(name), "You must specify a name for each page");
			}
			
			Id = id;
			Name = name;
			Type = type;
			_views = new List<AbstractView>();
		}
		
		#endregion
		
		#region JSON
		
		/// <summary>
		/// Serialize the page as JSON
		/// </summary>
		/// <returns>A string containing the page formatted as JSON</returns>
		public string ToJsonString() {
			return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings {
				TypeNameHandling = TypeNameHandling.Auto
			});
		}
		
		#endregion

		/// <summary>
		/// Get a string representation of this page converted into HTML
		/// </summary>
		/// <returns>An HTML representation of the page as a string</returns>
		public string ToHtml(int indent = 0) {
			
			var sb = new StringBuilder();
			//Open the containing div
			sb.Append(AbstractView.GetIndentStringFromNumber(indent));
			sb.Append("<div class=\"container\">");
			sb.Append(Environment.NewLine);
			indent++;
			sb.Append(AbstractView.GetIndentStringFromNumber(indent));
			sb.Append("<form>");
			sb.Append(Environment.NewLine);
			indent++;
			//Add all of the child views
			foreach (var view in _views) {
				sb.Append(view.ToHtml(indent));
			}
			indent--;
			sb.Append(AbstractView.GetIndentStringFromNumber(indent));
			sb.Append("</form>");
			sb.Append(Environment.NewLine);
			indent--;
			sb.Append(AbstractView.GetIndentStringFromNumber(indent));
			sb.Append("</div>");
			sb.Append(Environment.NewLine);
			
			return sb.ToString();
		}

		/// <summary>
		/// Get a dictionary mapping IDs to values for the views on this page with mutable values.
		/// <para>
		/// These include: InputViews, SelectListViews, and ToggleViews
		/// </para>
		/// </summary>
		/// <returns>A Dictionary of view IDs to view values</returns>
		public Dictionary<string, string> ToValueMap() {
			
			var settingMap = new Dictionary<string, string>();
			
			if (_views == null || _views.Count == 0) {
				return settingMap;
			}
			
			foreach (var view in _views) {
				try {
					var viewValue = view.GetStringValue();
					if (viewValue == null) {
						continue;
					}
					
					settingMap.Add(view.Id, viewValue);
				}
				catch (InvalidOperationException exception) {
					//Console.WriteLine(exception);
					if (!(view is ViewGroup vg)) {
						continue;
					}

					foreach (var vgView in vg.Views) {
						var vgViewValue = vgView.GetStringValue();
						if (vgViewValue == null) {
							continue;
						}
					
						settingMap.Add(vgView.Id, vgViewValue);
					}
				}
			}

			return settingMap;
		}

		/// <inheritdoc cref="ViewCollectionHelper.MapViewIds"/>
		private void MapViewIds() {
			
			ViewCollectionHelper.MapViewIds(_views, out _viewIds);
		}
		
		#region Views
		
		#region Create/Add

		/// <inheritdoc cref="ViewCollectionHelper.AddView"/>
		/// <summary>
		/// Add a view to the page
		/// </summary>
		public void AddView(AbstractView view) {
			ViewCollectionHelper.AddView(view, ref _views, ref _viewIds);
		}

		/// <summary>
		/// Add a view change to the page
		/// <para>
		/// Used to log value changes for views on settings pages.  All names are left blank
		/// </para>
		/// </summary>
		/// <param name="id">The id of the view</param>
		/// <param name="type">The EViewType of the view</param>
		/// <param name="value">The new value for the view</param>
		/// <exception cref="ArgumentNullException">A valid ID was not specified</exception>
		/// <exception cref="ArgumentOutOfRangeException">The type or integer value is invalid</exception>
		/// <exception cref="ArgumentException">The value type doesn't match the view type</exception>
		public void AddViewDelta(string id, int type, object value) {

			if (string.IsNullOrWhiteSpace(id)) {
				throw new ArgumentNullException(nameof(id), "ID cannot be blank");
			}

			if (type < 0) {
				throw new ArgumentOutOfRangeException(nameof(type));
			}

			AbstractView view = null;

			switch (value) {
				case string valueString:
					if (type == (int) EViewType.SelectList) {
						try {
							var intValue = int.Parse(valueString);
							if (intValue < 0) {
								throw new ArgumentOutOfRangeException(nameof(value), "Selection index must be greater than or equal to 0.");
							}
							view = new SelectListView(id, "", new List<string>(intValue+1), ESelectListType.DropDown, intValue);
							break;
						}
						catch (Exception exception) {
							throw new ArgumentException("Value type does not match the view type", exception);
						}
					}
					
					if (type != (int) EViewType.Input) {
						throw new ArgumentException("The view type does not match the value type");
					}
					
					view = new InputView(id, "", valueString);
					break;
				
				case int valueInt:
					
					if (type != (int) EViewType.SelectList) {
						throw new ArgumentException("The view type does not match the value type");
					}
					
					if (valueInt < 0) {
						throw new ArgumentOutOfRangeException(nameof(value), "Selection index must be greater than or equal to 0.");
					}

					var options = new List<string>();
					for (var i = 0; i <= valueInt; i++) {
						options.Add(i.ToString());
					}
					
					view = new SelectListView(id, "", options, ESelectListType.DropDown, valueInt);
					break;
				
				case bool valueBool:
					if (type != (int) EViewType.Toggle) {
						throw new ArgumentException("The view type does not match the value type");
					}
            
					view = new ToggleView(id, "", valueBool);
					break;
			}

			if (view == null) {
				throw new ArgumentException("Unable to build a view from the data provided");
			}
			
			AddView(view);
		}

		/// <inheritdoc cref="ViewCollectionHelper.AddViews"/>
		/// <summary>
		/// Add multiple views to the page
		/// </summary>
		public void AddViews(IEnumerable<AbstractView> views) {
			
			ViewCollectionHelper.AddViews(views, ref _views, ref _viewIds);
		}

		/// <inheritdoc cref="ViewCollectionHelper.SetViews"/>
		/// <summary>
		/// Set the list of views on this page
		/// </summary>
		public void SetViews(IEnumerable<AbstractView> views) {

			ViewCollectionHelper.SetViews(views, ref _views, ref _viewIds);
		}
		
		#endregion
		
		#region Read
		
		/// <inheritdoc cref="ViewCollectionHelper.ContainsViewWithId"/>
		public bool ContainsViewWithId(string viewId) {

			return ViewCollectionHelper.ContainsViewWithId(viewId, ref _views, ref _viewIds);
		}

		/// <summary>
		/// Get the view on the page with the given ID
		/// </summary>
		/// <param name="viewId">The ID of the view to get</param>
		/// <returns>
		/// The view with the specified ID as an <see cref="AbstractView"/>.
		/// This should be cast to the appropriate view type before use.
		/// </returns>
		/// <exception cref="ArgumentNullException">An invalid view ID was entered</exception>
		/// <exception cref="ArgumentException">No views are on the page to get</exception>
		/// <exception cref="IndexOutOfRangeException">The ID was found, but the view was not.  The page is probably malformed and should be recreated.</exception>
		/// <exception cref="KeyNotFoundException">No views with that ID were found</exception>
		public AbstractView GetViewById(string viewId) {
			
			return ViewCollectionHelper.GetViewById(viewId, ref _views, ref _viewIds);
		}
		
		#endregion
		
		#region Update

		/// <inheritdoc cref="ViewCollectionHelper.UpdateViewById"/>
		public void UpdateViewById(AbstractView view) {

			ViewCollectionHelper.UpdateViewById(view, ref _views, ref _viewIds);
		}
		
		/// <inheritdoc cref="ViewCollectionHelper.UpdateViewValueById"/>
		public void UpdateViewValueById(string id, string value) {

			ViewCollectionHelper.UpdateViewValueById(id, value, ref _views, ref _viewIds);
		}
		
		#endregion
		
		#region Delete

		/// <inheritdoc cref="ViewCollectionHelper.RemoveAllViews"/>
		public void RemoveAllViews() {
			
			ViewCollectionHelper.RemoveAllViews(out _views, out _viewIds);
		}
		
		#endregion
		
		#endregion

		/// <inheritdoc />
		/// <summary>
		/// Compares the Id, Name, and the number of views
		/// </summary>
		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;

			if (!(obj is Page otherPage)) {
				return false;
			}
			
			if (Id != otherPage.Id) {
				return false;
			}
			
			if (Name != otherPage.Name) {
				return false;
			}

			if (ViewCount != otherPage.ViewCount) {
				return false;
			}

			return true;
		}

		/// <inheritdoc />
		/// <summary>
		/// Compares the Id, Name, and the number of views
		/// </summary>
		public override int GetHashCode() {
			return Id.GetHashCode() * Name.GetHashCode() * ViewCount.GetHashCode();
		}

		/// <summary>
		/// A factory class for creating pages
		/// </summary>
		public static class Factory {

			/// <summary>
			/// Create a new settings page
			/// </summary>
			/// <param name="id">The ID for the page</param>
			/// <param name="name">The name of the page</param>
			/// <returns>A new Page with its type set to EPageType.Settings</returns>
			public static Page CreateSettingPage(string id, string name) {
				
				return new Page(id, name, EPageType.Settings);
			}
			
			/// <summary>
			/// Create a new feature page
			/// </summary>
			/// <param name="id">The ID for the page</param>
			/// <param name="name">The name of the page</param>
			/// <returns>A new Page with its type set to EPageType.Feature</returns>
			internal static Page CreateFeaturePage(string id, string name) {
				
				return new Page(id, name, EPageType.Feature);
			}
			
			/// <summary>
			/// Create a new device inclusion page
			/// </summary>
			/// <param name="id">The ID for the page</param>
			/// <param name="name">The name of the page</param>
			/// <returns>A new Page with its type set to EPageType.DeviceInclude</returns>
			internal static Page CreateDeviceIncPage(string id, string name) {
				
				return new Page(id, name, EPageType.DeviceInclude);
			}
			
			/// <summary>
			/// Create a new device configuration page
			/// </summary>
			/// <param name="id">The ID for the page</param>
			/// <param name="name">The name of the page</param>
			/// <returns>A new Page with its type set to EPageType.DeviceConfig</returns>
			public static Page CreateDeviceConfigPage(string id, string name) {
				
				return new Page(id, name, EPageType.DeviceConfig);
			}
			
			/// <summary>
			/// Create a new guided process page
			/// </summary>
			/// <param name="id">The ID for the page</param>
			/// <param name="name">The name of the page</param>
			/// <returns>A new Page with its type set to EPageType.Guide</returns>
			internal static Page CreateGuidedProcessPage(string id, string name) {
				
				return new Page(id, name, EPageType.Guide);
			}
			
			/// <summary>
			/// Create a new HTML feature page
			/// </summary>
			/// <param name="id">The ID for the page</param>
			/// <param name="name">The name of the page</param>
			/// <returns>A new Page with its type set to EPageType.FeatureHtml</returns>
			internal static Page CreateHtmlFeaturePage(string id, string name) {
				
				return new Page(id, name, EPageType.FeatureHtml);
			}
			
			/// <summary>
			/// Deserialize a JSON string to a page
			/// <para>
			/// This should always be wrapped in a try/catch in case the data received is malformed
			/// </para>
			/// </summary>
			/// <param name="jsonString">The JSON string containing the page</param>
			/// <returns>A Page</returns>
			/// <exception cref="JsonDataException">Thrown when there was a problem deserializing the page</exception>
			public static Page FromJsonString(string jsonString) {
				try {
					var page = JsonConvert.DeserializeObject<Page>(jsonString,
					                                               new JsonSerializerSettings
					                                               { TypeNameHandling = TypeNameHandling.Auto }
					                                              );
				
					page.MapViewIds();
					return page;
				}
				catch (JsonSerializationException exception) {
					
					throw new JsonDataException(exception);
				}
			}

			/// <summary>
			/// Serialize a list of pages to a JSON string
			/// </summary>
			/// <param name="pages">the List of Pages to serialize</param>
			/// <returns>A JSON string containing the serialized pages</returns>
			/// <exception cref="JsonDataException">Thrown when there was a problem serializing the page</exception>
			[Obsolete]
			public static string JsonFromList(List<Page> pages) {
				try {
					var serializedList = JsonConvert.SerializeObject(pages, 
					                                                 Formatting.None, 
					                                                 new JsonSerializerSettings {
																									TypeNameHandling = TypeNameHandling.Auto
					                                                                            });

					return serializedList;
				}
				catch (JsonSerializationException exception) {
					
					throw new JsonDataException(exception);
				}
			}

			/// <summary>
			/// Deserialize a JSON string to a List of Pages
			/// </summary>
			/// <param name="jsonString">The JSON string containing the list of pages</param>
			/// <returns>A List of Jui.Page objects</returns>
			/// <exception cref="JsonDataException">Thrown when there was a problem deserializing the page</exception>
			public static List<Page> ListFromJson(string jsonString) {
				try {
					var pages = JsonConvert.DeserializeObject<List<Page>>(jsonString,
					                                               new JsonSerializerSettings
					                                               { TypeNameHandling = TypeNameHandling.Auto }
					                                              );
				
					return pages;
				}
				catch (JsonSerializationException exception) {
					
					throw new JsonDataException(exception);
				}
			}

			/// <summary>
			/// Convert a list of JSON pages into a tabbed HTML page.
			/// </summary>
			/// <param name="jsonPages">A list of pages serialized as JSON strings</param>
			/// <param name="selectedPage">The index of the page that should be selected by default</param>
			/// <returns>A string containing a tabbed HTML page</returns>
			/// <exception cref="ArgumentNullException">Thrown if the supplied list of pages is null</exception>
			/// <exception cref="ArgumentException">Thrown if the list of pages is empty</exception>
			[Obsolete("Please use a SettingsCollection instead", true)]
			public static string PageListToHtml(List<string> jsonPages, int selectedPage = 0) {

				if (jsonPages == null) {
					throw new ArgumentNullException(nameof(jsonPages));
				}

				// ReSharper disable once SwitchStatementMissingSomeCases
				switch (jsonPages.Count) {
					case 0:
						throw new ArgumentException("There are no pages to convert to HTML");
					case 1: {
						var page = FromJsonString(jsonPages[0]);
						return page.ToHtml();
					}
				}

				var pages = jsonPages.Select(FromJsonString).ToList();
				
				var sb     = new StringBuilder();
				var indent = 0;
				//Open the tablist
				sb.Append("<ul class=\"nav nav-tabs hs-tabs\" role=\"tablist\">");
				sb.Append(Environment.NewLine);
				indent++;
				//Add tabs
				var pageIndex = -1;
				foreach (var page in pages) {
					pageIndex++;
					sb.Append(AbstractView.GetIndentStringFromNumber(indent));
					//Open nav item
					sb.Append("<li class=\"nav-item\">");
					sb.Append(Environment.NewLine);
					indent++;
					sb.Append(AbstractView.GetIndentStringFromNumber(indent));
					//Open anchor
					sb.Append("<a class=\"nav-link waves-light\" id=\"");
					sb.Append(page.Id);
					sb.Append(".tab\" data-toggle=\"tab\" href=\"#");
					sb.Append(page.Id);
					sb.Append("\" role=\"tab\" aria-controls=\"");
					sb.Append(page.Id);
					sb.Append("\" aria-selected=\"");
					sb.Append(pageIndex == selectedPage ? "true" : "false");
					sb.Append("\">");
					sb.Append(page.Name);
					//Close anchor
					sb.Append("</a>");
					sb.Append(Environment.NewLine);
					indent--;
					sb.Append(AbstractView.GetIndentStringFromNumber(indent));
					//Close nav item
					sb.Append("</li>");
					sb.Append(Environment.NewLine);
				}
				
				//Close the tablist
				sb.Append("</ul>");
				sb.Append(Environment.NewLine);
				//Open the content div
				sb.Append("<div class=\"tab-content container\">");
				sb.Append(Environment.NewLine);
				pageIndex = -1;
				//Add the tab content divs
				foreach (var page in pages) {
					pageIndex++;
					sb.Append(AbstractView.GetIndentStringFromNumber(indent));
					//Open the tab pane div
					sb.Append("<div class=\"tab-pane fade");
					if (pageIndex == 0) {
						sb.Append(" active show");
					}
					sb.Append("\" role=\"tabpanel\" aria-labelledby=\"");
					sb.Append(page.Id);
					sb.Append(".tab\" id=\"");
					sb.Append(page.Id);
					sb.Append("\">");
					sb.Append(Environment.NewLine);
					indent++;
					//Add the page content
					sb.Append(page.ToHtml(indent));
					indent--;
					sb.Append(AbstractView.GetIndentStringFromNumber(indent));
					//Close the tab content div
					sb.Append("</div>");
					sb.Append(Environment.NewLine);
				}
				//Close the content div
				sb.Append("</div>");

				return sb.ToString();
			}
			

		}

	}

}