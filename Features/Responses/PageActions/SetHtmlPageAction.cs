using System;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// A <see cref="FeaturePageAction"/> used to instruct the client to set the HTML for an HTML element
    /// </summary>
    /// <remarks>Uses the <a href="https://www.w3schools.com/jquery/html_html.asp">jQuery html() method</a></remarks>
    [JsonObject]
    public class SetHtmlPageAction : FeaturePageAction {

        /// <summary>
        /// Create a new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.SetHtml"/> action type
        /// </summary>
        [JsonConstructor]
        public SetHtmlPageAction() {
            PageAction = EPageAction.SetHtml.GetKey();
        }

        /// <summary>
        /// Create new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.SetHtml"/> action type using a specific JQuery selector
        /// </summary>
        /// <param name="selector">The JQuery selector to use</param>
        /// <param name="html">The html to set on the element</param>
        /// <exception cref="ArgumentNullException">Thrown if no selector is specified</exception>
        public SetHtmlPageAction(string selector, string html) {
            if (string.IsNullOrWhiteSpace(selector)) {
                throw new ArgumentNullException(nameof(selector));
            }

            PageAction = EPageAction.SetHtml.GetKey();
            Selector = selector;
            Data = html ?? "";
        }
    }
}