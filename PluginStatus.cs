using System;
// ReSharper disable MemberCanBePrivate.Global

namespace HomeSeer.PluginSdk {

    /// <summary>
    /// Represents the current operational state of a HomeSeer plugin. 
    /// </summary>
    /// <remarks>
    /// This is the expected return type when HomeSeer calls a
    ///  plugin's implementation of <see cref="IPlugin.OnStatusCheck"/>
    /// </remarks>
    /// <example>
    /// <code>
    /// public void BeforeReturnStatus() {
    ///     //Analyze the current state of the plugin
    ///     //...
    ///     //The plugin is operating as expected
    ///     Status = PluginStatus.Ok();
    /// }
    /// </code>
    /// </example>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class PluginStatus {

        /// <summary>
        /// The status of the plugin as an <see cref="EPluginStatus"/>
        /// </summary>
        public EPluginStatus Status { get; }
        
        /// <summary>
        /// A detailed explanation of the status of the plugin
        /// </summary>
        public string StatusText { get; }

        /// <summary>
        /// Creates a new PluginStatus with the specified <see cref="EPluginStatus"/> and status text
        /// </summary>
        /// <param name="status">The <see cref="EPluginStatus"/> representing the current state of the plugin</param>
        /// <param name="statusText">A detailed explanation of the status</param>
        /// <exception cref="ArgumentNullException">Thrown if no status text was supplied when the status is not OK</exception>
        public PluginStatus(EPluginStatus status, string statusText) {
            if (status != EPluginStatus.Ok && string.IsNullOrWhiteSpace(statusText)) {
                throw new ArgumentNullException(nameof(statusText), "You must supply status text if the status is anything other than OK");
            }
            Status = status;
            StatusText = statusText;
        }

        /// <summary>
        /// Creates a new PluginStatus of OK
        /// </summary>
        /// <returns>A PluginStatus with a status of OK</returns>
        public static PluginStatus Ok() {
            return new PluginStatus(EPluginStatus.Ok, "");
        }
        
        /// <summary>
        /// Creates a new PluginStatus with the status of Info and the specified description
        /// </summary>
        /// <param name="statusText">A detailed explanation of the status</param>
        /// <returns>A PluginStatus with a status of Info</returns>
        public static PluginStatus Info(string statusText) {
            return new PluginStatus(EPluginStatus.Info, statusText);
        }
        
        /// <summary>
        /// Creates a new PluginStatus with the status of Warning and the specified description
        /// </summary>
        /// <param name="statusText">A detailed explanation of the status</param>
        /// <returns>A PluginStatus with a status of Warning</returns>
        public static PluginStatus Warning(string statusText) {
            return new PluginStatus(EPluginStatus.Warning, statusText);
        }
        
        /// <summary>
        /// Creates a new PluginStatus with the status of Critical and the specified description
        /// </summary>
        /// <param name="statusText">A detailed explanation of the status</param>
        /// <returns>A PluginStatus with a status of Critical</returns>
        public static PluginStatus Critical(string statusText) {
            return new PluginStatus(EPluginStatus.Critical, statusText);
        }
        
        /// <summary>
        /// Creates a new PluginStatus with the status of Fatal and the specified description
        /// </summary>
        /// <param name="statusText">A detailed explanation of the status</param>
        /// <returns>A PluginStatus with a status of Fatal</returns>
        public static PluginStatus Fatal(string statusText) {
            return new PluginStatus(EPluginStatus.Fatal, statusText);
        }
        
        /// <summary>
        /// An Enum representing the current state of a plugin
        /// </summary>
        public enum EPluginStatus {
            /// <summary>
            /// Everything is fine.
            /// </summary>
            Ok = 0,
            /// <summary>
            /// Information
            /// </summary>
            Info = 1,
            /// <summary>
            /// Something wrong, but should not affect operation.
            /// </summary>
            Warning = 2,
            /// <summary>
            /// Something wrong that will prevent operation from being successful.
            /// </summary>
            Critical = 3,
            /// <summary>
            /// Something really wrong and the plug-in cannot function.
            /// </summary>
            Fatal = 4
        }

    }

}