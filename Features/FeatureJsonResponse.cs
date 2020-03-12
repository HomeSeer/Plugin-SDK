using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features {

    [JsonObject]
    public class FeatureJsonResponse : FeatureJsonRequest {

        protected const string ResponseKey = "response";

        [JsonIgnore]
        public string Response {
            get => Get(ResponseKey);
            set => Put(ResponseKey, value);
        }

        public FeatureJsonResponse() { }
        public FeatureJsonResponse(GenericJsonData genericJsonData) : base(genericJsonData) {
            if (!genericJsonData.Contains(ResponseKey)) {
                throw new KeyNotFoundException($"{ResponseKey} key not found in data");
            }
        }
    }

}