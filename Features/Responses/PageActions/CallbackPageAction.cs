using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// A <see cref="FeaturePageAction"/> used to instruct the client to callback to the plugin after waiting a specified amount of time
    /// </summary>
    /// <remarks>
    /// <para>
    /// Uses the <a href="https://www.w3schools.com/jsref/met_win_settimeout.asp">setTimeout() method</a> to schedule a call
    ///  to the featurePagePost JS method after a specified amount of time.
    /// </para>
    /// <para>
    /// The javascript that handles this is not included in the FeaturePagePost.js file by default and must be included manually.
    ///  Add the following to the end of the switch statement in the onFeaturePagePostSuccess, replacing PUTCALLBACKURLHERE
    ///  with the URL for your page:
    /// <code>
    /// case "callback":
    ///     setTimeout(function() {featurePagePost(pageAction.data, PUTCALLBACKURLHERE)}, pageAction.selector);
    ///     break;
    /// </code>
    /// </para>
    /// </remarks>
    [JsonObject]
    public class CallbackPageAction : FeaturePageAction {

        //TODO timeout separate from the selector so it can be used for the URL
        /*[JsonProperty("timeout")]
        public int Timeout { get; set; }*/

        /// <summary>
        /// Create a new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.Callback"/> action type
        /// </summary>
        [JsonConstructor]
        public CallbackPageAction() {
            PageAction = EPageAction.Callback.GetKey();
        }

        /// <summary>
        /// Create a new instance of a <see cref="FeaturePageAction"/> with the <see cref="EPageAction.Callback"/>
        ///  action type that will send the specified data blob after waiting the specified amount of time
        /// </summary>
        /// <param name="data">The data to include in the next request</param>
        /// <param name="timeout">The amount of time to wait, in millisecond, before making the callback request</param>
        public CallbackPageAction(string data, int timeout) {
            PageAction = EPageAction.Callback.GetKey();
            Selector = timeout.ToString();
            Data = data;
            //Timeout = timeout;
        }

    }
}