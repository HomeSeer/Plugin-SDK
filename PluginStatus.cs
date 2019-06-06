using System;

namespace HomeSeer.PluginSdk {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class PluginStatus {

        public EPluginStatus Status     { get; set; }
        public string        StatusText { get; set; }

        public PluginStatus() { }

        public PluginStatus(EPluginStatus status) {
            Status = status;
            StatusText = "";
        }

        public PluginStatus(EPluginStatus status, string statusText) {
            Status = status;
            StatusText = statusText;
        }

        public static PluginStatus OK() {
            return new PluginStatus(EPluginStatus.Ok);
        }
        
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