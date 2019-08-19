using System;
using System.Collections.Generic;
using System.Reflection;

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// A control associated with a feature on a HomeSeer system. Formerly referred to as a VSPair.
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class StatusControl {
        
        private string _label = "";
        private bool _isControl = true;
        private bool _isStatus = true;
        private EControlUse _controlUse = EControlUse.NotSpecified;
        private EControlType _controlType;
        private List<string> _controlStates = new List<string>();
        private double _targetValue;
        private bool _isRange = false;
        private ValueRange _targetRange = new ValueRange(0,0);
        private ControlLocation _location = new ControlLocation();

        /// <summary>
        /// Initialize a new control of the specified type
        /// </summary>
        /// <param name="type">The <see cref="EControlType"/> of the control</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if an invalid type is specified</exception>
        public StatusControl(EControlType type) {
            _controlType = type;
            switch (type) {
                case EControlType.StatusOnly:
                    _isControl = false;
                    _isStatus = true;
                    //throw new ArgumentException("You must specify a valid control type", nameof(type));
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
        
        public DeviceControlEvent CreateControlEvent(int devRef) {
            var dce = new DeviceControlEvent(devRef)
                      {
                          Label        = GetLabelForValue(_targetValue),
                          ControlUse   = _controlUse,
                          ControlType  = _controlType,
                          ControlValue = _targetValue
                      };
            
            return dce;
        }

        public DeviceControlEvent CreateControlEvent(int devRef, double value) {
            var dce = new DeviceControlEvent(devRef)
                      {
                          Label        = GetLabelForValue(value),
                          ControlUse   = _controlUse,
                          ControlType  = _controlType,
                          ControlValue = value
                      };
            
            return dce;
        }

        public bool IsValueInRange(double value) {
            if (_isRange) {
                return _targetRange.IsValueInRange(value);
            }

            return Math.Abs(_targetValue - value) < 0.01D;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override int GetHashCode() {
            return _isRange ? _targetRange.Min.GetHashCode() : _targetValue.GetHashCode();
        }

    }

}