using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features {

    [JsonObject]
    public class FeatureJsonRequest : GenericJsonData {

        protected const string RequestKey = "request";
        
        [JsonIgnore]
        public string Request {
            get => Get(RequestKey);
            set => Put(RequestKey, value);
        }

        public FeatureJsonRequest() { }
        public FeatureJsonRequest(GenericJsonData genericJsonData) {
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