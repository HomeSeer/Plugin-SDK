using System;
using System.Collections.Generic;
using HomeSeer.PluginSdk.Devices;
using HomeSeer.PluginSdk.Devices.Controls;
using HomeSeer.PluginSdk.Energy;
using HomeSeer.PluginSdk.Events;
using HomeSeer.PluginSdk.Logging;
using HSCF.Communication.ScsServices.Service;
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace HomeSeer.PluginSdk {

    /// <summary>
    /// The interface used by plugins to communicate with the HomeSeer software
    /// <para>
    /// An instance of this interface is automatically provided to an AbstractPlugin when AbstractPlugin.Connect(string[]) is called.
    /// </para>
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [ScsService]
    public interface IHsController {

        /// <summary>
        /// The current version of the HomeSeer Plugin API
        /// </summary>
        double APIVersion { get; }
        /// <summary>
        /// The number of devices connected to the HomeSeer system
        /// </summary>
        int DeviceCount { get; }
        
        /// <summary>
        /// Register a new plugin with HomeSeer
        /// <para>
        /// This will add the specified ID/filename pair to HomeSeer's list of plugins to check when it runs through
        ///  the plugin initialization process.
        /// </para>
        /// </summary>
        /// <param name="pluginId">The ID of the plugin to register</param>
        /// <param name="pluginName">The name of the plugin to register</param>
        /// <returns>
        /// TRUE if the plugin was registered successfully;
        ///  FALSE if there was a problem with registration
        /// </returns>
        bool RegisterPlugin(string pluginId, string pluginName);
        
        #region Settings and Config data
        
        /// <summary>
        /// Clear all of the settings saved in a section in a specific file
        /// </summary>
        /// <param name="sectionName">The section to clear</param>
        /// <param name="fileName">The name of the INI file to edit</param>
        void ClearIniSection(string sectionName, string fileName);

        /// <summary>
        /// Get the value of the setting saved to INI file
        /// </summary>
        /// <param name="sectionName">The name of the section the setting is saved to</param>
        /// <param name="key">The key of the setting</param>
        /// <param name="defaultVal">A default value to use if the setting was not previously saved</param>
        /// <param name="fileName">The name of the INI file to search</param>
        /// <returns></returns>
        string GetINISetting(string sectionName, string key, string defaultVal, string fileName = "");

        /// <summary>
        /// Save the new value of a setting
        /// </summary>
        /// <param name="sectionName">The name of the section the setting is saved to</param>
        /// <param name="key">The key of the setting</param>
        /// <param name="value">The value to save</param>
        /// <param name="fileName">The name of the INI file to save the setting to</param>
        void SaveINISetting(string sectionName, string key, string value, string fileName);

        /// <summary>
        /// Get a key-value map of settings saved in the specified section of the INI file
        /// </summary>
        /// <param name="section">The section to get</param>
        /// <param name="fileName">The name of the INI file</param>
        /// <returns>A Dictionary of setting keys and values</returns>
        Dictionary<string, string> GetIniSection(string section, string fileName);

        #endregion

        #region Features/Pages

        /// <summary>
        /// Register a feature page to create a link to it in the navigation menu in HomeSeer.
        /// <para>
        /// The PluginFilename must end with .html and not include the enclosing folder name.
        ///   The page must exist in the HomeSeer html folder as: PluginID/PluginFilename
        /// </para>
        /// </summary>
        /// <param name="pluginId">The ID of the plugin</param>
        /// <param name="pageFilename">The filename of the page, ending with .html</param>
        /// <param name="linkText">The text that appears in the navigation menu</param>
        void RegisterFeaturePage(string pluginId, string pageFilename, string linkText);

        /// <summary>
        /// Unregister a feature page to remove any navigation links to the page.
        /// </summary>
        /// <param name="pluginId">The ID of the plugin</param>
        /// <param name="pageFilename">
        /// The filename of the page, ending with .html.
        ///   This must be exactly the same as the filename used to register the page</param>
        void UnregisterFeaturePage(string pluginId, string pageFilename);

        /// <summary>
        /// Register a page as the device inclusion process guide for this plugin.
        /// <para>
        /// There can only be one device inclusion process for each plugin.
        ///   The page that is tagged as the device inclusion process will be displayed first in
        ///  the list of features for the plugin and be shown in the list of devices users can add.
        /// </para>
        /// </summary>
        /// <param name="pluginId">The ID of the plugin</param>
        /// <param name="pageFilename">The filename of the page, ending with .html</param>
        /// <param name="linkText">The text that appears in the navigation menu</param>
        void RegisterDeviceIncPage(string pluginId, string pageFilename, string linkText);

        /// <summary>
        /// Unregister the device inclusion page for this plugin.
        /// </summary>
        /// <param name="pluginId">The ID of the plugin</param>
        void UnregisterDeviceIncPage(string pluginId);
        
        #endregion
        
        #region Devices
        
        #region Create
        
        /// <summary>
        /// Create a new device in HomeSeer
        /// </summary>
        /// <param name="deviceData">
        /// <see cref="NewDeviceData"/> describing the device produced by <see cref="DeviceFactory"/>
        /// </param>
        /// <returns>The unique reference ID assigned to the device</returns>
        int CreateDevice(NewDeviceData deviceData);

        /// <summary>
        /// Create a new feature on a device in HomeSeer
        /// </summary>
        /// <param name="featureData">
        /// <see cref="NewFeatureData"/> describing the feature produced by <see cref="FeatureFactory"/>
        /// </param>
        /// <returns>The unique reference ID assigned to the feature</returns>
        int CreateFeatureForDevice(NewFeatureData featureData);
        
        #endregion
        
        #region Read
        
        //Both
        List<int> GetRefsByInterface(string interfaceName, bool deviceOnly = false);
        Dictionary<int, object> GetPropertyByInterface(string interfaceName, EProperty property, bool deviceOnly = false);
        string GetNameByRef(int devOrFeatRef);
        bool DoesRefExist(int devOrFeatRef);
        object GetPropertyByRef(int devOrFeatRef, EProperty property);
        bool IsFlagOnRef(int devOrFeatRef, EMiscFlag miscFlag);
        bool IsRefDevice(int devOrFeatRef);
        
        //Devices
        HsDevice GetDeviceByRef(int devRef);
        HsDevice GetDeviceWithFeaturesByRef(int devRef);
        HsDevice GetDeviceByAddress(string devAddress);
        
        //Features
        HsFeature GetFeatureByRef(int featRef);
        HsFeature GetFeatureByAddress(string featAddress);
        bool IsFeatureValueValid(int featRef);

        StatusControl GetStatusControlForValue(int featRef, double value);
        StatusControl GetStatusControlForLabel(int featRef, string label);
        List<StatusControl> GetStatusControlsForRange(int featRef, double min, double max);
        int GetStatusControlCountByRef(int featRef);
        List<StatusControl> GetStatusControlsByRef(int featRef);

        StatusControlCollection GetStatusControlCollectionByRef(int featRef);
        
        StatusGraphic GetStatusGraphicForValue(int featRef, double value);
        List<StatusGraphic> GetStatusGraphicsForRange(int featRef, double min, double max);
        int GetStatusGraphicCountByRef(int featRef);
        List<StatusGraphic> GetStatusGraphicsByRef(int featRef);
                
        #endregion
        
        #region Update
        HsDevice UpdateDeviceByRef(int devRef, Dictionary<EProperty, object> changes);
        HsFeature UpdateFeatureByRef(int featRef, Dictionary<EProperty, object> changes);
        //HsFeature UpdateFeatureValueByRef(int featRef, double value);

        void UpdatePropertyByRef(int devOrFeatRef, EProperty property, object value);
        
        void AddStatusControlToFeature(int featRef, StatusControl statusControl);
        bool DeleteStatusControlByValue(int featRef, double value);
        void ClearStatusControlsByRef(int featRef);
        
        void AddStatusGraphicToFeature(int featRef, StatusGraphic statusGraphic);
        bool DeleteStatusGraphicByValue(int featRef, double value);
        void ClearStatusGraphicsByRef(int featRef);

        #endregion
        
        #region Delete
        
        bool DeleteDevice(int devRef);
        bool DeleteFeature(int featRef);
        
        #endregion
        
        #region Control

        string ControlFeatureByValue(int featRef, double value);
        string ControlFeatureByString(int featRef, string value);
        
        #endregion

        #endregion
        
        #region Events
        
        #region Create
        
        int CreateEventWithNameInGroup(string name, string group);
        
        #endregion
        
        #region Read
        
        string GetEventNameByRef(int eventRef);
        DateTime GetEventTriggerTime(int evRef);
        string GetEventVoiceCommand(int evRef);
        int GetEventRefByName(string event_name);
        int GetEventRefByNameAndGroup(string event_name, string event_group);
        EventGroupData GetEventGroupById(int GroupRef);
        List<EventGroupData> GetAllEventGroups();
        EventData GetEventByRef(int eventRef);
        List<EventData> GetAllEvents();
        List<EventData> GetEventsByGroup(int GroupId);
        List<TrigActInfo> GetActionsByInterface(string pluginId);
        bool IsEventLoggingEnabledByRef(int eventRef);
        bool EventEnabled(int evRef);
        int EventCount { get; }
        bool EventExistsByRef(int evRef);
        
        TrigActInfo[] GetTriggersByInterface(string pluginId);
        
        #endregion
        
        #region Update
        
        bool TriggerEventByRef(int eventRef);
        string AddDeviceActionToEvent(int evRef, ControlEvent CC);
        bool EventSetTimeTrigger(int evRef, DateTime DT);
        bool EventSetRecurringTrigger(int evRef, TimeSpan Frequency, bool Once_Per_Hour, bool Reference_To_Hour);
        void AddActionRunScript(int @ref, string script, string method, string parms);
        void DisableEventByRef(int evref);
        void DeleteAfterTrigger_Set(int evRef);
        void EnableEventByRef(int evref);
        void DeleteAfterTrigger_Clear(int evRef);

        string UpdatePlugAction(string plugName, int evRef, TrigActInfo actInfo);
        
        #endregion
        
        #region Delete
        
        void DeleteEventByRef(int evRef);
        
        #endregion
        
        void RegisterGenericEventCB(string GenericType, string pluginId);
        void UnRegisterGenericEventCB(string GenericType, string pluginId);
        void RaiseGenericEventCB(string GenericType, object[] Parms, string pluginId);
        void RegisterEventCB(Constants.HSEvent evType, string pluginId);
        
        //This doesn't exist in the legacy API?
        //void UnRegisterEventCB(Constants.HSEvent evType, string pluginId);

        TrigActInfo[] TriggerMatches(string pluginName, int trigId, int subTrigId);
        void TriggerFire(string pluginName, TrigActInfo trigInfo);

        #endregion
        
        #region System
        
        string Version();
        Constants.editions GetHSEdition();
        string GetUsers();        
        bool IsLicensed();
        bool IsRegistered();
        System.Collections.SortedList GetLocationsList();
        System.Collections.SortedList GetLocations2List();
        int CheckRegistrationStatus(string piname);

        int GetOsType();
        
        #region DateTime
        
        DateTime SolarNoon { get; }
        DateTime Sunrise   { get; }
        DateTime Sunset    { get; }
        
        #endregion

        //TODO System methods
        //int InterfaceVersion();
        //bool IsApplicationRunning(string ApplicationName);
        //string RecurseFiles(string SourceDir);
        //string[] RecurseFilesEx(string SourceDir);
        //string GetAppPath();
        //string GetOSVersion();
        //string HSMemoryUsed();
        //int HSModules();
        //int HSThreads();
        //void PowerFailRecover();
        //void RestartSystem();
        //void ShutDown();
        //string SystemUpTime();
        //TimeSpan SystemUpTimeTS();
        //void WindowsLockSystem();
        //void WindowsLogoffSystem();
        //void WindowsShutdownSystem();
        //void WindowsRebootSystem();

        #endregion
        
        #region Logging

        void WriteLog(ELogType logType, string message, string pluginName, string color = "");
        
        #endregion
        
        #region Energy

        //Create
        string Energy_AddCalculator(int dvRef, string Name, TimeSpan Range, TimeSpan StartBack);
        string Energy_AddCalculatorEvenDay(int dvRef, string Name, TimeSpan Range, TimeSpan StartBack);
        bool Energy_AddData(int dvRef, EnergyData Data);
        bool Energy_AddDataArray(int dvRef, EnergyData[] colData);
        bool Energy_SetEnergyDevice(int dvRef, Constants.enumEnergyDevice DeviceType);
        
        //Read
        int Energy_CalcCount(int dvRef);
        SortedList<int, string> Energy_GetGraphDataIDs();
        SortedList<int, string> Energy_GetEnergyRefs(bool GetParentRefs);
        System.Drawing.Image Energy_GetGraph(int id, string dvRefs, int width, int height, string format);        
        List<EnergyData> Energy_GetData(int dvRef,DateTime dteStart,DateTime dteEnd);
        List<EnergyData> Energy_GetArchiveData(int dvRef, DateTime dteStart, DateTime dteEnd);
        List<EnergyData> Energy_GetArchiveDatas(string dvRefs, DateTime dteStart, DateTime dteEnd);
        EnergyCalcData Energy_GetCalcByName(int dvRef, string Name);
        EnergyCalcData Energy_GetCalcByIndex(int dvRef, int Index);
        EnergyGraphData Energy_GetGraphData(int ID);
        
        //Update
        int Energy_SaveGraphData(EnergyGraphData Data);
        
        //Delete
        int Energy_RemoveData(int dvRef, DateTime dteStart);
        
        #endregion

        #region Not Implemented

        #region Scripts

        //TODO Script methods
        //object PluginFunction(string plugname, string pluginstance, string func,object[] parms);
        //object PluginPropertyGet(string plugname, string pluginstance, string func,object[] parms);
        //void PluginPropertySet(string plugname, string pluginstance, string prop,object value);
        //int SendMessage(string message, string host, bool showballoon);
        //int Launch(string Name, string @params, string direc, int LaunchPri);
        //bool RegisterStatusChangeCB(string script, string func);
        //void UnRegisterStatusChangeCB(string script);
        //string GetScriptPath();
        //string InstallScript(string scr_name, object param);
        //bool IsScriptRunning(string scr);
        //object RunScript(string scr, bool Wait, bool SingleInstance);
        //object RunScriptFunc(string scr, string func, object param, bool Wait, bool SingleInstance);
        //string ScriptsRunning();
        //int ValidateScriptLicense(string LicenseID, string ProductID);
        //int ValidateScriptLicenseDisplay(string LicenseID, string ProductID, bool bDisplay);

        #endregion

        #region COM

        //TODO COM port methods
        //void CloseComPort(int port);
        //int GetComPortCount(int port);
        //object GetComPortData(int port);
        //string OpenComPort(int port, string Config, int mode, string cb_script, string cb_func);
        //string OpenComPortTerm(int port, string Config, int mode, string cb_script, string cb_func, string term);
        //void SendToComPort(int port, string sData);
        //void SendToComPortBytes(int port, byte[] Data);
        //void SetComPortRTSDTR(int port, bool rtsval, bool dtrval);

        #endregion

        #region Networking & Web

        //TODO Networking methods
        //bool WEBCheckUserRights(int rights);
        //string WEBLoggedInUser();
        //bool WEBValidateUser(string username, string password);
        //string GetIPAddress();
        //string GetLastRemoteIP();
        //string LANIP();
        //string WANIP();
        //int WebServerPort();
        //int WebServerSSLPort();

        //string GenCookieString(string Name, string Value, string expire = "", string path = "/");

        #endregion

        #region Images

        //TODO image methods
        //bool WriteHTMLImageFile(byte[] ImageFile, string Dest, bool OverWrite);
        //bool WriteHTMLImage(System.Drawing.Image Image, string Dest, bool OverWrite);
        //bool DeleteImageFile(string DeleteFile);

        #endregion

        #region AppCallback

        //TODO AppCallback methods
        //void RegisterProxySpeakPlug(string   PIName, string PIInstance);
        //void UnRegisterProxySpeakPlug(string PIName, string PIInstance);

        //string UpdatePlugAction(string PlugName, int evRef, TrigActInfo ActionInfo);

        #endregion

        //TODO other methods
        //bool AppStarting(bool wait = false);
        //string BackupDB();
        //object GetHSPRef();
        //string GetInstanceList();
        //string GetPlugLinks();
        //string[] GetPluginsList();
        //int GetRemoteTimeout();
        //string GetSource();
        //void Keys(string k, string title, bool waitf);
        //int LCID();

        //void SetRemoteTimeout(int timeout_seconds);
        //void SetSecurityMode(bool mode);
        //void WaitEvents();
        //Constants.eOSType GetOSType();
        //clsLastVR[] GetLastVRCollection();
        //Constants.REGISTRATION_MODES PluginLicenseMode(string IfaceName);

        #endregion

    }

}