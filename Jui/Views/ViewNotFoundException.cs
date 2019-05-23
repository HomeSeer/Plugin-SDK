using System;

namespace HomeSeer.Jui.Views {

	/// <summary>
	/// The exception that is thrown when a view is not found in a collection
	/// </summary>
	public class ViewNotFoundException : Exception {

		/// <summary>
		/// Create an exception with the default message
		/// </summary>
		public ViewNotFoundException() : base("A view with that ID was not found") { }
		/// <summary>
		/// Create an exception with a message
		/// </summary>
		/// <param name="message">The message to include with the exception</param>
		public ViewNotFoundException(string message) : base(message) { }
		/// <summary>
		/// Create an exception wrapping another exception with a message
		/// </summary>
		/// <param name="message">The message to include with the exception</param>
		/// <param name="innerException">The exception to wrap</param>
		public ViewNotFoundException(string message, Exception innerException) : base(message, innerException) { }

	}

}