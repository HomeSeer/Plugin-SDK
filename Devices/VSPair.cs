using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using HomeSeer.PluginSdk.CAPI;

namespace HomeSeer.PluginSdk {

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class VSPair {

        public const string ScaleReplace      = "@S@";
        public       bool   HasAdditionalData = false;
        public       bool   HasScale          = false;
        public       bool   IncludeValues     = true;

        private Constants.ePairControlUse mvarControlUse = Constants.ePairControlUse.Not_Specified;

        // Private mvarPairProtection As Integer 'ePairProtection = ePairProtection.Off
        private Constants.CAPIControlType        mvarPairRender = Constants.CAPIControlType.Not_Specified;
        private List<string>                     mvarStringList;
        private double                           mvarValue;
        public  string                           PairButtonImage     = "";
        public  Constants.CAPIControlButtonImage PairButtonImageType = Constants.CAPIControlButtonImage.Not_Specified;

        public Constants.VSVGPairType PairType;
        public double                 RangeEnd;
        public double                 RangeStart;
        public int                    RangeStatusDecimals = 0;
        public double                 RangeStatusDivisor  = 0;
        public string                 RangeStatusPrefix   = "";
        public string                 RangeStatusSuffix   = "";
        public CAPIControlLocation    Render_Location;
        public double                 ValueOffset = 0;
        public bool                   ZeroPadding = false;

        public VSPair(Constants.ePairStatusControl Status_Control) {
            ControlStatus = Status_Control;
        }
        
        public Constants.ePairStatusControl ControlStatus { get; private set; }

        public Constants.ePairControlUse ControlUse {
            get => mvarControlUse;
            set => mvarControlUse = value;
        }

        public Constants.ePairStatusControl NewControlStatusUpdateForUtilityOnly {
            set => ControlStatus = value;
        }

        public Constants.CAPIControlType Render {
            get => mvarPairRender;
            set {
                if ((value == Constants.CAPIControlType.List_Text_from_List) |
                    (value == Constants.CAPIControlType.Single_Text_from_List)) {
                    if (ControlStatus == Constants.ePairStatusControl.Control) { }
                    else
                        // List and Single Text are Control-Only render types, so we need to make a change.
                    {
                        ControlStatus = Constants.ePairStatusControl.Control;
                    }
                }

                mvarPairRender = value;
            }
        }

        public string Status {
            set {
                if (value == null)
                    value = "";
                if (string.IsNullOrEmpty(value.Trim()))
                    intStatus = "";
                else
                    intStatus = value;
            }
            get => intStatus;
        }

        internal string intStatus { get; private set; } = "";

        public double Value {
            get {
                if (PairType == Constants.VSVGPairType.SingleValue)
                    return mvarValue;
                return RangeStart;
            }
            set => mvarValue = value;
        }

        public string[] StringList {
            get {
                if (mvarStringList == null)
                    return null;
                return mvarStringList.ToArray();
            }
            set {
                if (value == null) {
                    mvarStringList = null;
                    return;
                }

                if (value.Length < 1) {
                    mvarStringList = null;
                    return;
                }

                mvarStringList = new List<string>();
                foreach (var s in value) {
                    if (string.IsNullOrEmpty(s))
                        continue;
                    mvarStringList.Add(s);
                }
            }
        }

        public string StringListAdd {
            set {
                if (string.IsNullOrEmpty(value))
                    return;
                if (mvarStringList == null)
                    mvarStringList = new List<string>();
                mvarStringList.Add(value);
            }
        }

        public static string AddDataReplace(int Index) {
            return "@%" + Index + "@";
        }

        public string GetPairString(double Value, string Scale, string[] AdditionalData) {
            var s = GetPairString(Value);
            if (HasScale) {
                if (string.IsNullOrEmpty(Scale))
                    Scale = "";
                s = s.Replace(ScaleReplace, Scale);
            }

            if (HasAdditionalData) {
                var idx = -1;
                if (AdditionalData != null && AdditionalData.Length > 0)
                    for (var x = 0; x <= AdditionalData.Length - 1; x++) {
                        idx = x;
                        if (string.IsNullOrEmpty(AdditionalData[x]))
                            AdditionalData[x] = "";
                        s = s.Replace(AddDataReplace(x), AdditionalData[x]);
                    }

                if (idx < 9)
                    for (var x = idx + 1; x <= 9; x++)
                        s = s.Replace(AddDataReplace(x), "");
            }

            return s;
        }

        public double ReverseRangeStatusToControl(double Value) {
            if (PairType != Constants.VSVGPairType.Range)
                return mvarValue;
            if (RangeStatusDecimals < 1) {
                if (RangeStatusDivisor != 0)
                    return Convert.ToInt32((Value + ValueOffset) * RangeStatusDivisor);
                return Convert.ToInt32(Value + ValueOffset);
            }

            if (RangeStatusDivisor != 0)
                return Math.Round((Value + ValueOffset) * RangeStatusDivisor, RangeStatusDecimals);
            return Math.Round(Value + ValueOffset, RangeStatusDecimals);
        }

        private string GetPairString(double Value) {
            if (PairType != Constants.VSVGPairType.Range)
                return intStatus;
            if ((Value < RangeStart) | (Value > RangeEnd))
                return "";
            if (RangeStatusPrefix == null)
                RangeStatusPrefix = "";
            if (RangeStatusSuffix == null)
                RangeStatusSuffix = "";
            if (string.IsNullOrEmpty(RangeStatusPrefix.Trim()))
                RangeStatusPrefix = "";
            if (string.IsNullOrEmpty(RangeStatusSuffix.Trim()))
                RangeStatusSuffix = "";
            if (IncludeValues) {
                if (RangeStatusDecimals < 1) {
                    if (RangeStatusDivisor != 0)
                        return RangeStatusPrefix + Convert.ToInt32((Value - ValueOffset) / RangeStatusDivisor) +
                               RangeStatusSuffix;
                    return RangeStatusPrefix + Convert.ToInt32(Value - ValueOffset) + RangeStatusSuffix;
                }

                var deci = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
                if (ZeroPadding) {
                    var sVal  = "";
                    var addto = 0;
                    if (RangeStatusDivisor != 0)
                        sVal = Math.Round((Value - ValueOffset) / RangeStatusDivisor, RangeStatusDecimals).ToString();
                    else
                        sVal = Math.Round(Value - ValueOffset, RangeStatusDecimals).ToString();
                    if (sVal.Contains(deci)) {
                        var tmp = sVal.Split(deci.ToCharArray());
                        try {
                            if (tmp[1].Trim().Length < RangeStatusDecimals)
                                return RangeStatusPrefix + tmp[0] + deci +
                                       tmp[1].Trim().PadRight(RangeStatusDecimals, '0') + RangeStatusSuffix;
                            return RangeStatusPrefix + sVal + RangeStatusSuffix;
                        }
                        catch (Exception ex) {
                            return RangeStatusPrefix + sVal + RangeStatusSuffix;
                        }
                    }

                    return RangeStatusPrefix + sVal + RangeStatusSuffix;
                }

                if (RangeStatusDivisor != 0) {
                    var Check = Math.Round((Value - ValueOffset) / RangeStatusDivisor, RangeStatusDecimals).ToString();

                    return RangeStatusPrefix + Check + RangeStatusSuffix;
                }

                return RangeStatusPrefix + Math.Round(Value - ValueOffset, RangeStatusDecimals) + RangeStatusSuffix;
            }

            return RangeStatusPrefix + RangeStatusSuffix;
        }

        public string GetRangePairString(double Value, string Scale, string[] AdditionalData) {
            var s = GetRangePairString(Value);
            if (HasScale) {
                if (string.IsNullOrEmpty(Scale))
                    Scale = "";
                s = s.Replace(ScaleReplace, Scale);
            }

            if (HasAdditionalData) {
                var idx = -1;
                if (AdditionalData != null && AdditionalData.Length > 0)
                    for (var x = 0; x <= AdditionalData.Length - 1; x++) {
                        idx = x;
                        if (string.IsNullOrEmpty(AdditionalData[x]))
                            AdditionalData[x] = "";
                        s = s.Replace(AddDataReplace(x), AdditionalData[x]);
                    }

                if (idx < 9)
                    for (var x = idx + 1; x <= 9; x++)
                        s = s.Replace(AddDataReplace(x), "");
            }

            return s;
        }

        private string GetRangePairString(double Value) {
            if (PairType != Constants.VSVGPairType.Range)
                return intStatus;
            if ((Value < RangeStart) | (Value > RangeEnd))
                return "";
            if (RangeStatusPrefix == null)
                RangeStatusPrefix = "";
            if (RangeStatusSuffix == null)
                RangeStatusSuffix = "";
            if (string.IsNullOrEmpty(RangeStatusPrefix.Trim()))
                RangeStatusPrefix = "";
            if (string.IsNullOrEmpty(RangeStatusSuffix.Trim()))
                RangeStatusSuffix = "";
            return RangeStatusPrefix + "(value)" + RangeStatusSuffix;
        }

    }

}