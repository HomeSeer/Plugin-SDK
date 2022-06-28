using System;
using System.Collections.Generic;
using System.Reflection;
using HomeSeer.PluginSdk.Devices;

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// A collection of <see cref="AbstractTriggerType"/>s that can be used by users to create HomeSeer Event Triggers.
    /// <para>
    /// An instance of this class is a field on <see cref="AbstractPlugin"/> initialized in the constructor.
    ///  In addition, all calls from HomeSeer related to triggers are automatically routed through that instance.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Add trigger types supported by your plugin using <see cref="AddTriggerType"/>
    /// <para>
    /// Due to the fact that HomeSeer saves the index of the trigger type in its internal database, avoid
    ///  changing the index of any of the types available to the user. Doing so may result in an incorrect
    ///  <see cref="AbstractTriggerType"/> being instantiated and producing errors because the configuration parameters
    ///  and signature of the trigger type do not match.
    /// </para>
    /// </remarks>
    public class TriggerTypeCollection : BaseTypeCollection<AbstractTriggerType> {

        /// <summary>
        /// Used to enable/disable internal logging to the console
        /// <para>
        /// When it is TRUE, log messages from the PluginSdk code will be written to the Console
        /// </para>
        /// </summary>
        public bool LogDebug { get; set; }
        
        /// <summary>
        /// <see cref="List{T}"/> of <see cref="Type">Types</see> that are a subclass of <see cref="AbstractTriggerType"/>
        /// </summary>
        /// <remarks>
        /// These represent the types of <see cref="AbstractTriggerType"/> that are available for users
        /// </remarks>
        public List<Type> TriggerTypes => _itemTypes;

        private ITriggerTypeListener _listener;

        /// <summary>
        /// Initialize a new instance of an <see cref="TriggerTypeCollection"/> with the specified listener
        /// </summary>
        /// <param name="listener">An <see cref="ITriggerTypeListener"/> that will be passed to trigger types</param>
        public TriggerTypeCollection(ITriggerTypeListener listener) {
            _listener = listener;
            MatchAllSignatures = false;
            ConstructorSignatures = new List<Type[]>();
            //trigInfo.UID, trigInfo.evRef, trigInfo.SubTANumber-1, trigInfo.DataIn ?? new byte[0], _listener, LogDebug
            var paramTypes = new[] {
                                       typeof(int),
                                       typeof(int),
                                       typeof(int),
                                       typeof(byte[]),
                                       typeof(ITriggerTypeListener),
                                       typeof(bool)
                                   };
            ConstructorSignatures.Add(paramTypes);
            //trigInfo.UID, trigInfo.evRef, trigInfo.SubTANumber-1, trigInfo.DataIn ?? new byte[0], _listener
            paramTypes = new[] {
                                       typeof(int),
                                       typeof(int),
                                       typeof(int),
                                       typeof(byte[]),
                                       typeof(ITriggerTypeListener)
                                   };
            ConstructorSignatures.Add(paramTypes);
            
        }

        /// <summary>
        /// Add the specified class type that derives from <see cref="AbstractTriggerType"/> to the list of triggers
        /// </summary>
        /// <param name="triggerType">
        /// The <see cref="Type"/> of the class that derives from <see cref="AbstractTriggerType"/>
        /// </param>
        /// <exception cref="ArgumentException">Thrown when the specified class type will not work as a trigger</exception>
        public void AddTriggerType(Type triggerType) {
            AddItemType(triggerType);
        }

        /// <summary>
        /// Get the name of the trigger type at the specified index
        /// </summary>
        /// <param name="triggerIndex">The 1 based index of the trigger type to get the name for</param>
        /// <returns>The name of the trigger type</returns>
        public string GetName(int triggerIndex) {
            
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
        
        /// <summary>
        /// Called by HomeSeer to determine the number of available sub-trigger types available under the
        ///  trigger type at the specified index.
        /// </summary>
        /// <param name="triggerIndex">The 1 based index of the trigger type to get the sub-trigger count for</param>
        /// <returns>The number of sub-trigger types available under the trigger type specified</returns>
        public int GetSubTriggerCount(int triggerIndex) {
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

        /// <summary>
        /// Get the name of the sub-trigger type at the specified index
        /// </summary>
        /// <param name="triggerIndex">The 1 based index of the trigger type the sub-trigger is a member of</param>
        /// <param name="subTriggerIndex">The 1 based index of the sub-trigger type to get the name for</param>
        /// <returns>The name of the sub-trigger type</returns>
        public string GetSubTriggerName(int triggerIndex, int subTriggerIndex) {
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

        /// <summary>
        /// Called by HomeSeer when it needs to display the configuration UI for a trigger type
        /// </summary>
        /// <param name="trigInfo">The trigger to display as defined by <see cref="TrigActInfo"/></param>
        /// <returns>HTML to display on the event page for the specified trigger</returns>
        public string OnGetTriggerUi(TrigActInfo trigInfo) {

            try {
                var curTrig = GetObjectFromTrigInfo(trigInfo);
                //curTrig = _listener?.OnBuildTriggerUi(curTrig) ?? curTrig;
                return curTrig.ToHtml();
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return exception.Message;
            }
            
        }

        /// <summary>
        /// Called by HomeSeer when a user updates the configuration of a trigger and those changes
        ///  are in need of processing.
        /// </summary>
        /// <param name="postData">A <see cref="Dictionary{TKey,TValue}"/> of changes to the trigger configuration</param>
        /// <param name="trigInfo">The trigger being configured</param>
        /// <returns>
        /// An <see cref="EventUpdateReturnData"/> describing the new state of the trigger that will be saved by HomeSeer.
        ///  The trigger configuration will be saved if the result returned is an empty string.
        /// </returns>
        public EventUpdateReturnData OnUpdateTriggerConfig(Dictionary<string, string> postData, TrigActInfo trigInfo) {
            
            var mr = new EventUpdateReturnData {
                         Result = "",
                         TrigActInfo = trigInfo
                     };

            try {
                var curTrig = GetObjectFromTrigInfo(trigInfo);
                var result = curTrig.ProcessPostData(postData);
                //curTrig = _listener?.OnTriggerConfigChange(curTrig) ?? curTrig;
                mr.Result = result ? "" : "Unknown Plugin Error";
                mr.DataOut = curTrig.Data;
                return mr;
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                mr.Result = exception.Message;
                return mr;
            }
        }

        /// <summary>
        /// Called by HomeSeer when it needs to determine if a specific trigger is completely configured
        ///  or requires additional configuration before it can be used.
        /// </summary>
        /// <param name="trigInfo">The trigger to check</param>
        /// <returns>
        /// TRUE if the trigger is completely configured,
        ///  FALSE if it is not. A call to <see cref="OnGetTriggerUi"/> will be called following this if FALSE is returned.
        /// </returns>
        public bool IsTriggerConfigured(TrigActInfo trigInfo) {

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

        /// <summary>
        /// Called by HomeSeer when it needs to get an easy to read, formatted string that communicates what
        ///  the trigger does to the user.
        /// </summary>
        /// <param name="trigInfo">The trigger that a pretty string is needed for</param>
        /// <returns>
        /// HTML formatted text communicating what the trigger does
        /// </returns>
        public string OnGetTriggerPrettyString(TrigActInfo trigInfo) {
            
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

        /// <summary>
        /// Called by HomeSeer to determine if a trigger's conditions have been met.
        /// </summary>
        /// <param name="trigInfo">The trigger to check</param>
        /// <param name="isCondition">TRUE if the trigger is paired with other triggers, FALSE if it is alone.</param>
        /// <returns>
        /// TRUE if the trigger's conditions have been met,
        ///  FALSE if they haven't
        /// </returns>
        public bool IsTriggerTrue(TrigActInfo trigInfo, bool isCondition) {
            try {
                var curTrig = GetObjectFromTrigInfo(trigInfo);
                //curTrig = _listener?.BeforeCheckTrigger(curTrig) ?? curTrig;
                return curTrig.IsTriggerTrue(isCondition);
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
        ///  particular trigger.
        /// </summary>
        /// <param name="devOrFeatRef">The unique <see cref="AbstractHsDevice.Ref"/> of the device/feature</param>
        /// <param name="trigInfo">The trigger to check</param>
        /// <returns>
        /// TRUE if the trigger references the specified device/feature,
        ///  FALSE if it does not.
        /// </returns>
        public bool TriggerReferencesDeviceOrFeature(int devOrFeatRef, TrigActInfo trigInfo) {
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

        /// <summary>
        /// Called by HomeSeer to determine if a particular trigger can be used as a condition or not.
        ///  A condition is a trigger that operates in conjunction with another trigger in an AND/OR pattern.
        /// </summary>
        /// <param name="triggerIndex">The 1 based index of the trigger type to check</param>
        /// <returns>
        /// TRUE if the trigger can be used as a condition,
        ///  FALSE if it can not.
        /// </returns>
        public bool TriggerCanBeCondition(int triggerIndex) {
            try {
                var targetTrig = GetObjectFromInfo(triggerIndex - 1);
                return targetTrig.CanBeCondition;
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return false;
            }
        }

        /// <summary>
        /// Get an instance of an <see cref="AbstractTriggerType"/> from a <see cref="TrigActInfo"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="TrigActInfo.TANumber"/> is used to identify which of the
        ///  <see cref="AbstractTriggerType">AbstractTriggerTypes</see> is used.
        /// </para>
        /// </remarks>
        /// <param name="trigInfo">An instance of <see cref="TrigActInfo"/> that represents a trigger.</param>
        /// <returns>
        /// An instance of an <see cref="AbstractTriggerType"/>. This will be one of the types in the <see cref="TriggerTypes"/> list.
        /// </returns>
        /// <exception cref="TypeLoadException">
        /// Thrown if the <see cref="AbstractTriggerType"/> that is referenced by <see cref="TrigActInfo.TANumber"/>
        ///  does not have the proper constructor.
        /// <list type="bullet">
        /// <item><see cref="AbstractTriggerType(int,int,int,System.Byte[],ITriggerTypeListener)"/></item>
        /// <item><see cref="AbstractTriggerType(int,int,int,System.Byte[],ITriggerTypeListener,bool)"/></item>
        /// </list>
        /// </exception>
        public AbstractTriggerType GetObjectFromTrigInfo(TrigActInfo trigInfo) {
            //trigInfo.UID, trigInfo.evRef, trigInfo.SubTANumber-1, trigInfo.DataIn ?? new byte[0], _listener, LogDebug
            if (TypeHasConstructor(trigInfo.TANumber - 1, 0)) {
                return GetObjectFromInfo(trigInfo.TANumber-1, 0, trigInfo.UID, trigInfo.evRef, trigInfo.SubTANumber-1, trigInfo.DataIn ?? Array.Empty<byte>(), _listener, LogDebug);
            }
            //trigInfo.UID, trigInfo.evRef, trigInfo.SubTANumber-1, trigInfo.DataIn ?? new byte[0], _listener
            if (TypeHasConstructor(trigInfo.TANumber - 1, 1)) {
                return GetObjectFromInfo(trigInfo.TANumber-1, 1, trigInfo.UID, trigInfo.evRef, trigInfo.SubTANumber-1, trigInfo.DataIn ?? Array.Empty<byte>(), _listener);
            }
            throw new TypeLoadException("Trigger instance cannot be created because no constructor was found that takes TrigActInfo data");
        }

        /// <summary>
        /// The base implementation of a trigger type interface that facilitates communication between
        ///  <see cref="AbstractTriggerType"/>s and the <see cref="AbstractPlugin"/> that owns them.
        /// <para>
        /// Extend this interface and have your <see cref="AbstractPlugin"/> implementation inherit it to make it
        ///  accessible through the <see cref="AbstractTriggerType.TriggerListener"/> field.
        /// </para>
        /// </summary>
        public interface ITriggerTypeListener {}

    }

}