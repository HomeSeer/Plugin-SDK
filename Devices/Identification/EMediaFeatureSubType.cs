namespace HomeSeer.PluginSdk.Devices.Identification {

    public enum EMediaFeatureSubType {

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
    
    /*
      Player_Status = 1
      Player_Status_Additional = 2
      Player_Control = 3
      Player_Volume = 4
      Player_Shuffle = 5
      Player_Repeat = 6
      Media_Genre = 7
      Media_Album = 8
      Media_Artist = 9
      Media_Track = 10
      Media_Playlist = 11
      Media_Type = 12
      Media_Selector_Control = 20 ' Used to track which instance of MusicAPI and selection mode (e.g. album, artists, playlists)
    */

}