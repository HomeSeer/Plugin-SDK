using System;

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// Status graphics are used to specify what image is displayed for a device when its value matches certain criteria
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class StatusGraphic {

        private string       _label = "";
        private bool         _isRange;
        private string       _graphicPath;
        private double       _value;
        private ValueRange   _targetRange = new ValueRange(0,0);
        
        /// <summary>
        /// Initialize a new StatusGraphic with the specified image that targets a single value
        /// </summary>
        /// <param name="imagePath">The path to the image used for the graphic</param>
        /// <param name="targetValue">The value targeted by the <see cref="StatusGraphic"/></param>
        public StatusGraphic(string imagePath, double targetValue) {
            _graphicPath = imagePath;
            _value = targetValue;
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
        /// The text displayed when the associated <see cref="HsFeature"/>'s <see cref="HsFeature.Value"/> field matches
        ///  the <see cref="Value"/> or <see cref="TargetRange"/>.
        /// <para>
        /// Leaving this blank will cause the StatusControl's Label field to be used instead
        /// </para>
        /// </summary>
        public string Label {
            get => _label;
            set => _label = value;
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
        /// Get the label for the specified value correctly formatted according to the <see cref="StatusGraphic"/>'s
        ///  configuration
        /// </summary>
        /// <param name="value">The value to get the label for</param>
        /// <returns>The value as a string formatted according to the <see cref="TargetRange"/> configuration.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not targeted by the <see cref="StatusGraphic"/></exception>
        public string GetLabelForValue(double value) {
            if (!_isRange) {
                return _label;
            }
            if (!_targetRange.IsValueInRange(value)) {
                throw new ArgumentOutOfRangeException(nameof(value), $"{value} is not valid for this graphic");
            }

            return _targetRange.GetStringForValue(value);
        }

        /// <summary>
        /// Try to get the label for the specified value correctly formatted according to the
        ///  <see cref="StatusGraphic"/>'s configuration.
        /// </summary>
        /// <param name="label">The string variable the label will be written to</param>
        /// <param name="value">The value to get the label for</param>
        /// <returns>
        /// TRUE if a label is available for the <see cref="StatusGraphic"/>,
        ///  FALSE if the value is not valid for this <see cref="StatusGraphic"/>
        /// </returns>
        public bool TryGetLabelForValue(out string label, double value) {
            if (!_isRange || _targetRange == null) {
                label = _label;
                return true;
            }
            if (!_targetRange.IsValueInRange(value)) {
                label = null;
                return false;
            }

            label = _targetRange.GetStringForValue(value);
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

            return Math.Abs(_value - value) < 0.01D;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override int GetHashCode() {
            return _isRange ? _targetRange.Min.GetHashCode() : _value.GetHashCode();
        }

    }

}