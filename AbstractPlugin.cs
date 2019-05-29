using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using HomeSeer.Jui.Views;

namespace HomeSeer.PluginSdk {

    public abstract class AbstractPlugin : IPlugin {

        #region Properties
        
        public bool IsShutdown { get; protected set; }

        protected List<Page> SettingsPages;
        protected Dictionary<string, int> SettingsPageIndexes;
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

        public IHsController HomeSeerSystem { get; set; }
        public IAppCallbackAPI ClientCallback { get; set; }

        protected const string SettingsSectionName = "Settings";
        protected abstract string SettingsFileName { get; }

        #endregion
        
        #region Constructor
        
        public AbstractPlugin() {
            SettingsPages = new List<Page>();
            SettingsPageIndexes = new Dictionary<string, int>();
        }
        
        #endregion
        
        #region Startup and Shutdown

        public bool RegisterWithController() {
            return HomeSeerSystem.RegisterPlugin(ID, Name);
        }

        protected abstract void Initialize();

        public virtual string InitIO(string port) {
            var result = "";
            try {
                LoadSettingsFromIni();
                //register settings pages
                HomeSeerSystem.RegisterJuiSettingsPages(SettingsPages.ToDictionary(p => p.Id, p => p.Name), ID);
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

        public abstract List<string> GetJuiSettingsPages();

        public abstract List<string> SaveJuiSettingsPages(List<string> pages);
        
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
        public abstract InterfaceStatus InterfaceStatus();
        
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

        //TODO all of these methods should be default implementations only
        public object PluginFunction(string procName, object[] parms) {
            //TODO PluginFunction
            return null;
        }

        public object PluginPropertyGet(string procName, object[] parms) {
            //TODO PluginPropertyGet
            return null;
        }

        public void PluginPropertySet(string procName, object value) {
            //TODO PluginPropertySet
            return;
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
            
            //TODO optimize this so that if no settings are saved, the process is skipped and default settings are written
            foreach (var settingsPage in SettingsPages) {
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