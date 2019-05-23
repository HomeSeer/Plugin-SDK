using System;
using System.Threading;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.EndPoints.Tcp;
using HSCF.Communication.ScsServices.Client;

namespace HomeSeer.PluginSdk {

    public class InstanceComManager {

        private const int HomeSeerPort = 10400;
        
        private static IScsServiceClient<IHsController>  Client;
        private static IScsServiceClient<IAppCallbackAPI> ClientCallback;

        private string _ipAddress = "127.0.0.1";

        private AbstractPlugin _plugin;        

        public InstanceComManager(AbstractPlugin plugin, string[] args) {
            _plugin = plugin;
            
            foreach (var arg in args) {
                var parts = arg.Split('=');
                switch (parts[0].ToLower()) {
                    case "server":
                        _ipAddress = parts[1];
                        break;
                    case "instance":
                        //TODO no more instances
                        break;
                }
            }
            
            Client         = ScsServiceClientBuilder.CreateClient<IHsController>(new ScsTcpEndPoint(_ipAddress, HomeSeerPort), _plugin);
            ClientCallback = ScsServiceClientBuilder.CreateClient<IAppCallbackAPI>(new ScsTcpEndPoint(_ipAddress, HomeSeerPort), _plugin);
        }

        public void Connect() {
            Connect(1);
        }
        
        private void Connect(int attempts) {

            try {
                Client.Connect();
                ClientCallback.Connect();
                var apiVersion = 0D;

                try {
                    _plugin.HomeSeerSystem       = Client.ServiceProxy;
                    apiVersion = _plugin.HomeSeerSystem.APIVersion;
                    Console.WriteLine("Host API Version: " + apiVersion);
                }
                catch (Exception exception) {
                    Console.WriteLine(exception);
                }

                try {
                    _plugin.ClientCallback = ClientCallback.ServiceProxy;
                    apiVersion = _plugin.ClientCallback.APIVersion;
                }
                catch (Exception exception) {
                    Console.WriteLine(exception);
                    return;
                }


            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                Console.WriteLine("Cannot connect attempt " + attempts);
                if (exception.Message.ToLower().Contains("timeout occurred.") && attempts < 6) {
                    Connect(attempts + 1);
                    if (Client != null) {
                        Client.Dispose();
                        Client = null;
                    }

                    if (ClientCallback != null) {
                        ClientCallback.Dispose();
                        ClientCallback = null;
                    }
                    return;
                }
            }
			
            Thread.Sleep(4000); //?

            try {
                _plugin.HomeSeerSystem.Connect(_plugin.Name, "primary");
                do {
                    Thread.Sleep(10);
                } while (Client.CommunicationState == CommunicationStates.Connected && !_plugin.IsShutdown);
				
                Client.Disconnect();
                ClientCallback.Disconnect();
				
                Thread.Sleep(2000); //?
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
                throw;
            }
        }

    }

}