namespace HomeSeer.PluginSdk {

    public class WebPageDesc {

        public string plugInName     = "";
        public string plugInInstance = "";
        public string link           = "";
        public string linktext       = "";
        public string PluginID       = ""; // holds unique ID for the plugin registering this link
        public string page_title     = "";
        public int    order;       // used by registerHelpLink only
        public bool   IsJUI;       // true if JUI page, link is ID of plugin
        public int    JUIPageType; // HomeSeer.Jui.Types.EPageType
        public string JUIPageID;   // for non settings, this is the page ID, settings is an array of pages

    }

}