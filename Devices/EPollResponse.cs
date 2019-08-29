using System;

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// Possible responses to the <see cref="IPlugin.UpdateStatusNow"/> call made by HomeSeer to get the latest
    ///  status for a device/feature.
    /// </summary>
    [Serializable]
    public enum EPollResponse {

        /// <summary>
        /// The device/feature is fully operational.
        /// </summary>
        Ok                  = 1,
        /// <summary>
        /// The hardware backing the device/feature was not found.
        /// </summary>
        NotFound            = 2,
        /// <summary>
        /// There was a problem retrieving the latest status for the device/feature.
        /// </summary>
        ErrorGettingStatus  = 3,
        /// <summary>
        /// The plugin did not respond to the request.
        /// </summary>
        CouldNotReachPlugin = 4,
        /// <summary>
        /// The device/feature did not respond in an appropriate amount of time to determine its current state.
        /// </summary>
        Timeout             = 6,
        /// <summary>
        /// Some error occured while trying to complete the request preventing any state from being determined.
        /// </summary>
        UnknownError        = 7,
        /// <summary>
        /// The device/feature does not support a displayed status and is only used for controls.
        /// </summary>
        StatusNotSupported  = 8

    }

}