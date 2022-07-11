using HomeSeer.Jui.Views;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSeer.PluginSdk.Events {

    [TestFixture(
        TestOf = typeof(TriggerTypeCollection),
        Description = "Tests of the TriggerTypeCollection class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class TriggerTypeCollectionTests {

        private static readonly Randomizer RANDOMIZER = Randomizer.CreateRandomizer();
        
        private static IEnumerable<bool> ValidBoolTestCaseSource() {
            yield return false;
            yield return true;
        }
        
        private static IEnumerable<AbstractTriggerType> ValidTriggerTypeTestCaseSource() {
            yield return new TestTriggerType();
        }
        
        private static IEnumerable<Type> InvalidTriggerTypeTestCaseSource() {
            yield return null;
            yield return typeof(TestActionType);
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
                    PageFactory.CreateEventTriggerPage(
                        $"{evRef}-{uId}",
                        TestTriggerType.TRIGGER_NAME).Page.ToJsonString()),
                /*Descriptor = new TrigActSupportInfo {
                    ComponentType = EEventComponentType.Trigger
                }*/
            };
            yield return info;
            info = new TrigActInfo {
                evRef = evRef,
                UID = uId,
                TANumber = 1,
                SubTANumber = 0,
                DataIn = Encoding.UTF8.GetBytes(
                    PageFactory.CreateEventTriggerPage(
                        $"{evRef}-{uId}",
                        TestTriggerType.TRIGGER_NAME).Page.ToJsonString()),
                /*Descriptor = new TrigActSupportInfo {
                    ComponentType = EEventComponentType.Condition
                }*/
            };
            yield return info;
        }

        private static IEnumerable<object[]> ValidSubTriggerCountTestCaseSource() {
            yield return new object[] {
                new List<Type> {
                    typeof(TestTriggerType)
                },
                1,
                0
            };
            yield return new object[] {
                new List<Type>(),
                1,
                0
            };
        }
        
        private static IEnumerable<object[]> ValidSubTriggerNameTestCaseSource() {
            yield return new object[] {
                new List<Type> {
                    typeof(TestTriggerType)
                },
                1,
                0,
                "No sub-trigger type for that index"
            };
        }

        private static IEnumerable<object[]> ValidCanBeConditionTestCaseSource() {
            yield return new object[] {
                new List<Type> {
                    typeof(TestTriggerType)
                },
                1,
                false
            };
        }

        [Test]
        [Description("Get LogDebug after creating a new TriggerTypeCollection and expect the default value to be returned.")]
        [Author("JLW")]
        public void LogDebug_Get_ReturnsDefault() {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            Assert.AreEqual(false, triggerTypeCollection.LogDebug);
        }

        [TestCaseSource(nameof(ValidBoolTestCaseSource))]
        [Description("Get LogDebug after creating a new TriggerTypeCollection and expect the default value to be returned.")]
        [Author("JLW")]
        public void LogDebug_Set_SetsLogDebug(bool logDebug) {
            var triggerTypeCollection = new TriggerTypeCollection(null) {
                LogDebug = logDebug
            };
            Assert.AreEqual(logDebug, triggerTypeCollection.LogDebug);
        }
        
        [Test]
        [Description("Create a new instance of TriggerTypeCollection and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_NullListener_DoesNotThrow() {
            Assert.DoesNotThrow(() => _ = new TriggerTypeCollection(null));
        }
        
        [TestCaseSource(nameof(ValidTriggerTypeTestCaseSource))]
        [Description("Call AddTriggerType with a valid TriggerTypeCollection and expect no exception to be thrown.")]
        [Author("JLW")]
        public void AddTriggerType_Valid_DoesNotThrow(AbstractTriggerType triggerType) {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            Assert.DoesNotThrow(() => triggerTypeCollection.AddTriggerType(triggerType.GetType()));
        }

        [TestCaseSource(nameof(InvalidTriggerTypeTestCaseSource))]
        [Description("Call AddTriggerType with an invalid TriggerTypeCollection and expect an exception to be thrown.")]
        [Author("JLW")]
        public void AddTriggerType_Invalid_Throws(Type triggerType) {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            Assert.Throws<ArgumentException>(() => triggerTypeCollection.AddTriggerType(triggerType?.GetType()));
        }
        
        [TestCaseSource(nameof(ValidTriggerTypeTestCaseSource))]
        [Description("Call GetName with a valid index and expect the right name to be returned.")]
        [Author("JLW")]
        public void GetName_Valid_ReturnsName(AbstractTriggerType triggerType) {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            triggerTypeCollection.AddTriggerType(triggerType.GetType());
            Assert.AreEqual(triggerType.Name, triggerTypeCollection.GetName(1));
        }

        [Test]
        [Description("Call GetName with an invalid index and expect error text to be returned.")]
        [Author("JLW")]
        public void GetName_Invalid_ReturnsError() {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            Assert.AreEqual("Error retrieving trigger name", triggerTypeCollection.GetName(1));
        }
        
        [TestCaseSource(nameof(ValidTrigActInfoTestCaseSource))]
        [Description("Call OnGetTriggerUi with a valid TrigActInfo and expect a string that is not empty to be returned.")]
        [Author("JLW")]
        public void OnGetActionUi_Valid_IsNotEmpty(TrigActInfo info) {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            triggerTypeCollection.AddTriggerType(typeof(TestTriggerType));
            Assert.IsNotEmpty(triggerTypeCollection.OnGetTriggerUi(info));
        }
        
        [TestCaseSource(nameof(ValidSubTriggerCountTestCaseSource))]
        [Description("Call GetSubTriggerCount with valid parameters and expect the correct sub-trigger count to be returned.")]
        [Author("JLW")]
        public void GetSubTriggerCount_Valid_ReturnsExpectedCount(IEnumerable<Type> types, int typeIndex, int expectedCount) {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            foreach (Type type in types) {
                triggerTypeCollection.AddTriggerType(type);
            }
            Assert.AreEqual(expectedCount, triggerTypeCollection.GetSubTriggerCount(typeIndex));
        }
        
        [TestCaseSource(nameof(ValidSubTriggerNameTestCaseSource))]
        [Description("Call GetSubTriggerName with valid parameters and expect the correct string to be returned.")]
        [Author("JLW")]
        public void GetSubTriggerName_Valid_ReturnsExpectedName(IEnumerable<Type> types, int typeIndex, int subTypeIndex, string expectedName) {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            foreach (Type type in types) {
                triggerTypeCollection.AddTriggerType(type);
            }
            Assert.AreEqual(expectedName, triggerTypeCollection.GetSubTriggerName(typeIndex, subTypeIndex));
        }
        
        //TODO : Test OnUpdateTriggerConfig
        
        [TestCaseSource(nameof(ValidTrigActInfoTestCaseSource))]
        [Description("Call IsTriggerConfigured with a valid TrigActInfo and expect it to return without any exceptions.")]
        [Author("JLW")]
        public void IsTriggerConfigured_Valid_Returns(TrigActInfo info) {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            triggerTypeCollection.AddTriggerType(typeof(TestTriggerType));
            _ = triggerTypeCollection.IsTriggerConfigured(info);
        }
        
        [TestCaseSource(nameof(ValidTrigActInfoTestCaseSource))]
        [Description("Call OnGetTriggerPrettyString with a valid TrigActInfo and expect it to return a string that is not empty.")]
        [Author("JLW")]
        public void OnGetTriggerPrettyString_Valid_IsNotEmpty(TrigActInfo info) {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            triggerTypeCollection.AddTriggerType(typeof(TestTriggerType));
            Assert.IsNotEmpty(triggerTypeCollection.OnGetTriggerPrettyString(info));
        }
        
        [TestCaseSource(nameof(ValidTrigActInfoTestCaseSource))]
        [Description("Call IsTriggerTrue with a valid TrigActInfo and expect it to return without any exceptions.")]
        [Author("JLW")]
        public void IsTriggerTrue_Valid_Returns(TrigActInfo info) {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            triggerTypeCollection.AddTriggerType(typeof(TestTriggerType));
            _ = triggerTypeCollection.IsTriggerTrue(info, RANDOMIZER.NextBool());
        }
        
        [TestCaseSource(nameof(ValidTrigActInfoTestCaseSource))]
        [Description("Call TriggerReferencesDeviceOrFeature with a valid TrigActInfo and expect it to return without any exceptions.")]
        [Author("JLW")]
        public void TriggerReferencesDeviceOrFeature_Valid_Returns(TrigActInfo info) {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            triggerTypeCollection.AddTriggerType(typeof(TestTriggerType));
            _ = triggerTypeCollection.TriggerReferencesDeviceOrFeature(RANDOMIZER.NextShort(), info);
        }
        
        [TestCaseSource(nameof(ValidCanBeConditionTestCaseSource))]
        [Description("Call TriggerCanBeCondition with a valid TrigActInfo and expect it to return the correct result.")]
        [Author("JLW")]
        public void TriggerCanBeCondition_Valid_ReturnsExpectedResult(IEnumerable<Type> types, int typeIndex, bool expectedResult) {
            var triggerTypeCollection = new TriggerTypeCollection(null);
            foreach (Type type in types) {
                triggerTypeCollection.AddTriggerType(type);
            }
            Assert.AreEqual(expectedResult, triggerTypeCollection.TriggerCanBeCondition(typeIndex));
        }

    }

}