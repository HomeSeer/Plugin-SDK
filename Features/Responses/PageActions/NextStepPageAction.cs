using System;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// A <see cref="FeaturePageAction"/> used to instruct the client to proceed to the next stepper step
    /// </summary>
    /// <remarks>Uses the <a href="https://mdbootstrap.com/docs/jquery/components/stepper">mdbootstrap stepper nextStep() method</a></remarks>
    [JsonObject]
    public class NextStepPageAction : FeaturePageAction {

        /// <summary>
        /// Create a new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.NextStep"/> action type
        /// </summary>
        [JsonConstructor]
        public NextStepPageAction() {
            PageAction = EPageAction.NextStep.GetKey();
        }

        /// <summary>
        /// Create new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.NextStep"/> action type using a specific JQuery selector
        /// </summary>
        /// <param name="selector">The JQuery selector to use</param>
        /// <exception cref="ArgumentNullException">Thrown if no selector is specified</exception>
        public NextStepPageAction(string selector) {
            if (string.IsNullOrWhiteSpace(selector)) {
                throw new ArgumentNullException(nameof(selector));
            }

            PageAction = EPageAction.NextStep.GetKey();
            Selector = selector;
        }
    }
}