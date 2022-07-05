using HomeSeer.Jui.Views;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSeer.PluginSdk.Events {
    
    //TODO add constructor test that uses a ConfigPage that is the wrong type

    [TestFixture(
        TestOf = typeof(AbstractTriggerType),
        Description = "Tests of the AbstractTriggerType class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class AbstractTriggerTypeTests {

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
        }
        
        private static IEnumerable<object[]> ValidConstructor5ParamTestCaseSource() {
            yield return new object[] {
                RANDOMIZER.NextShort(), 
                RANDOMIZER.NextShort(),
                RANDOMIZER.NextShort(),
                Encoding.UTF8.GetBytes(
                    PageFactory.CreateEventTriggerPage(
                        RANDOMIZER.GetString(),
                        RANDOMIZER.GetString()).Page.ToJsonString()),
                null
            };
        }
        
        private static IEnumerable<object[]> ValidConstructor6ParamTestCaseSource() {
            yield return new object[] {
                RANDOMIZER.NextShort(), 
                RANDOMIZER.NextShort(),
                RANDOMIZER.NextShort(),
                Encoding.UTF8.GetBytes(
                    PageFactory.CreateEventTriggerPage(
                        RANDOMIZER.GetString(),
                        RANDOMIZER.GetString()).Page.ToJsonString()),
                null,
                RANDOMIZER.NextBool()
            };
        }
        
        private static IEnumerable<object[]> ValidEqualsTestCaseSource() {
            var trigger = new TestTriggerType();
            yield return new object[] {
                trigger, 
                trigger
            };
            var evRef = RANDOMIZER.NextShort();
            var uId = RANDOMIZER.NextShort();
            var data = Encoding.UTF8.GetBytes(
                PageFactory.CreateEventTriggerPage(
                    $"{evRef}-{uId}",
                    TestTriggerType.TRIGGER_NAME).Page.ToJsonString());
            yield return new object[] {
                new TestTriggerType(uId, evRef, 0, data, null),
                new TestTriggerType(uId, evRef, 0, data, null)
            };
            
        }

        private static IEnumerable<object[]> InvalidEqualsTestCaseSource() {
            yield return new object[] {
                new TestTriggerType(), 
                null
            };
            yield return new object[] {
                new TestTriggerType(), 
                1
            };
            var data = Encoding.UTF8.GetBytes(
                PageFactory.CreateEventTriggerPage(
                    $"1-1",
                    TestTriggerType.TRIGGER_NAME).Page.ToJsonString());
            yield return new object[] {
                new TestTriggerType(1, 2, 0, data, null),
                new TestTriggerType(2, 2, 0, data, null)
            };
            yield return new object[] {
                new TestTriggerType(1, 1, 0, data, null),
                new TestTriggerType(1, 2, 0, data, null)
            };
            //TODO : Fix this - it works for Equals but does not work for GetHashCode
            /*yield return new object[] {
                new TestTriggerType(1, 2, 0, data, null),
                new TestTriggerType(1, 2, 0, Array.Empty<byte>(), null)
            };*/
        }
        
        [Test]
        [Description("Create a new instance of a AbstractTriggerType with no parameters and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_NoParam_Valid_DoesNotThrow() {
            Assert.DoesNotThrow(() => _ = new TestTriggerType());
        }
        
        [TestCaseSource(nameof(ValidConstructor3ParamTestCaseSource))]
        [Description("Create a new instance of a AbstractTriggerType with 3 parameters and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_3Param_Valid_DoesNotThrow(TrigActInfo trigInfo, 
            TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug) {
            Assert.DoesNotThrow(() => _ = new TestTriggerType(trigInfo, listener, logDebug));
        }

        [TestCaseSource(nameof(ValidConstructor5ParamTestCaseSource))]
        [Description("Create a new instance of a AbstractTriggerType with 5 parameters and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_5Param_Valid_DoesNotThrow(int id, int eventRef, int selectedSubTriggerIndex, 
            byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener) {
            Assert.DoesNotThrow(() => _ = new TestTriggerType(id, eventRef, selectedSubTriggerIndex, dataIn, listener));
        }

        [TestCaseSource(nameof(ValidConstructor6ParamTestCaseSource))]
        [Description("Create a new instance of a AbstractTriggerType with 6 parameters and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_6Param_Valid_DoesNotThrow(int id, int eventRef, int selectedSubTriggerIndex,
            byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug) {
            Assert.DoesNotThrow(() => _ = new TestTriggerType(id, eventRef, selectedSubTriggerIndex, dataIn, listener, logDebug));
        }
        
        [Test]
        [Description("Get LogDebug after creating a new AbstractTriggerType and expect the default value to be returned.")]
        [Author("JLW")]
        public void LogDebug_Get_ReturnsDefault() {
            var testTriggerType = new TestTriggerType();
            Assert.AreEqual(false, testTriggerType.LogDebug);
        }

        [TestCaseSource(nameof(ValidBoolTestCaseSource))]
        [Description("Get LogDebug after creating a new AbstractTriggerType and expect the default value to be returned.")]
        [Author("JLW")]
        public void LogDebug_Set_SetsLogDebug(bool logDebug) {
            var testTriggerType = new TestTriggerType {
                LogDebug = logDebug
            };
            Assert.AreEqual(logDebug, testTriggerType.LogDebug);
        }
        
        [Test]
        [Description("Get TriggerListener after creating a new AbstractTriggerType and expect the default value to be returned.")]
        [Author("JLW")]
        public void TriggerListener_Get_ReturnsDefault() {
            var testTriggerType = new TestTriggerType();
            Assert.AreEqual(null, testTriggerType.TriggerListener);
        }
        
        [Test]
        [Description("Get Id after creating a new AbstractTriggerType and expect the default value to be returned.")]
        [Author("JLW")]
        public void Id_Get_ReturnsDefault() {
            var testTriggerType = new TestTriggerType();
            Assert.AreEqual(0, testTriggerType.Id);
        }
        
        [Test]
        [Description("Get EventRef after creating a new AbstractTriggerType and expect the default value to be returned.")]
        [Author("JLW")]
        public void EventRef_Get_ReturnsDefault() {
            var testTriggerType = new TestTriggerType();
            Assert.AreEqual(0, testTriggerType.EventRef);
        }
        
        [Test]
        [Description("Get Data after creating a new AbstractTriggerType and expect the default value to be returned.")]
        [Author("JLW")]
        public void Data_Get_ReturnsDefault() {
            var testTriggerType = new TestTriggerType();
            var defaultData = Encoding.UTF8.GetBytes(PageFactory.CreateEventTriggerPage(
                "0-0", TestTriggerType.TRIGGER_NAME).Page.ToJsonString());
            Assert.AreEqual(defaultData, testTriggerType.Data);
        }
        
        [Test]
        [Description("Get Name after creating a new AbstractTriggerType and expect it to not be empty.")]
        [Author("JLW")]
        public void Name_Get_IsNotEmpty() {
            var testTriggerType = new TestTriggerType();
            Assert.IsNotEmpty(testTriggerType.Name);
        }
        
        [Test]
        [Description("Call ToHtml after creating a new AbstractTriggerType and expect the return value to not be empty.")]
        [Author("JLW")]
        public void ToHtml_Get_IsNotEmpty() {
            var testTriggerType = new TestTriggerType();
            Assert.IsNotEmpty(testTriggerType.ToHtml());
        }
        
        [Test]
        [Description("Get CanBeCondition after creating a new AbstractTriggerType and expect it to not be empty.")]
        [Author("JLW")]
        public void CanBeCondition_Get_ReturnsDefault() {
            var testTriggerType = new TestTriggerType();
            Assert.AreEqual(false, testTriggerType.CanBeCondition);
        }
        
        [Test]
        [Description("Get SubTriggerCount after creating a new AbstractTriggerType and expect it to not be empty.")]
        [Author("JLW")]
        public void SubTriggerCount_Get_ReturnsDefault() {
            var testTriggerType = new TestTriggerType();
            Assert.AreEqual(0, testTriggerType.SubTriggerCount);
        }
        
        /*[Test]
        [Description("Get IsCondition after creating a new AbstractTriggerType and expect it to be false.")]
        [Author("JLW")]
        public void IsCondition_Get_ReturnsDefault() {
            var testTriggerType = new TestTriggerType();
            Assert.AreEqual(false, testTriggerType.IsCondition);
        }
        
        [Test]
        [Description("Get IsCondition after creating a new AbstractTriggerType as a condition and expect it to be true.")]
        [Author("JLW")]
        public void IsCondition_Get_WhenCondition_ReturnsTrue() {
            var info = new TrigActInfo {
                evRef = RANDOMIZER.NextShort(),
                UID = RANDOMIZER.NextShort(),
                TANumber = RANDOMIZER.NextShort(),
                SubTANumber = RANDOMIZER.NextShort(),
                DataIn = Encoding.UTF8.GetBytes(
                    PageFactory.CreateEventActionPage(
                        RANDOMIZER.GetString(),
                        RANDOMIZER.GetString()).Page.ToJsonString()),
                Descriptor = new TrigActSupportInfo {
                    ComponentType = EEventComponentType.Condition
                }
            };
            var testTriggerType = new TestTriggerType(info, null);
            Assert.AreEqual(true, testTriggerType.IsCondition);
        }*/
        
        [Test]
        [Description("Call GetSubTriggerName after creating a new AbstractTriggerType and expect it to throw an exception.")]
        [Author("JLW")]
        public void GetSubTriggerName_Get_Default_ThrowsException() {
            var testTriggerType = new TestTriggerType();
            Assert.Throws<ArgumentOutOfRangeException>(() => testTriggerType.GetSubTriggerName(0));
        }
        
        [TestCaseSource(nameof(ValidEqualsTestCaseSource))]
        [Description("Compare an object with an instance of AbstractTriggerType and expect true to be returned.")]
        [Author("JLW")]
        public void Equals_Valid_ReturnsTrue(AbstractTriggerType trigger, object obj) {
            Assert.IsTrue(trigger.Equals(obj));
        }
        
        [TestCaseSource(nameof(InvalidEqualsTestCaseSource))]
        [Description("Compare an object with an instance of AbstractTriggerType and expect false to be returned.")]
        [Author("JLW")]
        public void Equals_Invalid_ReturnsFalse(AbstractTriggerType trigger, object obj) {
            Assert.IsFalse(trigger.Equals(obj));
        }
        
        [TestCaseSource(nameof(ValidEqualsTestCaseSource))]
        [Description("Compare the hash code of an instance of AbstractTriggerType with another and expect them to be the same.")]
        [Author("JLW")]
        public void GetHashCode_Valid_IsSame(AbstractTriggerType trigger, object obj) {
            Assert.AreEqual(obj.GetHashCode(), trigger.GetHashCode());
        }
        
        [TestCaseSource(nameof(InvalidEqualsTestCaseSource))]
        [Description("Compare the hash code of an instance of AbstractTriggerType with another and expect them to not be the same.")]
        [Author("JLW")]
        public void GetHashCode_Invalid_IsNotSame(AbstractTriggerType trigger, object obj) {
            Assert.AreNotEqual(obj?.GetHashCode(), trigger.GetHashCode());
        }
        
    }

}