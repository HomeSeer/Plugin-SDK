using System;
using HSCF.Communication.Scs.Communication;

namespace HSCF.Communication.Scs.Client
{
    /// <summary>
    /// Represents a client for SCS servers.
    /// </summary>
    public interface IConnectableClient : IDisposable
    {
        /// <summary>
        /// This event is raised when client connected to server.
        /// </summary>
        event EventHandler Connected;

        /// <summary>
        /// This event is raised when client disconnected from server.
        /// </summary>
        event EventHandler Disconnected;

        /// <summary>
        /// Gets the current communication state.
        /// </summary>
        CommunicationStates CommunicationState { get; }

        /// <summary>
        /// Connects to server.
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnects from server.
        /// </summary>
        void Disconnect();
    }
}
