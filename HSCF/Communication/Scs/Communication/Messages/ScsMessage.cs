using System;

namespace HSCF.Communication.Scs.Communication.Messages
{
    /// <summary>
    /// Represents a message that is sent and received by server and client.
    /// This is the base class for all messages.
    /// </summary>
    [Serializable]
    public class ScsMessage : IScsMessage
    {
        /// <summary>
        /// Unique identified for this message. 
        /// </summary>
        public string MessageId { get; private set; }

        /// <summary>
        /// Unique identified for this message. 
        /// </summary>
        public string RepliedMessageId { get; set; }

        /// <summary>
        /// Creates a new ScsMessage.
        /// This empty constructor is used for serializing.
        /// </summary>
        public ScsMessage()
        {
            MessageId = Guid.NewGuid().ToString();
        }
    }
}
