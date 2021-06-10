using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// A managed collection of <see cref="StatusGraphic"/>s
    /// </summary>
    /// <remarks>
    /// This is a <see cref="SortedDictionary{TKey,TValue}"/> where <see cref="StatusGraphic.Value"/> or
    ///  <see cref="StatusGraphic.RangeMin"/> is used as the key. This is used to ensure that there is only
    ///  one <see cref="StatusGraphic"/> per <see cref="AbstractHsDevice.Value"/>
    /// </remarks>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class StatusGraphicCollection {
        
        private SortedDictionary<double, StatusGraphic> _statusGraphics = new SortedDictionary<double, StatusGraphic>();

        //Create
        
        /// <summary>
        /// Add a <see cref="StatusGraphic"/> to the collection
        /// </summary>
        /// <param name="statusGraphic">A <see cref="StatusGraphic"/> to add. It must not target a value that is already handled by the collection.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="statusGraphic"/> targets a value that is already handled in the collection.</exception>
        public void Add(StatusGraphic statusGraphic) {
            if (Contains(statusGraphic)) {
                throw new ArgumentException("A status graphic with that value set already exists");
            }
            
            _statusGraphics.Add(statusGraphic.IsRange ? statusGraphic.RangeMin : statusGraphic.Value, statusGraphic);
        }

        /// <summary>
        /// Add multiple <see cref="StatusGraphic"/>s to the collection
        /// </summary>
        /// <param name="statusGraphics">A collection of <see cref="StatusGraphic"/>s to add. Make sure there is only one <see cref="StatusGraphic"/> handling each value.</param>
        /// <exception cref="ArgumentException">Thrown if any element in <paramref name="statusGraphics"/> targets a value that is already handled in the collection.</exception>
        public void AddRange(IEnumerable<StatusGraphic> statusGraphics) {
            foreach (var statusGraphic in statusGraphics) {
                if (Contains(statusGraphic)) {
                    throw new ArgumentException("A status graphic with that value set already exists");
                }
            
                _statusGraphics.Add(statusGraphic.IsRange ? statusGraphic.RangeMin : statusGraphic.Value, statusGraphic);
            }
        }
        
        //Read
        
        /// <summary>
        /// Get the <see cref="StatusGraphic"/> in the collection that handles the specified value
        /// </summary>
        /// <param name="value">The value associated with the desired <see cref="StatusGraphic"/></param>
        /// <exception cref="KeyNotFoundException">Thrown when no <see cref="StatusGraphic"/> is found that handles the specified <paramref name="value"/></exception>
        public StatusGraphic this[double value] {
            get {
                try {

                    StatusGraphic foundStatusGraphic = null;
                    foreach (double graphicKey in _statusGraphics.Keys) {
                        if (foundStatusGraphic == null) {
                            foundStatusGraphic = _statusGraphics[graphicKey];
                        }
                        if (value < graphicKey) {
                            break;
                        }
                        foundStatusGraphic = _statusGraphics[graphicKey];
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
        
        /// <summary>
        /// Determine if a <see cref="StatusGraphic"/> is already in the managed collection
        /// </summary>
        /// <remarks>
        /// This check is based on <see cref="StatusGraphic.Value"/> or <see cref="StatusGraphic.RangeMin"/>
        /// </remarks>
        /// <param name="statusGraphic">The <see cref="StatusGraphic"/> to search for</param>
        /// <returns><see langword="True"/> if the <paramref name="statusGraphic"/> is in the collection, <see langword="False"/> if it is not</returns>
        public bool Contains(StatusGraphic statusGraphic) {
            return ContainsValue(statusGraphic.IsRange ? statusGraphic.RangeMin : statusGraphic.Value);
        }
        
        /// <summary>
        /// Determine if a value is handled by the collection
        /// </summary>
        /// <param name="value">The value to check for</param>
        /// <returns><see langword="True"/> if the value is handled, <see langword="False"/> if it is not</returns>
        public bool ContainsValue(double value) {
            try {

                StatusGraphic foundStatusGraphic = null;
                foreach (double statusControlKey in _statusGraphics.Keys) {
                    if (foundStatusGraphic == null) {
                        foundStatusGraphic = _statusGraphics[statusControlKey];
                    }
                    if (value < statusControlKey) {
                        break;
                    }
                    foundStatusGraphic = _statusGraphics[statusControlKey];
                }

                return foundStatusGraphic != null && foundStatusGraphic.IsValueInRange(value);
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Get a list of <see cref="StatusGraphic"/>s that handle a given range of values
        /// </summary>
        /// <param name="min">The smallest number in the range</param>
        /// <param name="max">The largest number in the range</param>
        /// <returns>A List of <see cref="StatusGraphic"/>s</returns>
        public List<StatusGraphic> GetGraphicsForRange(double min, double max) {
            
            var foundStatusGraphics = new List<StatusGraphic>();
            foreach (double statusGraphicsKey in _statusGraphics.Keys) {
                
                if (statusGraphicsKey >= min && statusGraphicsKey <= max) {
                    foundStatusGraphics.Add(this[statusGraphicsKey]);
                }
            }

            return foundStatusGraphics;
        }
        
        /// <summary>
        /// Get an unordered list of the <see cref="StatusGraphic"/>s in the collection
        /// </summary>
        public List<StatusGraphic> Values => _statusGraphics.Values.ToList();
        
        /// <summary>
        /// The number of <see cref="StatusGraphic"/>s in the collection
        /// </summary>
        public int Count => _statusGraphics?.Count ?? 0;

        //Delete

        /// <summary>
        /// Remove the <see cref="StatusGraphic"/> that handles the specified <paramref name="value"/>
        /// </summary>
        /// <param name="value">The value handled by the <see cref="StatusGraphic"/> to remove</param>
        public void RemoveKey(double value) {
            var itemToDelete = this[value];
            var itemKey = itemToDelete.IsRange ? itemToDelete.RangeMin : itemToDelete.Value;
            _statusGraphics.Remove(itemKey);
        }
        
        /// <summary>
        /// Remove a <see cref="StatusGraphic"/> from the collection
        /// </summary>
        /// <remarks>
        /// A key is determined by the <see cref="StatusGraphic.Value"/> or <see cref="StatusGraphic.RangeMin"/>
        ///  of <paramref name="statusGraphic"/>. The found item is then compared by hash code to ensure they are the
        ///  same.
        /// </remarks>
        /// <param name="statusGraphic">A <see cref="StatusGraphic"/> to remove</param>
        public void Remove(StatusGraphic statusGraphic) {
            var itemKey = statusGraphic.IsRange ? statusGraphic.RangeMin : statusGraphic.Value;
            var itemToDelete = this[itemKey];
            
            if (itemToDelete.GetHashCode() == statusGraphic.GetHashCode()) {
                _statusGraphics.Remove(itemKey);
            }
        }

        /// <summary>
        /// Remove all <see cref="StatusGraphic"/>s in the collection
        /// </summary>
        public void RemoveAll() {
            _statusGraphics = new SortedDictionary<double, StatusGraphic>();
        }
        
    }
}
