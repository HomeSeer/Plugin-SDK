using System;
using System.Collections.Generic;
using System.Linq;
using HomeSeer.Jui.Views;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class PlugExtraData {
        
        #region Named
        
        public List<string> NamedKeys => _namedData?.Keys.ToList() ?? new List<string>();
        public int NamedCount => _namedData?.Count ?? 0;
        
        private Dictionary<string, string> _namedData = new Dictionary<string, string>();
        
        //Create
        public bool AddNamed(string key, string data) {
            if (string.IsNullOrWhiteSpace(key)) {
                throw new ArgumentNullException(nameof(key));
            }

            if (_namedData.ContainsKey(key)) {
                return false;
            }
                
            _namedData.Add(key, data);
            return true;
        }
        
        //Read
        public object GetNamed<TData>(string key) {

            var jsonString = this[key];
            
            try {
                var data = JsonConvert.DeserializeObject<TData>(jsonString);
				
                return data;
            }
            catch (JsonSerializationException exception) {
					
                throw new JsonDataException("Couldn't deserialize the data", exception);
            }
        }
        
        public string this[string key] {
            get {
                if (string.IsNullOrWhiteSpace(key)) {
                    throw new ArgumentNullException(nameof(key));
                }

                return _namedData[key];
            }
            set {
                if (string.IsNullOrWhiteSpace(key)) {
                    throw new ArgumentNullException(nameof(key));
                }

                _namedData[key] = value;
            }
        }

        public bool ContainsNamed(string key) {
            if (string.IsNullOrWhiteSpace(key)) {
                throw new ArgumentNullException(nameof(key));
            }
            
            return _namedData.ContainsKey(key);
        }
        
        //Delete
        public bool RemoveNamed(string key) {
            
            if (string.IsNullOrWhiteSpace(key)) {
                throw new ArgumentNullException(nameof(key));
            }

            return _namedData.Remove(key);
        }
        
        public void RemoveAllNamed() {
            _namedData = new Dictionary<string, string>();
        }
        
        #endregion
        
        #region UnNamed
        
        public List<string> UnNamed      => _unNamedData ?? new List<string>();
        public int          UnNamedCount => _unNamedData?.Count ?? 0;
        
        private List<string> _unNamedData = new List<string>();
        
        //Create
        public int AddUnNamed(string data) {

            if (string.IsNullOrWhiteSpace(data)) {
                throw new ArgumentNullException(nameof(data), "You cannot store empty strings or null data");
            }
            
            _unNamedData.Add(data);
            return _unNamedData.Count - 1;
        }
        
        //Read
        public object GetUnNamed<TData>(int index) {

            var jsonString = this[index];
            
            try {
                var data = JsonConvert.DeserializeObject<TData>(jsonString);
				
                return data;
            }
            catch (JsonSerializationException exception) {
					
                throw new JsonDataException("Couldn't deserialize the data", exception);
            }
        }

        public string this[int index] {
            get => _unNamedData[index];
            set => _unNamedData[index] = value;
        }
        
        //Delete
        public void RemoveUnNamedAt(int index) {
            
            _unNamedData.RemoveAt(index);
        }
        
        public bool RemoveUnNamed(string data) {
            
            if (string.IsNullOrWhiteSpace(data)) {
                throw new ArgumentNullException(nameof(data), "Empty strings or null data cannot be stored");
            }

            return _unNamedData.Remove(data);
        }
    
        public void RemoveAllUnNamed() {
            _unNamedData = new List<string>();
        }
        
        #endregion

    }

}