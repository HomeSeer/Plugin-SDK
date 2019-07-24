namespace HomeSeer.PluginSdk.Events {

    public struct TrigActInfo {

    /// <summary>
    /// This is the unique event reference ID number for the event that this trigger is a part of.
    /// </summary>
    /// <remarks></remarks>
    public int evRef;

    /// <summary>
    /// This is the unique ID for this trigger or action.  When the trigger is true, the plug-in will pass this 
    /// to HomeSeer to trigger to cause HomeSeer to check the conditions and trigger the event if appropriate.
    /// When the action needs to be carried out, HomeSeer will invoke the Handle procedure in the action, and 
    /// if there is action information stored by the plug-in, this property can be used to retrieve it.
    /// </summary>
    /// <remarks></remarks>
    public int UID;

    /// <summary>
    /// This is for plug-ins only since plug-ins can support multiple types of different triggers or actions.
    /// This identifies which, out of the triggers or actions offered by the plug-in, that this trigger
    /// or action is.
    /// </summary>
    /// <remarks></remarks>
    public int TANumber;


    /// <summary>
    /// When a trigger or action has sub-types, this is used to identify which sub-trigger or trigger sub-type,
    /// sub-action or action sub-type this trigger or action is set to.  For example, in HomeSeer there is a 
    /// TIME trigger - the sub-ID might identify whether it is the type AT, BEFORE, or AFTER a time value.
    /// </summary>
    /// <remarks></remarks>
    public int SubTANumber;

    /// <summary>
    /// This is exclusively for plug-ins.  Using a serialization procedure, the data for this plug-in 
    /// can be stored and managed within the HomeSeer database by the plug-in storing the serialized 
    /// object here.  When HomeSeer is calling into the plug-in and wants to provide the trigger object
    /// for the plug-in to analyze, it provides the serialized object data here in this byte array which
    /// the plug-in can use to de-serialize back into an object that holds the trigger info.  Since 
    /// parameters cannot be passed through the interface ByRef, this is read-only, but the plug-in is
    /// allowed to RETURN a structure that includes Data or DataOut, which is a byte array, and contains
    /// the serialized object after the plug-in updated it or made changes to it.
    /// </summary>
    /// <remarks></remarks>
    public byte[] DataIn;

    /// <summary>
    /// If the plug-in supports multiple instances and this trigger or action is for one of the instances, 
    /// then this field will have the instance name in it.
    /// </summary>
    /// <remarks></remarks>
    public string Instance;

    }

}