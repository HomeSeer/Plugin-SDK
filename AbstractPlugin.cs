using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Reflection;
using HomeSeer.Jui.Views;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.EndPoints.Tcp;
using HSCF.Communication.ScsServices.Client;

namespace HomeSeer.PluginSdk {

    public abstract class AbstractPlugin : IPlugin {

        #region Properties
        
        public bool IsShutdown { get; protected set; }

        public abstract bool HasSettings { get; }
        protected SettingsCollection Settings;
        //TODO feature pages
        
        //Chris says he rarely uses this to manage com ports with plugins
        public virtual bool            HSCOMPort          { get; } = false;
        public abstract string ID                 { get; }
        public abstract string Name               { get; }
        public virtual bool            ActionAdvancedMode { get; set; } = false;
        //TODO HasTriggers
        public virtual bool            HasTriggers        { get; } = false;
        //TODO TriggerCount
        public virtual int             TriggerCount       { get; } = 0;

        public string ActionName { get; }
        public bool Condition { get; set; }
        public bool HasConditions { get; }
        public int SubTriggerCount { get; }
        public string SubTriggerName { get; }
        public bool TriggerConfigured { get; }
        public string TriggerName { get; }

        public IHsController HomeSeerSystem { get; private set; }
        public IAppCallbackAPI AppCallback { get; private set; }

        protected const string SettingsSectionName = "Settings";
        protected abstract string SettingsFileName { get; }
        
        private const int HomeSeerPort = 10400;
        
        private static IScsServiceClient<IHsController>   Client;
        private static IScsServiceClient<IAppCallbackAPI> ClientCallback;

        private string _ipAddress = "127.0.0.1";

        #endregion
        
        #region Constructor
        
        protected AbstractPlugin() {
            Settings = new SettingsCollection();
        }
        
        #endregion
        
        #region Startup and Shutdown
        
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
            
            Client         = ScsServiceClientBuilder.CreateClient<IHsController>(new ScsTcpEndPoint(_ipAddress, HomeSeerPort), this);
            ClientCallback = ScsServiceClientBuilder.CreateClient<IAppCallbackAPI>(new ScsTcpEndPoint(_ipAddress, HomeSeerPort), this);
            
            Connect(1);
        }
        
        private void Connect(int attempts) {

            try {
                Client.Connect();
                ClientCallback.Connect();
                var apiVersion = 0D;

                try {
                    HomeSeerSystem       = Client.ServiceProxy;
                    apiVersion = HomeSeerSystem.APIVersion;
                    Console.WriteLine("Host API Version: " + apiVersion);
                }
                catch (Exception exception) {
                    Console.WriteLine(exception);
                }

                try {
                    AppCallback = ClientCallback.ServiceProxy;
                    apiVersion = AppCallback.APIVersion;
                }
                catch (Exception exception) {
                    Console.WriteLine(exception);
                    return;
                }


            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                Console.WriteLine("Cannot connect attempt " + attempts);
                if (exception.Message.ToLower().Contains("timeout occurred.") && attempts < 6) {
                    Connect(attempts + 1);
                    if (Client != null) {
                        Client.Dispose();
                        Client = null;
                    }

                    if (ClientCallback != null) {
                        ClientCallback.Dispose();
                        ClientCallback = null;
                    }
                    return;
                }
            }
			
            Thread.Sleep(4000); //?

            try {
                HomeSeerSystem.RegisterPlugin(ID, Name);
                do {
                    Thread.Sleep(10);
                } while (Client.CommunicationState == CommunicationStates.Connected && !IsShutdown);
				
                Client.Disconnect();
                ClientCallback.Disconnect();
				
                Thread.Sleep(2000); //?
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                throw;
            }
        }

        protected abstract void Initialize();

        public virtual string InitIO(string port) {
            var result = "";
            try {
                Console.WriteLine("InitIO");
                LoadSettingsFromIni();
                //register settings pages
                Console.WriteLine("Registering JUI Settings Pages");
                //HomeSeerSystem.RegisterJuiSettingsPages(SettingsPages.ToDictionary(p => p.Id, p => p.Name), ID);
                Console.WriteLine("Initializing");
                Initialize();
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                IsShutdown = true;
                result     = "Error on InitIO: " + exception.Message;
            }
            
            return result;
        }
        
        public virtual void ShutdownIO() {
            
            IsShutdown = true;
        }
        
        #endregion
        
        #region Settings

        public string GetJuiSettingsPages() {
            
            OnSettingsLoad();
            return Settings.ToJsonString();
        }

        protected virtual void OnSettingsLoad() {}

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

        protected abstract bool OnSettingsChange(List<Page> pages);
        
        #endregion
        
        #region Configuration and Status Info

        public virtual int AccessLevel() => 1;

        //This is always CA_IO
        public int Capabilities() => (int) Constants.eCapabilities.CA_IO;
        
        //This is always the same as the name now
        public string InstanceFriendlyName() => Name;
        
        //TRUE to indicate that the plugin should receive device change events
        //FALSE to indicate that the plugin should not receive any device change events
        public virtual bool RaisesGenericCallbacks() {
            //TODO assess this method
            return false;
        }

        //This indicates that the plugin supports the ability to add devices but was never fully implemented
        public virtual bool SupportsAddDevice() {
            //TODO device add screen
            return false;
        }

        //This indicates that the plugin supports the device config screen for devices mapped to their interface
        public virtual bool SupportsConfigDevice() {
            //TODO device config screen
            return false;
        }

        //This indicates that the plugin supports the device config screen for all devices on the system
        public virtual bool SupportsConfigDeviceAll() => false;

        //Used to lazily implement multi-device support
        public bool SupportsMultipleInstances() => false;

        //DEPRECATED -> Plugins should no longer support multiple instances
        public bool SupportsMultipleInstancesSingleEXE() => false;

        public virtual int ActionCount() {
            //TODO ActionCount
            return 0;
        }
        
        public virtual bool SupportsConfigDeviceJui() => true;
        
        //TODO clean up the documentation here to better indicate how it should be used
        public abstract PluginStatus OnStatusCheck();
        
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

        public virtual void HSEvent(Constants.HSEvent EventType, object[] parms) {
            //TODO process events
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

        #region Deprecated

        public string PagePut(string data) {
            return null;
        }
        
        public string GenPage(string link) {
            return null;
        }

        #endregion

        #region Dynamic Method/Property Calls

        public object PluginFunction(string procName, object[] parms) {
            try
            {
                Type ty = this.GetType();
                MethodInfo mi = ty.GetMethod(procName);
                if (mi == null)
                {
                    //Log("Method " + proc + " does not exist in this plugin.", LogType.LOG_TYPE_ERROR);
                    return null;
                }
                return (mi.Invoke(this, parms));
            }
            catch (Exception ex)
            {
                //Log("Error in PluginProc: " + ex.Message, LogType.LOG_TYPE_ERROR);
            }

            return null;
        }

        public object PluginPropertyGet(string procName, object[] parms) {
            try
            {
                Type ty = this.GetType();
                PropertyInfo mi = ty.GetProperty(procName);
                if (mi == null)
                {
                    //Log("Method " + proc + " does not exist in this plugin.", LogType.LOG_TYPE_ERROR);
                    return null;
                }
                return mi.GetValue(this, null);
            }
            catch (Exception ex)
            {
                //Log("Error in PluginProc: " + ex.Message, LogType.LOG_TYPE_ERROR);
            }
            return null;
        }

        public void PluginPropertySet(string procName, object value) {
            try
            {
                Type ty = this.GetType();
                PropertyInfo mi = ty.GetProperty(procName);
                if (mi == null)
                {
                    //Log("Property " + proc + " does not exist in this plugin.", LogType.LOG_TYPE_ERROR);
                }
                else
                {
                    mi.SetValue(this, value, null);
                }                
            }
            catch (Exception ex)
            {
                //Log("Error in PluginPropertySet: " + ex.Message, LogType.LOG_TYPE_ERROR);
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

        protected void LoadSettingsFromIni() {
            Console.WriteLine("Loading settings from INI");
            //TODO optimize this so that if no settings are saved, the process is skipped and default settings are written
            foreach (var settingsPage in Settings) {
                var pageValueMap = settingsPage.ToValueMap();
                foreach (var settingPair in pageValueMap) {
                    var savedValue =
                        HomeSeerSystem.GetINISetting(SettingsSectionName,
                                                     settingPair.Key,
                                                     null,
                                                     SettingsFileName);
                    //Check if a value was previously saved
                    if (savedValue == null) {
                        //Save the default value when nothing has previously been saved
                        HomeSeerSystem.SaveINISetting(SettingsSectionName, settingPair.Key, settingPair.Value, SettingsFileName);
                        continue;
                    }

                    settingsPage.UpdateViewValueById(settingPair.Key, savedValue);       
                    
                }
            }
        }

    }

}