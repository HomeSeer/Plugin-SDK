using System;
using System.Collections.Generic;
using System.Threading;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.Messages;
using HSCF.Communication.Scs.Communication.Protocols;
using HSCF.Threading;

namespace HSCF.Communication.ScsServices.Communication
{
    /// <summary>
    /// This class adds SendAndWaitResponse(...) method to a IMessenger for synchronous request/response style messaging.
    /// It also adds queued processing of incoming messages.
    /// </summary>
    /// <typeparam name="T">Type of IMessenger object to use as underlying communication</typeparam>
    public class RequestReplyMessenger<T> : IMessenger where T : IMessenger
    {
        /// <summary>
        /// This event is raised when a new message is received from underlying messenger.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReceived;

        /// <summary>
        /// Gets/sets wire protocol that is used while reading and writing messages.
        /// </summary>
        public IScsWireProtocol WireProtocol
        {
            get { return Messenger.WireProtocol; }
            set { Messenger.WireProtocol = value; }
        }

        /// <summary>
        /// Gets the time of the last succesfully received message.
        /// </summary>
        public DateTime LastReceivedMessageTime
        {
            get
            {
                return Messenger.LastReceivedMessageTime;
            }
        }

        /// <summary>
        /// Gets the time of the last succesfully received message.
        /// </summary>
        public DateTime LastSentMessageTime
        {
            get
            {
                return Messenger.LastSentMessageTime;
            }
        }

        /// <summary>
        /// Gets the underlying IMessenger object.
        /// </summary>
        public T Messenger { get; private set; }

        /// <summary>
        /// Timeout value as milliseconds to wait on SendMessageAndWaitForResponse method.
        /// Default value: 60000 (1 minute).
        /// </summary>
        public static int Timeout { get; set; }     // rjh changed this to static so it can be changed on the fly (change needed when HS is being shut down)

        /// <summary>
        /// Default Timeout value
        /// </summary>
        private const int DefaultTimeout = 60000;

        /// <summary>
        /// Incoming message queue.
        /// </summary>
        private readonly QueueProcessorThread<MessageEventArgs> _incomingMessageQueue;

        /// <summary>
        /// This messages are waiting for a response.
        /// Key: MessageID of waiting request message.
        /// Value: A WaitingMessage instance.
        /// </summary>
        private readonly SortedList<string, WaitingMessage> _waitingMessages;

        /// <summary>
        /// Creates a new RequestReplyMessenger.
        /// </summary>
        /// <param name="messenger">IMessenger object to use as underlying communication</param>
        public RequestReplyMessenger(T messenger)
        {
            Messenger = messenger;
            messenger.MessageReceived += Messenger_MessageReceived;
            _incomingMessageQueue = new QueueProcessorThread<MessageEventArgs>();
            _incomingMessageQueue.ProcessItem += IncomingMessageQueue_ProcessItem;
            _waitingMessages = new SortedList<string, WaitingMessage>();
            Timeout = DefaultTimeout;
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message">Message to be sent</param>
        public void SendMessage(IScsMessage message)
        {
            Messenger.SendMessage(message);
        }

        /// <summary>
        /// Sends a message and waits a response for that message.
        /// </summary>
        /// <param name="message">message to send</param>
        /// <returns>Response message</returns>
        public IScsMessage SendMessageAndWaitForResponse(IScsMessage message)
        {
            try
            {
                return SendMessageAndWaitForResponse(message, Timeout);
            }
            catch (Exception e)
            {
            }
            return null;
        }

        /// <summary>
        /// Sends a message and waits a response for that message.
        /// </summary>
        /// <param name="message">message to send</param>
        /// <param name="timeoutMilliseconds">Timeout duration as milliseconds.</param>
        /// <returns>Response message</returns>
        public IScsMessage SendMessageAndWaitForResponse(IScsMessage message, int timeoutMilliseconds)
        {
            //Create a waiting message record and add to list
            var waitingMessage = new WaitingMessage();
            lock (_waitingMessages)
            {
                _waitingMessages[message.MessageId] = waitingMessage;
            }

            try
            {
                //Send message
                Messenger.SendMessage(message);

                //Wait for response
                waitingMessage.WaitEvent.WaitOne(timeoutMilliseconds);
                
                //Check for exceptions
                switch (waitingMessage.State)
                {
                    case WaitingMessageStates.WaitingForResponse:
                        throw new Exception("Timeout occured. Did not receive a response.");
                    case WaitingMessageStates.Cancelled:
                        throw new Exception("Disconnected before response received.");
                }

                //return response message
                return waitingMessage.ResponseMessage;
            }
            finally
            {
                //Remove message from waiting messages
                lock (_waitingMessages)
                {
                    if (_waitingMessages.ContainsKey(message.MessageId))
                    {
                        _waitingMessages.Remove(message.MessageId);
                    }
                }
            }
        }

        /// <summary>
        /// Starts the messenger.
        /// </summary>
        public void Start()
        {
            _incomingMessageQueue.Start();
        }

        /// <summary>
        /// Stops the messenger.
        /// Cancels all waiting threads in SendMessageAndWaitForResponse method and stops message queue.
        /// SendMessageAndWaitForResponse method throws exception if there is a thread that is waiting for response message.
        /// </summary>
        public void Stop()
        {
            _incomingMessageQueue.Stop();

            //Pulse waiting threads for incoming messages, since underlying messenger is disconnected and can not receive messages anymore.
            lock (_waitingMessages)
            {
                foreach (var waitingMessage in _waitingMessages.Values)
                {
                    waitingMessage.State = WaitingMessageStates.Cancelled;
                    waitingMessage.WaitEvent.Set();
                }

                _waitingMessages.Clear();
            }
        }

        /// <summary>
        /// Raises MessageReceived event.
        /// </summary>
        /// <param name="message">Received message</param>
        protected virtual void OnMessageReceived(IScsMessage message)
        {
            if (MessageReceived != null)
            {
                try
                {
                    MessageReceived(this, new MessageEventArgs(message));
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Handles MessageReceived event of Messenger object.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void Messenger_MessageReceived(object sender, MessageEventArgs e)
        {
            //Check if there is a waiting thread for this message (in SendMessageAndWaitForResponse method)
            //if (e.Message == null) return;  // rjh
            if (!string.IsNullOrEmpty(e.Message.RepliedMessageId))
            {
                WaitingMessage waitingMessage = null;
                lock (_waitingMessages)
                {
                    if (_waitingMessages.ContainsKey(e.Message.RepliedMessageId))
                    {
                        waitingMessage = _waitingMessages[e.Message.RepliedMessageId];
                    }
                }

                //If there is a thread waiting for this response message, pulse it
                if (waitingMessage != null)
                {
                    waitingMessage.ResponseMessage = e.Message;
                    waitingMessage.State = WaitingMessageStates.ResponseReceived;
                    waitingMessage.WaitEvent.Set();
                    return;
                }
            }

            _incomingMessageQueue.Add(e);
        }

        /// <summary>
        /// Handles ProcessItem event of _incomingMessageQueue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IncomingMessageQueue_ProcessItem(object sender, ProcessQueueItemEventArgs<MessageEventArgs> e)
        {
            OnMessageReceived(e.ProcessItem.Message);
        }

        #region WaitingMessage class

        /// <summary>
        /// This class is used to store messaging context for a request message
        /// until response is received.
        /// </summary>
        private sealed class WaitingMessage
        {
            /// <summary>
            /// Response message for request message (null if response
            /// is not received yet).
            /// </summary>
            public IScsMessage ResponseMessage { get; set; }

            /// <summary>
            /// ManualResetEvent to block thread until response is received.
            /// </summary>
            public ManualResetEvent WaitEvent { get; private set; }

            /// <summary>
            /// State of the request message.
            /// </summary>
            public WaitingMessageStates State { get; set; }

            /// <summary>
            /// Creates a new WaitingMessage object.
            /// </summary>
            public WaitingMessage()
            {
                WaitEvent = new ManualResetEvent(false);
                State = WaitingMessageStates.WaitingForResponse;
            }
        }

        /// <summary>
        /// This enum is used to store the state of a waiting message.
        /// </summary>
        private enum WaitingMessageStates
        {
            /// <summary>
            /// Still waiting for response.
            /// </summary>
            WaitingForResponse,

            /// <summary>
            /// Message sending is cancelled.
            /// </summary>
            Cancelled,

            /// <summary>
            /// Response is properly received.
            /// </summary>
            ResponseReceived
        }

        #endregion
    }
}
