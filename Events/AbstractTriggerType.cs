using System;
using System.Collections.Generic;
using System.Text;
using HomeSeer.Jui.Views;

namespace HomeSeer.PluginSdk.Events {

    public abstract class AbstractTriggerType {

        public bool LogDebug { get; set; } = false;
        
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
            _data         = dataIn;
            ProcessData();
        }

        protected AbstractTriggerType() {}

        /// <summary>
        /// Use this as a unique prefix for all of your JUI views
        /// </summary>
        protected string PageId => $"{_eventRef}-{_id}";

        protected Page _page;
        
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
            return _page?.ToHtml() ?? "";
        }
        
        internal bool ProcessPostData(Dictionary<string, string> changes) {
            if (_page == null) {
                throw new Exception("Cannot process update.  There is no page to map changes to.");
            }

            if (changes == null || changes.Count == 0) {
                return true;
            }

            var pageChanges = PageFactory.CreateGenericPage(_page.Id, _page.Name).Page;

            foreach (var viewId in changes.Keys) {
                
                if (!_page.ContainsViewWithId(viewId)) {
                    continue;
                }

                var viewType = _page.GetViewById(viewId).Type;
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

        private void ProcessData() {
            //Is data null/empty?
            if (_data == null || _data.Length == 0) {
                _page = PageFactory.CreateEventTriggerPage(PageId, Name).Page;
                OnNewTrigger();
            }
            else {
                try {
                    //Get JSON string from byte[]
                    var pageJson = Encoding.UTF8.GetString(_data);
                    //Deserialize to page
                    _page = Page.FromJsonString(pageJson);
                }
                catch (Exception exception) {
                    if (LogDebug) {
                        Console.WriteLine(exception);
                    }
                    _page = PageFactory.CreateEventTriggerPage(PageId, Name).Page;
                    OnNewTrigger();
                }
            }
        }

        private byte[] GetData() {
            var pageJson = _page.ToJsonString();
            return Encoding.UTF8.GetBytes(pageJson);
        }

    }

}