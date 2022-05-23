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
	/// A view displayed as a flex item within a <see cref="GridRow"/>
	/// A size can be associated to this item for each <a href="https://mdbootstrap.com/docs/standard/layout/breakpoints/">responsive breakpoint</a>
	/// </summary>
	public class GridRowItem
	{

		#region Properties

		/// <summary>
		/// The view to display.
		/// </summary>
		[JsonProperty("view")] 
		public AbstractView View { get; set; }

		/// <summary>
		/// Size for screen width 0-576px.
		/// If the same size is used for all screen width, use this property to set it, and leave the other property sizes to <see cref="EColSize.None"/>
		/// </summary>
		[JsonProperty("extra_small_size")]
		public EColSize ExtraSmallSize { get; set; }

		/// <summary>
		/// Size for screen width >= 576px
		/// </summary>
		[JsonProperty("small_size")]
		public EColSize SmallSize { get; set; }

		/// <summary>
		/// Size for screen width >= 768px
		/// </summary>
		[JsonProperty("medium_size")]
		public EColSize MediumSize { get; set; }

		/// <summary>
		/// Size for screen width >= 960px
		/// </summary>
		[JsonProperty("large_size")]
		public EColSize LargeSize { get; set; }

		/// <summary>
		/// Size for screen width >= 1200px
		/// </summary>
		[JsonProperty("extra_large_size")]
		public EColSize ExtraLargeSize { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Create a new instance of a view row item
		/// </summary>
		[JsonConstructor]
		public GridRowItem(AbstractView view, 
							EColSize extraSmallSize = EColSize.Col, 
							EColSize smallSize = EColSize.None, 
							EColSize mediumSize = EColSize.None,
							EColSize largeSize = EColSize.None, 
							EColSize extraLargeSize = EColSize.None)  {
			View = view;
			ExtraSmallSize = extraSmallSize;
			SmallSize = smallSize;
			MediumSize = mediumSize;
			LargeSize = largeSize;
			ExtraLargeSize = extraLargeSize;
		}

		#endregion

		/// <summary>
		/// Get the HTML div class for this item
		/// </summary>
		/// <returns>The div class for this item</returns>
		public string GetHtmlDivClass()
        {
			string divClass = GetColSizeClass("col", ExtraSmallSize);
			
			divClass += GetColSizeClass("col-sm", SmallSize);
			divClass += GetColSizeClass("col-md", MediumSize);
			divClass += GetColSizeClass("col-lg", LargeSize);
			divClass += GetColSizeClass("col-xl", ExtraLargeSize);

			return divClass.Trim();
		}

		private string GetColSizeClass(string prefix, EColSize size)
        {
			switch(size)
            {
				case EColSize.None:
					return "";
				case EColSize.Col:
					return prefix + " ";
				case EColSize.Auto:
					return prefix + "-auto ";
				default:
					return prefix + "-" + ((int)size).ToString() + " ";
            }
        }
	}

}