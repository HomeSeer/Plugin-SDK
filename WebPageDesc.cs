using System;

namespace HomeSeer.PluginSdk {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class WebPageDesc {

        public string plugInName     = "";
        public string plugInInstance = "";
        public string link           = "";
        public string linktext       = "";
        public string PluginID       = ""; // holds unique ID for the plugin registering this link
        public string page_title     = "";
        public string fileName       = ""; // The name of the file that should be loaded for this page
        public int    order;       // used by registerHelpLink only
        public bool   IsJUI;       // true if JUI page, link is ID of plugin
        public int    JUIPageType; // HomeSeer.Jui.Types.EPageType
        public string JUIPageID;   // for non settings, this is the page ID, settings is an array of pages

    }

}