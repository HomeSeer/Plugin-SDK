using System;
using System.Collections.Generic;
using HomeSeer.PluginSdk.CAPI;
using HomeSeer.PluginSdk.Energy;
using HomeSeer.PluginSdk.Speech;
using HSCF.Communication.ScsServices.Service;

namespace HomeSeer.PluginSdk {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [ScsService]
    public interface IHsController {

        double APIVersion               { get; }
        //TODO bool   DayLightSavings          { get; }
        //TODO int    DebugMode                { get; set; }
        //TODO string ScheduleFile             { get; set; }
        //TODO bool   ShuttingDown             { get; }
        //TODO int    WEBStatsPageViews        { get; set; }
        
        bool RegisterPlugin(string pluginId, string pluginName);
        
        #region DateTime
        
        //TODO DateTime SolarNoon    { get; }
        //TODO string   Sunrise      { get; }
        //TODO DateTime SunriseDt    { get; }
        //TODO string   Sunset       { get; }
        //TODO DateTime SunsetDt     { get; }
        //TODO string   TimeZoneName { get; }
        //TODO int DaysLeftInYear();
        //TODO int DaysLeftInMonth();
        //TODO int DaysInMonth(DateTime inDate);
        //TODO DateTime GetLastWeekday(DateTime ForMonth);
        //TODO bool IsWeekend(DateTime inDate);
        //TODO bool IsWeekday(DateTime inDate);
        //TODO int LocalTimeZone();
        //TODO void Moon(DateTime dtStart, ref DateTime NMoon, ref DateTime FMoon, ref double CurCycle, ref string Desc);
        //TODO int WeeksLeftInYear();
        //TODO int WeeksLeftInYearEx(int WeekMode);
        //TODO int WeekNumber(DateTime inDate);
        //TODO int WeekNumberEx(DateTime inDate, int WeekMode);
        //TODO int WeekEndDays(DateTime dtStart, DateTime dtEnd);
        //TODO int WeekDays(DateTime dtStart, DateTime dtEnd);
        
        #endregion
        
        #region Settings and Config data
        
        //TODO void ClearINISection(string section, string FileName);
        //TODO string GetINISection(string section, string FileName);
        //TODO string[] GetINISectionEx(string section, string FileName);
        string GetINISetting(string section, string key, string default_val, string FileName = "");
        void SaveINISetting(string section, string key, string Value, string FileName);
        /// <summary>
        ///     ''' Register a list of HS-JUI settings pages for this plugin instance
        ///     ''' </summary>
        ///     ''' <param name="pages">A Map of page IDs and page Names. Their order in this list is the order they will be presented in</param>
        ///     ''' <param name="id">unique id of the plugin</param>
        void RegisterJuiSettingsPages(Dictionary<string, string> pages, string id);
        
        #endregion
        
        #region Features/Pages
        
        //TODO void RegisterHelpLink(WebPageDesc cbo);
        //TODO void RegisterLinkEx(WebPageDesc cbo);
        //TODO void UnRegisterLinkEx(WebPageDesc cbo);
        //TODO void SetPageContentType(string pageName, string contentType);
        //TODO string GetPageFooter(bool NoEndTags = false);
        //TODO string GetPageHeader(string pageName, string title, string extra_meta, string HSOnload,bool ExcludeNavLinks,bool NoHeader, bool HeadContentOnly = false, bool BodyContentOnly = false,bool BodyOnLoadOnly = false);
        //TODO string GetPageHeaderRaw(string pageName, string title, string extra_meta, string HSOnload,bool ExcludeNavLinks,bool NoHeader, bool HeadContentOnly = false, bool BodyContentOnly = false,bool BodyOnLoadOnly = false);
        /// <summary>
        ///     ''' Register a list of HTML feature page names for this plugin instance
        ///     ''' </summary>
        ///     ''' <param name="pageNames">A list of page names. Their order in this list is the order they will be presented in</param>
        ///     ''' <param name="id">unique id of the plugin</param>
        void RegisterHtmlFeaturePages(List<string> pageNames, string id);
        /// <summary>
        ///     ''' Register a device inclusion wizard for this plugin instance
        ///     ''' </summary>
        ///     ''' <param name="pageId">The ID of the page</param>
        ///     ''' <param name="pageName">The title of the page displayed to users</param>
        ///     ''' <param name="id">unique id of the plugin</param>
        void RegisterJuiDeviceIncPage(string pageId, string pageName, string id);
        /// <summary>
        ///     ''' Register a list of HS-JUI feature pages for this plugin instance
        ///     ''' </summary>
        ///     ''' <param name="pages">A Map of page IDs and page Names. Their order in this list is the order they will be presented in</param>
        ///     ''' <param name="id">unique id of the plugin</param>
        void RegisterJuiFeaturePages(Dictionary<string, string> pages, string id);
        
        string RegisterPage(string pageName, string pluginId);
        
        void UnRegisterHelpLinks(string plugin_name, string plugin_instance);
        /// <summary>
        ///     ''' Unregister all pages of a certain type for a plugin instance
        ///     ''' </summary>
        ///     ''' <param name="pageType">The type of page to unregister. See HomeSeer.Jui.Types.EPageType for a list of values</param>
        ///     ''' <param name="id">unique id of the plugin</param>
        void UnregisterJuiPagesByType(int pageType, string id);
        
        #endregion
        
        #region Devices
        
        //TODO void DeleteIODevices(string pluginName, string pluginInstance);
        //TODO bool DeviceExistsRef(int dvRef);
        //TODO int DeviceExistsAddress(string Address, bool CaseSensitive);
        //TODO int DeviceExistsAddressFull(string Address, bool CaseSensitive);
        //TODO int DeviceExistsCode(string Code);
        //TODO int GetDeviceRef(string sAddress);
        //TODO int GetDeviceRefByName(string device_name);
        //TODO int GetDeviceParentRefByRef(int @ref);
        //TODO string GetDeviceCode(string Name);
        //TODO object GetDeviceByRef(int @ref);
        //TODO object GetDeviceEnumerator();
        //TODO object GetDeviceEnumeratorUser(string user);
        //TODO string GetNextVirtualCode();
        //TODO int DeviceValue(int dvRef);
        //TODO double DeviceValueEx(int dvRef);
        //TODO int DeviceValueByName(string devname);
        //TODO double DeviceValueByNameEx(string devname);
        //TODO void SetDeviceValue(string Address, double Value);
        //TODO void SetDeviceValueByRef(int dvRef, double Valuenum, bool trigger);
        //TODO void SetDeviceValueByName(string devname, double Value);
        //TODO string DeviceString(int dvRef);
        //TODO string DeviceStringByName(string Name);
        //TODO void SetDeviceString(int dvRef, string st, bool reset);
        //TODO void SetDeviceStringByName(string devname, string strval, bool reset_lastchange);
        //TODO bool DeviceScriptButton_Add(int dvRef, string Label, string ScriptFile, string ScriptFunc, string ScriptParm,ushort Row, ushort Column, ushort ColumnSpan);
        //TODO bool DeviceScriptButton_Delete(int dvRef, string Label);
        //TODO void DeviceScriptButton_DeleteAll(int dvRef);
        //TODO string[] DeviceScriptButton_List(int dvRef);
        //TODO bool DeviceScriptButton_Location(int dvRef, string Label, ushort Row, ushort Column, ushort ColumnSpan);
        //TODO bool DeviceScriptButton_AddButton(int dvRef, string Label, double Value, string ScriptFile,string ScriptFunc,string ScriptParm, ushort Row, ushort Column, ushort ColumnSpan);
        //TODO bool DeviceScriptButton_DeleteButton(int dvRef, double Value);
        //TODO bool DeviceScriptButton_Locate(int dvRef, double Value, ushort Row, ushort Column, ushort ColumnSpan);
        //TODO int DeviceTime(int dvRef);
        //TODO int DeviceTimeByName(string dev_name);
        //TODO DateTime DeviceDateTime(int dvRef);
        //TODO void SetDeviceLastChange(int dvRef, DateTime change_time);
        //TODO DateTime DeviceLastChange(string Address);
        //TODO DateTime DeviceLastChangeRef(int dvRef);
        //TODO string DeviceName(int dvRef);
        //TODO bool DeviceNoLog(int dvRef);
        //TODO bool DeviceInvalidValue { get; set; }
        //TODO bool DeleteDevice(int dvRef);
        //TODO bool IsOff(int dvRef);
        //TODO bool IsON(int dvRef);
        //TODO int NewDevice(string Name);
        //TODO int NewDeviceRef(string Name);
        //TODO bool IsOffByName(string dev_name);
        //TODO bool IsOnByName(string dev_name);
        //TODO int DeviceCount { get; }
        //TODO int DeviceVSP_CountStatus(int dvRef);
        //TODO int DeviceVSP_CountControl(int dvRef);
        //TODO int DeviceVSP_CountAll(int dvRef);
        //TODO void DeviceVSP_ClearAll(int dvRef, bool TrueConfirm);
        //TODO bool DeviceVSP_ClearAny(int dvRef, double Value);
        //TODO bool DeviceVSP_ClearBoth(int dvRef, double Value);
        //TODO bool DeviceVSP_ClearControl(int dvRef, double Value);
        //TODO bool DeviceVSP_ClearStatus(int dvRef, double Value);
        //TODO bool DeviceVSP_PairsProtected(int dvRef);
        //TODO string DeviceVGP_GetGraphic(int dvRef, double Value);
        //TODO bool DeviceVGP_PairsProtected(int dvRef);
        
        #endregion
        
        #region Logging
        
        //TODO void WriteLog(string mtype, string message);
        //TODO void WriteLogEx(string mtype, string message, string Color);
        //TODO void WriteLogDetail(string mType, string Message, string Color, int Priority, string mFrom, int ErrorCode);
        //TODO void ClearLog();
        //TODO bool NoLog { get; set; }
        //TODO string LogGet();
        
        #endregion
        
        #region Energy
        
        //TODO int Energy_RemoveData(int dvRef,DateTime dteStart);
        //TODO string Energy_AddCalculator(int dvRef, string Name, TimeSpan Range, TimeSpan StartBack);
        //TODO string Energy_AddCalculatorEvenDay(int dvRef, string Name, TimeSpan Range, TimeSpan StartBack);
        //TODO int Energy_CalcCount(int dvRef);
        //TODO SortedList<int, string> Energy_GetGraphDataIDs();
        //TODO SortedList<int, string> Energy_GetEnergyRefs(bool GetParentRefs);
        //TODO System.Drawing.Image Energy_GetGraph(int id, string dvRefs, int width, int height, string format);
        
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
        
        //TODO string Version();
        //TODO Constants.editions GetHSEdition();
        //TODO string GetUsers();        
        //TODO int InterfaceVersion();
        //TODO bool IsApplicationRunning(string ApplicationName);
        //TODO bool IsLicensed();
        //TODO bool IsRegistered();
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
        
        #region Util
        
        //TODO void WaitSecs(double secs);
        //TODO TimeSpan TimerValue(string TimerName);
        //TODO void TimerReset(string TimerName);
        //TODO void CounterDecrement(string CounterName);
        //TODO void CounterIncrement(string CounterName);
        //TODO void CounterReset(string CounterName);
        //TODO double CounterValue(string CounterName);
        //TODO string CreateVar(string Name);
        //TODO void DeleteVar(string Name);
        //TODO object GetVar(string Name);
        //TODO string ReplaceVariables(string StrIn);
        //TODO string SaveVar(string Name, object obj);
        //TODO string DecryptString(string sToDecrypt, string sPassword, string KeyModifier = "");
        //TODO string EncryptString(string sToEncrypt, string sPassword);
        //TODO string EncryptStringEx(string sToEncrypt, string sPassword, string KeyModifier);
        //TODO string UnZip(string filename);
        //TODO string UnZipDest(string filename, string destination);
        //TODO string UnZipDestIgnore(string filename, string destination, bool IgnoreZipDirs);
        //TODO string UnZipEx(string filename, string destination, bool IgnoreZipDirs, bool OverWrite, string password);
        //TODO string Zip(string WhatToZip, string zipFileName);
        //TODO string ZipPass(string WhatToZip, string zipFileName, string pass);
        //TODO string ZipLevel(string WhatToZip, string zipFileName, int level, string pass);
        //TODO string ZipEx(string WhatToZip, string zipFileName, int level, string pass, bool RemoveBase, bool Flatten);
        
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
        
        //TODO string FTP(string host, string username, string password, string command, string Path, string local_file, string remote_file);
        //TODO string FTPLastError();
        //TODO string GenCookieString(string Name, string Value, string expire = "", string path = "/");
        //TODO string GetURL(string host_str, string page, bool strip_tags, int port, bool UTF8 = false);
        //TODO object GetURLEx(string Host, string WebPage, string ElapsedTime, int Port = 80, bool Strip_Tags = false, bool ByteArray = false, string FileName = "");
        //TODO string GetURLIE(string host, bool strip_tags);
        //TODO string URLAction(string url, string action, string data, string headers);
        //TODO bool WEBCheckUserRights(int rights);
        //TODO string WEBLoggedInUser();
        //TODO bool WEBValidateUser(string username, string password);
        //TODO string GetIPAddress();
        //TODO string GetLastRemoteIP();
        //TODO int ping(string host, int timeout = 0);
        //TODO string LANIP();
        //TODO string WANIP();
        //TODO int WebServerPort();
        //TODO int WebServerSSLPort();
        
        #endregion
        
        #region Images
        
        //TODO object GetURLImage(string host_str, string page, bool strip_tags, int port, string filename = "");
        //TODO string GetURLImageEx(string host_str, string page, string filename, int port = 80);
        //TODO bool WriteHTMLImageFile(byte[] ImageFile, string Dest, bool OverWrite);
        //TODO bool WriteHTMLImage(System.Drawing.Image Image, string Dest, bool OverWrite);
        //TODO bool DeleteImageFile(string DeleteFile);
        
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
        //TODO void SaveEventsDevices();
        //TODO void SetRemoteTimeout(int timeout_seconds);
        //TODO void SetSecurityMode(bool mode);
        //TODO void WaitEvents();

        #region Needs Rewrite
        
        //TODO Constants.eOSType GetOSType();
        //TODO clsLastVR[] GetLastVRCollection();
        //TODO Constants.REGISTRATION_MODES PluginLicenseMode(string IfaceName);
        //TODO Constants.CD_DAY_EvenOdd EvenOddDay(DateTime inDate);
        //TODO Constants.CD_DAY_EvenOdd EvenOddMonth(DateTime indate);
        //TODO DateTime GetSpecialDay(DayOfWeek DOW, Constants.CD_DAY_IS_TYPE inst,DateTime ForMonth, bool GetNext);
        //TODO bool IsSpecialDay(DateTime inDate, DayOfWeek DOW,Constants.CD_DAY_IS_TYPE inst, DateTime ForMonth);
        //TODO ICAPIStatus CAPIGetStatus(int dvRef);
        //TODO CAPIControl[] CAPIGetControl(int dvRef);
        //TODO CAPIControl[] CAPIGetControlEx(int dvRef,bool SingleRangeEntry);
        //TODO CAPIControl CAPIGetSingleControl(int dvRef,bool SingleRangeEntry, string Label,bool ExactCase, bool Contains);
        //TODO CAPIControl CAPIGetSingleControlByUse(int dvRef, Constants.ePairControlUse UseType);
        //TODO Constants.CAPIControlResponse CAPIControlHandler(CAPIControl CC);
        //TODO Constants.CAPIControlResponse CAPIControlsHandler(CAPIControl[] CC);
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
        //TODO bool DeviceVSP_AddPair(int dvRef, VSPair Pair);
        //TODO bool DeviceVSP_ChangePair(int dvRef, VSPair Existing, Constants.ePairStatusControl NewType);
        //TODO VSPair DeviceVSP_Get(int dvRef, double Value, Constants.ePairStatusControl VSPType);
        //TODO string DeviceVSP_GetStatus(int dvRef, double Value, Constants.ePairStatusControl VSPType);
        //TODO VSPair[] DeviceVSP_GetAllStatus(int dvRef);
        //TODO bool DeviceVGP_AddPair(int dvRef, VGPair Pair);
        //TODO int DeviceVGP_Count(int dvRef);
        //TODO void DeviceVGP_ClearAll(int dvRef, bool TrueConfirm);
        //TODO bool DeviceVGP_Clear(int dvRef, double Value);
        //TODO VGPair DeviceVGP_Get(int dvRef, double Value);
        //TODO void DeviceProperty_Int(int dvRef, Constants.eDeviceProperty Prop, int Value);
        //TODO void DeviceProperty_String(int dvRef, Constants.eDeviceProperty Prop, string Value);
        //TODO void DeviceProperty_StrArray(int dvRef, Constants.eDeviceProperty Prop, string[] Value);
        //TODO void DeviceProperty_Boolean(int dvRef, Constants.eDeviceProperty Prop, bool Value);
        //TODO void DeviceProperty_DevType(int dvRef, Constants.eDeviceProperty Prop, DeviceTypeInfo Value);
        //TODO void DeviceProperty_Date(int dvRef, Constants.eDeviceProperty Prop, DateTime Value);
        //TODO void DeviceProperty_dvMISC(int dvRef, Constants.eDeviceProperty Prop, Constants.dvMISC Value);
        //TODO void DeviceProperty_PlugData(int dvRef, Constants.eDeviceProperty Prop, clsPlugExtraData Value);
        //TODO LogEntry[] GetLog_Date(DateTime StartDate, DateTime EndDate);
        //TODO LogEntry[] GetLog_Date_Text(DateTime StartDate, DateTime EndDate, string mType, string mEntry,bool mEntry_RegEx);
        //TODO LogEntry[] GetLog_Date_Priority(DateTime StartDate, DateTime EndDate, int Priority_Start, int Priority_End,bool Show_No_Priority);
        //TODO LogEntry[] GetLog_Date_ErrorCode(DateTime StartDate, DateTime EndDate, int ErrorCode);
        //TODO LogEntry[] GetLog_FullFilter(DateTime StartDate, DateTime EndDate, string mType, string mEntry,bool mEntry_RegEx, int Priority_Start, int Priority_End,bool Show_No_Priority,int ErrorCode, bool ShowAllErrorCode);
        #endregion

    }

}