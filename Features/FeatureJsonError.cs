using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features {

    [JsonObject]
    public class FeatureJsonError : GenericJsonData {

        protected const string ErrorKey = "error";
        
        [JsonIgnore]
        public string Error {
            get => Get(ErrorKey);
            set => Put(ErrorKey, value);
        }
        
        public FeatureJsonError() { }

        public FeatureJsonError(string errorMessage) {
            if (string.IsNullOrWhiteSpace(errorMessage)) {
                throw new ArgumentNullException(nameof(errorMessage));
            }

            Error = errorMessage;
        }
        
        public FeatureJsonError(GenericJsonData genericJsonData) {
            if (genericJsonData == null) {
                throw new ArgumentNullException(nameof(genericJsonData));
            }
            if (!genericJsonData.Contains(ErrorKey)) {
                throw new KeyNotFoundException($"{ErrorKey} key not found in data");
            }

            Data = genericJsonData.Data;
        }

        public static string CreateJson(string message) {
            var jsonError = new FeatureJsonError(message);
            return jsonError.ToJson();
        }

    }

}