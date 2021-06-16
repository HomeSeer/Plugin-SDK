namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// <see cref="HsDevice"/> and <see cref="HsFeature"/> property types. Used for communicating device/feature
    ///  related changed to HomeSeer.
    /// </summary>
    public enum EProperty {
        /// <summary>
        /// <see cref="AbstractHsDevice.Ref"/>
        /// </summary>
        Ref                       = 0,
        /// <summary>
        /// <see cref="HsFeature.AdditionalStatusData"/>
        /// </summary>
        AdditionalStatusData      = 1,
        /// <summary>
        /// <see cref="AbstractHsDevice.Address"/>
        /// </summary>
        Address                   = 2,
        /// <summary>
        /// An abstraction of the <see cref="EMiscFlag.CanDim"/> flag that is stored in <see cref="AbstractHsDevice.Misc"/>
        /// </summary>
        CanDim                    = 8,
        /// <summary>
        /// <see cref="AbstractHsDevice.TypeInfo"/>
        /// </summary>
        DeviceType                = 10,
        /// <summary>
        /// <see cref="AbstractHsDevice.Image"/>
        /// </summary>
        Image                     = 12,
        /// <summary>
        /// <see cref="AbstractHsDevice.Interface"/>
        /// </summary>
        Interface                 = 14,
        /// <summary>
        /// <see cref="AbstractHsDevice.LastChange"/>
        /// </summary>
        LastChange                = 16,
        /// <summary>
        /// <see cref="AbstractHsDevice.Location"/>
        /// </summary>
        Location                  = 17,
        /// <summary>
        /// <see cref="AbstractHsDevice.Location2"/>
        /// </summary>
        Location2                 = 18,
        /// <summary>
        /// <see cref="AbstractHsDevice.Name"/>
        /// </summary>
        Name                      = 21,
        /// <summary>
        /// <see cref="AbstractHsDevice.PlugExtraData"/>
        /// </summary>
        PlugExtraData             = 23,
        /// <summary>
        /// <see cref="AbstractHsDevice.Relationship"/>
        /// </summary>
        Relationship              = 24,
        //ScriptName                = 25,
        //ScriptFunc                = 26,
        //StringSelected            = 29,
        /// <summary>
        /// <see cref="AbstractHsDevice.UserNote"/>
        /// </summary>
        UserNote                  = 30,
        /// <summary>
        /// <see cref="AbstractHsDevice.UserAccess"/>
        /// </summary>
        UserAccess                = 31,
        /// <summary>
        /// <see cref="AbstractHsDevice.VoiceCommand"/>
        /// </summary>
        VoiceCommand              = 33,
        /// <summary>
        /// <see cref="AbstractHsDevice.AssociatedDevices"/>
        /// </summary>
        AssociatedDevices         = 34,
        /// <summary>
        /// <see cref="AbstractHsDevice.Misc"/>
        /// </summary>
        Misc                      = 35,
        /// <summary>
        /// <see cref="AbstractHsDevice.Status"/>
        /// </summary>
        Status                    = 36,
        /// <summary>
        /// <see cref="HsFeature.StatusControls"/>
        /// </summary>
        StatusControls            = 37,
        /// <summary>
        /// <see cref="HsFeature.StatusGraphics"/>
        /// </summary>
        StatusGraphics            = 38,
        /// <summary>
        /// <see cref="AbstractHsDevice.Value"/>
        /// </summary>
        Value                     = 39,
        /// <summary>
        /// <see cref="AbstractHsDevice.IsValueInvalid"/>
        /// </summary>
        InvalidValue              = 40,
        /// <summary>
        /// <see cref="HsDevice.FeatureDisplayPriority"/>
        /// </summary>
        FeatureDisplayPriority    = 41,
        /// <summary>
        /// <see cref="HsFeature.DisplayType"/>
        /// </summary>
        FeatureDisplayType        = 42

    }

}