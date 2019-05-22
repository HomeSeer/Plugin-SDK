using System;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.Messages;
using HSCF.Communication.Scs.Communication.Channels;
using HSCF.Communication.Scs.Communication.Protocols;

namespace HSCF.Communication.Scs.Server
{
    /// <summary>
    /// This class provides base functionality for client classes.
    /// </summary>
    internal class ScsServerClient : IScsServerClient
    {

        public String ipaddress { get; set; }

        /// <summary>
        /// Unique identifier for this client in server.
        /// </summary>
        public long ClientId { get; set; }

        /// <summary>
        /// This event is raised when a new message is received.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        /// <summary>
        /// Gets/sets wire protocol that is used while reading and writing messages.
        /// </summary>
        public IScsWireProtocol WireProtocol
        {
            get { return _communicationChannel.WireProtocol; }
            set { _communicationChannel.WireProtocol = value; }
        }

        /// <summary>
        /// This event is raised when client is disconnected from server.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// Gets the communication state of the Client.
        /// </summary>
        public CommunicationStates CommunicationState
        {
            get
            {
                return _communicationChannel.CommunicationState;
            }
        }

        /// <summary>
        /// Gets the time of the last succesfully received message.
        /// </summary>
        public DateTime LastReceivedMessageTime
        {
            get
            {
                return _communicationChannel.LastReceivedMessageTime;
            }
        }

        /// <summary>
        /// Gets the time of the last succesfully received message.
        /// </summary>
        public DateTime LastSentMessageTime
        {
            get
            {
                return _communicationChannel.LastSentMessageTime;
            }
        }

        /// <summary>
        /// The communication channel that is used by client to send and receive messages.
        /// </summary>
        protected ICommunicationChannel _communicationChannel;

        /// <summary>
        /// Creates a new ScsClient object.
        /// </summary>
        /// <param name="communicationChannel">The communication channel that is used by client to send and receive messages</param>
        public ScsServerClient(ICommunicationChannel communicationChannel)
        {
            _communicationChannel = communicationChannel;
            _communicationChannel.MessageReceived += CommunicationChannel_MessageReceived;
            _communicationChannel.Disconnected += CommunicationChannel_Disconnected;
        }

        /// <summary>
        /// Disconnects from client and closes underlying communication channel.
        /// </summary>
        public void Disconnect()
        {
            _communicationChannel.Disconnect();
        }

        /// <summary>
        /// Sends a message to the client.
        /// </summary>
        /// <param name="message">Message to be sent</param>
        public void SendMessage(IScsMessage message)
        {
            _communicationChannel.SendMessage(message);
        }

        /// <summary>
        /// Raises Disconnected event.
        /// </summary>
        private void OnDisconnected()
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
        private void OnMessageReceived(IScsMessage message)
        {
            if (message is PingMessage)
            {
                _communicationChannel.SendMessage(new PingMessage {RepliedMessageId = message.MessageId});
            }

            if (MessageReceived == null)
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
        /// Handles Disconnected event of _communicationChannel object.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void CommunicationChannel_Disconnected(object sender, EventArgs e)
        {
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
