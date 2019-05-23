namespace HomeSeer.PluginSdk {

    public interface IAppCallbackAPI {

        double APIVersion { get; }
        int    CheckRegistrationStatus(string piname);

        void RegisterProxySpeakPlug(string   PIName, string PIInstance);
        void UnRegisterProxySpeakPlug(string PIName, string PIInstance);

        void RegisterGenericEventCB(string   GenericType, string   PIName, string PIInstance);
        void UnRegisterGenericEventCB(string GenericType, string   PIName, string PIInstance);
        void RaiseGenericEventCB(string      GenericType, object[] Parms,  string PIName, string PIInstance);

        void RegisterEventCB(Constants.HSEvent evType, string PIName, string PIInstance);

        void RegisterConfigLink(WebPageDesc cbo);
        void RegisterLink(WebPageDesc       cbo);
        void UnRegisterAllLinks(string      PIName);

        TrigActInfo[] TriggerMatches(string     Plug_Name, int         TrigID,    int SubTrig);             // new
        TrigActInfo[] TriggerMatchesInst(string Plug_Name, string      Plug_Inst, int TrigID, int SubTrig); // new
        void          TriggerFire(string        Plug_Name, TrigActInfo TrigInfo);                           // new

        TrigActInfo[] GetTriggers(string     PIName);
        TrigActInfo[] GetTriggersInst(string Plug_Name, string Plug_Inst);
        TrigActInfo[] GetActions(string      PIName);
        TrigActInfo[] GetActionsInst(string  Plug_Name, string Plug_Inst);

        string UpdatePlugAction(string PlugName, int evRef, TrigActInfo ActionInfo);

        System.Collections.SortedList GetLocationsList();
        System.Collections.SortedList GetLocations2List();

        void ConfigPageCommandsAdd(string key, string value);
        void ConfigDivToUpdateAdd(string  key, string value);
        void ConfigPropertySetAdd(string  key, string value);

    }

}