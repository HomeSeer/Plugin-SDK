using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HomeSeer.PluginSdk.Devices.Controls {

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class StatusControlCollection {

        private SortedDictionary<double, StatusControl> _statusControls = new SortedDictionary<double, StatusControl>();
        
        //Create
        
        public void Add(StatusControl statusControl) {
            if (Contains(statusControl)) {
                throw new ArgumentException("A status control covering all or a portion of that value range already exists");
            }
            
            _statusControls.Add(statusControl.IsRange ? statusControl.TargetRange.Min : statusControl.TargetValue, statusControl);
        }

        public void AddRange(List<StatusControl> statusControls) {

            foreach (var statusControl in statusControls) {
                if (Contains(statusControl)) {
                    var valRangeText = statusControl.IsRange ? $"{statusControl.TargetRange.Min}-{statusControl.TargetRange.Max}" : $"{statusControl.TargetValue}";
                    throw new ArgumentException($"A status control covering all or a portion of the value range {valRangeText} already exists");
                }
            
                _statusControls.Add(statusControl.IsRange ? statusControl.TargetRange.Min : statusControl.TargetValue, statusControl);
            }
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
                        if (value < statusControlKey) {
                            break;
                        }
                        foundStatusControl = (StatusControl) _statusControls[statusControlKey];
                    }

                    if (foundStatusControl == null) {
                        throw new KeyNotFoundException();
                    }

                    if (foundStatusControl.IsValueInRange(value)) {
                        return foundStatusControl;
                    }

                    throw new KeyNotFoundException();
                }
                catch (Exception ex) {
                    throw new KeyNotFoundException("Can't find", ex);
                }
            }
        }
        
        public bool Contains(StatusControl statusControl) 
        {    
            return ContainsValue(statusControl.IsRange ? statusControl.TargetRange.Min : statusControl.TargetValue);
        }

        public bool ContainsValue(double value) {
            try {

                StatusControl foundStatusControl = null;
                foreach (var statusControlKey in _statusControls.Keys) {
                    if (foundStatusControl == null) {
                        foundStatusControl = (StatusControl) _statusControls[statusControlKey];
                    }
                    if (value < statusControlKey) {
                        break;
                    }
                    foundStatusControl = (StatusControl) _statusControls[statusControlKey];
                }

                return foundStatusControl != null && foundStatusControl.IsValueInRange(value);
            }
            catch (Exception) {
                return false;
            }
        }
        
        public List<StatusControl> GetControlsForRange(double min, double max) {
            
            var foundStatusControls = new List<StatusControl>();
            foreach (double statusControlKey in _statusControls.Keys) {
                
                if (statusControlKey >= min && statusControlKey <= max) {
                    foundStatusControls.Add(this[statusControlKey]);
                }
            }

            return foundStatusControls;
        }
        
        public List<StatusControl> Values => _statusControls.Values.Cast<StatusControl>().ToList();
        
        public int Count => _statusControls?.Count ?? 0;
        
        //Update
        
        
        //Delete

        public void RemoveKey(double value) {
            if (!_statusControls.ContainsKey(value)) {
                return;
            }

            _statusControls.Remove(value);
        }
        
        public void Remove(StatusControl statusControl) {
            var itemKey = statusControl.IsRange ? statusControl.TargetRange.Min : statusControl.TargetValue;
            var itemToDelete = this[itemKey];
            
            if (itemToDelete.GetHashCode() == statusControl.GetHashCode()) {
                _statusControls.Remove(itemKey);
            }
        }

        public void RemoveAll() {
            _statusControls = new SortedDictionary<double, StatusControl>();
        }
  
    }

}