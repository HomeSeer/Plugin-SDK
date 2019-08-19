namespace HomeSeer.PluginSdk.Devices.Identification {

    /// <summary>
    /// An enum describing the type of relationship a device/feature has with the device or features listed in
    ///  its associated devices list.
    /// </summary>
    public enum ERelationship {

        /// <summary>
        /// No relationship type has been specified
        /// </summary>
        NotSet = 0,
        /// <summary>
        /// This item is a device and is associated with one or more features.
        /// <para>
        /// Previously called "ParentRoot" in the legacy API
        /// </para>
        /// </summary>
        Device = 2,
        //Standalone   = 3,
        /// <summary>
        /// This item is a feature and is associated with one device that owns it.
        /// <para>
        /// Previously called "Child" in the legacy API
        /// </para>
        /// </summary>
        Feature = 4

    }

}