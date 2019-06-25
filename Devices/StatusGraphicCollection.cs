using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using HomeSeer.PluginSdk;

namespace Classes {

    [Serializable()]
    public class StatusGraphicCollection {
        
        private System.Collections.Specialized.OrderedDictionary _statusGraphics = new System.Collections.Specialized.OrderedDictionary();

        //Create
        
        public void Add(StatusGraphic statusGraphic) {
            if (Contains(statusGraphic)) {
                throw new ArgumentException("A status graphic with that value set already exists");
            }
            
            _statusGraphics.Add(statusGraphic.IsRange ? statusGraphic.RangeMin : statusGraphic.Value, statusGraphic);
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
                        if (value <= graphicKey) {
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
        
        public List<StatusGraphic> Values => _statusGraphics.Values.Cast<StatusGraphic>().ToList();
        
        public int Count => _statusGraphics?.Count ?? 0;
        
        //Update
        
        
        //Delete

        public void Remove(double value) {
            var itemToDelete = this[value];
            var itemKey      = itemToDelete.IsRange ? itemToDelete.RangeMin : itemToDelete.Value;
            _statusGraphics.Remove(itemKey);
        }

        public void RemoveAll() {
            _statusGraphics = new OrderedDictionary();
        }
        
    }
}
