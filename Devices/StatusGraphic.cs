using System;

namespace HomeSeer.PluginSdk {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class StatusGraphic {

        private string       _graphicPath = "";
        private double       _value;
        public  double       _rangeStart;
        public  double       _rangeEnd;
        public string Graphic
        {
            set
            {
                _graphicPath = value;
            }
        }
        public double Value
        {
            get
            {
                return _value;
                /*if (this.PairType == Constants.VSVGPairType.SingleValue)
                    return _value;
                else
                    return _rangeStart;*/
            }
        }
        public double Set_Value {
            set
            {
                _value = value;
            }
        }

        public string GetGraphic(double Value) {
            /*if (PairType != Constants.VSVGPairType.Range)
                return _graphicPath;
            if ((Value < _rangeStart) | (Value > _rangeEnd))
                return "";*/
            return _graphicPath;
        }

    }

}