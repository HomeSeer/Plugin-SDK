namespace HomeSeer.PluginSdk.Features {

    /// <summary>
    /// The base implementation of a feature page in HS4.
    /// </summary>
    public interface IFeaturePage {

        /// <summary>
        /// The title of the page. Displayed in the plugin menu and by the browser client.
        /// </summary>
        string Title { get; set; }
        
        /// <summary>
        /// The name of the HTML file that backs this feature page. Ex "my-feature.html"
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Respond to an HTTP POST directed at this page.
        /// </summary>
        /// <param name="data">The body of data attached to the POST</param>
        /// <returns>The data to return as a string. Use JSON.</returns>
        string PostBackProc(string data, string user, int userRights);
        
        /// <summary>
        /// Get a fragment of HTML attached to a particular ID.
        /// </summary>
        /// <param name="fragmentId">The ID of the fragment to get</param>
        /// <returns>A fragment of HTML</returns>
        /// <remarks>Used by Scriban.</remarks>
        string GetHtmlFragment(string fragmentId);
    }

}