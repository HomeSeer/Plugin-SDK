namespace HomeSeer.PluginSdk.Devices.Controls {

    public enum EControlUse {

        NotSpecified    = 0,
        On              = 1,
        Off             = 2,
        Dim             = 3,
        OnAlternate    = 4,
        Play            = 5, // media control devices
        Pause           = 6,
        Stop            = 7,
        Forward         = 8,
        Rewind          = 9,
        Repeat          = 10,
        Shuffle         = 11,
        HeatSetPoint    = 12, // these are added so we can support IFTTT for now until they change their API
        CoolSetPoint    = 13,
        ThermModeOff    = 14,
        ThermModeHeat   = 15,
        ThermModeCool   = 16,
        ThermModeAuto   = 17,
        DoorLock        = 18,
        DoorUnLock      = 19,
        ThermFanAuto    = 20,
        ThermFanOn      = 21,
        ColorControl    = 22,
        DimFan          = 23,
        MotionActive    = 24,
        MotionInActive  = 25,
        ContactActive   = 26,
        ContactInActive = 27,
        Mute            = 28,
        UnMute          = 29,
        MuteToggle      = 30,
        Next            = 31,
        Previous        = 32,
        Volume          = 33

    }

}