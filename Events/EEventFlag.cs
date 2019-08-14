namespace HomeSeer.PluginSdk.Events {

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