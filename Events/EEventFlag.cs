namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
    ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
    ///  Please use with caution.
    /// </summary>
    public enum EEventFlag : uint {

        Enabled = 0x10,
        DeleteAfterTrigger = 0x20, 
        DoNotLog = 0x40,
        Delayed = 0x80,
        IncludeInPowerfail = 0x160,
        Security = 0x320,
        Priority = 0x640

    }

}