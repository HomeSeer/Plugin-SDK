using System;
using System.Collections.Generic;
using System.Text;
using HomeSeer.Jui.Views;

namespace HomeSeer.PluginSdk.Events {

    public abstract class AbstractTriggerType {

        public bool LogDebug { get; set; } = false;
        
        public TriggerTypeCollection.ITriggerTypeListener TriggerListener { get; internal set; }
        
        public int Id => _id;
        public int EventRef => _eventRef;
        public byte[] Data => GetData();
        public string Name => GetName();
        public virtual bool CanBeCondition => false;
        public int SubTriggerCount => SubTriggerTypeNames?.Count ?? 0;

        public int SelectedSubTriggerIndex {
            get => _selectedSubTriggerIndex;
            internal set => _selectedSubTriggerIndex = (value >= SubTriggerCount) ? -1 : value;
        }

        protected AbstractTriggerType(int id, int eventRef, byte[] dataIn) {
            _id           = id;
            _eventRef     = eventRef;
            ProcessData(dataIn);
        }

        protected AbstractTriggerType() {}

        /// <summary>
        /// Use this as a unique prefix for all of your JUI views
        /// </summary>
        protected string PageId => $"{_eventRef}-{_id}";

        protected Page ConfigPage;
        
        protected List<string> SubTriggerTypeNames = new List<string>();
        
        private int _id;
        private int _eventRef;
        private byte[] _data;
        private int _selectedSubTriggerIndex = -1;

        public abstract bool IsTriggerTrue(bool isCondition);
        
        public abstract bool IsFullyConfigured();

        public abstract string GetPrettyString();
        
        public abstract bool ReferencesDeviceOrFeature(int devOrFeatRef);
        
        protected abstract bool OnEditTrigger(Page viewChanges);
        
        protected abstract string GetName();

        protected abstract void OnNewTrigger();

        public string GetSubTriggerName(int subTriggerNum) {
            if (subTriggerNum >= SubTriggerTypeNames.Count) {
                throw new ArgumentOutOfRangeException(nameof(subTriggerNum), 
                                                      $"{subTriggerNum} is not within the range of 0-{SubTriggerTypeNames.Count}");
            }
            
            return SubTriggerTypeNames[subTriggerNum];
        }

        public string ToHtml() {
            return ConfigPage?.ToHtml() ?? "";
        }
        
        protected virtual void Initialize() {
            _data = new byte[0];
            ConfigPage = PageFactory.CreateEventTriggerPage(PageId, Name).Page;
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
            
            return OnEditTrigger(pageChanges);
        }

        /// <summary>
        /// Deserialize the trigger data to a <see cref="HomeSeer.Jui.Views.Page"/>.
        /// <para>
        /// Override this if you need to support legacy triggers. Convert the UI to the new format and save it in
        ///  the <see cref="ConfigPage"/>. Finally, call the base implementation of this method passing
        ///  <see cref="Data"/> for <see cref="inData"/>.  Use <see cref="TrigActInfo.DeserializeLegacyData"/> to
        ///  deserialize the data using the legacy method.
        /// </para>
        /// </summary>
        /// <param name="inData">A byte array describing the current trigger configuration.</param>
        protected virtual void ProcessData(byte[] inData) {
            //Is data null/empty?
            if (inData == null || inData.Length == 0) {
                Initialize();
                OnNewTrigger();
            }
            else {
                try {
                    //Get JSON string from byte[]
                    var pageJson = Encoding.UTF8.GetString(_data);
                    //Deserialize to page
                    ConfigPage = Page.FromJsonString(pageJson);
                    //Save the data
                    _data = inData;
                }
                catch (Exception exception) {
                    if (LogDebug) {
                        Console.WriteLine(exception);
                    }

                    Initialize();
                    OnNewTrigger();
                }
            }
        }

        private byte[] GetData() {
            var pageJson = ConfigPage.ToJsonString();
            return Encoding.UTF8.GetBytes(pageJson);
        }

    }

}