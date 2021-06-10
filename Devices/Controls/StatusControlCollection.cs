using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HomeSeer.PluginSdk.Devices.Controls {

    /// <summary>
    /// A managed collection of <see cref="StatusControl"/>s
    /// </summary>
    /// <remarks>
    /// This is a <see cref="SortedDictionary{TKey,TValue}"/> where <see cref="StatusControl.TargetValue"/> or
    ///  <see cref="ValueRange.Min"/> of <see cref="StatusControl.TargetRange"/> is used as the key.
    ///  This is used to ensure that there is only one <see cref="StatusControl"/> per <see cref="AbstractHsDevice.Value"/>
    /// </remarks>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class StatusControlCollection {

        private SortedDictionary<double, StatusControl> _statusControls = new SortedDictionary<double, StatusControl>();
        
        //Create
        
        /// <summary>
        /// Add a <see cref="StatusControl"/> to the collection
        /// </summary>
        /// <param name="statusControl">A <see cref="StatusControl"/> to add. It must not target a value that is already handled by the collection.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="statusControl"/> targets a value that is already handled in the collection.</exception>
        public void Add(StatusControl statusControl) {
            if (Contains(statusControl)) {
                throw new ArgumentException("A status control covering all or a portion of that value range already exists");
            }
            
            _statusControls.Add(statusControl.IsRange ? statusControl.TargetRange.Min : statusControl.TargetValue, statusControl);
        }

        /// <summary>
        /// Add multiple <see cref="StatusControl"/>s to the collection
        /// </summary>
        /// <param name="statusControls">A collection of <see cref="StatusControl"/>s to add. Make sure there is only one <see cref="StatusControl"/> handling each value.</param>
        /// <exception cref="ArgumentException">Thrown if any element in <paramref name="statusControls"/> targets a value that is already handled in the collection.</exception>
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
        
        /// <summary>
        /// Get the <see cref="StatusControl"/> in the collection that handles the specified value
        /// </summary>
        /// <param name="value">The value associated with the desired <see cref="StatusControl"/></param>
        /// <exception cref="KeyNotFoundException">Thrown when no <see cref="StatusControl"/> is found that handles the specified <paramref name="value"/></exception>
        public StatusControl this[double value] {
            get {
                try {

                    StatusControl foundStatusControl = null;
                    foreach (double statusControlKey in _statusControls.Keys) {
                        if (foundStatusControl == null) {
                            foundStatusControl = _statusControls[statusControlKey];
                        }
                        if (value < statusControlKey) {
                            break;
                        }
                        foundStatusControl = _statusControls[statusControlKey];
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
        
        /// <summary>
        /// Determine if a <see cref="StatusControl"/> is already in the managed collection
        /// </summary>
        /// <remarks>
        /// This check is based on <see cref="StatusControl.TargetValue"/> or the <see cref="ValueRange.Min"/> of
        ///  <see cref="StatusControl.TargetRange"/>
        /// </remarks>
        /// <param name="statusControl">The <see cref="StatusControl"/> to search for</param>
        /// <returns><see langword="True"/> if the <paramref name="statusControl"/> is in the collection, <see langword="False"/> if it is not</returns>
        public bool Contains(StatusControl statusControl) {    
            return ContainsValue(statusControl.IsRange ? statusControl.TargetRange.Min : statusControl.TargetValue);
        }

        /// <summary>
        /// Determine if a value is handled by the collection
        /// </summary>
        /// <param name="value">The value to check for</param>
        /// <returns><see langword="True"/> if the value is handled, <see langword="False"/> if it is not</returns>
        public bool ContainsValue(double value) {
            try {

                StatusControl foundStatusControl = null;
                foreach (var statusControlKey in _statusControls.Keys) {
                    if (foundStatusControl == null) {
                        foundStatusControl = _statusControls[statusControlKey];
                    }
                    if (value < statusControlKey) {
                        break;
                    }
                    foundStatusControl = _statusControls[statusControlKey];
                }

                return foundStatusControl != null && foundStatusControl.IsValueInRange(value);
            }
            catch (Exception) {
                return false;
            }
        }
        
        /// <summary>
        /// Get a list of <see cref="StatusControl"/>s that handle a given range of values
        /// </summary>
        /// <param name="min">The smallest number in the range</param>
        /// <param name="max">The largest number in the range</param>
        /// <returns>A List of <see cref="StatusControl"/>s</returns>
        public List<StatusControl> GetControlsForRange(double min, double max) {
            
            var foundStatusControls = new List<StatusControl>();
            foreach (double statusControlKey in _statusControls.Keys) {
                
                if (statusControlKey >= min && statusControlKey <= max) {
                    foundStatusControls.Add(this[statusControlKey]);
                }
            }

            return foundStatusControls;
        }
        
        /// <summary>
        /// Get an unordered list of the <see cref="StatusControl"/>s in the collection
        /// </summary>
        public List<StatusControl> Values => _statusControls.Values.ToList();
        
        /// <summary>
        /// The number of <see cref="StatusControl"/>s in the collection
        /// </summary>
        public int Count => _statusControls?.Count ?? 0;

        //Delete

        /// <summary>
        /// Remove the <see cref="StatusControl"/> that handles the specified <paramref name="value"/>
        /// </summary>
        /// <param name="value">The value handled by the <see cref="StatusControl"/> to remove</param>
        public void RemoveKey(double value) {
            if (!_statusControls.ContainsKey(value)) {
                return;
            }

            _statusControls.Remove(value);
        }
        
        /// <summary>
        /// Remove a <see cref="StatusControl"/> from the collection
        /// </summary>
        /// <remarks>
        /// A key is determined by the <see cref="StatusControl.TargetValue"/> or the <see cref="ValueRange.Min"/> of
        ///  <see cref="StatusControl.TargetRange"/> of <paramref name="statusControl"/>.
        ///  The found item is then compared by hash code to ensure they are the
        ///  same.
        /// </remarks>
        /// <param name="statusControl">A <see cref="StatusControl"/> to remove</param>
        public void Remove(StatusControl statusControl) {
            var itemKey = statusControl.IsRange ? statusControl.TargetRange.Min : statusControl.TargetValue;
            var itemToDelete = this[itemKey];
            
            if (itemToDelete.GetHashCode() == statusControl.GetHashCode()) {
                _statusControls.Remove(itemKey);
            }
        }

        /// <summary>
        /// Remove all <see cref="StatusControl"/>s in the collection
        /// </summary>
        public void RemoveAll() {
            _statusControls = new SortedDictionary<double, StatusControl>();
        }
  
    }

}