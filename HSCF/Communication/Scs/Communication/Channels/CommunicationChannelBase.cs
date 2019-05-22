using System;
using HSCF.Communication.Scs.Communication.Messages;
using HSCF.Communication.Scs.Communication.Protocols;

namespace HSCF.Communication.Scs.Communication.Channels
{
    /// <summary>
    /// This class provides base functionality for all communication channel classes.
    /// </summary>
    internal abstract class CommunicationChannelBase : ICommunicationChannel
    {
        /// <summary>
        /// This event is raised when a new message is received.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        /// <summary>
        /// This event is raised when communication channel closed.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// Gets the current communication state.
        /// </summary>
        public CommunicationStates CommunicationState { get; private set; }

        /// <summary>
        /// Gets/sets wire protocol that the channel uses.
        /// </summary>
        public IScsWireProtocol WireProtocol { get; set; }

        /// <summary>
        /// Gets the time of the last succesfully received message.
        /// </summary>
        public DateTime LastReceivedMessageTime { get; private set; }

        /// <summary>
        /// Gets the time of the last succesfully sent message.
        /// </summary>
        public DateTime LastSentMessageTime { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CommunicationChannelBase()
        {
            CommunicationState = CommunicationStates.Disconnected;
        }

        /// <summary>
        /// Starts the communication with remote application.
        /// </summary>
        public void Start()
        {
            StartInternal();
            CommunicationState = CommunicationStates.Connected;
        }

        /// <summary>
        /// Disconnects from remote application and closes channel.
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// Sends a message to the remote application.
        /// </summary>
        /// <param name="message">Message to be sent</param>
        public void SendMessage(IScsMessage message)
        {
            try
            {
                SendMessageInternal(message);
                LastSentMessageTime = DateTime.Now;
            }
            catch (Exception e)
            {
            }
        }
        
        /// <summary>
        /// Starts the communication with remote application really.
        /// </summary>
        protected abstract void StartInternal();

        /// <summary>
        /// Sends a message to the remote application.
        /// This method is overrided by derived classes to really send to message.
        /// </summary>
        /// <param name="message">Message to be sent</param>
        protected abstract void SendMessageInternal(IScsMessage message);
        
        /// <summary>
        /// Raises Disconnected event.
        /// </summary>
        protected virtual void OnDisconnected()
        {
            CommunicationState = CommunicationStates.Disconnected;

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
            LastReceivedMessageTime = DateTime.Now;
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
    }
}
