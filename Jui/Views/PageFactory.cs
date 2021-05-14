using System.Collections.Generic;
using HomeSeer.Jui.Types;

namespace HomeSeer.Jui.Views {

    //TODO documentation and examples
    
    /// <summary>
    /// A factory class for creating pages in a more streamlined way
    /// </summary>
    public class PageFactory {
        
        /// <summary>
        /// The page being created
        /// </summary>
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

        public static PageFactory CopyPage(Page page) {
            var pf = new PageFactory();
            pf.Page = new Page(page.Id, page.Name, page.Type);
            pf.Page.SetViews(page.Views);
            return pf;
        }

        public PageFactory WithView(AbstractView view) {
            Page.AddView(view);
            return this;
        }

        public PageFactory WithLabel(string id, string name) {
            var nl = new LabelView(id, name);
            Page.AddView(nl);
            return this;
        }

        public PageFactory WithLabel(string id, string name, string value = "") {
            var lv = new LabelView(id, name, value);
            Page.AddView(lv);
            return this;
        }

        public PageFactory WithInput(string id, string name, EInputType type = EInputType.Text) {
            var iv = new InputView(id, name, type);
            Page.AddView(iv);
            return this;
        }
        
        public PageFactory WithInput(string id, string name, string value, EInputType type = EInputType.Text) {
            var iv = new InputView(id, name, value, type);
            Page.AddView(iv);
            return this;
        }

        public PageFactory WithTextArea(string id, string name, int rows = 5)
        {
            var tav = new TextAreaView(id, name, rows);
            Page.AddView(tav);
            return this;
        }

        public PageFactory WithTextArea(string id, string name, string value, int rows = 5)
        {
            var tav = new TextAreaView(id, name, value, rows);
            Page.AddView(tav);
            return this;
        }

        public PageFactory WithDropDownSelectList(string id, string name, List<string> options, int selection = -1) {
            var slv = new SelectListView(id, name, options, ESelectListType.DropDown, selection);
            Page.AddView(slv);
            return this;
        }
        
        public PageFactory WithDropDownSelectList(string id, string name, List<string> options, List<string> optionKeys,
                                                  int selection = -1) {
            var slv = new SelectListView(id, name, options, optionKeys, ESelectListType.DropDown, selection);
            Page.AddView(slv);
            return this;
        }
        
        public PageFactory WithRadioSelectList(string id, string name, List<string> options, int selection = -1) {
            var slv = new SelectListView(id, name, options, ESelectListType.RadioList, selection);
            Page.AddView(slv);
            return this;
        }
        
        public PageFactory WithRadioSelectList(string id, string name, List<string> options, List<string> optionKeys,
                                               int selection = -1) {
            var slv = new SelectListView(id, name, options, optionKeys, ESelectListType.RadioList, selection);
            Page.AddView(slv);
            return this;
        }

        public PageFactory WithToggle(string id, string name, bool isEnabled = false) {
            var tv = new ToggleView(id, name, isEnabled);
            Page.AddView(tv);
            return this;
        }
        
        public PageFactory WithCheckBox(string id, string name, bool isEnabled = false) {
            var tv = new ToggleView(id, name, isEnabled);
            tv.ToggleType = EToggleType.Checkbox;
            Page.AddView(tv);
            return this;
        }

        public PageFactory WithGroup(string id, string name, IEnumerable<AbstractView> views) {
            var vg = new ViewGroup(id, name);
            vg.AddViews(views);
            Page.AddView(vg);
            return this;
        }

    }

}