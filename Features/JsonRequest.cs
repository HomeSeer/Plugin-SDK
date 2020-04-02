using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features {

    /// <summary>
    /// <para>
    /// A POST request with JSON data from a web client to a feature page as a collection of key-value pairs
    /// </para>
    /// <para>
    /// Use JSON stringify to build a request in JavaScript:
    /// <code>
    /// JSON.stringify({ data: {"request" : "load-page", "key" : "value"} });
    /// </code>
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// A default handler is provided for you in the FeaturePagePost.js page. Include that file with your plugin's
    ///  feature pages and customize it as needed. A shared version will be implemented in a future release.
    /// </para>
    /// </remarks>
    [JsonObject]
    public class JsonRequest : GenericJsonData {

        /// <summary>
        /// Key that identifies the type request being made
        /// </summary>
        protected const string RequestKey = "request";
        
        /// <summary>
        /// The type of request being made
        /// </summary>
        [JsonIgnore]
        public string Request {
            get => Get(RequestKey);
            set => Put(RequestKey, value);
        }

        /// <summary>
        /// Create a new, empty JsonRequest
        /// </summary>
        [JsonConstructor]
        public JsonRequest() { }
        
        /// <summary>
        /// Create a new JsonRequest using a set of Json data
        /// </summary>
        /// <param name="genericJsonData">The <see cref="GenericJsonData"/> that describes the request</param>
        /// <exception cref="ArgumentNullException">Thrown when no data is provided</exception>
        /// <exception cref="KeyNotFoundException">Thrown when no <see cref="Request"/> type is defined in the data</exception>
        public JsonRequest(GenericJsonData genericJsonData) {
            if (genericJsonData == null) {
                throw new ArgumentNullException(nameof(genericJsonData));
            }
            if (!genericJsonData.Contains(RequestKey)) {
                throw new KeyNotFoundException($"{RequestKey} key not found in data");
            }

            Data = genericJsonData.Data;
        }

    }

}