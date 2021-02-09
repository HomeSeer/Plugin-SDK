using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses {

    /// <summary>
    /// A POST response with JSON data for a feature page as a collection of key-value pairs
    /// </summary>
    /// <remarks>
    /// <para>
    /// A default handler is provided for you in the FeaturePagePost.js page. Include that file with your plugin's
    ///  feature pages and customize it as needed. A shared version will be implemented in a future release.
    /// </para>
    /// </remarks>
    [JsonObject]
    public class JsonResponse : JsonRequest {

        /// <summary>
        /// Key that identifies the type of response
        /// </summary>
        protected const string ResponseKey = "response";

        /// <summary>
        /// The type of the response. This determines the behavior of the client.
        /// </summary>
        [JsonIgnore]
        public string Response {
            get => Get(ResponseKey);
            set => Put(ResponseKey, value);
        }

        /// <summary>
        /// Create a new, empty JsonResponse
        /// </summary>
        [JsonConstructor]
        public JsonResponse() { }
        
        /// <summary>
        /// Create a new JsonResponse using a set of JSON data
        /// </summary>
        /// <param name="genericJsonData">The <see cref="GenericJsonData"/> that describes the response</param>
        /// <exception cref="KeyNotFoundException">Thrown if no <see cref="Response"/> type is defined in the data</exception>
        public JsonResponse(GenericJsonData genericJsonData) : base(genericJsonData) {
            if (!genericJsonData.Contains(ResponseKey)) {
                throw new KeyNotFoundException($"{ResponseKey} key not found in data");
            }
        }

        /// <summary>
        /// Create a new JsonResponse based on a <see cref="JsonRequest"/>
        /// </summary>
        /// <param name="request">The <see cref="JsonRequest"/> to base this response on</param>
        /// <param name="response">The type of response</param>
        public JsonResponse(JsonRequest request, string response) {
            Request = request?.Request?? "";
            Response = response;
        }

    }

}