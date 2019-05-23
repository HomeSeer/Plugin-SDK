using System;

namespace HomeSeer.PluginSdk.Energy {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class EnergyGraph {

        public int      ID;
        public string   Name                 = "Energy";
        public int      DefaultGraph;
        public int      IsLocked             = 0;
        public int      IsVisible            = 1;
        public int      MultiLayered         = 0;
        // not saved..., passed in as needed
        public int      YAxis;
        public string   dvRefs;
        public int      Width                = 950;
        public int      Height               = 300;
        public string   AdditionalParameters = "#DCDCFF,#FAFAD2,#00008B,#FF0000,45,#000000,#000000,#000000,#000000,#000000";
        public string   LinkedIDs;
        public DateTime DateGenerated        = DateTime.Now;

    }

}