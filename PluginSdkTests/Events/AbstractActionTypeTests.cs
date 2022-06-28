using HomeSeer.Jui.Views;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSeer.PluginSdk.Events {

    //TODO add constructor test that uses a ConfigPage that is the wrong type
    
    [TestFixture(
        TestOf = typeof(AbstractActionType),
        Description = "Tests of the AbstractActionType class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class AbstractActionTypeTests {
        
        private static readonly Randomizer RANDOMIZER = Randomizer.CreateRandomizer();

        private static IEnumerable<bool> ValidBoolTestCaseSource() {
            yield return false;
            yield return true;
        }

        private static IEnumerable<object[]> ValidConstructor4ParamTestCaseSource() {
            yield return new object[] {
                RANDOMIZER.NextShort(), 
                RANDOMIZER.NextShort(), 
                Encoding.UTF8.GetBytes(
                    PageFactory.CreateEventActionPage(
                        RANDOMIZER.GetString(), 
                        RANDOMIZER.GetString()).Page.ToJsonString()), 
                null
            };
            var byteBuffer = new byte[RANDOMIZER.NextShort(8, 256)];
            RANDOMIZER.NextBytes(byteBuffer);
            yield return new object[] {
                RANDOMIZER.NextShort(), 
                RANDOMIZER.NextShort(), 
                byteBuffer, 
                null
            };
            yield return new object[] {
                RANDOMIZER.NextShort(), 
                RANDOMIZER.NextShort(), 
                null, 
                null
            };
        }
        
        private static IEnumerable<object[]> ValidConstructor4ParamWithDebugTestCaseSource() {
            yield return new object[] {
                RANDOMIZER.NextShort(), 
                RANDOMIZER.NextShort(), 
                Encoding.UTF8.GetBytes(
                    PageFactory.CreateEventActionPage(
                        RANDOMIZER.GetString(), 
                        RANDOMIZER.GetString()).Page.ToJsonString()),
                null, 
                true
            };
            yield return new object[] {
                RANDOMIZER.NextShort(), 
                RANDOMIZER.NextShort(), 
                Encoding.UTF8.GetBytes(
                    PageFactory.CreateEventActionPage(
                        RANDOMIZER.GetString(), 
                        RANDOMIZER.GetString()).Page.ToJsonString()),
                null, 
                false
            };
        }

        private static IEnumerable<object[]> ValidConstructor5ParamTestCaseSource() {
            yield return new object[] {
                RANDOMIZER.NextShort(), 
                RANDOMIZER.NextShort(),
                RANDOMIZER.NextShort(),
                Encoding.UTF8.GetBytes(
                    PageFactory.CreateEventActionPage(
                        RANDOMIZER.GetString(),
                        RANDOMIZER.GetString()).Page.ToJsonString()),
                null
            };
        }

        private static IEnumerable<object[]> ValidEqualsTestCaseSource() {
            yield return new object[] {
                new TestActionType(), 
                new TestActionType()
            };
            var evRef = RANDOMIZER.NextShort();
            var uId = RANDOMIZER.NextShort();
            var data = Encoding.UTF8.GetBytes(
                PageFactory.CreateEventActionPage(
                    $"{evRef}-{uId}",
                    TestTriggerType.TRIGGER_NAME).Page.ToJsonString());
            yield return new object[] {
                new TestActionType(uId, evRef, data, null),
                new TestActionType(uId, evRef, data, null)
            };
            
        }

        private static IEnumerable<object[]> InvalidEqualsTestCaseSource() {
            yield return new object[] {
                new TestActionType(), 
                null
            };
            yield return new object[] {
                new TestActionType(), 
                1
            };
            var action = new TestActionType();
            yield return new object[] {
                action,
                action
            };
            var data = Encoding.UTF8.GetBytes(
                PageFactory.CreateEventActionPage(
                    $"1-1",
                    TestTriggerType.TRIGGER_NAME).Page.ToJsonString());
            yield return new object[] {
                new TestActionType(1, 2, data, null),
                new TestActionType(2, 2, data, null)
            };
            yield return new object[] {
                new TestActionType(1, 1, data, null),
                new TestActionType(1, 2, data, null)
            };
            yield return new object[] {
                new TestActionType(1, 2, data, null),
                new TestActionType(1, 2, Array.Empty<byte>(), null)
            };
        }

        [Test]
        [Description("Create a new instance of a AbstractActionType with no parameters and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_NoParam_Valid_DoesNotThrow() {
            Assert.DoesNotThrow(() => _ = new TestActionType());
        }

        [TestCaseSource(nameof(ValidConstructor4ParamTestCaseSource))]
        [Description("Create a new instance of a AbstractActionType with 4 parameters and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_4Param_Valid_DoesNotThrow(int id, int eventRef, byte[] dataIn, 
            ActionTypeCollection.IActionTypeListener listener) {
            Assert.DoesNotThrow(() => _ = new TestActionType(id, eventRef, dataIn, listener));
        }
        
        [TestCaseSource(nameof(ValidConstructor4ParamWithDebugTestCaseSource))]
        [Description("Create a new instance of a AbstractActionType with 4 parameters and LogDebug and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_4ParamWithDebug_Valid_DoesNotThrow(int id, int eventRef, byte[] dataIn, 
            ActionTypeCollection.IActionTypeListener listener, bool logDebug) {
            Assert.DoesNotThrow(() => _ = new TestActionType(id, eventRef, dataIn, listener, logDebug));
        }
        
        [TestCaseSource(nameof(ValidConstructor5ParamTestCaseSource))]
        [Description("Create a new instance of a AbstractActionType with 5 parameters and expect no exceptions to be thrown.")]
        [Author("JLW")]
        public void Constructor_5Param_Valid_DoesNotThrow(int id, int subTypeNumber, int eventRef, byte[] dataIn, 
            ActionTypeCollection.IActionTypeListener listener) {
            Assert.DoesNotThrow(() => _ = new TestActionType(id, subTypeNumber, eventRef, dataIn, listener));
        }
        
        [Test]
        [Description("Get LogDebug after creating a new AbstractActionType and expect the default value to be returned.")]
        [Author("JLW")]
        public void LogDebug_Get_ReturnsDefault() {
            var testActionType = new TestActionType();
            Assert.AreEqual(false, testActionType.LogDebug);
        }

        [TestCaseSource(nameof(ValidBoolTestCaseSource))]
        [Description("Get LogDebug after creating a new AbstractActionType and expect the default value to be returned.")]
        [Author("JLW")]
        public void LogDebug_Set_SetsLogDebug(bool logDebug) {
            var testActionType = new TestActionType {
                LogDebug = logDebug
            };
            Assert.AreEqual(logDebug, testActionType.LogDebug);
        }

        [Test]
        [Description("Get ActionListener after creating a new AbstractActionType and expect the default value to be returned.")]
        [Author("JLW")]
        public void ActionListener_Get_ReturnsDefault() {
            var testActionType = new TestActionType();
            Assert.AreEqual(null, testActionType.ActionListener);
        }
        
        [Test]
        [Description("Get Id after creating a new AbstractActionType and expect the default value to be returned.")]
        [Author("JLW")]
        public void Id_Get_ReturnsDefault() {
            var testActionType = new TestActionType();
            Assert.AreEqual(0, testActionType.Id);
        }
        
        [Test]
        [Description("Get EventRef after creating a new AbstractActionType and expect the default value to be returned.")]
        [Author("JLW")]
        public void EventRef_Get_ReturnsDefault() {
            var testActionType = new TestActionType();
            Assert.AreEqual(0, testActionType.EventRef);
        }
        
        [Test]
        [Description("Get Data after creating a new AbstractActionType and expect the default value to be returned.")]
        [Author("JLW")]
        public void Data_Get_ReturnsDefault() {
            var testActionType = new TestActionType();
            var defaultData = Encoding.UTF8.GetBytes(PageFactory.CreateEventActionPage(
                "0-0", TestActionType.ACTION_NAME).Page.ToJsonString());
            Assert.AreEqual(defaultData, testActionType.Data);
        }
        
        [Test]
        [Description("Get Name after creating a new AbstractActionType and expect it to not be empty.")]
        [Author("JLW")]
        public void Name_Get_IsNotEmpty() {
            var testActionType = new TestActionType();
            Assert.IsNotEmpty(testActionType.Name);
        }
        
        [Test]
        [Description("Call ToHtml after creating a new AbstractActionType and expect the return value to not be empty.")]
        [Author("JLW")]
        public void ToHtml_Get_IsNotEmpty() {
            var testActionType = new TestActionType();
            Assert.IsNotEmpty(testActionType.ToHtml());
        }
        
        //TODO ProcessPostData no changes returns true
        //TODO ProcessPostData with changes and blank ConfigPage returns true
        //TODO ProcessPostData with changes and non-blank ConfigPage returns true
        //TODO ProcessPostData with changes not on ConfigPage and non-blank ConfigPage returns true
        
        [TestCaseSource(nameof(ValidEqualsTestCaseSource))]
        [Description("Compare an object with an instance of AbstractActionType and expect true to be returned.")]
        [Author("JLW")]
        public void Equals_Valid_ReturnsTrue(AbstractActionType action, object obj) {
            Assert.IsTrue(action.Equals(obj));
        }
        
        [TestCaseSource(nameof(InvalidEqualsTestCaseSource))]
        [Description("Compare an object with an instance of AbstractActionType and expect false to be returned.")]
        [Author("JLW")]
        public void Equals_Invalid_ReturnsFalse(AbstractActionType action, object obj) {
            Assert.IsFalse(action.Equals(obj));
        }
        
        [TestCaseSource(nameof(ValidEqualsTestCaseSource))]
        [Description("Compare the hash code of an instance of AbstractActionType with another and expect them to be the same.")]
        [Author("JLW")]
        public void GetHashCode_Valid_IsSame(AbstractActionType action, object obj) {
            Assert.AreEqual(obj.GetHashCode(), action.GetHashCode());
        }
        
        [TestCaseSource(nameof(InvalidEqualsTestCaseSource))]
        [Description("Compare the hash code of an instance of AbstractActionType with another and expect them to not be the same.")]
        [Author("JLW")]
        public void GetHashCode_Invalid_IsNotSame(AbstractActionType action, object obj) {
            Assert.AreNotEqual(obj.GetHashCode(), action.GetHashCode());
        }

    }

}