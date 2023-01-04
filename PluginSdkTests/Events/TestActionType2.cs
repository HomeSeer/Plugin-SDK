using HomeSeer.Jui.Views;
using System.Collections.Generic;

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// A test action that can be used to evaluate the functionality of the <see cref="AbstractActionType2"/> class.
    /// </summary>
    public sealed class TestActionType2 : AbstractActionType2 {

        public const string ACTION_NAME = "Test Action 2";
        public string InputId => $"{PageId}-input";
        public const string INPUT_NAME = "Test Action Input";

        public TestActionType2(int id, int subTypeNumber, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener) : base(id, subTypeNumber, eventRef, dataIn, listener) { }
        public TestActionType2(int id, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener, bool logDebug = false) : base(id, eventRef, dataIn, listener, logDebug) { }
        public TestActionType2(int id, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener) : base(id, eventRef, dataIn, listener) { }
        public TestActionType2() { }

        protected override string GetName() {
            return ACTION_NAME;
        }

        protected override void OnInstantiateAction(Dictionary<string, string> viewIdValuePairs) {
            var confPage = PageFactory.CreateEventActionPage(PageId, GetName());
            string inputValue = "";

            if (viewIdValuePairs.ContainsKey(InputId)) {
                inputValue = viewIdValuePairs[InputId];
            }
            confPage.WithInput(InputId, INPUT_NAME, inputValue);
            ConfigPage = confPage.Page;
        }

        public override bool IsFullyConfigured() {
            var inputView = ConfigPage.GetViewById<InputView>(InputId);
            return !string.IsNullOrWhiteSpace(inputView.Value);
        }

        protected override bool OnConfigItemUpdate(AbstractView configViewChange) {
            return true;
        }

        public override string GetPrettyString() {
            var inputView = ConfigPage.GetViewById<InputView>(InputId);
            return inputView.Value;
        }

        public override bool OnRunAction() {
            return IsFullyConfigured();
        }

        public override bool ReferencesDeviceOrFeature(int devOrFeatRef) {
            return false;
        }

    }

}