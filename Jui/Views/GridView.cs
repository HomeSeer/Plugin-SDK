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
    /// A grid view defines a sub-grouping of views with a header.
    /// It uses the <a href="https://mdbootstrap.com/docs/standard/layout/grid/">bootstrap grid system</a> to layout the views within the group
    /// </summary>
    public class GridView : ViewGroup
    {

        #region Properties

        /// <summary>
        /// The rows to display within this group.
        /// </summary>
        [JsonProperty("rows")]
        private List<GridRow> _rows;

        #endregion

        #region Constructors

        /// <inheritdoc cref="ViewGroup"/>
        /// <summary>
        /// Create a new instance of a grid view with an ID
        /// </summary>
        /// <param name="id">The unique ID of the group</param>
        [JsonConstructor]
        protected GridView(string id) : base(id)
        {
            _rows = new List<GridRow>();
        }

        /// <inheritdoc cref="ViewGroup"/>
        /// <summary>
        /// Create a new instance of a grid view with an ID and Name
        /// </summary>
        /// <param name="id">The unique ID of the group</param>
        /// <param name="name">The name of the group. DEFAULT: null</param>
        public GridView(string id, string name = null) : base(id, name)
        {
            _rows = new List<GridRow>();
        }

        #endregion

        #region Views

        #region Create/Add

        /// <inheritdoc cref="ViewCollectionHelper.AddView"/>
        /// <summary>
        /// Add a view to the grid
        /// The view will be aded as a single row. 
        /// </summary>
        public override void AddView(AbstractView view)
        {
            var row = new GridRow();
            row.AddItem(view);
            AddRow(row);
        }

        /// <inheritdoc cref="ViewCollectionHelper.AddViews"/>
        /// <summary>
        /// Add multiple views to the grid
        /// The views will be added as stacked rows
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the specified collection of views is null</exception>
        public override void AddViews(IEnumerable<AbstractView> views)
        {
            if (views == null)
            {
                throw new ArgumentNullException(nameof(views));
            }
            foreach (var view in views)
            {
                AddView(view);
            }
        }

        /// <inheritdoc cref="ViewCollectionHelper.SetViews"/>
        /// <summary>
        /// Set the list of views in this group
        /// </summary>
        public override void SetViews(IEnumerable<AbstractView> views)
        {
            RemoveAllViews();
            AddViews(views);
        }

        /// <summary>
        /// Add a row to the group
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the specified row is null</exception>
        public void AddRow(GridRow row)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }
            _rows.Add(row);
            base.AddViews(row.Views);
        }

        #endregion

        #region Delete

        /// <inheritdoc cref="ViewGroup.RemoveAllViews"/>
        public override void RemoveAllViews()
        {
            base.RemoveAllViews();
            _rows.Clear();
        }

        #endregion

        #endregion

        /// <inheritdoc cref="AbstractView.ToHtml"/>
        public override string ToHtml(int indent = 0)
        {
            var sb = new StringBuilder();
            sb.Append(GetIndentStringFromNumber(indent));
            //Open the containing div
            sb.Append("<div id=\"");
            sb.Append(Id);
            sb.Append("\" class=\"jui-view jui-group\">");
            sb.Append(Environment.NewLine);
            //Add the title
            sb.Append(GetIndentStringFromNumber(indent + 1));
            sb.Append("<div id=\"");
            sb.Append(Id);
            sb.Append(".title\" class=\"jui-title\">");
            sb.Append("<small>");
            sb.Append(Name);
            sb.Append("</small>");
            sb.Append("</div>");
            sb.Append(Environment.NewLine);
            //Add rows
            foreach (var row in _rows)
            {
                sb.Append(row.ToHtml(indent + 1));
            }

            //Close the containing div
            sb.Append(GetIndentStringFromNumber(indent));
            sb.Append("</div>");
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }

    }

}