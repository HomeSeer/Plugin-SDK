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
        private bool _isControl = true;
        private bool _isStatus = true;
        //public int ControlUse { get; set; }
        private EControlUse _controlUse = EControlUse.NotSpecified;
        //public EControlType ControlType { get; set; }
        private EControlType _controlType;
        //public object ControlStringList { get; set; }
        private List<string> _controlStates = new List<string>();
        //public double ControlValue { get; set; }
        private double _targetValue;
        //public bool SingleRangeEntry { get; set; }
        private bool _isRange = false;
        private ValueRange _targetRange = new ValueRange(0,0);
        /*
         public ControlLocation ControlLocation { get; set; }
         public int ControlLocRow { get; set; }
         public int ControlLocColumn { get; set; }
         public int ControlLocColumnSpan { get; set; }
         */
        private ControlLocation _location = new ControlLocation();

        public StatusControl(EControlType type) {
            _controlType = type;
            switch (type) {
                case EControlType.NotSpecified:
                    throw new ArgumentException("You must specify a valid control type", nameof(type));
                    break;
                case EControlType.TextSelectList:
                    break;
                case EControlType.Button:
                    break;
                case EControlType.ValueRangeDropDown:
                    _isRange = true;
                    break;
                case EControlType.ValueRangeSlider:
                    _isRange = true;
                    break;
                case EControlType.TextBoxNumber:
                    break;
                case EControlType.TextBoxString:
                    break;
                case EControlType.RadioOption:
                    break;
                case EControlType.ButtonScript:
                    break;
                case EControlType.ColorPicker:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "You must specify a valid control type");
            }
        }

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

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            if (!(obj is StatusControl otherStatusControl)) {
                return false;
            }
			
            if (_isControl != otherStatusControl._isControl) {
                return false;
            }
            if (_isStatus != otherStatusControl._isStatus) {
                return false;
            }
            if (_isRange != otherStatusControl._isRange) {
                return false;
            }
            if (_label != otherStatusControl._label) {
                return false;
            }
            if (_controlUse != otherStatusControl._controlUse) {
                return false;
            }
            if (_controlType != otherStatusControl._controlType) {
                return false;
            }
            if (_targetValue != otherStatusControl._targetValue) {
                return false;
            }
            if (_location != otherStatusControl._location) {
                return false;
            }
            if (_controlStates != otherStatusControl._controlStates) {
                return false;
            }
            if (_targetRange != otherStatusControl._targetRange) {
                return false;
            }

            return true;
        }

        public override int GetHashCode() {
            return _isRange ? _targetRange.Min.GetHashCode() : _targetValue.GetHashCode();
        }

    }

}