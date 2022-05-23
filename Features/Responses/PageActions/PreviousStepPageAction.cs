using System;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// A <see cref="FeaturePageAction"/> used to instruct the client to return to the previous stepper step
    /// </summary>
    /// <remarks>Uses the <a href="https://mdbootstrap.com/docs/jquery/components/stepper">mdbootstrap stepper prevStep() method</a></remarks>
    [JsonObject]
    public class PreviousStepPageAction : FeaturePageAction {

        /// <summary>
        /// Create a new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.PreviousStep"/> action type
        /// </summary>
        [JsonConstructor]
        public PreviousStepPageAction() {
            PageAction = EPageAction.PreviousStep.GetKey();
        }

        /// <summary>
        /// Create new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.PreviousStep"/> action type using a specific JQuery selector
        /// </summary>
        /// <param name="selector">The JQuery selector to use</param>
        /// <exception cref="ArgumentNullException">Thrown if no selector is specified</exception>
        public PreviousStepPageAction(string selector) {
            if (string.IsNullOrWhiteSpace(selector)) {
                throw new ArgumentNullException(nameof(selector));
            }

            PageAction = EPageAction.PreviousStep.GetKey();
            Selector = selector;
        }
    }
}