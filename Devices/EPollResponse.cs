using System;

namespace HomeSeer.PluginSdk.Devices {

    [Serializable]
    public enum EPollResponse {

        OK                  = 1,
        NotFound            = 2,
        ErrorGettingStatus  = 3,
        CouldNotReachPlugin = 4,
        Unknown             = 5,
        TimeoutOK           = 6,
        OtherError          = 7,
        StatusNotSupported  = 8

    }

}