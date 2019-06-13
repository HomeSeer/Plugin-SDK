using System;
using System.Collections.Generic;
using HomeSeer.PluginSdk.CAPI;
using HomeSeer.PluginSdk.Energy;
using HomeSeer.PluginSdk.Speech;
using HSCF.Communication.ScsServices.Service;

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

        double APIVersion { get; }
        int DeviceCount { get; }
        
        //TODO Properties
        //bool   DayLightSavings          { get; }
        //int    DebugMode                { get; set; }
        //string ScheduleFile             { get; set; }
        //bool   ShuttingDown             { get; }
        //int    WEBStatsPageViews        { get; set; }
        
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

        string GetINISetting(string sectionName, string key, string defaultVal, string fileName = "");

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

        /*<summary>
        Register a page as the device inclusion process guide for this plugin.
        <para>
        There can only be one device inclusion process for each plugin.
          The page that is tagged as the device inclusion process will be displayed first in
         the list of features for the plugin and be shown in the list of devices users can add.
        </para>
        </summary>
        <param name="pluginId">The ID of the plugin</param>
        <param name="pageFilename">The filename of the page, ending with .html</param>
        <param name="linkText">The text that appears in the navigation menu</param>*/
        //void RegisterDeviceIncPage(string pluginId, string pageFilename, string linkText);

        /*<summary>
        Unregister the device inclusion page for this plugin.
        </summary>
        <param name="pluginId">The ID of the plugin</param>*/
        //void UnregisterDeviceIncPage(string pluginId);
        
        #endregion
        
        #region Devices
        
        #region Create
        
        int NewDeviceRef(string Name);
        //TODO int NewDevice(string Name);
        
        #endregion
        
        #region Read
        
        object GetDeviceByRef(int @ref);
        object GetDeviceEnumerator();
        
        //VSP
        
        VSPair DeviceVSP_Get(int dvRef, double Value, Constants.ePairStatusControl VSPType);
        string DeviceVSP_GetStatus(int dvRef, double Value, Constants.ePairStatusControl VSPType);
        VSPair[] DeviceVSP_GetAllStatus(int dvRef);
        int DeviceVSP_CountStatus(int dvRef);
        int DeviceVSP_CountControl(int dvRef);
        int DeviceVSP_CountAll(int dvRef);
        bool DeviceVSP_PairsProtected(int dvRef);
        
        //VGP
        
        VGPair DeviceVGP_Get(int dvRef, double Value);
        int DeviceVGP_Count(int dvRef);
        string DeviceVGP_GetGraphic(int dvRef, double Value);
        bool DeviceVGP_PairsProtected(int dvRef);
        
        bool DeviceExistsRef(int dvRef);
        int DeviceExistsAddress(string Address, bool CaseSensitive);
        int DeviceExistsAddressFull(string Address, bool CaseSensitive);
        int DeviceExistsCode(string Code);
        
        int GetDeviceRef(string sAddress);
        int GetDeviceRefByName(string device_name);
        int GetDeviceParentRefByRef(int @ref);
        string GetDeviceCode(string Name);
        //TODO object GetDeviceEnumeratorUser(string user); -> This returns a DeviceClass
        string GetNextVirtualCode();
        
        int DeviceValue(int dvRef);
        double DeviceValueEx(int dvRef);
        int DeviceValueByName(string devname);
        double DeviceValueByNameEx(string devname);
        string DeviceString(int dvRef);
        string DeviceStringByName(string Name);
        int DeviceTime(int dvRef);
        int DeviceTimeByName(string dev_name);
        DateTime DeviceDateTime(int dvRef);
        DateTime DeviceLastChange(string Address);
        DateTime DeviceLastChangeRef(int dvRef);
        string DeviceName(int dvRef);
        bool IsOff(int dvRef);
        bool IsON(int dvRef);
        bool IsOffByName(string dev_name);
        bool IsOnByName(string dev_name);
        

        //TODO ICAPIStatus CAPIGetStatus(int dvRef);
        //TODO CAPIControl[] CAPIGetControl(int dvRef);
        //TODO CAPIControl[] CAPIGetControlEx(int dvRef,bool SingleRangeEntry);
        //TODO CAPIControl CAPIGetSingleControl(int dvRef,bool SingleRangeEntry, string Label,bool ExactCase, bool Contains);
        //TODO CAPIControl CAPIGetSingleControlByUse(int dvRef, Constants.ePairControlUse UseType);
        
        #endregion
        
        #region Update
        
        void SetDeviceValueByRef(int dvRef, double Valuenum, bool trigger);
        void SetDeviceString(int dvRef, string st, bool reset);
        void DeviceVSP_ClearAll(int dvRef, bool TrueConfirm);
        bool DeviceVSP_ClearAny(int dvRef, double Value);
        bool DeviceVSP_ClearBoth(int dvRef, double Value);
        bool DeviceVSP_ClearControl(int dvRef, double Value);
        bool DeviceVSP_ClearStatus(int dvRef, double Value);
        bool DeviceVSP_ChangePair(int dvRef, VSPair Existing, Constants.ePairStatusControl NewType);
        bool DeviceVSP_AddPair(int dvRef, VSPair Pair);
        bool DeviceVGP_AddPair(int dvRef, VGPair Pair);
        void DeviceVGP_ClearAll(int dvRef, bool TrueConfirm);
        bool DeviceVGP_Clear(int dvRef, double Value);
        void DeviceProperty_Int(int dvRef, Constants.eDeviceProperty Prop, int Value);
        void DeviceProperty_String(int dvRef, Constants.eDeviceProperty Prop, string Value);
        void DeviceProperty_StrArray(int dvRef, Constants.eDeviceProperty Prop, string[] Value);
        void DeviceProperty_Boolean(int dvRef, Constants.eDeviceProperty Prop, bool Value);
        void DeviceProperty_DevType(int dvRef, Constants.eDeviceProperty Prop, DeviceTypeInfo Value);
        void DeviceProperty_Date(int dvRef, Constants.eDeviceProperty Prop, DateTime Value);
        void DeviceProperty_dvMISC(int dvRef, Constants.eDeviceProperty Prop, Constants.dvMISC Value);
        void DeviceProperty_PlugData(int dvRef, Constants.eDeviceProperty Prop, PlugExtraData Value);
        
        void SetDeviceValue(string Address, double Value);
        void SetDeviceValueByName(string devname, double Value);
        void SetDeviceStringByName(string devname, string strval, bool reset_lastchange);
        void SetDeviceLastChange(int dvRef, DateTime change_time);
        
        #endregion
        
        #region Delete
        
        bool DeleteDevice(int dvRef);
        //TODO void DeleteIODevices(string pluginName, string pluginInstance); -> Plugins operate by ID now; how do we handle this?
        
        #endregion

        void SaveEventsDevices();
        bool DeviceNoLog(int dvRef);
        //TODO bool DeviceInvalidValue { get; set; } -> What is this designed to do?
        
        Constants.CAPIControlResponse CAPIControlHandler(CAPIControl CC);
        Constants.CAPIControlResponse CAPIControlsHandler(CAPIControl[] CC);

        #endregion
        
        #region DateTime
        
        DateTime SolarNoon { get; }
        DateTime Sunrise { get; }
        DateTime Sunset { get; }
        
        #endregion
        
        #region Logging
        
        //TODO Logging
        //void WriteLog(string mtype, string message);
        //void WriteLogEx(string mtype, string message, string Color);
        //void WriteLogDetail(string mType, string Message, string Color, int Priority, string mFrom, int ErrorCode);
        //void ClearLog();
        //bool NoLog { get; set; }
        //string LogGet();
        //LogEntry[] GetLog_Date(DateTime StartDate, DateTime EndDate);
        //LogEntry[] GetLog_Date_Text(DateTime StartDate, DateTime EndDate, string mType, string mEntry,bool mEntry_RegEx);
        //LogEntry[] GetLog_Date_Priority(DateTime StartDate, DateTime EndDate, int Priority_Start, int Priority_End,bool Show_No_Priority);
        //LogEntry[] GetLog_Date_ErrorCode(DateTime StartDate, DateTime EndDate, int ErrorCode);
        //LogEntry[] GetLog_FullFilter(DateTime StartDate, DateTime EndDate, string mType, string mEntry,bool mEntry_RegEx, int Priority_Start, int Priority_End,bool Show_No_Priority,int ErrorCode, bool ShowAllErrorCode);
        
        #endregion
        
        #region Energy
        
        //TODO Energy methods
        //int Energy_RemoveData(int dvRef,DateTime dteStart);
        //string Energy_AddCalculator(int dvRef, string Name, TimeSpan Range, TimeSpan StartBack);
        //string Energy_AddCalculatorEvenDay(int dvRef, string Name, TimeSpan Range, TimeSpan StartBack);
        //int Energy_CalcCount(int dvRef);
        //SortedList<int, string> Energy_GetGraphDataIDs();
        //SortedList<int, string> Energy_GetEnergyRefs(bool GetParentRefs);
        //System.Drawing.Image Energy_GetGraph(int id, string dvRefs, int width, int height, string format);
        //bool Energy_SetEnergyDevice(int dvRef, Constants.enumEnergyDevice DeviceType);
        //bool Energy_AddData(int dvRef, EnergyData Data);
        //bool Energy_AddDataArray(int dvRef, EnergyData[] colData);
        //List<EnergyData> Energy_GetData(int dvRef,DateTime dteStart,DateTime dteEnd);
        //List<EnergyData> Energy_GetArchiveData(int dvRef, DateTime dteStart, DateTime dteEnd);
        //List<EnergyData> Energy_GetArchiveDatas(string dvRefs, DateTime dteStart, DateTime dteEnd);
        //EnergyCalcData Energy_GetCalcByName(int dvRef, string Name);
        //EnergyCalcData Energy_GetCalcByIndex(int dvRef, int Index);
        //int Energy_SaveGraphData(EnergyGraphData Data);
        //EnergyGraphData Energy_GetGraphData(int ID);
        
        #endregion
        
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
        
        #region System
        
        string Version();
        Constants.editions GetHSEdition();
        string GetUsers();        
        bool IsLicensed();
        bool IsRegistered();
        System.Collections.SortedList GetLocationsList();
        System.Collections.SortedList GetLocations2List();
        int CheckRegistrationStatus(string piname);
        
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

        //void RegisterGenericEventCB(string   GenericType, string   PIName, string PIInstance);
        //void UnRegisterGenericEventCB(string GenericType, string   PIName, string PIInstance);
        //void RaiseGenericEventCB(string      GenericType, object[] Parms,  string PIName, string PIInstance);
        //void RegisterEventCB(Constants.HSEvent evType, string PIName, string PIInstance);

        //TrigActInfo[] TriggerMatches(string     Plug_Name, int         TrigID,    int SubTrig);             // new
        //void          TriggerFire(string        Plug_Name, TrigActInfo TrigInfo);                           // new

        //TrigActInfo[] GetTriggers(string     PIName);
        //TrigActInfo[] GetActions(string      PIName);

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

    }

}