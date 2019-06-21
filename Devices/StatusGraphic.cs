using System;

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
        private double       _rangeStart;
        private double       _rangeEnd;
        
        public string Graphic {
            get => _graphicPath;
            set => _graphicPath = value;
        }

        public bool IsRange {
            get => _isRange;
            set => _isRange = value;
        }

        public double RangeStart {
            get => _rangeStart;
            set => _rangeStart = value;
        }

        public double RangeEnd {
            get => _rangeEnd;
            set => _rangeEnd = value;
        }

        public double Value {
            get => _value;
            set => _value = value;
        }

    }

}