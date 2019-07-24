using System;

namespace HomeSeer.PluginSdk.Events {

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
        public int                        Action_Count;
        public string[]                   Actions;
        public int                        Trigger_Count;
        public int                        Trigger_Group_Count;
        public EventTriggerGroupData[]    Trigger_Groups;

    }

}