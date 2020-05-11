using System;
using System.Collections.Generic;
using System.Linq;
using HomeSeer.Jui.Views;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// A collection of keyed and non-keyed data items attached to a <see cref="AbstractHsDevice"/>
    /// </summary>
    /// <remarks>
    /// <para>Use this to store any data specific to the operation of your plugin.</para>
    /// <para>Please note that all keys will be converted to lower case when stored in the HS database</para>
    /// </remarks>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class PlugExtraData {
        
        #region Named
        
        /// <summary>
        /// A list of keys for data items stored in this <see cref="PlugExtraData"/>
        /// </summary>
        public List<string> NamedKeys => _namedData?.Keys.ToList() ?? new List<string>();
        /// <summary>
        /// The number of keyed data items stored in the <see cref="PlugExtraData"/>
        /// </summary>
        public int NamedCount => _namedData?.Count ?? 0;
        
        private Dictionary<string, string> _namedData = new Dictionary<string, string>();
        //private Dictionary<string, object> _namedRawData = new Dictionary<string, object>();
        
        /// <summary>
        /// Add a new keyed data item to the collection
        /// </summary>
        /// <param name="key">The key for the data</param>
        /// <param name="data">The data to save</param>
        /// <returns>
        /// TRUE if the item was saved,
        ///  FALSE if it already exists
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the key is null or whitespace
        /// </exception>
        /// <para>Please note that all keys will be converted to lower case when stored in the HS database</para>
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
        
        /// <summary>
        /// Get the item with the specified key deserialized as the specified type
        /// </summary>
        /// <param name="key">The key of the item to get</param>
        /// <typeparam name="TData">The type of the object stored as a JSON serialized string</typeparam>
        /// <returns>
        /// The data stored at the specified key as an instance of the specified object type
        /// </returns>
        /// <exception cref="JsonDataException">Thrown when there was a problem deserializing the data</exception>
        /// <exception cref="ArgumentNullException">Thrown when the specified key is null or whitespace.</exception>
        public TData GetNamed<TData>(string key) {

            var jsonString = this[key];
            
            try {
                var data = JsonConvert.DeserializeObject<TData>(jsonString.Trim('\"', '\\'));
				
                return data;
            }
            catch (JsonSerializationException exception) {
					
                throw new JsonDataException("Couldn't deserialize the data", exception);
            }
        }
        
        /// <summary>
        /// Get the item with the specified key
        /// </summary>
        /// <param name="key">The key of the item to get</param>
        /// <exception cref="ArgumentNullException">Thrown when the specified key is null or whitespace.</exception>
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

        /// <summary>
        /// Determine if a data item with the specified key exists in the collection
        /// </summary>
        /// <param name="key">The key of the data item to look for</param>
        /// <returns>
        /// TRUE if the item exists in the collection,
        ///  FALSE if it does not.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the specified key is null or whitespace.</exception>
        public bool ContainsNamed(string key) {
            if (string.IsNullOrWhiteSpace(key)) {
                throw new ArgumentNullException(nameof(key));
            }
            
            return _namedData.ContainsKey(key);
        }
        
        /// <summary>
        /// Remove the data item with the specified key from the collection.
        /// </summary>
        /// <param name="key">The key of the data item to remove</param>
        /// <returns>
        /// TRUE if the item was removed from the collection,
        ///  FALSE if it was not.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the specified key is null or whitespace.</exception>
        public bool RemoveNamed(string key) {
            
            if (string.IsNullOrWhiteSpace(key)) {
                throw new ArgumentNullException(nameof(key));
            }

            return _namedData.Remove(key);
        }
        
        /// <summary>
        /// Remove all keyed items from the collection.
        /// </summary>
        public void RemoveAllNamed() {
            _namedData = new Dictionary<string, string>();
        }
        
        #endregion
        
        #region UnNamed
        
        /// <summary>
        /// A collection of non-keyed data items stored in the <see cref="PlugExtraData"/>
        /// </summary>
        public List<string> UnNamed      => _unNamedData ?? new List<string>();
        /// <summary>
        /// The number of non-keyed items in the <see cref="PlugExtraData"/>
        /// </summary>
        public int          UnNamedCount => _unNamedData?.Count ?? 0;
        
        private List<string> _unNamedData = new List<string>();
        
        /// <summary>
        /// Add a new item to the collection without a key.
        /// </summary>
        /// <param name="data">The data for the item to save.</param>
        /// <returns>The index of the item in the collection.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the data to be stored is null or whitespace.</exception>
        /// <para>Please note that all keys will be converted to lower case when stored in the HS database</para>
        public int AddUnNamed(string data) {

            if (string.IsNullOrWhiteSpace(data)) {
                throw new ArgumentNullException(nameof(data), "You cannot store empty strings or null data");
            }
            
            _unNamedData.Add(data);
            return _unNamedData.Count - 1;
        }
        
        /// <summary>
        /// Get the non-keyed item located at the specified index in the collection deserialized to the specified type.
        /// </summary>
        /// <param name="index">The index of the non-keyed item to get.</param>
        /// <typeparam name="TData">The type the data should be deserialized to.</typeparam>
        /// <returns>
        /// An instance of the specified type of the data at the specified index in the collection.
        /// </returns>
        /// <exception cref="JsonDataException">
        /// Thrown when there is an error deserializing the data to the type specified.
        /// </exception>
        public TData GetUnNamed<TData>(int index) {

            var jsonString = this[index];
            
            try {
                var data = JsonConvert.DeserializeObject<TData>(jsonString.Trim('\"', '\\'));
				
                return data;
            }
            catch (JsonSerializationException exception) {
					
                throw new JsonDataException("Couldn't deserialize the data", exception);
            }
        }

        /// <summary>
        /// Get the non-keyed item located at the specified index in the collection.
        /// </summary>
        /// <param name="index">The index of the non-keyed item to get.</param>
        public string this[int index] {
            get => _unNamedData[index];
            set => _unNamedData[index] = value;
        }
        
        /// <summary>
        /// Remove the non-keyed item at the specified index from the collection.
        /// </summary>
        /// <param name="index">The index of the non-keyed item to remove from the collection.</param>
        public void RemoveUnNamedAt(int index) {
            
            _unNamedData.RemoveAt(index);
        }
        
        /// <summary>
        /// Remove the specified non-keyed item from the collection using the default Equals implementation of the strings.
        /// </summary>
        /// <param name="data">The non-keyed item to remove from the collection.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the specified data is null or an empty string.</exception>
        public bool RemoveUnNamed(string data) {
            
            if (string.IsNullOrWhiteSpace(data)) {
                throw new ArgumentNullException(nameof(data), "Empty strings or null data cannot be stored");
            }

            return _unNamedData.Remove(data);
        }
    
        /// <summary>
        /// Remove all non-keyed items from the collection.
        /// </summary>
        public void RemoveAllUnNamed() {
            _unNamedData = new List<string>();
        }
        
        #endregion

    }

}