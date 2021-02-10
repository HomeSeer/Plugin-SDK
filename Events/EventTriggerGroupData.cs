using System;

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
    ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
    ///  Please use with caution.
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public struct EventTriggerGroupData {

        public int      GroupNumber;
        public string[] Triggers;

    }

}