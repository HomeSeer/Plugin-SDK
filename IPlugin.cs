using System;
using System.Collections.Generic;
using HomeSeer.PluginSdk.CAPI;

namespace HomeSeer.PluginSdk {

    public interface IPlugin {

    /// <summary>
    /// Return True if HomeSeer is to manage the communications (COM) port for this plug-in
    /// </summary>
    /// <remarks>
    /// A COM port selection box will be displayed on the interfaces page so the user can enter a COM port number. 
    /// If the plug-in supports multiple instances, the COM port will be managed for each instance. 
    /// When InitIO is called in the plug-in the selected COM port will passed as a parameter.
    /// </remarks>
    /// <returns>TRUE if HomeSeer should manage the COM port for this plugin, FALSE if not</returns>
    bool HSCOMPort { get; }
    /// <summary>
    /// The name of the plugin
    /// <para>
    /// Do NOT use special characters in your plug-in name with the exception of "-", ".", and " " (space).
    /// </para>
    /// <para>
    /// This is used to identify your plug-in to HomeSeer and your users. 
    /// Keep the name to 16 characters or less. 
    /// </para>
    /// </summary>
    /// <remarks>
    /// Do not access any hardware in this function as HomeSeer will call this function using 
    /// .NET reflection when it scans all plug-in EXE files so it should only return the 
    /// text string of your plug-in.
    /// </remarks>
    /// <returns>The name of the plugin</returns>
    string Name { get; }
    
    bool HasSettings { get; }

    /// <summary>
    /// Return the access level of this plug-in. Access level is the licensing mode.
    /// </summary>
    /// <returns>
    /// 1 = Plug-in is not licensed and may be enabled and run without purchasing a license. Use this value for free plug-ins.
    /// 2 = Plug-in Is licensed And a user must purchase a license in order to use this plug-in. When the plug-in Is first enabled, it will will run as a trial for 30 days.
    /// </returns>
    int AccessLevel();
    int Capabilities();
    string ConfigDevice(int @ref, string user, int userRights, bool newDevice);   // changed name and parameter
    Constants.ConfigDevicePostReturn ConfigDevicePost(int @ref, string data, string user, int userRights);  // changed paramters
    string GetPagePlugin(string page, string user, int userRights, string queryString);
    
    /// <summary>
    /// When you wish to have HomeSeer call back in to your plug-in or application when certain events 
    /// happen in the system, call the RegisterEventCB procedure and provide it with event you wish to monitor.  
    /// See RegisterEventCB for more information and an example and event types.
    /// <para>
    /// The parameters are passed in an array of objects.  Each entry in the array is a parameter.  
    /// The number of entries depends on the type of event and are described below.  
    /// The event type is always present in the first entry or parms(0).
    /// </para>
    /// </summary>
    /// <param name="EventType"></param>
    /// <param name="parms"></param>
    void HSEvent(Constants.HSEvent EventType, object[] parms);
    string InitIO(string port);
    string InstanceFriendlyName();
    InterfaceStatus InterfaceStatus();
    /// <summary>
    /// Interface for plugin specific calls
    /// </summary>
    /// <param name="procName"></param>
    /// <param name="parms"></param>
    /// <returns></returns>
    object PluginFunction(string procName, object[] parms);
    object PluginPropertyGet(string procName, object[] parms);
    void PluginPropertySet(string procName, object value);
    PollResultInfo PollDevice(int dvref);
    string PostBackProc(string page, string data, string user, int userRights);
    bool RaisesGenericCallbacks();
    Constants.SearchReturn[] Search(string SearchString, bool RegEx);
    void SetIOMulti(System.Collections.Generic.List<CAPIControl> colSend);
    void ShutdownIO();
    void SpeakIn(int device, string txt, bool w, string host);
    bool SupportsAddDevice();
    bool SupportsConfigDevice();
    bool SupportsConfigDeviceAll();
    bool SupportsMultipleInstances();
    bool SupportsMultipleInstancesSingleEXE();

    /// <summary>
    /// Whether this plugin supports advanced edit mode or not
    /// <para>
    /// Set to TRUE to enable advanced mode, you should display advanced controls.
    /// </para>
    /// </summary>
    /// <remarks>
    /// The HomeSeer events page has an option to set the editing mode to "Advanced Mode."  
    /// This is typically used to enable options that may only be of interest to advanced users or programmers. 
    /// The Set in this function is called when advanced mode is enabled. 
    /// Your plug-in can also enable this mode if an advanced selection was saved and needs to be displayed.
    /// </remarks>
    /// <returns>TRUE if advanced mode is set, you may enable this mode if you detect advanced selections have already been made.</returns>
    bool ActionAdvancedMode { get; set; }
    /// <summary>
    /// Return the name of the action given an action number
    /// <para>The name of the action will be displayed in the HomeSeer events actions list.</para>
    /// </summary>
    /// <param name="ActionNumber">The number of the action to get the name for</param>
    /// <returns>The name of the action associated with the action number</returns>
    string ActionName { get; }

    string ActionBuildUI(string sUnique, TrigActInfo ActInfo);
    bool ActionConfigured(TrigActInfo ActInfo);
    int ActionCount();
    string ActionFormatUI(TrigActInfo ActInfo);
    MultiReturn ActionProcessPostUI(System.Collections.Specialized.NameValueCollection PostData, TrigActInfo TrigInfoIN);
    bool ActionReferencesDevice(TrigActInfo ActInfo, int dvRef);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ActInfo"></param>
    /// <returns>true = OK, false = error</returns>
    bool HandleAction(TrigActInfo ActInfo);

    /// <summary>
    /// HomeSeer will set this to TRUE if the trigger is being used as a CONDITION
    /// <para>
    /// Check this value in BuildUI and other procedures to change how the trigger is rendered if it is being used as a condition or a trigger.
    /// </para>
    /// </summary>
    /// <param name="TrigInfo"></param>
    /// <returns></returns>
    bool Condition { get; set; }
    /// <summary>
    /// Called by HomeSeer to determine if a given trigger can also be used as a condition
    /// </summary>
    /// <param name="TriggerNumber">The number of the trigger to check</param>
    /// <returns>TRUE if the given trigger can also be used as a condition, for the given trigger number.</returns>
    bool HasConditions { get; }
    /// <summary>
    /// Whether the plugin has triggers or not
    /// </summary>
    /// <returns>True if your plugin contains any triggers, else return false</returns>
    bool HasTriggers { get; }
    /// <summary>
    /// Returns the number of sub triggers the plugin supports for the specified trigger number
    /// </summary>
    /// <param name="TriggerNumber">The number of the trigger to check</param>
    /// <returns>The number of sub triggers the specified trigger number supports</returns>
    int SubTriggerCount { get; }
    /// <summary>
    /// The name of the sub trigger with the specified number of the trigger with the specified number
    /// </summary>
    /// <param name="TriggerNumber">The number of the trigger to check</param>
    /// <param name="SubTriggerNumber">The number of the sub trigger to check</param>
    /// <returns>The name of the sub trigger</returns>
    string SubTriggerName { get; }
    /// <summary>
    /// Given a TrigActInfo object, detect if this trigger is configured properly
    /// </summary>
    /// <param name="TrigInfo">The trigger info to validate</param>
    /// <returns>TRUE if the trigger is configured properly, FALSE otherwise</returns>
    bool TriggerConfigured { get; }
    /// <summary>
    /// Returns the number of triggers the plugin supports
    /// </summary>
    /// <returns></returns>
    int TriggerCount { get; }
    /// <summary>
    /// Return the name of the given trigger based on the specified trigger number
    /// </summary>
    /// <param name="TriggerNumber">The trigger number to get the name for</param>
    /// <returns>The name of the trigger</returns>
    string TriggerName { get; }

    string TriggerBuildUI(string sUnique, TrigActInfo TrigInfo);
    string TriggerFormatUI(TrigActInfo TrigInfo);
    MultiReturn TriggerProcessPostUI(System.Collections.Specialized.NameValueCollection PostData, TrigActInfo TrigInfoIN);
    bool TriggerReferencesDevice(TrigActInfo TrigInfo, int dvRef);
    bool TriggerTrue(TrigActInfo TrigInfo);
    
    /// <summary>
    /// Unique ID for this plugin, needs to be unique for all plugins
    /// </summary>
    string ID { get; }

    /// <summary>
    /// Called by the HomeSeer software to run a particular procedure based on the action ID specified.  
    /// This is called when a user clicks a button on one of the HS-JUI pages.
    /// </summary>
    /// <param name="actionId">The ID of the action to execute</param>
    /// <param name="params">A map of view IDs and values</param>
    /// <returns>A JSON serialized Jui.Message or Jui.Form</returns>
    string ExecuteActionById(string actionId, Dictionary<string, string> @params);

    /// <summary>
    /// Called by the HomeSeer software to obtain a HS-JUI page by ID serialized into a JSON string
    /// </summary>
    /// <param name="pageId">The ID of the page to get</param>
    /// <returns>A JSON serialized Jui.Page</returns>
    [Obsolete("All feature pages should be driven by HTML and settings pages are now handled with GetJuiSettingsPages()")]
    string GetJuiPagePlugin(string pageId);

    /// <summary>
    /// Called by the HomeSeer software to obtain a HS-JUI device configuration page for a specific device
    /// </summary>
    /// <param name="deviceRef">The device reference to get the page for</param>
    /// <returns>A JSON serialized Jui.Page</returns>
    string GetJuiDeviceConfigPage(string deviceRef);

    /// <summary>
    /// Called by the HomeSeer software to obtain a list of settings pages
    /// </summary>
    /// <returns>
    /// A List of Pages serialized to JSON strings
    /// </returns>
    List<string> GetJuiSettingsPages();

    /// <summary>
    /// Save updated values for a HS-JUI formatted device config page
    /// </summary>
    /// <param name="pageContent">A JSON serialized Jui.Page describing what has changed about the page</param>
    /// <param name="deviceRef">The reference of the device the config page is for</param>
    /// <returns></returns>
    string SaveJuiDeviceConfigPage(string pageContent, int deviceRef);

    /// <summary>
    /// Save updated values for a HS-JUI formatted page
    /// </summary>
    /// <param name="pageContent">A JSON serialized Jui.Page describing what has changed about the page</param>
    /// <returns></returns>
    [Obsolete("All feature pages should be driven by HTML and settings pages are now saved with SaveJuiSettingsPages()")]
    string SaveJuiPage(string pageContent);

    /// <summary>
    /// Save changes to settings pages
    /// </summary>
    /// <param name="jsonString">A List of Jui.Pages containing views that have changed, serialized as JSON</param>
    /// <returns>
    /// TRUE if the save was successful; FALSE if it was unsuccessful. 
    /// <para>
    /// An exception should be thrown with details about the error if it was unsuccessful
    /// </para>
    /// </returns>
    bool SaveJuiSettingsPages(string jsonString);

    /// <summary>
    /// Called by the HomeSeer software to determine if this plugin allows for device configuration, in HS-JUI format, via the device utility page
    /// </summary>
    /// <returns>
    /// TRUE if the plugin supports a HS-JUI device configuration page
    /// FALSE if the plugin does not support a HS-JUI device configuration page
    /// </returns>
    bool SupportsConfigDeviceJui();

    }

}