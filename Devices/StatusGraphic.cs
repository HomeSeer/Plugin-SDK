using System;
using HomeSeer.PluginSdk.Devices;

namespace HomeSeer.PluginSdk {

    /// <summary>
    /// Status graphics are used to specify what image is displayed for a device when its value matches certain criteria
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class StatusGraphic {

        private bool         _isRange = false;
        private string       _graphicPath = "";
        private double       _value;
        private ValueRange   _range = new ValueRange(0,0);
        
        public StatusGraphic(string imagePath, double targetValue) {
            _graphicPath = imagePath;
            _value = targetValue;
        }
        
        public StatusGraphic(string imagePath, double minValue, double maxValue) {
            _graphicPath = imagePath;
            _isRange = true;
            _range.Min = minValue;
            _range.Max = maxValue;
        }

        public string Graphic {
            get => _graphicPath;
            set => _graphicPath = value;
        }

        public bool IsRange {
            get => _isRange;
            set => _isRange = value;
        }

        public double RangeMin {
            get => _range?.Min ?? 0;
            set => _range.Min = value;
        }

        public double RangeMax {
            get => _range?.Max ?? 0;
            set => _range.Max = value;
        }

        public double Value {
            get => _value;
            set => _value = value;
        }

        public bool IsValueInRange(double value) {
            if (_isRange) {
                return _range.IsValueInRange(value);
            }

            return Math.Abs(_value - value) < 0.01D;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            if (!(obj is StatusGraphic otherSg)) {
                return false;
            }

            if (_isRange != otherSg._isRange) {
                return false;
            }
            if (_graphicPath != otherSg._graphicPath) {
                return false;
            }
            if (_value != otherSg._value) {
                return false;
            }
            if (_range != otherSg._range) {
                return false;
            }

            return true;
        }

        public override int GetHashCode() {
            return _isRange ? _range.Min.GetHashCode() : _value.GetHashCode();
        }

    }

}