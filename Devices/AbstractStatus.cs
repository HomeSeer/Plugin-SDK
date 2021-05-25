using System;
using System.Reflection;
using HomeSeer.PluginSdk.Devices.Controls;

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// The base implementation of a status defined for a <see cref="HsFeature"/> either as a
    /// <see cref="StatusControl"/> or <see cref="StatusGraphic"/>
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public abstract class AbstractStatus {

        private string     _label = "";
        private double     _targetValue;
        private bool       _isRange;
        private ValueRange _targetRange = new ValueRange(0,0);
        
        /// <summary>
        /// The label displayed for this status
        /// </summary>
        public string Label {
            get => _label;
            set => _label = value;
        }
        
        /// <summary>
        /// Whether this status is targeted by a range of values instead of a single value
        /// </summary>
        public bool IsRange {
            get => _isRange;
            set => _isRange = value;
        }
        
        /// <summary>
        /// The range of values the status targets
        /// </summary>
        /// <seealso cref="ValueRange"/>
        public ValueRange TargetRange {
            get => _targetRange;
            set => _targetRange = value;
        }
        
        /// <summary>
        /// The minimum value targeted by the <see cref="TargetRange"/>
        /// </summary>
        public double RangeMin {
            get => _targetRange?.Min ?? 0;
            set => _targetRange.Min = value;
        }

        /// <summary>
        /// The maximum value targeted by the <see cref="TargetRange"/>
        /// </summary>
        public double RangeMax {
            get => _targetRange?.Max ?? 0;
            set => _targetRange.Max = value;
        }

        /// <summary>
        /// The value the status targets.
        /// <para>
        /// If <see cref="IsRange"/> is TRUE then this is ignored in favor of <see cref="TargetRange"/>
        /// </para>
        /// </summary>
        public double Value {
            get => _targetValue;
            set => _targetValue = value;
        }
        
        /// <summary>
        /// Get the label for the specified value correctly formatted according to the status's configuration
        /// </summary>
        /// <param name="value">The value to get the label for</param>
        /// <returns>The value as a string formatted according to the <see cref="TargetRange"/> configuration.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not targeted by the status</exception>
        public string GetLabelForValue(double value) {
            if (!_isRange) {
                return _label;
            }
            if (!_targetRange.IsValueInRange(value)) {
                throw new ArgumentOutOfRangeException(nameof(value), $"{value} is not valid for this status");
            }

            return _targetRange.GetStringForValue(value);
        }

        /// <summary>
        /// Try to get the label for the specified value correctly formatted according to the
        ///  status's configuration.
        /// </summary>
        /// <param name="label">The string variable the label will be written to</param>
        /// <param name="value">The value to get the label for</param>
        /// <returns>
        /// TRUE if a label is available for the status,
        ///  FALSE if the value is not valid for this status or there is no label defined.
        /// </returns>
        public bool TryGetLabelForValue(out string label, double value) {
            if (string.IsNullOrWhiteSpace(_label)) {
                label = null;
                return false;
            }
            
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
        /// Determine if a specified value is targeted by the status
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>
        /// TRUE if the value is targeted by the status,
        ///  FALSE if it is not
        /// </returns>
        public bool IsValueInRange(double value) {
            if (_isRange) {
                return _targetRange.IsValueInRange(value);
            }

            return Math.Abs(_targetValue - value) < 1E-20;
        }

    }

}