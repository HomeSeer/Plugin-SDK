using System;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// A <see cref="FeaturePageAction"/> used to instruct the client to go to a particular stepper step
    /// </summary>
    /// <remarks>Uses the <a href="https://mdbootstrap.com/docs/jquery/components/stepper">mdbootstrap stepper setStep() method</a></remarks>
    [JsonObject]
    public class SetStepPageAction : FeaturePageAction {

        /// <summary>
        /// Create a new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.SetStep"/> action type
        /// </summary>
        [JsonConstructor]
        public SetStepPageAction() {
            PageAction = EPageAction.SetStep.GetKey();
        }

        /// <summary>
        /// Create new <see cref="FeaturePageAction"/> with the <see cref="EPageAction.SetStep"/> action type using a specific JQuery selector
        /// </summary>
        /// <param name="selector">The JQuery selector to use</param>
        /// <exception cref="ArgumentNullException">Thrown if no selector is specified</exception>
        public SetStepPageAction(string selector) {
            if (string.IsNullOrWhiteSpace(selector)) {
                throw new ArgumentNullException(nameof(selector));
            }

            PageAction = EPageAction.SetStep.GetKey();
            Selector = selector;
        }
    }
}