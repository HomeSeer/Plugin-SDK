using System;
using System.Collections.Generic;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk.Devices;

// ReSharper disable UnusedParameter.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// The base implementation of a plugin action type available for users to select in HomeSeer
    /// <para>
    /// Inherit from this class to define your own action types and store them in your plugin's <see cref="ActionTypeCollection"/>
    /// </para>
    /// </summary>
    public abstract class AbstractActionType {

        /// <summary>
        /// Used to enable/disable internal logging to the console
        /// <para>
        /// When it is TRUE, log messages from the PluginSdk code will be written to the Console
        /// </para>
        /// </summary>
        public bool LogDebug { get; set; }

        /// <summary>
        /// An interface reference to the plugin that owns this action type.
        /// <para>
        /// Define your own interface that inherits from <see cref="ActionTypeCollection.IActionTypeListener"/>
        ///  and then cast this as the type you defined to get a reference to your plugin that can handle any methods
        ///  you wish to define.
        /// </para>
        /// <para>
        /// This is usually used to have the plugin handle running the action.
        /// </para>
        /// </summary>
        public ActionTypeCollection.IActionTypeListener ActionListener { get; internal set; }
        
        /// <summary>
        /// The unique ID for the action.
        /// </summary>
        public int Id => _id;
        /// <summary>
        /// The reference ID of the event the action is associated with.
        /// </summary>
        public int EventRef => _eventRef;
        /// <summary>
        /// The byte[] describing the current state of the <see cref="ConfigPage"/> for the action.
        /// </summary>
        public byte[] Data => GetData();
        /// <summary>
        /// The generic name of this action type that is displayed in the list of available actions
        ///  a user can select from on the events page.
        /// </summary>
        public string Name => GetName();
        /// <summary>
        /// The currently selected sub-action index
        /// </summary>
        /// <remarks>
        /// -1 is it is not set
        /// </remarks>
        protected int SelectedSubActionIndex { get; private set; } = -1;
        
        /// <summary>
        /// Initialize a new <see cref="AbstractActionType"/> with the specified ID, Event Ref, and Data byte array.
        ///  The byte array will be automatically parsed for a <see cref="Page"/>, and a new one will be created if
        ///  the array is empty.
        /// <para>
        /// This is called through reflection by the <see cref="ActionTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// <para>
        /// You MUST implement one of these constructors in any class that derives from <see cref="AbstractActionType"/>
        /// </para>
        /// </summary>
        /// <param name="id">The unique ID of this action in HomeSeer</param>
        /// <param name="eventRef">The event reference ID that this action is associated with in HomeSeer</param>
        /// <param name="dataIn">A byte array containing the definition for a <see cref="Page"/></param>
        protected AbstractActionType(int id, int subTypeNumber, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener) {
            _id                    = id;
            SelectedSubActionIndex = subTypeNumber;
            _eventRef              = eventRef;
            _inData                = dataIn;
            ActionListener         = listener;
            InflateActionFromData();
        }

        /// <summary>
        /// Initialize a new <see cref="AbstractActionType"/> with the specified ID, Event Ref, and Data byte array.
        ///  The byte array will be automatically parsed for a <see cref="Page"/>, and a new one will be created if
        ///  the array is empty.
        /// <para>
        /// This is called through reflection by the <see cref="ActionTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// <para>
        /// You MUST implement this constructor in any class that derives from <see cref="AbstractActionType"/>
        /// </para>
        /// </summary>
        /// <param name="id">The unique ID of this action in HomeSeer</param>
        /// <param name="eventRef">The event reference ID that this action is associated with in HomeSeer</param>
        /// <param name="dataIn">A byte array containing the definition for a <see cref="Page"/></param>
        protected AbstractActionType(int id, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener, bool logDebug = false) {
            _id           = id;
            _eventRef     = eventRef;
            _inData = dataIn;
            ActionListener = listener;
            LogDebug = logDebug;
            InflateActionFromData();
        }
        
        /// <summary>
        /// Initialize a new <see cref="AbstractActionType"/> with the specified ID, Event Ref, and Data byte array.
        ///  The byte array will be automatically parsed for a <see cref="Page"/>, and a new one will be created if
        ///  the array is empty.
        /// <para>
        /// This is called through reflection by the <see cref="ActionTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// <para>
        /// You MUST implement one of these constructors in any class that derives from <see cref="AbstractActionType"/>
        /// </para>
        /// </summary>
        /// <param name="id">The unique ID of this action in HomeSeer</param>
        /// <param name="eventRef">The event reference ID that this action is associated with in HomeSeer</param>
        /// <param name="dataIn">A byte array containing the definition for a <see cref="Page"/></param>
        protected AbstractActionType(int id, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener) {
            _id            = id;
            _eventRef      = eventRef;
            _inData        = dataIn;
            ActionListener = listener;
            InflateActionFromData();
        }

        /// <summary>
        /// Initialize a new, unconfigured <see cref="AbstractActionType"/>
        /// <para>
        /// This is called through reflection by the <see cref="ActionTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// </summary>
        protected AbstractActionType() {}

        /// <summary>
        /// Use this as a unique prefix for all of your JUI views and as the ID for the <see cref="ConfigPage"/>
        /// </summary>
        protected string PageId => $"{_eventRef}-{_id}";

        /// <summary>
        /// The <see cref="Jui.Views.Page"/> displayed to users to allow them to configure this action.
        /// <para>
        /// The <see cref="Page.Name"/> of this page is not used or displayed anywhere and is not important.
        /// </para>
        /// </summary>
        /// <remarks>
        /// The ID of this page must be equal to the automatic <see cref="PageId"/>.
        /// </remarks>
        protected Page ConfigPage {
            get => _configPage ?? (_configPage = PageFactory.CreateEventActionPage(PageId, Name).Page);
            set {
                if (value.Id != PageId) {
                    throw new Exception($"The page ID must be {PageId}");
                }

                if (value.Type != EPageType.EventAction) {
                    throw new ViewTypeMismatchException("The page type must be EPageType.EventAction");
                }

                _configPage = value;
            }
        }
        
        private readonly int _id;
        private readonly int _eventRef;
        private byte[] _data;
        private Page _configPage;

        private readonly byte[] _inData;
        
        /// <summary>
        /// Called by HomeSeer to obtain the name of this action type.
        /// </summary>
        /// <returns>A generic name for this action to be displayed in the list of available actions.</returns>
        protected abstract string GetName();

        /// <summary>
        /// Called when a new action of this type is being created. Initialize the <see cref="ConfigPage"/> to
        ///  the action's starting state so users can begin configuring it.
        /// <para>
        /// Initialize a new <see cref="ConfigPage"/> and add views to it so the user can configure the trigger.
        ///  Any JUI view added to the <see cref="ConfigPage"/> must use a unique ID as it will
        ///  be displayed on an event page that could also be housing HTML from other plugins. It is recommended
        ///  to use the <see cref="PageId"/> as a prefix for all views added to ensure that their IDs are unique.
        /// </para>
        /// </summary>
        protected abstract void OnNewAction();
        
        /// <summary>
        /// Called to determine if this action is configured completely or if there is still more to configure.
        /// </summary>
        /// <returns>
        /// TRUE if the action is configured and can be formatted for display,
        ///  FALSE if there are more options to configure before the action can be used.
        /// </returns>
        public abstract bool IsFullyConfigured();

        /// <summary>
        /// Called when an action of this type is being edited and changes need to be propagated to the <see cref="ConfigPage"/>
        /// </summary>
        /// <param name="viewChanges">A <see cref="Page"/> containing changes to the <see cref="ConfigPage"/></param>
        protected virtual void OnEditAction(Page viewChanges) {
            var viewCount = ConfigPage?.ViewCount ?? 0;
            foreach (var changedView in viewChanges.Views) {

                if (!(ConfigPage?.ContainsViewWithId(changedView.Id) ?? false)) {
                    continue;
                }

                if (OnConfigItemUpdate(changedView)) {
                    ConfigPage.UpdateViewById(changedView);
                }

                if (viewCount != (ConfigPage?.ViewCount ?? 0)) {
                    break;
                }
            }
        }

        /// <summary>
        /// Called when a view on the <see cref="ConfigPage"/> has been updated by a user and needs to be processed.
        /// </summary>
        /// <param name="configViewChange">The new state of the view that was changed</param>
        /// <returns>
        /// TRUE to update the view in the <see cref="ConfigPage"/> and save the change, or
        ///  FALSE to discard the change.
        /// </returns>
        protected abstract bool OnConfigItemUpdate(AbstractView configViewChange);

        /// <summary>
        /// Called by HomeSeer when the action is configured and needs to be displayed to the user as an
        ///  easy to read sentence that flows with an IF... THEN... format.
        /// </summary>
        /// <returns>
        /// An easy to read, HTML formatted string describing the action as it would follow a THEN...
        /// </returns>
        public abstract string GetPrettyString();

        /// <summary>
        /// Called when this action needs to be executed.
        /// </summary>
        /// <returns></returns>
        public abstract bool OnRunAction();

        /// <summary>
        /// Called by HomeSeer to determine if this action references the device or feature with the specified ref.
        /// </summary>
        /// <param name="devOrFeatRef">The unique <see cref="AbstractHsDevice.Ref"/> to check for</param>
        /// <returns>
        /// TRUE if the action references the specified device/feature,
        ///  FALSE if it does not.
        /// </returns>
        public abstract bool ReferencesDeviceOrFeature(int devOrFeatRef);

        /// <summary>
        /// Called by <see cref="ActionTypeCollection"/> when <see cref="IPlugin.ActionBuildUI"/> is called to get
        ///  the HTML to display to the user so they can configure the action.
        /// <para>
        /// This HTML is automatically generated by the <see cref="ConfigPage"/> defined in the action.
        /// </para>
        /// </summary>
        /// <returns>HTML to show on the HomeSeer events page for the user.</returns>
        public string ToHtml() {
            return ConfigPage?.ToHtml() ?? "";
        }

        internal bool ProcessPostData(Dictionary<string, string> changes) {
            if (ConfigPage == null) {
                throw new Exception("Cannot process update.  There is no page to map changes to.");
            }

            if (changes == null || changes.Count == 0) {
                return true;
            }

            var pageChanges = PageFactory.CreateGenericPage(ConfigPage.Id, ConfigPage.Name).Page;

            foreach (var viewId in changes.Keys) {
                
                if (!ConfigPage.ContainsViewWithId(viewId)) {
                    continue;
                }

                var viewType = ConfigPage.GetViewById(viewId).Type;
                try {
                    pageChanges.AddViewDelta(viewId, (int) viewType, changes[viewId]);
                }
                catch (Exception exception) {
                    //Failed to add view change
                    if (LogDebug) {
                        Console.WriteLine(exception);
                    }
                }
            }

            if (pageChanges.ViewCount == 0) {
                return true;
            }

            OnEditAction(pageChanges);
            return true;
        }

        /// <summary>
        /// Deserialize the action data to a <see cref="HomeSeer.Jui.Views.Page"/>.
        /// <para>
        /// Override this if you need to support legacy actions. Convert the UI to the new format and save it in
        ///  the <see cref="ConfigPage"/>. Finally, return <see cref="Data"/> to automatically
        ///  serialize the ConfigPage to byte[].  Use <see cref="TrigActInfo.DeserializeLegacyData"/> to
        ///  deserialize the data using the legacy method.
        /// </para>
        /// </summary>
        /// <param name="inData">A byte array describing the current action configuration.</param>
        /// <returns>
        /// A byte array describing the current action configuration.
        /// </returns>
        protected virtual byte[] ProcessData(byte[] inData) {
            //Is data null/empty?
            if (inData == null || inData.Length == 0) {
                
                return new byte[0];
            }
            
            try {
                //Get JSON string from byte[]
                var pageJson = Encoding.UTF8.GetString(inData);
                //Deserialize to page
                ConfigPage = Page.FromJsonString(pageJson);

                return inData;
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
            }
            
            return new byte[0];
        }

        private void InflateActionFromData() {

            try {
                var processedData = ProcessData(_inData);
                if (processedData.Length == 0) {
                    _data = new byte[0];
                    OnNewAction();
                }
                else {
                    //Save the data
                    _data = processedData;
                }
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception.StackTrace);
                }
                _data = new byte[0];
                OnNewAction();
            }
        }

        private byte[] GetData() {
            var pageJson = ConfigPage.ToJsonString();
            return Encoding.UTF8.GetBytes(pageJson);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            if (!(obj is AbstractActionType otherActionType)) {
                return false;
            }

            if (_id != otherActionType._id) {
                return false;
            }

            if (_eventRef != otherActionType._eventRef) {
                return false;
            }

            if (_data != otherActionType._data) {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return 271828 * _id.GetHashCode() * _eventRef.GetHashCode();
        }

    }

}