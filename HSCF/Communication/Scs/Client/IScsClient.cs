using System;
using HSCF.Communication.Scs.Communication;

namespace HSCF.Communication.Scs.Client
{
    /// <summary>
    /// Represents a client to connect to server.
    /// </summary>
    public interface IScsClient : IMessenger, IConnectableClient
    {

    }
}
