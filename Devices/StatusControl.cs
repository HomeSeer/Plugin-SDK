using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using HomeSeer.PluginSdk.CAPI;

namespace HomeSeer.PluginSdk.Devices {

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class StatusControl {
        
        //public string Label { get; set; }
        private string _label = "";
        private bool _isControl;
        private bool _isStatus = true;
        //public int ControlUse { get; set; }
        private EControlUse _controlUse = EControlUse.NotSpecified;
        //public EControlType ControlType { get; set; }
        private EControlType _controlType = EControlType.NotSpecified;
        //public object ControlStringList { get; set; }
        private List<string> _controlStates = new List<string>();
        //public double ControlValue { get; set; }
        private double _targetValue;
        //public bool SingleRangeEntry { get; set; }
        private bool _isRange;
        private ValueRange _targetRange = new ValueRange();
        /*
         public ControlLocation ControlLocation { get; set; }
         public int ControlLocRow { get; set; }
         public int ControlLocColumn { get; set; }
         public int ControlLocColumnSpan { get; set; }
         */
        private ControlLocation _location = new ControlLocation();

        public string Label {
            get => _label;
            set => _label = value;
        }

        public bool IsControl {
            get => _isControl;
            set => _isControl = value;
        }

        public bool IsStatus {
            get => _isStatus;
            set => _isStatus = value;
        }

        public EControlUse ControlUse {
            get => _controlUse;
            set => _controlUse = value;
        }

        public EControlType ControlType {
            get => _controlType;
            set => _controlType = value;
        }

        public List<string> ControlStates {
            get => _controlStates;
            set => _controlStates = value;
        }

        public double TargetValue {
            get => _targetValue;
            set => _targetValue = value;
        }

        public bool IsRange {
            get => _isRange;
            set => _isRange = value;
        }

        public ValueRange TargetRange {
            get => _targetRange;
            set => _targetRange = value;
        }

        public ControlLocation Location {
            get => _location;
            set => _location = value;
        }

        public int Column => _location?.Column ?? 0;

        public int Row => _location?.Row ?? 0;

        public int Width => _location?.Width ?? 0;

        public double ValueOffset => _targetRange?.ValueOffset ?? 0;
        
        public string GetLabelForValue(double value) {
            if (!_isRange) {
                return _label;
            }
            if (!_targetRange.IsValueInRange(value)) {
                //TODO throw exception?
                return "";
            }

            return _targetRange.GetStringForValue(value);
        }

    }

}