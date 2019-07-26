using System;
using System.Reflection;

namespace HomeSeer.PluginSdk.Devices {

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class ValueRange {
        
        public double ValueOffset  { get; set; } = 0;
        public int    Steps        { get; set; } = 10;
        
        private int    _decimalPlaces;
        private double _min;
        private double _max;
        private string _prefix = "";
        private string _suffix = "";

        public ValueRange(double min, double max) {
            _min = min;
            _max = max;

            if (min % 1 != 0 || max % 1 != 0) {
                _decimalPlaces = 2;
            }
            else {
                _decimalPlaces = 0;
            }
        }

        public double Min {
            get => _min;
            set {
                if (_max != 0 && value > _max) throw new ArgumentOutOfRangeException();

                _min = value;
            }
        }

        public double Max {
            get => _max;
            set {
                if (_min != 0 && value < _min) throw new ArgumentOutOfRangeException();

                _max = value;
            }
        }

        public int DecimalPlaces {
            get => _decimalPlaces;
            set {
                if (value < 0) throw new ArgumentOutOfRangeException();

                _decimalPlaces = value;
            }
        }

        public string Prefix {
            get => _prefix;
            set => _prefix = string.IsNullOrWhiteSpace(value) ? "" : value;
        }

        public string Suffix {
            get => _suffix;
            set => _suffix = string.IsNullOrWhiteSpace(value) ? "" : value;
        }

        public string GetStringForValue(double value) {
            var offsetValue = value - ValueOffset;
            var stringValue = $"{_prefix}{offsetValue.ToString($"F{_decimalPlaces}")}{_suffix}";

            return stringValue;
        }

        public bool IsValueInRange(double value) {
            return value >= _min && value <= _max;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            if (!(obj is ValueRange otherValueRange)) {
                return false;
            }

            if (_min != otherValueRange._min) {
                return false;
            }
            if (_max != otherValueRange._max) {
                return false;
            }
            if (_prefix != otherValueRange._prefix) {
                return false;
            }
            if (_suffix != otherValueRange._suffix) {
                return false;
            }
            if (_decimalPlaces != otherValueRange._decimalPlaces) {
                return false;
            }
            if (ValueOffset != otherValueRange.ValueOffset) {
                return false;
            }
            if (Steps != otherValueRange.Steps) {
                return false;
            }

            return true;
        }

        public override int GetHashCode() {
            return _min.GetHashCode() * _max.GetHashCode();
        }

    }

}