using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Text;
using HomeSeer.Jui.Views;
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
    /// <para>All plugins (the Hspi class) should extend this class.</para>
    /// </summary>
    public abstract class AbstractPlugin : IPlugin {

        #region Properties

        /// <inheritdoc />
        /// <remarks>
        /// The default implementation should be sufficient for all purposes;
        ///  returning TRUE if the <see cref="Settings"/> property contains pages
        ///  or FALSE if it doesn't.
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

        private const int HomeSeerPort = 10400;

        private static IScsServiceClient<IHsController> _client;

        private string _ipAddress = "127.0.0.1";
        private bool   _isShutdown;

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
                        _ipAddress = parts[1];
                        break;
                    default:
                        Console.WriteLine("Unknown command-line argument : " + parts[0].ToLower());
                        break;
                }
            }

            _client = ScsServiceClientBuilder.CreateClient<IHsController>(new ScsTcpEndPoint(_ipAddress, HomeSeerPort),
                                                                          this);

            Connect(1);
        }

        private void Connect(int attempts) {
            try {
                _client.Connect();

                try {
                    HomeSeerSystem = _client.ServiceProxy;
                    var apiVersion = HomeSeerSystem.APIVersion;
                    Console.WriteLine("Host API Version: " + apiVersion);
                }
                catch (Exception exception) {
                    Console.WriteLine(exception);
                }
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                Console.WriteLine("Cannot connect attempt " + attempts);
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
                HomeSeerSystem.RegisterPlugin(Id, Name);
                do {
                    Thread.Sleep(10);
                } while (_client.CommunicationState == CommunicationStates.Connected && !_isShutdown);

                _client.Disconnect();
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
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
                Console.WriteLine("InitIO");
                LoadSettingsFromIni();
                Console.WriteLine("Calling Initialize");
                Initialize();
                return true;
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                _isShutdown = true;
                throw new Exception("Error on InitIO: " + exception.Message, exception);
            }
        }

        /// <inheritdoc />
        public void ShutdownIO() {
            try {
                OnShutdown();
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
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
            try {
                var deserializedPages = Page.Factory.ListFromJson(jsonString);
                return OnSettingsChange(deserializedPages);
            }
            catch (KeyNotFoundException exception) {
                Console.WriteLine(exception);
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
        /// Loads the plugin settings saved to INI and saves default values if none exist.
        /// <para>
        /// This is automatically called during InitIO
        /// </para>
        /// </summary>
        protected void LoadSettingsFromIni() {
            Console.WriteLine("Loading settings from INI");
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
        public virtual void SetIOMulti(List<CAPI.CAPIControl> colSend) {
            //TODO SetIOMulti -> process the device update
            /*foreach (var control in colSend) {
                
            }*/
        }

        /// <inheritdoc />
        public virtual string GetJuiDeviceConfigPage(string deviceRef) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual string SaveJuiDeviceConfigPage(string pageContent, int deviceRef) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool ActionReferencesDevice(TrigActInfo actInfo, int dvRef) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual string ActionBuildUI(string sUnique, TrigActInfo actInfo) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool ActionConfigured(TrigActInfo actInfo) {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

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
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool TriggerTrue(TrigActInfo trigInfo) {
            throw new NotImplementedException();
        }

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

        /// <inheritdoc />
        public virtual string GetActionNameByNumber(int actionNum) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool TriggerHasConditions(int triggerNum) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual int GetSubTriggerCount(int triggerNum) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual string GetSubTriggerNameByNumber(int triggerNum, int subTriggerNum) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool IsTriggerConfigValid(TrigActInfo trigInfo) {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual string GetTriggerNameByNumber(int triggerNum) {
            throw new NotImplementedException();
        }

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
                Console.WriteLine(exception.Message);
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
                Console.WriteLine(exception.Message);
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
                    Console.WriteLine(message);
                }
                else {
                    mi.SetValue(this, value, null);
                }
            }
            catch (Exception exception) {
                Console.WriteLine(exception.Message);
            }
        }

        #endregion

    }

}