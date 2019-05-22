using System;
using System.Reflection;
using HSCF.Communication.Scs.Client;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.Messages;
using HSCF.Communication.ScsServices.Communication;
using HSCF.Communication.ScsServices.Communication.Messages;
using HSCF.Communication.Scs.Server;

namespace HSCF.Communication.ScsServices.Client
{
    /// <summary>
    /// Represents a service client that consumes a SCS service.
    /// </summary>
    /// <typeparam name="T">Type of service interface</typeparam>
    internal class ScsServiceClient<T> : IScsServiceClient<T> where T : class
    {
        /// <summary>
        /// This event is raised when client connected to server.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// This event is raised when client disconnected from server.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// Gets the current communication state.
        /// </summary>
        public CommunicationStates CommunicationState
        {
            get { return _client.CommunicationState; }
        }

        /// <summary>
        /// Reference to the service proxy to invoke remote service methods.
        /// </summary>
        public T ServiceProxy { get; private set; }

        /// <summary>
        /// Timeout value when invoking a service method.
        /// If timeout occurs before end of remote method call, an exception is thrown.
        /// Use -1 for no timeout (wait indefinite).
        /// </summary>
        public int Timeout
        {
            get { return RequestReplyMessenger<IScsServerClient>.Timeout; }
            set { RequestReplyMessenger<IScsServerClient>.Timeout = value; }
        }

        /// <summary>
        /// Underlying IScsClient object to communicate with server.
        /// </summary>
        private readonly IScsClient _client;

        /// <summary>
        /// Messenger object to send/receive messages over _client.
        /// </summary>
        private readonly RequestReplyMessenger<IScsClient> _requestReplyMessenger;

        /// <summary>
        /// This object is used to create a transparent proxy to invoke remote methods on server.
        /// </summary>
        private readonly RemoteInvokeProxy<T, IScsClient> _realProxy;

        /// <summary>
        /// The client object that is used to call method invokes in client side.
        /// May be null if client has no methods to be invoked by server.
        /// </summary>
        private readonly object _clientObject;

        /// <summary>
        /// Creates a new ScsServiceClient object.
        /// </summary>
        /// <param name="client">Underlying IScsClient object to communicate with server</param>
        /// <param name="clientObject">The client object that is used to call method invokes in client side.
        /// May be null if client has no methods to be invoked by server.</param>
        public ScsServiceClient(IScsClient client, object clientObject)
        {
            _client = client;
            _clientObject = clientObject;

            _client.Connected += Client_Connected;
            _client.Disconnected += Client_Disconnected;

            _requestReplyMessenger = new RequestReplyMessenger<IScsClient>(client);
            _requestReplyMessenger.MessageReceived += RequestReplyMessenger_MessageReceived;
            _requestReplyMessenger.Start();
            
            _realProxy = new RemoteInvokeProxy<T, IScsClient>(_requestReplyMessenger);
            ServiceProxy = (T)_realProxy.GetTransparentProxy();
        }

        /// <summary>
        /// Connects to server.
        /// </summary>
        public void Connect()
        {
            _client.Connect();
        }

        /// <summary>
        /// Disconnects from server.
        /// </summary>
        public void Disconnect()
        {
            _client.Disconnect();
        }

        /// <summary>
        /// Disposes this object and closes underlying connection.
        /// </summary>
        public void Dispose()
        {
            if (CommunicationState == CommunicationStates.Connected)
            {
                Disconnect();
            }
            this._requestReplyMessenger.Stop();     // rjh stop the queue processing so the app can exit gracefully
        }
        
        /// <summary>
        /// Handles MessageReceived event of messenger.
        /// It gets messages from server and invokes appropriate method.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void RequestReplyMessenger_MessageReceived(object sender, MessageEventArgs e)
        {
            //Cast message to ScsRemoteInvokeMessage and check it
            var invokeMessage = e.Message as ScsRemoteInvokeMessage;
            if (invokeMessage == null)
            {
                return;
            }

            //Check client object.
            if(_clientObject == null)
            {
                SendInvokeResponse(invokeMessage, null, new ScsRemoteException("Client does not wait for method invocations by server."));
                return;
            }

            //Invoke method
            object returnValue;
            try
            {
                var type = _clientObject.GetType();
                var method = type.GetMethod(invokeMessage.MethodName);
                returnValue = method.Invoke(_clientObject, invokeMessage.Parameters);
            }
            catch (TargetInvocationException ex)
            {
                var innerEx = ex.InnerException;
                SendInvokeResponse(invokeMessage, null, new ScsRemoteException(innerEx.Message, innerEx));
                return;
            }
            catch (Exception ex)
            {
                SendInvokeResponse(invokeMessage, null, new ScsRemoteException(ex.Message, ex));
                return;
            }

            //Send return value
            SendInvokeResponse(invokeMessage, returnValue, null);
        }

        /// <summary>
        /// Sends response to the remote application that invoked a service method.
        /// </summary>
        /// <param name="requestMessage">Request message</param>
        /// <param name="returnValue">Return value to send</param>
        /// <param name="exception">Exception to send</param>
        private void SendInvokeResponse(IScsMessage requestMessage, object returnValue, ScsRemoteException exception)
        {
            try
            {
                _requestReplyMessenger.SendMessage(
                    new ScsRemoteInvokeReturnMessage
                    {
                        RepliedMessageId = requestMessage.MessageId,
                        ReturnValue = returnValue,
                        RemoteException = exception
                    });
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error SendInvokeResponse: " + ex.Message);
            }
        }
        
        /// <summary>
        /// Handles Connected event of _client object.
        /// </summary>
        /// <param name="sender">Source of object</param>
        /// <param name="e">Event arguments</param>
        private void Client_Connected(object sender, EventArgs e)
        {
            _requestReplyMessenger.Start();
            OnConnected();
        }

        /// <summary>
        /// Handles Disconnected event of _client object.
        /// </summary>
        /// <param name="sender">Source of object</param>
        /// <param name="e">Event arguments</param>
        private void Client_Disconnected(object sender, EventArgs e)
        {
            _requestReplyMessenger.Stop();
            OnDisconnected();
        }
        
        /// <summary>
        /// Raises Connected event.
        /// </summary>
        private void OnConnected()
        {
            if (Connected != null)
            {
                try
                {
                    Connected(this, new EventArgs());
                }
                catch
                {

                }
            }
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