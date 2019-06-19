using System;
using System.Reflection;
using HomeSeer.PluginSdk.CAPI;

namespace HomeSeer.PluginSdk {

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class DeviceButtonScript {

        private CAPIControlLocation mvarButtonLocation;
        private string              mvarLabel       = "";
        private string              mvarScriptFile  = "";
        private string              mvarScriptFunc  = "";
        private string              mvarScriptParam = "";
        private double              mvarValue;

        public DeviceButtonScript() {
            mvarButtonLocation.Column     = 1;
            mvarButtonLocation.Row        = 1;
            mvarButtonLocation.ColumnSpan = 1;
        }

        public double Value {
            get => mvarValue;
            set => mvarValue = value;
        }


        public string Label {
            get {
                if (mvarLabel == null)
                    mvarLabel = "";
                return mvarLabel;
            }
            set {
                if (value == null)
                    value = "";
                mvarLabel = value;
            }
        }

        public string ScriptFile {
            get {
                if (mvarScriptFile == null)
                    mvarScriptFile = "";
                return mvarScriptFile;
            }
            set {
                if (value == null)
                    value = "";
                mvarScriptFile = value;
            }
        }

        public string ScriptFunc {
            get {
                if (mvarScriptFunc == null)
                    mvarScriptFunc = "";
                return mvarScriptFunc;
            }
            set {
                if (value == null)
                    value = "";
                mvarScriptFunc = value;
            }
        }

        public string ScriptParam {
            get {
                if (mvarScriptParam == null)
                    mvarScriptParam = "";
                return mvarScriptParam;
            }
            set {
                if (value == null)
                    value = "";
                mvarScriptParam = value;
            }
        }

        public CAPIControlLocation ButtonLocation => mvarButtonLocation;

        public int ButtonRow {
            get => mvarButtonLocation.Row;
            set => mvarButtonLocation.Row = value;
        }

        public int ButtonColumn {
            get => mvarButtonLocation.Column;
            set => mvarButtonLocation.Column = value;
        }

        public int ButtonColumnSpan {
            get => mvarButtonLocation.ColumnSpan;
            set => mvarButtonLocation.ColumnSpan = value;
        }

    }

}