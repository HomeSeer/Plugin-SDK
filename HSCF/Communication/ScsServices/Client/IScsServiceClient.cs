using HSCF.Communication.Scs.Client;

namespace HSCF.Communication.ScsServices.Client
{
    /// <summary>
    /// Represents a service client that consumes a SCS service.
    /// </summary>
    /// <typeparam name="T">Type of service interface</typeparam>
    public interface IScsServiceClient<out T> : IConnectableClient where T : class
    {
        /// <summary>
        /// Reference to the service proxy to invoke remote service methods.
        /// </summary>
        T ServiceProxy { get; }

        /// <summary>
        /// Timeout value when invoking a service method.
        /// If timeout occurs before end of remote method call, an exception is thrown.
        /// Use -1 for no timeout (wait indefinite).
        /// </summary>
        int Timeout { get; set; }
    }
}
