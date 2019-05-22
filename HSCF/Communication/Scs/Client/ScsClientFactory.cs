using HSCF.Communication.Scs.Communication.EndPoints;

namespace HSCF.Communication.Scs.Client
{
    /// <summary>
    /// This class is used to create Clients to connect to server.
    /// </summary>
    public static class ScsClientFactory
    {
        /// <summary>
        /// Creates a new client to connect to a server using an end point.
        /// </summary>
        /// <param name="endpoint">End point of the server to connect it</param>
        /// <returns>Created TCP client</returns>
        public static IScsClient CreateClient(ScsEndPoint endpoint)
        {
            return endpoint.CreateClient();
        }
    }
}
