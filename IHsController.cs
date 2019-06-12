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
        //TODO bool   DayLightSavings          { get; }
        //TODO int    DebugMode                { get; set; }
        //TODO string ScheduleFile             { get; set; }
        //TODO bool   ShuttingDown             { get; }
        //TODO int    WEBStatsPageViews        { get; set; }
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
        void ClearINISection(string sectionName, string fileName);
        string GetINISetting(string sectionName, string key, string defaultVal, string fileName = "");
        void SaveINISetting(string sectionName, string key, string value, string fileName);

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
        
        //TODO void WriteLog(string mtype, string message);
        //TODO void WriteLogEx(string mtype, string message, string Color);
        //TODO void WriteLogDetail(string mType, string Message, string Color, int Priority, string mFrom, int ErrorCode);
        //TODO void ClearLog();
        //TODO bool NoLog { get; set; }
        //TODO string LogGet();
        //TODO LogEntry[] GetLog_Date(DateTime StartDate, DateTime EndDate);
        //TODO LogEntry[] GetLog_Date_Text(DateTime StartDate, DateTime EndDate, string mType, string mEntry,bool mEntry_RegEx);
        //TODO LogEntry[] GetLog_Date_Priority(DateTime StartDate, DateTime EndDate, int Priority_Start, int Priority_End,bool Show_No_Priority);
        //TODO LogEntry[] GetLog_Date_ErrorCode(DateTime StartDate, DateTime EndDate, int ErrorCode);
        //TODO LogEntry[] GetLog_FullFilter(DateTime StartDate, DateTime EndDate, string mType, string mEntry,bool mEntry_RegEx, int Priority_Start, int Priority_End,bool Show_No_Priority,int ErrorCode, bool ShowAllErrorCode);
        
        #endregion
        
        #region Energy
        
        //TODO int Energy_RemoveData(int dvRef,DateTime dteStart);
        //TODO string Energy_AddCalculator(int dvRef, string Name, TimeSpan Range, TimeSpan StartBack);
        //TODO string Energy_AddCalculatorEvenDay(int dvRef, string Name, TimeSpan Range, TimeSpan StartBack);
        //TODO int Energy_CalcCount(int dvRef);
        //TODO SortedList<int, string> Energy_GetGraphDataIDs();
        //TODO SortedList<int, string> Energy_GetEnergyRefs(bool GetParentRefs);
        //TODO System.Drawing.Image Energy_GetGraph(int id, string dvRefs, int width, int height, string format);
        //TODO bool Energy_SetEnergyDevice(int dvRef, Constants.enumEnergyDevice DeviceType);
        //TODO bool Energy_AddData(int dvRef, EnergyData Data);
        //TODO bool Energy_AddDataArray(int dvRef, EnergyData[] colData);
        //TODO List<EnergyData> Energy_GetData(int dvRef,DateTime dteStart,DateTime dteEnd);
        //TODO List<EnergyData> Energy_GetArchiveData(int dvRef, DateTime dteStart, DateTime dteEnd);
        //TODO List<EnergyData> Energy_GetArchiveDatas(string dvRefs, DateTime dteStart, DateTime dteEnd);
        //TODO EnergyCalcData Energy_GetCalcByName(int dvRef, string Name);
        //TODO EnergyCalcData Energy_GetCalcByIndex(int dvRef, int Index);
        //TODO int Energy_SaveGraphData(EnergyGraphData Data);
        //TODO EnergyGraphData Energy_GetGraphData(int ID);
        
        #endregion
        
        #region Scripts
        
        //TODO object PluginFunction(string plugname, string pluginstance, string func,object[] parms);
        //TODO object PluginPropertyGet(string plugname, string pluginstance, string func,object[] parms);
        //TODO void PluginPropertySet(string plugname, string pluginstance, string prop,object value);
        //TODO int SendMessage(string message, string host, bool showballoon);
        //TODO int Launch(string Name, string @params, string direc, int LaunchPri);
        //TODO bool RegisterStatusChangeCB(string script, string func);
        //TODO void UnRegisterStatusChangeCB(string script);
        //TODO string GetScriptPath();
        //TODO string InstallScript(string scr_name, object param);
        //TODO bool IsScriptRunning(string scr);
        //TODO object RunScript(string scr, bool Wait, bool SingleInstance);
        //TODO object RunScriptFunc(string scr, string func, object param, bool Wait, bool SingleInstance);
        //TODO string ScriptsRunning();
        //TODO int ValidateScriptLicense(string LicenseID, string ProductID);
        //TODO int ValidateScriptLicenseDisplay(string LicenseID, string ProductID, bool bDisplay);
        
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
        
        //TODO int InterfaceVersion();
        //TODO bool IsApplicationRunning(string ApplicationName);
        //TODO string RecurseFiles(string SourceDir);
        //TODO string[] RecurseFilesEx(string SourceDir);
        //TODO string GetAppPath();
        //TODO string GetOSVersion();
        //TODO string HSMemoryUsed();
        //TODO int HSModules();
        //TODO int HSThreads();
        //TODO void PowerFailRecover();
        //TODO void RestartSystem();
        //TODO void ShutDown();
        //TODO string SystemUpTime();
        //TODO TimeSpan SystemUpTimeTS();
        //TODO void WindowsLockSystem();
        //TODO void WindowsLogoffSystem();
        //TODO void WindowsShutdownSystem();
        //TODO void WindowsRebootSystem();
        
        #endregion
        
        #region COM
        
        //TODO void CloseComPort(int port);
        //TODO int GetComPortCount(int port);
        //TODO object GetComPortData(int port);
        //TODO string OpenComPort(int port, string Config, int mode, string cb_script, string cb_func);
        //TODO string OpenComPortTerm(int port, string Config, int mode, string cb_script, string cb_func, string term);
        //TODO void SendToComPort(int port, string sData);
        //TODO void SendToComPortBytes(int port, byte[] Data);
        //TODO void SetComPortRTSDTR(int port, bool rtsval, bool dtrval);
        
        #endregion
        
        #region Networking & Web
        
        //TODO bool WEBCheckUserRights(int rights);
        //TODO string WEBLoggedInUser();
        //TODO bool WEBValidateUser(string username, string password);
        //TODO string GetIPAddress();
        //TODO string GetLastRemoteIP();
        //TODO string LANIP();
        //TODO string WANIP();
        //TODO int WebServerPort();
        //TODO int WebServerSSLPort();
        
        //TODO string GenCookieString(string Name, string Value, string expire = "", string path = "/");
        
        #endregion
        
        #region Images
        
        //TODO bool WriteHTMLImageFile(byte[] ImageFile, string Dest, bool OverWrite);
        //TODO bool WriteHTMLImage(System.Drawing.Image Image, string Dest, bool OverWrite);
        //TODO bool DeleteImageFile(string DeleteFile);
        
        #endregion
        
        #region AppCallback

        //TODO void RegisterProxySpeakPlug(string   PIName, string PIInstance);
        //TODO void UnRegisterProxySpeakPlug(string PIName, string PIInstance);

        //TODO void RegisterGenericEventCB(string   GenericType, string   PIName, string PIInstance);
        //TODO void UnRegisterGenericEventCB(string GenericType, string   PIName, string PIInstance);
        //TODO void RaiseGenericEventCB(string      GenericType, object[] Parms,  string PIName, string PIInstance);
        //TODO void RegisterEventCB(Constants.HSEvent evType, string PIName, string PIInstance);

        //TODO TrigActInfo[] TriggerMatches(string     Plug_Name, int         TrigID,    int SubTrig);             // new
        //TODO void          TriggerFire(string        Plug_Name, TrigActInfo TrigInfo);                           // new

        //TODO TrigActInfo[] GetTriggers(string     PIName);
        //TODO TrigActInfo[] GetActions(string      PIName);

        //TODO string UpdatePlugAction(string PlugName, int evRef, TrigActInfo ActionInfo);

        #endregion
        
        //TODO bool AppStarting(bool wait = false);
        //TODO string BackupDB();
        //TODO object GetHSPRef();
        //TODO string GetInstanceList();
        //TODO string GetPlugLinks();
        //TODO string[] GetPluginsList();
        //TODO int GetRemoteTimeout();
        //TODO string GetSource();
        //TODO void Keys(string k, string title, bool waitf);
        //TODO int LCID();
        
        //TODO void SetRemoteTimeout(int timeout_seconds);
        //TODO void SetSecurityMode(bool mode);
        //TODO void WaitEvents();
        //TODO Constants.eOSType GetOSType();
        //TODO clsLastVR[] GetLastVRCollection();
        //TODO Constants.REGISTRATION_MODES PluginLicenseMode(string IfaceName);

    }

}