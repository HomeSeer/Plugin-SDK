using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// The internal data stored by HomeSeer describing a particular event action or trigger.
    ///  Instances of this class are created and managed by HomeSeer and are passed through the <see cref="AbstractPlugin"/>
    ///  to the <see cref="ActionTypeCollection"/> and <see cref="TriggerTypeCollection"/> respectively.
    ///  You shouldn't need to work with this class directly and can rely on the decoded pieces exposed through the
    ///  <see cref="AbstractActionType"/> and <see cref="AbstractTriggerType"/> classes in most situations.
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class TrigActInfo {

        /// <summary>
        /// This is the unique event reference ID number for the event that this trigger is a part of.
        /// </summary>
        /// <remarks></remarks>
        /// <seealso cref="EventData"/>
        public int evRef;

        /// <summary>
        /// This is the unique ID for the trigger or action within an event that this <see cref="TrigActInfo"/> is for.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When the trigger is true, the plugin will pass this to HomeSeer to trigger to cause HomeSeer to check the
        /// conditions and trigger the event if appropriate. When the action needs to be carried out,
        /// HomeSeer will invoke the Handle procedure in the action, and if there is action information stored
        /// by the plugin, this property can be used to retrieve it.
        /// </para>
        /// </remarks>
        public int UID;

        /// <summary>
        /// This is for plugin reference only. Plugins can support multiple types of different triggers or actions.
        /// This identifies which type of trigger or action, out of the triggers or actions offered by the plugin,
        /// that this <see cref="TrigActInfo"/> is for.
        /// </summary>
        /// <remarks></remarks>
        /// <seealso cref="AbstractPlugin.ActionTypes"/>
        /// <seealso cref="AbstractPlugin.TriggerTypes"/>
        /// <seealso cref="ActionTypeCollection"/>
        /// <seealso cref="TriggerTypeCollection"/>
        public int TANumber;
        
        /// <summary>
        /// When a trigger has subtypes, this is used to identify which sub-trigger or trigger subtype, this trigger is set to.
        /// <para>
        /// For example, in HomeSeer there is a TIME trigger - this might identify whether it is the type AT, BEFORE, or AFTER a time value.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is only used for <see cref="AbstractTriggerType">triggers</see> and can be ignored in favor of managing
        /// a subtype directly. HomeSeer will present a list of trigger subtypes to choose from if
        /// <see cref="AbstractTriggerType.SubTriggerCount"/> > 1. HomeSeer does not do the same for
        /// <see cref="AbstractActionType">actions</see>, and there is no corresponding <code>SubActionCount</code> property.
        /// </para>
        /// </remarks>
        public int SubTANumber;

        /// <summary>
        /// Serialized data for this <see cref="AbstractActionType">action</see> or <see cref="AbstractTriggerType">trigger</see>
        /// that the plugin needs. This is stored within the HomeSeer database and serves as the configuration for the
        /// <see cref="ActionTypeCollection"/> and <see cref="TriggerTypeCollection"/> this is for.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is automatically unpacked by <see cref="AbstractActionType"/> and <see cref="AbstractTriggerType"/> if it
        /// contains a JSON string describing a <see cref="Jui.Views.Page"/>. If it contains any other data format, like
        /// data from an HS3 plugin, <see cref="AbstractTriggerType.ConvertLegacyData">AbstractTriggerType.ConvertLegacyData</see>
        /// or <see cref="AbstractActionType.ConvertLegacyData">AbstractActionType.ConvertLegacyData</see>
        /// will be called. You can override these methods to define your own method for unpacking the data.
        /// </para>
        /// </remarks>
        /// <seealso cref="AbstractTriggerType"/>
        /// <seealso cref="AbstractActionType"/>
        public byte[] DataIn;

        /// <summary>
        /// If the plug-in supports multiple instances and this trigger or action is for one of the instances, 
        /// then this field will have the instance name in it.
        /// </summary>
        /// <remarks>
        /// Multi-instance plugins were supported in HS3, but support for them was not carried over to HS4. This may change
        /// in the future, but, for now, this is an unused field.
        /// </remarks>
        public string Instance;
        
        //TODO : Add a new field that encapsulates all additional configuration properties for an action/trigger and any helpful references like device/feature refs

        /// <summary>
        /// Deserialize the specified byte array to an object of type <typeparamref name="TOutObject"/> using the legacy
        ///  HomeSeer method for deserializing trigger/action data.
        /// </summary>
        /// <param name="inData">The byte array to deserialize.</param>
        /// <param name="willLog">Whether the method should write log messages to the console.</param>
        /// <typeparam name="TOutObject">The type of object to deserialize the data to. Must be a class.</typeparam>
        /// <returns>An object of type <typeparamref name="TOutObject"/> or null if it was unsuccessful.</returns>
        public static TOutObject DeserializeLegacyData<TOutObject>(byte[] inData, bool willLog = false) where TOutObject : class {
            if (inData == null || inData.Length == 0) {
                return null;
                //throw new ArgumentNullException(nameof(inData));
            }
            var sf = new BinaryFormatter();
            try {
                using (var ms = new MemoryStream(inData)) {
                    return sf.Deserialize(ms) as TOutObject;
                }
            }
            catch (InvalidCastException exIC) {
                if (willLog) {
                    Console.WriteLine(exIC.StackTrace);
                }
            }
            catch (Exception ex) {
                if (willLog) {
                    Console.WriteLine($"Error Deserializing object: {ex.Message}");
                }
            }

            return null;
        }

    }

}