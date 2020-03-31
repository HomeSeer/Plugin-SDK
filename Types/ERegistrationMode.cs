namespace HomeSeer.PluginSdk.Types {

    /// <summary>
    /// The registration mode of a plugin
    /// </summary>
    public enum ERegistrationMode {
        
        /// <summary>
        /// The registration mode is not known
        /// </summary>
        Unknown           = 0,
        /// <summary>
        /// The plugin is unregistered
        /// </summary>
        Unregistered             = 1,
        /// <summary>
        /// The plugin is running as a trial
        /// </summary>
        Trial             = 2,
        /// <summary>
        /// The plugin is registered
        /// </summary>
        Registered        = 3,
        /// <summary>
        /// For HSPRO, plugin does not have a license and needs to be enabled to get one
        /// </summary>
        ReadyToRegister = 4

    }

}