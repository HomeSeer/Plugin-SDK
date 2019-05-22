using System.IO;
using HSCF.Communication.Scs.Communication.Messages;

namespace HSCF.Communication.Scs.Communication.Protocols
{
    /// <summary>
    /// Represents a communication protocol between server and clients to send and receive a message.
    /// </summary>
    public interface IScsWireProtocol
    {
        /// <summary>
        /// Writes a message to a stream.
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="stream">Stream that is used to write message</param>
        void WriteMessage(IScsMessage message, Stream stream);

        /// <summary>
        /// Reads a message from a stream.
        /// </summary>
        /// <param name="stream">Stream taht is used to read message</param>
        /// <returns>Message that is read from stream</returns>
        IScsMessage ReadMessage(Stream stream);
    }
}
