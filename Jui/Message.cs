using Newtonsoft.Json;

namespace HomeSeer.Jui {

	/// <summary>
	/// Instructions sent from a plugin to a client either take the shape of a form view or a message.
	/// Where a form view expects user interaction, a message does not.
	/// Messages are used for things like indicating what the current state of a long running task is so
	/// the client knows whether to continue waiting or not or for telling the client to load a particular
	/// URL using the default web browser.
	/// </summary>
	internal sealed class Message {

		/// <summary>
		/// The unique code indicating how this response message should be handled and the type of data contained within the content.
		/// </summary>
		[JsonProperty("code")]
		public int Code { get; set; }
		
		/// <summary>
		/// A message to display to the user or a URL that the client should navigate to
		/// </summary>
		[JsonProperty("content")]
		public string Content { get; set; }

		/// <summary>
		/// Create an instance of a message with a code and content
		/// </summary>
		/// <param name="code">The code of the message</param>
		/// <param name="content">The content of the message</param>
		[JsonConstructor]
		private Message(int code, string content) {
			Code = code;
			Content = content;
		}
		
		/// <summary>
		/// Serialize the message as JSON
		/// </summary>
		/// <returns>A string containing the message formatted as JSON</returns>
		public string ToJsonString() {
			return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings {
				TypeNameHandling = TypeNameHandling.Auto
			});
		}

		/// <summary>
		/// Factory class for creating messages
		/// </summary>
		public static class Factory {
			
			/// <summary>
			/// Create a new wait message.
			///<para>
			/// This is used to indicate to a user that the action is being processed
			/// and they should continue to wait until it completes or times-out.
			/// </para>
			/// </summary>
			/// <param name="content">The message to display to the user</param>
			/// <returns>A new Message with a code of 102 for WAIT</returns>
			public static Message CreateWaitMessage(string content) {
				return new Message(102, content);
			}
			
			/// <summary>
			/// Create a new success message.
			/// <para>
			/// This is used to indicate to a user that the action has completed successfully.
			/// </para>
			/// </summary>
			/// <param name="content">The message to display to the user</param>
			/// <returns>A new Message with a code of 200 for OK</returns>
			public static Message CreateSuccessMessage(string content) {
				return new Message(200, content);
			}
			
			/// <summary>
			/// Create a new URL redirect message
			/// <para>
			/// This is used to indicate to a client that it should load the URL contained in the content.
			/// </para>
			/// </summary>
			/// <param name="url">The URL to load</param>
			/// <returns>A new Message with a code of 303 for REDIRECT</returns>
			public static Message CreateUrlMessage(string url) {
				return new Message(303, url);
			}
			
			/// <summary>
			/// Create a new action failed message
			/// <para>
			/// This is used to indicate to a user that the action has failed to complete.
			/// </para>
			/// </summary>
			/// <param name="content">The message to display to the user</param>
			/// <returns>A new Message with a code of 500 for FAIL</returns>
			public static Message CreateFailMessage(string content) {
				return new Message(500, content);
			}
			
			/// <summary>
			/// Deserialize a JSON string to a message.
			/// <para>
			/// Plugins should never need to use this because clients shouldn't ever send a message back
			/// </para>
			/// </summary>
			/// <param name="jsonString">The JSON string containing the message</param>
			/// <returns>A Message</returns>
			public static Message FromJsonString(string jsonString) {
				return JsonConvert.DeserializeObject<Message>(jsonString, new JsonSerializerSettings {
					TypeNameHandling = TypeNameHandling.Auto
				});
			}

		}

	}

}