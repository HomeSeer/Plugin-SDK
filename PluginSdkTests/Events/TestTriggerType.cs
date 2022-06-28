using HomeSeer.Jui.Views;

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// A test trigger that can be used to evaluate the functionality of the <see cref="AbstractTriggerType"/> class.
    /// </summary>
    public class TestTriggerType : AbstractTriggerType {
        
        public const string TRIGGER_NAME = "Test Trigger";
        public string InputId => $"{PageId}-input";
        public const string INPUT_NAME = "Test Trigger Input";

        public TestTriggerType(int id, int eventRef, int selectedSubTriggerIndex, byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug = false) : base(id, eventRef, selectedSubTriggerIndex, dataIn, listener, logDebug) { }
        public TestTriggerType(int id, int eventRef, int selectedSubTriggerIndex, byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener) : base(id, eventRef, selectedSubTriggerIndex, dataIn, listener) { }
        public TestTriggerType(TrigActInfo trigInfo, TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug = false) : base(trigInfo, listener, logDebug) { }
        public TestTriggerType() { }

        protected override string GetName() {
            return TRIGGER_NAME;
        }

        protected override void OnNewTrigger() {
            var confPage = PageFactory.CreateEventTriggerPage(PageId, GetName());
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

        public override bool IsTriggerTrue(bool isCondition) {
            return IsFullyConfigured();
        }

        public override bool ReferencesDeviceOrFeature(int devOrFeatRef) {
            return false;
        }

    }

}