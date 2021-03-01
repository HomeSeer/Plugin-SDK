using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using HomeSeer.PluginSdk;

namespace HomeSeer.PluginSdk.Devices {

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class StatusGraphicCollection {
        
        private SortedDictionary<double, StatusGraphic> _statusGraphics = new SortedDictionary<double, StatusGraphic>();

        //Create
        
        public void Add(StatusGraphic statusGraphic) {
            if (Contains(statusGraphic)) {
                throw new ArgumentException("A status graphic with that value set already exists");
            }
            
            _statusGraphics.Add(statusGraphic.IsRange ? statusGraphic.RangeMin : statusGraphic.Value, statusGraphic);
        }

        public void AddRange(List<StatusGraphic> statusGraphics) {
            foreach (var statusGraphic in statusGraphics) {
                if (Contains(statusGraphic)) {
                    throw new ArgumentException("A status graphic with that value set already exists");
                }
            
                _statusGraphics.Add(statusGraphic.IsRange ? statusGraphic.RangeMin : statusGraphic.Value, statusGraphic);
            }
        }
        
        //Read
        
        public StatusGraphic this[double value] {
            get {
                try {

                    StatusGraphic foundStatusGraphic = null;
                    foreach (double graphicKey in _statusGraphics.Keys) {
                        if (foundStatusGraphic == null) {
                            foundStatusGraphic = (StatusGraphic) _statusGraphics[graphicKey];
                        }
                        if (value < graphicKey) {
                            break;
                        }
                        foundStatusGraphic = (StatusGraphic) _statusGraphics[graphicKey];
                    }

                    if (foundStatusGraphic == null) {
                        throw new KeyNotFoundException();
                    }

                    if (foundStatusGraphic.IsValueInRange(value)) {
                        return foundStatusGraphic;
                    }

                    throw new KeyNotFoundException();
                }
                catch (Exception ex) {
                    throw new KeyNotFoundException("Can't find", ex);
                }
            }
        }
        
        public bool Contains(StatusGraphic statusGraphic) {
            try {
                var unused = statusGraphic.IsRange ? this[statusGraphic.RangeMin] : this[statusGraphic.Value];
                return true;
            }
            catch (KeyNotFoundException exception) {
                return false;
            }
        }
        
        public bool ContainsValue(double value) {
            try {

                StatusGraphic foundStatusGraphic = null;
                foreach (double statusControlKey in _statusGraphics.Keys) {
                    if (foundStatusGraphic == null) {
                        foundStatusGraphic = (StatusGraphic) _statusGraphics[statusControlKey];
                    }
                    if (value < statusControlKey) {
                        break;
                    }
                    foundStatusGraphic = (StatusGraphic) _statusGraphics[statusControlKey];
                }

                return foundStatusGraphic != null && foundStatusGraphic.IsValueInRange(value);
            }
            catch (Exception) {
                return false;
            }
        }

        public List<StatusGraphic> GetGraphicsForRange(double min, double max) {
            
            var foundStatusGraphics = new List<StatusGraphic>();
            foreach (double statusGraphicsKey in _statusGraphics.Keys) {
                
                if (statusGraphicsKey >= min && statusGraphicsKey <= max) {
                    foundStatusGraphics.Add(this[statusGraphicsKey]);
                }
            }

            return foundStatusGraphics;
        }
        
        public List<StatusGraphic> Values => _statusGraphics.Values.Cast<StatusGraphic>().ToList();
        
        public int Count => _statusGraphics?.Count ?? 0;
        
        //Update
        
        
        //Delete

        public void RemoveKey(double value) {
            var itemToDelete = this[value];
            var itemKey      = itemToDelete.IsRange ? itemToDelete.RangeMin : itemToDelete.Value;
            _statusGraphics.Remove(value);
        }
        
        public void Remove(StatusGraphic statusGraphic) {
            var itemKey = statusGraphic.IsRange ? statusGraphic.RangeMin : statusGraphic.Value;
            var itemToDelete = this[itemKey];
            
            if (itemToDelete.GetHashCode() == statusGraphic.GetHashCode()) {
                _statusGraphics.Remove(itemKey);
            }
        }

        public void RemoveAll() {
            _statusGraphics = new SortedDictionary<double, StatusGraphic>();
        }
        
    }
}
