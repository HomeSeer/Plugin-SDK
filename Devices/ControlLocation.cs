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

    }

}