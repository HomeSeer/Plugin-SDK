using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features {

    /// <summary>
    /// A POST request with JSON data from a web client to a feature page as a collection of key-value pairs
    /// </summary>
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