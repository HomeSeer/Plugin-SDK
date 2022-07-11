using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using HomeSeer.PluginSdkTests;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace HomeSeer.PluginSdk.Events {

    [TestFixture(
        TestOf = typeof(TrigActInfo),
        Description = "Tests of the TrigActInfo class to ensure it behaves as expected under normal conditions.",
        Author = "JLW")]
    public class TrigActInfoTests {

        private static readonly Randomizer RANDOMIZER = Randomizer.CreateRandomizer();

        private static IEnumerable<byte[]> InvalidDataTestCaseSource() {
            yield return null;
            yield return Array.Empty<byte>();
        }

        private static IEnumerable<TestData> ValidDataTestCaseSource() {
            yield return new TestData();
            yield return new TestData(RANDOMIZER.GetString(), RANDOMIZER.NextShort());
        }
        
        private static IEnumerable<int> ValidIntegerTestCaseSource() {
            yield return 0;
            yield return RANDOMIZER.NextShort();
        }
        
        private static IEnumerable<byte[]> ValidByteArrayTestCaseSource() {
            yield return Array.Empty<byte>();
            var byteBuffer = new byte[RANDOMIZER.NextShort(8, 256)];
            RANDOMIZER.NextBytes(byteBuffer);
            yield return byteBuffer;
        }

        /*private static IEnumerable<TrigActSupportInfo> ValidSupportInfoTestCaseSource() {
            yield return new TrigActSupportInfo();
        }*/

        [Test]
        [Description("Get evRef after creating a new TrigActInfo and expect the default value to be returned.")]
        [Author("JLW")]
        public void evRef_Get_ReturnsDefault() {
            var testInfo = new TrigActInfo();
            Assert.AreEqual(0, testInfo.evRef);
        }

        [TestCaseSource(nameof(ValidIntegerTestCaseSource))]
        [Description("Set evRef to a valid value and expect the same value to be returned when getting evRef after.")]
        [Author("JLW")]
        public void evRef_Set_SetsEvRef(int testEvRef) {
            var testInfo = new TrigActInfo {
                evRef = testEvRef
            };
            Assert.AreEqual(testEvRef, testInfo.evRef);
        }
        
        [Test]
        [Description("Get UID after creating a new TrigActInfo and expect the default value to be returned.")]
        [Author("JLW")]
        public void UID_Get_ReturnsDefault() {
            var testInfo = new TrigActInfo();
            Assert.AreEqual(0, testInfo.UID);
        }
        
        [TestCaseSource(nameof(ValidIntegerTestCaseSource))]
        [Description("Set UID to a valid value and expect the same value to be returned when getting UID after.")]
        [Author("JLW")]
        public void UID_Set_SetsUID(int testUID) {
            var testInfo = new TrigActInfo {
                UID = testUID
            };
            Assert.AreEqual(testUID, testInfo.UID);
        }
        
        [Test]
        [Description("Get TANumber after creating a new TrigActInfo and expect the default value to be returned.")]
        [Author("JLW")]
        public void TANumber_Get_ReturnsDefault() {
            var testInfo = new TrigActInfo();
            Assert.AreEqual(0, testInfo.TANumber);
        }
        
        [TestCaseSource(nameof(ValidIntegerTestCaseSource))]
        [Description("Set TANumber to a valid value and expect the same value to be returned when getting TANumber after.")]
        [Author("JLW")]
        public void TANumber_Set_SetsTANumber(int testTANumber) {
            var testInfo = new TrigActInfo {
                TANumber = testTANumber
            };
            Assert.AreEqual(testTANumber, testInfo.TANumber);
        }
        
        [Test]
        [Description("Get SubTANumber after creating a new TrigActInfo and expect the default value to be returned.")]
        [Author("JLW")]
        public void SubTANumber_Get_ReturnsDefault() {
            var testInfo = new TrigActInfo();
            Assert.AreEqual(0, testInfo.SubTANumber);
        }
        
        [TestCaseSource(nameof(ValidIntegerTestCaseSource))]
        [Description("Set SubTANumber to a valid value and expect the same value to be returned when getting SubTANumber after.")]
        [Author("JLW")]
        public void SubTANumber_Set_SetsSubTANumber(int testSubTANumber) {
            var testInfo = new TrigActInfo {
                SubTANumber = testSubTANumber
            };
            Assert.AreEqual(testSubTANumber, testInfo.SubTANumber);
        }
        
        [Test]
        [Description("Get DataIn after creating a new TrigActInfo and expect the default value to be returned.")]
        [Author("JLW")]
        public void DataIn_Get_ReturnsDefault() {
            var testInfo = new TrigActInfo();
            Assert.AreEqual(null, testInfo.DataIn);
        }
        
        [TestCaseSource(nameof(ValidByteArrayTestCaseSource))]
        [Description("Set DataIn to a valid value and expect the same value to be returned when getting DataIn after.")]
        [Author("JLW")]
        public void DataIn_Set_SetsDataIn(byte[] testDataIn) {
            Console.WriteLine($"Using a byte array of size {testDataIn.Length}");
            var testInfo = new TrigActInfo {
                DataIn = testDataIn
            };
            Assert.AreEqual(testDataIn, testInfo.DataIn);
        }
        
        /*[Test]
        [Description("Get Descriptor after creating a new TrigActInfo and expect the default value to be returned.")]
        [Author("JLW")]
        public void Descriptor_Get_ReturnsDefault() {
            var testInfo = new TrigActInfo();
            Assert.AreEqual(null, testInfo.Descriptor);
        }*/
        
        /*[TestCaseSource(nameof(ValidSupportInfoTestCaseSource))]
        [Description("Set Descriptor to a valid value and expect the same value to be returned when getting Descriptor after.")]
        [Author("JLW")]
        public void Descriptor_Set_SetsDescriptor(TrigActSupportInfo testDescriptor) {
            var testInfo = new TrigActInfo {
                Descriptor = testDescriptor
            };
            Assert.AreEqual(testDescriptor, testInfo.Descriptor);
        }*/

        [TestCaseSource(nameof(InvalidDataTestCaseSource))]
        [Description("Call DeserializeLegacyData with invalid data and expect null to be returned.")]
        [Author("JLW")]
        public void DeserializeLegacyData_InvalidData_ReturnsNull(byte[] data) {
            Assert.IsNull(TrigActInfo.DeserializeLegacyData<TestData>(data, true));
        }

        [Test]
        [Description("Call DeserializeLegacyData with an invalid type for the data and expect null to be returned.")]
        [Author("JLW")]
        public void DeserializeLegacyData_InvalidType_ReturnsNull() {
            var data = new byte[3];
            Assert.IsNull(TrigActInfo.DeserializeLegacyData<TestData>(data, true));
        }

        [TestCaseSource(nameof(ValidDataTestCaseSource))]
        [Description("Call DeserializeLegacyData with valid data and expect the right object to be returned.")]
        [Author("JLW")]
        public void DeserializeLegacyData_ValidTestData_ReturnsExpected(TestData data) {
            var serializedData = SerializeLegacyData(data);
            Assert.AreEqual(data, TrigActInfo.DeserializeLegacyData<TestData>(serializedData, true));
        }

        private static byte[] SerializeLegacyData(object data) {
            if (data == null) {
                return Array.Empty<byte>();
            }
            var sf = new BinaryFormatter();
            try {
                using (var ms = new MemoryStream()) {
                    sf.Serialize(ms, data);
                    return ms.ToArray();
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Error Serializing object: {ex.Message}");
            }
            return Array.Empty<byte>();
        }

    }

}