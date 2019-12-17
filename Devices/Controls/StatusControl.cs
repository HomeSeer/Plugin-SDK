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
        private bool _isRange;
        private ValueRange _targetRange = new ValueRange(0,0);
        private ControlLocation _location = new ControlLocation();
        private uint _flags;

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
                case EControlType.Values:
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
        /// Whether the <see cref="StatusControl"/> label includes additional data tokens to be replaced by strings
        ///  in <see cref="HsFeature.AdditionalStatusData"/>
        /// </summary>
        public bool HasAdditionalData {
            get => ContainsFlag(EControlFlag.HasAdditionalData);
            set {
                if (value) {
                    AddFlag(EControlFlag.HasAdditionalData);
                }
                else {
                    RemoveFlag(EControlFlag.HasAdditionalData);
                }
            }
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
        /// Whether the StatusControl should be available as a status target for events and other platform integrations
        /// </summary>
        /// <returns>
        /// FALSE if the StatusControl is a valid status target,
        ///  TRUE if it is not.
        /// </returns>
        /// See also: <seealso cref="EControlFlag.InvalidStatusTarget"/>
        public bool IsInvalidStatusTarget {
            get => ContainsFlag(EControlFlag.InvalidStatusTarget);
            set {
                if (value) {
                    AddFlag(EControlFlag.InvalidStatusTarget);
                }
                else {
                    RemoveFlag(EControlFlag.InvalidStatusTarget);
                }
            }
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
        
        /// <summary>
        /// Get the label for the specified value correctly formatted according to the <see cref="StatusControl"/>'s
        ///  configuration
        /// </summary>
        /// <param name="value">The value to get the label for</param>
        /// <param name="additionalData">
        /// Additional data to include in the status label that replaces any tokens from
        ///  <see cref="HsFeature.GetAdditionalDataToken"/> included in the status.
        /// </param>
        /// <returns>The value as a string formatted according to the <see cref="TargetRange"/> configuration.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not targeted by the <see cref="StatusControl"/></exception>
        public string GetLabelForValue(double value, string[] additionalData = null) {
            if (!_isRange) {
                return additionalData == null ? _label : ReplaceAdditionalData(_label, additionalData);
            }
            if (!_targetRange.IsValueInRange(value)) {
                throw new ArgumentOutOfRangeException(nameof(value), $"{value} is not valid for this control");
            }

            return additionalData == null ? _targetRange.GetStringForValue(value) : ReplaceAdditionalData(_targetRange.GetStringForValue(value), additionalData);
        }
        
        /// <summary>
        /// Try to get the label for the specified value correctly formatted according to the
        ///  <see cref="StatusControl"/>'s configuration.
        /// </summary>
        /// <param name="label">The string variable the label will be written to</param>
        /// <param name="value">The value to get the label for</param>
        /// <param name="additionalData">
        /// Additional data to include in the status label that replaces any tokens from
        ///  <see cref="HsFeature.GetAdditionalDataToken"/> included in the status.
        /// </param>
        /// <returns>
        /// TRUE if a label is available for the <see cref="StatusControl"/>,
        ///  FALSE if the value is not valid for this <see cref="StatusControl"/> or there is no label defined.
        /// </returns>
        public bool TryGetLabelForValue(out string label, double value, string[] additionalData = null) {

            if (!_isRange || _targetRange == null) {
                label = _label;
                if (string.IsNullOrWhiteSpace(_label)) {
                    return false;
                }
                if (additionalData == null) {
                    return true;
                }

                label = ReplaceAdditionalData(label, additionalData);
                return true;
            }
            if (!_targetRange.IsValueInRange(value)) {
                label = null;
                return false;
            }

            label = _targetRange.GetStringForValue(value);
            if (additionalData == null) {
                return true;
            }

            label = ReplaceAdditionalData(label, additionalData);
            return true;
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
        
        private string ReplaceAdditionalData(string label, string[] additionalData) {

            var finalLabel = label;

            for (var i = 0; i < additionalData.Length; i++) {
                finalLabel = finalLabel.Replace(HsFeature.GetAdditionalDataToken(i), additionalData[i]);
            }

            return finalLabel;
        }
        
        /// <summary>
        /// Add the specified <see cref="EControlFlag"/> to the StatusControl
        /// </summary>
        /// <param name="controlFlag">The <see cref="EControlFlag"/> to add</param>
        private void AddFlag(EControlFlag controlFlag) {
            
            var currentFlags = _flags | (uint) controlFlag;;
            _flags = currentFlags;
        }

        /// <summary>
        /// Determine if the StatusControl contains the specified <see cref="EControlFlag"/>
        /// </summary>
        /// <param name="controlFlag">The <see cref="EControlFlag"/> to look for</param>
        /// <returns>
        /// TRUE if the StatusControl contains the <see cref="EControlFlag"/>,
        ///  FALSE if it does not.
        /// </returns>
        private bool ContainsFlag(EControlFlag controlFlag) {

            return (_flags & (uint) controlFlag) != 0;
        }

        /// <summary>
        /// Remove the specified <see cref="EControlFlag"/> from the StatusControl
        /// </summary>
        /// <param name="controlFlag">The <see cref="EControlFlag"/> to remove</param>
        private void RemoveFlag(EControlFlag controlFlag) {
            
            var currentFlags = _flags ^ (uint) controlFlag;;
            _flags = currentFlags;
        }

        /// <summary>
        /// Clear all <see cref="EControlFlag"/>s on the StatusControl.
        /// </summary>
        private void ClearFlags() {
            
            _flags = 0;
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
            if (_flags != otherStatusControl._flags) {
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