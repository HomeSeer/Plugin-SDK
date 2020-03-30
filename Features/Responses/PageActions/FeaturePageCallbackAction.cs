using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// A <see cref="FeaturePageAction"/> used to instruct the client to callback to the plugin after waiting a specified amount of time
    /// </summary>
    /// <remarks>
    /// Use <see cref="FeaturePageAction.Data"/> to store the data for the next request and specify how long to wait in <see cref="FeaturePageAction.Selector"/>
    /// </remarks>
    [JsonObject]
    public class FeaturePageCallbackAction : FeaturePageAction {

        //TODO timeout separate from the selector so it can be used for the URL
        /*[JsonProperty("timeout")]
        public int Timeout { get; set; }*/
        
        /// <summary>
        /// Create a new, empty FeaturePageCallbackAction
        /// </summary>
        [JsonConstructor]
        public FeaturePageCallbackAction() { }

        /// <summary>
        /// Create a new instance of a FeaturePageCallbackAction that will send the specified data blob after waiting the specified amount of time
        /// </summary>
        /// <param name="data">The data to include in the next request</param>
        /// <param name="timeout">The amount of time to wait, in millisecond, before making the callback request</param>
        public FeaturePageCallbackAction(string data, int timeout) {
            PageAction = "callback";
            Selector = timeout.ToString();
            Data = data;
            //Timeout = timeout;
        }

    }
}