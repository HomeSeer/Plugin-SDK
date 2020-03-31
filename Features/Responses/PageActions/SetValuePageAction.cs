using System;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// A <see cref="FeaturePageAction"/> used to instruct the client to set the value for an HTML element
    /// </summary>
    /// <remarks>Uses the <a href="https://www.w3schools.com/jquery/html_val.asp">jQuery val() method</a></remarks>
    [JsonObject]
    public class SetValuePageAction : FeaturePageAction {

        /// <summary>
        /// Create a new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.SetValue"/> action type
        /// </summary>
        [JsonConstructor]
        public SetValuePageAction() {
            PageAction = EPageAction.SetValue.GetKey();
        }

        /// <summary>
        /// Create new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.SetValue"/> action type using a specific JQuery selector
        /// </summary>
        /// <param name="selector">The JQuery selector to use</param>
        /// <param name="value">The value to set on the element</param>
        /// <exception cref="ArgumentNullException">Thrown if no selector is specified</exception>
        public SetValuePageAction(string selector, string value) {
            if (string.IsNullOrWhiteSpace(selector)) {
                throw new ArgumentNullException(nameof(selector));
            }

            PageAction = EPageAction.SetValue.GetKey();
            Selector = selector;
            Data = value ?? "";
        }
    }
}