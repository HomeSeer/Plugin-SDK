using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Text;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk.Devices;
using HomeSeer.PluginSdk.Events;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.EndPoints.Tcp;
using HSCF.Communication.ScsServices.Client;

namespace HomeSeer.PluginSdk {

    /// <inheritdoc />
    /// <summary>
    /// The base implementation of the <see cref="IPlugin"/> interface.
    /// <para>
    /// It includes default implementations for most of the IPlugin members and wraps others
    ///  with convenience methods and objects that make it simpler to interface with the HomeSeer system.
    ///
    /// Once the containing plugin application is started, initiate a connection
    ///  to the HomeSeer system by calling <see cref="Connect(string[])"/>.
    /// </para>
    /// <para>All plugins (the HSPI class) should extend this class.</para>
    /// </summary>
    public abstract class AbstractPlugin : IPlugin {

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

        //Chris says he rarely uses this to manage com ports with plugins
        public virtual bool HSCOMPort { get; } = false;

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
        public virtual bool ActionAdvancedMode { get; set; } = false;

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
        /// The IP Address that the HomeSeer system is located at
        /// </summary>
        protected string IpAddress { get; set; } = "127.0.0.1";

        protected bool LogDebug { get; set; } = false;

        private const int HomeSeerPort = 10400;

        private static IScsServiceClient<IHsController> _client;
        
        private bool   _isShutdown;
        
        //Actions
        
        //Triggers

        #endregion

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
        /// <para>DO NOT OVERRIDE this method unless you are certain you need to modify the default behavior.</para>
        /// <para>Perform all initialization logic in <see cref="Initialize"/></para>
        /// </summary>
        public virtual bool InitIO(string port) {
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

        /// <inheritdoc />
        public virtual bool RaisesGenericCallbacks { get; } = false;

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
            //TODO Send to HsInterfaceConnection
            //Calculate the new device state according to controls
            //Translate to OnDeviceControl()
            
        }

        /// <inheritdoc />
        public virtual string GetJuiDeviceConfigPage(string deviceRef) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool SaveJuiDeviceConfigPage(string pageContent, int deviceRef) {
            if (LogDebug) {
                Console.WriteLine("SaveJuiDeviceConfigPage");
            }
            try {
                var deserializedPage = Page.Factory.FromJsonString(pageContent);
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
        protected abstract bool OnDeviceConfigChange(Page deviceConfigPage, int deviceRef);

        #endregion

        #region Features

        /// <inheritdoc />
        public string GetPagePlugin(string page, string user, int userRights, string queryString) {
            return "";
        }

        /// <inheritdoc />
        public virtual string PostBackProc(string page, string data, string user, int userRights) {
            return "";
        }

        #endregion

        #region Events

        /// <inheritdoc />
        public virtual void HsEvent(Constants.HSEvent eventType, object[] @params) {
            //TODO process events
        }

        #region Actions

        /// <inheritdoc />
        public virtual bool ActionReferencesDevice(TrigActInfo actInfo, int dvRef) {
            return false;
        }

        /// <inheritdoc />
        public virtual string ActionBuildUI(string sUnique, TrigActInfo actInfo) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool ActionConfigured(TrigActInfo actInfo) {
            return false;
        }

        /// <inheritdoc />
        public virtual string ActionFormatUI(TrigActInfo actInfo) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual MultiReturn ActionProcessPostUI(NameValueCollection postData, TrigActInfo trigInfoIn) {
            throw new NotImplementedException();
            /*var result = new MultiReturn
                         {
                             sResult = "", DataOut = trigInfoIn.DataIn, TrigActInfo = trigInfoIn
                         };
            return result;*/
        }

        /// <inheritdoc />
        public virtual bool HandleAction(TrigActInfo actInfo) {
            return false;
        }
        
        /// <inheritdoc />
        public virtual string GetActionNameByNumber(int actionNum) {
            return "Unknown";
        }

        #endregion

        #region Triggers

        /// <inheritdoc />
        public virtual string TriggerBuildUI(string sUnique, TrigActInfo trigInfo) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual string TriggerFormatUI(TrigActInfo trigInfo) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual MultiReturn TriggerProcessPostUI(NameValueCollection postData, TrigActInfo trigInfoIn) {
            throw new NotImplementedException();
            /*var result = new MultiReturn
                         {
                             sResult = "", DataOut = trigInfoIn.DataIn, TrigActInfo = trigInfoIn
                         };
            return result;*/
        }

        /// <inheritdoc />
        public virtual bool TriggerReferencesDevice(TrigActInfo trigInfo, int devRef) {
            return false;
        }

        /// <inheritdoc />
        public virtual bool TriggerHasConditions(int triggerNum) {
            return false;
        }

        /// <inheritdoc />
        public virtual int GetSubTriggerCount(int triggerNum) {
            return 0;
        }

        /// <inheritdoc />
        public virtual string GetSubTriggerNameByNumber(int triggerNum, int subTriggerNum) {
            return "Unknown";
        }

        /// <inheritdoc />
        public virtual bool IsTriggerConfigValid(TrigActInfo trigInfo) {
            return false;
        }

        /// <inheritdoc />
        public virtual string GetTriggerNameByNumber(int triggerNum) {
            return "Unknown";
        }

        /// <inheritdoc />
        public virtual bool TriggerTrue(TrigActInfo trigInfo, bool isCondition = false) {
            return false;
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