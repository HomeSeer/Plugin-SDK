using System;

namespace HomeSeer.PluginSdk.Energy {

    /// <summary>
    /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
    ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
    ///  significant changes in the near future. Please use with caution.
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class EnergyGraphData {

        public int                      ID;
        public string                   Name = "Energy";
        public Constants.eGraphType     Type;
        public Constants.eGraphInterval Interval;
        public DateTime?                StartDate;
        public DateTime?                EndDate;
        public int                      GoBack;
        public Constants.eGraphInterval gbInterval;
        public int                      Timespan;
        public Constants.eGraphInterval tsInterval;
        public string                   dvRefs;

        /// <summary>
        /// FirstOf is really a boolean, but for database compatibility, we'll use integer.
        /// <para>
        /// If true, goes to the first instance of the gbInterval e.g: 
        /// if a day, to the beginning of the day, 
        /// if a week, to the beginning of the week.
        /// </para>
        /// </summary>
        public int FirstOf = 1;

        /// <summary>
        /// DynamicCalc is really a boolean, but for database compatibility, we'll use integer.
        /// <para>
        /// This can be inferred, but is added to explicitly show whether to use static dates or calc them out.
        /// </para>
        /// </summary>
        public int DynamicCalc;

        public int Width        = 950;
        public int Height       = 300;
        public int MobileWidth  = 300;
        public int MobileHeight = 200;
        public int DefaultGraph;
        public int YAxis;
        public int IsLocked = 0;
        public int IsVisible = 0;

        // not saved..., passed in as needed
        /// <summary>
        /// back_color,pane_color,bar1_color,bar2_color,use_gradient,font_color,pane_color_left,
        /// pane_color_bottom,pane_color_top,pane_color_right
        /// </summary>
        public string AdditionalParameters =
            "#DCDCFF,#FAFAD2,#00008B,#FF0000,45,#000000,#000000,#000000,#000000,#000000";


        public EnergyGraphData() {
            StartDate = default(DateTime?);
            EndDate   = default(DateTime?);
        }

    }

}