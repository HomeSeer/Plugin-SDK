namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// <para> NOTE - THIS IS PREVIEW MATERIAL AND WILL NOT FUNCTION UNTIL HS v4.2.0.0 </para>
    /// <para>
    /// The display type for a <see cref="HsFeature"/>.
    /// This controls the way a feature and its controls are displayed in the HomeSeer UI.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>While in grid view:</para>
    /// <para>
    /// When there are sibling features set as <see cref="Important"/>, <see cref="Normal"/> features will be shown in the status bar.
    /// When there are no sibling features set as <see cref="Important"/>, all <see cref="Normal"/> features will be analyzed
    ///  to determine a single <see cref="Important"/> feature to display.
    /// </para>
    /// <para>While in list view:</para>
    /// <para>
    /// <see cref="HsFeature"/>s are displayed <see cref="Important"/> first and then <see cref="Normal"/>.
    /// </para>
    /// <para> The term 'sibling features' refers to features that are a part of the same device. </para>
    /// </remarks>
    public enum EFeatureDisplayType {

        /// <summary>
        /// Do not apply any special display behavior to the <see cref="HsFeature"/>.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Prioritize the display of this <see cref="HsFeature"/> and its controls.
        /// </summary>
        Important = 1,
        /// <summary>
        /// Hide the <see cref="HsFeature"/> in grid view and list view.
        /// </summary>
        Hidden = 2
    }

}