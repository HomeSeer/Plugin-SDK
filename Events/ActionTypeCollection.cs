using System;
using System.Collections.Generic;
using System.Reflection;
using HomeSeer.PluginSdk.Devices;

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// A collection of <see cref="AbstractActionType"/>s that can be used by users to create HomeSeer Event Actions.
    /// <para>
    /// An instance of this class is a field on <see cref="AbstractPlugin"/> initialized in the constructor.
    ///  In addition, all calls from HomeSeer related to actions are automatically routed through that instance.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Add action types supported by your plugin using <see cref="AddActionType"/>
    /// <para>
    /// Due to the fact that HomeSeer saves the index of the action type in its internal database, avoid
    ///  changing the index of any of the types available to the user. Doing so may result in an incorrect
    ///  <see cref="AbstractActionType"/> being instantiated and producing errors because the configuration parameters
    ///  and signature of the action type do not match.
    /// </para>
    /// </remarks>
    public class ActionTypeCollection {
        
        /// <summary>
        /// The number of action types that are supported by the plugin
        /// </summary>
        public int Count => _actionTypes?.Count ?? 0;
        
        /// <summary>
        /// Used to enable/disable internal logging to the console
        /// <para>
        /// When it is TRUE, log messages from the PluginSdk code will be written to the Console
        /// </para>
        /// </summary>
        public bool LogDebug { get; set; }

        private IActionTypeListener _listener;

        private List<Type> _actionTypes = new List<Type>();
        private HashSet<string> _actionTypeNames = new HashSet<string>();

        /// <summary>
        /// Initialize a new instance of an <see cref="ActionTypeCollection"/> with the specified listener
        /// </summary>
        /// <param name="listener">An <see cref="IActionTypeListener"/> that will be passed to action types</param>
        public ActionTypeCollection(IActionTypeListener listener) {
            _listener = listener;
        }

        /// <summary>
        /// Add the specified class type that derives from <see cref="AbstractActionType"/> to the list of actions
        /// </summary>
        /// <param name="actionType">
        /// The <see cref="Type"/> of the class that derives from <see cref="AbstractActionType"/>
        /// </param>
        /// <exception cref="ArgumentException">Thrown when the specified class type will not work as an action</exception>
        public void AddActionType(Type actionType) {
            if (actionType?.FullName == null) {
                throw new ArgumentException(nameof(actionType));
            }
            if (!actionType.IsSubclassOf(typeof(AbstractActionType))) {
                throw new ArgumentException($"{actionType} is not derived from AbstractActionType", nameof(actionType));
            }
            AssertTypeHasConstructors(actionType);
            if (_actionTypeNames.Contains(actionType.FullName)) {
                throw new ArgumentException($"{actionType.FullName} has already been added to the collection.");
            }

            if (_actionTypeNames.Add(actionType.FullName)) {
                _actionTypes.Add(actionType);
            }
        }

        /// <summary>
        /// Get the name of the action type at the specified index
        /// </summary>
        /// <param name="actionIndex">The 1 based index of the action type to get the name for</param>
        /// <returns>The name of the action type</returns>
        public string GetName(int actionIndex) {
            
            try {
                var targetAct = GetObjectFromInfo(actionIndex-1);
                return targetAct.Name;
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return "Error retrieving action name";
            }
        }

        /// <summary>
        /// Called by HomeSeer when it needs to display the configuration UI for an action type
        /// </summary>
        /// <param name="actInfo">The action to display as defined by <see cref="TrigActInfo"/></param>
        /// <returns>HTML to display on the event page for the specified action</returns>
        public string OnGetActionUi(TrigActInfo actInfo) {

            try {
                var curAct = GetObjectFromActInfo(actInfo);
                //curAct = _listener?.OnBuildActionUi(curAct) ?? curAct;
                return curAct.ToHtml();
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return exception.Message;
            }
            
        }

        /// <summary>
        /// Called by HomeSeer when a user updates the configuration of an action and those changes
        ///  are in need of processing.
        /// </summary>
        /// <param name="postData">A <see cref="Dictionary"/> of changes to the action configuration</param>
        /// <param name="actInfo">The action being configured</param>
        /// <returns>
        /// An <see cref="EventUpdateReturnData"/> describing the new state of the action that will be saved by HomeSeer.
        ///  The action configuration will be saved if the result returned is an empty string.
        /// </returns>
        public EventUpdateReturnData OnUpdateActionConfig(Dictionary<string, string> postData, TrigActInfo actInfo) {
            
            var eurd = new EventUpdateReturnData {
                         Result = "",
                         TrigActInfo = actInfo
                     };

            try {
                var curAct = GetObjectFromActInfo(actInfo);
                var result = curAct.ProcessPostData(postData);
                //curAct = _listener?.OnActionConfigChange(curAct) ?? curAct;
                eurd.Result = result ? "" : "Unknown Plugin Error";
                eurd.DataOut = curAct.Data;
                return eurd;
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                eurd.Result = exception.Message;
                return eurd;
            }
        }

        /// <summary>
        /// Called by HomeSeer when it needs to determine if a specific action is completely configured
        ///  or requires additional configuration before it can be used.
        /// </summary>
        /// <param name="actInfo">The action to check</param>
        /// <returns>
        /// TRUE if the action is completely configured,
        ///  FALSE if it is not. A call to <see cref="OnGetActionUi"/> will be called following this if FALSE is returned.
        /// </returns>
        public bool IsActionConfigured(TrigActInfo actInfo) {

            try {
                var curAct = GetObjectFromActInfo(actInfo);
                return curAct.IsFullyConfigured();
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return true;
            }
        }

        /// <summary>
        /// Called by HomeSeer when it needs to get an easy to read, formatted string that communicates what
        ///  the action does to the user.
        /// </summary>
        /// <param name="actInfo">The action that a pretty string is needed for</param>
        /// <returns>
        /// HTML formatted text communicating what the action does
        /// </returns>
        public string OnGetActionPrettyString(TrigActInfo actInfo) {
            
            try {
                var curAct = GetObjectFromActInfo(actInfo);
                return curAct.GetPrettyString();
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return exception.Message;
            }
        }

        /// <summary>
        /// Called by HomeSeer when an event has been triggered and a corresponding action needs to be processed.
        /// </summary>
        /// <param name="actInfo">The action that is being executed</param>
        /// <returns>
        /// TRUE if the action executed successfully,
        ///  FALSE if there was an error executing the action.
        /// </returns>
        public bool HandleAction(TrigActInfo actInfo) {
            try {
                var curAct = GetObjectFromActInfo(actInfo);
                //curAct = _listener?.BeforeRunAction(curAct) ?? curAct;
                return curAct.OnRunAction();
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return false;
            }
        }

        /// <summary>
        /// Called by HomeSeer when it needs to determine if a specific device/feature is referenced by a
        ///  particular action.
        /// </summary>
        /// <param name="devOrFeatRef">The unique <see cref="AbstractHsDevice.Ref"/> of the device/feature</param>
        /// <param name="actInfo">The action to check</param>
        /// <returns>
        /// TRUE if the action references the specified device/feature,
        ///  FALSE if it does not.
        /// </returns>
        public bool ActionReferencesDeviceOrFeature(int devOrFeatRef, TrigActInfo actInfo) {
            try {
                var curAct = GetObjectFromActInfo(actInfo);
                return curAct.ReferencesDeviceOrFeature(devOrFeatRef);
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return false;
            }
        }

        private AbstractActionType GetObjectFromActInfo(TrigActInfo actInfo) {
            return GetObjectFromInfo(actInfo.TANumber-1, actInfo.UID, actInfo.evRef, actInfo.DataIn ?? new byte[0]);
        }

        private AbstractActionType GetObjectFromInfo(int actNumber, params object[] actInfoParams) {
            if (_actionTypes.Count < actNumber) {
                throw new KeyNotFoundException("No action type exists with that number");
            }

            var targetType = _actionTypes[actNumber];
            var paramTypes = new List<Type>();
            foreach (var infoParam in actInfoParams) {
                paramTypes.Add(infoParam.GetType());
            }
            var typeConstructor = targetType.GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                                                            null,
                                                            CallingConventions.Standard,
                                                            paramTypes.ToArray(),
                                                            null);

            if (typeConstructor == null) {
                throw new TypeLoadException("Cannot find the correct constructor for this action type");
            }

            //new object[] { actInfo.UID, actInfo.evRef, actInfo.DataIn };
            if (!(typeConstructor.Invoke(actInfoParams) is AbstractActionType curAct)) {
                throw new TypeLoadException("This constructor did not produce a class derived from AbstractActionType");
            }

            curAct.LogDebug = LogDebug;
            curAct.ActionListener = _listener;
            return curAct;
        }

        private static void AssertTypeHasConstructors(Type targetType) {
            var paramTypes = new[] {
                                       typeof(int),
                                       typeof(int),
                                       typeof(byte[])
                                   };
            var typeConstructor = targetType.GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                                                            null,
                                                            CallingConventions.Standard,
                                                            paramTypes,
                                                            null);
            if (typeConstructor == null) {
                throw new TypeLoadException("Type does not have a constructor with parameters (int id, int eventRef, byte[] dataIn)");
            }

            paramTypes = new Type[0];
            typeConstructor = targetType.GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                                                        null,
                                                        CallingConventions.Standard,
                                                        paramTypes, 
                                                        null);
            if (typeConstructor == null) {
                throw new TypeLoadException("Type does not have a constructor with parameters ()");
            }
        }
        
        /// <summary>
        /// The base implementation of an action type interface that facilitates communication between
        ///  <see cref="AbstractActionType"/>s and the <see cref="AbstractPlugin"/> that owns them.
        /// <para>
        /// Extend this interface and have your <see cref="AbstractPlugin"/> implementation inherit it to make it
        ///  accessible through the <see cref="AbstractActionType.ActionListener"/> field.
        /// </para>
        /// </summary>
        public interface IActionTypeListener {}

    }

}