using System;

namespace HSCF.Communication.Scs.Communication.Messages
{
    /// <summary>
    /// This message is used to send/receive ping messages.
    /// Ping messages is used to keep connection alive between server and client.
    /// </summary>
    [Serializable]
    internal sealed class PingMessage : ScsMessage
    {
        
    }
}
