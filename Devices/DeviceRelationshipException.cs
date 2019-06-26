using System;

namespace HomeSeer.PluginSdk.Devices {

    public class DeviceRelationshipException : Exception {

        public DeviceRelationshipException() { }
        public DeviceRelationshipException(string message) : base(message) { }
        public DeviceRelationshipException(string message, Exception innerException) : base(message, innerException) { }

    }

}