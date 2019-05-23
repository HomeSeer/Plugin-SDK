using System;

namespace HomeSeer.PluginSdk.Energy {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class EnergyCalcData {

        public TimeSpan Range;            // The amount of time to be included in the calculation starting from the starting point.
        public TimeSpan StartBack;        // The amount of time to be subtracted from NOW to get our starting point.
        public bool     RoundDay = false; // Whether to round the time to an even day.

        private string mvarName = "";
        public string Name {
            get {
                if (string.IsNullOrEmpty(mvarName)) {
                    mvarName = "";
                }

                return mvarName;
            }
            set {
                if (string.IsNullOrWhiteSpace(value)) {
                    mvarName = "";
                    return;
                }
                
                mvarName = value.Trim();
            }
        }

        public DateTime LateDate = DateTime.MinValue;
        public double mvAmount;
        public double Amount => Math.Round(mvAmount, 3);

        public double AmountPrecise => mvAmount;

        public double mvCost;
        public double Cost => Math.Round(mvCost, 2);

        public double CostPrecise => mvCost;

    }

}