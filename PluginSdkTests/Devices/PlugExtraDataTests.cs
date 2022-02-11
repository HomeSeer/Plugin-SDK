using NUnit.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework.Internal;

namespace HomeSeer.PluginSdk.Devices.Tests {
    
    [TestFixture(Description = "Tests of the PlugExtraData class to ensure it behaves as expected under normal conditions.",
                    Author = "JLW",
                    TestOf = typeof(PlugExtraData))]
    public class PlugExtraDataTests {

        private const string _validKeyCharacters   = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789_-.!@#$%^&*()+=";
        private const string _invalidKeyCharacters = "\\\n\r\"'";

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
        /// <seealso cref="_validKeyCharacters"/>
        private static IEnumerable<string> ValidTestKeys() {
            var randomizer = Randomizer.CreateRandomizer();
            yield return randomizer.GetString(1, _validKeyCharacters);
            yield return randomizer.GetString(2, _validKeyCharacters);
            yield return randomizer.GetString(3, _validKeyCharacters);
            yield return randomizer.GetString(4, _validKeyCharacters);
            yield return randomizer.GetString(5, _validKeyCharacters);
            yield return randomizer.GetString(6, _validKeyCharacters);
            yield return randomizer.GetString(7, _validKeyCharacters);
            yield return randomizer.GetString(8, _validKeyCharacters);
            yield return randomizer.GetString(16, _validKeyCharacters);
            yield return randomizer.GetString(24, _validKeyCharacters);
            yield return randomizer.GetString(32, _validKeyCharacters);
            yield return randomizer.GetString(40, _validKeyCharacters);
            yield return randomizer.GetString(48, _validKeyCharacters);
            yield return randomizer.GetString(56, _validKeyCharacters);
            yield return randomizer.GetString(64, _validKeyCharacters);
            yield return randomizer.GetString(72, _validKeyCharacters);
            yield return randomizer.GetString(80, _validKeyCharacters);
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
        
        /// <summary>
        /// Invalid test keys used as a test case source for testing keys
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="string"/></returns>
        /// <seealso cref="Key_Invalid_ReturnsFalse"/>
        /// <seealso cref="_invalidKeyCharacters"/>
        private static IEnumerable<string> InvalidTestKeys() {
            var invalidChars = _invalidKeyCharacters.ToCharArray();
            yield return null;
            yield return string.Empty;
            foreach (var invalidChar in invalidChars) {
                yield return invalidChar.ToString();
            }
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
        
        private static IEnumerable<object[]> NamedTestEntries() {
            var randomizer = Randomizer.CreateRandomizer();
            //Numbers
            yield return new object[] {
                "num-short", 
                randomizer.NextShort()
            };
            yield return new object[] {
                "num-long", 
                randomizer.NextLong()
            };
            yield return new object[] {
                "num-decimal", 
                randomizer.NextDecimal()
            };
            yield return new object[] {
                "num-double", 
                randomizer.NextDouble()
            };
            yield return new object[] {
                "num-float", 
                randomizer.NextFloat()
            };
            //Strings
            yield return new object[] {
                "string", 
                randomizer.GetString()
            };
            //Booleans
            yield return new object[] {
                "bool", 
                randomizer.NextBool()
            };
            //Objects
            yield return new object[] {
                "TestExtraData_1",
                new TestExtraData(randomizer.GetString(), 
                    randomizer.NextShort(), 
                    randomizer.NextBool())
            };
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
        
        //TODO GetNamed<TData> tests

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
        
        private static IEnumerable<object> UnNamedTestEntries() {
            var randomizer = Randomizer.CreateRandomizer();
            //Numbers
            yield return randomizer.NextShort();
            yield return randomizer.NextLong();
            yield return randomizer.NextDecimal();
            yield return randomizer.NextDouble();
            yield return randomizer.NextFloat();
            //Strings
            yield return randomizer.GetString();
            //Booleans
            yield return randomizer.NextBool();
            //Objects
            yield return new TestExtraData(randomizer.GetString(),
                randomizer.NextShort(),
                randomizer.NextBool());
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
        
        //TODO GetUnNamed<TData> tests

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
        private sealed class TestExtraData {

            private string Text { get; }
            private int Number { get; }
            private bool Condition { get; }

            public TestExtraData(string text, int number, bool condition) {
                Text = text;
                Number = number;
                Condition = condition;
            }
            
            private bool Equals(TestExtraData other) {
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