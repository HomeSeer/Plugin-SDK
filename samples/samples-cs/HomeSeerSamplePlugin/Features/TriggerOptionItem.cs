using System;

namespace HSPI_HomeSeerSamplePlugin.Features {

    /// <summary>
    /// A simple Key-Value pair used by HTML liquid tags for representing a Trigger Option checkbox item
    ///  on the Sample Trigger Feature Page.
    /// </summary>
    [Serializable]
    public class TriggerOptionItem {
        
        public int Id { get; set; }
        public string Name { get; set; }

        public TriggerOptionItem(int id, string name) {
            Id = id;
            Name = name;
        }

        public TriggerOptionItem() { }

    }

}