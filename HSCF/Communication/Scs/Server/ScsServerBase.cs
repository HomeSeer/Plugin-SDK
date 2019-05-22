using System;
using HSCF.CollectionsSCF;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.Channels;
using HSCF.Communication.Scs.Communication.Protocols;

namespace HSCF.Communication.Scs.Server
{
    /// <summary>
    /// This class provides base functionality for server classes.
    /// </summary>
    internal abstract class ScsServerBase : IScsServer
    {
        /// <summary>
        /// This event is raised when a new client is connected.
        /// </summary>
        public event EventHandler<ServerClientEventArgs> ClientConnected;

        /// <summary>
        /// Gets/sets wire protocol that is used while reading and writing messages.
        /// </summary>
        public IScsWireProtocol WireProtocol { get; set; }

        /// <summary>
        /// A collection of clients that are connected to the server.
        /// </summary>
        public ThreadSafeSortedList<long, IScsServerClient> Clients { get; private set; }

        /// <summary>
        /// This object is used to listen incoming connections.
        /// </summary>
        protected IConnectionListener _connectionListener;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ScsServerBase()
        {
            Clients = new ThreadSafeSortedList<long, IScsServerClient>();
            WireProtocol = WireProtocolManager.GetDefaultWireProtocol();
        }

        /// <summary>
        /// Starts server.
        /// </summary>
        public virtual void Start()
        {
            _connectionListener = CreateConnectionListener();
            _connectionListener.CommunicationChannelConnected += ConnectionListener_CommunicationChannelConnected;
            _connectionListener.Start();
        }

        /// <summary>
        /// Stops server.
        /// </summary>
        public virtual void Stop()
        {
            if (_connectionListener != null)
            {
                _connectionListener.Stop();
            }

            foreach (var client in Clients.GetAllItems())
            {
                client.Disconnect();
            }
        }

        /// <summary>
        /// This method is implemented by derived classes to create appropriate connection listener to listen incoming connection requets.
        /// </summary>
        /// <returns></returns>
        protected abstract IConnectionListener CreateConnectionListener();

        /// <summary>
        /// Handles CommunicationChannelConnected event of _connectionListener object.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void ConnectionListener_CommunicationChannelConnected(object sender, CommunicationChannelEventArgs e)
        {
            var client = new ScsServerClient(e.Channel)
                         {
                             ClientId = ScsServerManager.GetClientId(),
                             WireProtocol = WireProtocol
                         };
            client.ipaddress = e.ipaddress;
            client.Disconnected += Client_Disconnected;
            Clients[client.ClientId] = client;
            OnClientConnected(client);
            e.Channel.Start();
        }

        /// <summary>
        /// Handles Disconnected events of all connected clients.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void Client_Disconnected(object sender, EventArgs e)
        {
            Clients.Remove(((IScsServerClient)sender).ClientId);
        }

        /// <summary>
        /// Raises ClientConnected event.
        /// </summary>
        /// <param name="client">Connected client</param>
        protected virtual void OnClientConnected(IScsServerClient client)
        {
            if (ClientConnected != null)
            {
                try
                {
                    ClientConnected(this, new ServerClientEventArgs(client));
                }
                catch
                {

                }
            }
        }
    }
}
