using System;

namespace HomeSeer.PluginSdk.Devices {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class ControlLocation {

        private int _row;
        private int _column;
        private int _width;

        public int Row {
            get => _row;
            set => _row = value < 0 ? 0 : value;
        }

        public int Column {
            get => _column;
            set => _column = value < 0 ? 0 : value;
        }

        public int Width {
            get => _width;
            set => _width = value < 1 ? 1 : value;
        }

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

        public override int GetHashCode() {
            return _row.GetHashCode() * _column.GetHashCode() * _width.GetHashCode();
        }

    }

}