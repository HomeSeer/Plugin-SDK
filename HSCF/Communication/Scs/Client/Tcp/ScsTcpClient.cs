using HSCF.Communication.Scs.Communication.Channels;
using HSCF.Communication.Scs.Communication.Channels.Tcp;
using System.Net;

namespace HSCF.Communication.Scs.Client.Tcp
{
    /// <summary>
    /// This class is used to communicate with server over TCP/IP protocol.
    /// </summary>
    internal class ScsTcpClient : ScsClientBase
    {
        /// <summary>
        /// IP Address of the server.
        /// </summary>
        public string ServerIpAddress { get; private set; }

        /// <summary>
        /// TCP port of the server.
        /// </summary>
        public int ServerPort { get; private set; }

        /// <summary>
        /// Timeout value if can not connect to the server.
        /// </summary>
        private const int ConnectionAttemptTimeout = 15000; //15 seconds.

        /// <summary>
        /// Creates a new ScsTcpClient object.
        /// </summary>
        /// <param name="serverIpAddress">IP Address of the server</param>
        /// <param name="serverPort">TCP port of the server</param>
        public ScsTcpClient(string serverIpAddress, int serverPort)
        {
            ServerIpAddress = serverIpAddress;
            ServerPort = serverPort;
        }

        /// <summary>
        /// Creates a communication channel using ServerIpAddress and ServerPort.
        /// </summary>
        /// <returns>Ready communication channel to communicate</returns>
        protected override ICommunicationChannel CreateCommunicationChannel()
        {
            var socket = TcpHelper.ConnectToServer(new IPEndPoint(IPAddress.Parse(ServerIpAddress), ServerPort), ConnectionAttemptTimeout);
            socket.NoDelay = true;
            return new TcpCommunicationChannel(socket);
        }
    }
}
