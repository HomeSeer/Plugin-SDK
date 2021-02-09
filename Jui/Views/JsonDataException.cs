using System;
using System.Runtime.Serialization;

namespace HomeSeer.Jui.Views {

    /// <summary>
    /// The exception thrown when there is a problem serializing/deserializing JSON data
    /// </summary>
    [Serializable]
    public class JsonDataException : Exception {

        /// <summary>
        /// Create an exception with the default message
        /// </summary>
        public JsonDataException() : base("There is a problem with the JSON data causing it to fail serialization/deserialization") { }
        /// <summary>
        /// Create an exception with the default message that wraps another exception
        /// </summary>
        /// <param name="innerException">The exception to wrap</param>
        public JsonDataException(Exception innerException) : base("There is a problem with the JSON data causing it to fail serialization/deserialization", innerException) { }
        /// <summary>
        /// Create an exception with a message
        /// </summary>
        /// <param name="message">The message to include with the exception</param>
        public JsonDataException(string message) : base(message) { }
        /// <summary>
        /// Create an exception wrapping another exception with a message
        /// </summary>
        /// <param name="message">The message to include with the exception</param>
        /// <param name="innerException">The exception to wrap</param>
        public JsonDataException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Special constructor used for deserialization. 
        /// This is mandatory in order for HSCF to be able to deserialize this exception.
        /// </summary>
        /// <param name="info">The data to deserialize from</param>
        /// <param name="context">The context of the source stream</param>
        protected JsonDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }

}