using System;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.IO;
using HSCF.Communication.Scs.Communication.Messages;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HSCF.Communication.Scs.Communication.Channels.Tcp
{
    /// <summary>
    /// This class is used to communicate with a remote application over TCP/IP protocol.
    /// </summary>
    internal class TcpCommunicationChannel : CommunicationChannelBase
    {
        /// <summary>
        /// Socket object to send/reveice messages.
        /// </summary>
        public readonly Socket _clientSocket;

        /// <summary>
        /// Network stream to send/receive messages over _clientSocket.
        /// </summary>
        private readonly NetworkStream _networkStream;

        /// <summary>
        /// Message listening thread from remote application.
        /// </summary>
        private Thread _thread;

        /// <summary>
        /// A flag to control thread's running
        /// </summary>
        private volatile bool _running;
        
        /// <summary>
        /// This object is just used for thread synchronizing (locking).
        /// </summary>
        private readonly object _syncLock;

        /// <summary>
        /// Creates a new TcpCommunicationChannel object.
        /// </summary>
        /// <param name="clientSocket">A connected Socket object that is used to communicate over</param>
        public TcpCommunicationChannel(Socket clientSocket)
        {
            _clientSocket = clientSocket;
            _clientSocket.NoDelay = true;
            _networkStream = new NetworkStream(_clientSocket);
            _syncLock = new object();
        }

        /// <summary>
        /// Starts the thread to receive messages from socket.
        /// </summary>
        protected override void StartInternal()
        {
            _running = true;
            _thread = new Thread(ListenMessages);
            _thread.Start();
        }

        /// <summary>
        /// Disconnects from remote application and closes channel.
        /// </summary>
        public override void Disconnect()
        {
            _running = false;
            try
            {
                if (_clientSocket.Connected)
                {
                    _clientSocket.Close();
                }
            }
            catch
            {
                
            }
        }


        /// <summary>
        /// Sends a message to the remote application.
        /// </summary>
        /// <param name="message">Message to be sent</param>
#if false
protected override void SendMessageInternal(IScsMessage message)
{
    //Create a byte array from message
    var memoryStream = new MemoryStream();
    WireProtocol.WriteMessage(message, memoryStream);
    var sendBuffer = memoryStream.ToArray();

    //Send message
    var totalSent = 0;
    lock (_syncLock)
    {
        while (totalSent < sendBuffer.Length)
        {
            var sent = _clientSocket.Send(sendBuffer, totalSent, sendBuffer.Length - totalSent, SocketFlags.None);

            if (sent <= 0)
            {
                throw new Exception("Message can not be sent via TCP socket. Only " + totalSent + " bytes of " + sendBuffer.Length + " bytes are sent.");
            }

            totalSent += sent;
        }
    }
}
#endif

#if true
        protected override void SendMessageInternal(IScsMessage message)
        {
            // rjh 10/23/14 ver 1.0.3
            // changed comms to send byte count first then data, see deserialize code for explanation

            lock (_syncLock)
            {
                var memoryStream = new MemoryStream();
                //WireProtocol.WriteMessage(message, memoryStream);
                new BinaryFormatter().Serialize(memoryStream, message);
                byte[] sb = memoryStream.ToArray();

                //Console.WriteLine("Send: " + sb.Length.ToString());
                try
                {
                    byte[] header = new byte[2];
                    header[0] = 0xFD;
                    header[1] = 0xFE;
                    _networkStream.Write(header, 0, 2);

                    byte[] userDataLen = BitConverter.GetBytes((Int32)sb.Length);
                    _networkStream.Write(userDataLen, 0, 4);

                    _networkStream.Write(sb, 0, sb.Length);
                    _networkStream.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error sending over stream: " + e.Message);
                }
            }
            return;
        }
#endif

        /// <summary>
        /// This is the thread method to listen and receive incoming messages.
        /// </summary>
        private void ListenMessages()
        {
            try
            {
                while (_running)
                {
                    OnMessageReceived(WireProtocol.ReadMessage(_networkStream));
                }
            }
            catch(Exception e)
            {
                //Console.WriteLine("Error in ListenMessages: " + e.Message);
                //Console.WriteLine("Error deserializing message: " + e.Message);
            }
            OnDisconnected();
        }
    }
}
