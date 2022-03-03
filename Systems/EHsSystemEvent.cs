using System;
using HomeSeer.PluginSdk.Devices;

namespace HomeSeer.PluginSdk.Systems {
    
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
        /// <summary>
        /// When the event log is written, this event is fired.
        /// Parameters are:
        /// <list type="table">
        ///     <listheader>
        ///         <term>Parameter index</term>
        ///         <description>Content</description>
        ///     </listheader>
        ///     <item>
        ///         <term>0</term>
        ///         <description>The event type as an <see cref="int"/> with a value of <see cref="Log"/></description>
        ///     </item>
        ///     <item>
        ///         <term>1</term>
        ///         <description>The date and time of the log entry as a <see cref="string"/></description>
        ///     </item>
        ///     <item>
        ///         <term>2</term>
        ///         <description>The type of the log entry as a <see cref="string"/></description> TODO switch to use ELogType
        ///     </item>
        ///     <item>
        ///         <term>3</term>
        ///         <description>The content of the log entry as a <see cref="string"/></description>
        ///     </item>
        ///     <item>
        ///         <term>4</term>
        ///         <description>The color of the log entry as a <see cref="string"/> formatted in HTML color format</description>
        ///     </item>
        ///     <item>
        ///         <term>5</term>
        ///         <description>DEPRECATED</description>
        ///     </item>
        ///     <item>
        ///         <term>6</term>
        ///         <description>The source of the log entry as a <see cref="string"/></description>
        ///     </item>
        ///     <item>
        ///         <term>7</term>
        ///         <description>The error code associated with the log entry as a <see cref="int"/></description>
        ///     </item>
        ///     <item>
        ///         <term>8</term>
        ///         <description>The date and time of the log entry as a <see cref="DateTime"/></description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <remarks>Parameter index 2 will be undergoing some changes to ensure it matches <see cref="Logging.ELogType"/></remarks>
        Log = 2,
        
        /// <summary>
        /// When audio is started or stopped, this event is fired.
        /// This includes playing an audio file or speaking TTS
        /// Parameters are:
        /// <list type="table">
        ///     <listheader>
        ///         <term>Parameter index</term>
        ///         <description>Content</description>
        ///     </listheader>
        ///     <item>
        ///         <term>0</term>
        ///         <description>The event type as an <see cref="int"/> with a value of <see cref="Audio"/></description>
        ///     </item>
        ///     <item>
        ///         <term>1</term>
        ///         <description>
        ///             A <see cref="bool"/> representing whether audio has started or stopped.
        ///             <see langword="True"/> means audio has started, and <see langword="false"/> means audio has stopped.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>2</term>
        ///         <description>The audio device number as an <see cref="int"/> or 0 if not using Windows PC audio.</description>
        ///     </item>
        ///     <item>
        ///         <term>3</term>
        ///         <description>The speaker host name as a <see cref="string"/> if audio is being sent to a client.</description>
        ///     </item>
        ///     <item>
        ///         <term>4</term>
        ///         <description>The speaker host instance as a <see cref="string"/> if audio is being sent to a client.</description>
        ///     </item> 
        /// </list>
        /// </summary>
        Audio = 8,
        
        /// <summary>
        /// When a change is made to the configuration of the system, such as Setup, this event is fired. 
        /// Parameters are:
        /// <list type="table">
        ///     <listheader>
        ///         <term>Parameter index</term>
        ///         <description>Content</description>
        ///     </listheader>
        ///     <item>
        ///         <term>0</term>
        ///         <description>The event type as an <see cref="int"/> with a value of <see cref="ConfigChange"/></description>
        ///     </item>
        ///     <item>
        ///         <term>1</term>
        ///         <description>The target of the config change event as defined by <see cref="EConfigChangeTarget"/> and represented as an <see cref="int"/></description>
        ///     </item>
        ///     <item>
        ///         <term>2</term>
        ///         <description>UNUSED - always 0</description>
        ///     </item>
        ///     <item>
        ///         <term>3</term>
        ///         <description>Target unique reference number as an <see cref="int"/></description>
        ///     </item>
        ///     <item>
        ///         <term>4</term>
        ///         <description>The type of change happening as defined by <see cref="EConfigChangeAction"/> and represented by an <see cref="int"/></description>
        ///     </item>
        ///     <item>
        ///         <term>5</term>
        ///         <description>A description of what changed as a <see cref="string"/></description>
        ///     </item>
        /// </list>
        /// </summary>
        ConfigChange = 0x20,

        /// <summary>
        /// When the string value of a device is changed, this event is fired.
        /// Parameters are:
        /// <list type="table">
        ///     <listheader>
        ///         <term>Parameter index</term>
        ///         <description>Content</description>
        ///     </listheader>
        ///     <item>
        ///         <term>0</term>
        ///         <description>The event type as an <see cref="int"/> with a value of <see cref="StringChange"/></description>
        ///     </item>
        ///     <item>
        ///         <term>1</term>
        ///         <description>The <see cref="AbstractHsDevice.Address"/> of the device that has changed</description>
        ///     </item>
        ///     <item>
        ///         <term>2</term>
        ///         <description>The new value as a <see cref="string"/></description>
        ///     </item>
        ///     <item>
        ///         <term>3</term>
        ///         <description>The <see cref="AbstractHsDevice.Ref"/> of the device that has changed</description>
        ///     </item>
        /// </list>
        /// </summary>
        StringChange = 0x40,

        /// <summary>
        /// When a new speaker client connects to the system, this event is fired.
        /// Parameters are:
        /// <list type="table">
        ///     <listheader>
        ///         <term>Parameter index</term>
        ///         <description>Content</description>
        ///     </listheader>
        ///     <item>
        ///         <term>0</term>
        ///         <description>The event type as an <see cref="int"/> with a value of <see cref="SpeakerConnect"/></description>
        ///     </item>
        ///     <item>
        ///         <term>1</term>
        ///         <description>The name of the speaker host as a <see cref="string"/></description>
        ///     </item>
        ///     <item>
        ///         <term>2</term>
        ///         <description>The instance of the speaker host as a <see cref="string"/></description>
        ///     </item>
        ///     <item>
        ///         <term>3</term>
        ///         <description>
        ///             An <see cref="int"/> representing what the speaker host is doing.
        ///             1 means the host is connecting.
        ///             2 means it is disconnecting.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>4</term>
        ///         <description>The IP address of the speaker host as a <see cref="string"/></description>
        ///     </item>
        /// </list>
        /// </summary>
        SpeakerConnect = 0x80,
        
        
        /// <summary>
        /// Deprecated, no longer supported
        /// </summary>
        [Obsolete("This event type is no longer supported")]
        CallerId       = 0x100,
        
        /// <summary>
        /// When the value of a device changes, this event is fired.
        /// Parameters are:
        /// <list type="table">
        ///     <listheader>
        ///         <term>Parameter index</term>
        ///         <description>Content</description>
        ///     </listheader>
        ///     <item>
        ///         <term>0</term>
        ///         <description>The event type as an <see cref="int"/> with a value of <see cref="ValueChange"/></description>
        ///     </item>
        ///     <item>
        ///         <term>1</term>
        ///         <description>The <see cref="AbstractHsDevice.Address"/> of the device that has changed</description>
        ///     </item>
        ///     <item>
        ///         <term>2</term>
        ///         <description>The new value as a <see cref="double"/></description>
        ///     </item>
        ///     <item>
        ///         <term>3</term>
        ///         <description>The old value as a <see cref="double"/></description>
        ///     </item>
        ///     <item>
        ///         <term>4</term>
        ///         <description>The <see cref="AbstractHsDevice.Ref"/> of the device that has changed</description>
        ///     </item>
        /// </list>
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
        /// <list type="table">
        ///     <listheader>
        ///         <term>Parameter index</term>
        ///         <description>Content</description>
        ///     </listheader>
        ///     <item>
        ///         <term>0</term>
        ///         <description>The event type as an <see cref="int"/> with a value of <see cref="ValueSet"/></description>
        ///     </item>
        ///     <item>
        ///         <term>1</term>
        ///         <description>The <see cref="AbstractHsDevice.Address"/> of the device that has been set</description>
        ///     </item>
        ///     <item>
        ///         <term>2</term>
        ///         <description>The new value as a <see cref="double"/></description>
        ///     </item>
        ///     <item>
        ///         <term>3</term>
        ///         <description>The old value as a <see cref="double"/></description>
        ///     </item>
        ///     <item>
        ///         <term>4</term>
        ///         <description>The <see cref="AbstractHsDevice.Ref"/> of the device that has been set</description>
        ///     </item>
        /// </list>
        /// P(0) = EHsSystemEvent.ValueSet
        /// P(1) = HSAddress of device that changed
        /// P(2) = New value being set (double)
        /// P(3) = Previous value (double)
        /// P(4) = Device reference number
        /// </summary>        
        ValueSet = 0x800,
        
        /// <summary>
        /// Deprecated, no longer supported
        /// </summary>
        [Obsolete("This event type is no longer supported")]
        VoiceRec          = 0x1000,

        /// <summary>
        /// When an item in setup is changed, this event is fired. The parameters note the settings.ini file values that are set.
        /// Parameters are:
        /// <list type="table">
        ///     <listheader>
        ///         <term>Parameter index</term>
        ///         <description>Content</description>
        ///     </listheader>
        ///     <item>
        ///         <term>0</term>
        ///         <description>The event type as an <see cref="int"/> with a value of <see cref="SetupChange"/></description>
        ///     </item>
        ///     <item>
        ///         <term>1</term>
        ///         <description>The INI section as a <see cref="string"/></description>
        ///     </item>
        ///     <item>
        ///         <term>2</term>
        ///         <description>The INI key as a <see cref="string"/></description>
        ///     </item>
        ///     <item>
        ///         <term>3</term>
        ///         <description>The new INI value as a <see cref="string"/></description>
        ///     </item>
        /// </list>
        /// P(0) = EHsSystemEvent.SetupChange
        /// P(1) = INI section
        /// P(2) = INI key
        /// P(3) = INI value
        /// </summary>
        SetupChange = 0x2000,

        /// <summary>
        /// If a script includes a callback script, this event is fired before the script is run.
        /// Note that this event does not include event ID in P(0).
        /// Parameters are:
        /// <list type="table">
        ///     <listheader>
        ///         <term>Parameter index</term>
        ///         <description>Content</description>
        ///     </listheader>
        ///     <item>
        ///         <term>0</term>
        ///         <description>The <see cref="AbstractHsDevice.Code"/> of the device that has been changed</description>
        ///     </item>
        ///     <item>
        ///         <term>1</term>
        ///         <description>The <see cref="AbstractHsDevice.Address"/> of the device that has been changed</description>
        ///     </item>
        ///     <item>
        ///         <term>2</term>
        ///         <description>The new value as a <see cref="double"/></description>
        ///     </item>
        ///     <item>
        ///         <term>3</term>
        ///         <description>The old value as a <see cref="double"/></description>
        ///     </item>
        ///     <item>
        ///         <term>4</term>
        ///         <description>The <see cref="AbstractHsDevice.Ref"/> of the device that has been changed</description>
        ///     </item>
        /// </list>
        /// P(0) = Device HSCode
        /// P(1) = Device HSAddress
        /// P(2) = New device value being set (double)
        /// P(3) = Previous device value (double)
        /// P(4) = Device reference number
        /// </summary>
        /// <remarks>The behavior of this event callback is not clear and may be revised.</remarks>
        ValueStringChangeRunScript = 0x4000,

        /// <summary>
        /// If a generic event is registered, this event is fired when the event is triggered.
        /// The event is typically triggered by a plugin. Parameter 0 is EHsSystemEvent.Generic.
        /// The remaining parameters are defined by the event that is registered.
        /// </summary>
        Generic = 0x8000

    }

}