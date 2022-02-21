using NUnit.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework.Internal;

namespace HomeSeer.PluginSdk.Devices {
    
    [TestFixture(Description = "Tests of the PlugExtraData class to ensure it behaves as expected under normal conditions.",
        Author = "JLW",
        TestOf = typeof(PlugExtraData))]
    public class PlugExtraDataTests {

        private const string VALID_KEY_CHARACTERS   = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789_-.!@#$%^&*()+=";
        private static readonly Randomizer RANDOMIZER = Randomizer.CreateRandomizer();

        private PlugExtraData _ped;

        [SetUp]
        public void InitPed() {
            _ped = new PlugExtraData();
        }

        /// <summary>
        /// Valid test keys used as a test case source for testing keys
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="string"/></returns>
        /// <seealso cref="Key_Valid_ReturnsTrue"/>
        /// <seealso cref="VALID_KEY_CHARACTERS"/>
        private static IEnumerable<string> ValidTestKeys() {
            yield return RANDOMIZER.GetString(1, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(2, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(3, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(4, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(5, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(6, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(7, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(8, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(16, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(24, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(32, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(40, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(48, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(56, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(64, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(72, VALID_KEY_CHARACTERS);
            yield return RANDOMIZER.GetString(80, VALID_KEY_CHARACTERS);
        }
        
        /// <summary>
        /// Invalid test keys used as a test case source for testing keys
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="string"/></returns>
        /// <seealso cref="Key_Invalid_ReturnsFalse"/>
        private static IEnumerable<string> InvalidTestKeys() {
            yield return null;
            yield return string.Empty;
        }
        
        private static IEnumerable<object[]> NamedTestEntries() {
            //Numbers
            yield return new object[] {
                "num-short", 
                RANDOMIZER.NextShort()
            };
            yield return new object[] {
                "num-long", 
                RANDOMIZER.NextLong()
            };
            yield return new object[] {
                "num-decimal", 
                RANDOMIZER.NextDecimal()
            };
            yield return new object[] {
                "num-double", 
                RANDOMIZER.NextDouble()
            };
            yield return new object[] {
                "num-float", 
                RANDOMIZER.NextFloat()
            };
            //Strings
            yield return new object[] {
                "string", 
                RANDOMIZER.GetString()
            };
            //Booleans
            yield return new object[] {
                "bool", 
                RANDOMIZER.NextBool()
            };
            //Objects
            yield return new object[] {
                "TestExtraData_1",
                new TestExtraData(RANDOMIZER.GetString(), 
                    RANDOMIZER.NextShort(), 
                    RANDOMIZER.NextBool())
            };
        }
        
        private static IEnumerable<object> UnNamedTestEntries() {
            //Numbers
            yield return RANDOMIZER.NextShort();
            yield return RANDOMIZER.NextLong();
            yield return RANDOMIZER.NextDecimal();
            yield return RANDOMIZER.NextDouble();
            yield return RANDOMIZER.NextFloat();
            //Strings
            yield return RANDOMIZER.GetString();
            //Booleans
            yield return RANDOMIZER.NextBool();
            //Objects
            yield return new TestExtraData(RANDOMIZER.GetString(),
                RANDOMIZER.NextShort(),
                RANDOMIZER.NextBool());
        }

        private static IEnumerable<TestExtraData> TestDataTestCaseSource() {
            yield return new TestExtraData(RANDOMIZER.GetString(), 0, false);
            yield return new TestExtraData("", RANDOMIZER.NextShort(), false);
            yield return new TestExtraData(RANDOMIZER.GetString(), RANDOMIZER.NextShort(), RANDOMIZER.NextBool());
        }

        [TestCaseSource(nameof(ValidTestKeys))]
        [Description("Add a named entry with AddNamed using a valid key and expect TRUE to be returned.")]
        [Author("JLW")]
        [Order(10)]
        public void Key_Valid_ReturnsTrue(string key) {
            Assume.That(key != null);
            Assume.That(!string.IsNullOrWhiteSpace(key));
            const string value = "test";
            Assert.IsTrue(_ped.AddNamed(key, value));
        }

        [TestCaseSource(nameof(InvalidTestKeys))]
        [Description("Add a named entry with AddNamed using an invalid key and expect FALSE to be returned.")]
        [Author("JLW")]
        [Order(20)]
        public void Key_Invalid_ReturnsFalse(string key) {
            const string value = "test";
            Assert.Throws<ArgumentNullException>(() => {
                 _ = _ped.AddNamed(key, value); 
             });
        }

        [TestCaseSource(nameof(NamedTestEntries))]
        [Description("Add a named entry with AddNamed using a new key and expect TRUE to be returned.")]
        [Author("JLW")]
        [Order(100)]
        public void AddNamed_NewKey_ReturnsTrue(string key, object value) {
            Assert.IsNotNull(key);
            Assert.IsNotNull(value);
            var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
            Assert.IsTrue(_ped.AddNamed(key, serializedValue));
        }
        
        [TestCaseSource(nameof(NamedTestEntries))]
        [Description("Add a named entry with AddNamed using an existing key and expect FALSE to be returned.")]
        [Author("JLW")]
        [Order(110)]
        public void AddNamed_ExistingKey_ReturnsFalse(string key, object value) {
            Assert.IsNotNull(key);
            Assert.IsNotNull(value);
            var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
            Assume.That(_ped.AddNamed(key, serializedValue));
            Assert.IsFalse(_ped.AddNamed(key, serializedValue));
        }
        
        [TestCaseSource(nameof(InvalidTestKeys))]
        [Description("Add a named entry with AddNamed using an invalid key and expect an exception to be thrown.")]
        [Author("JLW")]
        [Order(120)]
        public void AddNamed_InvalidKey_Throws(string key) {
            Assert.Throws<ArgumentNullException>(() => {
                _ped.AddNamed(key, "test");
            });
        }

        [Test]
        [Description("Add a named entry with AddNamed<TData> using invalid data and expect an exception to be thrown.")]
        [Author("JLW")]
        [Order(130)]
        public void AddNamed_InvalidTestData_Throws() {
            Assert.Throws<ArgumentNullException>(() => {
                _ped.AddNamed<TestExtraData>("key", null);
            });
        }

        [TestCaseSource(nameof(TestDataTestCaseSource))]
        [Description("Add a named entry with AddNamed<TData> using valid TestExtraData and expect true to be returned.")]
        [Author("JLW")]
        [Order(140)]
        public void AddNamed_ValidTestData_ReturnsTrue(TestExtraData data) {
            Assert.IsTrue(_ped.AddNamed("key", data));
        }
        
        [TestCaseSource(nameof(NamedTestEntries))]
        [Description("Get a named entry that exists and expect the correct object.")]
        [Author("JLW")]
        [Order(200)]
        public void GetNamed_KeyExists_ReturnsSameObject(string key, object value) {
            Assert.IsNotNull(key);
            Assert.IsNotNull(value);
            var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
            Assume.That(_ped.AddNamed(key, serializedValue));
            Assert.AreEqual(serializedValue, _ped.GetNamed(key));
            Assert.AreEqual(serializedValue, _ped[key]);
        }
        
        [TestCaseSource(nameof(NamedTestEntries))]
        [Description("Get a named entry that does not exist and expect an exception to be thrown.")]
        [Author("JLW")]
        [Order(210)]
        public void GetNamed_KeyDoesNotExist_Throws(string key, object value) {
            Assert.IsNotNull(key);
            Assert.Throws<KeyNotFoundException>(() => {
                _ = _ped.GetNamed(key);
            });
            Assert.Throws<KeyNotFoundException>(() => {
                _ = _ped[key];
            });
        }
        
        [TestCaseSource(nameof(InvalidTestKeys))]
        [Description("Get a named entry using an invalid key and expect an exception to be thrown.")]
        [Author("JLW")]
        [Order(220)]
        public void GetNamed_InvalidKey_Throws(string key) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = _ped.GetNamed(key);
            });
            Assert.Throws<ArgumentNullException>(() => {
                _ = _ped[key];
            });
        }
        
        [TestCaseSource(nameof(TestDataTestCaseSource))]
        [Description("Get a named entry and expect the correct object to be returned.")]
        [Author("JLW")]
        [Order(230)]
        public void GetNamed_TestData_ReturnsExpected(TestExtraData data) {
            Assert.IsNotNull(data);
            const string key = "key";
            Assert.DoesNotThrow(() => {
                Assume.That(_ped.AddNamed(key, data));
            });
            Assert.AreEqual(data, _ped.GetNamed<TestExtraData>(key));
        }

        [TestCaseSource(nameof(NamedTestEntries))]
        [Description("Check to see if a specific key is present and expect TRUE to be returned.")]
        [Author("JLW")]
        [Order(300)]
        public void ContainsNamed_KeyExists_ReturnsTrue(string key, object value) {
            Assert.IsNotNull(key);
            Assert.IsNotNull(value);
            var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
            Assume.That(_ped.AddNamed(key, serializedValue));
            Assert.IsTrue(_ped.ContainsNamed(key));
        }
        
        [TestCaseSource(nameof(NamedTestEntries))]
        [Description("Check to see if a specific key is present and expect FALSE to be returned.")]
        [Author("JLW")]
        [Order(310)]
        public void ContainsNamed_KeyDoesNotExist_ReturnsFalse(string key, object value) {
            Assert.IsNotNull(key);
            Assert.IsFalse(_ped.ContainsNamed(key));
        }
        
        [TestCaseSource(nameof(InvalidTestKeys))]
        [Description("Check to see if a specific key is present using an invalid key and expect an exception to be thrown.")]
        [Author("JLW")]
        [Order(320)]
        public void ContainsNamed_InvalidKey_Throws(string key) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = _ped.ContainsNamed(key);
            });
        }
        
        [TestCaseSource(nameof(NamedTestEntries))]
        [Description("Remove a specific key and expect TRUE to be returned.")]
        [Author("JLW")]
        [Order(400)]
        public void RemoveNamed_KeyExists_ReturnsTrue(string key, object value) {
            Assert.IsNotNull(key);
            Assert.IsNotNull(value);
            var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
            Assume.That(_ped.AddNamed(key, serializedValue));
            Assert.IsTrue(_ped.RemoveNamed(key));
        }
        
        [TestCaseSource(nameof(NamedTestEntries))]
        [Description("Remove a specific key and expect TRUE to be returned.")]
        [Author("JLW")]
        [Order(410)]
        public void RemoveNamed_KeyDoesNotExist_ReturnsFalse(string key, object value) {
            Assert.IsNotNull(key);
            Assert.IsFalse(_ped.RemoveNamed(key));
        }
        
        [TestCaseSource(nameof(InvalidTestKeys))]
        [Description("Remove an invalid key and expect an exception to be thrown.")]
        [Author("JLW")]
        [Order(420)]
        public void RemoveNamed_InvalidKey_Throws(string key) {
            Assert.Throws<ArgumentNullException>(() => {
                _ = _ped.RemoveNamed(key);
            });
        }
        
        [Test]
        [Description("Remove all named entries and expect no exceptions to be thrown and no named entries to remain.")]
        [Author("JLW")]
        [Order(500)]
        public void RemoveAllNamed_NoNamedRemain() {
            foreach (var namedTestEntry in NamedTestEntries()) {
                var key = namedTestEntry[0] as string;
                var value = namedTestEntry[1];
                var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
                Assume.That(_ped.AddNamed(key, serializedValue));
            }
            Assert.DoesNotThrow(() => {
                _ped.RemoveAllNamed();
            });
            Assert.AreEqual(0, _ped.NamedCount);
        }

        [TestCaseSource(nameof(UnNamedTestEntries))]
        [Description("Add an un-named entry and expect its index to be returned.")]
        [Author("JLW")]
        [Order(600)]
        public void AddUnNamed_ReturnsNextIndex(object value) {
            Assert.IsNotNull(value);
            var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
            var numLoops = Randomizer.CreateRandomizer().NextUShort(1, 5);
            for (var i = 0; i < numLoops; i++) {
                Assert.AreEqual(i, _ped.AddUnNamed(serializedValue));
            }
        }
        
        [Test]
        [Description("Add an un-named entry with AddUnNamed<TData> using invalid data and expect an exception to be thrown.")]
        [Author("JLW")]
        [Order(610)]
        public void AddUnNamed_InvalidTestData_Throws() {
            Assert.Throws<ArgumentNullException>(() => {
                _ped.AddUnNamed<TestExtraData>(null);
            });
        }

        [TestCaseSource(nameof(TestDataTestCaseSource))]
        [Description("Add an un-named entry with AddUnNamed<TData> using valid TestExtraData and expect the next index to be returned.")]
        [Author("JLW")]
        [Order(620)]
        public void AddUnNamed_ValidTestData_ReturnsNextIndex(TestExtraData data) {
            Assert.AreEqual(0, _ped.AddUnNamed(data));
        }

        [TestCaseSource(nameof(UnNamedTestEntries))]
        [Description("Get an un-named entry at a specific index and expect the correct object to be returned.")]
        [Author("JLW")]
        [Order(700)]
        public void GetUnNamed_IndexExists_ReturnsSameObject(object value) {
            Assert.IsNotNull(value);
            var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
            var index = _ped.AddUnNamed(serializedValue);
            Assert.AreEqual(serializedValue, _ped.GetUnNamed(index));
            Assert.AreEqual(serializedValue, _ped[index]);
        }
        
        [TestCaseSource(nameof(UnNamedTestEntries))]
        [Description("Get an un-named entry at a specific index that does not exist and expect an exception to be thrown.")]
        [Author("JLW")]
        [Order(710)]
        public void GetUnNamed_IndexDoesNotExist_Throws(object value) {
            Assert.IsNotNull(value);
            var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
            var index = _ped.AddUnNamed(serializedValue);
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                _ = _ped.GetUnNamed(index + 1);
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                _ = _ped[index + 1];
            });
        }
        
        [TestCaseSource(nameof(TestDataTestCaseSource))]
        [Description("Get an un-named entry and expect the correct object to be returned.")]
        [Author("JLW")]
        [Order(720)]
        public void GetUnNamed_TestData_ReturnsExpected(TestExtraData data) {
            Assert.IsNotNull(data);
            Assert.DoesNotThrow(() => {
                var index = _ped.AddUnNamed(data);
                Assert.AreEqual(data, _ped.GetUnNamed<TestExtraData>(index));
            });
        }

        [TestCaseSource(nameof(UnNamedTestEntries))]
        [Description("Remove an un-named entry at a specific index and expect no exceptions to be thrown.")]
        [Author("JLW")]
        [Order(800)]
        public void RemoveUnNamedAt_IndexExists_DoesNotThrow(object value) {
            Assert.IsNotNull(value);
            var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
            var index = _ped.AddUnNamed(serializedValue);
            Assert.DoesNotThrow(() => {
                _ped.RemoveUnNamedAt(index);
            });
        }
        
        [TestCaseSource(nameof(UnNamedTestEntries))]
        [Description("Remove an un-named entry at a specific index and expect no exceptions to be thrown.")]
        [Author("JLW")]
        [Order(810)]
        public void RemoveUnNamedAt_IndexDoesNotExist_Throws(object value) {
            Assert.IsNotNull(value);
            var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
            var index = _ped.AddUnNamed(serializedValue);
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                _ped.RemoveUnNamedAt(index+1);
            });
        }

        [TestCaseSource(nameof(UnNamedTestEntries))]
        [Description("Remove an un-named entry and expect TRUE to be returned.")]
        [Author("JLW")]
        [Order(900)]
        public void RemoveUnNamed_ItemExists_ReturnsTrue(object value) {
            Assert.IsNotNull(value);
            var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
            Assume.That(_ped.AddUnNamed(serializedValue) == 0);
            Assert.IsTrue(_ped.RemoveUnNamed(serializedValue));
        }
        
        [TestCaseSource(nameof(UnNamedTestEntries))]
        [Description("Try to remove an un-named entry that does not exist and expect FALSE to be returned.")]
        [Author("JLW")]
        [Order(910)]
        public void RemoveUnNamed_ItemDoesNotExist_ReturnsFalse(object value) {
            Assert.IsNotNull(value);
            var serializedValue = value is string s ? s : JsonConvert.SerializeObject(value);
            Assert.IsFalse(_ped.RemoveUnNamed(serializedValue));
        }

        [Test]
        [Description("Remove all un-named entries and expect none to remain.")]
        [Author("JLW")]
        [Order(1000)]
        public void RemoveAllUnNamed_NoUnNamedRemain() {
            foreach (var unNamedTestEntry in UnNamedTestEntries()) {
                var serializedValue = unNamedTestEntry is string s ? s : JsonConvert.SerializeObject(unNamedTestEntry);
                _ = _ped.AddUnNamed(serializedValue);
            }
            Assert.DoesNotThrow(() => {
                _ped.RemoveAllUnNamed();
            });
            Assert.AreEqual(0, _ped.UnNamedCount);
        }

        /// <summary>
        /// Extra data used to test how <see cref="PlugExtraData"/> handles objects
        /// </summary>
        [Serializable]
        public sealed class TestExtraData {

            public string Text { get; }
            public int Number { get; }
            public bool Condition { get; }

            public TestExtraData(string text, int number, bool condition) {
                Text = text;
                Number = number;
                Condition = condition;
            }
            
            public bool Equals(TestExtraData other) {
                return Text == other.Text && Number == other.Number && Condition == other.Condition;
            }

            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) {
                    return false;
                }
                if (ReferenceEquals(this, obj)) {
                    return true;
                }
                if (obj.GetType() != GetType()) {
                    return false;
                }
                return Equals((TestExtraData)obj);
            }

            public override int GetHashCode() {
                unchecked {
                    var hashCode = (Text != null ? Text.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ Number;
                    hashCode = (hashCode * 397) ^ Condition.GetHashCode();
                    return hashCode;
                }
            }
        }
    }
}