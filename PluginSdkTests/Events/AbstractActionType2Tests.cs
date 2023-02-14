using HomeSeer.Jui.Views;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSeer.PluginSdk.Events {

    [TestFixture(
        TestOf = typeof(AbstractActionType2),
        Description = "Tests of the AbstractActionType2 class to ensure it behaves as expected under normal conditions.")]
    public class AbstractActionType2Tests {
        
        private static readonly Randomizer RANDOMIZER = Randomizer.CreateRandomizer();

        private static IEnumerable<bool> ValidBoolTestCaseSource() {
            yield return false;
            yield return true;
        }

        private static IEnumerable<object[]> ValidConstructor4ParamTestCaseSource() {
            yield return new object[] {
                RANDOMIZER.NextShort(),
                RANDOMIZER.NextShort(),
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { "viewId", "viewValue" } }, Formatting.None)),
                null
            };
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
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { "viewId", "viewValue" } }, Formatting.None)),
                null, 
                true
            };
            yield return new object[] {
                RANDOMIZER.NextShort(), 
                RANDOMIZER.NextShort(),
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { "viewId", "viewValue" } }, Formatting.None)),
                null, 
                false
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

        private static IEnumerable<object[]> ValidEqualsTestCaseSource() {
            yield return new object[] {
                new TestActionType2(), 
                new TestActionType2()
            };
            var evRef = RANDOMIZER.NextShort();
            var uId = RANDOMIZER.NextShort();
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { "viewId", "viewValue" } }, Formatting.None));
            yield return new object[] {
                new TestActionType2(uId, evRef, data, null),
                new TestActionType2(uId, evRef, data, null)
            };

        }

        private static IEnumerable<object[]> InvalidEqualsTestCaseSource() {
            yield return new object[] {
                new TestActionType2(), 
                null
            };
            yield return new object[] {
                new TestActionType2(), 
                1
            };
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { "viewId", "viewValue" } }, Formatting.None));
            yield return new object[] {
                new TestActionType2(1, 2, data, null),
                new TestActionType2(2, 2, data, null)
            };
            yield return new object[] {
                new TestActionType2(1, 1, data, null),
                new TestActionType2(1, 2, data, null)
            };
            //TODO : This fails for GetHashCode but works for Equals 
            /*yield return new object[] {
                new TestActionType2(1, 2, data, null),
                new TestActionType2(1, 2, Array.Empty<byte>(), null)
            };*/
        }

        [Test]
        [Description("Create a new instance of a AbstractActionType2 with no parameters and expect no exceptions to be thrown.")]
        public void Constructor_NoParam_Valid_DoesNotThrow() {
            Assert.DoesNotThrow(() => _ = new TestActionType2());
        }

        [TestCaseSource(nameof(ValidConstructor4ParamTestCaseSource))]
        [Description("Create a new instance of a AbstractActionType2 with 4 parameters and expect no exceptions to be thrown.")]
        public void Constructor_4Param_Valid_DoesNotThrow(int id, int eventRef, byte[] dataIn, 
            ActionTypeCollection.IActionTypeListener listener) {
            Assert.DoesNotThrow(() => _ = new TestActionType2(id, eventRef, dataIn, listener));
        }
        
        [TestCaseSource(nameof(ValidConstructor4ParamWithDebugTestCaseSource))]
        [Description("Create a new instance of a AbstractActionType2 with 4 parameters and LogDebug and expect no exceptions to be thrown.")]
        public void Constructor_4ParamWithDebug_Valid_DoesNotThrow(int id, int eventRef, byte[] dataIn, 
            ActionTypeCollection.IActionTypeListener listener, bool logDebug) {
            Assert.DoesNotThrow(() => _ = new TestActionType2(id, eventRef, dataIn, listener, logDebug));
        }
        
        [TestCaseSource(nameof(ValidConstructor5ParamTestCaseSource))]
        [Description("Create a new instance of a AbstractActionType2 with 5 parameters and expect no exceptions to be thrown.")]
        public void Constructor_5Param_Valid_DoesNotThrow(int id, int subTypeNumber, int eventRef, byte[] dataIn, 
            ActionTypeCollection.IActionTypeListener listener) {
            Assert.DoesNotThrow(() => _ = new TestActionType2(id, subTypeNumber, eventRef, dataIn, listener));
        }
        
        [Test]
        [Description("Get LogDebug after creating a new AbstractActionType2 and expect the default value to be returned.")]
        public void LogDebug_Get_ReturnsDefault() {
            var testActionType2 = new TestActionType2();
            Assert.AreEqual(false, testActionType2.LogDebug);
        }

        [TestCaseSource(nameof(ValidBoolTestCaseSource))]
        [Description("Get LogDebug after creating a new AbstractActionType2 and expect the default value to be returned.")]
        public void LogDebug_Set_SetsLogDebug(bool logDebug) {
            var testActionType2 = new TestActionType2 {
                LogDebug = logDebug
            };
            Assert.AreEqual(logDebug, testActionType2.LogDebug);
        }

        [Test]
        [Description("Get ActionListener after creating a new AbstractActionType2 and expect the default value to be returned.")]
        public void ActionListener_Get_ReturnsDefault() {
            var testActionType2 = new TestActionType2();
            Assert.AreEqual(null, testActionType2.ActionListener);
        }
        
        [Test]
        [Description("Get Id after creating a new AbstractActionType2 and expect the default value to be returned.")]
        public void Id_Get_ReturnsDefault() {
            var testActionType2 = new TestActionType2();
            Assert.AreEqual(0, testActionType2.Id);
        }
        
        [Test]
        [Description("Get EventRef after creating a new AbstractActionType2 and expect the default value to be returned.")]
        public void EventRef_Get_ReturnsDefault() {
            var testActionType2 = new TestActionType2();
            Assert.AreEqual(0, testActionType2.EventRef);
        }
        
        [Test]
        [Description("Get Data after creating a new AbstractActionType2 and expect the default value to be returned.")]
        public void Data_Get_ReturnsDefault() {
            var testActionType2 = new TestActionType2(1, 2, null, null);
            var defaultData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Dictionary<string, string>() { { testActionType2.InputId, "" } }, Formatting.None));
            Assert.AreEqual(defaultData, testActionType2.Data);
        }
        
        [Test]
        [Description("Get Name after creating a new AbstractActionType2 and expect it to not be empty.")]
        public void Name_Get_IsNotEmpty() {
            var testActionType2 = new TestActionType2();
            Assert.IsNotEmpty(testActionType2.Name);
        }
        
        [Test]
        [Description("Call ToHtml after creating a new AbstractActionType2 and expect the return value to not be empty.")]
        public void ToHtml_Get_IsNotEmpty() {
            var testActionType2 = new TestActionType2();
            Assert.IsNotEmpty(testActionType2.ToHtml());
        }
        
        //TODO ProcessPostData no changes returns true
        //TODO ProcessPostData with changes and blank ConfigPage returns true
        //TODO ProcessPostData with changes and non-blank ConfigPage returns true
        //TODO ProcessPostData with changes not on ConfigPage and non-blank ConfigPage returns true
        
        [TestCaseSource(nameof(ValidEqualsTestCaseSource))]
        [Description("Compare an object with an instance of AbstractActionType2 and expect true to be returned.")]
        public void Equals_Valid_ReturnsTrue(AbstractActionType2 action, object obj) {
            Assert.IsTrue(action.Equals(obj));
        }
        
        [TestCaseSource(nameof(InvalidEqualsTestCaseSource))]
        [Description("Compare an object with an instance of AbstractActionType2 and expect false to be returned.")]
        public void Equals_Invalid_ReturnsFalse(AbstractActionType2 action, object obj) {
            Assert.IsFalse(action.Equals(obj));
        }
        
        [TestCaseSource(nameof(ValidEqualsTestCaseSource))]
        [Description("Compare the hash code of an instance of AbstractActionType2 with another and expect them to be the same.")]
        public void GetHashCode_Valid_IsSame(AbstractActionType2 action, object obj) {
            Assert.AreEqual(obj.GetHashCode(), action.GetHashCode());
        }
        
        [TestCaseSource(nameof(InvalidEqualsTestCaseSource))]
        [Description("Compare the hash code of an instance of AbstractActionType2 with another and expect them to not be the same.")]
        public void GetHashCode_Invalid_IsNotSame(AbstractActionType2 action, object obj) {
            Assert.AreNotEqual(obj?.GetHashCode(), action.GetHashCode());
        }

    }

}