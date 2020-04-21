using System;

namespace HomeSeer.PluginSdk.Types {

    /// <summary>
    /// License requirement modes for plugins. Set <see cref="IPlugin.AccessLevel"/> to the integer value corresponding to the desired level.
    /// </summary>
    public enum EAccessLevel {

        /// <summary>
        /// Plug-in is not licensed and may be enabled and run without purchasing a license. Use this value for free plug-ins.
        /// </summary>
        LicenseNotRequired = 1,
        /// <summary>
        /// Plug-in is licensed and a user must purchase a license in order to use this plug-in. When the plug-in Is first enabled, it will will run as a trial for 30 days.
        /// </summary>
        RequiresLicense    = 2,
        /// <summary>
        /// Reserved for internal compatibility management. Do not use.
        /// </summary>
        [Obsolete("Reserved for internal compatibility management. Do not use.", true)]
        DoNotUse = 3,
        /// <summary>
        /// Plug-in is licensed and a user must purchase a license in order to use this plug-in. When the plug-in Is first enabled, it will will run as a trial for 30 days. Legacy (HS3) plugin licenses are also accepted.
        /// </summary>
        AcceptsLegacyLicense = 4

    }

}