using System;

namespace HomeSeer.PluginSdkTests {
    
    [Serializable]
    public class TestData {
        public string Key { get; set; }
        public int Value { get; set; }

        public TestData() { }

        public TestData(string key, int value) {
            Key = key;
            Value = value;
        }

        protected bool Equals(TestData other) {
            return Key == other.Key && Value == other.Value;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TestData)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((Key != null ? Key.GetHashCode() : 0) * 397) ^ Value;
            }
        }
    }
}