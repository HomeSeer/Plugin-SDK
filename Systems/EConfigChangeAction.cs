namespace HomeSeer.PluginSdk.Systems {
    /// <summary>
    /// The action that is occuring during a <see cref="EHsSystemEvent.ConfigChange"/> system event
    /// </summary>
    public enum EConfigChangeAction {
        /// <summary>
        /// The action type is unknown or unspecified
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The <see cref="EConfigChangeTarget"/> is being added
        /// </summary>
        Add = 1,
        /// <summary>
        /// The <see cref="EConfigChangeTarget"/> is being deleted
        /// </summary>
        Delete = 2,
        /// <summary>
        /// The <see cref="EConfigChangeTarget"/> is being edited
        /// </summary>
        Change = 3
    }
}