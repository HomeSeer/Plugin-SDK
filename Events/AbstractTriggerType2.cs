using System;
using System.Collections.Generic;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk.Devices;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// The base implementation of a plugin trigger type available for users to select in HomeSeer
    /// <para>
    /// The difference between <see cref="AbstractTriggerType2"/> and <see cref="AbstractTriggerType"/>, is that with
    /// <see cref="AbstractTriggerType2"/> only a collection of view Id/Value pairs is stored in database whereas with 
    /// <see cref="AbstractTriggerType"/> the whole <see cref="AbstractTriggerType.ConfigPage"/> is stored. This allows 
    /// the plugin to build the views in <see cref="OnInstantiateTrigger"/> every time a trigger is instantiated.
    /// </para>
    /// <para>
    /// Inherit from this class to define your own trigger types and store them in your plugin's <see cref="TriggerTypeCollection"/>
    /// </para>
    /// </summary>
    public abstract class AbstractTriggerType2 : AbstractTriggerType {

        /// <summary>
        /// Initialize a new <see cref="AbstractTriggerType2"/> with the specified ID, Event Ref, Data byte array, listener, and logDebug flag.
        ///  The byte array will be automatically parsed to a collection of view Id/Value pairs, and <see cref="OnInstantiateTrigger"/> will be called.
        /// <para>
        /// This is called through reflection by the <see cref="TriggerTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// <para>
        /// You MUST implement one of these constructor signatures in any class that derives from <see cref="AbstractTriggerType2"/>
        /// </para>
        /// </summary>
        /// <param name="id">The unique ID of this trigger in HomeSeer</param>
        /// <param name="eventRef">The event reference ID that this trigger is associated with in HomeSeer</param>
        /// <param name="selectedSubTriggerIndex">The 0 based index of the sub-trigger type selected for this trigger</param>
        /// <param name="dataIn">A byte array containing the definition for a <see cref="Page"/></param>
        /// <param name="listener">The listener that facilitates the communication with <see cref="AbstractPlugin"/></param>
        /// <param name="logDebug">If true debug messages will be written to the console</param>
        protected AbstractTriggerType2(int id, int eventRef, int selectedSubTriggerIndex, byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug = false)
            : base(id, eventRef, selectedSubTriggerIndex, dataIn, listener, logDebug) {
        }

        /// <summary>
        /// Initialize a new <see cref="AbstractTriggerType2"/> with the specified ID, Event Ref, Data byte array and listener.
        ///  The byte array will be automatically parsed to a collection of view Id/Value pairs, and <see cref="OnInstantiateTrigger"/> will be called.
        /// <para>
        /// This is called through reflection by the <see cref="TriggerTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// <para>
        /// You MUST implement one of these constructor signatures in any class that derives from <see cref="AbstractTriggerType2"/>
        /// </para>
        /// </summary>
        /// <param name="id">The unique ID of this trigger in HomeSeer</param>
        /// <param name="eventRef">The event reference ID that this trigger is associated with in HomeSeer</param>
        /// <param name="selectedSubTriggerIndex">The 0 based index of the sub-trigger type selected for this trigger</param>
        /// <param name="dataIn">A byte array containing the definition for a <see cref="Page"/></param>
        /// <param name="listener">The listener that facilitates the communication with <see cref="AbstractPlugin"/></param>
        protected AbstractTriggerType2(int id, int eventRef, int selectedSubTriggerIndex, byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener)
        : base(id, eventRef, selectedSubTriggerIndex, dataIn, listener) {
        }

        /// <summary>
        /// Initialize a new <see cref="AbstractTriggerType2"/> from a <see cref="TrigActInfo"/> and with the specified listener, and logDebug flag.
        ///  The byte array in <paramref name="trigInfo"/> will be automatically parsed to a collection of view Id/Value pairs, 
        ///  and <see cref="OnInstantiateTrigger"/> will be called.
        /// <para>
        /// This is called through reflection by the <see cref="TriggerTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// <para>
        /// You MUST implement one of these constructor signatures in any class that derives from <see cref="AbstractTriggerType2"/>
        /// </para>
        /// </summary>
        /// <param name="trigInfo">The <see cref="TrigActInfo"/> containing all the trigger information</param>
        /// <param name="listener">The listener that facilitates the communication with <see cref="AbstractPlugin"/></param>
        /// <param name="logDebug">If true debug messages will be written to the console</param>
        protected AbstractTriggerType2(TrigActInfo trigInfo, TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug = false) :
            base(trigInfo, listener, logDebug) {
        }

        /// <summary>
        /// Initialize a new, unconfigured <see cref="AbstractTriggerType2"/>
        /// <para>
        /// This is called through reflection by the <see cref="TriggerTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// </summary>
        protected AbstractTriggerType2() : base() { }


        /// <inheritdoc cref="AbstractTriggerType.OnNewTrigger" />
        /// <remarks>
        /// With <see cref="AbstractTriggerType2"/> there is no need to override this method. <see cref="OnInstantiateTrigger"/>
        /// will be called instead, with an empty Dictionary as parameter.
        /// </remarks> 
        protected override void OnNewTrigger() {
            OnInstantiateTrigger(new Dictionary<string, string>());
        }


        /// <summary>
        /// Called when a trigger of this type is being instantiated. Create the <see cref="AbstractTriggerType.ConfigPage"/> according
        /// to the values passed as parameters. If no value is passed it means it's a new trigger, so initialize the
        /// <see cref="AbstractTriggerType.ConfigPage"/> to the trigger's starting state so users can begin configuring it.
        /// <para>
        ///  Any JUI view added to the <see cref="AbstractTriggerType.ConfigPage"/> must use a unique ID as it will
        ///  be displayed on an event page that could also be housing HTML from other plugins. It is recommended
        ///  to use the <see cref="AbstractTriggerType.PageId"/> as a prefix for all views added to ensure that their IDs are unique.
        /// </para>
        /// <param name="viewIdValuePairs">View Id/Value pairs containing the existing values for this trigger</param>
        /// </summary>
        protected abstract void OnInstantiateTrigger(Dictionary<string, string> viewIdValuePairs);

        internal override byte[] ProcessData(byte[] inData) {
            //Is data null/empty?
            if (inData == null || inData.Length == 0) {
                return new byte[0];
            }

            try {
                //Get JSON string from byte[]
                var valueMapJson = Encoding.UTF8.GetString(inData);
                //Deserialize to values map
                var valueMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(valueMapJson);
                //Call the plugin to build the ConfigPage
                OnInstantiateTrigger(valueMap);
                //Save the data
                return inData;
            }
            catch (Exception exception) {
                //Exception is expected if the data is version 1 type or legacy type
                if (LogDebug) {
                    Console.WriteLine($"Exception while trying to execute ProcessData on trigger data, possibly version 1 or legacy data - {exception.Message}");
                }
            }

            //If deserialization failed, try to call the ProcessData method from AbstractTriggerType so that it is directly deserialized as the ConfigPage
            return base.ProcessData(inData);
        }

        internal override byte[] GetData() {
            var valueMap = ConfigPage?.ToValueMap() ?? new Dictionary<string, string>();
            var valueMapJson = JsonConvert.SerializeObject(valueMap, Formatting.None);
            return Encoding.UTF8.GetBytes(valueMapJson);
        }

    }

}