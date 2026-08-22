using HomeSeer.Jui.Views;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Text;

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// Tests of the way a trigger learns whether it is being used as a condition - the "IF" or "AND IF" part of
    ///  an event - rather than as a trigger - the "WHEN" or "OR WHEN" part.
    /// </summary>
    [TestFixture(
        TestOf = typeof(AbstractTriggerType),
        Description = "Tests that AbstractTriggerType.IsCondition reflects the role HomeSeer gave the trigger.",
        Author = "spud")]
    public class TriggerConditionTests {

        private static readonly Randomizer RANDOMIZER = Randomizer.CreateRandomizer();

        /// <summary>
        /// A trigger that records the value of <see cref="AbstractTriggerType.IsCondition"/> at the moments that
        ///  matter, so the tests can prove when the flag becomes readable.
        /// </summary>
        public class ConditionAwareTriggerType : AbstractTriggerType {

            public const string TRIGGER_NAME = "Condition Aware Trigger";

            /// <summary>
            /// The value of <see cref="AbstractTriggerType.IsCondition"/> while the config page was being built.
            /// </summary>
            public static bool WasConditionWhenBuilt { get; set; }

            /// <summary>
            /// The value of <see cref="AbstractTriggerType.IsCondition"/> while the trigger was being evaluated.
            /// </summary>
            public static bool WasConditionWhenEvaluated { get; set; }

            public ConditionAwareTriggerType(int id, int eventRef, int selectedSubTriggerIndex, byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug = false) : base(id, eventRef, selectedSubTriggerIndex, dataIn, listener, logDebug) { }
            public ConditionAwareTriggerType(int id, int eventRef, int selectedSubTriggerIndex, byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener) : base(id, eventRef, selectedSubTriggerIndex, dataIn, listener) { }
            public ConditionAwareTriggerType() { }

            public override bool CanBeCondition => true;

            protected override string GetName() {
                return TRIGGER_NAME;
            }

            protected override void OnNewTrigger() {
                WasConditionWhenBuilt = IsCondition;
                ConfigPage = PageFactory.CreateEventTriggerPage(PageId, GetName()).Page;
            }

            public override bool IsFullyConfigured() {
                return true;
            }

            protected override bool OnConfigItemUpdate(AbstractView configViewChange) {
                return true;
            }

            public override string GetPrettyString() {
                return IsCondition ? "and if something" : "when something";
            }

            public override bool IsTriggerTrue(bool isCondition) {
                WasConditionWhenEvaluated = IsCondition;
                return true;
            }

            public override bool ReferencesDeviceOrFeature(int devOrFeatRef) {
                return false;
            }

        }

        /// <summary>
        /// The collection passes the listener to every trigger it creates and rejects a null one.
        /// </summary>
        private class DummyListener : TriggerTypeCollection.ITriggerTypeListener { }

        private TriggerTypeCollection _collection;

        [SetUp]
        public void InitCollection() {
            _collection = new TriggerTypeCollection(new DummyListener());
            _collection.AddTriggerType(typeof(ConditionAwareTriggerType));
            ConditionAwareTriggerType.WasConditionWhenBuilt = false;
            ConditionAwareTriggerType.WasConditionWhenEvaluated = false;
        }

        private static TrigActInfo NewTrigActInfo(bool isCondition) {
            return new TrigActInfo {
                evRef = RANDOMIZER.NextShort(),
                UID = RANDOMIZER.NextShort(),
                TANumber = 1,
                SubTANumber = 0,
                DataIn = null,
                IsCondition = isCondition
            };
        }

        [Test]
        [Description("Create a TrigActInfo and expect IsCondition to default to false, so anything HomeSeer does " +
                     "not flag is read as a trigger.")]
        public void TrigActInfo_IsCondition_DefaultsToFalse() {
            Assert.IsFalse(new TrigActInfo().IsCondition);
        }

        [Test]
        [Description("Create a trigger from a TrigActInfo and expect IsCondition to match it.")]
        public void GetObjectFromTrigInfo_IsCondition_MatchesTrigActInfo([Values(true, false)] bool isCondition) {
            var trigger = _collection.GetObjectFromTrigInfo(NewTrigActInfo(isCondition));

            Assert.AreEqual(isCondition, trigger.IsCondition);
        }

        [Test]
        [Description("Create a trigger from a TrigActInfo and expect IsCondition to already be readable while " +
                     "the config page is being built, because that is when a trigger decides how to render.")]
        public void GetObjectFromTrigInfo_IsCondition_IsSetBeforeTheConfigPageIsBuilt([Values(true, false)] bool isCondition) {
            _collection.GetObjectFromTrigInfo(NewTrigActInfo(isCondition));

            Assert.AreEqual(isCondition, ConditionAwareTriggerType.WasConditionWhenBuilt);
        }

        [Test]
        [Description("Create a condition and then a trigger on the same thread, and expect the second one to be " +
                     "built as a trigger - the role of the first must not leak into it.")]
        public void GetObjectFromTrigInfo_IsCondition_DoesNotLeakToTheNextTrigger() {
            _collection.GetObjectFromTrigInfo(NewTrigActInfo(true));
            var trigger = _collection.GetObjectFromTrigInfo(NewTrigActInfo(false));

            Assert.IsFalse(trigger.IsCondition);
            Assert.IsFalse(ConditionAwareTriggerType.WasConditionWhenBuilt);
        }

        [Test]
        [Description("Evaluate a condition and expect IsCondition to be readable while it runs. HomeSeer only " +
                     "evaluates conditions, so the isCondition argument is of no use and is ignored.")]
        public void IsTriggerTrue_IsCondition_IsReadableWhileEvaluating() {
            _collection.IsTriggerTrue(NewTrigActInfo(true), true);

            Assert.IsTrue(ConditionAwareTriggerType.WasConditionWhenEvaluated);
        }

        [Test]
        [Description("Ask the collection whether the trigger type can be a condition and expect it to say yes " +
                     "without an instance being configured.")]
        public void TriggerCanBeCondition_ReturnsTrue() {
            Assert.IsTrue(_collection.TriggerCanBeCondition(1));
        }

    }

}
