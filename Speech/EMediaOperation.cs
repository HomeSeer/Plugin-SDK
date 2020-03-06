namespace HomeSeer.PluginSdk.Speech {

    /// <summary>
    /// PLEASE NOTE: Code related to the Speech components in HomeSeer were ported from the HS3 plugin API and
    ///  have not been fully tested to verify full functionality from the new SDK. The Speech API may undergo
    ///  significant changes in the near future. Please use with caution.
    /// </summary>
    public enum EMediaOperation {

        MediaStop         = 1,
        MediaPause        = 2,
        MediaMute         = 3,
        /// <summary>
        /// Return true if media player is currently playing
        /// </summary>
        MediaIsPlaying    = 4,
        /// <summary>
        /// 0 -> 100 100=full
        /// </summary>
        MediaSetVolume    = 5,
        MediaGetVolume    = 6,
        MediaGetFilename  = 7,
        MediaSetFilename  = 8,
        MediaUnpause      = 9,
        MediaUnmute       = 10

    }

}