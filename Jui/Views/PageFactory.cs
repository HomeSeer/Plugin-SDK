using HomeSeer.Jui.Types;

namespace HomeSeer.Jui.Views {

    public class PageFactory {
        
        public Page Page { get; private set; }

        /// <summary>
        /// Create a new settings page
        /// </summary>
        /// <param name="id">The ID for the page</param>
        /// <param name="name">The name of the page</param>
        /// <returns>A new PageFactory containing a page with its type set to EPageType.Settings</returns>
        public static PageFactory CreateSettingsPage(string id, string name) {
            var pf = new PageFactory();
            pf.Page = new Page(id, name, EPageType.Settings);
            return pf;
        }
        
        /// <summary>
        /// Create a new device configuration page
        /// </summary>
        /// <param name="id">The ID for the page</param>
        /// <param name="name">The name of the page</param>
        /// <returns>A new PageFactory containing a page with its type set to EPageType.DeviceConfig</returns>
        public static PageFactory CreateDeviceConfigPage(string id, string name) {
				
            var pf = new PageFactory();
            pf.Page = new Page(id, name, EPageType.DeviceConfig);
            return pf;
        }
        
        /// <summary>
        /// Create a new, generic page
        /// </summary>
        /// <param name="id">The ID for the page</param>
        /// <param name="name">The name of the page</param>
        /// <returns>A new PageFactory containing a page with its type set to EPageType.Generic</returns>
        public static PageFactory CreateGenericPage(string id, string name) {
				
            var pf = new PageFactory();
            pf.Page = new Page(id, name, EPageType.Generic);
            return pf;
        }
			
        /// <summary>
        /// Create a new, event action page
        /// </summary>
        /// <param name="id">The ID for the page</param>
        /// <param name="name">The name of the page</param>
        /// <returns>A new PageFactory containing a page with its type set to EPageType.EventAction</returns>
        public static PageFactory CreateEventActionPage(string id, string name) {
				
            var pf = new PageFactory();
            pf.Page = new Page(id, name, EPageType.EventAction);
            return pf;
        }
			
        /// <summary>
        /// Create a new, event trigger page
        /// </summary>
        /// <param name="id">The ID for the page</param>
        /// <param name="name">The name of the page</param>
        /// <returns>A new PageFactory containing a page with its type set to EPageType.EventTrigger</returns>
        public static PageFactory CreateEventTriggerPage(string id, string name) {
				
            var pf = new PageFactory();
            pf.Page = new Page(id, name, EPageType.EventTrigger);
            return pf;
        }

        public PageFactory WithLabel(LabelView label) {
            Page.AddView(label);
            return this;
        }

        public PageFactory WithLabel(string id, string name, string value = "") {
            var lv = new LabelView(id, name, value);
            Page.AddView(lv);
            return this;
        }

        public PageFactory WithInput(InputView input) {
            Page.AddView(input);
            return this;
        }
        
        public PageFactory WithSelectList(SelectListView selectList) {
            Page.AddView(selectList);
            return this;
        }
        
        public PageFactory WithToggle(ToggleView toggle) {
            Page.AddView(toggle);
            return this;
        }
        
        public PageFactory WithGroup(ViewGroup group) {
            Page.AddView(group);
            return this;
        }

    }

}