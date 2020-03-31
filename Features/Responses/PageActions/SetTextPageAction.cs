using System;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// A <see cref="FeaturePageAction"/> used to instruct the client to set the text for an HTML element
    /// </summary>
    /// <remarks>Uses the <a href="https://www.w3schools.com/jquery/html_text.asp">jQuery text() method</a></remarks>
    [JsonObject]
    public class SetTextPageAction : FeaturePageAction {

        /// <summary>
        /// Create a new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.SetText"/> action type
        /// </summary>
        [JsonConstructor]
        public SetTextPageAction() {
            PageAction = EPageAction.SetText.GetKey();
        }

        /// <summary>
        /// Create new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.SetText"/> action type using a specific JQuery selector
        /// </summary>
        /// <param name="selector">The JQuery selector to use</param>
        /// <param name="text">The text to set on the element</param>
        /// <exception cref="ArgumentNullException">Thrown if no selector is specified</exception>
        public SetTextPageAction(string selector, string text) {
            if (string.IsNullOrWhiteSpace(selector)) {
                throw new ArgumentNullException(nameof(selector));
            }

            PageAction = EPageAction.SetText.GetKey();
            Selector = selector;
            Data = text ?? "";
        }
    }
}