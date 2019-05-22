using System;
using System.Collections.Generic;
using System.Reflection;
using HSCF.CollectionsSCF;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.Messages;
using HSCF.Communication.Scs.Server;
using HSCF.Communication.ScsServices.Communication;
using HSCF.Communication.ScsServices.Communication.Messages;

namespace HSCF.Communication.ScsServices.Service
{
    /// <summary>
    /// Implements IScsServiceApplication and provides all functionallity.
    /// </summary>
    internal class ScsServiceApplication : IScsServiceApplication
    {
        #region Public events

        /// <summary>
        /// This event is raised when a new client is connected.
        /// </summary>
        public event EventHandler<ServiceClientEventArgs> ClientConnected;

        #endregion

        #region Private fields

        /// <summary>
        /// Underlying IScsServer object to accept and manage client connections.
        /// </summary>
        private readonly IScsServer _scsServer;

        /// <summary>
        /// User service objects that is used to invoke incoming method invocation requests.
        /// Key: Service interface type's name.
        /// Value: Service object.
        /// </summary>
        private readonly ThreadSafeSortedList<string, ServiceObject> _serviceObjects;

        /// <summary>
        /// All connected clients to service.
        /// Key: Client's unique Id.
        /// Value: Reference to the client.
        /// </summary>
        private readonly ThreadSafeSortedList<long, IScsServiceClient> _serviceClients;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ScsServiceApplication object.
        /// </summary>
        /// <param name="scsServer">Underlying IScsServer object to accept and manage client connections</param>
        internal ScsServiceApplication(IScsServer scsServer)
        {
            if (scsServer == null)
            {
                throw new ArgumentNullException("scsServer");
            }

            _scsServer = scsServer;
            _scsServer.ClientConnected += ScsServer_ClientConnected;
            _serviceObjects = new ThreadSafeSortedList<string, ServiceObject>();
            _serviceClients = new ThreadSafeSortedList<long, IScsServiceClient>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Starts service application.
        /// </summary>
        public void Start()
        {
            _scsServer.Start();
        }

        /// <summary>
        /// Stops service application.
        /// </summary>
        public void Stop()
        {
            _scsServer.Stop();
        }

        private int _ConnectionTimeout;
        public int ConnectionTimeout { get; set; }


        public RequestReplyMessenger<IScsServerClient> messenger { get; set; }


        /// <summary>
        /// Adds a service object to this service application.
        /// Only single service object can be added for a service interface type.
        /// </summary>
        /// <typeparam name="TServiceInterface">Service interface type</typeparam>
        /// <typeparam name="TServiceClass">Service class type. Must be delivered from ScsService and must implement TServiceInterface.</typeparam>
        /// <param name="service">An instance of TServiceClass.</param>
        public void AddService<TServiceInterface, TServiceClass>(TServiceClass service) 
            where TServiceClass : ScsService, TServiceInterface
            where TServiceInterface : class
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            var type = typeof(TServiceInterface);
            if(_serviceObjects[type.Name] != null)
            {
                throw new Exception("Service '" + type.Name + "' is already added.");                
            }

            _serviceObjects[type.Name] = new ServiceObject(type, service);
        }

        /// <summary>
        /// Removes a previously added service object from this service application.
        /// It removes object according to interface type.
        /// </summary>
        /// <typeparam name="TServiceInterface">Service interface type</typeparam>
        /// <returns>True: removed. False: no service object with this interface</returns>
        public bool RemoveService<TServiceInterface>()
            where TServiceInterface : class
        {
            return _serviceObjects.Remove(typeof(TServiceInterface).Name);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Handles ClientConnected event of _scsServer object.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void ScsServer_ClientConnected(object sender, ServerClientEventArgs e)
        {
            //var requestReplyMessenger = new RequestReplyMessenger<IScsServerClient>(e.Client);
            messenger = new RequestReplyMessenger<IScsServerClient>(e.Client);
            messenger.MessageReceived += Client_MessageReceived;
            messenger.Start();
            if (ConnectionTimeout == 0) ConnectionTimeout = 30000;
            RequestReplyMessenger<IScsServerClient>.Timeout = ConnectionTimeout;

            var serviceClient = ScsServiceClientFactory.CreateServiceClient(e.Client, messenger);
            serviceClient.Disconnected += Client_Disconnected;

            _serviceClients[serviceClient.ClientId] = serviceClient;
            OnClientConnected(serviceClient);
        }

        /// <summary>
        /// Handles MessageReceived events of all clients, evaluates each message,
        /// finds appropriate service object and invokes appropriate method.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void Client_MessageReceived(object sender, MessageEventArgs e)
        {
            //Get RequestReplyMessenger object (sender of event) to get client
            var requestReplyMessenger = (RequestReplyMessenger<IScsServerClient>) sender;

            //Cast message to ScsRemoteInvokeMessage and check it
            var invokeMessage = e.Message as ScsRemoteInvokeMessage;
            if (invokeMessage == null)
            {
                return;
            }

            try
            {
                //Get client object
                var client = _serviceClients[requestReplyMessenger.Messenger.ClientId];
                if (client == null)
                {
                    requestReplyMessenger.Messenger.Disconnect();
                    return;
                }

                //Get service object
                var serviceObject = _serviceObjects[invokeMessage.ServiceClassName];
                if (serviceObject == null)
                {
                    SendInvokeResponse(requestReplyMessenger, invokeMessage, null, new ScsRemoteException("There is no service with name '" + invokeMessage.ServiceClassName + "'"));
                    return;
                }

                //Invoke method
                try
                {
                    object returnValue;
                    //Add client to service object's client list before method invocation, so user service can get client in service method.
                    serviceObject.Service.AddClient(client);
                    try
                    {
                        returnValue = serviceObject.InvokeMethod(invokeMessage.MethodName, invokeMessage.Parameters);
                    }
                    finally
                    {
                        //Remove client from service object, since it is not neccessery anymore.
                        serviceObject.Service.RemoveClient();
                    }

                    //Send method invocation return value to the client
                    SendInvokeResponse(requestReplyMessenger, invokeMessage, returnValue, null);
                }
                catch (TargetInvocationException ex)
                {
                    var innerEx = ex.InnerException;
                    SendInvokeResponse(requestReplyMessenger, invokeMessage, null, new ScsRemoteException(innerEx.Message + Environment.NewLine + "Service Version: " + serviceObject.ServiceAttribute.Version, innerEx));
                    return;
                }
                catch (Exception ex)
                {
                    SendInvokeResponse(requestReplyMessenger, invokeMessage, null, new ScsRemoteException(ex.Message + Environment.NewLine + "Service Version: " + serviceObject.ServiceAttribute.Version, ex));
                    return;
                }
            }
            catch (Exception ex)
            {
                SendInvokeResponse(requestReplyMessenger, invokeMessage, null, new ScsRemoteException("An error occured during remote service method call.", ex));
                return;
            }
        }

        /// <summary>
        /// Sends response to the remote application that invoked a service method.
        /// </summary>
        /// <param name="client">Client that sent invoke message</param>
        /// <param name="requestMessage">Request message</param>
        /// <param name="returnValue">Return value to send</param>
        /// <param name="exception">Exception to send</param>
        private static void SendInvokeResponse(IMessenger client, IScsMessage requestMessage, object returnValue, ScsRemoteException exception)
        {
            try
            {
                client.SendMessage(
                    new ScsRemoteInvokeReturnMessage
                        {
                            RepliedMessageId = requestMessage.MessageId,
                            ReturnValue = returnValue,
                            RemoteException = exception
                        });
            }
            catch
            {

            }
        }

        /// <summary>
        /// Handles Disconnected events of all clients.
        /// </summary>
        /// <param name="sender">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void Client_Disconnected(object sender, EventArgs e)
        {
            _serviceClients.Remove(((IScsServiceClient)sender).ClientId);
        }

        /// <summary>
        /// Raises ClientConnected event.
        /// </summary>
        /// <param name="client"></param>
        private void OnClientConnected(IScsServiceClient client)
        {
            if (ClientConnected == null)
            {
                return;
            }

            try
            {
                ClientConnected(this, new ServiceClientEventArgs(client));
            }
            catch
            {

            }
        }

        #endregion

        #region ServiceObject class

        /// <summary>
        /// Represents a user service object.
        /// It is used to invoke methods on a ScsService object.
        /// </summary>
        private class ServiceObject
        {
            /// <summary>
            /// The service object that is used to invoke methods on.
            /// </summary>
            public ScsService Service { get; private set; }

            /// <summary>
            /// ScsService attribute of Service object's class.
            /// </summary>
            public ScsServiceAttribute ServiceAttribute { get; private set; }

            /// <summary>
            /// This collection stores a list of all methods of service object.
            /// Key: Method name
            /// Value: Informations about method. 
            /// </summary>
            private readonly SortedList<string, MethodInfo> _methods;

            /// <summary>
            /// Creates a new ServiceObject.
            /// </summary>
            /// <param name="serviceInterfaceType">Type of service interface</param>
            /// <param name="service">The service object that is used to invoke methods on</param>
            public ServiceObject(Type serviceInterfaceType, ScsService service)
            {
                Service = service;
                var classAttributes = serviceInterfaceType.GetCustomAttributes(typeof(ScsServiceAttribute), true);
                if (classAttributes.Length <= 0)
                {
                    throw new Exception("Service class must has ScsService attribute.");
                }

                ServiceAttribute = classAttributes[0] as ScsServiceAttribute;

                _methods = new SortedList<string, MethodInfo>();

                // need to get methods from inherited interfaces, this is a way to do that
                foreach (MethodInfo member in GetMembers(serviceInterfaceType, BindingFlags.Public | BindingFlags.Instance))
                {
                    //Console.WriteLine(member.Name);
                    _methods.Add(member.Name, member);
                }

                // Original code to get methods, does not include inherited interfaces!

                //System.Reflection.MethodInfo[] method_list = serviceInterfaceType.GetMethods(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                //foreach (var methodInfo in method_list)
                //{
                //    //Console.WriteLine(methodInfo.Name);
                //    try
                //    {
                //        _methods.Add(methodInfo.Name, methodInfo);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("Error adding method: " + methodInfo.Name);
                //        throw new Exception("Cannot add method " + methodInfo.Name + " overloads not supported in interface");
                //    }
                //}
            }

            private static ICollection<MethodInfo> GetMembers(Type type, BindingFlags flags)
            {
                HashSet<MethodInfo> members = new HashSet<MethodInfo>();
                GetMembersRecursively(type, flags, members);
                return members;
            }



            private static void GetMembersRecursively(Type type, BindingFlags flags, HashSet<MethodInfo> members)
            {
                MethodInfo[] childMembers = type.GetMethods(flags);        // type.GetMembers(flags);
                members.UnionWith(childMembers);

                foreach (Type interfaceType in type.GetInterfaces())
                {
                    GetMembersRecursively(interfaceType, flags, members);
                }
            }

            /// <summary>
            /// Invokes a method of Service object.
            /// </summary>
            /// <param name="methodName">Name of the method to invoke</param>
            /// <param name="parameters">Parameters of method</param>
            /// <returns>Return value of method</returns>
            public object InvokeMethod(string methodName, params object[] parameters)
            {
                //Check if there is a method with name methodName
                if (!_methods.ContainsKey(methodName))
                {
                    throw new Exception("There is not a method with name '" + methodName + "' in service class.");
                }

                //Get method
                var method = _methods[methodName];

                //Invoke method and return invoke result
                return method.Invoke(Service, parameters);
            }
        }

        #endregion
    }
}
