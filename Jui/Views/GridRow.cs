using System;
using HomeSeer.Jui.Types;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace HomeSeer.Jui.Views {
    /// <summary>
    /// A set of views that are displayed as a flexbox row within a <see cref="GridView"/>
    /// </summary>
    public class GridRow {
        #region Properties

        /// <summary>
        /// The views to display within this row. This is for access only.
        /// </summary>
        [JsonIgnore]
        public ReadOnlyCollection<AbstractView> Views => _items.ConvertAll(x => x.View).AsReadOnly();

        /// <summary>
        /// The  <a href="https://getbootstrap.com/docs/4.0/layout/grid/#horizontal-alignment">horizontal alignment</a> of the items in the row
        /// </summary>
        [JsonProperty("horizontal_alignment")]
        public EHorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// The  <a href="https://getbootstrap.com/docs/4.0/layout/grid/#vertical-alignment">vertical alignment</a> of the items in the row
        /// </summary>
        [JsonProperty("vertical_alignment")]
        public EVerticalAlignment VerticalAlignment { get; set; }

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
        public GridRow() {
            _items = new List<GridRowItem>();
        }

        #endregion

        #region Items

        #region Add

        /// <summary>
        /// Add an item to the row
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the specified view or its ID is null</exception>
        public void AddItem(AbstractView view,
                            EColSize extraSmallSize = EColSize.Col,
                            EColSize smallSize = EColSize.None,
                            EColSize mediumSize = EColSize.None,
                            EColSize largeSize = EColSize.None,
                            EColSize extraLargeSize = EColSize.None) {
            if (view?.Id == null) {
                throw new ArgumentNullException(nameof(view), "The view or its ID is null");
            }
            _items.Add(new GridRowItem(view, extraSmallSize, smallSize, mediumSize, largeSize, extraLargeSize));
        }

        #endregion

        #endregion

        private string GetHorizontalAlignmentClass() {
            switch (HorizontalAlignment) {
                case EHorizontalAlignment.JustifyContentStart:
                    return "justify-content-start";
                case EHorizontalAlignment.JustifyContentEnd:
                    return "justify-content-end";
                case EHorizontalAlignment.JustifyContentCenter:
                    return "justify-content-center";
                case EHorizontalAlignment.JustifyContentAround:
                    return "justify-content-around";
                case EHorizontalAlignment.JustifyContentBetween:
                    return "justify-content-between";
                default:
                    return "";
            }
        }

        private string GetVerticalAlignmentClass() {
            switch (VerticalAlignment) {
                case EVerticalAlignment.AlignItemsStart:
                    return "align-items-start";
                case EVerticalAlignment.AlignItemsEnd:
                    return "align-items-end";
                case EVerticalAlignment.AlignItemsCenter:
                    return "align-items-center";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Get a string representation of this grid row converted into HTML
        /// </summary>
        /// <returns>An HTML representation of the view as a string</returns>
        public string ToHtml(int indent = 0) {
            var sb = new StringBuilder();
            sb.Append(AbstractView.GetIndentStringFromNumber(indent));
            //Open the containing div
            sb.Append("<div class=\"row");
            if (HorizontalAlignment != EHorizontalAlignment.None) {
                sb.Append($" {GetHorizontalAlignmentClass()}");
            }
            if (VerticalAlignment != EVerticalAlignment.None) {
                sb.Append($" {GetVerticalAlignmentClass()}");
            }
            sb.Append("\">");
            sb.Append(Environment.NewLine);
            //Add items
            foreach (var item in _items) {
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