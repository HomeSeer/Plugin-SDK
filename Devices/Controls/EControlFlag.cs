namespace HomeSeer.PluginSdk.Devices.Controls {

    public enum EControlFlag : uint {
        
        /// <summary>
        /// The StatusControl should not show up as a status target for events.
        ///  IE it is only for creating control events
        /// </summary>
        InvalidStatusTarget = 1,
        /// <summary>
        /// The StatusControl label includes additional data tokens to be replaced by strings
        ///  in <see cref="HsFeature.AdditionalStatusData"/>
        /// </summary>
        HasAdditionalData = 2
        //8
        //16
        //32
        //64
        //128
        //256
    }

}