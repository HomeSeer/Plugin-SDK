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
        private ValueRange        _range = new ValueRange();
        
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

    }

}