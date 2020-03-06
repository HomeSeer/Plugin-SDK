using System;

namespace HomeSeer.PluginSdk.Energy {

    /// <summary>
    /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
    ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
    ///  significant changes in the near future. Please use with caution.
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class EnergyCalcData {

        /// <summary>
        /// The amount of time to be included in the calculation starting from the starting point.
        /// </summary>
        public TimeSpan Range;
        /// <summary>
        /// The amount of time to be subtracted from NOW to get our starting point.
        /// </summary>
        public TimeSpan StartBack;
        /// <summary>
        /// Whether to round the time to an even day.
        /// </summary>
        public bool     RoundDay = false;

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