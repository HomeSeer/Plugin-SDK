using System;
using HomeSeer.Jui.Types;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views 
{
	/// <summary>
	/// A set of views that are displayed as a flexbox row within a <see cref="GridView"/>
	/// </summary>
	public class GridRow
	{
		#region Properties

		/// <summary>
		/// The views to display within this row. This is for access only.
		/// </summary>
		[JsonIgnore]
		public ReadOnlyCollection<AbstractView> Views => _items.ConvertAll( x => x.View).AsReadOnly();

		/// <summary>
		/// The items to display within this row.
		/// </summary>
		[JsonProperty("items")] 
		private List<GridRowItem> _items;
		

		#endregion

		#region Constructors

		/// <summary>
		/// Create a new instance of a view group row
		/// </summary>
		[JsonConstructor]
		public GridRow() 
		{
			_items = new List<GridRowItem>();
		}
		
		#endregion
		
		#region Items
		
		#region Add

		/// <summary>
		/// Add an item to the row
		/// </summary>
        public void AddItem(AbstractView view,
							EColSize extraSmallSize = EColSize.Col,
							EColSize smallSize = EColSize.None,
							EColSize mediumSize = EColSize.None,
							EColSize largeSize = EColSize.None,
							EColSize extraLargeSize = EColSize.None) 
		{
			if (view?.Id == null)
			{
				throw new ArgumentNullException(nameof(view), "The view or its ID is null");
			}
			_items.Add(new GridRowItem(view, extraSmallSize, smallSize, mediumSize, largeSize, extraLargeSize));
		}

		#endregion

		#endregion

		/// <summary>
		/// Get a string representation of this grid row converted into HTML
		/// </summary>
		/// <returns>An HTML representation of the view as a string</returns>
		public string ToHtml(int indent = 0)
		{
			var sb = new StringBuilder();
			sb.Append(AbstractView.GetIndentStringFromNumber(indent));
			//Open the containing div
			sb.Append("<div class=\"row\">");
			sb.Append(Environment.NewLine);
			//Add items
			foreach (var item in _items)
			{
				sb.Append(AbstractView.GetIndentStringFromNumber(indent + 1));
				sb.Append("<div class=\"");
				sb.Append(item.GetHtmlDivClass());
				sb.Append("\">");
				sb.Append(Environment.NewLine);
				sb.Append(item.View.ToHtml(indent + 2));
				sb.Append(AbstractView.GetIndentStringFromNumber(indent + 1));
				sb.Append("</div>");
				sb.Append(Environment.NewLine);
			}
			//Close the containing div
			sb.Append(AbstractView.GetIndentStringFromNumber(indent));
			sb.Append("</div>");
			sb.Append(Environment.NewLine);

			return sb.ToString();
		}
	}

}