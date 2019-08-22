using System;
using System.Collections.Generic;
using System.Reflection;
using HomeSeer.PluginSdk.Devices;

namespace HomeSeer.PluginSdk.Events {

    public class ActionTypeCollection {
        
        public bool LogDebug { get; set; } = false;

        private IActionTypeListener _listener;

        private List<Type> _actionTypes = new List<Type>();
        private HashSet<string> _actionTypeNames = new HashSet<string>();

        public ActionTypeCollection(IActionTypeListener listener) {
            _listener = listener;
        }

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

        public virtual string GetName(int actionIndex) {
            
            try {
                var targetAct = GetObjectFromInfo(actionIndex);
                return targetAct.Name;
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return "Error retrieving action name";
            }
        }

        public virtual string OnGetActionUi(TrigActInfo actInfo) {

            try {
                var curAct = GetObjectFromActInfo(actInfo);
                curAct = _listener?.OnBuildActionUi(curAct) ?? curAct;
                return curAct.ToHtml();
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return exception.Message;
            }
            
        }

        public virtual MultiReturn OnUpdateActionConfig(Dictionary<string, string> postData, TrigActInfo actInfo) {
            
            var mr = new MultiReturn {
                         sResult = "",
                         TrigActInfo = actInfo
                     };

            try {
                var curAct = GetObjectFromActInfo(actInfo);
                var result = curAct.ProcessPostData(postData);
                curAct = _listener?.OnActionConfigChange(curAct) ?? curAct;
                mr.sResult = result ? "" : "Unknown Plugin Error";
                mr.DataOut = curAct.Data;
                return mr;
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                mr.sResult = exception.Message;
                return mr;
            }
        }

        public virtual bool IsActionConfigured(TrigActInfo actInfo) {

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

        public virtual string OnGetActionPrettyString(TrigActInfo actInfo) {
            
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

        //TODO update this
        public virtual bool HandleAction(TrigActInfo actInfo) {
            try {
                var curAct = GetObjectFromActInfo(actInfo);
                curAct = _listener?.BeforeRunAction(curAct) ?? curAct;
                return curAct.OnRunAction();
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return false;
            }
        }

        public virtual bool ActionReferencesDeviceOrFeature(int devOrFeatRef, TrigActInfo actInfo) {
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
            return GetObjectFromInfo(actInfo.TANumber, actInfo.UID, actInfo.evRef, actInfo.DataIn);
        }

        private AbstractActionType GetObjectFromInfo(int actNumber, params object[] actInfoParams) {
            if (_actionTypes.Count >= actNumber) {
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
        
        public interface IActionTypeListener {

            /// <summary>
            /// Called during HomeSeer's request for the HTML to display for a given event action.
            /// <para>
            /// Use this opportunity to load in any additional data needed for the action configuration
            /// </para>
            /// </summary>
            /// <param name="action">The action being configured</param>
            /// <returns>The modified action being configured</returns>
            AbstractActionType OnBuildActionUi(AbstractActionType action);
            /// <summary>
            /// Called when configuration changes to an action needs to be processed.
            /// <para>
            /// Use this opportunity to load in any additional data needed for the action configuration
            /// </para>
            /// </summary>
            /// <param name="action">The action being configured</param>
            /// <returns>The modified action being configured</returns>
            AbstractActionType OnActionConfigChange(AbstractActionType action);
            /// <summary>
            /// Called before an action is executed
            /// <para>
            /// Use this opportunity to load in any additional data needed for the operation of the action
            /// </para>
            /// </summary>
            /// <param name="action">The action being executed</param>
            /// <returns>The modified action to execute</returns>
            AbstractActionType BeforeRunAction(AbstractActionType action);

        }

    }

}