using System;
using HSCF.Communication.Scs.Communication.Channels.Tcp;

namespace HSCF.Communication.Scs.Communication.Channels
{
    /// <summary>
    /// This class provides base functionality for communication listener classes.
    /// </summary>
    internal abstract class ConnectionListenerBase : IConnectionListener
    {
        /// <summary>
        /// This event is raised when a new communication channel is connected.
        /// </summary>
        public event EventHandler<CommunicationChannelEventArgs> CommunicationChannelConnected;

        /// <summary>
        /// Starts listening incoming connections.
        /// </summary>
        public abstract void Start();
        
        /// <summary>
        /// Stops listening incoming connections.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Raises CommunicationChannelConnected event.
        /// </summary>
        /// <param name="client"></param>
        /// TcpCommunicationChannel
        //protected virtual void OnCommunicationChannelConnected(ICommunicationChannel client)
        protected virtual void OnCommunicationChannelConnected(TcpCommunicationChannel client)
        {

            //socket s = client._clientSocket;


            if (CommunicationChannelConnected == null)
            {
                return;
            }

            try
            {
                CommunicationChannelConnected(this, new CommunicationChannelEventArgs(client));
            }
            catch
            {

            }
        }
    }
}
