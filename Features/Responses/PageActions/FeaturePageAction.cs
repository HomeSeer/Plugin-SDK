using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// An action to be executed on a feature page using javascript
    /// </summary>
    /// <seealso cref="PageActionResponse"/>
    [JsonObject]
    public class FeaturePageAction {

        /// <summary>
        /// The page action to take. See <see cref="EPageAction"/> for acceptable keys
        /// </summary>
        [JsonProperty("page_action")]
        public string PageAction { get; set; }
        
        /// <summary>
        /// The JQuery selector to use when performing the action
        /// </summary>
        [JsonProperty("selector")]
        public string Selector { get; set; }
        
        /// <summary>
        /// Additional data associated with the action
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// Create a new, empty FeaturePageAction
        /// </summary>
        [JsonConstructor]
        public FeaturePageAction() { }

        /// <summary>
        /// Create a new FeaturePageAction
        /// </summary>
        /// <param name="pageAction">The key for the action to take. See <see cref="EPageAction"/></param>
        /// <param name="selector">The JQuery <see cref="Selector"/> to use</param>
        /// <param name="data">Data to include with the action. See documentation on each action for more info.</param>
        public FeaturePageAction(string pageAction, string selector, string data = null) {
            PageAction = pageAction;
            Selector = selector;
            Data = data;
        }

    }
}