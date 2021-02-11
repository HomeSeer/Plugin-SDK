using System;
using System.Runtime.Serialization;

namespace HomeSeer.Jui.Views {

    /// <summary>
    /// The exception that is thrown when a view's type doesn't match its class
    /// </summary>
    [Serializable]
    public class ViewTypeMismatchException : Exception
    {

        /// <summary>
        /// Create an exception with the default message
        /// </summary>
        public ViewTypeMismatchException() : base("The view's listed type and actual type do not match") { }
        /// <summary>
        /// Create an exception with a message
        /// </summary>
        /// <param name="message">The message to include with the exception</param>
        public ViewTypeMismatchException(string message) : base(message) { }
        /// <summary>
        /// Create an exception wrapping another exception with a message
        /// </summary>
        /// <param name="message">The message to include with the exception</param>
        /// <param name="innerException">The exception to wrap</param>
        public ViewTypeMismatchException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Special constructor used for deserialization. 
        /// This is mandatory in order for HSCF to be able to deserialize this exception.
        /// </summary>
        /// <param name="info">The data to deserialize from</param>
        /// <param name="context">The context of the source stream</param>
        protected ViewTypeMismatchException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}