using System;
using System.Collections.Generic;
using System.Text;
using HomeSeer.Jui.Views;
// ReSharper disable UnusedParameter.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace HomeSeer.PluginSdk.Events {

    public abstract class AbstractActionType {

        public bool LogDebug { get; set; } = false;
        
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
        /// Initialize a new <see cref="AbstractActionType"/> with the specified ID, Event Ref, and Data byte array.
        ///  The byte array will be automatically parsed for a <see cref="Page"/>, and a new one will be created if
        ///  the array is empty.
        /// <para>
        /// This is called through reflection by the <see cref="ActionTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// </summary>
        /// <param name="id">The unique ID of this action in HomeSeer</param>
        /// <param name="eventRef">The event reference ID that this action is associated with in HomeSeer</param>
        /// <param name="dataIn">A byte array containing the definition for a <see cref="Page"/></param>
        protected AbstractActionType(int id, int eventRef, byte[] dataIn) {
            _id           = id;
            _eventRef     = eventRef;
            _data         = dataIn;
            ProcessData();
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
        /// Use this as a unique prefix for all of your JUI views. Also automatically used as the ID for the <see cref="ConfigPage"/>
        /// </summary>
        protected string PageId => $"{_eventRef}-{_id}";

        /// <summary>
        /// The <see cref="Jui.Views.Page"/> displayed to users to allow them to configure this action.
        /// </summary>
        protected Page ConfigPage { get; private set; }
        
        private readonly int _id;
        private readonly int _eventRef;
        private readonly byte[] _data;
        
        /// <summary>
        /// Called to determine if this action is configured completely or if there is still more to configure.
        /// </summary>
        /// <returns>
        /// TRUE if the action is configured and can be formatted for display,
        ///  FALSE if there are more options to configure before the action can be used.
        /// </returns>
        public abstract bool IsFullyConfigured();

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
        /// <para>
        /// 
        /// </para>
        /// </summary>
        /// <param name="devOrFeatRef"></param>
        /// <returns></returns>
        public abstract bool ReferencesDeviceOrFeature(int devOrFeatRef);
        
        /// <summary>
        /// Called when an action of this type is being edited and changes need to be propagated to the <see cref="ConfigPage"/>
        /// </summary>
        /// <param name="viewChanges">A <see cref="Page"/> containing changes to the <see cref="ConfigPage"/></param>
        protected abstract void OnEditAction(Page viewChanges);
        
        /// <summary>
        /// Called by HomeSeer to obtain the name of this action type.
        /// </summary>
        /// <returns>A generic name for this action to be displayed in the list of available actions.</returns>
        protected abstract string GetName();

        /// <summary>
        /// Called when a new action of this type is being created. Initialize the <see cref="ConfigPage"/> to
        ///  the action's starting state so users can begin configuring it.
        /// <para>
        /// A new <see cref="Jui.Views.Page"/> is already created at this point with a unique ID provided
        ///  by <see cref="PageId"/>. Any JUI view added to the <see cref="ConfigPage"/> must use a unique ID as it will
        ///  be displayed on an event page that could also be housing HTML from other plugins. It is recommended
        ///  to use the <see cref="PageId"/> as a prefix for all views added to ensure that their IDs are unique.
        /// </para>
        /// </summary>
        protected abstract void OnNewAction();

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

        protected virtual void InitializePage() {
            ConfigPage = PageFactory.CreateEventActionPage(PageId, Name).Page;
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

        private void ProcessData() {
            //Is data null/empty?
            if (_data == null || _data.Length == 0) {
                InitializePage();
                OnNewAction();
            }
            else {
                try {
                    //Get JSON string from byte[]
                    var pageJson = Encoding.UTF8.GetString(_data);
                    //Deserialize to page
                    ConfigPage = Page.FromJsonString(pageJson);
                }
                catch (Exception exception) {
                    if (LogDebug) {
                        Console.WriteLine(exception);
                    }
                    InitializePage();
                    OnNewAction();
                }
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