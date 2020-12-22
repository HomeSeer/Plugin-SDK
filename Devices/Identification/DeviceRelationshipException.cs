using System;
using System.Runtime.Serialization;

namespace HomeSeer.PluginSdk.Devices.Identification {

    /// <summary>
    /// Represents errors occuring while trying to set the relationship of a device or feature
    /// </summary>
    [Serializable]
    public class DeviceRelationshipException : Exception {
        
        /// <summary>
        /// Initialize a new instance of the DeviceRelationshipException class with a specified message
        /// </summary>
        /// <param name="message">The error message</param>
        public DeviceRelationshipException(string message) : base(message) { }
        /// <summary>
        /// Initialize a new instance of the DeviceRelationshipException class with a specified message
        ///  and a reference to an inner exception
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="innerException">An <see cref="Exception"/> that caused this exception</param>
        public DeviceRelationshipException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Special constructor used for deserialization. 
        /// This is mandatory in order for HSCF to be able to deserialize this exception.
        /// </summary>
        /// <param name="info">The data to deserialize from</param>
        /// <param name="context">The context of the source stream</param>
        protected DeviceRelationshipException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }

}