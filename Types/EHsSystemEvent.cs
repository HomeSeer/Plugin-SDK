namespace HomeSeer.PluginSdk.Types {
    
    /// <summary>
    /// Types of system events.
    /// </summary>
    /// <remarks>
    /// <para>
    /// System events represent changes in the system that plugins can react to.
    ///  They are not the same thing as Events configured by users.
    /// </para>
    /// <para>
    /// This replaces <see cref="Constants.HSEvent"/>
    /// </para>
    /// </remarks>
    public enum EHsSystemEvent {
        // For HSEvent callbacks
        
        // X10 = 1
        
        //TODO document Log
        /// <summary>
        /// 
        /// </summary>
        Log = 2,
        
        // STATUS_CHANGE = 4       ' a device changed status or value
        
        //TODO document Audio
        /// <summary>
        /// 
        /// </summary>
        Audio = 8,
        
        // X10_TRANSMIT = &H10S
        
        //TODO document ConfigChange
        /// <summary>
        /// 
        /// </summary>
        ConfigChange   = 0x20,
        
        //TODO document StringChange
        /// <summary>
        /// 
        /// </summary>
        StringChange   = 0x40,
        
        //TODO document SpeakerConnect
        /// <summary>
        /// 
        /// </summary>
        SpeakerConnect = 0x80,
        
        //TODO document CallerId
        /// <summary>
        /// 
        /// </summary>
        CallerId       = 0x100,
        
        // ZWAVE = &H200
        
        //TODO document ValueChange
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// rjh added in 2.2.0.3
        /// </remarks>
        ValueChange = 0x400,
        
        /// <summary>
        /// A device has been deleted
        /// </summary>
        DeleteDevice = 0x600,
        
        //TODO document ValueSet
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// rjh added in 3.0.0.292
        /// </remarks>
        ValueSet    = 0x800,
        
        // STATUS_ONLY_CHANGE = &H800 ' rjh added in 2.2.0.10
        
        //TODO document VoiceRec
        /// <summary>
        /// 
        /// </summary>
        VoiceRec          = 0x1000,
        
        //TODO document SetupChange
        /// <summary>
        /// 
        /// </summary>
        SetupChange       = 0x2000,
        
        //TODO document RunScriptSpecial
        /// <summary>
        /// 
        /// </summary>
        RunScriptSpecial = 0x4000,
        
        //TODO document Generic
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// RVCT added 3/10/08, 2.2.0.76ish
        /// </remarks>
        Generic            = 0x8000

    }

}