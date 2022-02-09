using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk;
using HomeSeer.PluginSdk.Devices;
using HomeSeer.PluginSdk.Logging;
using HSPI_HomeSeerSamplePlugin.Features;
using Newtonsoft.Json;

namespace HSPI_HomeSeerSamplePlugin {

    /// <inheritdoc cref="AbstractPlugin"/>
    /// <summary>
    /// The plugin class for HomeSeer Sample Plugin that implements the <see cref="AbstractPlugin"/> base class.
    /// </summary>
    /// <remarks>
    /// This class is accessed by HomeSeer and requires that its name be "HSPI" and be located in a namespace
    ///  that corresponds to the name of the executable. For this plugin, "HomeSeerSamplePlugin" the executable
    ///  file is "HSPI_HomeSeerSamplePlugin.exe" and this class is HSPI_HomeSeerSamplePlugin.HSPI
    /// <para>
    /// If HomeSeer is unable to find this class, the plugin will not start.
    /// </para>
    /// </remarks>
    // ReSharper disable once InconsistentNaming
    public class HSPI : AbstractPlugin, WriteLogSampleActionType.IWriteLogActionListener {

        // Local vars

        // speaker client instance
        private SpeakerClient _speakerClient;

        #region Properties

        /// <inheritdoc />
        /// <remarks>
        /// This ID is used to identify the plugin and should be unique across all plugins
        /// <para>
        /// This must match the MSBuild property $(PluginId) as this will be used to copy
        ///  all of the HTML feature pages located in .\html\ to a relative directory
        ///  within the HomeSeer html folder.
        /// </para>
        /// <para>
        /// The relative address for all of the HTML pages will end up looking like this:
        ///  ..\Homeseer\Homeseer\html\HomeSeerSamplePlugin\
        /// </para>
        /// </remarks>
        public override string Id { get; } = "HomeSeerSamplePlugin";
        
        /// <inheritdoc />
        /// <remarks>
        /// This is the readable name for the plugin that is displayed throughout HomeSeer
        /// </remarks>
        public override string Name { get; } = "Sample Plugin";
        
        /// <inheritdoc />
        protected override string SettingsFileName { get; } = "HomeSeerSamplePlugin.ini";

        public override bool SupportsConfigDevice => true;

        #endregion

        public HSPI() {
            //Initialize the plugin 
            
            //Enable internal debug logging to console
            LogDebug = true;
            //Setup anything that needs to be configured before a connection to HomeSeer is established
            // like initializing the starting state of anything needed for the operation of the plugin
            
            //Such as initializing the settings pages presented to the user (currently saved state is loaded later)
            InitializeSettingsPages();
            
            //Or adding an event action or trigger type definition to the list of types supported by your plugin
            ActionTypes.AddActionType(typeof(WriteLogSampleActionType));
            TriggerTypes.AddTriggerType(typeof(SampleTriggerType));
        }

        /// <summary>
        /// Initialize the starting state of the settings pages for the HomeSeerSamplePlugin.
        ///  This constructs the framework that the user configurable settings for the plugin live in.
        ///  Any saved configuration options are loaded later in <see cref="Initialize"/> using
        ///  <see cref="AbstractPlugin.LoadSettingsFromIni"/>
        /// </summary>
        /// <remarks>
        /// For ease of use throughout the plugin, all of the view IDs, names, and values (non-volatile data)
        ///  are stored in the <see cref="HSPI_HomeSeerSamplePlugin.Constants.Settings"/> static class.
        /// </remarks>
        private void InitializeSettingsPages() {
            //Initialize the first settings page
            // This page is used to manipulate the behavior of the sample plugin
            
            //Start a PageFactory to construct the Page
            var settingsPage1 = PageFactory.CreateSettingsPage(Constants.Settings.SettingsPage1Id, 
                                                               Constants.Settings.SettingsPage1Name);
            //Add a LabelView to the page
            settingsPage1.WithLabel(Constants.Settings.Sp1ColorLabelId, null, 
                                    Constants.Settings.Sp1ColorLabelValue);

            //Create a group of ToggleViews displayed as a flexbox grid 
            var colorViewGroup = new GridView(Constants.Settings.Sp1ColorGroupId, Constants.Settings.Sp1ColorGroupName);
            var colorFirstRow = new  GridRow();
            colorFirstRow.AddItem(new ToggleView(Constants.Settings.Sp1ColorToggleRedId, Constants.Settings.ColorRedName, true) { ToggleType = EToggleType.Checkbox }, extraSmallSize: EColSize.Col6, largeSize: EColSize.Col3);
            colorFirstRow.AddItem(new ToggleView(Constants.Settings.Sp1ColorToggleOrangeId, Constants.Settings.ColorOrangeName, true) { ToggleType = EToggleType.Checkbox }, extraSmallSize: EColSize.Col6, largeSize: EColSize.Col3);
            colorFirstRow.AddItem(new ToggleView(Constants.Settings.Sp1ColorToggleYellowId, Constants.Settings.ColorYellowName, true) { ToggleType = EToggleType.Checkbox }, extraSmallSize: EColSize.Col6, largeSize: EColSize.Col3);
            colorFirstRow.AddItem(new ToggleView(Constants.Settings.Sp1ColorToggleGreenId, Constants.Settings.ColorGreenName, true) { ToggleType = EToggleType.Checkbox }, extraSmallSize: EColSize.Col6, largeSize: EColSize.Col3);
            var colorSecondRow = new GridRow();
            colorSecondRow.AddItem(new ToggleView(Constants.Settings.Sp1ColorToggleBlueId, Constants.Settings.ColorBlueName, true) { ToggleType = EToggleType.Checkbox }, extraSmallSize: EColSize.Col6, largeSize: EColSize.Col3);
            colorSecondRow.AddItem(new ToggleView(Constants.Settings.Sp1ColorToggleIndigoId, Constants.Settings.ColorIndigoName, true) { ToggleType = EToggleType.Checkbox }, extraSmallSize: EColSize.Col6, largeSize: EColSize.Col3);
            colorSecondRow.AddItem(new ToggleView(Constants.Settings.Sp1ColorToggleVioletId, Constants.Settings.ColorVioletName, true) { ToggleType = EToggleType.Checkbox }, extraSmallSize: EColSize.Col6, largeSize: EColSize.Col3);

            colorViewGroup.AddRow(colorFirstRow);
            colorViewGroup.AddRow(colorSecondRow);
            //Add the ViewGroup containing all of the ToggleViews to the page
            settingsPage1.WithView(colorViewGroup);

            //Create 2 ToggleViews for controlling the visibility of the other two settings pages
            var pageToggles = new List<ToggleView> {
                                  new ToggleView(Constants.Settings.Sp1PageVisToggle1Id, Constants.Settings.Sp1PageVisToggle1Name, true),
                                  new ToggleView(Constants.Settings.Sp1PageVisToggle2Id, Constants.Settings.Sp1PageVisToggle2Name, true),
                              };
            //Add a ViewGroup containing all of the ToggleViews to the page
            settingsPage1.WithGroup(Constants.Settings.Sp1PageToggleGroupId,
                                    Constants.Settings.Sp1PageToggleGroupName,
                                    pageToggles);
            //Add the first page to the list of plugin settings pages
            Settings.Add(settingsPage1.Page);
            
            //Initialize the second settings page
            // This page is used to visually demonstrate all of the available JUI views except for InputViews.
            // None of these views interact with the plugin and are merely for show.
            
            //Start a PageFactory to construct the Page
            var settingsPage2 = PageFactory.CreateSettingsPage(Constants.Settings.SettingsPage2Id, 
                                                               Constants.Settings.SettingsPage2Name);
            //Add a LabelView with a title to the page
            settingsPage2.WithLabel(Constants.Settings.Sp2LabelWTitleId, 
                                    Constants.Settings.Sp2LabelWTitleName, 
                                    Constants.Settings.Sp2LabelWTitleValue);
            //Add a LabelView without a title to the page
            settingsPage2.WithLabel(Constants.Settings.Sp2LabelWoTitleId,
                                    null,
                                    Constants.Settings.Sp2LabelWoTitleValue);
            //Add a toggle switch to the page
            settingsPage2.WithToggle(Constants.Settings.Sp2SampleToggleId, Constants.Settings.Sp2SampleToggleName);
            //Add a checkbox to the page
            settingsPage2.WithCheckBox(Constants.Settings.Sp2SampleCheckBoxId, Constants.Settings.Sp2SampleCheckBoxName);
            //Add a drop down select list to the page
            settingsPage2.WithDropDownSelectList(Constants.Settings.Sp2SelectListId,
                                         Constants.Settings.Sp2SelectListName,
                                         Constants.Settings.Sp2SelectListOptions);
            //Add a radio select list to the page
            settingsPage2.WithRadioSelectList(Constants.Settings.Sp2RadioSlId,
                                         Constants.Settings.Sp2RadioSlName,
                                         Constants.Settings.Sp2SelectListOptions);

            //Add a text area to the page
            settingsPage2.WithTextArea(Constants.Settings.Sp2TextAreaId,
                                         Constants.Settings.Sp2TextAreaName,
                                         3);
                                         
            //Add a time span to the page
            settingsPage2.WithTimeSpan(Constants.Settings.Sp2SampleTimeSpanId, Constants.Settings.Sp2SampleTimeSpanName);
            
            //Add the second page to the list of plugin settings pages
            Settings.Add(settingsPage2.Page);
            
            //Initialize the third settings page
            // This page is used to visually demonstrate the different types of JUI InputViews.
            
            //Start a PageFactory to construct the Page
            var settingsPage3 = PageFactory.CreateSettingsPage(Constants.Settings.SettingsPage3Id, Constants.Settings.SettingsPage3Name);
            //Add a text InputView to the page
            settingsPage3.WithInput(Constants.Settings.Sp3SampleInput1Id,
                                    Constants.Settings.Sp3SampleInput1Name);
            //Add a number InputView to the page
            settingsPage3.WithInput(Constants.Settings.Sp3SampleInput2Id,
                                    Constants.Settings.Sp3SampleInput2Name,
                                    EInputType.Number);
            //Add an email InputView to the page
            settingsPage3.WithInput(Constants.Settings.Sp3SampleInput3Id,
                                    Constants.Settings.Sp3SampleInput3Name,
                                    EInputType.Email);
            //Add a URL InputView to the page
            settingsPage3.WithInput(Constants.Settings.Sp3SampleInput4Id,
                                    Constants.Settings.Sp3SampleInput4Name,
                                    EInputType.Url);
            //Add a password InputView to the page
            settingsPage3.WithInput(Constants.Settings.Sp3SampleInput5Id,
                                    Constants.Settings.Sp3SampleInput5Name,
                                    EInputType.Password);
            //Add a decimal InputView to the page
            settingsPage3.WithInput(Constants.Settings.Sp3SampleInput6Id,
                                    Constants.Settings.Sp3SampleInput6Name,
                                    EInputType.Decimal);
            //Add the third page to the list of plugin settings pages
            Settings.Add(settingsPage3.Page);
        }

        protected override void Initialize() {
            //Load the state of Settings saved to INI if there are any.
            LoadSettingsFromIni();
            if (LogDebug) {
                Console.WriteLine("Registering feature pages");
            }
            //Initialize feature pages            
            HomeSeerSystem.RegisterFeaturePage(Id, "sample-guided-process.html", "Sample Guided Process");
            HomeSeerSystem.RegisterFeaturePage(Id, "sample-blank.html", "Sample Blank Page");
            HomeSeerSystem.RegisterFeaturePage(Id, "sample-trigger-feature.html", "Trigger Feature Page");
            HomeSeerSystem.RegisterDeviceIncPage(Id, "add-sample-device.html", "Add Sample Device");

            // If a speaker client is needed that handles sending speech to an audio device, initialize that here.
            // If you are supporting multiple speak devices such as multiple speakers, you would make this call
            // in your reoutine that initializes each speaker device. Create a new instance of the speaker client
            // for each speaker. We simply initalize one here as a sample implementation
            _speakerClient = new SpeakerClient(Name);       
            // if the HS system has the setting "No password required for local subnet" enabled, the user/pass passed to Connect are ignored
            // if the connection is from the local subnet, else the user/pass passed here are must exist as a user in the system
            // You will need to allow the user to supply a user/pass in your plugin settings
            // This functions connects your speaker client to the system. Your client will then appear as a speaker client in the system
            // and can be selected as a target for speech and audio in event actions.
            // When the system speaks to your client, your SpeakText function is called in SpeakerClient class
            _speakerClient.Connect("default", "default");

            Console.WriteLine("Initialized");
            Status = PluginStatus.Ok();
        }

        protected override void OnShutdown()
        {
            Console.WriteLine("Shutting down");
            _speakerClient.Disconnect();
        }

        protected override bool OnSettingChange(string pageId, AbstractView currentView, AbstractView changedView) {

            //React to the toggles that control the visibility of the last 2 settings pages
            if (changedView.Id == Constants.Settings.Sp1PageVisToggle1Id) {
                //Make sure the changed view is a ToggleView
                if (!(changedView is ToggleView tView)) {
                    return false;
                }
                
                //Show/Hide the second page based on the new state of the toggle
                if (tView.IsEnabled) {
                    Settings.ShowPageById(Constants.Settings.SettingsPage2Id);
                }
                else {
                    Settings.HidePageById(Constants.Settings.SettingsPage2Id);
                }
            }
            else if (changedView.Id == Constants.Settings.Sp1PageVisToggle2Id) {
                //Make sure the changed view is a ToggleView
                if (!(changedView is ToggleView tView)) {
                    return false;
                }
                
                //Show/Hide the second page based on the new state of the toggle
                if (tView.IsEnabled) {
                    Settings.ShowPageById(Constants.Settings.SettingsPage3Id);
                }
                else {
                    Settings.HidePageById(Constants.Settings.SettingsPage3Id);
                }
            }
            else {
                if (LogDebug) {
                    Console.WriteLine($"View ID {changedView.Id} does not match any views on the page.");
                }
            }

            return true;
        }

        /// <inheritdoc />
        /// <remarks>
        /// This plugin does not have a shifting operational state; so this method is not used.
        /// </remarks>
        protected override void BeforeReturnStatus() {}

        /// <inheritdoc />
        public override string GetJuiDeviceConfigPage(int deviceRef)
        {
            // Read values saved in PlugExtraData
            bool toggleValue = GetExtraData(deviceRef, Constants.Devices.DeviceConfigSampleToggleId) == true.ToString();
            bool checkboxValue = GetExtraData(deviceRef, Constants.Devices.DeviceConfigSampleCheckBoxId) == true.ToString();
            string dropdownSavedValue = GetExtraData(deviceRef, Constants.Devices.DeviceConfigSelectListId);
            int dropdownValue = -1;
            if(!string.IsNullOrEmpty(dropdownSavedValue))
            {
                dropdownValue = Convert.ToInt32(dropdownSavedValue);
            }
            string radioSelectSavedValue = GetExtraData(deviceRef, Constants.Devices.DeviceConfigRadioSlId);
            int radioSelectValue = -1;
            if (!string.IsNullOrEmpty(radioSelectSavedValue))
            {
                radioSelectValue = Convert.ToInt32(radioSelectSavedValue);
            }
            string inputSavedValue = GetExtraData(deviceRef, Constants.Devices.DeviceConfigInputId);
            string inputValue = Constants.Devices.DeviceConfigInputValue;
            if (!string.IsNullOrEmpty(inputSavedValue))
            {
                inputValue = inputSavedValue;
            }
            string textAreaSavedValue = GetExtraData(deviceRef, Constants.Devices.DeviceConfigTextAreaId);
            string textAreaValue = "";
            if (!string.IsNullOrEmpty(textAreaSavedValue))
            {
                textAreaValue = textAreaSavedValue;
            }
            string timeSpanSavedValue = GetExtraData(deviceRef, Constants.Devices.DeviceConfigTimeSpanId);
            TimeSpan timeSpanValue = TimeSpan.Zero;
            if(!string.IsNullOrEmpty(timeSpanSavedValue))
            {
                TimeSpan.TryParse(timeSpanSavedValue, out timeSpanValue);
            }

            //Start a PageFactory to construct the Page
            var deviceConfigPage = PageFactory.CreateDeviceConfigPage(Constants.Devices.DeviceConfigPageId,
                                                                       Constants.Devices.DeviceConfigPageName);
            //Add a LabelView with a title to the page
            deviceConfigPage.WithLabel(Constants.Devices.DeviceConfigLabelWTitleId,
                                    Constants.Devices.DeviceConfigLabelWTitleName,
                                    Constants.Devices.DeviceConfigLabelWTitleValue);
            //Add a LabelView without a title to the page
            deviceConfigPage.WithLabel(Constants.Devices.DeviceConfigLabelWoTitleId,
                                    null,
                                    Constants.Devices.DeviceConfigLabelWoTitleValue);
            //Add a toggle switch to the page
            deviceConfigPage.WithToggle(Constants.Devices.DeviceConfigSampleToggleId, Constants.Devices.DeviceConfigSampleToggleName, toggleValue);
            //Add a checkbox to the page
            deviceConfigPage.WithCheckBox(Constants.Devices.DeviceConfigSampleCheckBoxId, Constants.Devices.DeviceConfigSampleCheckBoxName, checkboxValue);
            //Add a drop down select list to the page
            deviceConfigPage.WithDropDownSelectList(Constants.Devices.DeviceConfigSelectListId,
                                         Constants.Devices.DeviceConfigSelectListName,
                                         Constants.Devices.DeviceConfigSelectListOptions,
                                         dropdownValue);
            //Add a radio select list to the page
            deviceConfigPage.WithRadioSelectList(Constants.Devices.DeviceConfigRadioSlId,
                                         Constants.Devices.DeviceConfigRadioSlName,
                                         Constants.Devices.DeviceConfigSelectListOptions,
                                         radioSelectValue);

            //Add a text InputView to the page
            deviceConfigPage.WithInput(Constants.Devices.DeviceConfigInputId,
                                       Constants.Devices.DeviceConfigInputName,
                                       inputValue);

            //Add a text area to the page
            deviceConfigPage.WithTextArea(Constants.Devices.DeviceConfigTextAreaId,
                                       Constants.Devices.DeviceConfigTextAreaName,
                                       textAreaValue);

            //Add a time span to the page
            deviceConfigPage.WithTimeSpan(Constants.Devices.DeviceConfigTimeSpanId,
                                       Constants.Devices.DeviceConfigTimeSpanName,
                                       timeSpanValue,
                                       true,
                                       false);
                                       
            return deviceConfigPage.Page.ToJsonString();
        }

        /// <inheritdoc />
        protected override bool OnDeviceConfigChange(Page deviceConfigPage, int deviceRef)
        {
            foreach (AbstractView view in deviceConfigPage.Views)
            {
                if(view.Id == Constants.Devices.DeviceConfigSampleToggleId)
                {
                    ToggleView v = view as ToggleView;
                    if (v  != null)
                    {
                        SetExtraData(deviceRef, Constants.Devices.DeviceConfigSampleToggleId, v.IsEnabled.ToString());
                    }
                }
                else if (view.Id == Constants.Devices.DeviceConfigSampleCheckBoxId)
                {
                    ToggleView v = view as ToggleView;
                    if (v != null)
                    {
                        SetExtraData(deviceRef, Constants.Devices.DeviceConfigSampleCheckBoxId, v.IsEnabled.ToString());
                    }
                }
                else if (view.Id == Constants.Devices.DeviceConfigSelectListId)
                {
                    SelectListView v = view as SelectListView;
                    if (v != null)
                    {
                        SetExtraData(deviceRef, Constants.Devices.DeviceConfigSelectListId, v.Selection.ToString());
                    }
                }
                else if (view.Id == Constants.Devices.DeviceConfigRadioSlId)
                {
                    SelectListView v = view as SelectListView;
                    if (v != null)
                    {
                        SetExtraData(deviceRef, Constants.Devices.DeviceConfigRadioSlId, v.Selection.ToString());
                    }
                }
                else if (view.Id == Constants.Devices.DeviceConfigInputId)
                {
                    InputView v = view as InputView;
                    if (v != null)
                    {
                        SetExtraData(deviceRef, Constants.Devices.DeviceConfigInputId, v.Value);
                    }
                }
                else if (view.Id == Constants.Devices.DeviceConfigTextAreaId)
                {
                    TextAreaView v = view as TextAreaView;
                    if (v != null)
                    {
                        SetExtraData(deviceRef, Constants.Devices.DeviceConfigTextAreaId, v.Value);
                    }
                }
                else if (view.Id == Constants.Devices.DeviceConfigTimeSpanId)
                {
                    TimeSpanView v = view as TimeSpanView;
                    if (v != null)
                    {
                        SetExtraData(deviceRef, Constants.Devices.DeviceConfigTimeSpanId, v.GetStringValue());
                    }
                }
            }
            return true;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Process any HTTP POST requests targeting pages registered to your plugin.
        /// <para>
        /// This is a very flexible process that does not have a predefined structure. The form <see cref="data"/> sends
        ///  from a page is entirely up to you and what works for you.  JSON and Base64 strings are encouraged because
        ///  of how readily available resources are to translate to/from these types. In Javascript, see JSON.stringify();
        ///  and window.btoa();
        /// </para>
        /// </remarks>
        public override string PostBackProc(string page, string data, string user, int userRights) {
            if (LogDebug) {
                Console.WriteLine("PostBack");
            }
            
            var response = "";
            
            switch(page) {
                case "sample-trigger-feature.html":

                    //Handle the Trigger Feature page
                    try {
                        var triggerOptions = JsonConvert.DeserializeObject<List<bool>>(data);
                        
                        //Get all triggers configured on the HomeSeer system that are of the SampleTriggerType
                        var configuredTriggers = HomeSeerSystem.GetTriggersByType(Id, SampleTriggerType.TriggerNumber);
                        if (configuredTriggers.Length == 0) {
                            return "No triggers configured to fire.";
                        }

                        //Handle each trigger that matches
                        foreach (var configuredTrigger in configuredTriggers) {
                            var trig = new SampleTriggerType(configuredTrigger, this, LogDebug);
                            if (trig.ShouldTriggerFire(triggerOptions.ToArray())) {
                                HomeSeerSystem.TriggerFire(Id, configuredTrigger);
                            }
                        }
                    }
                    catch (JsonSerializationException exception) {
                        if (LogDebug) {
                            Console.WriteLine(exception);
                        }
                        response = $"Error while deserializing data: {exception.Message}";
                    }
                    
                    break;
                    
                case "sample-guided-process.html":
                    
                    //Handle the Guided Process page
                    try {
                        var postData = JsonConvert.DeserializeObject<SampleGuidedProcessData>(data);

                        if (LogDebug) {
                            Console.WriteLine("Post back from sample-guided-process page");
                        }
                        response = postData.GetResponse();

                    }
                    catch (JsonSerializationException exception) {
                        if (LogDebug) {
                            Console.WriteLine(exception.Message);
                        }
                        response = "error";
                    }

                    break;
                case "add-sample-device.html":

                    try {
                        var postData = JsonConvert.DeserializeObject<DeviceAddPostData>(data);
                        if (LogDebug) {
                            Console.WriteLine("Post back from add-sample-device page");
                        }

                        if (postData.Action == "verify") {
                            response = JsonConvert.SerializeObject(postData.Device);
                        }
                        else {
                            var deviceData = postData.Device;
                            var device = deviceData.BuildDevice(Id);
                            var devRef = HomeSeerSystem.CreateDevice(device);
                            deviceData.Ref = devRef;
                            response = JsonConvert.SerializeObject(deviceData);
                        }
                    }
                    catch (Exception exception) {
                        if (LogDebug) {
                            Console.WriteLine(exception.Message);
                        }
                        response = "error";
                    }
                    break;
                default:
                    response = "error";
                    break;
            }
            return response;
        }
        
        /// <summary>
        /// Called by the sample guided process feature page through a liquid tag to provide the list of available colors
        /// <para>
        /// {{plugin_function 'HomeSeerSamplePlugin' 'GetSampleSelectList' []}}
        /// </para>
        /// </summary>
        /// <returns>The HTML for the list of select list options</returns>
        public string GetSampleSelectList() {
            if (LogDebug) {
                Console.WriteLine("Getting sample select list for sample-guided-process page");
            }
            var sb = new StringBuilder("<select class=\"mdb-select md-form\" id=\"step3SampleSelectList\">");
            sb.Append(Environment.NewLine);
            sb.Append("<option value=\"\" disabled selected>Color</option>");
            sb.Append(Environment.NewLine);
            
            var colorList = new List<string>();

            try {
                var colorSettings = Settings[Constants.Settings.SettingsPage1Id].GetViewById(Constants.Settings.Sp1ColorGroupId);
                if (!(colorSettings is ViewGroup colorViewGroup)) {
                    throw new ViewTypeMismatchException("No View Group found containing colors");
                }

                foreach (var view in colorViewGroup.Views) {
                    if (!(view is ToggleView colorView)) {
                        continue;
                    }
                    
                    colorList.Add(colorView.IsEnabled ? colorView.Name : "");
                }
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                colorList = Constants.Settings.ColorMap.Values.ToList();
            }
           
            for (var i = 0; i < colorList.Count; i++) {
                var color = colorList[i];
                if (string.IsNullOrEmpty(color)) {
                    continue;
                }
                
                sb.Append("<option value=\"");
                sb.Append(i);
                sb.Append("\">");
                sb.Append(color);
                sb.Append("</option>");
                sb.Append(Environment.NewLine);
            }

            sb.Append("</select>");
            return sb.ToString();
        }
        
        /// <summary>
        /// Called by the sample trigger feature page to get the HTML for a list of checkboxes to use a trigger options
        /// <para>
        /// {{list=plugin_function 'HomeSeerSamplePlugin' 'GetTriggerOptionsHtml' [2]}}
        /// </para>
        /// </summary>
        /// <param name="numTriggerOptions">The number of checkboxes to generate</param>
        /// <returns>
        /// A List of HTML strings representing checkbox input elements
        /// </returns>
        public List<string> GetTriggerOptionsHtml(int numTriggerOptions) {
            var triggerOptions = new List<string>();
            for (var i = 1; i <= numTriggerOptions; i++) {
                var cbTrigOpt = new ToggleView($"liquid-checkbox-triggeroption{i}", $"Trigger Option {i}")
                                {
                                    ToggleType = EToggleType.Checkbox
                                };
                triggerOptions.Add(cbTrigOpt.ToHtml());
            }

            return triggerOptions;
        }
        
        /// <summary>
        /// Called by the sample trigger feature page to get trigger option items as a list to populate HTML on the page.
        /// <para>
        /// {{list2=plugin_function 'HomeSeerSamplePlugin' 'GetTriggerOptions' [2]}}
        /// </para>
        /// </summary>
        /// <param name="numTriggerOptions">The number of trigger options to generate.</param>
        /// <returns>
        /// A List of <see cref="TriggerOptionItem"/>s used for checkbox input HTML element IDs and Names
        /// </returns>
        public List<TriggerOptionItem> GetTriggerOption(int numTriggerOptions) {
            var triggerOptions = new List<TriggerOptionItem>();
            for (var i = 1; i <= numTriggerOptions; i++) {
                triggerOptions.Add(new TriggerOptionItem(i, $"Trigger Option {i}"));
            }

            return triggerOptions;
        }

        /// <inheritdoc />
        public void WriteLog(ELogType logType, string message) {
            
            HomeSeerSystem.WriteLog(logType, message, Name);
        }

        private string GetExtraData(int deviceRef, string key)
        {
            PlugExtraData extraData = (PlugExtraData)HomeSeerSystem.GetPropertyByRef(deviceRef, EProperty.PlugExtraData);
            if (extraData != null && extraData.ContainsNamed(key))
            {
                return extraData[key];
            }
            return "";
        }

        private void SetExtraData(int deviceRef, string key, string value)
        {
            PlugExtraData extraData = (PlugExtraData)HomeSeerSystem.GetPropertyByRef(deviceRef, EProperty.PlugExtraData);
            if (extraData == null)
            {
                extraData = new PlugExtraData();
            }
            extraData[key] = value;
            HomeSeerSystem.UpdatePropertyByRef(deviceRef, EProperty.PlugExtraData, extraData);
        }

    }

}