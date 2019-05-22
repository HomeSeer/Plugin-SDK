using System;
using HSCF.CollectionsSCF;
using HSCF.Communication.Scs.Communication.Protocols;

namespace HSCF.Communication.Scs.Server
{
    /// <summary>
    /// Represents a SCS server that is used to accept and manage client connections.
    /// </summary>
    public interface IScsServer
    {
        /// <summary>
        /// This event is raised when a new client is connected.
        /// </summary>
        event EventHandler<ServerClientEventArgs> ClientConnected;

        /// <summary>
        /// Gets/sets wire protocol that is used while reading and writing messages.
        /// </summary>
        IScsWireProtocol WireProtocol { get; set; }

        /// <summary>
        /// A collection of clients that are connected to the server.
        /// </summary>
        ThreadSafeSortedList<long, IScsServerClient> Clients { get; }
        
        /// <summary>
        /// Starts the server.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the server.
        /// </summary>
        void Stop();
    }
}
