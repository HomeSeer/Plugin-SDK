using System;
using System.Collections.Generic;
using System.Text;
using HomeSeer.Jui.Types;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk.Devices;
using Newtonsoft.Json;

namespace HomeSeer.PluginSdk.Events {

    /// <summary>
    /// The base implementation of a plugin action type available for users to select in HomeSeer
    /// <para>
    /// The difference between <see cref="AbstractActionType2"/> and <see cref="AbstractActionType"/>, is that with
    /// <see cref="AbstractActionType2"/> only a collection of view Id/Value pairs is stored in database whereas with 
    /// <see cref="AbstractActionType"/> the whole <see cref="AbstractActionType.ConfigPage"/> is stored. This allows 
    /// the plugin to build the views in <see cref="OnInstantiateAction"/> every time an action is instantiated.
    /// </para>
    /// <para>
    /// Inherit from this class to define your own action types and store them in your plugin's <see cref="ActionTypeCollection"/>
    /// </para>
    /// </summary>
    public abstract class AbstractActionType2: AbstractActionType {

        /// <summary>
        /// Initialize a new <see cref="AbstractActionType2"/> with the specified ID, SubType number, Event Ref, Data byte array and listener.
        ///  The byte array will be automatically parsed to a collection of view Id/Value pairs, and <see cref="OnInstantiateAction"/> will be called
        /// <para>
        /// This is called through reflection by the <see cref="ActionTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// <para>
        /// You MUST implement one of these constructors in any class that derives from <see cref="AbstractActionType2"/>
        /// </para>
        /// </summary>
        /// <param name="id">The unique ID of this action in HomeSeer</param>
        /// <param name="subTypeNumber">The action subtype number</param>
        /// <param name="eventRef">The event reference ID that this action is associated with in HomeSeer</param>
        /// <param name="dataIn">A byte array containing a collection of view Id/Value pairs</param>
        /// <param name="listener">The listener that facilitates the communication with <see cref="AbstractPlugin"/></param>
        protected AbstractActionType2(int id, int subTypeNumber, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener) 
            :base(id, subTypeNumber, eventRef, dataIn, listener) {
        }

        /// <summary>
        /// Initialize a new <see cref="AbstractActionType2"/> with the specified ID, Event Ref, Data byte array, listener, and logDebug flag.
        ///  The byte array will be automatically parsed to a collection of view Id/Value pairs, and <see cref="OnInstantiateAction"/> will be called
        /// <para>
        /// This is called through reflection by the <see cref="ActionTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// <para>
        /// You MUST implement one of these constructors in any class that derives from <see cref="AbstractActionType2"/>
        /// </para>
        /// </summary>
        /// <param name="id">The unique ID of this action in HomeSeer</param>
        /// <param name="eventRef">The event reference ID that this action is associated with in HomeSeer</param>
        /// <param name="dataIn">A byte array containing a collection of view Id/Value pairs</param>
        /// <param name="listener">The listener that facilitates the communication with <see cref="AbstractPlugin"/></param>
        /// <param name="logDebug">If true debug messages will be written to the console</param>
        protected AbstractActionType2(int id, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener, bool logDebug = false) :
            base(id, eventRef, dataIn, listener, logDebug) {
        }

        /// <summary>
        /// Initialize a new <see cref="AbstractActionType2"/> with the specified ID, Event Ref, and Data byte array.
        ///  The byte array will be automatically parsed to a collection of view Id/Value pairs, and <see cref="OnInstantiateAction"/> will be called.
        /// <para>
        /// This is called through reflection by the <see cref="ActionTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// <para>
        /// You MUST implement one of these constructors in any class that derives from <see cref="AbstractActionType2"/>
        /// </para>
        /// </summary>
        /// <param name="id">The unique ID of this action in HomeSeer</param>
        /// <param name="eventRef">The event reference ID that this action is associated with in HomeSeer</param>
        /// <param name="dataIn">A byte array containing a collection of view Id/Value pairs</param>
        /// <param name="listener">The listener that facilitates the communication with <see cref="AbstractPlugin"/></param>
        protected AbstractActionType2(int id, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener) :
            base(id, eventRef, dataIn, listener) {
        }

        /// <summary>
        /// Initialize a new, unconfigured <see cref="AbstractActionType2"/>
        /// <para>
        /// This is called through reflection by the <see cref="ActionTypeCollection"/> class if a class that
        ///  derives from this type is added to its list.
        /// </para>
        /// </summary>
        protected AbstractActionType2() : base() {}

        /// <inheritdoc cref="AbstractActionType.OnNewAction" />
        /// <remarks>
        /// With <see cref="AbstractActionType2"/> there is no need to override this method. <see cref="OnInstantiateAction"/>
        /// will be called instead, with an empty Dictionary as parameter.
        /// </remarks> 
        protected override void OnNewAction() {
            OnInstantiateAction(new Dictionary<string, string>());
        }

        /// <summary>
        /// Called when an action of this type is being instantiated. Create the <see cref="AbstractActionType.ConfigPage"/> according
        /// to the values passed as parameters. If no value is passed it means it's a new action, so initialize the
        /// <see cref="AbstractActionType.ConfigPage"/> to the action's starting state so users can begin configuring it.
        /// <para>
        ///  Any JUI view added to the <see cref="AbstractActionType.ConfigPage"/> must use a unique ID as it will
        ///  be displayed on an event page that could also be housing HTML from other plugins. It is recommended
        ///  to use the <see cref="AbstractActionType.PageId"/> as a prefix for all views added to ensure that their IDs are unique.
        /// </para>
        /// <param name="viewIdValuePairs">View Id/Value pairs containing the existing values for this action</param>
        /// </summary>
        protected abstract void OnInstantiateAction(Dictionary<string, string> viewIdValuePairs);

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
                OnInstantiateAction(valueMap);
                //Save the data
                return inData;
            }
            catch (Exception exception) {
                //Exception is expected if the data is version 1 type or legacy type
                if (LogDebug) {
                    Console.WriteLine($"Exception while trying to execute ProcessData on action data, possibly version 1 or legacy data - {exception.Message}");
                }
            }
            
            //If deserialization failed try to call the ProcessData method from AbstractActionType so that it is directly deserialized as the ConfigPage
            return base.ProcessData(inData);
        }

        internal override byte[] GetData() {
            var valueMap = ConfigPage?.ToValueMap() ?? new Dictionary<string, string>();
            var valueMapJson = JsonConvert.SerializeObject(valueMap, Formatting.None);
            return Encoding.UTF8.GetBytes(valueMapJson);
        }
    }

}