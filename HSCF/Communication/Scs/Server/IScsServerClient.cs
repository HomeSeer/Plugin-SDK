using System;
using HSCF.Communication.Scs.Communication;

namespace HSCF.Communication.Scs.Server
{
    /// <summary>
    /// Represents a client from a perspective of a server.
    /// </summary>
    public interface IScsServerClient : IMessenger
    {
        /// <summary>
        /// This event is raised when client disconnected from server.
        /// </summary>
        event EventHandler Disconnected;
        
        /// <summary>
        /// Unique identifier for this client in server.
        /// </summary>
        long ClientId { get; }

        String ipaddress { get; set; }

        /// <summary>
        /// Gets the current communication state.
        /// </summary>
        CommunicationStates CommunicationState { get; }

        /// <summary>
        /// Disconnects from server.
        /// </summary>
        void Disconnect();
    }
}
