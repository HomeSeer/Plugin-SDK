using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Text;
using HomeSeer.Jui.Views;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.EndPoints.Tcp;
using HSCF.Communication.ScsServices.Client;

namespace HomeSeer.PluginSdk {

    public abstract class AbstractPlugin : IPlugin {

        #region Properties
        
        public bool IsShutdown { get; protected set; }

        /// <inheritdoc />
        public abstract bool HasSettings { get; }
        protected SettingsCollection Settings;
        
        //Chris says he rarely uses this to manage com ports with plugins
        public virtual bool HSCOMPort { get; } = false;
        /// <inheritdoc />
        public abstract string ID { get; }
        /// <inheritdoc />
        public abstract string Name { get; }
        /// <inheritdoc />
        public virtual int AccessLevel { get; } = 1;
        /// <inheritdoc />
        public virtual bool SupportsAddDevice { get; } = false;
        /// <inheritdoc />
        public virtual bool SupportsConfigDevice { get; } = false;
        /// <inheritdoc />
        public virtual bool SupportsConfigDeviceAll { get; } = false;
        
        
        public virtual bool ActionAdvancedMode { get; set; } = false;
        //TODO HasTriggers
        public virtual bool HasTriggers { get; } = false;
        //TODO TriggerCount
        public virtual int  TriggerCount { get; } = 0;

        public virtual int ActionCount { get; } = 0;
        public virtual string ActionName { get; }
        public virtual bool Condition { get; set; }
        public virtual bool HasConditions { get; } = false;
        public virtual int SubTriggerCount { get; } = 0;
        public virtual string SubTriggerName { get; }
        public virtual bool TriggerConfigured { get; }
        public virtual string TriggerName { get; }

        /// <summary>
        /// An instance of the HomeSeer system
        /// </summary>
        public IHsController HomeSeerSystem { get; private set; }

        protected const string SettingsSectionName = "Settings";
        protected abstract string SettingsFileName { get; }
        
        private const int HomeSeerPort = 10400;
        
        private static IScsServiceClient<IHsController> _client;
        
        private string _ipAddress = "127.0.0.1";

        #endregion
        
        #region Constructor
        
        protected AbstractPlugin() {
            Settings = new SettingsCollection();
        }
        
        #endregion
        
        #region Startup and Shutdown
        
        /// <summary>
        /// Attempt to establish a connection to the HomeSeer system
        /// </summary>
        /// <param name="args">Command line arguments included in the execution of the program</param>
        public void Connect(string[] args) {
            
            foreach (var arg in args) {
                var parts = arg.Split('=');
                switch (parts[0].ToLower()) {
                    case "server":
                        _ipAddress = parts[1];
                        break;
                    case "instance":
                        //TODO no more instances
                        break;
                }
            }
            
            _client         = ScsServiceClientBuilder.CreateClient<IHsController>(new ScsTcpEndPoint(_ipAddress, HomeSeerPort), this);
            
            Connect(1);
        }
        
        private void Connect(int attempts) {

            try {
                _client.Connect();
                var apiVersion = 0D;

                try {
                    HomeSeerSystem       = _client.ServiceProxy;
                    apiVersion = HomeSeerSystem.APIVersion;
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
			
            Thread.Sleep(4000); //?

            try {
                HomeSeerSystem.RegisterPlugin(ID, Name);
                do {
                    Thread.Sleep(10);
                } while (_client.CommunicationState == CommunicationStates.Connected && !IsShutdown);
				
                _client.Disconnect();
				
                Thread.Sleep(2000); //?
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
                IsShutdown = true;
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
            
            IsShutdown = true;
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
        /// Called right before the data held in Settings is serialized and sent to HomeSeer.
        /// <para>
        /// Use this if you need to process anything when the plugin settings are loaded.
        /// </para>
        /// </summary>
        protected virtual void OnSettingsLoad() {}

        /// <inheritdoc cref="IPlugin.SaveJuiSettingsPages"/>
        public bool SaveJuiSettingsPages(string jsonString) {

            try {
                var deserializedPages = Page.Factory.ListFromJson(jsonString);
                return OnSettingsChange(deserializedPages);
            }
            catch (KeyNotFoundException exception) {
                Console.WriteLine(exception);
                throw new KeyNotFoundException("Cannot save settings; no settings pages exist with that ID.", exception);
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
                    HomeSeerSystem.SaveINISetting(SettingsSectionName, settingPair.Key, settingPair.Value, SettingsFileName);     
                }
            }
        }
        
        #endregion
        
        #region Configuration and Status Info
        
        /// <inheritdoc cref="IPlugin.OnStatusCheck"/>
        public abstract PluginStatus OnStatusCheck();

        

        //TRUE to indicate that the plugin should receive device change events
        //FALSE to indicate that the plugin should not receive any device change events
        public virtual bool RaisesGenericCallbacks { get; } = false;
        
        public virtual bool SupportsConfigDeviceJui() => true;
        
        
        
        #endregion
        
        #region Devices

        public virtual string ConfigDevice(int @ref, string user, int userRights, bool newDevice) {
            //TODO ConfigDevice
            return "Intentionally blank";
        }

        public virtual Constants.ConfigDevicePostReturn ConfigDevicePost(int @ref, string data, string user, int userRights) {
            //TODO ConfigDevicePost
            return Constants.ConfigDevicePostReturn.DoneAndCancelAndStay;
        }
        
        //Chris says he rarely uses this.  What is it used for?
        public virtual PollResultInfo PollDevice(int dvref) {
            //TODO PollDevice
            var pollResult = new PollResultInfo {Result = Constants.enumPollResult.OK};
            return pollResult;
        }

        public virtual void SetIOMulti(List<CAPI.CAPIControl> colSend) {

            foreach (var control in colSend) {
                //TODO process the device update
            }
        }
        
        public virtual string GetJuiDeviceConfigPage(string deviceRef) {
            throw new System.NotImplementedException();
        }
        
        public virtual string SaveJuiDeviceConfigPage(string pageContent, int deviceRef) {
            throw new System.NotImplementedException();
        }
        
        public virtual bool ActionReferencesDevice(TrigActInfo ActInfo, int dvRef) {
            //TODO ActionReferencesDevice
            return false;
        }
        
        public virtual string ActionBuildUI(string sUnique, TrigActInfo ActInfo) {
            //TODO ActionBuildUI
            return "";
        }

        public virtual bool ActionConfigured(TrigActInfo ActInfo) {
            //TODO ActionConfigured
            return false;
        }

        public virtual string ActionFormatUI(TrigActInfo ActInfo) {
            //TODO ActionFormatUI
            return "";
        }

        public virtual MultiReturn ActionProcessPostUI(NameValueCollection PostData, TrigActInfo TrigInfoIN) {
            //TODO ActionProcessPostUI
            var result = new MultiReturn
                         {
                             sResult = "", DataOut = TrigInfoIN.DataIn, TrigActInfo = TrigInfoIN
                         };
            return result;
        }

        public virtual bool HandleAction(TrigActInfo ActInfo) {
            //TODO HandleAction
            return false;
        }

        public virtual string TriggerBuildUI(string sUnique, TrigActInfo TrigInfo) {
            //TODO TriggerBuildUI
            return "";
        }

        public virtual string TriggerFormatUI(TrigActInfo TrigInfo) {
            //TODO TriggerFormatUI
            return "";
        }

        public virtual MultiReturn TriggerProcessPostUI(NameValueCollection PostData, TrigActInfo TrigInfoIN) {
            //TODO TriggerProcessPostUI
            var result = new MultiReturn
                         {
                             sResult = "", DataOut = TrigInfoIN.DataIn, TrigActInfo = TrigInfoIN
                         };
            return result;
        }

        public virtual bool TriggerReferencesDevice(TrigActInfo TrigInfo, int dvRef) {
            //TODO TriggerReferencesDevice
            return false;
        }

        public virtual bool TriggerTrue(TrigActInfo TrigInfo) {
            //TODO TriggerTrue
            return true;
        }
        
        #endregion
        
        #region Features

        public virtual string GetPagePlugin(string page, string user, int userRights, string queryString) {
            //TODO GetPagePlugin
            return "";
        }
        
        public virtual string PostBackProc(string page, string data, string user, int userRights) {
            //TODO PostBackProc
            return "";
        }
        
        public virtual string ExecuteActionById(string actionId, Dictionary<string, string> @params) {
            //TODO ExecuteActionById
            return "";
        }

        public virtual string GetJuiPagePlugin(string pageId) {
            //TODO GetJuiPagePlugin
            return "";
        }
        
        public virtual string SaveJuiPage(string pageContent) {
            //TODO SaveJuiPage
            return "";
        }
        
        #endregion
        
        #region Events

        public virtual void HSEvent(Constants.HSEvent EventType, object[] @params) {
            //TODO process events
        }

        public string GetActionNameByNumber(int actionNum) {
            throw new NotImplementedException();
        }

        public bool TriggerHasConditions(int triggerNum) {
            throw new NotImplementedException();
        }

        public int GetSubTriggerCount(int triggerNum) {
            throw new NotImplementedException();
        }

        public string GetSubTriggerNameByNumber(int triggerNum, int subTriggerNum) {
            throw new NotImplementedException();
        }

        public bool IsTriggerConfigValid(TrigActInfo trigInfo) {
            throw new NotImplementedException();
        }

        public string GetTriggerNameByNumber(int triggerNum) {
            throw new NotImplementedException();
        }

        #endregion

        #region Dynamic Method/Property Calls

        /// <inheritdoc cref="IPlugin.PluginFunction"/>
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

        /// <inheritdoc cref="IPlugin.PluginPropertyGet"/>
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

        /// <inheritdoc cref="IPlugin.PluginPropertySet"/>
        public void PluginPropertySet(string propName, object value) {
            try {
                var ty = GetType();
                var mi = ty.GetProperty(propName);
                if (mi == null) {
                    //TODO log
                    var message = new StringBuilder("Property ").Append(propName).Append(" does not exist in this plugin.");
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

        //This was never fully implemented
        public Constants.SearchReturn[] Search(string SearchString, bool RegEx) {
            //TODO Search
            var searchResults = new List<Constants.SearchReturn>();
            return searchResults.ToArray();
        }

        //Deprecated? Rich might have a new/better approach for this
        public void SpeakIn(int device, string txt, bool w, string host) {
            //TODO SpeakIn
        }        

    }

}