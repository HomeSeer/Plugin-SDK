using HomeSeer.Jui.Views;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSeer.PluginSdk.Events {

    [TestFixture(
        TestOf = typeof(ActionTypeCollection),
        Description = "Tests of the ActionTypeCollection class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class ActionTypeCollectionTests {

        private static readonly Randomizer RANDOMIZER = Randomizer.CreateRandomizer();

        private static IEnumerable<bool> ValidBoolTestCaseSource() {
            yield return false;
            yield return true;
        }

        private static IEnumerable<AbstractActionType> ValidActionTypeTestCaseSource() {
            yield return new TestActionType();
        }
        
        private static IEnumerable<AbstractActionType> InvalidActionTypeTestCaseSource() {
            yield return null;
        }
        
        private static IEnumerable<TrigActInfo> ValidTrigActInfoTestCaseSource() {
            var evRef = RANDOMIZER.NextShort();
            var uId = RANDOMIZER.NextShort();
            var info = new TrigActInfo {
                evRef = evRef,
                UID = uId,
                TANumber = 1,
                SubTANumber = 0,
                DataIn = Encoding.UTF8.GetBytes(
                    PageFactory.CreateEventActionPage(
                        $"{evRef}-{uId}",
                        TestTriggerType.TRIGGER_NAME).Page.ToJsonString()),
                /*Descriptor = new TrigActSupportInfo {
                    ComponentType = EEventComponentType.Action
                }*/
            };
            yield return info;
        }

        [Test]
        [Description("Get LogDebug after creating a new ActionTypeCollection and expect the default value to be returned.")]
        [Author("JLW")]
        public void LogDebug_Get_ReturnsDefault() {
            var actionTypeCollection = new ActionTypeCollection(null);
            Assert.AreEqual(false, actionTypeCollection.LogDebug);
        }

        [TestCaseSource(nameof(ValidBoolTestCaseSource))]
        [Description("Get LogDebug after creating a new ActionTypeCollection and expect the default value to be returned.")]
        [Author("JLW")]
        public void LogDebug_Set_SetsLogDebug(bool logDebug) {
            var actionTypeCollection = new ActionTypeCollection(null) {
                LogDebug = logDebug
            };
            Assert.AreEqual(logDebug, actionTypeCollection.LogDebug);
        }
        
        [Test]
        [Description("Create a new instance of ActionTypeCollection and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_NullListener_DoesNotThrow() {
            Assert.DoesNotThrow(() => _ = new ActionTypeCollection(null));
        }
        
        [TestCaseSource(nameof(ValidActionTypeTestCaseSource))]
        [Description("Call AddActionType with a valid AbstractActionType and expect no exception to be thrown.")]
        [Author("JLW")]
        public void AddActionType_Valid_DoesNotThrow(AbstractActionType actionType) {
            var actionTypeCollection = new ActionTypeCollection(null);
            Assert.DoesNotThrow(() => actionTypeCollection.AddActionType(actionType.GetType()));
        }

        [TestCaseSource(nameof(InvalidActionTypeTestCaseSource))]
        [Description("Call AddActionType with an invalid AbstractActionType and expect an exception to be thrown.")]
        [Author("JLW")]
        public void AddActionType_Invalid_Throws(AbstractActionType actionType) {
            var actionTypeCollection = new ActionTypeCollection(null);
            Assert.Throws<ArgumentException>(() => actionTypeCollection.AddActionType(actionType?.GetType()));
        }
        
        [TestCaseSource(nameof(ValidActionTypeTestCaseSource))]
        [Description("Call GetName with a valid index and expect the right name to be returned.")]
        [Author("JLW")]
        public void GetName_Valid_ReturnsName(AbstractActionType actionType) {
            var actionTypeCollection = new ActionTypeCollection(null);
            actionTypeCollection.AddActionType(actionType.GetType());
            Assert.AreEqual(actionType.Name, actionTypeCollection.GetName(1));
        }

        [Test]
        [Description("Call GetName with an invalid index and expect error text to be returned.")]
        [Author("JLW")]
        public void GetName_Invalid_ReturnsError() {
            var actionTypeCollection = new ActionTypeCollection(null);
            Assert.AreEqual("Error retrieving action name", actionTypeCollection.GetName(1));
        }
        
        [TestCaseSource(nameof(ValidTrigActInfoTestCaseSource))]
        [Description("Call OnGetActionUi with a valid TrigActInfo and expect a string that is not empty to be returned.")]
        [Author("JLW")]
        public void OnGetActionUi_Valid_IsNotEmpty(TrigActInfo info) {
            var actionTypeCollection = new ActionTypeCollection(null);
            actionTypeCollection.AddActionType(typeof(TestActionType));
            Assert.IsNotEmpty(actionTypeCollection.OnGetActionUi(info));
        }
        
        //TODO OnUpdateActionConfig
        
        [TestCaseSource(nameof(ValidTrigActInfoTestCaseSource))]
        [Description("Call IsActionConfigured with a valid TrigActInfo and expect it to return without any exceptions.")]
        [Author("JLW")]
        public void IsActionConfigured_Valid_Returns(TrigActInfo info) {
            var actionTypeCollection = new ActionTypeCollection(null);
            actionTypeCollection.AddActionType(typeof(TestActionType));
            _ = actionTypeCollection.IsActionConfigured(info);
        }
        
        [TestCaseSource(nameof(ValidTrigActInfoTestCaseSource))]
        [Description("Call OnGetActionPrettyString with a valid TrigActInfo and expect a string that is not empty to be returned.")]
        [Author("JLW")]
        public void OnGetActionPrettyString_Valid_IsNotEmpty(TrigActInfo info) {
            var actionTypeCollection = new ActionTypeCollection(null);
            actionTypeCollection.AddActionType(typeof(TestActionType));
            Assert.IsNotEmpty(actionTypeCollection.OnGetActionPrettyString(info));
        }
        
        [TestCaseSource(nameof(ValidTrigActInfoTestCaseSource))]
        [Description("Call HandleAction with a valid TrigActInfo and expect it to return without any exceptions.")]
        [Author("JLW")]
        public void HandleAction_Valid_Returns(TrigActInfo info) {
            var actionTypeCollection = new ActionTypeCollection(null);
            actionTypeCollection.AddActionType(typeof(TestActionType));
            _ = actionTypeCollection.HandleAction(info);
        }
        
        [TestCaseSource(nameof(ValidTrigActInfoTestCaseSource))]
        [Description("Call ActionReferencesDeviceOrFeature with a valid TrigActInfo and expect it to return without any exceptions.")]
        [Author("JLW")]
        public void ActionReferencesDeviceOrFeature_Valid_Returns(TrigActInfo info) {
            var actionTypeCollection = new ActionTypeCollection(null);
            actionTypeCollection.AddActionType(typeof(TestActionType));
            _ = actionTypeCollection.ActionReferencesDeviceOrFeature(RANDOMIZER.NextShort(), info);
        }

    }

}