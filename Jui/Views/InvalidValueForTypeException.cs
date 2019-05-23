using System;

namespace HomeSeer.Jui.Views {

    /// <summary>
    /// The exception that is thrown when a value is invalid for a view's configured type
    /// </summary>
    public class InvalidValueForTypeException : Exception {

        /// <summary>
        /// Create an exception with the default message
        /// </summary>
        public InvalidValueForTypeException() : base("The value is invalid for the type") { }
        /// <summary>
        /// Create an exception with a message
        /// </summary>
        /// <param name="message">The message to include with the exception</param>
        public InvalidValueForTypeException(string message) : base(message) { }
        /// <summary>
        /// Create an exception wrapping another exception with a message
        /// </summary>
        /// <param name="message">The message to include with the exception</param>
        /// <param name="innerException">The exception to wrap</param>
        public InvalidValueForTypeException(string message, Exception innerException) : base(message, innerException) { }

    }

}