using System;

namespace HomeSeer.PluginSdk.CAPI {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable()]
    public class ValueRange {

        public  double RangeStart;
        public  double RangeEnd;
        public  int    RangeStatusDecimals;
        private string mvarRangeStatusPrefix = "";
        private string mvarRangeStatusSuffix = "";
        public string RangeStatusPrefix
        {
            get => mvarRangeStatusPrefix;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    mvarRangeStatusPrefix = "";
                    return;
                }
                if (string.IsNullOrEmpty(value.Trim()))
                {
                    mvarRangeStatusPrefix = "";
                    return;
                }
                mvarRangeStatusPrefix = value;
            }
        }
        public string RangeStatusSuffix
        {
            get
            {
                return mvarRangeStatusSuffix;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    mvarRangeStatusSuffix = "";
                    return;
                }
                if (string.IsNullOrEmpty(value.Trim()))
                {
                    mvarRangeStatusSuffix = "";
                    return;
                }
                mvarRangeStatusSuffix = value;
            }
        }
        public double RangeStatusValueOffset;
        public double RangeStatusDivisor;
        public string ScaleReplace = "";
        public bool   HasScale     = false;

    }

}