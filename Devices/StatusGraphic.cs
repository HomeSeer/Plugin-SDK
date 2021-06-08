using System;
using HomeSeer.PluginSdk.Devices.Controls;

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// Status graphics are used to specify what image is displayed for a device when its value matches certain criteria
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class StatusGraphic {

        private string       _label = "";
        private EControlUse  _controlUse = EControlUse.NotSpecified;
        private bool         _isRange;
        private bool         _hasAdditionalData;
        private string       _graphicPath;
        private double       _value;
        private ValueRange   _targetRange = new ValueRange(0,0);

        /// <summary>
        /// Initialize a new StatusGraphic with the specified image that targets a single value
        /// </summary>
        /// <param name="imagePath">The path to the image used for the graphic</param>
        /// <param name="targetValue">The value targeted by the <see cref="StatusGraphic"/></param>
        /// <param name="label">The status text</param>
        public StatusGraphic(string imagePath, double targetValue, string label="") {
            _graphicPath = imagePath;
            _value = targetValue;
            _label = label;
        }
        
        /// <summary>
        /// Initialize a new StatusGraphic with the specified image that targets a range of values
        /// </summary>
        /// <param name="imagePath">The path to the image used for the graphic</param>
        /// <param name="minValue">The minimum value targeted by the <see cref="StatusGraphic"/></param>
        /// <param name="maxValue">The maximum value targeted by the <see cref="StatusGraphic"/></param>
        public StatusGraphic(string imagePath, double minValue, double maxValue) {
            _graphicPath = imagePath;
            _isRange = true;
            _targetRange.Max = maxValue;
            _targetRange.Min = minValue;
        }

        /// <summary>
        /// Initialize a new StatusGraphic with the specified image that targets a range of values
        /// </summary>
        /// <param name="imagePath">The path to the image used for the graphic</param>
        /// <param name="targetRange">The range of values targeted by the <see cref="StatusGraphic"/></param>
        public StatusGraphic(string imagePath, ValueRange targetRange) {
            _graphicPath = imagePath;
            _isRange = true;
            _targetRange = targetRange;
        }

        /// <summary>
        /// The text displayed when the associated <see cref="HsFeature"/>'s <see cref="HsFeature.Value"/> field matches
        ///  the <see cref="Value"/> or <see cref="TargetRange"/>.
        /// <para>
        /// Leaving this blank will cause the StatusControl's Label field to be used instead
        /// </para>
        /// <para>
        /// Set this to a single space " " to ensure that the corresponding status is left blank when displayed
        /// </para>
        /// </summary>
        public string Label {
            get => _label;
            set => _label = value;
        }
        
        /// <summary>
        /// What the <see cref="StatusGraphic"/> is used for.
        ///  See <see cref="EControlUse"/> for more information.
        /// </summary>
        /// <seealso cref="EControlUse"/>
        public EControlUse ControlUse {
            get => _controlUse;
            set => _controlUse = value;
        }

        /// <summary>
        /// The path to an image displayed by the associated <see cref="HsFeature"/> when its
        ///  <see cref="HsFeature.Value"/> field matches the <see cref="Value"/> or <see cref="TargetRange"/>
        ///  on this <see cref="StatusGraphic"/>
        /// </summary>
        public string Graphic {
            get => _graphicPath;
            set => _graphicPath = value;
        }

        /// <summary>
        /// Whether the <see cref="StatusGraphic"/> targets a range of values or a single value.
        /// <para>
        /// Settings this to TRUE will cause the <see cref="Value"/> field to be ignored in favor of
        ///  the <see cref="TargetRange"/>
        /// </para>
        /// </summary>
        public bool IsRange {
            get => _isRange;
            set => _isRange = value;
        }
        
        /// <summary>
        /// Whether the <see cref="StatusGraphic"/> label includes additional data tokens to be replaced by strings
        ///  in <see cref="HsFeature.AdditionalStatusData"/>
        /// </summary>
        public bool HasAdditionalData {
            get => _hasAdditionalData;
            set => _hasAdditionalData = value;
        }
        
        /// <summary>
        /// The range of values that the <see cref="StatusGraphic"/> targets.
        /// <para>
        /// If <see cref="IsRange"/> is FALSE then this is ignored in favor of <see cref="Value"/>
        /// </para>
        /// </summary>
        /// <seealso cref="ValueRange"/>
        public ValueRange TargetRange {
            get => _targetRange;
            set => _targetRange = value;
        }

        /// <summary>
        /// The minimum value targeted by the <see cref="StatusGraphic"/>
        /// </summary>
        public double RangeMin {
            get => _targetRange?.Min ?? 0;
            set => _targetRange.Min = value;
        }

        /// <summary>
        /// The maximum value targeted by the <see cref="StatusGraphic"/>
        /// </summary>
        public double RangeMax {
            get => _targetRange?.Max ?? 0;
            set => _targetRange.Max = value;
        }

        /// <summary>
        /// The value this StatusControl targets.
        /// <para>
        /// If <see cref="IsRange"/> is TRUE then this is ignored in favor of <see cref="TargetRange"/>
        /// </para>
        /// </summary>
        public double Value {
            get => _value;
            set => _value = value;
        }

        /// <summary>
        /// Create a deep copy of this <see cref="StatusGraphic"/>
        /// </summary>
        /// <returns>The deep copy of this <see cref="StatusGraphic"/></returns>
        public StatusGraphic Clone()
        {
            var clone = new StatusGraphic(Graphic, Value);
            clone.TargetRange = TargetRange.Clone();
            clone.IsRange = IsRange;
            clone.Label = Label;
            clone.ControlUse = ControlUse;
            clone.HasAdditionalData = HasAdditionalData;
            return clone;
        }

        /// <summary>
        /// Get the target value of the <see cref="StatusGraphic"/> based on whether it is a range or not
        /// </summary>
        /// <returns>
        /// Returns <see cref="StatusGraphic.RangeMin"/> if it is a range and returns <see cref="StatusGraphic.Value"/> if it isn't
        /// </returns>
        public double GetTargetValue()
        {
            return IsRange ? RangeMin : Value;
        }
        
        /// <summary>
        /// Get the label for the specified value correctly formatted according to the <see cref="StatusGraphic"/>'s
        ///  configuration
        /// </summary>
        /// <param name="value">The value to get the label for</param>
        /// <param name="additionalData">
        /// Additional data to include in the status label that replaces any tokens from
        ///  <see cref="HsFeature.GetAdditionalDataToken"/> included in the status.
        /// </param>
        /// <returns>The value as a string formatted according to the <see cref="TargetRange"/> configuration.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not targeted by the <see cref="StatusGraphic"/></exception>
        public string GetLabelForValue(double value, string[] additionalData = null) {
            var label = _label;
            if (!string.IsNullOrEmpty(label)) {
                //If there is a label defined
                if (additionalData == null || !_hasAdditionalData) {
                    return label;
                }
                label = ReplaceAdditionalData(label, additionalData);
                return label;
            }
            //No label is defined
            if (!_isRange) {
                return additionalData == null || !_hasAdditionalData ? 
                    _label : 
                    ReplaceAdditionalData(_label, additionalData);
            }
            if (!_targetRange.IsValueInRange(value)) {
                throw new ArgumentOutOfRangeException(nameof(value), $"{value} is not valid for this graphic");
            }

            return additionalData == null || !_hasAdditionalData ? 
                _targetRange.GetStringForValue(value) : 
                ReplaceAdditionalData(_targetRange.GetStringForValue(value), additionalData);
        }

        /// <summary>
        /// Try to get the label for the specified value correctly formatted according to the
        ///  <see cref="StatusGraphic"/>'s configuration.
        /// </summary>
        /// <param name="label">The string variable the label will be written to</param>
        /// <param name="value">The value to get the label for</param>
        /// <param name="additionalData">
        /// Additional data to include in the status label that replaces any tokens from
        ///  <see cref="HsFeature.GetAdditionalDataToken"/> included in the status.
        /// </param>
        /// <returns>
        /// TRUE if a label is available for the <see cref="StatusGraphic"/>,
        ///  FALSE if the value is not valid for this <see cref="StatusGraphic"/> or there is no label defined.
        /// </returns>
        public bool TryGetLabelForValue(out string label, double value, string[] additionalData = null) {
            
            if (!string.IsNullOrEmpty(_label)) {
                //If there is a label defined
                label = _label;
                if (additionalData == null || !_hasAdditionalData) {
                    return true;
                }
                label = ReplaceAdditionalData(label, additionalData);
                return true;
            }
            //No label is defined
            if (!_isRange || _targetRange == null) {
                //If the graphic is for one value
                label = null;
                //Label is null or empty based on previous condition
                return false;
            }
            //No label is defined and it is a range
            if (!_targetRange.IsValueInRange(value)) {
                //Value is not in range so the label is null
                label = null;
                return false;
            }
            //No label is defined, it is a range, and the current value is in that range
            label = _targetRange.GetStringForValue(value);
            if (additionalData == null || !_hasAdditionalData) {
                //Additional data is not being used - return as is
                return true;
            }
            //Additional data is being used
            label = ReplaceAdditionalData(label, additionalData);
            return true;
        }

        /// <summary>
        /// Determine if a specified value is targeted by the <see cref="StatusGraphic"/>"/>
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>
        /// TRUE if the value is targeted by the <see cref="StatusGraphic"/>,
        ///  FALSE if it is not
        /// </returns>
        public bool IsValueInRange(double value) {
            if (_isRange) {
                return _targetRange.IsValueInRange(value);
            }

            return Math.Abs(_value - value) < 1E-20;
        }

        private string ReplaceAdditionalData(string label, string[] additionalData) {

            var finalLabel = label;

            for (var i = 0; i < additionalData.Length; i++) {
                finalLabel = finalLabel.Replace(HsFeature.GetAdditionalDataToken(i), additionalData[i]);
            }

            return finalLabel;
        }

        /// <summary>
        /// Compare this object with another to see if they are equal
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>True if they are equal, False if they are not</returns>
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
            if (_targetRange != otherSg._targetRange) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get the hash code
        /// </summary>
        /// <returns>A hash code based on the <see cref="TargetRange.Min"/> if <see cref="IsRange"/> is true or <see cref="Value"/> if it is false.</returns>
        public override int GetHashCode() {
            return _isRange ? _targetRange.Min.GetHashCode() : _value.GetHashCode();
        }

    }

}