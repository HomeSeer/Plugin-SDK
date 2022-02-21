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