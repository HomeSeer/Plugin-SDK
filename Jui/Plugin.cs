using System;
using System.Collections.Generic;
using HomeSeer.Jui.Views;
using Newtonsoft.Json;

namespace HomeSeer.Jui {

	/// <summary>
	/// A plugin that is installed on the HomeSeer system
	/// </summary>
    public class Plugin {

	    /// <summary>
	    /// A unique identifier for the plugin
	    /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
		
	    /// <summary>
	    /// The name of the plugin.  This is the title shown to the user
	    /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
		
	    /// <summary>
	    /// The version number of the plugin
	    /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
		
	    /// <summary>
	    /// Whether the plugin is enabled or not
	    /// </summary>
        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; }
		
	    /// <summary>
	    /// Whether the plugin has settings pages or not
	    /// </summary>
        [JsonProperty("has_settings")]
        public bool HasSettingsPages { get; set; }
		
	    /// <summary>
	    /// A URL pointing to the help documentation for the plugin
	    /// </summary>
        [JsonProperty("help_page")]
        public string HelpPage { get; set; }
		
	    /// <summary>
	    /// A list of feature pages registered by the plugin
	    /// </summary>
        [JsonProperty("pages")]
        public List<Page> Pages { get; set; }

	    /// <summary>
	    /// Create a new instance of a plugin with a unique ID and name
	    /// <para>
	    /// This plugin is initialized disabled
	    /// </para>
	    /// </summary>
	    /// <param name="id">The unique ID of the plugin</param>
	    /// <param name="name">The name of the plugin; the title shown to the user</param>
	    /// <exception cref="ArgumentNullException">Thrown if a Plugin is created with an invalid ID or Name</exception>
	    [JsonConstructor]
	    public Plugin(string id, string name) {
		    
		    if (string.IsNullOrWhiteSpace(id)) {
			    throw new ArgumentNullException(nameof(id), "A unique ID must be specified");
		    }

		    if (string.IsNullOrWhiteSpace(name)) {
			    throw new ArgumentNullException(nameof(name), "A plugin must have a name");
		    }
		    
		    Id   = id;
		    Name = name;
		    Version = null;
		    IsEnabled = false;
		    HasSettingsPages = false;
		    HelpPage = null;
		    Pages = new List<Page>();
	    }

	    /// <inheritdoc />
	    /// <summary>
	    /// Compares the Id, Version, Name, and the number of pages
	    /// </summary>
	    public override bool Equals(object obj) {
		    if (ReferenceEquals(null, obj)) return false;
		    if (ReferenceEquals(this, obj)) return true;
		    if (obj.GetType() != GetType()) return false;
		    
		    if (!(obj is Plugin otherPlugin)) {
			    return false;
		    }
			
		    if (Id != otherPlugin.Id) {
			    return false;
		    }

		    if (Version != otherPlugin.Version) {
			    return false;
		    }
		    
		    if (Name != otherPlugin.Name) {
			    return false;
		    }
		    
		    if (Pages.Count != otherPlugin.Pages.Count) {
			    return false;
		    }

		    return true;
	    }

	    /// <inheritdoc />
	    /// <summary>
	    /// Compares the Id, Version, Name, and the number of pages
	    /// </summary>
	    public override int GetHashCode() {
		    
		    return Id.GetHashCode() * Version.GetHashCode() * Name.GetHashCode() * Pages.Count.GetHashCode();
	    }

	}

}