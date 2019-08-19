using System;

namespace HomeSeer.PluginSdk.Devices.Controls {

    //TODO preset locations
    /// <summary>
    /// The location and size of a control available on an <see cref="HsFeature"/>.
    /// <para>
    /// It describes the row and column a control occupies in the grid as well as how many columns wide it is.
    /// </para>
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class ControlLocation {

        private int _row;
        private int _column;
        private int _width;

        /// <summary>
        /// Initialize a new location with a row and column position of 0 and a width of 1 column.
        /// </summary>
        public ControlLocation() {
            _row = 0;
            _column = 0;
            _width = 1;
        }

        /// <summary>
        /// The row the control is located in.
        /// <para>
        /// This is a 0 based index starting from the top and going down.
        /// </para>
        /// </summary>
        public int Row {
            get => _row;
            set => _row = value < 0 ? 0 : value;
        }

        /// <summary>
        /// The column the control is located in.
        /// <para>
        /// This is a 0 based index starting from the left side and going right.
        /// </para>
        /// </summary>
        public int Column {
            get => _column;
            set => _column = value < 0 ? 0 : value;
        }

        /// <summary>
        /// The number of columns the control takes up
        /// <para>
        /// Must be between 1 and 4.
        /// </para>
        /// </summary>
        public int Width {
            get => _width;
            set {
                if (value < 1) {
                    _width = 1;
                }
                else if (value > 4) {
                    _width = 4;
                }
                else {
                    _width = value;
                }
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            if (!(obj is ControlLocation otherLocation)) {
                return false;
            }

            if (_row != otherLocation._row) {
                return false;
            }
            if (_column != otherLocation._column) {
                return false;
            }
            if (_width != otherLocation._width) {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return _row.GetHashCode() * _column.GetHashCode() * _width.GetHashCode();
        }

    }

}