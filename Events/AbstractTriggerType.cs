using System;
using System.Collections.Generic;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk.Devices;

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// The base implementation of a plugin trigger type available for users to select in HomeSeer
    /// <para>
    /// Inherit from this class to define your own trigger types and store them in your plugin's <see cref="TriggerTypeCollection"/>
    /// </para>
    /// </summary>
    public abstract class AbstractTriggerType {

        /// <summary>
        /// Used to enable/disable internal logging to the console
        /// <para>
        /// When it is TRUE, log messages from the PluginSdk code will be written to the Console
        /// </para>
        /// </summary>
        public bool LogDebug { get; set; }
        
        /// <summary>
        /// An interface reference to the plugin that owns this trigger type.
        /// <para>
        /// Define your own interface that inherits from <see cref="TriggerTypeCollection.ITriggerTypeListener"/>
        ///  and then cast this as the type you defined to get a reference to your plugin that can handle any methods
        ///  you wish to define.
        /// </para>
        /// </summary>
        public TriggerTypeCollection.ITriggerTypeListener TriggerListener { get; internal set; }
        
        /// <summary>
        /// The unique ID for the trigger.
        /// </summary>
        public int Id => _id;
        /// <summary>
        /// The reference ID of the event the trigger is associated with.
        /// </summary>
        public int EventRef => _eventRef;
        /// <summary>
        /// The byte[] describing the current state of the <see cref="ConfigPage"/> for the trigger.
        /// </summary>
        public byte[] Data => GetData();
        /// <summary>
        /// The generic name of this trigger type that is displayed in the list of available triggers
        ///  a user can select from on the events page.
        /// </summary>
        public string Name => GetName();
        /// <summary>
        /// A boolean value indicating whether this trigger type can be used as a condition or not.
        ///  A condition is a trigger that operates in conjunction with another trigger in an AND/OR pattern.
        /// </summary>
        public virtual bool CanBeCondition => false;
        /// <summary>
        /// The number of sub-trigger types this trigger type supports.
        /// </summary>
        public int SubTriggerCount => SubTriggerTypeNames?.Count ?? 0;

        /// <summary>
        /// The currently selected sub-trigger index
        /// </summary>
        protected int SelectedSubTriggerIndex {
            get => _selectedSubTriggerIndex;
            private set => _selectedSubTriggerIndex = (value >= SubTriggerCount) ? -1 : value;
        }

        private readonly byte[] _inData;

        /// <summary>
        /// Initialize a new <see cref="AbstractTriggerType"/> with the specified ID, Event Ref, and Data byte array.
        ///  The byte array will be automatically parsed for a <see cref="Page"/>, and a new one will be created if
        ///  the array is empty.
        /// <para>
        /// This is called through reflection by the <see cref="TriggerTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// <para>
        /// You MUST implement one of these constructor signatures in any class that derives from <see cref="AbstractTriggerType"/>
        /// </para>
        /// </summary>
        /// <param name="id">The unique ID of this trigger in HomeSeer</param>
        /// <param name="eventRef">The event reference ID that this trigger is associated with in HomeSeer</param>
        /// <param name="selectedSubTriggerIndex">The 0 based index of the sub-trigger type selected for this trigger</param>
        /// <param name="dataIn">A byte array containing the definition for a <see cref="Page"/></param>
        protected AbstractTriggerType(int id, int eventRef, int selectedSubTriggerIndex, byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug = false) {
            _id           = id;
            _eventRef     = eventRef;
            SelectedSubTriggerIndex = selectedSubTriggerIndex;
            _inData = dataIn;
            TriggerListener = listener;
            LogDebug = logDebug;
            InflateTriggerFromData();
        }
        
        /// <summary>
        /// Initialize a new <see cref="AbstractTriggerType"/> with the specified ID, Event Ref, and Data byte array.
        ///  The byte array will be automatically parsed for a <see cref="Page"/>, and a new one will be created if
        ///  the array is empty.
        /// <para>
        /// This is called through reflection by the <see cref="TriggerTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// <para>
        /// You MUST implement one of these constructor signatures in any class that derives from <see cref="AbstractTriggerType"/>
        /// </para>
        /// </summary>
        /// <param name="id">The unique ID of this trigger in HomeSeer</param>
        /// <param name="eventRef">The event reference ID that this trigger is associated with in HomeSeer</param>
        /// <param name="selectedSubTriggerIndex">The 0 based index of the sub-trigger type selected for this trigger</param>
        /// <param name="dataIn">A byte array containing the definition for a <see cref="Page"/></param>
        protected AbstractTriggerType(int id, int eventRef, int selectedSubTriggerIndex, byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener) {
            _id                     = id;
            _eventRef               = eventRef;
            SelectedSubTriggerIndex = selectedSubTriggerIndex;
            _inData                 = dataIn;
            TriggerListener         = listener;
            InflateTriggerFromData();
        }

        protected AbstractTriggerType(TrigActInfo trigInfo, TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug = false) {
            _id = trigInfo.UID;
            _eventRef = trigInfo.evRef;
            SelectedSubTriggerIndex = trigInfo.SubTANumber-1;
            _inData = trigInfo.DataIn;
            TriggerListener = listener;
            LogDebug        = logDebug;
            InflateTriggerFromData();
        }

        /// <summary>
        /// Initialize a new, unconfigured <see cref="AbstractTriggerType"/>
        /// <para>
        /// This is called through reflection by the <see cref="TriggerTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// </summary>
        protected AbstractTriggerType() {}

        /// <summary>
        /// Use this as a unique prefix for all of your JUI views and as the ID for the <see cref="ConfigPage"/>
        /// </summary>
        protected string PageId => $"{_eventRef}-{_id}";

        /// <summary>
        /// The <see cref="Jui.Views.Page"/> displayed to users to allow them to configure this trigger.
        /// <para>
        /// The <see cref="Page.Name"/> of this page is not used or displayed anywhere and is not important.
        /// </para>
        /// </summary>
        /// <remarks>
        /// The ID of this page must be equal to the automatic <see cref="PageId"/>.
        /// </remarks>
        protected Page ConfigPage {
            get => _configPage ?? (_configPage = PageFactory.CreateEventTriggerPage(PageId, Name).Page);
            set {
                if (value.Id != PageId) {
                    throw new Exception($"The page ID must be {PageId}");
                }

                if (value.Type != EPageType.EventTrigger) {
                    throw new ViewTypeMismatchException("The page type must be EPageType.EventAction");
                }

                _configPage = value;
            }
        }
        
        /// <summary>
        /// A <see cref="List{T}"/> of names for the available sub-trigger types users can select from for this trigger type.
        ///  Leave this list empty if the trigger type does not support any subtypes.
        /// </summary>
        protected virtual List<string> SubTriggerTypeNames { get; set; } = new List<string>();
        
        private int _id;
        private int _eventRef;
        private byte[] _data;
        private Page _configPage;
        private int _selectedSubTriggerIndex = -1;
        
        /// <summary>
        /// Called by HomeSeer to obtain the name of this trigger type.
        /// </summary>
        /// <returns>A generic name for this trigger to be displayed in the list of available triggers.</returns>
        protected abstract string GetName();
        
        /// <summary>
        /// Called by HomeSeer to obtain the name of the sub-trigger with the specified index
        /// </summary>
        /// <param name="subTriggerNum">The index of the requested sub-trigger in the <see cref="SubTriggerTypeNames"/> property</param>
        /// <returns>The name of the sub-trigger for the specified index</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the index requested does not exist in the list of available sub-triggers for this trigger type.
        /// </exception>
        public string GetSubTriggerName(int subTriggerNum) {
            if (subTriggerNum >= SubTriggerTypeNames.Count) {
                throw new ArgumentOutOfRangeException(nameof(subTriggerNum), 
                                                      $"{subTriggerNum} is not within the range of 0-{SubTriggerTypeNames.Count}");
            }
            
            return SubTriggerTypeNames[subTriggerNum];
        }

        /// <summary>
        /// Called when a new trigger of this type is being created. Initialize the <see cref="ConfigPage"/> to
        ///  the trigger's starting state so users can begin configuring it.
        /// <para>
        /// You must create a new <see cref="Jui.Views.Page"/> with a unique ID provided by <see cref="PageId"/>
        ///  and be of the type <see cref="EPageType.EventTrigger"/>.
        ///  Any JUI view added to the <see cref="ConfigPage"/> must use a unique ID as it will
        ///  be displayed on an event page that could also be housing HTML from other plugins. It is recommended
        ///  to use the <see cref="PageId"/> as a prefix for all views added to ensure that their IDs are unique.
        /// </para>
        /// <para>
        /// If no page is set, a blank page will be auto initialized.
        /// </para>
        /// </summary>
        protected abstract void OnNewTrigger();
        
        /// <summary>
        /// Called to determine if this trigger is configured completely or if there is still more to configure.
        /// </summary>
        /// <returns>
        /// TRUE if the trigger is configured and can be formatted for display,
        ///  FALSE if there are more options to configure before the trigger can be used.
        /// </returns>
        public abstract bool IsFullyConfigured();

        /// <summary>
        /// Called when a trigger of this type is being edited and changes need to be propagated to the <see cref="ConfigPage"/>
        /// <para>
        /// We do not recommend overriding this method unless you specifically want to adjust the way view changes
        ///  are processed as a whole.
        /// </para>
        /// </summary>
        /// <param name="viewChanges">A <see cref="Page"/> containing changes to the <see cref="ConfigPage"/></param>
        protected virtual void OnEditTrigger(Page viewChanges) {
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
        /// Called by HomeSeer when the trigger is configured and needs to be displayed to the user as an
        ///  easy to read sentence that flows with an IF... THEN... format.
        /// <para>
        /// This is currently a WIP (Work in Progress) The end goal is to provide a StringBuilder-like class that
        ///  makes it easy to output pre-formatted HTML so that all common items are quickly identifiable by users.
        ///  (ex. All device/feature names are colored the same and bolded)
        /// </para>
        /// </summary>
        /// <returns>
        /// An easy to read string describing the trigger as it would follow an IF...
        /// </returns>
        public abstract string GetPrettyString();
        
        /// <summary>
        /// Called by HomeSeer to determine if this trigger's conditions have been met.
        /// </summary>
        /// <remarks>
        /// Always return TRUE if the trigger cannot be a condition and there is nothing to check when an event is
        ///  manually executed by a user.
        /// </remarks>
        /// <param name="isCondition">TRUE if the trigger is paired with other triggers, FALSE if it is alone.</param>
        /// <returns>
        /// TRUE if the trigger's conditions have been met,
        ///  FALSE if they haven't
        /// </returns>
        public abstract bool IsTriggerTrue(bool isCondition);
        
        /// <summary>
        /// Called by HomeSeer to determine if this trigger references the device or feature with the specified ref.
        /// </summary>
        /// <param name="devOrFeatRef">The unique <see cref="AbstractHsDevice.Ref"/> to check for</param>
        /// <returns>
        /// TRUE if the trigger references the specified device/feature,
        ///  FALSE if it does not.
        /// </returns>
        public abstract bool ReferencesDeviceOrFeature(int devOrFeatRef);
        
        /// <summary>
        /// Called by <see cref="TriggerTypeCollection"/> when <see cref="IPlugin.TriggerBuildUI"/> is called to get
        ///  the HTML to display to the user so they can configure the action.
        /// <para>
        /// This HTML is automatically generated by the <see cref="ConfigPage"/> defined in the trigger.
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

            OnEditTrigger(pageChanges);
            return true;
        }

        private byte[] ProcessData(byte[] inData) {
            //Is data null/empty?
            if (inData == null || inData.Length == 0) {
                return new byte[0];
            }
            
            try {
                //Get JSON string from byte[]
                var pageJson = Encoding.UTF8.GetString(inData);
                //Deserialize to page
                ConfigPage = Page.FromJsonString(pageJson);
                //Save the data
                return inData;
            }
            catch (Exception exception) {
                //Exception is expected if the data is of a legacy type
                if (LogDebug) {
                    Console.WriteLine($"Exception while trying to execute ProcessData on trigger data, possibly legacy data - {exception.Message}");
                }
            }

            //If deserialization failed, try to convert legacy data to new format
            return ConvertLegacyData(inData);
        }

        /// <summary>
        /// Called when legacy trigger data needs to be converted to the new format
        /// <para>
        /// Override this if you need to support legacy triggers. Convert the UI to the new format and save it in
        ///  the <see cref="ConfigPage"/>. Finally, return <see cref="Data"/> to automatically
        ///  serialize the ConfigPage to byte[].  Use <see cref="TrigActInfo.DeserializeLegacyData"/> to
        ///  deserialize the data using the legacy method.
        /// </para>
        /// </summary>
        /// <remarks>
        /// This is also called if there was an error while trying to deserialize the modern data format as a fallback
        /// </remarks>
        /// <param name="inData">A byte array describing the current trigger configuration in legacy format.</param>
        /// <returns>
        /// A byte array describing the current trigger configuration in new format.
        /// </returns>
        protected virtual byte[] ConvertLegacyData(byte[] inData) {
            return new byte[0];
        }

        private void InflateTriggerFromData() {

            try {
                var processedData = ProcessData(_inData);
                if (processedData.Length == 0) {
                    _data = new byte[0];
                    OnNewTrigger();
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
                OnNewTrigger();
            }
        }

        private byte[] GetData() {
            var pageJson = ConfigPage.ToJsonString();
            return Encoding.UTF8.GetBytes(pageJson);
        }
        
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            if (!(obj is AbstractTriggerType otherTriggerType)) {
                return false;
            }

            if (_id != otherTriggerType._id) {
                return false;
            }

            if (_eventRef != otherTriggerType._eventRef) {
                return false;
            }

            if (_data != otherTriggerType._data) {
                return false;
            }

            return true;
        }

        public override int GetHashCode() {
            return 271828 * _id.GetHashCode() * _eventRef.GetHashCode();
        }

    }

}