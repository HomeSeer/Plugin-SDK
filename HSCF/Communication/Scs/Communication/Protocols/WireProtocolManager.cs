namespace HSCF.Communication.Scs.Communication.Protocols
{
    /// <summary>
    /// This class is used to simplify using WireProtocols.
    /// </summary>
    internal static class WireProtocolManager
    {
        /// <summary>
        /// Gets the current wire protocol to be used while communicating.
        /// </summary>
        /// <returns>Current wire protocol</returns>
        public static IScsWireProtocol GetDefaultWireProtocol()
        {
            return new BinarySerializationProtocol();
        }
    }
}
