using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// A POST response with JSON data for a feature page as a collection of key-value pairs that describes a series of
    ///  actions that will be executed by JavaScript
    /// </summary>
    [JsonObject]
    public class PageActionResponse : JsonResponse {

        /// <summary>
        /// The key for the collection of actions to execute
        /// </summary>
        protected const string PageActionsKey = "page_actions";
        
        /// <summary>
        /// A collection of <see cref="PageActions"/> to execute on the Feature Page
        /// </summary>
        [JsonIgnore]
        public List<FeaturePageAction> PageActions {
            get {
                var featurePageActionsJson = Get(PageActionsKey);
                return JsonConvert.DeserializeObject<List<FeaturePageAction>>(featurePageActionsJson);
            }
            set {
                var featurePageActionsJson = JsonConvert.SerializeObject(value);
                Put(PageActionsKey, featurePageActionsJson);
            }
        }

        /// <summary>
        /// Create a new, empty PageActionReponse
        /// </summary>
        [JsonConstructor]
        public PageActionResponse() { }

        /// <summary>
        /// Create a new PageActionResponse based on a received <see cref="JsonRequest"/>
        /// </summary>
        /// <param name="request"></param>
        public PageActionResponse(JsonRequest request) : base(request, PageActionsKey) { }

    }
}