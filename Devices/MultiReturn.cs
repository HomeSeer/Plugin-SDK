namespace HomeSeer.PluginSdk {

    public struct MultiReturn {

        /// <summary>
        /// When plug-in calls such as ...BuildUI, ...ProcessPostUI, or ...FormatUI are called and there is
        /// feedback or an error condition that needs to be reported back to the user, this string field 
        /// can contain the message to be displayed to the user in HomeSeer UI.  This field is cleared by
        /// HomeSeer after it is displayed to the user.
        /// </summary>
        /// <remarks></remarks>
        public string sResult;


        /// <summary>
        /// This is the trigger or action info from HomeSeer - see the structure for more information.
        /// </summary>
        /// <remarks></remarks>
        public TrigActInfo TrigActInfo;


        /// <summary>
        /// (Also see DataIn of strTrigInfo) The serialization data for the plug-in object cannot be 
        /// passed ByRef which means it can be passed only one-way through the interface to HomeSeer.
        /// If the plug-in receives DataIn, de-serializes it into an object, and then makes a change 
        /// to the object, this is where the object can be serialized again and passed back to HomeSeer
        /// for storage in the HomeSeer database.
        /// </summary>
        /// <remarks></remarks>
        public byte[] DataOut;

    }

}