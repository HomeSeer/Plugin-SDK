using System;
using System.Runtime.Remoting.Proxies;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Server;
using HSCF.Communication.ScsServices.Communication;

namespace HSCF.Communication.ScsServices.Service
{
    /// <summary>
    /// Implements IScsServiceClient.
    /// It is used to manage and monitor a service client.
    /// </summary>
    internal class ScsServiceClient : IScsServiceClient
    {
        public String ipaddress
        {
            get { return _serverClient.ipaddress; }
        }

        /// <summary>
        /// This event is raised when this client is disconnected from server.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// Unique identifier for this client.
        /// </summary>
        public long ClientId
        {
            get { return _serverClient.ClientId; }
        }

        /// <summary>
        /// Gets the communication state of the Client.
        /// </summary>
        public CommunicationStates CommunicationState
        {
            get
            {
                return _serverClient.CommunicationState;
            }
        }

        /// <summary>
        /// Reference to underlying IScsServerClient object.
        /// </summary>
        private readonly IScsServerClient _serverClient;

        /// <summary>
        /// This object is used to send messages to client.
        /// </summary>
        private readonly RequestReplyMessenger<IScsServerClient> _requestReplyMessenger;

        /// <summary>
        /// Last created proxy object to invoke remote medhods.
        /// </summary>
        private RealProxy _realProxy;

        /// <summary>
        /// Creates a new ScsServiceClient object.
        /// </summary>
        /// <param name="serverClient">Reference to underlying IScsServerClient object</param>
        /// <param name="requestReplyMessenger">RequestReplyMessenger to send messages</param>
        public ScsServiceClient(IScsServerClient serverClient, RequestReplyMessenger<IScsServerClient> requestReplyMessenger)
        {
            _serverClient = serverClient;
            _serverClient.Disconnected += Client_Disconnected;
            _requestReplyMessenger = requestReplyMessenger;
        }

        /// <summary>
        /// Closes client connection.
        /// </summary>
        public void Disconnect()
        {
            _serverClient.Disconnect();
        }

        /// <summary>
        /// Gets the client proxy interface that provides calling client methods remotely.
        /// </summary>
        /// <typeparam name="T">Type of client interface</typeparam>
        /// <returns>Client interface</returns>
        public T GetClientProxy<T>() where T : class
        {
            _realProxy = new RemoteInvokeProxy<T, IScsServerClient>(_requestReplyMessenger);
            return (T)_realProxy.GetTransparentProxy();
        }

        /// <summary>
        /// Handles disconnect event of _serverClient object.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void Client_Disconnected(object sender, EventArgs e)
        {
            _requestReplyMessenger.Stop();
            OnDisconnected();
        }

        /// <summary>
        /// Raises Disconnected event.
        /// </summary>
        private void OnDisconnected()
        {
            if (Disconnected != null)
            {
                try
                {
                    Disconnected(this, new EventArgs());
                }
                catch
                {

                }
            }
        }
    }
}
