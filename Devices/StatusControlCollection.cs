using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using HomeSeer.PluginSdk;
using HomeSeer.PluginSdk.Devices;

namespace Classes {

    [Serializable()]
    public class StatusControlCollection {

        private System.Collections.Specialized.OrderedDictionary _statusControls = new System.Collections.Specialized.OrderedDictionary();

        //Create
        
        public void Add(StatusControl statusControl) {
            if (Contains(statusControl)) {
                throw new ArgumentException("A status control with that value set already exists");
            }
            
            _statusControls.Add(statusControl.IsRange ? statusControl.TargetRange.Min : statusControl.TargetValue, statusControl);
        }
        
        //Read
        
        public StatusControl this[double value] {
            get {
                try {

                    StatusControl foundStatusControl = null;
                    foreach (double statusControlKey in _statusControls.Keys) {
                        if (foundStatusControl == null) {
                            foundStatusControl = (StatusControl) _statusControls[statusControlKey];
                        }
                        if (value <= statusControlKey) {
                            break;
                        }
                        foundStatusControl = (StatusControl) _statusControls[statusControlKey];
                    }

                    if (foundStatusControl == null) {
                        throw new KeyNotFoundException();
                    }

                    if (foundStatusControl.TargetRange.IsValueInRange(value)) {
                        return foundStatusControl;
                    }

                    throw new KeyNotFoundException();
                }
                catch (Exception ex) {
                    throw new KeyNotFoundException("Can't find", ex);
                }
            }
        }
        
        public bool Contains(StatusControl statusControl) {
            try {
                var unused = statusControl.IsRange ? this[statusControl.TargetRange.Min] : this[statusControl.TargetValue];
                return true;
            }
            catch (KeyNotFoundException exception) {
                return false;
            }
        }
        
        public List<StatusControl> Values => _statusControls.Values.Cast<StatusControl>().ToList();
        
        public int Count => _statusControls?.Count ?? 0;
        
        //Update
        
        
        //Delete

        public void Remove(double value) {
            var itemToDelete = this[value];
            var itemKey      = itemToDelete.IsRange ? itemToDelete.TargetRange.Min : itemToDelete.TargetValue;
            _statusControls.Remove(itemKey);
        }

        public void RemoveAll() {
            _statusControls = new OrderedDictionary();
        }
  
    }

}