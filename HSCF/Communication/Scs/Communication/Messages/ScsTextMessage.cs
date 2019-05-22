using System;

namespace HSCF.Communication.Scs.Communication.Messages
{
    /// <summary>
    /// This message is used to send/receive a text as message data.
    /// </summary>
    [Serializable]
    public class ScsTextMessage : ScsMessage
    {
        /// <summary>
        /// Message text that is being transmitted.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Default empty constructor.
        /// </summary>
        public ScsTextMessage()
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Message text that is being transmitted</param>
        public ScsTextMessage(string text)
        {
            Text = text;
        }
    }
}
