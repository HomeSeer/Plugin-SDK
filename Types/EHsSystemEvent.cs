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

        /// <summary>
        /// When the event log is written, this event is fired.
        /// Parameters are:
        /// P(0) = EHsSystemEvent.LOG
        /// P(1) = Date and time of log entry
        /// P(2) = Log class
        /// P(3) = Log message
        /// P(4) = Color as HTML color format
        /// P(5) = Pri (deprecated)
        /// P(6) = Name of source
        /// P(7) = ErrorCode
        /// P(8) = Date and time of log entry as DateTime object
        /// </summary>
        Log = 2,

        // STATUS_CHANGE = 4       ' a device changed status or value

        /// <summary>
        /// When audio is started or stopped, this event is fired.
        /// This includes playing an audio file or speaking TTS
        /// Parameters are:
        /// P(0) = EHsSystemEvent.Audio
        /// P(1) = True=audio started, False=audio has stopped
        /// P(2) = Windows audio device if using PC audio, 0 otherwise
        /// P(3) = If audio is being sent to a speaker client, this is speaker host name
        /// P(4) = If audio is being sent to a speaker client, this is speaker host instance
        /// </summary>
        Audio = 8,

        // X10_TRANSMIT = &H10S

        /// <summary>
        /// When a change is made to the configuration of the system, such as Setup, this event is fired. 
        /// Parameters are:
        /// P(0) = EHsSystemEvent.ConfigChange
        /// P(1) = ConfigChangeType
        ///        change_device = 0
        ///        change_event = 1
        ///        change_event_group = 2
        ///        change_setup_item = 3
        /// P(2) = id (not used, always 0)
        /// P(3) = Device or event reference number
        /// P(4) = Delete_Add_Change
        ///        Unknown = 0
        ///        Add = 1
        ///        Delete = 2
        ///        Change = 3
        /// P(5) = Description of what changed
        /// </summary>
        ConfigChange = 0x20,

        /// <summary>
        /// When the string value of a device is changed, this event is fired.
        /// Parameters are:
        /// P(0) = EHsSystemEvent.StringChange
        /// P(1) = HSAddress of the device that changed
        /// P(2) = New string value
        /// P(3) = Device reference number of the device that changed
        /// </summary>
        StringChange = 0x40,

        /// <summary>
        /// When a new speaker client connects to the system, this event is fired.
        /// Parameters are:
        /// P(0) = EHsSystemEvent.SpeakerConnect
        /// P(1) = Speaker host name
        /// P(2) = Speaker host instance
        /// P(3) = 1=Speaker host is connecting, 0=disconnecting
        /// P(4) = IP address of speaker host
        /// </summary>
        SpeakerConnect = 0x80,
        
        
        /// <summary>
        /// Deprecated, no longer supported
        /// </summary>
        CallerId       = 0x100,

        // ZWAVE = &H200

        /// <summary>
        /// When the value of a device changes, this event is fired.
        /// Parameters are:
        /// P(0) = EHsSystemEvent.ValueChange
        /// P(1) = HSAddress of device that changed
        /// P(2) = New value being set (double)
        /// P(3) = Previous value (double)
        /// P(4) = Device reference number
        /// </summary>        
        ValueChange = 0x400,
        
        /// <summary>
        /// A device has been deleted
        /// </summary>
        DeleteDevice = 0x600,

        /// <summary>
        /// When a device has it's value set, this event is fired.
        /// This will fire even if the device is being set to the same value.
        /// Parameters are:
        /// P(0) = EHsSystemEvent.ValueSet
        /// P(1) = HSAddress of device that changed
        /// P(2) = New value being set (double)
        /// P(3) = Previous value (double)
        /// P(4) = Device reference number
        /// </summary>        
        ValueSet = 0x800,
        
        // STATUS_ONLY_CHANGE = &H800 ' rjh added in 2.2.0.10
        
        /// <summary>
        /// Deprecated, no longer supported
        /// </summary>
        VoiceRec          = 0x1000,

        /// <summary>
        /// When an item in setup is changed, this event is fired. The parameters note the settings.ini file values that are set.
        /// Parameters are:
        /// P(0) = EHsSystemEvent.SetupChange
        /// P(1) = INI section
        /// P(2) = INI key
        /// P(3) = INI value
        /// </summary>
        SetupChange = 0x2000,

        /// <summary>
        /// If a script includes a callback script, this event is fired before the script is run.
        /// Note that this event does not include event ID in P(0).
        /// P(0) = Device HSCode
        /// P(1) = Device HSAddress
        /// P(2) = New device value being set (double)
        /// P(3) = Previous device value (double)
        /// P(4) = Device reference number
        /// </summary>
        RunScriptSpecial = 0x4000,

        /// <summary>
        /// If a generic event is registered, this event is fired when the event is triggered.
        /// The event is typically triggered by a plugin. Parameter 0 is EHsSystemEvent.Generic.
        /// The remaining parameters are defined by the event that is registered.
        /// </summary>
        Generic = 0x8000

    }

}