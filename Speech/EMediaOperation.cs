namespace HomeSeer.PluginSdk.Speech {

    public enum EMediaOperation {

        MediaStop         = 1,
        MediaPause        = 2,
        MediaMute         = 3,
        MediaIsPlaying    = 4, // returns true if media player is currently playing
        MediaSetVolume    = 5, // 0 -> 100  100=FULL
        MediaGetVolume    = 6,
        MediaGetFilename  = 7,
        MediaSetFilename  = 8,
        MediaUnpause      = 9,
        MediaUnmute       = 10

    }

}