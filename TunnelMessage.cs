namespace HomeSeer.PluginSdk {

    public class TunnelMessage {

        public Constants.TunnelCommand Command = Constants.TunnelCommand.Unknown;
        public int           ID;
        public int           version = 1;
        public byte[]        Data    = null;

    }

}