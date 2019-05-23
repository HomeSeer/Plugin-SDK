using System;

namespace HomeSeer.PluginSdk.CAPI {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class CAPIControl {
        
        private string mvarControlLabel = "";
        private Constants.CAPIControlType mvarControlType;
        private CAPIControlLocation mvarControlLocation;
        private double mvarControlValue;
        private string mvarControlString = "";
        private string[] mvarControlStringList = null;
        private string[] mvarControlStringSelected = null;
        private bool mvarControlFlag;
        private int mvarRef;
        private int mvarIndex = -1;
        private ValueRange mvarRange = null;
        private Constants.ePairControlUse mvarControlUse = Constants.ePairControlUse.Not_Specified;
    
        public bool Do_Update = true;  // Set to False to prevent CommitDeviceStatus from triggering events.
        public bool SingleRangeEntry;  // TRACKS (does not SET) the value of SingleRangeEntry when the control (and thus Index) was generated.
        
        public int CCIndex {
            get => mvarIndex;
            set => mvarIndex = value;
        }
        
        public ValueRange Range {
            get => mvarRange;
            set {
                if (value == null) {
                    mvarRange = null;
                    return;
                }

                mvarRange = new ValueRange
                            {
                                RangeStart             = value.RangeStart,
                                RangeEnd               = value.RangeEnd,
                                RangeStatusDecimals    = value.RangeStatusDecimals,
                                RangeStatusPrefix      = value.RangeStatusPrefix,
                                RangeStatusSuffix      = value.RangeStatusSuffix,
                                RangeStatusValueOffset = value.RangeStatusValueOffset
                            };
            }
        }
        // note, to deserialize this property, it has to be writable, so any app referencing this will need to reference a copy of this
        public int Ref => mvarRef;

        public string Label {
            get => mvarControlLabel;
            set => mvarControlLabel = value;
        }
        public Constants.CAPIControlType ControlType {
            get => mvarControlType;
            set => mvarControlType = value;
        }
        public Constants.CAPIControlButtonImage ControlButtonType = Constants.CAPIControlButtonImage.Not_Specified;
        public string ControlButtonCustom = "";
    
        public CAPIControlLocation ControlLocation {
            get => mvarControlLocation;
            set => mvarControlLocation = value;
        }
        public int ControlLoc_Row {
            get => mvarControlLocation.Row;
            set => mvarControlLocation.Row = value;
        }
        public int ControlLoc_Column {
            get => mvarControlLocation.Column;
            set => mvarControlLocation.Column = value;
        }
        public int ControlLoc_ColumnSpan {
            get => mvarControlLocation.ColumnSpan;
            set => mvarControlLocation.ColumnSpan = value;
        }
        public Constants.ePairControlUse ControlUse {
            get => mvarControlUse;
            set => mvarControlUse = value;
        }
        public double ControlValue {
            get => mvarControlValue;
            set => mvarControlValue = value;
        }
        public string ControlString {
            get => mvarControlString;
            set {
                if (value == null) {
                    mvarControlString = "";
                    return;
                }
                mvarControlString = value;
            }
        }
        public string[] ControlStringList {
            get => mvarControlStringList;
            set => mvarControlStringList = value;
        }
        public string[] ControlStringSelected {
            get => mvarControlStringSelected;
            set => mvarControlStringSelected = value;
        }
        public bool ControlFlag {
            get => mvarControlFlag;
            set => mvarControlFlag = value;
        }
    
        public CAPIControl(int dvRef) {
            mvarRef = dvRef;
            mvarControlLocation = new CAPIControlLocation {Row = 1, Column = 1};
        }
    }

}