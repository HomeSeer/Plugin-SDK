using System;

namespace HomeSeer.PluginSdk {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class VGPair {

        public  Constants.VSVGPairType PairType;
        private string       mvarGraphic = "";
        private double       mvarValue;
        public  double       RangeStart;
        public  double       RangeEnd;
        public string Graphic
        {
            set
            {
                mvarGraphic = value;
            }
        }
        public double Value
        {
            get
            {
                if (this.PairType == Constants.VSVGPairType.SingleValue)
                    return mvarValue;
                else
                    return RangeStart;
            }
        }
        public double Set_Value
        {
            set
            {
                mvarValue = value;
            }
        }
        public string GetGraphic(double Value)
        {
            if (PairType != Constants.VSVGPairType.Range)
                return mvarGraphic;
            if ((Value < RangeStart) | (Value > RangeEnd))
                return "";
            return mvarGraphic;
        }

        private int mvarPairProtection; // ePairProtection = ePairProtection.Off
        public int Protection
        {
            get
            {
                return mvarPairProtection;
            }
        } // ePairProtection
        public int ProtectionSet
        {
            set
            {
                mvarPairProtection = value;
            }
        } // ePairProtection

    }

}