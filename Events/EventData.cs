using System;

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
    ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
    ///  Please use with caution.
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public struct EventData {

        public int                        Event_Ref;
        public string                     Event_Name;
        public string                     Event_Type;
        public int                        GroupID;
        public string                     GroupName;
        public string                     UserNote;
        public DateTime                   Last_Triggered;
        public TimeSpan                   Retrigger_Delay;
        public bool                       Flag_Enabled;
        public bool                       Flag_Delete_After_Trigger;
        public bool                       Flag_Do_Not_Log;
        public bool                       Flag_Delayed_Event;
        public bool                       Flag_Include_in_Powerfail;
        public bool                       Flag_Security;
        public bool                       Flag_Priority_Event;
        /// <summary>
        /// Event can be controlled by voice using the event name or the voice command override, must be set if Amazon or Google is set
        /// </summary>
        public bool FlagVoiceCommand;
        /// <summary>
        /// Event can be discovered by Google Home
        /// </summary>
        public bool FlagGoogleDiscoveryEnabled;
        /// <summary>
        /// Event can be discovered by Amazon alexa
        /// </summary>
        public bool FlagAlexaDiscoveryEnabled;
        public int                        Action_Count;
        public string[]                   Actions;
        public int                        Trigger_Count;
        public int                        Trigger_Group_Count;
        public EventTriggerGroupData[]    Trigger_Groups;

    }

}