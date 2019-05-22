using System;

namespace HSCF.Communication.Scs.Communication.Messages
{
    /// <summary>
    /// This message is used to send/receive a raw byte array as message data.
    /// </summary>
    [Serializable]
    public class ScsRawDataMessage : ScsMessage
    {
        /// <summary>
        /// Message data that is being transmitted.
        /// </summary>
        public byte[] MessageData { get; set; }

        /// <summary>
        /// Default empty constructor.
        /// </summary>
        public ScsRawDataMessage()
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messageData">Message data that is being transmitted</param>
        public ScsRawDataMessage(byte[] messageData)
        {
            MessageData = messageData;
        }
    }
}
