using System;
using System.Collections.Generic;

namespace HomeSeer.PluginSdk.Features {

    /// <summary>
    /// An abstract implementation of the <see cref="IFeaturePage"/> interface establishing a basic structure to build
    ///  a feature page from.
    /// </summary>
    public abstract class AbstractFeaturePage : IFeaturePage {

        private const string RequestKeyIsAlive = "isalive";

        /// <inheritdoc cref="IFeaturePage.Title"/>
        public abstract string Title { get; set; }
        
        /// <inheritdoc cref="IFeaturePage.FileName"/>
        public abstract string FileName { get; set; }
        
        /// <summary>
        /// A map of keys and methods used as callbacks for requests that match their key.
        ///  Use <see cref="RegisterRequestCallback"/> and <see cref="UnregisterRequestCallback"/>.
        /// </summary>
        protected Dictionary<string, Func<FeatureJsonRequest, FeatureJsonResponse>> RequestMap { get; set; } = new Dictionary<string, Func<FeatureJsonRequest, FeatureJsonResponse>>();

        /// <summary>
        /// Create a new instance of an <see cref="AbstractFeaturePage"/>.
        ///  Register the <see cref="IsAlive"/> method for feature page access validation.
        /// </summary>
        protected AbstractFeaturePage() {
            RegisterRequestCallback(RequestKeyIsAlive, IsAlive);
        }

        /// <inheritdoc cref="IFeaturePage.GetHtmlFragment"/>
        public abstract string GetHtmlFragment(string fragmentId);

        /// <inheritdoc cref="IFeaturePage.PostBackProc"/>
        public virtual string PostBackProc(string data, string user, int userRights) {
            //TODO user
            //TODO userRights
            if (string.IsNullOrWhiteSpace(data)) {
                return FeatureJsonError.CreateJson($"{Title} - POST error : data is null");
            }

            FeatureJsonRequest request;
            try {
                var featureJsonData = GenericJsonData.FromJson(data);
                request = new FeatureJsonRequest(featureJsonData);
            }
            catch (Exception exception) {
                //TODO better error catching
                Console.WriteLine(exception);
                return FeatureJsonError.CreateJson($"{Title} - POST error : {exception.Message}");
            }

            if (RequestMap == null) {
                RequestMap = new Dictionary<string, Func<FeatureJsonRequest, FeatureJsonResponse>>();
            }

            if (!RequestMap.ContainsKey(request.Request)) {
                return FeatureJsonError.CreateJson($"{Title} - POST error : no request callback registered for the key {request.Request}");
            }
            
            try {
                var requestCallback = RequestMap[request.Request];
                var response = requestCallback.Invoke(request);
                return response.ToJson();
            }
            catch (Exception exception) {
                //TODO better error catching
                Console.WriteLine(exception);
                return FeatureJsonError.CreateJson($"{Title} - POST error : {exception.Message}");
            }
        }

        /// <summary>
        /// Register a callback method to be invoked when a POST request is submitted using a specific key.
        /// </summary>
        /// <param name="requestKey">The key to match the callback method to</param>
        /// <param name="callback">
        /// A method that takes a <see cref="FeatureJsonRequest"/> as a single parameter and returns a <see cref="FeatureJsonResponse"/>
        /// </param>
        /// <remarks>
        /// Calling this successive times with the same key will replace any existing callback method tied to that key.
        /// </remarks>
        protected void RegisterRequestCallback(string requestKey, Func<FeatureJsonRequest, FeatureJsonResponse> callback) {
            //TODO allow for registration based on method name alone
            if (RequestMap == null) {
                RequestMap = new Dictionary<string, Func<FeatureJsonRequest, FeatureJsonResponse>>();
            }

            if (RequestMap.ContainsKey(requestKey)) {
                RequestMap[requestKey] = callback;
            }
            else {
                RequestMap.Add(requestKey, callback);
            }
        }

        /// <summary>
        /// Unregister an existing callback method so it will not longer be executed when a request is submitted using a particular key.
        /// </summary>
        /// <param name="requestKey">The key to stop responding to</param>
        protected void UnregisterRequestCallback(string requestKey) {
            if (RequestMap == null) {
                RequestMap = new Dictionary<string, Func<FeatureJsonRequest, FeatureJsonResponse>>();
                return;
            }

            if (!RequestMap.ContainsKey(requestKey)) {
                return;
            }

            RequestMap.Remove(requestKey);
        }

        /// <summary>
        /// A sample callback method that can be used to validate that a page is up and responding to POSTs
        /// </summary>
        /// <param name="request">The request data submitted in the POST</param>
        /// <returns>Response message of "Title is alive"</returns>
        private FeatureJsonResponse IsAlive(FeatureJsonRequest request) {
            var response = new FeatureJsonResponse();
            response.Response = $"{Title} is alive";
            return response;
        }
        
    }

}