using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk.Devices;
using HomeSeer.PluginSdk.Events;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.EndPoints.Tcp;
using HSCF.Communication.ScsServices.Client;
// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace HomeSeer.PluginSdk {

    /// <inheritdoc cref="IPlugin"/>
    /// <summary>
    /// The base implementation of the <see cref="IPlugin"/> interface.
    /// <para>
    /// It includes default implementations for most of the IPlugin members and wraps others
    ///  with convenience methods and objects that make it simpler to interface with the HomeSeer system.
    ///
    /// Once the containing plugin application is started, initiate a connection
    ///  to the HomeSeer system by calling <see cref="Connect(string[])"/>.
    /// </para>
    /// <para>All plugins (the HSPI class) should derive from this class.</para>
    /// </summary>
    public abstract class AbstractPlugin : IPlugin, ActionTypeCollection.IActionTypeListener, TriggerTypeCollection.ITriggerTypeListener {

        #region Properties

        /// <inheritdoc />
        /// <remarks>
        /// The default implementation should be sufficient for all purposes;
        ///  returning TRUE if the <see cref="Settings"/> property contains pages
        ///  or FALSE if it doesn't.
        /// <para>
        /// Override this and always return TRUE if you plan on adding settings pages to the collection later
        /// </para>
        /// </remarks>
        public virtual bool HasSettings => (Settings?.Count ?? 0) > 0;
        
        /// <inheritdoc />
        public abstract string Id { get; }

        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public virtual int AccessLevel { get; } = 1;

        /// <inheritdoc />
        public virtual bool SupportsConfigDevice { get; } = false;

        /// <inheritdoc />
        public virtual bool SupportsConfigDeviceAll { get; } = false;

        /// <inheritdoc />
        public virtual bool HasTriggers { get; } = false;

        /// <inheritdoc />
        public virtual int TriggerCount { get; } = 0;

        /// <inheritdoc />
        public virtual int ActionCount { get; } = 0;

        /// <summary>
        /// Default section name for storing settings in an INI file
        /// </summary>
        protected const string SettingsSectionName = "Settings";
        
        /// <summary>
        /// An instance of the HomeSeer system
        /// </summary>
        protected IHsController HomeSeerSystem { get; private set; }

        /// <summary>
        /// The name of the settings INI file for the plugin.
        /// <para>
        /// It is recommended to use [PLUGIN-ID].ini where [PLUGIN-ID] is the ID of this plugin
        /// </para>
        /// </summary>
        protected abstract string SettingsFileName { get; }

        /// <summary>
        /// The collection of settings pages for the plugin.
        ///   See <see cref="SettingsCollection"/> for more information.
        /// </summary>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        protected SettingsCollection Settings { get; set; } = new SettingsCollection();
        
        /// <summary>
        /// The collection of action types that this plugin hosts for HomeSeer
        /// </summary>
        protected ActionTypeCollection ActionTypes { get; set; }
        
        /// <summary>
        /// The collection of trigger types that this plugin hosts for HomeSeer
        /// </summary>
        protected TriggerTypeCollection TriggerTypes { get; set; }
        
        /// <summary>
        /// The IP Address that the HomeSeer system is located at
        /// </summary>
        protected string IpAddress { get; set; } = "127.0.0.1";

        /// <summary>
        /// Used to enable/disable internal logging to the console
        /// <para>
        /// When it is TRUE, log messages from the PluginSdk code will be written to the Console
        /// </para>
        /// </summary>
        protected bool LogDebug {
            get => _logDebug;
            set {
                _logDebug = value;
                ActionTypes.LogDebug = value;
                TriggerTypes.LogDebug = value;
            }
        }

        private const int HomeSeerPort = 10400;

        private static IScsServiceClient<IHsController> _client;
        
        private bool   _isShutdown;
        private bool _logDebug;
        
        //Actions
        
        //Triggers

        #endregion

        /// <summary>
        /// Default constructor that initializes the Action and Trigger type collections
        /// </summary>
        protected AbstractPlugin() {
            ActionTypes = new ActionTypeCollection(this);
            TriggerTypes = new TriggerTypeCollection(this);
            LogDebug = _logDebug;
        }

        #region Startup and Shutdown

        /// <summary>
        /// Attempt to establish a connection to the HomeSeer system
        /// <para>
        /// This connection will be maintained as long as the program is running
        ///  or until <see cref="ShutdownIO"/> is called.
        /// </para>
        /// </summary>
        /// <param name="args">Command line arguments included in the execution of the program</param>
        // ReSharper disable once ParameterTypeCanBeEnumerable.Global
        public void Connect(string[] args) {
            foreach (var arg in args) {
                var parts = arg.Split('=');
                switch (parts[0].ToLower()) {
                    case "server":
                        IpAddress = parts[1];
                        break;
                    default:
                        Console.WriteLine("Unknown command-line argument : " + parts[0].ToLower());
                        break;
                }
            }

            _client = ScsServiceClientBuilder.CreateClient<IHsController>(new ScsTcpEndPoint(IpAddress, HomeSeerPort),
                                                                          this);

            Console.WriteLine("Connecting to HomeSeer...");
            Connect(1);
        }

        private void Connect(int attempts) {
            try {
                _client.Connect();

                try {
                    HomeSeerSystem = _client.ServiceProxy;
                    var apiVersion = HomeSeerSystem.APIVersion;
                    Console.WriteLine("Connected to HomeSeer");
                    if (LogDebug) {
                        Console.WriteLine($"Host API Version: {apiVersion}");
                    }
                }
                catch (Exception exception) {
                    if (LogDebug) {
                        Console.WriteLine(exception);
                    }
                }
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                Console.WriteLine($"Cannot connect attempt {attempts.ToString()}");
                if (exception.Message.ToLower().Contains("timeout occurred.") && attempts < 6) {
                    Connect(attempts + 1);
                    if (_client != null) {
                        _client.Dispose();
                        _client = null;
                    }

                    return;
                }
            }

            try {
                Console.WriteLine("Waiting for initialization...");
                HomeSeerSystem.RegisterPlugin(Id, Name);
                do {
                    Thread.Sleep(10);
                } while (_client.CommunicationState == CommunicationStates.Connected && !_isShutdown);

                _client.Disconnect();
                Console.WriteLine("Disconnected from HomeSeer");
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                throw;
            }
        }

        /// <inheritdoc cref="IPlugin.InitIO"/>
        protected abstract void Initialize();

        /// <inheritdoc cref="IPlugin.InitIO"/>
        /// <summary>
        /// Called by HomeSeer to initialize the plugin.
        /// <para>Perform all initialization logic in <see cref="Initialize"/></para>
        /// </summary>
        public bool InitIO() {
            try {
                if (LogDebug) {
                    Console.WriteLine("InitIO called by HomeSeer");
                }
                Initialize();
                return true;
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                _isShutdown = true;
                throw new Exception("Error on InitIO: " + exception.Message, exception);
            }
        }

        /// <inheritdoc />
        public void ShutdownIO() {
            try {
                Console.WriteLine("Disconnecting from HomeSeer...");
                OnShutdown();
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
            }

            _isShutdown = true;
        }

        /// <summary>
        /// Called right before the plugin shuts down.
        /// </summary>
        protected virtual void OnShutdown() { }

        #endregion

        #region Settings

        /// <inheritdoc cref="IPlugin.GetJuiSettingsPages"/>
        public string GetJuiSettingsPages() {
            if (LogDebug) {
                Console.WriteLine("GetJuiSettingsPages");
            }
            OnSettingsLoad();
            return Settings.ToJsonString();
        }

        /// <summary>
        /// Called right before the data held in <see cref="Settings"/> is serialized and sent to HomeSeer.
        /// <para>
        /// Use this if you need to process anything when the plugin settings are loaded.
        ///  Otherwise, it is typically unnecessary to override this.  The <see cref="SettingsCollection"/> class
        ///  automatically takes care of the JSON serialization/deserialization process.
        /// </para>
        /// </summary>
        protected virtual void OnSettingsLoad() { }

        /// <inheritdoc cref="IPlugin.SaveJuiSettingsPages"/>
        public bool SaveJuiSettingsPages(string jsonString) {
            if (LogDebug) {
                Console.WriteLine("SaveJuiSettingsPages");
            }
            try {
                var deserializedPages = SettingsCollection.FromJsonString(jsonString).Pages;
                return OnSettingsChange(deserializedPages);
            }
            catch (KeyNotFoundException exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                throw new KeyNotFoundException("Cannot save settings; no settings pages exist with that ID.",
                                               exception);
            }
        }

        /// <summary>
        /// Called when there are changes to the plugin settings that need to be processed and saved
        /// </summary>
        /// <param name="pages">A list of JUI pages containing only the new state of any changed views</param>
        /// <returns>
        /// TRUE if the save was successful; FALSE if it was not
        /// <para>
        /// You should throw an exception including a detailed message whenever possible over returning FALSE
        /// </para>
        /// </returns>
        protected abstract bool OnSettingsChange(List<Page> pages);

        /// <summary>
        /// Loads the plugin settings saved to INI to Settings and saves default values if none exist.
        /// </summary>
        protected void LoadSettingsFromIni() {
            if (LogDebug) {
                Console.WriteLine("Loading settings from INI");
            }
            //Get the whole section for settings
            var savedSettings = HomeSeerSystem.GetIniSection(SettingsSectionName, SettingsFileName);
            //Loop through each settings page
            foreach (var settingsPage in Settings) {
                //Build a map of settings on the page
                var pageValueMap = settingsPage.ToValueMap();
                //Loop through each setting
                foreach (var settingPair in pageValueMap) {
                    //If there is a saved value for the setting
                    // Always true after the first time the plugin starts
                    if (savedSettings.ContainsKey(settingPair.Key)) {
                        //Pull the saved value into memory
                        if (LogDebug) {
                            Console.WriteLine("Updating view");
                        }
                        settingsPage.UpdateViewValueById(settingPair.Key, savedSettings[settingPair.Key]);
                        //Go to the next setting
                        continue;
                    }

                    //Save the setting if there is no default value already saved
                    HomeSeerSystem.SaveINISetting(SettingsSectionName,
                                                  settingPair.Key,
                                                  settingPair.Value,
                                                  SettingsFileName);
                }
            }
        }

        #endregion

        #region Configuration and Status Info

        /// <inheritdoc />
        public abstract PluginStatus OnStatusCheck();

        #endregion

        #region Devices

        //Chris says he rarely uses this.  What is it used for?
        /// <inheritdoc />
        public virtual PollResultInfo PollDevice(int devRef) {
            var pollResult = new PollResultInfo {Result = Constants.enumPollResult.OK};
            return pollResult;
        }

        /// <inheritdoc />
        public virtual void SetIOMulti(List<DeviceControlEvent> colSend) {
            //Default behavior is to do nothing
        }

        /// <inheritdoc />
        public virtual string GetJuiDeviceConfigPage(string deviceRef) {
            return $"No device config page registered by plugin {Id}";
        }

        /// <inheritdoc />
        public bool SaveJuiDeviceConfigPage(string pageContent, int deviceRef) {
            if (LogDebug) {
                Console.WriteLine("SaveJuiDeviceConfigPage");
            }
            try {
                var deserializedPage = Page.FromJsonString(pageContent);
                return OnDeviceConfigChange(deserializedPage, deviceRef);
            }
            catch (KeyNotFoundException exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                throw new KeyNotFoundException("Cannot save device config; no pages exist with that ID.",
                                               exception);
            }
        }
        
        // ReSharper disable UnusedParameter.Global
        
        /// <summary>
        /// Called when there are changes to the device config page that need to be processed and saved
        /// </summary>
        /// <param name="deviceConfigPage">A JUI page containing only the new state of any changed views</param>
        /// <param name="deviceRef">The reference of the device the config page is for</param>
        /// <returns>
        /// TRUE if the save was successful; FALSE if it was not
        /// <para>
        /// You should throw an exception including a detailed message whenever possible over returning FALSE
        /// </para>
        /// </returns>
        protected virtual bool OnDeviceConfigChange(Page deviceConfigPage, int deviceRef) {
            
            return true;
        }
        
        // ReSharper restore UnusedParameter.Global

        #endregion

        #region Features

        /// <inheritdoc />
        public virtual string PostBackProc(string page, string data, string user, int userRights) {
            return "";
        }

        #endregion

        #region Events

        /// <inheritdoc />
        public virtual void HsEvent(Constants.HSEvent eventType, object[] @params) {
            //process events?
        }

        #region Actions

        /// <inheritdoc />
        public virtual AbstractActionType OnBuildActionUi(AbstractActionType action) {
            return action;
        }

        /// <inheritdoc />
        public virtual AbstractActionType OnActionConfigChange(AbstractActionType action) {
            return action;
        }

        /// <inheritdoc />
        public virtual AbstractActionType BeforeRunAction(AbstractActionType action) {
            return action;
        }

        /// <inheritdoc />
        public bool ActionReferencesDevice(TrigActInfo actInfo, int dvRef) {
            return ActionTypes?.ActionReferencesDeviceOrFeature(dvRef, actInfo) ?? false;
        }

        /// <inheritdoc />
        public string ActionBuildUI(TrigActInfo actInfo) {
            return ActionTypes?.OnGetActionUi(actInfo) ?? "Plugin returned malformed data";
        }

        /// <inheritdoc />
        public bool ActionConfigured(TrigActInfo actInfo) {
            return ActionTypes?.IsActionConfigured(actInfo) ?? true;
        }

        /// <inheritdoc />
        public string ActionFormatUI(TrigActInfo actInfo) {
            return ActionTypes?.OnGetActionPrettyString(actInfo) ?? "Plugin returned malformed data";
        }

        /// <inheritdoc />
        public MultiReturn ActionProcessPostUI(Dictionary<string, string> postData, TrigActInfo actInfo) {
            return ActionTypes?.OnUpdateActionConfig(postData, actInfo) ?? 
                   new MultiReturn {
                       DataOut = actInfo.DataIn, sResult = "Plugin returned malformed data", TrigActInfo = actInfo
                   };
        }

        /// <inheritdoc />
        public bool HandleAction(TrigActInfo actInfo) {
            return ActionTypes?.HandleAction(actInfo) ?? false;
        }
        
        /// <inheritdoc />
        public string GetActionNameByNumber(int actionNum) {
            return ActionTypes?.GetName(actionNum) ?? "Error retrieving action name";
        }

        #endregion

        #region Triggers

        /// <inheritdoc />
        public AbstractTriggerType OnBuildTriggerUi(AbstractTriggerType trigger) {
            return trigger;
        }

        /// <inheritdoc />
        public AbstractTriggerType OnTriggerConfigChange(AbstractTriggerType trigger) {
            return trigger;
        }

        /// <inheritdoc />
        public AbstractTriggerType BeforeCheckTrigger(AbstractTriggerType trigger) {
            return trigger;
        }

        /// <inheritdoc />
        public virtual string TriggerBuildUI(TrigActInfo trigInfo) {
            return TriggerTypes?.OnGetTriggerUi(trigInfo) ?? "Plugin returned malformed data";
        }

        /// <inheritdoc />
        public virtual string TriggerFormatUI(TrigActInfo trigInfo) {
            return TriggerTypes?.OnGetTriggerPrettyString(trigInfo) ?? "Plugin returned malformed data";
        }

        /// <inheritdoc />
        public virtual MultiReturn TriggerProcessPostUI(Dictionary<string, string> postData, TrigActInfo trigInfoIn) {
            return TriggerTypes?.OnUpdateTriggerConfig(postData, trigInfoIn) ?? 
                   new MultiReturn {
                                       DataOut = trigInfoIn.DataIn, sResult = "Plugin returned malformed data", 
                                       TrigActInfo = trigInfoIn
                                   };
        }

        /// <inheritdoc />
        public virtual bool TriggerReferencesDeviceOrFeature(TrigActInfo trigInfo, int devOrFeatRef) {
            return TriggerTypes?.TriggerReferencesDeviceOrFeature(devOrFeatRef, trigInfo) ?? false;
        }

        /// <inheritdoc />
        public virtual bool TriggerCanBeCondition(int triggerNum) {
            return TriggerTypes?.TriggerCanBeCondition(triggerNum) ?? false;
        }

        /// <inheritdoc />
        public virtual int GetSubTriggerCount(int triggerNum) {
            return TriggerTypes?.GetSubTriggerCount(triggerNum) ?? 0;
        }

        /// <inheritdoc />
        public virtual string GetSubTriggerNameByNumber(int triggerNum, int subTriggerNum) {
            return TriggerTypes?.GetSubTriggerName(triggerNum, subTriggerNum) ?? "Plugin returned malformed data";
        }

        /// <inheritdoc />
        public virtual bool IsTriggerConfigValid(TrigActInfo trigInfo) {
            return TriggerTypes?.IsTriggerConfigured(trigInfo) ?? true;
        }

        /// <inheritdoc />
        public virtual string GetTriggerNameByNumber(int triggerNum) {
            return TriggerTypes?.GetName(triggerNum) ?? "Plugin returned malformed data";
        }

        /// <inheritdoc />
        public virtual bool TriggerTrue(TrigActInfo trigInfo, bool isCondition = false) {
            return TriggerTypes?.IsTriggerTrue(trigInfo, isCondition) ?? false;
        }

        #endregion

        #endregion

        #region Dynamic Method/Property Calls

        /// <inheritdoc />
        public object PluginFunction(string procName, object[] @params) {
            try {
                var ty = GetType();
                var mi = ty.GetMethod(procName);
                return mi == null ? null : mi.Invoke(this, @params);
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception.Message);
                }
            }

            return null;
        }

        /// <inheritdoc />
        public object PluginPropertyGet(string propName, object[] @params) {
            try {
                var ty = GetType();
                var mi = ty.GetProperty(propName);
                return mi == null ? null : mi.GetValue(this, null);
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception.Message);
                }
            }

            return null;
        }

        /// <inheritdoc />
        public void PluginPropertySet(string propName, object value) {
            try {
                var ty = GetType();
                var mi = ty.GetProperty(propName);
                if (mi == null) {
                    var message = new StringBuilder("Property ")
                                  .Append(propName).Append(" does not exist in this plugin.");
                    if (LogDebug) {
                        Console.WriteLine(message);
                    }
                }
                else {
                    mi.SetValue(this, value, null);
                }
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        #endregion

    }

}