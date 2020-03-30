using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses {

    /// <summary>
    /// A POST error response for a feature page as a collection of key-value pairs 
    /// </summary>
    [JsonObject]
    public class JsonError : GenericJsonData {

        /// <summary>
        /// Key that identifies the error message data
        /// </summary>
        protected const string ErrorKey = "error";
        
        /// <summary>
        /// The error message associated with this response
        /// </summary>
        [JsonIgnore]
        public string Error {
            get => Get(ErrorKey);
            set => Put(ErrorKey, value);
        }
        
        /// <summary>
        /// Create a new, empty JsonError
        /// </summary>
        [JsonConstructor]
        public JsonError() { }

        /// <summary>
        /// Create a new JsonError with a specific message
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        /// <exception cref="ArgumentNullException">Thrown if no error message is provided</exception>
        public JsonError(string errorMessage) {
            if (string.IsNullOrWhiteSpace(errorMessage)) {
                throw new ArgumentNullException(nameof(errorMessage));
            }

            Error = errorMessage;
        }
        
        /// <summary>
        /// Create a new JsonError using a set of data
        /// </summary>
        /// <param name="genericJsonData">The <see cref="GenericJsonData"/> that describes the error</param>
        /// <exception cref="ArgumentNullException">Thrown when no data is provided</exception>
        /// <exception cref="KeyNotFoundException">Thrown when no <see cref="Error"/> message is defined in the data</exception>
        public JsonError(GenericJsonData genericJsonData) {
            if (genericJsonData == null) {
                throw new ArgumentNullException(nameof(genericJsonData));
            }
            if (!genericJsonData.Contains(ErrorKey)) {
                throw new KeyNotFoundException($"{ErrorKey} key not found in data");
            }

            Data = genericJsonData.Data;
        }

        /// <summary>
        /// Generate the JSON necessary to describe a <see cref="JsonError"/> based on a specified message
        /// </summary>
        /// <param name="message">The error message</param>
        /// <returns>A string of JSON describing a <see cref="JsonError"/></returns>
        public static string CreateJson(string message) {
            var jsonError = new JsonError(message);
            return jsonError.ToJson();
        }

    }

}