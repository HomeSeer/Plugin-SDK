using HSCF.Communication.Scs.Communication.Channels;
using HSCF.Communication.Scs.Communication.Channels.Tcp;

namespace HSCF.Communication.Scs.Server.Tcp
{
    /// <summary>
    /// This class is used to create a TCP server.
    /// </summary>
    internal class ScsTcpServer : ScsServerBase
    {
        /// <summary>
        /// TCP port of server to listen incoming connections.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Creates a new ScsTcpServer object.
        /// </summary>
        /// <param name="port">TCP port of server to listen incoming connections</param>
        public ScsTcpServer(int port)
        {
            Port = port;
        }

        /// <summary>
        /// Creates a TCP connection listener.
        /// </summary>
        /// <returns></returns>
        protected override IConnectionListener CreateConnectionListener()
        {
            return new TcpConnectionListener(Port);
        }
    }
}
