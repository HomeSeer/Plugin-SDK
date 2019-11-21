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

        /// <summary>
        /// Delete all devices, and their corresponding features, from the HomeSeer system that are managed by
        ///  the specified plugin interface
        /// </summary>
        /// <param name="interfaceName">
        /// The name of the interface that owns all of the devices and features to delete. This is usually the plugin Id
        /// </param>
        /// <returns>TRUE if the delete was successful, FALSE if there was a problem during the process.</returns>
        bool DeleteDevicesByInterface(string interfaceName);
        
        #endregion
        
        #region Control

        /// <summary>
        /// Set the value on a feature and trigger HomeSeer to process the update to update the status accordingly.
        /// <para>
        /// To update the value without triggering HomeSeer to process the update, call
        ///  <see cref="UpdatePropertyByRef"/>
        /// </para>
        /// </summary>
        /// <remarks>
        /// This is the same as the legacy method SetDeviceValueByRef(Integer, Double, True).
        /// </remarks>
        /// <param name="featRef">The unique reference of the feature to control</param>
        /// <param name="value">The new value to set on the feature</param>
        /// <returns>TRUE if the control sent correctly, FALSE if there was a problem</returns>
        bool UpdateFeatureValueByRef(int featRef, double value);
        /// <summary>
        /// Set the value on a feature by string and trigger HomeSeer to process the update to update the status
        ///  accordingly
        /// </summary>
        /// <remarks>
        /// This is the same as the legacy method SetDeviceString(Integer, String, True)
        /// </remarks>
        /// <param name="featRef">The unique reference of the feature to control</param>
        /// <param name="value">The new value to set on the feature</param>
        /// <returns>TRUE if the control sent correctly, FALSE if there was a problem</returns>
        bool UpdateFeatureValueStringByRef(int featRef, string value);

        //bool SendControlForFeatureByValue(int featRef, double value);
        
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
        TrigActInfo[] GetTriggersByType(string pluginName, int trigId);
        void TriggerFire(string pluginName, TrigActInfo trigInfo);

        #endregion
        
        #region System
        
        string Version();
        Constants.editions GetHSEdition();
        string GetUsers();        
        bool IsLicensed();
        bool IsRegistered();
        
        /// <summary>
        /// Determine if Location1 is used first on devices/features.
        /// </summary>
        /// <remarks>
        /// By default, Location2 is used as the first logical location when organizing devices/features.
        ///  For this reason, it is important to check which location is marked as the first location before working
        ///  with locations.
        /// </remarks>
        /// <returns>TRUE if Location1 is used first, FALSE if Location2 is used first</returns>
        bool IsLocation1First();
        
        /// <summary>
        /// Get an alpha-sorted list of Location1 strings
        /// </summary>
        /// <returns>A SortedList of Location1 location strings</returns>
        System.Collections.SortedList GetLocationsList();
        
        /// <summary>
        /// Get the name of the Location1 location
        /// </summary>
        /// <returns>The user defined name of Location1</returns>
        string GetLocation1Name();
        
        /// <summary>
        /// Get an alpha-sorted list of Location2 strings
        /// </summary>
        /// <returns>A SortedList of Location2 location strings</returns>
        System.Collections.SortedList GetLocations2List();
        
        /// <summary>
        /// Get the name of the Location2 location
        /// </summary>
        /// <returns>The user defined name of Location2</returns>
        string GetLocation2Name();

        /// <summary>
        /// Get the name of the first location.
        /// <para>
        /// This is the name of the location that is marked as first according to <see cref="IsLocation1First"/>
        /// </para>
        /// </summary>
        /// <returns>The name of the first location</returns>
        string GetFirstLocationName();
        
        /// <summary>
        /// Get the name of the second location.
        /// <para>
        /// This is the name of the location that is marked as second according to <see cref="IsLocation1First"/>
        /// </para>
        /// </summary>
        /// <returns>The name of the second location</returns>
        string GetSecondLocationName();
        
        /// <summary>
        /// Get an alpha-sorted list of the location strings marked as first
        /// <para>
        /// This is the list of location strings that are marked as first according to <see cref="IsLocation1First"/>
        /// </para>
        /// </summary>
        /// <returns>A List of the first location strings</returns>
        List<string> GetFirstLocationList();
        
        /// <summary>
        /// Get an alpha-sorted list of the location strings marked as second
        /// <para>
        /// This is the list of location strings that are marked as second according to <see cref="IsLocation1First"/>
        /// </para>
        /// </summary>
        /// <returns>A List of the second location strings</returns>
        List<string> GetSecondLocationList();
        
        int CheckRegistrationStatus(string piname);

        int GetOsType();
        
        /// <summary>
        /// Obtain the IP address the HomeSeer system is accessible through
        /// </summary>
        /// <returns>A string representation of the IP address HomeSeer is running on</returns>
        string GetIpAddress();
        
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
        
        #region Speech
        
        /// <summary>
        /// This procedure is used to cause HomeSeer to speak something when a speak proxy is registered and active.
        ///  Since speak commands when a speak proxy plug-in is registered are trapped and passed to the SpeakIn
        ///  procedure of the speak proxy plug-in, this command is used when the speak proxy plug-in is ready to do
        ///  the real speaking.
        /// </summary>
        /// <param name="speechDevice">
        /// This is the device that is to be used for the speaking.  In older versions of HomeSeer, this value was
        ///  used to indicate the sound card to use, and if it was over 100, then it indicated that it was speaking
        ///  for HomeSeer Phone (device - 100 = phone line), or the WAV audio device to use.
        ///  Although this is still used for HomeSeer Phone, speaks for HomeSeer phone are never proxied and so
        ///  values >= 100 should never been seen in the device parameter.
        ///  Pass the device parameter unchanged to SpeakProxy.
        /// </param>
        /// <param name="spokenText">
        /// This is the text to be spoken, or if it is a WAV file to be played, then the characters ":\" will be
        ///  found starting at position 2 of the string as playing a WAV file with the speak command in HomeSeer
        ///  REQUIRES a fully qualified path and filename of the WAV file to play.
        /// </param>
        /// <param name="wait">
        /// This parameter tells HomeSeer whether to continue processing commands immediately or to wait until
        ///  the speak command is finished - pass this parameter unchanged to SpeakProxy.
        /// </param>
        /// <param name="host">
        /// This is a list of host:instances to speak or play the WAV file on.
        ///  An empty string or a single asterisk (*) indicates all connected speaker clients on all hosts.
        ///  Normally this parameter is passed to SpeakProxy unchanged.
        /// </param>
        void SpeakProxy(int speechDevice, string spokenText, bool wait, string host = "");

        /// <summary>
        /// Sends TTS to a file using the system voice
        /// </summary>
        /// <param name="Text">The text to speak</param>
        /// <param name="Voice">The voice to use, SAPI only on Windows</param>
        /// <param name="FileName">Filename to send the speech to</param>
        /// <returns></returns>
        bool SpeakToFileV2(string Text, string Voice, string FileName);

        #if WIP
        /// <summary>
        /// Register your plug-in as a Speak Proxy plug-in.
        /// <para>
        /// After this registration, whenever a Speak command is issued in HomeSeer,
        ///  your plug-in's SpeakIn procedure will be called instead.
        ///  When your plug-in wishes to have HomeSeer actually speak something, it uses SpeakProxy instead of Speak.
        /// </para>
        /// <para>
        /// If you no longer wish to proxy Speak commands in your plug-in, or when your plug-in has its Shutdown
        ///  procedure called, use UnRegisterProxySpeakPlug to remove the registration as a speak proxy.
        /// </para>
        /// </summary>
        /// <param name="pluginId">The Id of your plugin</param>
        void RegisterProxySpeakPlug(string pluginId);
        
        /// <summary>
        /// Unregister a plug-in as a Speak proxy that was previously
        ///  registered using RegisterProxySpeakPlug.
        /// </summary>
        /// <param name="pluginId">The Id of your plugin</param>
        void UnRegisterProxySpeakPlug(string pluginId);
#endif

        #endregion

        /// <summary>
        /// HomeSeer supports the use of replacement variables, which is the use of special tags to indicate where
        ///  HomeSeer should replace the tag with text information.  A full list of replacement variables is listed
        ///  in HomeSeer's help file.
        /// </summary>
        /// <param name="strIn">A string with the replacement variables</param>
        /// <returns>A string with the replacement variables removed with the indicated values put in their place</returns>
        string ReplaceVariables(string strIn);

        /// <summary>
        /// Returns the path to the HS executable. Some plugins need this when running remotely
        /// </summary>
        /// <returns>The path to the HomeSeer executable</returns>
        string GetAppPath();
        
        #region Images

        /// <summary>
        /// Save the specified image, as a byte array, to file in the HomeSeer html images directory
        /// </summary>
        /// <param name="imageBytes">A byte array of the image to save</param>
        /// <param name="destinationFile">The path of the image following "\html\images\"</param>
        /// <param name="overwriteExistingFile">TRUE to overwrite any existing file, FALSE to not</param>
        /// <returns>TRUE if the file was saved successfully, FALSE if there was a problem</returns>
        /// <example>
        /// The following example shows how to download an image from a URL and save the bytes to file from the HSPI class.
        /// 
        /// <code>
        /// var url = "http://homeseer.com/images/HS4/hs4-64.png";
        /// var webClient = new WebClient();
        /// var imageBytes = webClient.DownloadData(url);
        /// var filePath = $"{Id}\\{Path.GetFileName(url)}";
        /// if (!HomeSeerSystem.SaveImageFile(imageBytes, filePath, true)) {
        ///     Console.WriteLine($"Error saving {url} to {filePath}");
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// The following example shows how to convert an image to bytes and save them from the HSPI class.
        ///
        /// <code>
        /// var myImage = System.Drawing.Image.FromFile("sampleImage.png");
        /// var imageBytes = new byte[0];
        /// using (var ms = new MemoryStream()) {
        ///     myImage.Save(ms, myImage.RawFormat);
        ///     imageBytes = ms.toArray();
        /// }
        /// var filePath = $"{Id}\\sampleImage.png";
        /// if (!HomeSeerSystem.SaveImageFile(imageBytes, filePath, true)) {
        ///     Console.WriteLine($"Error saving sampleImage.png to {filePath}");
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="DeleteImageFile"/>
        bool SaveImageFile(byte[] imageBytes, string destinationFile, bool overwriteExistingFile);
        /// <summary>
        /// Delete the specified file from HomeSeer's HTML image directory.
        /// </summary>
        /// <param name="targetFile">The path of the image following "\html\images\"</param>
        /// <returns>TRUE if the file was deleted successfully, FALSE if it still exists</returns>
        /// <example>
        /// The following example shows how to delete an image from HomeSeer's HTML image directory.
        /// 
        /// <code>
        /// var filePath = $"{Id}\\sampleImage.png";
        /// if (!HomeSeerSystem.DeleteImageFile(filePath)) {
        ///     Console.WriteLine($"Error deleting {filePath}");
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="SaveImageFile"/>
        bool DeleteImageFile(string targetFile);

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
        //string GetLastRemoteIP();
        //string LANIP();
        //string WANIP();
        //int WebServerPort();
        //int WebServerSSLPort();

        //string GenCookieString(string Name, string Value, string expire = "", string path = "/");

        #endregion

        #region AppCallback

        //TODO AppCallback methods
        
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