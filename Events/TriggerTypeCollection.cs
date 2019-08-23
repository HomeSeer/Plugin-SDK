using System;
using System.Collections.Generic;
using System.Reflection;
using HomeSeer.PluginSdk.Devices;

namespace HomeSeer.PluginSdk.Events {

    public class TriggerTypeCollection {

        public bool LogDebug { get; set; } = false;

        private ITriggerTypeListener _listener;

        private List<Type> _triggerTypes = new List<Type>();
        private HashSet<string> _triggerTypeNames = new HashSet<string>();

        public TriggerTypeCollection(ITriggerTypeListener listener) {
            _listener = listener;
        }

        public void AddTriggerType(Type triggerType) {
            if (triggerType?.FullName == null) {
                throw new ArgumentException(nameof(triggerType));
            }
            if (!triggerType.IsSubclassOf(typeof(AbstractTriggerType))) {
                throw new ArgumentException($"{triggerType} is not derived from AbstractTriggerType", nameof(triggerType));
            }
            AssertTypeHasConstructors(triggerType);
            if (_triggerTypeNames.Contains(triggerType.FullName)) {
                throw new ArgumentException($"{triggerType.FullName} has already been added to the collection.");
            }

            if (_triggerTypeNames.Add(triggerType.FullName)) {
                _triggerTypes.Add(triggerType);
            }
        }

        public virtual string GetName(int triggerIndex) {
            
            try {
                var targetTrig = GetObjectFromInfo(triggerIndex-1);
                return targetTrig.Name;
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return "Error retrieving trigger name";
            }
        }
        
        public virtual int GetSubTriggerCount(int triggerIndex) {
            try {
                var targetTrig = GetObjectFromInfo(triggerIndex-1);
                return targetTrig.SubTriggerCount;
            }
            catch (ArgumentOutOfRangeException) {
                return 0;
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return 0;
            }
        }

        public virtual string GetSubTriggerName(int triggerIndex, int subTriggerIndex) {
            try {
                var targetTrig = GetObjectFromInfo(triggerIndex-1);
                return targetTrig.GetSubTriggerName(subTriggerIndex-1);
            }
            catch (ArgumentOutOfRangeException) {
                return "No sub-trigger type for that index";
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return "Error retrieving trigger name";
            }
        }

        public virtual string OnGetTriggerUi(TrigActInfo trigInfo) {

            try {
                var curTrig = GetObjectFromTrigInfo(trigInfo);
                curTrig = _listener?.OnBuildTriggerUi(curTrig) ?? curTrig;
                return curTrig.ToHtml();
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return exception.Message;
            }
            
        }

        public virtual MultiReturn OnUpdateTriggerConfig(Dictionary<string, string> postData, TrigActInfo trigInfo) {
            
            var mr = new MultiReturn {
                         sResult = "",
                         TrigActInfo = trigInfo
                     };

            try {
                var curTrig = GetObjectFromTrigInfo(trigInfo);
                var result = curTrig.ProcessPostData(postData);
                curTrig = _listener?.OnTriggerConfigChange(curTrig) ?? curTrig;
                mr.sResult = result ? "" : "Unknown Plugin Error";
                mr.DataOut = curTrig.Data;
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

        public virtual bool IsTriggerConfigured(TrigActInfo trigInfo) {

            try {
                var curAct = GetObjectFromTrigInfo(trigInfo);
                return curAct.IsFullyConfigured();
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return true;
            }
        }

        public virtual string OnGetTriggerPrettyString(TrigActInfo trigInfo) {
            
            try {
                var curAct = GetObjectFromTrigInfo(trigInfo);
                return curAct.GetPrettyString();
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return exception.Message;
            }
        }

        public virtual bool IsTriggerTrue(TrigActInfo trigInfo, bool isCondition) {
            try {
                var curTrig = GetObjectFromTrigInfo(trigInfo);
                curTrig = _listener?.BeforeCheckTrigger(curTrig) ?? curTrig;
                return curTrig.IsTriggerTrue(isCondition);
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return false;
            }
        }

        public virtual bool TriggerReferencesDeviceOrFeature(int devOrFeatRef, TrigActInfo trigInfo) {
            try {
                var curTrig = GetObjectFromTrigInfo(trigInfo);
                return curTrig.ReferencesDeviceOrFeature(devOrFeatRef);
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return false;
            }
        }

        public virtual bool TriggerCanBeCondition(int triggerIndex) {
            try {
                var targetTrig = GetObjectFromInfo(triggerIndex);
                return targetTrig.CanBeCondition;
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return false;
            }
        }

        private AbstractTriggerType GetObjectFromTrigInfo(TrigActInfo trigInfo) {
            return GetObjectFromInfo(trigInfo.TANumber-1, trigInfo.UID, trigInfo.evRef, trigInfo.DataIn ?? new byte[0]);
        }

        private AbstractTriggerType GetObjectFromInfo(int trigNumber, params object[] trigInfoParams) {
            if (_triggerTypes.Count < trigNumber) {
                throw new KeyNotFoundException("No trigger type exists with that number");
            }

            var targetType = _triggerTypes[trigNumber];
            var paramTypes = new List<Type>();
            foreach (var infoParam in trigInfoParams) {
                paramTypes.Add(infoParam.GetType());
            }
            var typeConstructor = targetType.GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                                                            null,
                                                            CallingConventions.Standard,
                                                            paramTypes.ToArray(),
                                                            null);

            if (typeConstructor == null) {
                throw new TypeLoadException("Cannot find the correct constructor for this trigger type");
            }

            //new object[] { actInfo.UID, actInfo.evRef, actInfo.DataIn };
            if (!(typeConstructor.Invoke(trigInfoParams) is AbstractTriggerType curTrig)) {
                throw new TypeLoadException("This constructor did not produce a class derived from AbstractTriggerType");
            }

            curTrig.LogDebug = LogDebug;
            return curTrig;
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
        
        public interface ITriggerTypeListener {

            AbstractTriggerType OnBuildTriggerUi(AbstractTriggerType trigger);
            AbstractTriggerType OnTriggerConfigChange(AbstractTriggerType trigger);
            AbstractTriggerType BeforeCheckTrigger(AbstractTriggerType trigger);

        }

    }

}