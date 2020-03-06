using System;

namespace HomeSeer.PluginSdk.Energy {

    /// <summary>
    /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
    ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
    ///  significant changes in the near future. Please use with caution.
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class EnergyData {

        public int ID;
        /// <summary>
        /// Device reference ID number for the device this energy data is for.
        /// </summary>
        public int dvRef;
        /// <summary>
        /// Indicates whether the energy was consumed (used) or produced (created).
        /// </summary>
        public Constants.enumEnergyDirection Direction = Constants.enumEnergyDirection.Consumed;
        /// <summary>
        /// Always measured in Watts
        /// </summary>
        public double Amount;
        /// <summary>
        /// The start of the time period this measurement is for.
        /// </summary>
        public DateTime Amount_Start = DateTime.MinValue;
        /// <summary>
        /// The end of the time period this measurement is for.
        /// </summary>
        public DateTime Amount_End = DateTime.MinValue;
        /// <summary>
        /// When the data is compacted, time periods missed (perhaps HS was shut down) end up having the actual amounts averaged across the missing time period.
        /// <para>
        /// For example, if you log 500W at 1:00PM through 1:05PM, then HS was shut down for 5 minutes, 
        /// then log 500W from 1:10PM through 1:15PM, then when data compaction happens and 
        /// the records are consolidated, the only thing that could happen is to show 1000W
        /// from 1:00PM through 1:15PM, which is accurate, but does not provide evidence of
        /// the fact that there was a 5 minute period when data was not collected.  Furthermore,
        /// calculations of the COST which must include the time to get to kWh will be adversely
        /// affected (more for electricity produced than consumed!).  This duration field 
        /// takes care of that NOT by providing any record of the missing periods, but by 
        /// at least keeping track of the total time within the consolidated (compacted) 
        /// record which only reflects the start period and end period.
        /// </para>
        /// </summary>
        public long Duration = 0;
        /// <summary>
        /// Always measured in kWH
        /// </summary>
        public float Rate;
        /// <summary>
        /// For the user to indicate something about this reading.
        /// </summary>
        public int UserCode;
        /// <summary>
        /// Indicates the type of device.
        /// </summary>
        public Constants.enumEnergyDevice Device = Constants.enumEnergyDevice.Other;
        
        public EnergyData(Constants.enumEnergyDirection consumedOrProduced) {
            Direction = consumedOrProduced;
        }

    }

}