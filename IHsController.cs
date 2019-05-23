using System;
using System.Collections.Generic;
using HomeSeer.PluginSdk.CAPI;
using HomeSeer.PluginSdk.Energy;
using HomeSeer.PluginSdk.Speech;

namespace HomeSeer.PluginSdk {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public interface IHsController {

        double APIVersion               { get; }
        bool   DayLightSavings          { get; }
        int    DebugMode                { get; set; }
        string LastCommandSelected      { get; }
        string LastVoiceCommand         { get; set; }
        string LastVoiceCommandPhone    { get; set; }
        string LastVoiceCommandHost     { get; set; }
        string LastVoiceCommandInstance { get; set; }
        string LastVoiceCommandRaw      { get; set; }
        string MEDIAFilename            { get; set; }
        int    MEDIAVolume              { get; set; }
        bool   MuteSpeech               { get; set; }
        string ScheduleFile             { get; set; }
        bool   ShuttingDown             { get; }
        int    WEBStatsPageViews        { get; set; }


        void   AddVoiceCommand(string cmd, string host = "");
        bool   AppStarting(bool       wait = false);
        string BackupDB();
        void   ClearAllVoiceCommands(string host = "");
        void   ClearINISection(string       section, string FileName);
        void   CloseComPort(int             port);
        bool   Connect(string               PluginName, string InstanceName);
        void   CounterDecrement(string      CounterName);
        void   CounterIncrement(string      CounterName);
        void   CounterReset(string          CounterName);
        double CounterValue(string          CounterName);
        string CreateVar(string             Name);
        string DecryptString(string         sToDecrypt, string sPassword, string KeyModifier = "");
        void   DelayTrigger(int             secs,       string evname);
        void   DeleteIODevices(string       pluginName, string pluginInstance);
        void   DeleteVar(string             Name);
        string EncryptString(string         sToEncrypt, string sPassword);
        string EncryptStringEx(string       sToEncrypt, string sPassword, string KeyModifier);

        string FTP(string host, string username, string password, string command, string Path, string local_file,
                   string remote_file);

        string FTPLastError();
        string GenCookieString(string Name, string Value, string expire = "", string path = "/");
        string GetAppPath();
        int    GetComPortCount(int port);

        object GetComPortData(int port);

        // Function GetHSObject() As IHSApplication
        Constants.editions GetHSEdition();
        object             GetHSPRef();
        string             GetINISection(string   section, string FileName);
        string[]           GetINISectionEx(string section, string FileName);
        string             GetINISetting(string   section, string key, string default_val, string FileName = "");
        string             GetInstanceList();
        string             GetIPAddress();
        string             GetLastRemoteIP();
        string             GetLastEvent();
        clsLastVR[]        GetLastVRCollection();
        bool               GetListenStatus(string host = "");
        bool               GetMuteStatus(string   host = "");
        string             GetOSVersion();
        Constants.eOSType  GetOSType();
        string             GetPageFooter(bool NoEndTags = false);

        string GetPageHeader(string pageName, string title, string extra_meta, string HSOnload,
                             bool   ExcludeNavLinks,
                             bool   NoHeader, bool HeadContentOnly = false, bool BodyContentOnly = false,
                             bool   BodyOnLoadOnly = false);

        string GetPageHeaderRaw(string pageName, string title, string extra_meta, string HSOnload,
                                bool   ExcludeNavLinks,
                                bool   NoHeader, bool HeadContentOnly = false, bool BodyContentOnly = false,
                                bool   BodyOnLoadOnly = false);

        int      GetPauseStatus(string host = "");
        string   GetPlugLinks();
        string[] GetPluginsList();
        void     GetPreviousEventRan(string sEvent, DateTime dtEvent);
        int      GetRemoteTimeout();
        string   GetScriptPath();
        string   GetSource();
        string   GetURL(string host_str, string page, bool strip_tags, int port, bool UTF8 = false);

        object GetURLEx(string Host, string WebPage, string ElapsedTime, int Port = 80,
                        bool   Strip_Tags = false,
                        bool   ByteArray  = false, string FileName = "");

        string GetURLIE(string      host,     bool   strip_tags);
        object GetURLImage(string   host_str, string page, bool   strip_tags, int port, string filename = "");
        string GetURLImageEx(string host_str, string page, string filename,   int port = 80);
        string GetUsers();
        object GetVar(string       Name);
        string GetVoiceName(string host = "");
        int    GetVolume(string    host = "");

        string HSMemoryUsed();

        // Function HSVMUsed() As String
        int    HSModules();
        int    HSThreads();
        int    InterfaceVersion();
        string InstallScript(string        scr_name, object param);
        bool   IsApplicationRunning(string ApplicationName);
        bool   IsLicensed();
        bool   IsRegistered();
        bool   IsScriptRunning(string    scr);
        bool   IsSpeakerBusy(string      host = "");
        bool   IsSpeakerConnected(string host = "", string instance = "");
        void   Keys(string               k,         string title, bool waitf);
        string LastCallerInfo(int        line);
        string LastVoiceMailInfo(bool    MessageInfo = false);
        int    Launch(string             Name, string @params, string direc, int LaunchPri);
        int    LCID();
        void   ListenForCommands(bool action, string host);
        int    ListenMode(string      host);
        bool   MEDIAIsPlaying(string  Host              = "");
        void   MEDIAMute(bool         mode, string Host = "");
        void   MEDIAPause(string      Host     = "");
        void   MEDIAPlay(string       filename = "", string host = "");
        void   MEDIAStop(string       Host     = "");
        void   MEDIAUnPause(string    Host     = "");
        void   MuteAudio(string       Host     = "");

        string OpenComPort(int                          port, string Config, int mode, string cb_script,
                                                 string cb_func);

        string OpenComPortTerm(int                          port,    string Config, int mode, string cb_script,
                                                     string cb_func, string term);

        void PauseAudio(string Host = "");
        void PhoneEvent(int    eline, int evnum, string dtmf);
        int  ping(string       host,  int timeout = 0);

        void PlayWavFile(string                       FileName, string Host = "",
                                                 bool Wait = false);

        void PlayWavFileVol(string                         FileName,  int  vol_left, int vol_right,
                                                    string Host = "", bool Wait = false);

        object PluginFunction(string                         plugname, string pluginstance, string func,
                                                    object[] parms);

        object PluginPropertyGet(string                         plugname, string pluginstance, string func,
                                                       object[] parms);

        void PluginPropertySet(string                         plugname, string pluginstance, string prop,
                                                       object value);

        Constants.REGISTRATION_MODES PluginLicenseMode(string IfaceName);
        void                         PowerFailRecover();
        string                       RecurseFiles(string               SourceDir);
        string[]                     RecurseFilesEx(string             SourceDir);
        void                         RegisterEventCB(Constants.HSEvent evtype, string ScriptFile, string ScriptProc);

        /// <summary>
        ///     ''' Register the web address for the help documentation for this plugin.
        ///     ''' </summary>
        ///     ''' <param name="cbo"></param>
        void RegisterHelpLink(WebPageDesc cbo);

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

        /// <summary>
        ///     ''' Register a list of HS-JUI settings pages for this plugin instance
        ///     ''' </summary>
        ///     ''' <param name="pages">A Map of page IDs and page Names. Their order in this list is the order they will be presented in</param>
        ///     ''' <param name="id">unique id of the plugin</param>
        void RegisterJuiSettingsPages(Dictionary<string, string> pages, string id);

        void   RegisterLinkEx(WebPageDesc    cbo);
        string RegisterPage(string           pageName, string PluginName, string pluginInstance);
        bool   RegisterStatusChangeCB(string script,   string func);
        void   RemoveDelayedEvent(int        dvRef,    string event_name);
        string ReplaceVariables(string       StrIn);
        void   RestartSystem();
        object RunScript(string     scr, bool   Wait, bool   SingleInstance);
        object RunScriptFunc(string scr, string func, object param, bool Wait, bool SingleInstance);
        void   SaveEventsDevices();
        void   SaveINISetting(string section, string key, string Value, string FileName);
        string SaveVar(string        Name,    object obj);
        string ScriptsRunning();
        int    SendMessage(string        message,  string host, bool showballoon);
        void   SendToComPort(int         port,     string sData);
        void   SendToComPortBytes(int    port,     byte[] Data);
        void   SetComPortRTSDTR(int      port,     bool   rtsval, bool dtrval);
        void   SetPageContentType(string pageName, string contentType);
        void   SetRemoteTimeout(int      timeout_seconds);
        void   SetSecurityMode(bool      mode);
        void   SetSpeaker(string         speaker_name, string host);
        int    SetSpeakingSpeed(int      Speed,        string Host = "");
        int    SetVoice(string           VoiceName,    string Host = "");
        void   SetVolume(int             Level,        string Host = "");
        void   ShutDown();
        void   Speak(string       Text,   bool   Wait = false, string Host              = "");
        void   SpeakEx(int        Device, string Text,         bool   Wait, string Host = "");
        void   SpeakProxy(int     Device, string Text,         bool   Wait, string host = "");
        void   SpeakToFile(string Text,   string Voice,        string FileName);
        void   StartListen(string Host);
        void   StopListen(string  Host);

        void StopSpeaking(string Host = "");

        // Sub SwitchLog()
        string   SystemUpTime();
        TimeSpan SystemUpTimeTS();
        TimeSpan TimerValue(string            TimerName);
        void     TimerReset(string            TimerName);
        void     UnMuteAudio(string           Host = "");
        void     UnPauseAudio(string          Host = "");
        void     UnRegisterHelpLinks(string   plugin_name, string plugin_instance);
        void     UnRegisterLinkEx(WebPageDesc cbo);

        /// <summary>
        ///     ''' Unregister all pages of a certain type for a plugin instance
        ///     ''' </summary>
        ///     ''' <param name="pageType">The type of page to unregister. See HomeSeer.Jui.Types.EPageType for a list of values</param>
        ///     ''' <param name="id">unique id of the plugin</param>
        void UnregisterJuiPagesByType(int pageType, string id);

        void   UnRegisterStatusChangeCB(string script);
        string UnZip(string                    filename);
        string UnZipDest(string                filename, string destination);
        string UnZipDestIgnore(string          filename, string destination, bool IgnoreZipDirs);

        string UnZipEx(string filename,  string destination, bool IgnoreZipDirs,
                       bool   OverWrite, string password);

        string URLAction(string                    url,       string action, string data, string headers);
        int    ValidateScriptLicense(string        LicenseID, string ProductID);
        int    ValidateScriptLicenseDisplay(string LicenseID, string ProductID, bool bDisplay);
        string Version();
        void   WaitEvents();
        void   WaitSecs(double        secs);
        bool   WEBCheckUserRights(int rights);
        string WEBLoggedInUser();
        bool   WEBValidateUser(string username, string password);
        void   WindowsLockSystem();
        void   WindowsLogoffSystem();
        void   WindowsShutdownSystem();
        void   WindowsRebootSystem();
        string Zip(string      WhatToZip, string zipFileName);
        string ZipPass(string  WhatToZip, string zipFileName, string pass);
        string ZipLevel(string WhatToZip, string zipFileName, int    level, string pass);

        string ZipEx(string WhatToZip, string zipFileName, int level, string pass, bool RemoveBase, bool Flatten);
        // Sub LogFileAccess(ByVal allow_access As Boolean)
        // Function CheckRunStatus(ByVal app_name As String, ByVal timeout As Integer) As Boolean
        // Function Edition() As String
        // Function PRO100() As Boolean
        // Function PruneLogs(ByVal iDays As Integer) As Integer
        // Function UpdaterIsDownloading() As Boolean

        string LANIP();
        string WANIP();
        int    WebServerPort();
        int    WebServerSSLPort();

        // added in 3.0.0.35
        bool WriteHTMLImageFile(byte[]           ImageFile, string Dest, bool OverWrite);
        bool WriteHTMLImage(System.Drawing.Image Image,     string Dest, bool OverWrite);
        bool DeleteImageFile(string              DeleteFile);

        DateTime SolarNoon    { get; }
        string   Sunrise      { get; }
        DateTime SunriseDt    { get; }
        string   Sunset       { get; }
        DateTime SunsetDt     { get; }
        string   TimeZoneName { get; }

        int                      DaysLeftInYear();
        int                      DaysLeftInMonth();
        int                      DaysInMonth(DateTime    inDate);
        Constants.CD_DAY_EvenOdd EvenOddDay(DateTime     inDate);
        Constants.CD_DAY_EvenOdd EvenOddMonth(DateTime   indate);
        DateTime                 GetLastWeekday(DateTime ForMonth);

        DateTime GetSpecialDay(DayOfWeek                DOW,      Constants.CD_DAY_IS_TYPE inst,
                                               DateTime ForMonth, bool                     GetNext);

        bool IsSpecialDay(DateTime                                     inDate, DayOfWeek DOW,
                                              Constants.CD_DAY_IS_TYPE inst,   DateTime  ForMonth);

        bool IsWeekend(DateTime inDate);
        bool IsWeekday(DateTime inDate);
        int  LocalTimeZone();

        void Moon(DateTime                       dtStart, ref DateTime NMoon, ref DateTime FMoon, ref double CurCycle,
                                      ref string Desc);

        int WeeksLeftInYear();
        int WeeksLeftInYearEx(int WeekMode);
        int WeekNumber(DateTime   inDate);
        int WeekNumberEx(DateTime inDate,  int      WeekMode);
        int WeekEndDays(DateTime  dtStart, DateTime dtEnd);
        int WeekDays(DateTime     dtStart, DateTime dtEnd);

        string MailDate(int        index);
        void   MailDelete(short    index);
        string MailFrom(int        index);
        string MailFromDisplay(int index);
        int    MailMsgCount();
        string MailSubject(int   index);
        string MailText(int      index);
        string MailTo(int        index);
        string MailToDisplay(int index);
        int    MailTrigger();

        void SendEmail(string mto, string mfrom, string mCC, string mBCC, string msubject, string message,
                       string attach);

        ICAPIStatus   CAPIGetStatus(int  dvRef);
        CAPIControl[] CAPIGetControl(int dvRef);

        CAPIControl[] CAPIGetControlEx(int                  dvRef,
                                                       bool SingleRangeEntry);

        CAPIControl CAPIGetSingleControl(int                    dvRef,
                                                           bool SingleRangeEntry, string Label,
                                                           bool ExactCase,        bool   Contains);

        CAPIControl                   CAPIGetSingleControlByUse(int     dvRef, Constants.ePairControlUse UseType);
        Constants.CAPIControlResponse CAPIControlHandler(CAPIControl    CC);
        Constants.CAPIControlResponse CAPIControlsHandler(CAPIControl[] CC);


        // Procedures that use the 'Address' field. (Not guaranteed to be unique.)
        bool   DeviceExistsRef(int            dvRef);
        int    DeviceExistsAddress(string     Address, bool CaseSensitive);
        int    DeviceExistsAddressFull(string Address, bool CaseSensitive);
        int    DeviceExistsCode(string        Code);
        int    GetDeviceRef(string            sAddress);
        int    GetDeviceRefByName(string      device_name);
        int    GetDeviceParentRefByRef(int    @ref);
        string GetDeviceCode(string           Name);

        object GetDeviceByRef(int @ref);
        object GetDeviceEnumerator();
        object GetDeviceEnumeratorUser(string user);

        string GetNextVirtualCode();


        int    DeviceValue(int              dvRef);
        double DeviceValueEx(int            dvRef);
        int    DeviceValueByName(string     devname);
        double DeviceValueByNameEx(string   devname);
        void   SetDeviceValue(string        Address, double Value);
        void   SetDeviceValueByRef(int      dvRef,   double Valuenum, bool trigger);
        void   SetDeviceValueByName(string  devname, double Value);
        string DeviceString(int             dvRef);
        string DeviceStringByName(string    Name);
        void   SetDeviceString(int          dvRef,   string st,     bool reset);
        void   SetDeviceStringByName(string devname, string strval, bool reset_lastchange);


        bool DeviceScriptButton_Add(int    dvRef, string Label, string ScriptFile, string ScriptFunc,
                                    string ScriptParm,
                                    UInt16 Row, UInt16 Column, UInt16 ColumnSpan);

        bool     DeviceScriptButton_Delete(int    dvRef, string Label);
        void     DeviceScriptButton_DeleteAll(int dvRef);
        string[] DeviceScriptButton_List(int      dvRef);
        bool     DeviceScriptButton_Location(int  dvRef, string Label, UInt16 Row, UInt16 Column, UInt16 ColumnSpan);

        bool DeviceScriptButton_AddButton(int    dvRef, string Label, double Value, string ScriptFile,
                                          string ScriptFunc,
                                          string ScriptParm, UInt16 Row, UInt16 Column, UInt16 ColumnSpan);

        bool DeviceScriptButton_DeleteButton(int dvRef, double Value);
        bool DeviceScriptButton_Locate(int       dvRef, double Value, UInt16 Row, UInt16 Column, UInt16 ColumnSpan);


        int      DeviceTime(int          dvRef);
        int      DeviceTimeByName(string dev_name);
        DateTime DeviceDateTime(int      dvRef);
        void     SetDeviceLastChange(int dvRef, DateTime change_time);
        DateTime DeviceLastChange(string Address);
        DateTime DeviceLastChangeRef(int dvRef);


        string DeviceName(int  dvRef);
        bool   DeviceNoLog(int dvRef);
        bool   DeviceInvalidValue { get; set; }
        bool   DeleteDevice(int dvRef);


        bool IsOff(int dvRef);
        bool IsON(int  dvRef);

        int  NewDevice(string    Name);
        int  NewDeviceRef(string Name);
        bool IsOffByName(string  dev_name);
        bool IsOnByName(string   dev_name);

        // Other device related Procedures
        int DeviceCount { get; }

        bool Energy_SetEnergyDevice(
            int dvRef, Constants.enumEnergyDevice DeviceType);

        bool Energy_AddData(int dvRef, EnergyData Data);

        bool Energy_AddDataArray(
            int dvRef, EnergyData[] colData);

        int Energy_RemoveData(int                                              dvRef,
                                                                      DateTime dteStart);

        System.Collections.Generic.List<EnergyData> Energy_GetData(int      dvRef,
                                                                   DateTime dteStart,
                                                                   DateTime dteEnd);

        System.Collections.Generic.List<EnergyData>
            Energy_GetArchiveData(int dvRef, DateTime dteStart, DateTime dteEnd);

        System.Collections.Generic.List<EnergyData> Energy_GetArchiveDatas(
            string dvRefs, DateTime dteStart, DateTime dteEnd);

        string Energy_AddCalculator(
            int dvRef, string Name, TimeSpan Range, TimeSpan StartBack);

        string Energy_AddCalculatorEvenDay(
            int dvRef, string Name, TimeSpan Range, TimeSpan StartBack);

        int                                                Energy_CalcCount(int                 dvRef);
        EnergyCalcData                                     Energy_GetCalcByName(int             dvRef, string Name);
        EnergyCalcData                                     Energy_GetCalcByIndex(int            dvRef, int    Index);
        int                                                Energy_SaveGraphData(EnergyGraphData Data);
        EnergyGraphData                                    Energy_GetGraphData(int              ID);
        System.Collections.Generic.SortedList<int, string> Energy_GetGraphDataIDs();
        System.Collections.Generic.SortedList<int, string> Energy_GetEnergyRefs(bool GetParentRefs);

        System.Drawing.Image Energy_GetGraph(
            int id, string dvRefs, int width, int height, string format);

        // Sub DeviceValuesAdd(ByVal dvRef As Integer, ByVal values As String, ByVal show As Boolean)
        // The following two are RENAMED
        // Function DeviceVSP_Add(ByVal dvRef As Integer, ByVal Status As String, ByVal Value As Double, ByVal ControlStatus As ePairStatusControl, ByVal show As Boolean) As Boolean
        // Function DeviceVSP_AddRange(ByVal dvRef As Integer, ByVal Status As String, ByVal VStart As Double, ByVal VEnd As Double, ByVal ControlStatus As ePairStatusControl, ByVal show As Boolean, ByVal DecimalPlaces As Integer, ByVal RangeValPrefix As String, ByVal RangeValSuffix As String, ByVal IncludeValues As Boolean, ByVal ValueOffset As Double) As Boolean
        bool     DeviceVSP_AddPair(int        dvRef, VSPair Pair);
        bool     DeviceVSP_ChangePair(int     dvRef, VSPair Existing, Constants.ePairStatusControl NewType);
        int      DeviceVSP_CountStatus(int    dvRef);
        int      DeviceVSP_CountControl(int   dvRef);
        int      DeviceVSP_CountAll(int       dvRef);
        void     DeviceVSP_ClearAll(int       dvRef, bool   TrueConfirm);
        bool     DeviceVSP_ClearAny(int       dvRef, double Value);
        bool     DeviceVSP_ClearBoth(int      dvRef, double Value);
        bool     DeviceVSP_ClearControl(int   dvRef, double Value);
        bool     DeviceVSP_ClearStatus(int    dvRef, double Value);
        VSPair   DeviceVSP_Get(int            dvRef, double Value, Constants.ePairStatusControl VSPType);
        string   DeviceVSP_GetStatus(int      dvRef, double Value, Constants.ePairStatusControl VSPType);
        VSPair[] DeviceVSP_GetAllStatus(int   dvRef);
        bool     DeviceVSP_PairsProtected(int dvRef);

        bool   DeviceVGP_AddPair(int  dvRef, VGPair Pair);
        int    DeviceVGP_Count(int    dvRef);
        void   DeviceVGP_ClearAll(int dvRef, bool   TrueConfirm);
        bool   DeviceVGP_Clear(int    dvRef, double Value);
        VGPair DeviceVGP_Get(int      dvRef, double Value);

        string DeviceVGP_GetGraphic(int dvRef, double Value);

        // Function DeviceVGP_Add(ByVal dvRef As Integer, ByVal Graphic As String, ByVal Value As Double) As Boolean
        // Function DeviceVGP_AddRange(ByVal dvRef As Integer, ByVal Graphic As String, ByVal VStart As Double, ByVal VEnd As Double) As Boolean
        bool DeviceVGP_PairsProtected(int dvRef);


        // Overloads Sub DeviceProperty(ByVal dvRef As Integer, ByVal Prop As eDeviceProperty, ByVal Value As Double)
        void DeviceProperty_Int(int      dvRef, Constants.eDeviceProperty Prop, int              Value);
        void DeviceProperty_String(int   dvRef, Constants.eDeviceProperty Prop, string           Value);
        void DeviceProperty_StrArray(int dvRef, Constants.eDeviceProperty Prop, string[]         Value);
        void DeviceProperty_Boolean(int  dvRef, Constants.eDeviceProperty Prop, bool             Value);
        void DeviceProperty_DevType(int  dvRef, Constants.eDeviceProperty Prop, DeviceTypeInfo   Value);
        void DeviceProperty_Date(int     dvRef, Constants.eDeviceProperty Prop, DateTime         Value);
        void DeviceProperty_dvMISC(int   dvRef, Constants.eDeviceProperty Prop, Constants.dvMISC Value);
        void DeviceProperty_PlugData(int dvRef, Constants.eDeviceProperty Prop, clsPlugExtraData Value);


        string AddDeviceActionToEvent(int evRef, CAPIControl CC);
        int    NewEventEx(string          Name,  string      Group, string sType);
        int    NewEventGetRef(string      Name,  string      Group, string sType);
        bool   EventSetTimeTrigger(int    evRef, DateTime    DT);

        bool EventSetRecurringTrigger(int              evRef,         TimeSpan Frequency,
                                                  bool Once_Per_Hour, bool     Reference_To_Hour);

        bool EventSetVRTrigger(int evRef, string VR, Constants.enumVCMDType VRFor);

        void AddActionRunScript(int                @ref, string script, string method,
                                            string parms);

        void             DisableEvent(string          evname);
        void             DisableEventByRef(int        evref);
        void             DeleteAfterTrigger_Set(int   evRef);
        void             EnableEvent(string           evname);
        void             EnableEventByRef(int         evref);
        bool             EventEnabled(int             evRef);
        void             DeleteAfterTrigger_Clear(int evRef);
        void             DeleteEvent(string           evname);
        void             DeleteEventByRef(int         evRef);
        int              EventCount         { get; }
        clsPlugExtraData EventPlugExtraData { get; set; }
        bool             EventExists(string               event_name);
        bool             EventExistsByRef(int             evRef);
        string           EventName(int                    evRef);
        bool             EventNoLog(int                   evRef);
        int              GetEventRefByName(string         event_name);
        int              GetEventRefByNameAndGroup(string event_name, string event_group);
        int              TriggerEventAndWait(string       Name);
        int              TriggerEvent(string              Name);
        bool             TriggerEventByRef(int            evRef);
        int              TriggerEventEx(int               line, string Name, string voice_command);
        EventGroupData   Event_Group_Info(int             GroupRef);
        EventGroupData[] Event_Group_Info_All();
        EventData        Event_Info(int evRef);
        EventData[]      Event_Info_All();
        EventData[]      Event_Info_Group(int     GroupID);
        DateTime         GetEventTriggerTime(int  evref);
        string           GetEventVoiceCommand(int evref);

        LogEntry[] GetLog_Date(DateTime StartDate, DateTime EndDate);

        LogEntry[] GetLog_Date_Text(DateTime StartDate, DateTime EndDate, string mType, string mEntry,
                                    bool     mEntry_RegEx);

        LogEntry[] GetLog_Date_Priority(DateTime StartDate, DateTime EndDate, int Priority_Start, int Priority_End,
                                        bool     Show_No_Priority);

        LogEntry[] GetLog_Date_ErrorCode(DateTime StartDate, DateTime EndDate, int ErrorCode);

        LogEntry[] GetLog_FullFilter(DateTime StartDate,    DateTime EndDate,        string mType, string mEntry,
                                     bool     mEntry_RegEx, int      Priority_Start, int    Priority_End,
                                     bool     Show_No_Priority,
                                     int      ErrorCode, bool ShowAllErrorCode);

        void   WriteLog(string       mtype, string message);
        void   WriteLogEx(string     mtype, string message, string Color);
        void   WriteLogDetail(string mType, string Message, string Color, int Priority, string mFrom, int ErrorCode);
        void   ClearLog();
        bool   NoLog { get; set; }
        string LogGet();

    }

}