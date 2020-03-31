using System;
using System.Collections.Generic;
using HomeSeer.PluginSdk.Devices;
using HomeSeer.PluginSdk.Devices.Controls;
using HomeSeer.PluginSdk.Energy;
using HomeSeer.PluginSdk.Events;
using HomeSeer.PluginSdk.Logging;
using HSCF.Communication.ScsServices.Service;
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace HomeSeer.PluginSdk {

    /// <summary>
    /// The interface used by plugins to communicate with the HomeSeer software
    /// <para>
    /// An instance of this interface is automatically provided to an AbstractPlugin when AbstractPlugin.Connect(string[]) is called.
    /// </para>
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [ScsService]
    public interface IHsController {

        /// <summary>
        /// The current version of the HomeSeer Plugin API
        /// </summary>
        double APIVersion { get; }
        
        /// <summary>
        /// The number of devices connected to the HomeSeer system
        /// </summary>
        int DeviceCount { get; }
        
        /// <summary>
        /// Register a new plugin with HomeSeer
        /// <para>
        /// This will add the specified ID/filename pair to HomeSeer's list of plugins to check when it runs through
        ///  the plugin initialization process.
        /// </para>
        /// </summary>
        /// <param name="pluginId">The ID of the plugin to register</param>
        /// <param name="pluginName">The name of the plugin to register</param>
        /// <returns>
        /// TRUE if the plugin was registered successfully;
        ///  FALSE if there was a problem with registration
        /// </returns>
        bool RegisterPlugin(string pluginId, string pluginName);
        
        #region Settings and Config data
        
        /// <summary>
        /// Clear all of the settings saved in a section in a specific file
        /// </summary>
        /// <param name="sectionName">The section to clear</param>
        /// <param name="fileName">The name of the INI file to edit</param>
        void ClearIniSection(string sectionName, string fileName);

        /// <summary>
        /// Get the value of the setting saved to INI file
        /// </summary>
        /// <param name="sectionName">The name of the section the setting is saved to</param>
        /// <param name="key">The key of the setting</param>
        /// <param name="defaultVal">A default value to use if the setting was not previously saved</param>
        /// <param name="fileName">The name of the INI file to search</param>
        /// <returns></returns>
        string GetINISetting(string sectionName, string key, string defaultVal, string fileName = "");

        /// <summary>
        /// Save the new value of a setting
        /// </summary>
        /// <param name="sectionName">The name of the section the setting is saved to</param>
        /// <param name="key">The key of the setting</param>
        /// <param name="value">The value to save</param>
        /// <param name="fileName">The name of the INI file to save the setting to</param>
        void SaveINISetting(string sectionName, string key, string value, string fileName);

        /// <summary>
        /// Get a key-value map of settings saved in the specified section of the INI file
        /// </summary>
        /// <param name="section">The section to get</param>
        /// <param name="fileName">The name of the INI file</param>
        /// <returns>A Dictionary of setting keys and values</returns>
        Dictionary<string, string> GetIniSection(string section, string fileName);

        #endregion

        #region Features/Pages

        /// <summary>
        /// Register a feature page to create a link to it in the navigation menu in HomeSeer.
        /// <para>
        /// The PluginFilename must end with .html and not include the enclosing folder name.
        ///   The page must exist in the HomeSeer html folder as: PluginID/PluginFilename
        /// </para>
        /// </summary>
        /// <param name="pluginId">The ID of the plugin</param>
        /// <param name="pageFilename">The filename of the page, ending with .html</param>
        /// <param name="linkText">The text that appears in the navigation menu</param>
        void RegisterFeaturePage(string pluginId, string pageFilename, string linkText);

        /// <summary>
        /// Unregister a feature page to remove any navigation links to the page.
        /// </summary>
        /// <param name="pluginId">The ID of the plugin</param>
        /// <param name="pageFilename">
        /// The filename of the page, ending with .html.
        ///   This must be exactly the same as the filename used to register the page</param>
        void UnregisterFeaturePage(string pluginId, string pageFilename);

        /// <summary>
        /// Register a page as the device inclusion process guide for this plugin.
        /// <para>
        /// There can only be one device inclusion process for each plugin.
        ///   The page that is tagged as the device inclusion process will be displayed first in
        ///  the list of features for the plugin and be shown in the list of devices users can add.
        /// </para>
        /// </summary>
        /// <param name="pluginId">The ID of the plugin</param>
        /// <param name="pageFilename">The filename of the page, ending with .html</param>
        /// <param name="linkText">The text that appears in the navigation menu</param>
        void RegisterDeviceIncPage(string pluginId, string pageFilename, string linkText);

        /// <summary>
        /// Unregister the device inclusion page for this plugin.
        /// </summary>
        /// <param name="pluginId">The ID of the plugin</param>
        void UnregisterDeviceIncPage(string pluginId);
        
        #endregion
        
        #region Devices
        
        #region Create
        
        /// <summary>
        /// Create a new device in HomeSeer
        /// </summary>
        /// <param name="deviceData">
        /// <see cref="NewDeviceData"/> describing the device produced by <see cref="DeviceFactory"/>
        /// </param>
        /// <returns>The unique reference ID assigned to the device</returns>
        int CreateDevice(NewDeviceData deviceData);

        /// <summary>
        /// Create a new feature on a device in HomeSeer
        /// </summary>
        /// <param name="featureData">
        /// <see cref="NewFeatureData"/> describing the feature produced by <see cref="FeatureFactory"/>
        /// </param>
        /// <returns>The unique reference ID assigned to the feature</returns>
        int CreateFeatureForDevice(NewFeatureData featureData);
        
        #endregion
        
        #region Read
        
        //Both
        
        /// <summary>
        /// Get a list of device/feature references that are associated with the specified plugin interface
        /// </summary>
        /// <param name="interfaceName">The ID of the plugin interface to get devices and features for</param>
        /// <param name="deviceOnly"> Whether to get refs for devices or both devices and features</param>
        /// <returns>A list of device/feature reference IDs</returns>
        List<int> GetRefsByInterface(string interfaceName, bool deviceOnly = false);
        
        /// <summary>
        /// Get a map containing the value of a specific property for every device owned by a particular plugin
        /// </summary>
        /// <param name="interfaceName">The ID of the plugin that owns the devices</param>
        /// <param name="property">The EProperty type to read</param>
        /// <param name="deviceOnly">Whether the result should only contain devices or both devices and features</param>
        /// <returns>A Dictionary of device/feature refs and the value of the EProperty requested</returns>
        Dictionary<int, object> GetPropertyByInterface(string interfaceName, EProperty property, bool deviceOnly = false);
        
        /// <summary>
        /// Get the name of a specific device/feature by its <see cref="AbstractHsDevice.Ref"/>
        /// </summary>
        /// <param name="devOrFeatRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <returns>The name of the device/feature with the matching <see cref="AbstractHsDevice.Ref"/></returns>
        string GetNameByRef(int devOrFeatRef);
        
        /// <summary>
        /// Determine if a specific device/feature <see cref="AbstractHsDevice.Ref"/> exists in the HomeSeer system
        /// </summary>
        /// <param name="devOrFeatRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <returns>TRUE if the <see cref="AbstractHsDevice.Ref"/> exists, FALSE if it does not</returns>
        bool DoesRefExist(int devOrFeatRef);
        
        /// <summary>
        /// Get the value of the <see cref="EProperty"/> for the <see cref="AbstractHsDevice"/> with the specified <see cref="AbstractHsDevice.Ref"/> 
        /// </summary>
        /// <param name="devOrFeatRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <param name="property">The <see cref="EProperty"/> to read</param>
        /// <returns>The value of the requested <see cref="EProperty"/> of the <see cref="AbstractHsDevice"/></returns>
        object GetPropertyByRef(int devOrFeatRef, EProperty property);
        
        /// <summary>
        /// Determine if a <see cref="EMiscFlag"/> is turned on for a particular <see cref="AbstractHsDevice"/>
        /// </summary>
        /// <param name="devOrFeatRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <param name="miscFlag">The <see cref="EMiscFlag"/> to read</param>
        /// <returns>TRUE if the <see cref="AbstractHsDevice"/> found contains the specified <see cref="EMiscFlag"/>, FALSE if it doesn't</returns>
        bool IsFlagOnRef(int devOrFeatRef, EMiscFlag miscFlag);
        
        /// <summary>
        /// Determine if the <see cref="AbstractHsDevice"/> with the specified <see cref="AbstractHsDevice.Ref"/> is a <see cref="HsDevice"/> or a <see cref="HsFeature"/> of a device
        /// </summary>
        /// <param name="devOrFeatRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <returns>TRUE if the <see cref="AbstractHsDevice"/> found is a <see cref="HsDevice"/>, FALSE if it is a <see cref="HsFeature"/></returns>
        bool IsRefDevice(int devOrFeatRef);
        
        /// <summary>
        /// Get a list of all of the device and feature refs present in the HomeSeer system
        /// <para>
        /// To get just a list of devices, call <see cref="GetAllDeviceRefs"/>
        ///  or to get a list of features, call <see cref="GetAllFeatureRefs"/>
        /// </para>
        /// </summary>
        /// <returns>A list of integers corresponding to the device and feature refs managed by the HomeSeer system</returns>
        List<int> GetAllRefs();
        
        //Devices
        /// <summary>
        /// Get the <see cref="AbstractHsDevice"/> as a <see cref="HsDevice"/> with the specified <see cref="AbstractHsDevice.Ref"/>.
        ///  The <see cref="HsDevice.Features"/> property will be empty. To include <see cref="HsFeature"/>s use <see cref="GetDeviceWithFeaturesByRef"/>
        /// </summary>
        /// <remarks>
        /// Calling this using the <see cref="AbstractHsDevice.Ref"/> of a <see cref="HsFeature"/> may have adverse effects.
        /// </remarks>
        /// <param name="devRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <returns>A <see cref="HsDevice"/> whether it is a <see cref="Devices.Identification.ERelationship.Device"/> or <see cref="Devices.Identification.ERelationship.Feature"/></returns>
        HsDevice GetDeviceByRef(int devRef);
        
        /// <summary>
        /// Get the <see cref="AbstractHsDevice"/> as a <see cref="HsDevice"/> with the specified <see cref="AbstractHsDevice.Ref"/>.
        ///  The <see cref="HsDevice.Features"/> property will be populated with associated features.
        /// </summary>
        /// <remarks>
        /// Calling this using the <see cref="AbstractHsDevice.Ref"/> of a <see cref="HsFeature"/> may have adverse effects.
        /// </remarks>
        /// <param name="devRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <returns>A <see cref="HsDevice"/> whether it is a <see cref="Devices.Identification.ERelationship.Device"/> or <see cref="Devices.Identification.ERelationship.Feature"/></returns>
        HsDevice GetDeviceWithFeaturesByRef(int devRef);
        
        /// <summary>
        /// Get the <see cref="AbstractHsDevice"/> as a <see cref="HsDevice"/> with the specified <see cref="AbstractHsDevice.Address"/>.
        ///  The <see cref="HsDevice.Features"/> property will be empty. To include <see cref="HsFeature"/>s use <see cref="GetDeviceWithFeaturesByRef"/>
        /// </summary>
        /// <remarks>
        /// Calling this using the <see cref="AbstractHsDevice.Address"/> of a <see cref="HsFeature"/> may have adverse effects.
        /// </remarks>
        /// <param name="devAddress">The <see cref="AbstractHsDevice.Address"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <returns>A <see cref="HsDevice"/> whether it is a <see cref="Devices.Identification.ERelationship.Device"/> or <see cref="Devices.Identification.ERelationship.Feature"/></returns>
        HsDevice GetDeviceByAddress(string devAddress);

        /// <summary>
        /// Get a list of all of the device refs present in the HomeSeer system
        /// <para>
        /// This does not include refs for features. To get those in addition to these, use <see cref="GetAllRefs"/>
        ///  or call <see cref="GetAllFeatureRefs"/> to get a list of just features
        /// </para>
        /// </summary>
        /// <returns>A list of integers corresponding to the device refs managed by the HomeSeer system</returns>
        List<int> GetAllDeviceRefs();
        
        /// <summary>
        /// Get a list of all of the devices managed by the HomeSeer system without associated features.
        /// <para>
        /// WARNING - this is an expensive method to execute and it should be used with the utmost discretion
        /// </para>
        /// </summary>
        /// <param name="withFeatures">
        /// TRUE if associated features should be attached to their devices,
        ///  or FALSE if features should be left out.
        /// </param>
        /// <returns>
        /// A list of <see cref="HsDevice"/>s managed by the HomeSeer system with or without associated features linked.
        /// </returns>
        List<HsDevice> GetAllDevices(bool withFeatures);
        
#if BETA
        
        /// <summary>
        /// Get a list of all of the device refs present in the HomeSeer system that match the specified pattern
        /// </summary>
        /// <param name="matchPattern">
        /// The property state to match devices to. Create an instance of <see cref="HsDevice"/> and use
        ///  <see cref="AbstractHsDevice.Changes"/> for this parameter.
        /// </param>
        /// <returns>
        /// A list of integers corresponding to the device refs managed by the HomeSeer system that match
        ///  the specified pattern.
        /// </returns>
        List<int> GetAllMatchingDeviceRefs(Dictionary<EProperty, object> matchPattern);
        
        /// <summary>
        /// Get a list of all of the devices present in the HomeSeer system that match the specified pattern
        /// <para>
        /// WARNING - this is an expensive method to execute and it should be used with the utmost discretion
        /// </para>
        /// </summary>
        /// <param name="matchPattern">
        /// The property state to match devices to. Create an instance of <see cref="HsDevice"/> and use
        ///  <see cref="AbstractHsDevice.Changes"/> for this parameter.
        /// </param>
        /// <param name="withFeatures">
        /// TRUE if associated features should be attached to their devices,
        ///  or FALSE if features should be left out.
        /// </param>
        /// <returns>
        /// A list of <see cref="HsDevice"/>s managed by the HomeSeer system that match the specified pattern.
        /// </returns>
        List<HsDevice> GetAllMatchingDevices(Dictionary<EProperty, object> matchPattern, bool withFeatures);

#endif
        
        //Features
        /// <summary>
        /// Get the <see cref="AbstractHsDevice"/> as a <see cref="HsFeature"/> with the specified <see cref="AbstractHsDevice.Ref"/>.
        /// </summary>
        /// <remarks>
        /// Calling this using the <see cref="AbstractHsDevice.Ref"/> of a <see cref="HsDevice"/> may have adverse effects.
        /// </remarks>
        /// <param name="featRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <returns>A <see cref="HsFeature"/> whether it is a <see cref="Devices.Identification.ERelationship.Device"/> or <see cref="Devices.Identification.ERelationship.Feature"/></returns>
        HsFeature GetFeatureByRef(int featRef);
        
        /// <summary>
        /// Get the <see cref="AbstractHsDevice"/> as a <see cref="HsFeature"/> with the specified <see cref="AbstractHsDevice.Address"/>.
        /// </summary>
        /// <remarks>
        /// Calling this using the <see cref="AbstractHsDevice.Address"/> of a <see cref="HsDevice"/> may have adverse effects.
        /// </remarks>
        /// <param name="featAddress">The <see cref="AbstractHsDevice.Address"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <returns>A <see cref="HsFeature"/> whether it is a <see cref="Devices.Identification.ERelationship.Device"/> or <see cref="Devices.Identification.ERelationship.Feature"/></returns>
        HsFeature GetFeatureByAddress(string featAddress);
        
        /// <summary>
        /// Determine if the current status value of a <see cref="HsFeature"/> is considered valid.
        ///  This calls <see cref="HsFeature.IsValueValid"/> on the <see cref="HsFeature"/> to determine validity.
        /// </summary>
        /// <param name="featRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="HsFeature"/> to read</param>
        /// <returns>The result of <see cref="HsFeature.IsValueValid"/></returns>
        bool IsFeatureValueValid(int featRef);

        /// <summary>
        /// Get a list of all of the feature refs present in the HomeSeer system
        /// <para>
        /// This does not include refs for devices. To get those in addition to these, use <see cref="GetAllRefs"/>
        ///  or call <see cref="GetAllDeviceRefs"/> to get a list of just devices
        /// </para>
        /// </summary>
        /// <returns>A list of integers corresponding to the feature refs managed by the HomeSeer system</returns>
        List<int> GetAllFeatureRefs();

#if BETA
        
        /// <summary>
        /// Get a list of all of the feature refs present in the HomeSeer system that match the specified pattern
        /// </summary>
        /// <param name="matchPattern">
        /// The property state to match features to. Create an instance of <see cref="HsFeature"/> and use
        ///  <see cref="AbstractHsDevice.Changes"/> for this parameter.
        /// </param>
        /// <returns>
        /// A list of integers corresponding to the feature refs managed by the HomeSeer system that match
        ///  the specified pattern.
        /// </returns>
        List<int> GetAllMatchingFeatureRefs(Dictionary<EProperty, object> matchPattern);
        
        /// <summary>
        /// Get a list of all of the features present in the HomeSeer system that match the specified pattern
        /// <para>
        /// WARNING - this is an expensive method to execute and it should be used with the utmost discretion
        /// </para>
        /// </summary>
        /// <param name="matchPattern">
        /// The property state to match features to. Create an instance of <see cref="HsFeature"/> and use
        ///  <see cref="AbstractHsDevice.Changes"/> for this parameter.
        /// </param>
        /// <returns>A list of features managed by the HomeSeer system that match the specified pattern.</returns>
        List<HsFeature> GetAllMatchingFeatures(Dictionary<EProperty, object> matchPattern);

#endif

        /// <summary>
        /// Get the <see cref="StatusControl"/> for a value on an <see cref="HsFeature"/>
        /// </summary>
        /// <param name="featRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <param name="value">The <see cref="AbstractHsDevice.Value"/> managed by the <see cref="StatusControl"/></param>
        /// <returns>A <see cref="StatusControl"/> that manages the value specified for the <see cref="HsFeature"/></returns>
        StatusControl GetStatusControlForValue(int featRef, double value);
        
        /// <summary>
        /// Get the <see cref="StatusControl"/> for a <see cref="StatusControl.Label"/> on an <see cref="HsFeature"/>
        /// </summary>
        /// <param name="featRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <param name="label">The <see cref="StatusControl.Label"/> used by the <see cref="StatusControl"/></param>
        /// <returns>A <see cref="StatusControl"/> with the specified <see cref="StatusControl.Label"/> for the <see cref="HsFeature"/></returns>
        StatusControl GetStatusControlForLabel(int featRef, string label);
        
        /// <summary>
        /// Get a list of <see cref="StatusControl"/>s for a range of values on an <see cref="HsFeature"/>
        /// </summary>
        /// <param name="featRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <param name="min">The minimum </param>
        /// <param name="max"></param>
        /// <returns></returns>
        List<StatusControl> GetStatusControlsForRange(int featRef, double min, double max);
        int GetStatusControlCountByRef(int featRef);
        List<StatusControl> GetStatusControlsByRef(int featRef);

        StatusControlCollection GetStatusControlCollectionByRef(int featRef);
        
        /// <summary>
        /// Get the <see cref="StatusGraphic"/> for a value on an <see cref="HsFeature"/>
        /// </summary>
        /// <param name="featRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="AbstractHsDevice"/> to read</param>
        /// <param name="value">The <see cref="AbstractHsDevice.Value"/> managed by the <see cref="StatusGraphic"/></param>
        /// <returns>A <see cref="StatusGraphic"/> that manages the value specified for the <see cref="HsFeature"/></returns>
        StatusGraphic GetStatusGraphicForValue(int featRef, double value);
        List<StatusGraphic> GetStatusGraphicsForRange(int featRef, double min, double max);
        int GetStatusGraphicCountByRef(int featRef);
        List<StatusGraphic> GetStatusGraphicsByRef(int featRef);
                
        #endregion
        
        #region Update
        HsDevice UpdateDeviceByRef(int devRef, Dictionary<EProperty, object> changes);
        HsFeature UpdateFeatureByRef(int featRef, Dictionary<EProperty, object> changes);
        //HsFeature UpdateFeatureValueByRef(int featRef, double value);

        void UpdatePropertyByRef(int devOrFeatRef, EProperty property, object value);
        
        void AddStatusControlToFeature(int featRef, StatusControl statusControl);
        bool DeleteStatusControlByValue(int featRef, double value);
        void ClearStatusControlsByRef(int featRef);
        
        void AddStatusGraphicToFeature(int featRef, StatusGraphic statusGraphic);
        bool DeleteStatusGraphicByValue(int featRef, double value);
        void ClearStatusGraphicsByRef(int featRef);

        #endregion
        
        #region Delete
        
        /// <summary>
        /// Delete the <see cref="HsDevice"/> with the specified <see cref="AbstractHsDevice.Ref"/> and all
        ///  other <see cref="AbstractHsDevice"/>s associated with it.
        /// </summary>
        /// <param name="devRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="HsDevice"/> to delete</param>
        /// <returns>
        /// TRUE if the <see cref="HsDevice"/> was deleted, FALSE if there was an error.
        ///  Check the HS logs for more info on the error.
        /// </returns>
        bool DeleteDevice(int devRef);
        
        /// <summary>
        /// Delete the <see cref="HsFeature"/> with the specified <see cref="AbstractHsDevice.Ref"/>
        /// </summary>
        /// <param name="featRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="HsFeature"/> to delete</param>
        /// <returns>
        /// TRUE if the <see cref="HsFeature"/> was deleted, FALSE if there was an error.
        ///  Check the HS logs for more info on the error.
        /// </returns>
        bool DeleteFeature(int featRef);

        /// <summary>
        /// Delete all devices, and their corresponding features, from the HomeSeer system that are managed by
        ///  the specified plugin interface
        /// </summary>
        /// <param name="interfaceName">
        /// The name of the interface that owns all of the devices and features to delete. This is usually the plugin Id
        /// </param>
        /// <returns>TRUE if the delete was successful, FALSE if there was a problem during the process.</returns>
        bool DeleteDevicesByInterface(string interfaceName);
        
        #endregion
        
        #region Control

        /// <summary>
        /// Set the value on a feature and trigger HomeSeer to process the update to update the status accordingly.
        /// <para>
        /// To update the value without triggering HomeSeer to process the update, call
        ///  <see cref="UpdatePropertyByRef"/>
        /// </para>
        /// </summary>
        /// <remarks>
        /// This is the same as the legacy method SetDeviceValueByRef(Integer, Double, True).
        /// </remarks>
        /// <param name="featRef">The unique reference of the feature to control</param>
        /// <param name="value">The new value to set on the feature</param>
        /// <returns>TRUE if the control sent correctly, FALSE if there was a problem</returns>
        bool UpdateFeatureValueByRef(int featRef, double value);
        /// <summary>
        /// Set the value on a feature by string and trigger HomeSeer to process the update to update the status
        ///  accordingly
        /// </summary>
        /// <remarks>
        /// This is the same as the legacy method SetDeviceString(Integer, String, True)
        /// </remarks>
        /// <param name="featRef">The unique reference of the feature to control</param>
        /// <param name="value">The new value to set on the feature</param>
        /// <returns>TRUE if the control sent correctly, FALSE if there was a problem</returns>
        bool UpdateFeatureValueStringByRef(int featRef, string value);

        //bool SendControlForFeatureByValue(int featRef, double value);
        
        #endregion

        #endregion
        
        #region Events
        
        #region Create
        
        /// <summary>
        /// Create a new event with a specific name in a particular group
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="name">The name of the new event</param>
        /// <param name="group">The group to add the event to</param>
        /// <returns>The Ref of the new event</returns>
        int CreateEventWithNameInGroup(string name, string group);
        
        #endregion
        
        #region Read
        
        /// <summary>
        /// Get the name of the event with the specific Ref
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="eventRef">The Ref of the event to read</param>
        /// <returns>The name of the event</returns>
        string GetEventNameByRef(int eventRef);
        
        /// <summary>
        /// Get the DateTime of the last time a specific event was triggered
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="evRef">The Ref of the event to read</param>
        /// <returns>The DateTime the event was last triggered</returns>
        DateTime GetEventTriggerTime(int evRef);
        
        /// <summary>
        /// Get the voice command attached to an event
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="evRef">The Ref of the event to read</param>
        /// <returns>The voice command string set on the event</returns>
        string GetEventVoiceCommand(int evRef);
        
        /// <summary>
        /// Get the Ref of an event by name
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="eventName">The name of the event to read</param>
        /// <returns>The Ref of the event</returns>
        int GetEventRefByName(string eventName);
        
        /// <summary>
        /// Get the Ref of an event by its name and the group it is in
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="eventName">The name of the event</param>
        /// <param name="eventGroup">The name of the group</param>
        /// <returns>The Ref of the event</returns>
        int GetEventRefByNameAndGroup(string eventName, string eventGroup);
        
        /// <summary>
        /// Get the data for an event group
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="groupRef">The Ref of the group</param>
        /// <returns>The data of the event group</returns>
        EventGroupData GetEventGroupById(int groupRef);
        
        /// <summary>
        /// Get the data for all event groups
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <returns>A list of all of the event groups</returns>
        List<EventGroupData> GetAllEventGroups();
        
        /// <summary>
        /// Get the data for an event
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="eventRef">The Ref of the event</param>
        /// <returns>The data of the event</returns>
        EventData GetEventByRef(int eventRef);
        
        /// <summary>
        /// Get the data for all of the events
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <returns>A list of all of the events</returns>
        List<EventData> GetAllEvents();
        
        /// <summary>
        /// Get all of the events in a particular group
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="groupId">The Ref of the event group</param>
        /// <returns>A list of all of the events in the event group</returns>
        List<EventData> GetEventsByGroup(int groupId);
        
        /// <summary>
        /// Get all of the event actions managed by a specific plugin
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="pluginId">The ID of the plugin</param>
        /// <returns>A list of all of the actions managed by the plugin</returns>
        List<TrigActInfo> GetActionsByInterface(string pluginId);
        
        /// <summary>
        /// Determine if logging is enabled on an event
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="eventRef">The Ref of the event</param>
        /// <returns>TRUE if logging is enabled for the event</returns>
        bool IsEventLoggingEnabledByRef(int eventRef);
        
        /// <summary>
        /// Determine if an event is enabled or not
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="evRef">The Ref of the event</param>
        /// <returns>TRUE if the event is enabled</returns>
        bool EventEnabled(int evRef);
        
        /// <summary>
        /// The number of events configured on the HomeSeer system
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        int EventCount { get; }
        
        /// <summary>
        /// Determine if an event with a particular Ref exists
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="evRef">The Ref of the event</param>
        /// <returns>TRUE if an event with the specified Ref exists</returns>
        bool EventExistsByRef(int evRef);
        
        /// <summary>
        /// Get all of the triggers managed by a specific plugin
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="pluginId">The ID of the plugin</param>
        /// <returns>An array of triggers managed by the plugin</returns>
        TrigActInfo[] GetTriggersByInterface(string pluginId);
        
        #endregion
        
        #region Update
        
        /// <summary>
        /// Run an event causing it to execute its actions
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="eventRef">The Ref of the event</param>
        /// <returns>TRUE if the event run started successfully. This does not indicate that the actions succeeded.</returns>
        bool TriggerEventByRef(int eventRef);
        
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        string AddDeviceActionToEvent(int evRef, ControlEvent CC);
        
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        bool EventSetTimeTrigger(int evRef, DateTime DT);
        
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        bool EventSetRecurringTrigger(int evRef, TimeSpan Frequency, bool Once_Per_Hour, bool Reference_To_Hour);
        
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        void AddActionRunScript(int @ref, string script, string method, string parms);
        
        /// <summary>
        /// Disable an event
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="evRef">The Ref of the event to disable</param>
        void DisableEventByRef(int evRef);
        
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        void DeleteAfterTrigger_Set(int evRef);
        
        /// <summary>
        /// Enable an event
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="evref">The Ref of the event to enable</param>
        void EnableEventByRef(int evref);
        
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        void DeleteAfterTrigger_Clear(int evRef);

        //TODO Unable to point to a specific action on an event with multiple actions
        /// <summary>
        /// Update the data saved to an event action
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="plugId">The ID of the plugin</param>
        /// <param name="evRef">The Ref of the event</param>
        /// <param name="actInfo">The data to save to the event action</param>
        /// <returns>A message describing the result. Empty if it was successful</returns>
        string UpdatePlugAction(string plugId, int evRef, TrigActInfo actInfo);
        //TODO UpdatePlugTrigger
        
        #endregion
        
        #region Delete
        
        /// <summary>
        /// Delete an event
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="evRef">The Ref of the event to delete</param>
        void DeleteEventByRef(int evRef);
        
        #endregion
        
        /// <summary>
        /// HomeSeer has the ability to raise events in applications and plug-ins when one of a list of specific
        ///  events in HomeSeer occurs (See RegisterEventCB).  RegisterGenericEventCB allows an application or
        ///  plug-in writer the opportunity to have custom events raised and to enable other applications and plug-ins
        ///  to receive those callbacks. To remove the callback script, call UnRegisterGenericEventCB.
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="genericType">
        /// This is a string that identifies the callback.  For example, a type of "MyPlugEvent" would mean that calls
        ///  to RaiseGenericEventCB using something other than "MyPlugEvent" would be ignored. This string should be
        ///  unique, and should be provided to all applications wishing to register to receive these callbacks.
        ///  A special value of a single asterisk (*) can be used to indicate that you wish to receive ALL generic
        ///  type callbacks from other plug-ins/applications.
        /// </param>
        /// <param name="pluginId">The ID of the plugin to call</param>
        void RegisterGenericEventCB(string genericType, string pluginId);
        
        /// <summary>
        /// This will remove an application or plug-in from the list that should receive generic event callbacks
        ///  for the type indicated (See RegisterGenericEventCB).
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="genericType">This is the generic type string that was used to register the callback with RegisterGenericEventCB.</param>
        /// <param name="pluginId">The ID of the plugin remove from the callback listen</param>
        void UnRegisterGenericEventCB(string genericType, string pluginId);
        
        /// <summary>
        /// When an application or plug-in registers to receive specific types of generic HSEvent callbacks,
        ///  this procedure is used to raise those callbacks and send information to that application.
        ///  See RegisterGenericEventCB , UnRegisterGenericEventCB ,and HSEvent  information for more details.
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="genericType">
        /// This is the generic type name that was used when the receiving plug-in or application called RegisterGenericEventCB.
        ///  If you know that the plug-in or application that you wish to raise the generic event with used an asterisk
        ///  as the Generic Type, then you can use any text here as that plug-in will receive all generic event callbacks.
        /// </param>
        /// <param name="parms">
        /// These are parameters that you wish to be passed to the receiving application.
        ///  As an array of objects, it can contain strings, integers, other objects, etc.
        /// </param>
        /// <param name="pluginId">The ID of the plugin</param>
        void RaiseGenericEventCB(string genericType, object[] parms, string pluginId);
        
        /// <summary>
        /// Call this function when your plugin initializes to notify HomeSeer that you want to be called when a
        ///  specific event happens. The normal use for this is to be notified when a device changes value or
        ///  it's displayed string changes. You will be notified about any device change, not just changes to
        ///  your own devices. However, if your device is controlled and your SetIOMulti() call is made, after you
        ///  call back with hs.SetDeviceValueByRef you will get an HSEvent notifying you about the value change.
        ///  Since the change is to your device, this notification should be ignored. When an event is detected that
        ///  has beenn registered by your plug-in, call is made to the HSEvent function in your plug-in.
        ///  You can then handle the event. See HSEvent  for more information and an example.
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="evType">The type of event to register a callback for</param>
        /// <param name="pluginId">The ID of the plugin that should be called</param>
        void RegisterEventCB(Constants.HSEvent evType, string pluginId);
        
        //This doesn't exist in the legacy API?
        //void UnRegisterEventCB(Constants.HSEvent evType, string pluginId);

        /// <summary>
        /// This function returns an array of strTrigActInfo which matches the given plug-in, trigger number, and
        ///  sub-trigger number provided.  GetTriggers returns all triggers, so use TriggerMatches when you only
        ///  want to know if there are triggers in events for a specific plug-in's trigger.
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="pluginId">The ID of the plugin</param>
        /// <param name="trigId">The ID of the trigger</param>
        /// <param name="subTrigId">The ID of the subtrigger</param>
        /// <returns></returns>
        TrigActInfo[] TriggerMatches(string pluginId, int trigId, int subTrigId);
        
        /// <summary>
        /// Get all of the triggers of a particular type
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="pluginName">The ID of the plugin that owns the trigger type</param>
        /// <param name="trigId">The ID of the trigger type</param>
        /// <returns>An array of trigger data</returns>
        TrigActInfo[] GetTriggersByType(string pluginName, int trigId);
        
        /// <summary>
        /// This function is a callback function and is called when a plugin detects that a trigger condition is true.
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: This was ported directly from the legacy HS3 API and has not been fully reviewed to ensure
        ///  proper compatibility and support through this SDK.  This may undergo significant change in the near future.
        ///  Please use with caution.
        /// </remarks>
        /// <param name="pluginId">The ID of the plugin</param>
        /// <param name="trigInfo">The data of the trigger to fire</param>
        void TriggerFire(string pluginId, TrigActInfo trigInfo);

        #endregion
        
        #region System
        
        /// <summary>
        /// Get the current version of HomeSeer that is running
        /// </summary>
        /// <returns>The version string for HomeSeer in the format of MAJOR.MINOR.PATCH.BUILD</returns>
        string Version();
        
        //TODO PSDK-60 IHsController.GetHsEdition() is out of date
        /// <summary>
        /// Get the current edition of HomeSeer that is running
        /// </summary>
        /// <returns>The edition of HomeSeer currently running</returns>
        Constants.editions GetHSEdition();
        
        /// <summary>
        /// Get a list of users and their rights
        /// </summary>
        /// <returns>a list of users in this format: username|rights,username2|rights2...</returns>
        string GetUsers();
        
        /// <summary>
        /// Determine if the HomeSeer system is licensed using any license, including a trial
        /// </summary>
        /// <returns>TRUE if the HomeSeer system is licensed</returns>
        bool IsLicensed();
        
        /// <summary>
        /// Determine if the HomeSeer system is registered using a paid license
        /// </summary>
        /// <returns>TRUE if the HomeSeer system is registered with a paid license</returns>
        bool IsRegistered();
        
        /// <summary>
        /// Determine if Location1 is used first on devices/features.
        /// </summary>
        /// <remarks>
        /// By default, Location2 is used as the first logical location when organizing devices/features.
        ///  For this reason, it is important to check which location is marked as the first location before working
        ///  with locations.
        /// </remarks>
        /// <returns>TRUE if Location1 is used first, FALSE if Location2 is used first</returns>
        bool IsLocation1First();
        
        /// <summary>
        /// Get an alpha-sorted list of Location1 strings
        /// </summary>
        /// <returns>A SortedList of Location1 location strings</returns>
        System.Collections.SortedList GetLocationsList();
        
        /// <summary>
        /// Get the name of the Location1 location
        /// </summary>
        /// <returns>The user defined name of Location1</returns>
        string GetLocation1Name();
        
        /// <summary>
        /// Get an alpha-sorted list of Location2 strings
        /// </summary>
        /// <returns>A SortedList of Location2 location strings</returns>
        System.Collections.SortedList GetLocations2List();
        
        /// <summary>
        /// Get the name of the Location2 location
        /// </summary>
        /// <returns>The user defined name of Location2</returns>
        string GetLocation2Name();

        /// <summary>
        /// Get the name of the first location.
        /// <para>
        /// This is the name of the location that is marked as first according to <see cref="IsLocation1First"/>
        /// </para>
        /// </summary>
        /// <returns>The name of the first location</returns>
        string GetFirstLocationName();
        
        /// <summary>
        /// Get the name of the second location.
        /// <para>
        /// This is the name of the location that is marked as second according to <see cref="IsLocation1First"/>
        /// </para>
        /// </summary>
        /// <returns>The name of the second location</returns>
        string GetSecondLocationName();
        
        /// <summary>
        /// Get an alpha-sorted list of the location strings marked as first
        /// <para>
        /// This is the list of location strings that are marked as first according to <see cref="IsLocation1First"/>
        /// </para>
        /// </summary>
        /// <returns>A List of the first location strings</returns>
        List<string> GetFirstLocationList();
        
        /// <summary>
        /// Get an alpha-sorted list of the location strings marked as second
        /// <para>
        /// This is the list of location strings that are marked as second according to <see cref="IsLocation1First"/>
        /// </para>
        /// </summary>
        /// <returns>A List of the second location strings</returns>
        List<string> GetSecondLocationList();
        
        /// <summary>
        /// Get the <see cref="Types.ERegistrationMode"/> of a plugin
        /// </summary>
        /// <param name="pluginId">The ID of the plugin to read</param>
        /// <returns>The <see cref="Types.ERegistrationMode"/> of the plugin with the specified ID</returns>
        int CheckRegistrationStatus(string pluginId);

        /// <summary>
        /// Get the type of OS HomeSeer is running on as <see cref="Types.EOsType"/>
        /// </summary>
        /// <returns>0 = windows, 1 = linux</returns>
        /// <seealso cref="Types.EOsType"/>
        int GetOsType();
        
        /// <summary>
        /// Obtain the IP address the HomeSeer system is accessible through
        /// </summary>
        /// <returns>A string representation of the IP address HomeSeer is running on</returns>
        string GetIpAddress();
        
        #region DateTime
        
        /// <summary>
        /// Get the DateTime for Solar Noon from the HomeSeer system
        /// </summary>
        DateTime SolarNoon { get; }
        /// <summary>
        /// Get the DateTime for Sunrise from the HomeSeer system
        /// </summary>
        DateTime Sunrise   { get; }
        /// <summary>
        /// Get the DateTime for Sunset from the HomeSeer system
        /// </summary>
        DateTime Sunset    { get; }
        
        #endregion

        //TODO System methods
        //int InterfaceVersion();
        //bool IsApplicationRunning(string ApplicationName);
        //string RecurseFiles(string SourceDir);
        //string[] RecurseFilesEx(string SourceDir);
        
        //string GetOSVersion();
        //string HSMemoryUsed();
        //int HSModules();
        //int HSThreads();
        //void PowerFailRecover();
        //void RestartSystem();
        //void ShutDown();
        //string SystemUpTime();
        //TimeSpan SystemUpTimeTS();
        //void WindowsLockSystem();
        //void WindowsLogoffSystem();
        //void WindowsShutdownSystem();
        //void WindowsRebootSystem();

        #endregion
        
        #region Logging

        /// <summary>
        /// Write a message to the HomeSeer logs
        /// </summary>
        /// <param name="logType">The <see cref="ELogType"/> to write</param>
        /// <param name="message">The message to write to the log</param>
        /// <param name="pluginName">The name of your plugin, used to mark the source of the log message</param>
        /// <param name="color">The color code to use. NOTE: Legacy HS3 API implementation</param>
        void WriteLog(ELogType logType, string message, string pluginName, string color = "");
        
        #endregion
        
        #region Energy

        //Create
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        string Energy_AddCalculator(int dvRef, string Name, TimeSpan Range, TimeSpan StartBack);
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        string Energy_AddCalculatorEvenDay(int dvRef, string Name, TimeSpan Range, TimeSpan StartBack);
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        bool Energy_AddData(int dvRef, EnergyData Data);
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        bool Energy_AddDataArray(int dvRef, EnergyData[] colData);
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        bool Energy_SetEnergyDevice(int dvRef, Constants.enumEnergyDevice DeviceType);
        
        //Read
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        int Energy_CalcCount(int dvRef);
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        SortedList<int, string> Energy_GetGraphDataIDs();
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        SortedList<int, string> Energy_GetEnergyRefs(bool GetParentRefs);
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        System.Drawing.Image Energy_GetGraph(int id, string dvRefs, int width, int height, string format);        
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        List<EnergyData> Energy_GetData(int dvRef,DateTime dteStart,DateTime dteEnd);
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        List<EnergyData> Energy_GetArchiveData(int dvRef, DateTime dteStart, DateTime dteEnd);
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        List<EnergyData> Energy_GetArchiveDatas(string dvRefs, DateTime dteStart, DateTime dteEnd);
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        EnergyCalcData Energy_GetCalcByName(int dvRef, string Name);
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        EnergyCalcData Energy_GetCalcByIndex(int dvRef, int Index);
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        EnergyGraphData Energy_GetGraphData(int ID);
        
        //Update
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        int Energy_SaveGraphData(EnergyGraphData Data);
        
        //Delete
        /// <summary>
        /// PLEASE NOTE: Code related to the Energy components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Energy API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </summary>
        int Energy_RemoveData(int dvRef, DateTime dteStart);
        
        #endregion
        
        #region Speech
        
        /// <summary>
        /// This procedure is used to cause HomeSeer to speak something when a speak proxy is registered and active.
        ///  Since speak commands when a speak proxy plug-in is registered are trapped and passed to the SpeakIn
        ///  procedure of the speak proxy plug-in, this command is used when the speak proxy plug-in is ready to do
        ///  the real speaking.
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: Code related to the Speech components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Speech API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </remarks>
        /// <param name="speechDevice">
        /// This is the device that is to be used for the speaking.  In older versions of HomeSeer, this value was
        ///  used to indicate the sound card to use, and if it was over 100, then it indicated that it was speaking
        ///  for HomeSeer Phone (device - 100 = phone line), or the WAV audio device to use.
        ///  Although this is still used for HomeSeer Phone, speaks for HomeSeer phone are never proxied and so
        ///  values >= 100 should never been seen in the device parameter.
        ///  Pass the device parameter unchanged to SpeakProxy.
        /// </param>
        /// <param name="spokenText">
        /// This is the text to be spoken, or if it is a WAV file to be played, then the characters ":\" will be
        ///  found starting at position 2 of the string as playing a WAV file with the speak command in HomeSeer
        ///  REQUIRES a fully qualified path and filename of the WAV file to play.
        /// </param>
        /// <param name="wait">
        /// This parameter tells HomeSeer whether to continue processing commands immediately or to wait until
        ///  the speak command is finished - pass this parameter unchanged to SpeakProxy.
        /// </param>
        /// <param name="host">
        /// This is a list of host:instances to speak or play the WAV file on.
        ///  An empty string or a single asterisk (*) indicates all connected speaker clients on all hosts.
        ///  Normally this parameter is passed to SpeakProxy unchanged.
        /// </param>
        void SpeakProxy(int speechDevice, string spokenText, bool wait, string host = "");

        /// <summary>
        /// Sends TTS to a file using the system voice
        /// </summary>
        /// <remarks>
        /// PLEASE NOTE: Code related to the Speech components in HomeSeer were ported from the HS3 plugin API and
        ///  have not been fully tested to verify full functionality from the new SDK. The Speech API may undergo
        ///  significant changes in the near future. Please use with caution.
        /// </remarks>
        /// <param name="Text">The text to speak</param>
        /// <param name="Voice">The voice to use, SAPI only on Windows</param>
        /// <param name="FileName">Filename to send the speech to</param>
        /// <returns></returns>
        bool SpeakToFile(string Text, string Voice, string FileName);

        #if WIP
        /// <summary>
        /// Register your plug-in as a Speak Proxy plug-in.
        /// <para>
        /// After this registration, whenever a Speak command is issued in HomeSeer,
        ///  your plug-in's SpeakIn procedure will be called instead.
        ///  When your plug-in wishes to have HomeSeer actually speak something, it uses SpeakProxy instead of Speak.
        /// </para>
        /// <para>
        /// If you no longer wish to proxy Speak commands in your plug-in, or when your plug-in has its Shutdown
        ///  procedure called, use UnRegisterProxySpeakPlug to remove the registration as a speak proxy.
        /// </para>
        /// </summary>
        /// <param name="pluginId">The Id of your plugin</param>
        void RegisterProxySpeakPlug(string pluginId);
        
        /// <summary>
        /// Unregister a plug-in as a Speak proxy that was previously
        ///  registered using RegisterProxySpeakPlug.
        /// </summary>
        /// <param name="pluginId">The Id of your plugin</param>
        void UnRegisterProxySpeakPlug(string pluginId);
#endif

        #endregion

        /// <summary>
        /// HomeSeer supports the use of replacement variables, which is the use of special tags to indicate where
        ///  HomeSeer should replace the tag with text information.  A full list of replacement variables is listed
        ///  in HomeSeer's help file.
        /// </summary>
        /// <param name="strIn">A string with the replacement variables</param>
        /// <returns>A string with the replacement variables removed with the indicated values put in their place</returns>
        string ReplaceVariables(string strIn);

        /// <summary>
        /// Returns the path to the HS executable. Some plugins need this when running remotely
        /// </summary>
        /// <returns>The path to the HomeSeer executable</returns>
        string GetAppPath();
        
        #region Images

        /// <summary>
        /// Save the specified image, as a byte array, to file in the HomeSeer html images directory
        /// </summary>
        /// <param name="imageBytes">A byte array of the image to save</param>
        /// <param name="destinationFile">The path of the image following "\html\images\"</param>
        /// <param name="overwriteExistingFile">TRUE to overwrite any existing file, FALSE to not</param>
        /// <returns>TRUE if the file was saved successfully, FALSE if there was a problem</returns>
        /// <example>
        /// The following example shows how to download an image from a URL and save the bytes to file from the HSPI class.
        /// 
        /// <code>
        /// var url = "http://homeseer.com/images/HS4/hs4-64.png";
        /// var webClient = new WebClient();
        /// var imageBytes = webClient.DownloadData(url);
        /// var filePath = $"{Id}\\{Path.GetFileName(url)}";
        /// if (!HomeSeerSystem.SaveImageFile(imageBytes, filePath, true)) {
        ///     Console.WriteLine($"Error saving {url} to {filePath}");
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// The following example shows how to convert an image to bytes and save them from the HSPI class.
        ///
        /// <code>
        /// var myImage = System.Drawing.Image.FromFile("sampleImage.png");
        /// var imageBytes = new byte[0];
        /// using (var ms = new MemoryStream()) {
        ///     myImage.Save(ms, myImage.RawFormat);
        ///     imageBytes = ms.toArray();
        /// }
        /// var filePath = $"{Id}\\sampleImage.png";
        /// if (!HomeSeerSystem.SaveImageFile(imageBytes, filePath, true)) {
        ///     Console.WriteLine($"Error saving sampleImage.png to {filePath}");
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="DeleteImageFile"/>
        bool SaveImageFile(byte[] imageBytes, string destinationFile, bool overwriteExistingFile);
        /// <summary>
        /// Delete the specified file from HomeSeer's HTML image directory.
        /// </summary>
        /// <param name="targetFile">The path of the image following "\html\images\"</param>
        /// <returns>TRUE if the file was deleted successfully, FALSE if it still exists</returns>
        /// <example>
        /// The following example shows how to delete an image from HomeSeer's HTML image directory.
        /// 
        /// <code>
        /// var filePath = $"{Id}\\sampleImage.png";
        /// if (!HomeSeerSystem.DeleteImageFile(filePath)) {
        ///     Console.WriteLine($"Error deleting {filePath}");
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="SaveImageFile"/>
        bool DeleteImageFile(string targetFile);

        #endregion

        #region Not Implemented

        #region Scripts

        //TODO Script methods
        //object PluginFunction(string plugname, string pluginstance, string func,object[] parms);
        //object PluginPropertyGet(string plugname, string pluginstance, string func,object[] parms);
        //void PluginPropertySet(string plugname, string pluginstance, string prop,object value);
        //int SendMessage(string message, string host, bool showballoon);
        //int Launch(string Name, string @params, string direc, int LaunchPri);
        //bool RegisterStatusChangeCB(string script, string func);
        //void UnRegisterStatusChangeCB(string script);
        //string GetScriptPath();
        //string InstallScript(string scr_name, object param);
        //bool IsScriptRunning(string scr);
        //object RunScript(string scr, bool Wait, bool SingleInstance);
        //object RunScriptFunc(string scr, string func, object param, bool Wait, bool SingleInstance);
        //string ScriptsRunning();
        //int ValidateScriptLicense(string LicenseID, string ProductID);
        //int ValidateScriptLicenseDisplay(string LicenseID, string ProductID, bool bDisplay);

        #endregion

        #region COM

        //TODO COM port methods
        //void CloseComPort(int port);
        //int GetComPortCount(int port);
        //object GetComPortData(int port);
        //string OpenComPort(int port, string Config, int mode, string cb_script, string cb_func);
        //string OpenComPortTerm(int port, string Config, int mode, string cb_script, string cb_func, string term);
        //void SendToComPort(int port, string sData);
        //void SendToComPortBytes(int port, byte[] Data);
        //void SetComPortRTSDTR(int port, bool rtsval, bool dtrval);

        #endregion

        #region Networking & Web

        //TODO Networking methods
        //bool WEBCheckUserRights(int rights);
        //string WEBLoggedInUser();
        //bool WEBValidateUser(string username, string password);
        //string GetLastRemoteIP();
        //string LANIP();
        //string WANIP();
        //int WebServerPort();
        //int WebServerSSLPort();

        //string GenCookieString(string Name, string Value, string expire = "", string path = "/");

        #endregion

        #region AppCallback

        //TODO AppCallback methods
        
        #endregion

        //TODO other methods
        //bool AppStarting(bool wait = false);
        //string BackupDB();
        //object GetHSPRef();
        //string GetInstanceList();
        //string GetPlugLinks();
        //string[] GetPluginsList();
        //int GetRemoteTimeout();
        //string GetSource();
        //void Keys(string k, string title, bool waitf);
        //int LCID();

        //void SetRemoteTimeout(int timeout_seconds);
        //void SetSecurityMode(bool mode);
        //void WaitEvents();
        //Constants.eOSType GetOSType();
        //clsLastVR[] GetLastVRCollection();
        //Constants.REGISTRATION_MODES PluginLicenseMode(string IfaceName);

        #endregion

    }

}