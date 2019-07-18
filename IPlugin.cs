using System.Collections.Generic;
using HomeSeer.PluginSdk.Devices;

namespace HomeSeer.PluginSdk {

    /// <summary>
    /// The core plugin interface used by HomeSeer to interact with third-party plugins.
    /// <para>
    /// The <see cref="AbstractPlugin"/> class provides a default implementation of this interface
    ///  that should be used to develop plugins.
    /// </para>
    /// </summary>
    public interface IPlugin {
        
        #region Properties

        /// <summary>
        /// Unique ID for this plugin, needs to be unique for all plugins
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// Whether HomeSeer is to manage the communications (COM) port for this plug-in or not
        /// <para>
        /// TRUE if HomeSeer should manage the COM port for this plugin, FALSE if not
        /// </para>
        /// </summary>
        /// <remarks>
        /// A COM port selection box will be displayed on the interfaces page so the user can enter a COM port number. 
        /// When InitIO is called in the plug-in the selected COM port will passed as a parameter.
        /// </remarks>
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
        string Name { get; }
        
        /// <summary>
        /// Whether the plugin has settings pages or not.
        /// <para>
        /// If this is TRUE, you must return valid JUI data in GetJuiSettingsPages()
        /// </para>
        /// </summary>
        bool HasSettings { get; }
    
        //TODO AccessLevel -> What are we doing with this? Leaving as is?
        /// <summary>
        /// Return the access level of this plug-in. Access level is the licensing mode.
        /// <para>
        /// 1 = Plug-in is not licensed and may be enabled and run without purchasing a license. Use this value for free plug-ins.
        /// 2 = Plug-in is licensed and a user must purchase a license in order to use this plug-in. When the plug-in Is first enabled, it will will run as a trial for 30 days.
        /// </para>
        /// </summary>
        int AccessLevel { get; }
        
        /// <summary>
        /// Whether this plugin supports a device configuration page for devices created/managed by it
        /// <para>
        /// TRUE will cause HomeSeer to call GetJuiDeviceConfigPage() for devices this plugin manages.
        ///   FALSE means HomeSeer will not call GetJuiDeviceConfigPage() for any devices
        /// </para>
        /// </summary>
        bool SupportsConfigDevice    { get; }
        /// <summary>
        /// Whether this plugin supports a device configuration page for all devices
        /// <para>
        /// TRUE will cause HomeSeer to call GetJuiDeviceConfigPage() for every device.
        ///   FALSE means HomeSeer will not call GetJuiDeviceConfigPage() for all devices
        /// </para>
        /// </summary>
        bool SupportsConfigDeviceAll { get; }
        
        //TODO RaisesGenericCallbacks -> What does this do? documentation
        /// <summary>
        /// Indicates to HomeSeer that this plugin...
        /// <para>TRUE to indicate that the plugin should receive device change events</para>
        /// <para>FALSE to indicate that the plugin should not receive any device change events</para>
        /// </summary>
        bool RaisesGenericCallbacks { get; }
        
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
        /// The number of unique event actions the plugin supports
        /// </summary>
        int ActionCount { get; }
        
        /// <summary>
        /// Whether the plugin has triggers or not
        /// </summary>
        bool HasTriggers { get; }
        
        /// <summary>
        /// The number of unique event triggers the plugin supports
        /// </summary>
        int TriggerCount { get; }
        
        #endregion
        
        #region Startup, Status, and Shutdown
        
        //TODO InitIO -> What are going to do with the port parameter now?
        /// <summary>
        /// Called by the HomeSeer system to initialize the plugin.
        /// <para>
        /// This is the primary entry point for all plugins.  Start the plugin and get it ready for use.
        /// </para>
        /// </summary>
        /// <param name="port"></param>
        /// <returns>
        /// TRUE if the plugin started successfully; FALSE if it did not
        /// <para>
        /// You should opt for throwing an exception that contains a detailed messaged over
        ///  returning FALSE whenever possible.
        /// </para>
        /// </returns>
        bool InitIO(string port);
        
        /// <summary>
        /// Called by the HomeSeer system to determine the status of the plugin.
        /// </summary>
        /// <returns>A PluginStatus object describing the state of the plugin</returns>
        PluginStatus OnStatusCheck();
        
        /// <summary>
        /// Called by the HomeSeer system to shutdown the plugin and its operations
        /// </summary>
        void ShutdownIO();
        
        #endregion
        
        #region Devices
        
        //TODO string ConfigDevice(int @ref, string user, int userRights, bool newDevice) -> replaced with new JUI Config Page
        //TODO Constants.ConfigDevicePostReturn ConfigDevicePost(int @ref, string data, string user, int userRights) -> replaced with new JUI Config Page
        /// <summary>
        /// Called by the HomeSeer system when a device that this plugin owns is controlled.
        /// <para>
        /// A plugin owns a device when its Interface property is set to the plugin ID.
        /// </para>
        /// </summary>
        /// <param name="colSend">
        /// A collection of <see cref="DeviceControlEvent"/> objects,
        ///  one for each device being controlled
        /// </param>
        void SetIOMulti(List<DeviceControlEvent> controlEvents);
        
        /// <summary>
        /// Called by the HomeSeer software to obtain a HS-JUI device configuration page for a specific device
        /// </summary>
        /// <param name="deviceRef">The device reference to get the page for</param>
        /// <returns>A JSON serialized Jui.Page</returns>
        string GetJuiDeviceConfigPage(string deviceRef);
    
        /// <summary>
        /// Save updated values for a HS-JUI formatted device config page
        /// </summary>
        /// <param name="pageContent">A JSON serialized Jui.Page describing what has changed about the page</param>
        /// <param name="deviceRef">The reference of the device the config page is for</param>
        /// <returns></returns>
        string SaveJuiDeviceConfigPage(string pageContent, int deviceRef);
        
        /// <summary>
        /// Called by the HomeSeer system when it needs the current status for a device owned by the plugin.
        /// <para>
        /// This should force the device to report its current status to HomeSeer.
        /// </para>
        /// </summary>
        /// <param name="devRef">The reference ID of the device to poll</param>
        /// <returns>A <see cref="PollResultInfo"/> describing the current state of the device</returns>
        PollResultInfo PollDevice(int devRef);
        
        #endregion
        
        #region Settings
        
        /// <summary>
        /// Called by the HomeSeer software to obtain a list of settings pages
        /// </summary>
        /// <returns>
        /// A SettingsCollection serialized to a JSON string
        /// </returns>
        string GetJuiSettingsPages();
        
        /// <summary>
        /// Called by the HomeSeer system when settings changes need to be saved
        /// </summary>
        /// <param name="jsonString">A List of Jui.Pages containing views that have changed, serialized as JSON</param>
        /// <returns>
        /// TRUE if the save was successful; FALSE if it was unsuccessful. 
        /// <para>
        /// An exception should be thrown with details about the error if it was unsuccessful
        /// </para>
        /// </returns>
        bool SaveJuiSettingsPages(string jsonString);
        
        #endregion
        
        #region Events
        
        /// <summary>
        /// When you wish to have HomeSeer call back in to your plug-in or application when certain events 
        /// happen in the system, call the RegisterEventCB procedure and provide it with event you wish to monitor.  
        /// See RegisterEventCB for more information and an example and event types.
        /// <para>
        /// The parameters are passed in an array of objects.  Each entry in the array is a parameter.  
        /// The number of entries depends on the type of event and are described below.  
        /// The event type is always present in the first entry or params(0).
        /// </para>
        /// </summary>
        /// <param name="eventType">The type of event that has occurred</param>
        /// <param name="params">The data associated with the event</param>
        void HsEvent(Constants.HSEvent eventType, object[] @params);
        
        //TODO bool Condition -> Move into TrigActInfo as a property (IsCondition)

        #region Actions
        
        /// <summary>
        /// Return the name of the action given an action number
        /// <para>The name of the action will be displayed in the HomeSeer events actions list.</para>
        /// </summary>
        /// <param name="actionNum">The number of the action to get the name for</param>
        /// <returns>The name of the action associated with the action number</returns>
        string GetActionNameByNumber(int actionNum);
    
        /// <summary>
        /// Called by the HomeSeer system when an event is in edit mode and in need of HTML controls for the user.
        /// </summary>
        /// <param name="unique">A unique string that can be used with your HTML controls to identify the control. All controls need to have a unique ID.</param>
        /// <param name="actInfo">Object that contains information about the action like current selections.</param>
        /// <returns>HTML controls that need to be displayed so the user can select the action parameters.</returns>
        string ActionBuildUI(string unique, TrigActInfo actInfo);
        
        /// <summary>
        /// Called by the HomeSeer system to verify that the configuration is valid and can be saved
        /// </summary>
        /// <param name="actInfo">Object describing the action.</param>
        /// <returns>TRUE if the given action is configured properly; FALSE if the action shouldn't be saved</returns>
        bool ActionConfigured(TrigActInfo actInfo);

        string ActionFormatUI(TrigActInfo actInfo);
        
        /// <summary>
        /// Called by the HomeSeer system to process selections when a user edits your event actions.
        /// </summary>
        /// <param name="postData">A collection of name value pairs that include the user's selections.</param>
        /// <param name="trigInfoIn">Object that contains information about the action.</param>
        /// <returns>Object the holds the parsed information for the action. HomeSeer will save this information for you in the database.</returns>
        MultiReturn ActionProcessPostUI(System.Collections.Specialized.NameValueCollection postData, TrigActInfo trigInfoIn);
        
        /// <summary>
        /// Called by the HomeSeer system to determine if a specified device is referenced by a certain action.
        /// </summary>
        /// <param name="actInfo">Object describing the action.</param>
        /// <param name="devRef">The reference ID of the device to check</param>
        /// <returns>TRUE if the action references the device; FALSE if it does not</returns>
        bool ActionReferencesDevice(TrigActInfo actInfo, int devRef);

        /// <summary>
        /// Called by the HomeSeer system when an event is triggered and the plugin needs to carry out a specific action.
        /// </summary>
        /// <param name="actInfo">Object describing the trigger and action.</param>
        /// <returns>TRUE if the action was executed successfully; FALSE if there was an error</returns>
        bool HandleAction(TrigActInfo actInfo);
        
        #endregion
        
        #region Triggers

        /// <summary>
        /// Called by HomeSeer to determine if a given trigger can also be used as a condition
        /// </summary>
        /// <param name="triggerNum">The number of the trigger to check</param>
        /// <returns>TRUE if the given trigger can also be used as a condition, for the given trigger number.</returns>
        bool TriggerHasConditions(int triggerNum);

        /// <summary>
        /// Returns the number of sub triggers the plugin supports for the specified trigger number
        /// </summary>
        /// <param name="triggerNum">The number of the trigger to check</param>
        /// <returns>The number of sub triggers the specified trigger number supports</returns>
        int GetSubTriggerCount(int triggerNum);

        /// <summary>
        /// The name of the sub trigger with the specified number of the trigger with the specified number
        /// </summary>
        /// <param name="triggerNum">The number of the trigger to check</param>
        /// <param name="subTriggerNum">The number of the sub trigger to check</param>
        /// <returns>The name of the sub trigger</returns>
        string GetSubTriggerNameByNumber(int triggerNum, int subTriggerNum);

        /// <summary>
        /// Given a TrigActInfo object, detect if this trigger is configured properly
        /// </summary>
        /// <param name="trigInfo">The trigger info to validate</param>
        /// <returns>TRUE if the trigger is configured properly, FALSE otherwise</returns>
        bool IsTriggerConfigValid(TrigActInfo trigInfo);

        /// <summary>
        /// Return the name of the given trigger based on the specified trigger number
        /// </summary>
        /// <param name="triggerNum">The trigger number to get the name for</param>
        /// <returns>The name of the trigger</returns>
        string GetTriggerNameByNumber(int triggerNum);
    
        /// <summary>
        /// Called by the HomeSeer system when an event is in edit mode and in need of HTML controls for the user.
        /// </summary>
        /// <param name="unique">A unique string that can be used with your HTML controls to identify the control. All controls need to have a unique ID.</param>
        /// <param name="trigInfo">Object that contains information about the trigger like current selections.</param>
        /// <returns>HTML controls that need to be displayed so the user can select the action parameters.</returns>
        string TriggerBuildUI(string unique, TrigActInfo trigInfo);
        
        string TriggerFormatUI(TrigActInfo trigInfo);
        
        /// <summary>
        /// Called by the HomeSeer system to process selections when a user edits your event triggers.
        /// </summary>
        /// <param name="postData">A collection of name value pairs that include the user's selections.</param>
        /// <param name="trigInfoIn">Object that contains information about the trigger.</param>
        /// <returns>Object the holds the parsed information for the trigger. HomeSeer will save this information for you in the database.</returns>
        MultiReturn TriggerProcessPostUI(System.Collections.Specialized.NameValueCollection postData, TrigActInfo trigInfoIn);
        
        /// <summary>
        /// Called by the HomeSeer system to determine if a specified device is referenced by a certain trigger.
        /// </summary>
        /// <param name="trigInfo">Object describing the trigger.</param>
        /// <param name="devRef">The reference ID of the device to check</param>
        /// <returns>TRUE if the trigger references the device; FALSE if it does not</returns>
        bool TriggerReferencesDevice(TrigActInfo trigInfo, int devRef);
        
        /// <summary>
        /// Called by HomeSeer when a trigger needs to be evaluated as a condition
        /// </summary>
        /// <param name="trigInfo">Object describing the trigger</param>
        /// <param name="isCondition">TRUE if the trigger represents a condition, FALSE if it is a trigger</param>
        /// <returns>TRUE if the conditions are met; FALSE if they are not</returns>
        bool TriggerTrue(TrigActInfo trigInfo, bool isCondition = false);
        
        #endregion
        
        #endregion
        
        #region Dynamic Method/Property Calls
        
        /// <summary>
        /// Called by the HomeSeer system to run a plugin function by name using reflection
        /// </summary>
        /// <param name="procName">The name of the method to execute</param>
        /// <param name="params">The parameters to execute the method with</param>
        /// <returns>The result of the method execution</returns>
        object PluginFunction(string procName, object[] @params);
        /// <summary>
        /// Called by the HomeSeer system to get the value of a property by name using reflection
        /// </summary>
        /// <param name="propName">The name of the property</param>
        /// <param name="params"></param>
        /// <returns>The value of the property</returns>
        object PluginPropertyGet(string propName, object[] @params);
        /// <summary>
        /// Called by the HomeSeer system to set the value of a property by name using reflection
        /// </summary>
        /// <param name="propName">The name of the property</param>
        /// <param name="value">The new value of the property</param>
        void PluginPropertySet(string propName, object value);
        
        #endregion
        
        //TODO PostBackProc -> More documentation on the return value and its uses
        /// <summary>
        /// Called by the HomeSeer system when a page owned by this plugin receives an HTTP POST request
        /// </summary>
        /// <param name="page">The page that received the POST request</param>
        /// <param name="data">The data included in the request</param>
        /// <param name="user">The user responsible for initiating the request</param>
        /// <param name="userRights">The user's rights</param>
        /// <returns></returns>
        string PostBackProc(string page, string data, string user, int userRights);

        //TODO ExecuteActionById -> What do we do with this now?
        //string ExecuteActionById(string actionId, Dictionary<string, string> @params);

    }

}