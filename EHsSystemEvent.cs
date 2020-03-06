namespace HomeSeer.PluginSdk {
    
    public enum EHsSystemEvent {
        // For HSEvent callbacks
        
        // X10 = 1
        Log = 2,
        // STATUS_CHANGE = 4       ' a device changed status or value
        Audio = 8,
        // X10_TRANSMIT = &H10S
        ConfigChange   = 0x20,
        StringChange   = 0x40,
        SpeakerConnect = 0x80,
        CallerId       = 0x100,
        // ZWAVE = &H200
        ValueChange = 0x400, // rjh added in 2.2.0.3
        /// <summary>
        /// A device has been deleted
        /// </summary>
        DeleteDevice = 0x600,
        ValueSet    = 0x800, // rjh added in 3.0.0.292
        // STATUS_ONLY_CHANGE = &H800 ' rjh added in 2.2.0.10
        VoiceRec          = 0x1000,
        SetupChange       = 0x2000,
        RunScriptSpecial = 0x4000,
        Generic            = 0x8000 // RVCT added 3/10/08, 2.2.0.76ish

    }

}