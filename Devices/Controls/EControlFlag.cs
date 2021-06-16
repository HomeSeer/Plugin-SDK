namespace HomeSeer.PluginSdk.Devices.Controls {

    /// <summary>
    /// Special configuration flags that adjust the behavior of a <see cref="StatusControl"/>.
    /// </summary>
    public enum EControlFlag : uint {
        
        /// <summary>
        /// The StatusControl should not show up as a status target for events.
        ///  IE it is only for creating control events
        /// <para>This is toggled by setting <see cref="StatusControl.IsInvalidStatusTarget"/> to <see langword="true"/></para>
        /// </summary>
        InvalidStatusTarget = 1,
        /// <summary>
        /// The StatusControl label includes additional data tokens to be replaced by strings
        ///  in <see cref="HsFeature.AdditionalStatusData"/>
        /// <para>This is toggled by setting <see cref="StatusControl.HasAdditionalData"/> to <see langword="true"/></para>
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