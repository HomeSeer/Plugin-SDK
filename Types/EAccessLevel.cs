using System;

namespace HomeSeer.PluginSdk.Types {

    public enum EAccessLevel {

        /// <summary>
        /// Plug-in is not licensed and may be enabled and run without purchasing a license. Use this value for free plug-ins.
        /// </summary>
        LicenseNotRequired = 1,
        /// <summary>
        /// Plug-in is licensed and a user must purchase a license in order to use this plug-in. When the plug-in Is first enabled, it will will run as a trial for 30 days.
        /// </summary>
        RequiresLicense    = 2,

        [Obsolete("Reserved for internal compatibility management. Do not use.", true)]
        DoNotUse = 3,

        AcceptsHs3License = 4

    }

}