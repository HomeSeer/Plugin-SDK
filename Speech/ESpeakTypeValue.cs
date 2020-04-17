namespace HomeSeer.PluginSdk.Speech {

    /// <summary>
    /// PLEASE NOTE: Code related to the Speech components in HomeSeer were ported from the HS3 plugin API and
    ///  have not been fully tested to verify full functionality from the new SDK. The Speech API may undergo
    ///  significant changes in the near future. Please use with caution.
    /// </summary>
    public enum ESpeakTypeValue {
        /// <summary>
        /// TTS voice
        /// </summary>
        Tts      = 1,
        /// <summary>
        /// Text message
        /// </summary>
        Message  = 2,
        /// <summary>
        /// Play a wave file
        /// </summary>
        WaveFile = 3 
    }

}