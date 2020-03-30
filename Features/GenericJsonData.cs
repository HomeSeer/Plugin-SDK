using System;
using System.Collections.Generic;
using HomeSeer.PluginSdk.Features.Responses;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features {

    /// <summary>
    /// Generic data collection used as a JSON object. Takes the form of {"data":[]}.
    /// See <see cref="JsonRequest"/>, <see cref="JsonResponse"/>, and <see cref="JsonError"/> for basic implementation
    /// </summary>
    [Serializable, JsonObject]
    public class GenericJsonData {

        /// <summary>
        /// A collection of data keys and values
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Create a new GenericJsonData object
        /// </summary>
        [JsonConstructor]
        public GenericJsonData() { }

        /// <summary>
        /// Create a new GenericJsonData object with data
        /// </summary>
        /// <param name="data"></param>
        public GenericJsonData(Dictionary<string, string> data) {
            Data = data;
        }

        /// <summary>
        /// Deserialize a GenericJsonData object from JSON
        /// </summary>
        /// <param name="json">JSON string containing a serialized GenericJsonData</param>
        /// <returns>A GenericJsonData object</returns>
        public static GenericJsonData FromJson(string json) {
            if (string.IsNullOrWhiteSpace(json)) {
                return new GenericJsonData();
            }
            
            return JsonConvert.DeserializeObject<GenericJsonData>(json);
        }

        /// <summary>
        /// Determine if a key exists in the data collection
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <returns>TRUE if the key exists in the collection, FALSE if it does not</returns>
        public bool Contains(string key) {
            if (Data == null) {
                Data = new Dictionary<string, string>();
                return false;
            }

            return Data.ContainsKey(key);
        }

        /// <summary>
        /// Get the data value for a key
        /// </summary>
        /// <param name="key">The key for the data to get</param>
        /// <returns>The data value corresponding to the key</returns>
        public string Get(string key) {
            if (Data == null) {
                Data = new Dictionary<string, string>();
            }

            return Data[key];
        }

        /// <summary>
        /// Put a value in the data collection
        /// </summary>
        /// <param name="key">The key to map the data with</param>
        /// <param name="value">The data value to save</param>
        public void Put(string key, string value) {
            if (Data == null) {
                Data = new Dictionary<string, string>();
            }

            if (Data.ContainsKey(key)) {
                Data[key] = value;
            }
            else {
                Data.Add(key, value);
            }
        }

        /// <summary>
        /// Remove the value associated with a key from the data
        /// </summary>
        /// <param name="key">The key to remove from the data</param>
        public void Remove(string key) {
            if (Data == null) {
                Data = new Dictionary<string, string>();
                return;
            }

            if (!Data.ContainsKey(key)) {
                return;
            }
            
            Data.Remove(key);
        }

        /// <summary>
        /// Remove all data from the collection
        /// </summary>
        public void Clear() {
            Data = new Dictionary<string, string>();
        }

        /// <summary>
        /// Serialize the data to a JSON string
        /// </summary>
        /// <returns>A JSON string</returns>
        public string ToJson() {
            if (Data == null) {
                Data = new Dictionary<string, string>();
            }

            return JsonConvert.SerializeObject(this);
        }
    }

}