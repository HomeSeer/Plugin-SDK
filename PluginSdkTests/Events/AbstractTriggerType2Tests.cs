using HomeSeer.Jui.Views;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSeer.PluginSdk.Events {

    [TestFixture(
        TestOf = typeof(AbstractTriggerType2),
        Description = "Tests of the AbstractTriggerType2 class to ensure it behaves as expected under normal conditions.")]
    public class AbstractTriggerType2Tests {

        private static readonly Randomizer RANDOMIZER = Randomizer.CreateRandomizer();

        private static IEnumerable<bool> ValidBoolTestCaseSource() {
            yield return false;
            yield return true;
        }

        private static IEnumerable<object[]> ValidConstructor3ParamTestCaseSource() {
            var byteBuffer = new byte[RANDOMIZER.NextShort(8, 256)];
            RANDOMIZER.NextBytes(byteBuffer);
            var info = new TrigActInfo {
                evRef = RANDOMIZER.NextShort(),
                UID = RANDOMIZER.NextShort(),
                TANumber = RANDOMIZER.NextShort(),
                SubTANumber = RANDOMIZER.NextShort(),
                DataIn = byteBuffer,
                /*Descriptor = new TrigActSupportInfo {
                    ComponentType = EEventComponentType.Trigger
                }*/
            };
            yield return new object[] {
                info,
                null,
                RANDOMIZER.NextBool()
            };
            info = new TrigActInfo {
                evRef = RANDOMIZER.NextShort(),
                UID = RANDOMIZER.NextShort(),
                TANumber = RANDOMIZER.NextShort(),
                SubTANumber = RANDOMIZER.NextShort(),
                DataIn = Encoding.UTF8.GetBytes(
                    PageFactory.CreateEventActionPage(
                        RANDOMIZER.GetString(),
                        RANDOMIZER.GetString()).Page.ToJsonString()),
                /*Descriptor = new TrigActSupportInfo {
                    ComponentType = EEventComponentType.Condition
                }*/
            };
            yield return new object[] {
                info,
                null,
                RANDOMIZER.NextBool()
            };
            info = new TrigActInfo {
                evRef = RANDOMIZER.NextShort(),
                UID = RANDOMIZER.NextShort(),
                TANumber = RANDOMIZER.NextShort(),
                SubTANumber = RANDOMIZER.NextShort(),
                DataIn = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { "viewId", "viewValue" } }, Formatting.None)),
                /*Descriptor = new TrigActSupportInfo {
                    ComponentType = EEventComponentType.Condition
                }*/
            };
            yield return new object[] {
                info,
                null,
                RANDOMIZER.NextBool()
            };
        }

        private static IEnumerable<object[]> ValidConstructor5ParamTestCaseSource() {
            yield return new object[] {
                RANDOMIZER.NextShort(),
                RANDOMIZER.NextShort(),
                RANDOMIZER.NextShort(),
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { "viewId", "viewValue" } }, Formatting.None)),
                null
            };
        }

        private static IEnumerable<object[]> ValidConstructor6ParamTestCaseSource() {
            yield return new object[] {
                RANDOMIZER.NextShort(),
                RANDOMIZER.NextShort(),
                RANDOMIZER.NextShort(),
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { "viewId", "viewValue" } }, Formatting.None)),
                null,
                RANDOMIZER.NextBool()
            };
        }

        private static IEnumerable<object[]> ValidEqualsTestCaseSource() {
            var trigger = new TestTriggerType2();
            yield return new object[] {
                trigger,
                trigger
            };
            var evRef = RANDOMIZER.NextShort();
            var uId = RANDOMIZER.NextShort();
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { "viewId", "viewValue" } }, Formatting.None));
            yield return new object[] {
                new TestTriggerType2(uId, evRef, 0, data, null),
                new TestTriggerType2(uId, evRef, 0, data, null)
            };

        }

        private static IEnumerable<object[]> InvalidEqualsTestCaseSource() {
            yield return new object[] {
                new TestTriggerType2(),
                null
            };
            yield return new object[] {
                new TestTriggerType2(),
                1
            };
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { "viewId", "viewValue" } }, Formatting.None));
            yield return new object[] {
                new TestTriggerType2(1, 2, 0, data, null),
                new TestTriggerType2(2, 2, 0, data, null)
            };
            yield return new object[] {
                new TestTriggerType2(1, 1, 0, data, null),
                new TestTriggerType2(1, 2, 0, data, null)
            };
            //TODO : Fix this - it works for Equals but does not work for GetHashCode
            /*yield return new object[] {
                new TestTriggerType2(1, 2, 0, data, null),
                new TestTriggerType2(1, 2, 0, Array.Empty<byte>(), null)
            };*/
        }

        [Test]
        [Description("Create a new instance of a AbstractTriggerType2 with no parameters and expect no exceptions to be thrown.")]
        public void Constructor_NoParam_Valid_DoesNotThrow() {
            Assert.DoesNotThrow(() => _ = new TestTriggerType2());
        }

        [TestCaseSource(nameof(ValidConstructor3ParamTestCaseSource))]
        [Description("Create a new instance of a AbstractTriggerType2 with 3 parameters and expect no exceptions to be thrown.")]
        public void Constructor_3Param_Valid_DoesNotThrow(TrigActInfo trigInfo,
            TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug) {
            Assert.DoesNotThrow(() => _ = new TestTriggerType2(trigInfo, listener, logDebug));
        }

        [TestCaseSource(nameof(ValidConstructor5ParamTestCaseSource))]
        [Description("Create a new instance of a AbstractTriggerType2 with 5 parameters and expect no exceptions to be thrown.")]
        public void Constructor_5Param_Valid_DoesNotThrow(int id, int eventRef, int selectedSubTriggerIndex,
            byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener) {
            Assert.DoesNotThrow(() => _ = new TestTriggerType2(id, eventRef, selectedSubTriggerIndex, dataIn, listener));
        }

        [TestCaseSource(nameof(ValidConstructor6ParamTestCaseSource))]
        [Description("Create a new instance of a AbstractTriggerType2 with 6 parameters and expect no exceptions to be thrown.")]
        public void Constructor_6Param_Valid_DoesNotThrow(int id, int eventRef, int selectedSubTriggerIndex,
            byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug) {
            Assert.DoesNotThrow(() => _ = new TestTriggerType2(id, eventRef, selectedSubTriggerIndex, dataIn, listener, logDebug));
        }

        [Test]
        [Description("Get LogDebug after creating a new AbstractTriggerType2 and expect the default value to be returned.")]
        public void LogDebug_Get_ReturnsDefault() {
            var testTriggerType2 = new TestTriggerType2();
            Assert.AreEqual(false, testTriggerType2.LogDebug);
        }

        [TestCaseSource(nameof(ValidBoolTestCaseSource))]
        [Description("Get LogDebug after creating a new AbstractTriggerType2 and expect the default value to be returned.")]
        public void LogDebug_Set_SetsLogDebug(bool logDebug) {
            var testTriggerType2 = new TestTriggerType2 {
                LogDebug = logDebug
            };
            Assert.AreEqual(logDebug, testTriggerType2.LogDebug);
        }

        [Test]
        [Description("Get TriggerListener after creating a new AbstractTriggerType2 and expect the default value to be returned.")]
        public void TriggerListener_Get_ReturnsDefault() {
            var testTriggerType2 = new TestTriggerType2();
            Assert.AreEqual(null, testTriggerType2.TriggerListener);
        }

        [Test]
        [Description("Get Id after creating a new AbstractTriggerType2 and expect the default value to be returned.")]
        public void Id_Get_ReturnsDefault() {
            var testTriggerType2 = new TestTriggerType2();
            Assert.AreEqual(0, testTriggerType2.Id);
        }

        [Test]
        [Description("Get EventRef after creating a new AbstractTriggerType2 and expect the default value to be returned.")]
        public void EventRef_Get_ReturnsDefault() {
            var testTriggerType2 = new TestTriggerType2();
            Assert.AreEqual(0, testTriggerType2.EventRef);
        }

        [Test]
        [Description("Get Data after creating a new AbstractTriggerType2 and expect the default value to be returned.")]
        public void Data_Get_ReturnsDefault() {
            var testTriggerType2 = new TestTriggerType2(1, 2, 0, null, null);
            var defaultData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { testTriggerType2.InputId, "" } }, Formatting.None));
            Assert.AreEqual(defaultData, testTriggerType2.Data);
        }

        [Test]
        [Description("Get Name after creating a new AbstractTriggerType2 and expect it to not be empty.")]
        public void Name_Get_IsNotEmpty() {
            var testTriggerType2 = new TestTriggerType2();
            Assert.IsNotEmpty(testTriggerType2.Name);
        }

        [Test]
        [Description("Call ToHtml after creating a new AbstractTriggerType2 and expect the return value to not be empty.")]
        public void ToHtml_Get_IsNotEmpty() {
            var testTriggerType2 = new TestTriggerType2();
            Assert.IsNotEmpty(testTriggerType2.ToHtml());
        }

        [Test]
        [Description("Get CanBeCondition after creating a new AbstractTriggerType2 and expect it to not be empty.")]
        public void CanBeCondition_Get_ReturnsDefault() {
            var testTriggerType2 = new TestTriggerType2();
            Assert.AreEqual(false, testTriggerType2.CanBeCondition);
        }

        [Test]
        [Description("Get SubTriggerCount after creating a new AbstractTriggerType2 and expect it to not be empty.")]
        public void SubTriggerCount_Get_ReturnsDefault() {
            var testTriggerType2 = new TestTriggerType2();
            Assert.AreEqual(0, testTriggerType2.SubTriggerCount);
        }

        /*[Test]
        [Description("Get IsCondition after creating a new AbstractTriggerType2 and expect it to be false.")]
        public void IsCondition_Get_ReturnsDefault() {
            var testTriggerType2 = new TestTriggerType2();
            Assert.AreEqual(false, testTriggerType2.IsCondition);
        }
        
        [Test]
        [Description("Get IsCondition after creating a new AbstractTriggerType2 as a condition and expect it to be true.")]
        public void IsCondition_Get_WhenCondition_ReturnsTrue() {
            var info = new TrigActInfo {
                evRef = RANDOMIZER.NextShort(),
                UID = RANDOMIZER.NextShort(),
                TANumber = RANDOMIZER.NextShort(),
                SubTANumber = RANDOMIZER.NextShort(),
                DataIn = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { "viewId", "viewValue" } }, Formatting.None)),
                Descriptor = new TrigActSupportInfo {
                    ComponentType = EEventComponentType.Condition
                }
            };
            var testTriggerType2 = new TestTriggerType2(info, null);
            Assert.AreEqual(true, testTriggerType2.IsCondition);
        }*/

        [Test]
        [Description("Call GetSubTriggerName after creating a new AbstractTriggerType2 and expect it to throw an exception.")]
        public void GetSubTriggerName_Get_Default_ThrowsException() {
            var testTriggerType2 = new TestTriggerType2();
            Assert.Throws<ArgumentOutOfRangeException>(() => testTriggerType2.GetSubTriggerName(0));
        }

        [TestCaseSource(nameof(ValidEqualsTestCaseSource))]
        [Description("Compare an object with an instance of AbstractTriggerType2 and expect true to be returned.")]
        public void Equals_Valid_ReturnsTrue(AbstractTriggerType2 trigger, object obj) {
            Assert.IsTrue(trigger.Equals(obj));
        }

        [TestCaseSource(nameof(InvalidEqualsTestCaseSource))]
        [Description("Compare an object with an instance of AbstractTriggerType2 and expect false to be returned.")]
        public void Equals_Invalid_ReturnsFalse(AbstractTriggerType2 trigger, object obj) {
            Assert.IsFalse(trigger.Equals(obj));
        }

        [TestCaseSource(nameof(ValidEqualsTestCaseSource))]
        [Description("Compare the hash code of an instance of AbstractTriggerType2 with another and expect them to be the same.")]
        public void GetHashCode_Valid_IsSame(AbstractTriggerType2 trigger, object obj) {
            Assert.AreEqual(obj.GetHashCode(), trigger.GetHashCode());
        }

        [TestCaseSource(nameof(InvalidEqualsTestCaseSource))]
        [Description("Compare the hash code of an instance of AbstractTriggerType2 with another and expect them to not be the same.")]
        public void GetHashCode_Invalid_IsNotSame(AbstractTriggerType2 trigger, object obj) {
            Assert.AreNotEqual(obj?.GetHashCode(), trigger.GetHashCode());
        }

    }

}