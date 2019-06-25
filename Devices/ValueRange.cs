using System;

namespace HomeSeer.PluginSdk.Devices {

    public class ValueRange {
        
        public double ValueOffset  { get; set; } = 0;
        
        private int    _decimalPlaces;
        private double _min;
        private double _max;
        private string _prefix = "";
        private string _suffix = "";

        public double Min {
            get => _min;
            set {
                if (value > _max) throw new ArgumentOutOfRangeException();

                _min = value;
            }
        }

        public double Max {
            get => _max;
            set {
                if (value < _min) throw new ArgumentOutOfRangeException();

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
            return value >= _min | value <= _max;
        }

    }

}