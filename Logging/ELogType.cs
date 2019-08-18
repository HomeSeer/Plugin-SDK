// ReSharper disable UnusedMember.Global
namespace HomeSeer.PluginSdk.Logging {

    /// <summary>
    /// Used to categorize log entries in increasing level of severity.
    /// </summary>
    /// <remarks>
    /// Users are able to filter the HomeSeer log by type based on what kind of information they are looking for.
    ///  Properly categorizing your log entries will make it easier for users and HomeSeer support staff to
    ///  troubleshoot any issues with the expected operation of your plugin.
    /// </remarks>
    public enum ELogType {

        /// <summary>
        /// Messages typically used for marking where the plugin process is in code.
        /// <para>
        /// The least severe and most verbose log type.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Use this type for all entries that fall outside of the scope of the other types.
        /// </remarks>
        Trace,
        /// <summary>
        /// Messages used for debugging the plugin process.
        /// </summary>
        Debug,
        /// <summary>
        /// Informational messages about the plugin process that require no attention but may be useful for users.
        /// </summary>
        Info,
        /// <summary>
        /// Messages indicating that there may be an issue interfering with the normal operation
        ///  of the plugin that the user might want to address.
        /// </summary>
        Warning,
        /// <summary>
        /// Critical messages about issues that require user attention and may be interfering with normal operation.
        /// <para>
        /// The most severe log type.
        /// </para>
        /// </summary>
        Error,

    }

}