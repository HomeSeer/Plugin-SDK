using System;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// A <see cref="FeaturePageAction"/> used to instruct the client to HIDE a specific HTML element
    /// </summary>
    /// <remarks>Uses the <a href="https://www.w3schools.com/jquery/jquery_hide_show.asp">jQuery hide() method</a></remarks>
    [JsonObject]
    public class HidePageAction : FeaturePageAction {

        /// <summary>
        /// Create a new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.Hide"/> action type
        /// </summary>
        [JsonConstructor]
        public HidePageAction() {
            PageAction = EPageAction.Hide.GetKey();
        }

        /// <summary>
        /// Create new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.Hide"/> action type using a specific JQuery selector
        /// </summary>
        /// <param name="selector">The JQuery selector to use</param>
        /// <exception cref="ArgumentNullException">Thrown if no selector is specified</exception>
        public HidePageAction(string selector) {
            if (string.IsNullOrWhiteSpace(selector)) {
                throw new ArgumentNullException(nameof(selector));
            }

            PageAction = EPageAction.Hide.GetKey();
            Selector = selector;
        }
    }
}