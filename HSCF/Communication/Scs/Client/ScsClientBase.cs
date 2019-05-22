using System;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.Messages;
using HSCF.Communication.Scs.Communication.Channels;
using HSCF.Communication.Scs.Communication.Protocols;
using HSCF.Threading;

namespace HSCF.Communication.Scs.Client
{
    /// <summary>
    /// This class provides base functionality for client classes.
    /// </summary>
    internal abstract class ScsClientBase : IScsClient
    {
        /// <summary>
        /// This event is raised when a new message is received.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        /// <summary>
        /// Gets/sets wire protocol that is used while reading and writing messages.
        /// </summary>
        public IScsWireProtocol WireProtocol
        {
            get { return _wireProtocol; }
            set
            {
                if (CommunicationState == CommunicationStates.Connected)
                {
                    throw new ApplicationException("Wire protocol can not be changed while connected to server.");
                }

                _wireProtocol = value;
            }
        }
        private IScsWireProtocol _wireProtocol;

        /// <summary>
        /// This event is raised when communication channel closed.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// This event is raised when client disconnected from server.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// Gets the communication state of the Client.
        /// </summary>
        public CommunicationStates CommunicationState
        {
            get
            {
                return _communicationChannel != null
                           ? _communicationChannel.CommunicationState
                           : CommunicationStates.Disconnected;
            }
        }

        /// <summary>
        /// Gets the time of the last succesfully received message.
        /// </summary>
        public DateTime LastReceivedMessageTime
        {
            get
            {
                return _communicationChannel != null
                           ? _communicationChannel.LastReceivedMessageTime
                           : DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets the time of the last succesfully received message.
        /// </summary>
        public DateTime LastSentMessageTime
        {
            get
            {
                return _communicationChannel != null
                           ? _communicationChannel.LastSentMessageTime
                           : DateTime.MinValue;
            }
        }

        /// <summary>
        /// The communication channel that is used by client to send and receive messages.
        /// </summary>
        protected ICommunicationChannel _communicationChannel;

        /// <summary>
        /// This timer is used to send PingMessage messages to server periodically.
        /// </summary>
        private readonly Timer _pingTimer;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ScsClientBase()
        {
            _pingTimer = new Timer(30000);
            _pingTimer.Elapsed += PingTimer_Elapsed;
            _wireProtocol = WireProtocolManager.GetDefaultWireProtocol();
        }

        /// <summary>
        /// Connects to server.
        /// </summary>
        public void Connect()
        {
            _communicationChannel = CreateCommunicationChannel();
            _communicationChannel.WireProtocol = _wireProtocol;
            _communicationChannel.Disconnected += CommunicationChannel_Disconnected;
            _communicationChannel.MessageReceived += CommunicationChannel_MessageReceived;
            _communicationChannel.Start();
            _pingTimer.Start();
            OnConnected();
        }

        /// <summary>
        /// Disconnects from server.
        /// </summary>
        public void Disconnect()
        {
            _communicationChannel.Disconnect();
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
        }

        /// <summary>
        /// Sends a message to server.
        /// </summary>
        /// <param name="message">Message to be sent</param>
        public void SendMessage(IScsMessage message)
        {
            if (CommunicationState != CommunicationStates.Connected)
            {
                throw new Exception("Client is not connected to the server.");
            }

            _communicationChannel.SendMessage(message);
        }

        /// <summary>
        /// This method is implemented by derived classes to create appropriate communication channel.
        /// </summary>
        /// <returns>Ready communication channel to communicate</returns>
        protected abstract ICommunicationChannel CreateCommunicationChannel();

        /// <summary>
        /// Raises Connected event.
        /// </summary>
        protected virtual void OnConnected()
        {
            if (Connected == null)
            {
                return;
            }

            try
            {
                Connected(this, new EventArgs());
            }
            catch
            {

            }
        }

        /// <summary>
        /// Raises Disconnected event.
        /// </summary>
        protected virtual void OnDisconnected()
        {
            if (Disconnected == null)
            {
                return;
            }

            try
            {
                Disconnected(this, new EventArgs());
            }
            catch
            {

            }
        }

        /// <summary>
        /// Raises MessageReceived event.
        /// </summary>
        /// <param name="message">Received message</param>
        protected virtual void OnMessageReceived(IScsMessage message)
        {
            if (MessageReceived == null || message is PingMessage)
            {
                return;
            }

            try
            {
                MessageReceived(this, new MessageEventArgs(message));
            }
            catch
            {

            }
        }

        /// <summary>
        /// Handles Elapsed event of _pingTimer to send PingMessage messages to server.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void PingTimer_Elapsed(object sender, EventArgs e)
        {
            if (CommunicationState != CommunicationStates.Connected)
            {
                return;
            }

            try
            {
                var lastMinute = DateTime.Now.AddMinutes(-1);
                if (_communicationChannel.LastReceivedMessageTime > lastMinute || _communicationChannel.LastSentMessageTime > lastMinute)
                {
                    return;
                }

                _communicationChannel.SendMessage(new PingMessage());
            }
            catch
            {

            }
        }

        /// <summary>
        /// Handles Disconnected event of _communicationChannel object.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void CommunicationChannel_Disconnected(object sender, EventArgs e)
        {
            _pingTimer.Stop();
            OnDisconnected();
        }

        /// <summary>
        /// Handles MessageReceived event of _communicationChannel object.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void CommunicationChannel_MessageReceived(object sender, MessageEventArgs e)
        {
            OnMessageReceived(e.Message);
        }
    }
}
