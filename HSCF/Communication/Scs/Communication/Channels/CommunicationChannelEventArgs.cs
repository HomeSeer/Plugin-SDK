using System;
using HSCF.Communication.Scs.Communication.Channels.Tcp;

namespace HSCF.Communication.Scs.Communication.Channels
{
    /// <summary>
    /// Stores communication channel information to be used by an event.
    /// </summary>
    internal class CommunicationChannelEventArgs : EventArgs
    {
        /// <summary>
        /// Communication channel that is associated with this event.
        /// </summary>
        public ICommunicationChannel Channel { get; private set; }
        public String ipaddress;
        /// <summary>
        /// Creates a new CommunicationChannelEventArgs object.
        /// </summary>
        /// <param name="channel">Communication channel that is associated with this event</param>
        //public CommunicationChannelEventArgs(ICommunicationChannel channel)
        public CommunicationChannelEventArgs(TcpCommunicationChannel channel)
        {
            Channel = channel;
            ipaddress = channel._clientSocket.RemoteEndPoint.ToString();
        }
    }
}
