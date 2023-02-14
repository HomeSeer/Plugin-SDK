namespace HomeSeer.Jui.Views {

    /// <summary>
    /// The possible behaviors for the horizontal alignment of items in a row 
    /// </summary>
    public enum EHorizontalAlignment {
        /// <summary>
        /// None
        /// </summary>
        None = -1,
        /// <summary>
        /// Items are packed towards the start of the row
        /// </summary>
        JustifyContentStart = 0,
        /// <summary>
        /// Items are packed towards the end of the row
        /// </summary>
        JustifyContentEnd = 1,
        /// <summary>
        /// Items are centered along the row
        /// </summary>
        JustifyContentCenter = 2,
        /// <summary>
        /// Items are evenly distributed in the row with equal space around them
        /// </summary>
        JustifyContentAround = 3,
        /// <summary>
        /// Items are evenly distributed in the row; first item is on the start of the row, last item on the end of the row
        /// </summary>
        JustifyContentBetween = 4,
    }

}