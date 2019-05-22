using System.Net.Sockets;
using System.Threading;

namespace HSCF.Communication.Scs.Communication.Channels.Tcp
{
    /// <summary>
    /// This class is used to listen and accept incoming TCP connection requests on a TCP port.
    /// </summary>
    internal class TcpConnectionListener : ConnectionListenerBase
    {
        /// <summary>
        /// Listening TCP port number.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Server socket to listen incoming connection requests.
        /// </summary>
        private TcpListener _listenerSocket;

        /// <summary>
        /// The thread to listen socket
        /// </summary>
        private Thread _thread;

        /// <summary>
        /// A flag to control thread's running
        /// </summary>
        private volatile bool _running;
        
        /// <summary>
        /// Creates a new TcpConnectionListener with given tcp port.
        /// </summary>
        /// <param name="tcpPort">Listening TCP port</param>
        public TcpConnectionListener(int tcpPort)
        {
            Port = tcpPort;
        }

        /// <summary>
        /// Starts listening incoming connections.
        /// </summary>
        public override void Start()
        {
            StartSocket();
            _running = true;
            _thread = new Thread(DoListenAsThread);
            _thread.Start();
        }

        /// <summary>
        /// Stops listening incoming connections.
        /// </summary>
        public override void Stop()
        {
            _running = false;
            StopSocket();
        }

        /// <summary>
        /// Starts listening socket.
        /// </summary>
        private void StartSocket()
        {
            _listenerSocket = new TcpListener(System.Net.IPAddress.Any, Port);
            _listenerSocket.Start();
        }

        /// <summary>
        /// Stops listening socket.
        /// </summary>
        /// <returns>Indicates the result of operation</returns>
        private void StopSocket()
        {
            try
            {
                _listenerSocket.Stop();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Entrance point of the thread.
        /// This method is used by the thread to listen incoming requests.
        /// </summary>
        private void DoListenAsThread()
        {
            while (_running)
            {
                try
                {
                    var clientSocket = _listenerSocket.AcceptSocket();
                    if (clientSocket.Connected)
                    {
                        OnCommunicationChannelConnected(new TcpCommunicationChannel(clientSocket));
                    }
                }
                catch
                {
                    //Disconnect, wait for a while and connect again.
                    StopSocket();
                    Thread.Sleep(1000);
                    if (!_running)
                    {
                        return;
                    }

                    try
                    {
                        StartSocket();
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
