namespace HomeSeer.PluginSdk.Systems {
    /// <summary>
    /// The target of a <see cref="EHsSystemEvent.ConfigChange"/> system event
    /// </summary>
    public enum EConfigChangeTarget {
        /// <summary>
        /// A device is being changed
        /// </summary>
        Device = 0,
        /// <summary>
        /// An event is being changed
        /// </summary>
        Event = 1,
        /// <summary>
        /// An event group is being changed
        /// </summary>
        EventGroup = 2,
        /// <summary>
        /// A setup item is being changed
        /// </summary>
        SetupItem = 3
    }
}