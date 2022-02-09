using NUnit.Framework;
using HomeSeer.PluginSdk.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Devices.Tests {
    
    [TestFixture()]
    [TestOf(typeof(PlugExtraData))]
    public class PlugExtraDataTests {
        
        // 1 - Create an instance of PlugExtraData
        // 2 - Add a bunch of items to it
        // 3 - Check that items are in the instance
        // 4 - Remove all items from the instance

        [Test()]
        public void AddNamedTest(string key, object value) {
            Assume.That(key != null);
            Assume.That(value != null);

            var ped = new PlugExtraData();
            var serializedValue = JsonConvert.SerializeObject(value);
            Assert.IsTrue(ped.AddNamed(key, serializedValue));
        }

        [Test()]
        public void GetNamedTest(string key, object value) {
            Assume.That(key != null);
            Assume.That(value != null);

            var ped = new PlugExtraData();
            var serializedValue = JsonConvert.SerializeObject(value);
            Assert.IsTrue(ped.AddNamed(key, serializedValue));
            Assert.AreEqual(serializedValue, ped[key]);
        }

        [Test()]
        public void ContainsNamedTest(string key, object value) {
            Assume.That(key != null);
            Assume.That(value != null);

            var ped = new PlugExtraData();
            var serializedValue = JsonConvert.SerializeObject(value);
            Assert.IsTrue(ped.AddNamed(key, serializedValue));
            Assert.IsTrue(ped.ContainsNamed(key));
        }

        [Test()]
        public void RemoveNamedTest(string key, object value) {
            Assume.That(key != null);
            Assume.That(value != null);
            
            var ped = new PlugExtraData();
            var serializedValue = JsonConvert.SerializeObject(value);
            Assert.IsTrue(ped.AddNamed(key, serializedValue));
            Assert.IsTrue(ped.RemoveNamed(key));
        }

        [Test()]
        public void RemoveAllNamedTest(string key, object value) {
            Assume.That(key != null);
            Assume.That(value != null);
            
            var ped = new PlugExtraData();
            var serializedValue = JsonConvert.SerializeObject(value);
            Assert.IsTrue(ped.AddNamed(key, serializedValue));
            Assert.DoesNotThrow(() => {
                ped.RemoveAllNamed();
            });
            Assert.AreEqual(0, ped.NamedCount);
        }

        [Test()]
        public void AddUnNamedTest(object value) {
            Assume.That(value != null);
            
            var ped = new PlugExtraData();
            var serializedValue = JsonConvert.SerializeObject(value);
            Assert.AreEqual(0, ped.AddUnNamed(serializedValue));
        }

        [Test()]
        public void GetUnNamedTest(object value) {
            Assume.That(value != null);
            
            var ped = new PlugExtraData();
            var serializedValue = JsonConvert.SerializeObject(value);
            var itemIndex = ped.AddUnNamed(serializedValue);
            Assert.AreEqual(serializedValue, ped[itemIndex]);
        }

        [Test()]
        public void RemoveUnNamedAtTest(object value) {
            Assume.That(value != null);
            
            var ped = new PlugExtraData();
            var serializedValue = JsonConvert.SerializeObject(value);
            var itemIndex = ped.AddUnNamed(serializedValue);
            Assert.DoesNotThrow(() => {
                ped.RemoveUnNamedAt(itemIndex);
            });
        }

        [Test()]
        public void RemoveUnNamedTest(object value) {
            Assume.That(value != null);
            
            var ped = new PlugExtraData();
            var serializedValue = JsonConvert.SerializeObject(value);
            Assert.AreEqual(0, ped.AddUnNamed(serializedValue));
            Assert.IsTrue(ped.RemoveUnNamed(serializedValue));
        }

        [Test()]
        public void RemoveAllUnNamedTest(object value) {
            Assume.That(value != null);
            
            var ped = new PlugExtraData();
            var serializedValue = JsonConvert.SerializeObject(value);
            Assert.AreEqual(0, ped.AddUnNamed(serializedValue));
            Assert.DoesNotThrow(() => {
                ped.RemoveAllUnNamed();
            });
        }
    }
}