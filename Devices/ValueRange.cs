using System;
using System.Reflection;
using HomeSeer.PluginSdk.Devices.Controls;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// A range of values that can be targeted by <see cref="StatusControl"/>s and <see cref="StatusGraphic"/>s
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class ValueRange {
        
        private int    _decimalPlaces;
        private double _min;
        private double _max;
        private double _offset;
        private string _prefix = "";
        private string _suffix = "";
        private double _divisor = 1;

        /// <summary>
        /// Initialize a new range of values
        /// </summary>
        /// <param name="min">The smallest value permitted</param>
        /// <param name="max">The largest value permitted</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="min"/> is less than <paramref name="max"/></exception>
        public ValueRange(double min, double max) {
            if (min > max) {
                throw new ArgumentException($"Min {min} must be less than Max {max}");
            }
            _min = min;
            _max = max;

            if (min % 1 != 0 || max % 1 != 0) {
                _decimalPlaces = 2;
            }
            else {
                _decimalPlaces = 0;
            }
        }

        /// <summary>
        /// The minimum value permitted by the range
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the new minimum value is larger than the current maximum
        /// </exception>
        public double Min {
            get => _min;
            set {
                if (_max != 0 && value > _max) throw new ArgumentOutOfRangeException();

                _min = value;
            }
        }

        /// <summary>
        /// The maximum value permitted by the range
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if the new maximum value is smaller than the current minimum
        /// </exception>
        public double Max {
            get => _max;
            set {
                if (_min != 0 && value < _min) throw new ArgumentOutOfRangeException();

                _max = value;
            }
        }

        /// <summary>
        /// The amount to subtract from the value for display.
        /// <para>
        /// E.G. A value of 501 will be displayed as 1 with an offset of 500
        /// </para>
        /// </summary>
        public double Offset {
            get => _offset;
            set => _offset = value;
        }

        /// <summary>
        /// The number of decimal places of accuracy displayed by the range
        /// <para>
        /// The displayed value will be the rounded value to the nearest number of decimals. 
        /// E.G. If <see cref="DecimalPlaces"/> is 1, a value of 7.1589  will be displayed as 7.2
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if you try to set the decimal places to a value less than 0
        /// </exception>
        public int DecimalPlaces {
            get => _decimalPlaces;
            set {
                if (value < 0) throw new ArgumentOutOfRangeException();

                _decimalPlaces = value;
            }
        }

        /// <summary>
        /// A text prefix to include with the value when displayed
        /// </summary>
        public string Prefix {
            get => _prefix;
            set => _prefix = string.IsNullOrWhiteSpace(value) ? "" : value;
        }

        /// <summary>
        /// A text suffix to include with the value when displayed
        /// </summary>
        public string Suffix {
            get => _suffix;
            set => _suffix = string.IsNullOrWhiteSpace(value) ? "" : value;
        }

        /// <summary>
        /// The amount to divide the value by for display.
        /// <para>
        /// E.G. A value of 45 will be displayed as 22.5 with a divisor of 2
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if you try to set the divisor less than or equal to 0
        /// </exception>
        public double Divisor {
            get => _divisor;
            set {
                if (value <= 0) throw new ArgumentOutOfRangeException();

                _divisor = value;
            }
        }

        /// <summary>
        /// Create a deep copy of this <see cref="ValueRange"/>
        /// </summary>
        /// <returns>The deep copy of this <see cref="ValueRange"/></returns>
        public ValueRange Clone()  {
            var clone = new ValueRange(Min, Max) 
                        {
                            DecimalPlaces = DecimalPlaces, 
                            Offset = Offset, 
                            Prefix = Prefix, 
                            Suffix = Suffix,
                            Divisor = Divisor
                        };
            return clone;
        }

        /// <summary>
        /// Obtain the string representation of the specified value according to the range's configuration
        /// </summary>
        /// <remarks>
        /// This returns <c>$"{_prefix}{((value - _offset) / _divisor).ToString($"F{_decimalPlaces}")}{_suffix}"</c>
        /// </remarks>
        /// <param name="value">The value to use in the string</param>
        /// <returns>The value correctly formatted according to the range</returns>
        public string GetStringForValue(double value) {
            var stringValue = $"{_prefix}{((value - _offset) / _divisor).ToString($"F{_decimalPlaces}")}{_suffix}";

            return stringValue;
        }

        /// <summary>
        /// Determine if the specified value is valid for this range
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns>
        /// TRUE is it is valid for the range,
        ///  FALSE if it is not
        /// </returns>
        public bool IsValueInRange(double value) {
            return (_min - value) < 1E-15 && (value - _max) < 1E-15;
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

            if (!(obj is ValueRange otherValueRange)) {
                return false;
            }

            if (Math.Abs(_min - otherValueRange._min) > 1E-15) {
                return false;
            }
            if (Math.Abs(_max - otherValueRange._max) > 1E-15) {
                return false;
            }
            if (_prefix != otherValueRange._prefix) {
                return false;
            }
            if (_suffix != otherValueRange._suffix) {
                return false;
            }
            if (_decimalPlaces != otherValueRange._decimalPlaces) {
                return false;
            }
            if (_divisor != otherValueRange._divisor) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get the hash code
        /// </summary>
        /// <returns>A hash code based on the <see cref="Min"/> and <see cref="Max"/> value</returns>
        public override int GetHashCode() {
            return _min.GetHashCode() * _max.GetHashCode();
        }

    }

}