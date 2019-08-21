using System;
using System.Collections.Generic;
using System.Reflection;

namespace HomeSeer.PluginSdk.Devices.Controls {

    /// <summary>
    /// A control associated with a feature on a HomeSeer system. Formerly referred to as a VSPair.
    /// <para>
    /// This defines a control that will be available for a user to interact with.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Legacy VSPairs used to be able to be defined as either status-only, control-only, or both,
    ///  but this is no longer allowed. All StatusControls are considered both, and a <see cref="StatusGraphic"/>
    ///  will override the <see cref="AbstractHsDevice.Status"/> on the feature if it is configured for the feature's
    ///  current <see cref="AbstractHsDevice.Value"/>.
    /// <para>
    ///  If you are looking to add a status-only control to a feature, create a <see cref="StatusGraphic"/> instead.
    /// </para>
    /// </remarks>
    /// <seealso cref="StatusGraphic"/>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class StatusControl {
        
        private string _label = "";
        private EControlUse _controlUse = EControlUse.NotSpecified;
        private EControlType _controlType;
        private List<string> _controlStates = new List<string>();
        private double _targetValue;
        private bool _isRange = false;
        private ValueRange _targetRange = new ValueRange(0,0);
        private ControlLocation _location = new ControlLocation();

        /// <summary>
        /// Initialize a new StatusControl of the specified type
        /// </summary>
        /// <param name="type">The <see cref="EControlType"/> of the control</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if an invalid type is specified</exception>
        public StatusControl(EControlType type) {
            _controlType = type;
            switch (type) {
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
        
        /// <summary>
        /// The text displayed on the control.
        /// <para>
        /// Unless overridden by a <see cref="StatusGraphic.Label"/> associated with the same feature,
        ///  this text is used as the status for the corresponding value.
        /// </para>
        /// </summary>
        public string Label {
            get => _label;
            set => _label = value;
        }

        /// <summary>
        /// What the <see cref="StatusControl"/> is used for.
        ///  See <see cref="EControlUse"/> for more information.
        /// </summary>
        /// <seealso cref="EControlUse"/>
        public EControlUse ControlUse {
            get => _controlUse;
            set => _controlUse = value;
        }

        /// <summary>
        /// The style the control is displayed as to users
        /// </summary>
        /// <seealso cref="EControlType"/>
        public EControlType ControlType {
            get => _controlType;
            set => _controlType = value;
        }

        /// <summary>
        /// The possible states available for users to set this control to.
        /// <para>
        /// This is only used when the <see cref="ControlType"/> is set to <see cref="EControlType.TextSelectList"/>
        /// </para>
        /// </summary>
        public List<string> ControlStates {
            get => _controlStates;
            set => _controlStates = value;
        }

        /// <summary>
        /// The value this StatusControl targets.
        /// <para>
        /// If <see cref="IsRange"/> is TRUE then this is ignored in favor of <see cref="TargetRange"/>
        /// </para>
        /// </summary>
        public double TargetValue {
            get => _targetValue;
            set => _targetValue = value;
        }

        /// <summary>
        /// Whether the <see cref="StatusControl"/> targets a range of values or a single value.
        /// <para>
        /// Settings this to TRUE will cause the <see cref="TargetValue"/> field to be ignored in favor of
        ///  the <see cref="TargetRange"/>
        /// </para>
        /// </summary>
        public bool IsRange {
            get => _isRange;
            set => _isRange = value;
        }

        /// <summary>
        /// The range of values that the <see cref="StatusControl"/> targets.
        /// <para>
        /// If <see cref="IsRange"/> is FALSE then this is ignored in favor of <see cref="TargetValue"/>
        /// </para>
        /// </summary>
        /// <seealso cref="ValueRange"/>
        public ValueRange TargetRange {
            get => _targetRange;
            set => _targetRange = value;
        }

        /// <summary>
        /// The location of the <see cref="StatusControl"/> when displayed to users
        /// </summary>
        /// <seealso cref="ControlLocation"/>
        public ControlLocation Location {
            get => _location;
            set => _location = value;
        }

        /// <summary>
        /// The column that the <see cref="StatusControl"/> is located at
        /// </summary>
        public int Column => _location?.Column ?? 0;

        /// <summary>
        /// The row that the <see cref="StatusControl"/> is located at
        /// </summary>
        public int Row => _location?.Row ?? 0;

        /// <summary>
        /// The number of columns the <see cref="StatusControl"/> occupies
        /// </summary>
        public int Width => _location?.Width ?? 0;

        public double ValueOffset => _targetRange?.ValueOffset ?? 0;
        
        /// <summary>
        /// Get the label for the specified value correctly formatted according to the <see cref="StatusControl"/>'s
        ///  configuration
        /// </summary>
        /// <param name="value">The value to get the label for</param>
        /// <returns>The value as a string formatted according to the <see cref="TargetRange"/> configuration.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not targeted by the <see cref="StatusControl"/></exception>
        public string GetLabelForValue(double value) {
            if (!_isRange) {
                return _label;
            }
            if (!_targetRange.IsValueInRange(value)) {
                throw new ArgumentOutOfRangeException(nameof(value), $"{value} is not valid for this control");
            }

            return _targetRange.GetStringForValue(value);
        }
        
        
        public ControlEvent CreateControlEvent(int devRef) {
            var dce = new ControlEvent(devRef)
                      {
                          Label        = GetLabelForValue(_targetValue),
                          ControlUse   = _controlUse,
                          ControlType  = _controlType,
                          ControlValue = _targetValue
                      };
            
            return dce;
        }

        public ControlEvent CreateControlEvent(int devRef, double value) {
            var dce = new ControlEvent(devRef)
                      {
                          Label        = GetLabelForValue(value),
                          ControlUse   = _controlUse,
                          ControlType  = _controlType,
                          ControlValue = value
                      };
            
            return dce;
        }

        /// <summary>
        /// Determine if a specified value is targeted by the <see cref="StatusControl"/>"/>
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>
        /// TRUE if the value is targeted by the <see cref="StatusControl"/>,
        ///  FALSE if it is not
        /// </returns>
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