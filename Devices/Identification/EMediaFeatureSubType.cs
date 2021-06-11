namespace HomeSeer.PluginSdk.Devices.Identification {

    /// <summary>
    /// The specific use of a <see cref="EFeatureType.Media"/> <see cref="HsFeature"/>
    /// </summary>
    /// <remarks>
    /// <para>This has not been fully migrated from the legacy API. Expect future changes.</para>
    /// </remarks>
    /// <seealso cref="TypeInfo.SubType"/>
    /// <see cref="EFeatureType.Media"/>
    public enum EMediaFeatureSubType {
        //JLW TODO Document EMediaFeatureSubType members
        PlayerStatus = 1,
        PlayerStatusAdditional = 2,
        PlayerControl = 3,
        PlayerVolume = 4,
        PlayerShuffle = 5,
        PlayerRepeat = 6,
        MediaGenre = 7,
        MediaAlbum = 8,
        MediaArtist = 9,
        MediaTrack = 10,
        MediaPlaylist = 11,
        MediaType = 12,
        PlayerMute = 13,
        PlayerAlbumArt = 14,
        MediaSelectorControl = 20 // Used to track which instance of MusicAPI and selection mode (e.g. album, artists, playlists)

    }

}