using System;
using System.Threading;
using HSCF.CollectionsSCF;

namespace HSCF.Communication.ScsServices.Service
{
    /// <summary>
    /// Base class for all services that is serviced by IScsServiceApplication.
    /// A class must be derived from ScsService to serve as a SCS service.
    /// </summary>
    /// rjh added Serializable and MarshalByRef for ASP.NET support
    [Serializable]
    public abstract class ScsService : MarshalByRefObject
    {
        /// <summary>
        /// Active clients list.
        /// List of all clients those are currently served by this service.
        /// This list is used to find which client called service's method.
        /// See <see cref="CurrentClient"/> property.
        /// Key: ManagedThreadId of client's thread.
        /// Value: IScsServiceClient object.
        /// </summary>
        private readonly ThreadSafeSortedList<int, IScsServiceClient> _clients;

        /// <summary>
        /// Gets the current client that is called method.
        /// This property is thread-safe, if returns correct client when called in a method
        /// if the method is called by SCS system, else throws exception.
        /// </summary>
        protected IScsServiceClient CurrentClient
        {
            get { return GetCurrentClient(); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ScsService()
        {
            _clients = new ThreadSafeSortedList<int, IScsServiceClient>();
        }

        /// <summary>
        /// Adds a client to client list. This method is called before call a service method.
        /// So, service method can obtain client which called the method.
        /// </summary>
        /// <param name="client">Client to be added</param>
        internal void AddClient(IScsServiceClient client)
        {
            _clients[Thread.CurrentThread.ManagedThreadId] = client;
        }

        /// <summary>
        /// Removes a client from client list. This method is called after a service method call.
        /// </summary>
        internal void RemoveClient()
        {
            _clients.Remove(Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// This method is called from <see cref="CurrentClient"/> property.
        /// </summary>
        /// <returns>Current client</returns>
        private IScsServiceClient GetCurrentClient()
        {
            var client = _clients[Thread.CurrentThread.ManagedThreadId];
            if(client == null)
            {
                throw new Exception("Client channel can not be obtained.");                
            }

            return client;
        }
    }
}
