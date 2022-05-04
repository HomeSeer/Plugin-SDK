using HomeSeer.Jui.Views;

namespace HomeSeer.PluginSdk.Events {

    public sealed class TestActionType : AbstractActionType {

        public const string ACTION_NAME = "Test Action";
        public string InputId => $"{PageId}-input";
        public const string INPUT_NAME = "Test Action Input";

        public TestActionType(int id, int subTypeNumber, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener) : base(id, subTypeNumber, eventRef, dataIn, listener) { }
        public TestActionType(int id, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener, bool logDebug = false) : base(id, eventRef, dataIn, listener, logDebug) { }
        public TestActionType(int id, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener) : base(id, eventRef, dataIn, listener) { }
        public TestActionType() { }

        protected override string GetName() {
            return ACTION_NAME;
        }

        protected override void OnNewAction() {
            var confPage = PageFactory.CreateEventActionPage(PageId, GetName());
            confPage.WithInput(InputId, INPUT_NAME);
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