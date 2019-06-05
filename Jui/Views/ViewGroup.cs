using System;
using HomeSeer.Jui.Types;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {

	/// <inheritdoc />
	/// <summary>
	/// View groups define a vertical sub-grouping of views with a header.
	/// </summary>
	public class ViewGroup : AbstractView {

		#region Properties
		
		/// <summary>
		/// The views to display within this group.   This is for access only.
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
		/// The number of views that are in this group.
		/// </summary>
		[JsonIgnore]
		public int ViewCount => _viewIds?.Count ?? 0;

		/// <summary>
		/// The views to display within this group.
		/// </summary>
		[JsonProperty("views")] 
		private List<AbstractView> _views;
		
		/// <summary>
		/// A set of IDs for the views in this group.  This is used to ensure that there are no duplicate IDs used.
		/// </summary>
		[JsonIgnore]
		private Dictionary<string, int> _viewIds = new Dictionary<string, int>();

		#endregion
		
		#region Constructors
		
		/// <inheritdoc />
		/// <summary>
		/// Create a new instance of a view group with an ID
		/// </summary>
		/// <param name="id">The unique ID of the group</param>
		[JsonConstructor]
		protected ViewGroup(string id) : base(id) {
			Type = EViewType.Group;
			_views = new List<AbstractView>();
		}

		/// <inheritdoc />
		/// <summary>
		/// Create a new instance of a view group with an ID and Name
		/// </summary>
		/// <param name="id">The unique ID of the group</param>
		/// <param name="name">The unique ID of the name. DEFAULT: null</param>
		public ViewGroup(string id, string name = null) : base(id, name) {
			Type = EViewType.Group;
			_views = new List<AbstractView>();
		}
		
		#endregion
		
		#region Views
		
		#region Create/Add

		/// <inheritdoc cref="ViewCollectionHelper.AddView"/>
		/// <summary>
		/// Add a view to the group
		/// </summary>
        public void AddView(AbstractView view) {

			ViewCollectionHelper.AddView(view, ref _views, ref _viewIds, true);
		}

		/// <inheritdoc cref="ViewCollectionHelper.AddViews"/>
		/// <summary>
		/// Add multiple views to the group
		/// </summary>
		public void AddViews(IEnumerable<AbstractView> views) {
			
			ViewCollectionHelper.AddViews(views, ref _views, ref _viewIds, true);
		}

		/// <inheritdoc cref="ViewCollectionHelper.SetViews"/>
		/// <summary>
		/// Set the list of views in this group
		/// </summary>
		public void SetViews(IEnumerable<AbstractView> views) {

			ViewCollectionHelper.SetViews(views, ref _views, ref _viewIds, true);
		}
		
		#endregion
		
		#region Read
		
		/// <inheritdoc cref="ViewCollectionHelper.ContainsViewWithId"/>
		public bool ContainsViewWithId(string viewId) {

			return ViewCollectionHelper.ContainsViewWithId(viewId, ref _views, ref _viewIds);
		}

		/// <inheritdoc cref="ViewCollectionHelper.GetViewById"/>
		public AbstractView GetViewById(string viewId) {

			return ViewCollectionHelper.GetViewById(viewId, ref _views, ref _viewIds);
		}
		
		/// <inheritdoc/>
		/// <exception cref="InvalidOperationException">Thrown to indicate that this ViewGroup contains other views</exception>
		public override string GetStringValue() {
			if (_views.Count > 0) {
				throw new InvalidOperationException("This is a ViewGroup that contains other views.");
			}

			return null;
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
		
		/// <inheritdoc cref="ViewCollectionHelper.MapViewIds"/>
		internal void MapViewIds() {
			
			ViewCollectionHelper.MapViewIds(_views, out _viewIds, true);
		}

		/// <inheritdoc />
		/// <summary>
		/// Not used by ViewGroups
		/// </summary>
		public override void Update(AbstractView newViewState) {
			throw new NotImplementedException("Do not call Update on a ViewGroup; call UpdateViewById(AbstractView) instead.");
		}

		#endregion
		
		#region Delete

		/// <inheritdoc cref="ViewCollectionHelper.RemoveAllViews"/>
		public void RemoveAllViews() {
			
			ViewCollectionHelper.RemoveAllViews(out _views, out _viewIds);
		}
		
		#endregion
		
		#endregion

		internal override string ToHtml(int indent = 0) {

			var sb = new StringBuilder();
			sb.Append(GetIndentStringFromNumber(indent));
			//Open the containing div
			sb.Append("<div id=\"");
			sb.Append(Id);
			sb.Append("\" class=\"jui-view\">");
			sb.Append(Environment.NewLine);
			//Add the title
			sb.Append(GetIndentStringFromNumber(indent+1));
			sb.Append("<div id=\"");
			sb.Append(Id);
			sb.Append(".title\" class=\"jui-title\">");
			sb.Append("<small>");
			sb.Append(Name);
			sb.Append("</small>");
			sb.Append("</div>");
			sb.Append(Environment.NewLine);
			//Add child views
			foreach (var view in _views) {
				sb.Append(view.ToHtml(indent + 1));
			}
			
			//Close the containing div
			sb.Append(GetIndentStringFromNumber(indent));
			sb.Append("</div>");
			sb.Append(Environment.NewLine);

			return sb.ToString();
		}

	}

}