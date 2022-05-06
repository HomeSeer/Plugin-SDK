using System;
using System.Collections.Generic;
using System.Linq;
using HomeSeer.PluginSdk.Devices.Controls;
using HomeSeer.PluginSdk.Devices.Identification;
// ReSharper disable MemberCanBePrivate.Global

namespace HomeSeer.PluginSdk.Devices {

    /// <summary>
    /// Factory class for defining new <see cref="HsFeature"/>s for HomeSeer
    /// </summary>
    public class FeatureFactory {
        
        internal HsFeature Feature => _feature;

        private HsFeature _feature;

        /// <summary>
        /// Prepare a new feature definition
        /// </summary>
        /// <remarks>
        /// Make sure to associate this feature with a device before creating it. You can do this with
        ///  <see cref="OnDevice"/> or <see cref="PrepareForHsDevice"/> if you are creating this feature on its own,
        ///  or you can add this to a <see cref="DeviceFactory"/> with <see cref="DeviceFactory.WithFeature"/>.
        /// </remarks>
        /// <param name="pluginId">The <see cref="IPlugin.Id"/> of the plugin that owns the new feature</param>
        /// <returns>A <see cref="FeatureFactory"/> containing information about the new feature</returns>
        public static FeatureFactory CreateFeature(string pluginId) {
            var ff = new FeatureFactory();
            var feature = new HsFeature
                          {
                              Relationship = ERelationship.Feature,
                              Interface    = pluginId
                          };
            feature.Changes.Add(EProperty.Misc, 
                                AbstractHsDevice.GetMiscForFlags(
                                                                 EMiscFlag.ShowValues, 
                                                                 EMiscFlag.SetDoesNotChangeLastChange)
                                );
            feature.Changes.Add(EProperty.UserAccess, "Any");
            feature.Changes.Add(EProperty.Location2, "Plugin");
            feature.Changes.Add(EProperty.Location, pluginId);
            ff._feature = feature;

            return ff;
        }

        /// <summary>
        /// Prepare a new feature definition for a specific <see cref="HsDevice"/>
        /// </summary>
        /// <param name="pluginId">The <see cref="IPlugin.Id"/> of the plugin that owns the new feature</param>
        /// <param name="devRef">The <see cref="AbstractHsDevice.Ref"/> of the device that should own the new feature</param>
        /// <returns>A <see cref="FeatureFactory"/> containing information about the new feature</returns>
        public static FeatureFactory CreateFeature(string pluginId, int devRef) {
            if (devRef <= 0) {
                throw new ArgumentOutOfRangeException(nameof(devRef));
            }

            var ff = CreateFeature(pluginId);
            ff._feature.AssociatedDevices = new HashSet<int> {devRef};

            return ff;
        }
        
        /// <summary>
        /// Create a new generic, binary control feature that has 2 button controls and 2
        ///  corresponding status graphics.
        /// </summary>
        /// <param name="pluginId">The Id of the plugin to be used as the interface property</param>
        /// <param name="name">The name of the feature</param>
        /// <param name="onText">The text on the On button</param>
        /// <param name="offText">The text on the Off button</param>
        /// <param name="onValue">The corresponding value for the On state</param>
        /// <param name="offValue">The corresponding value for the Off state</param>
        /// <returns>A FeatureFactory representing the desired feature</returns>
        public static FeatureFactory CreateGenericBinaryControl(string pluginId, string name, 
                                                                string onText, string offText, 
                                                                double onValue = 1, double offValue = 0) {

            var ff = CreateFeature(pluginId).WithName(name);
            ff.AsType(EFeatureType.Generic, (int) EGenericFeatureSubType.BinaryControl);
            ff.AddButton(offValue,
                         offText,
                         new ControlLocation(1,1),
                         EControlUse.Off);
            ff.AddButton(onValue,
                         onText,
                         new ControlLocation(1,2),
                         EControlUse.On);
            ff.AddGraphicForValue("/images/HomeSeer/status/off.gif", offValue, offText);
            ff.AddGraphicForValue("/images/HomeSeer/status/on.gif", onValue, onText);

            return ff;
        }
        
        /// <summary>
        /// Create a new generic, binary sensor feature that has 2 status graphics representing 2 different
        ///  sensor states.
        /// </summary>
        /// <param name="pluginId">The Id of the plugin to be used as the interface property</param>
        /// <param name="name">The name of the feature</param>
        /// <param name="onText">The text displayed when the status is active</param>
        /// <param name="offText">The text displayed when the status is passive</param>
        /// <param name="onValue">The corresponding value for the active state</param>
        /// <param name="offValue">The corresponding value for the passive state</param>
        /// <returns>A FeatureFactory representing the desired feature</returns>
        public static FeatureFactory CreateGenericBinarySensor(string pluginId, string name, 
                                                               string onText, string offText, 
                                                               double onValue = 1, double offValue = 0) {

            var ff = CreateFeature(pluginId).WithName(name);
            ff.AsType(EFeatureType.Generic, (int) EGenericFeatureSubType.BinarySensor);
            ff.AddGraphicForValue("/images/HomeSeer/status/noevent.png", offValue, offText);
            ff.AddGraphicForValue("/images/HomeSeer/status/on-open-motion.png", onValue, onText);

            return ff;
        }
        
        #region Feature Properties

        /// <summary>
        /// Associate the feature with a specific <see cref="HsDevice"/>
        /// </summary>
        /// <param name="devRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="HsDevice"/> that owns the feature</param>
        /// <returns>The calling FeatureFactory with its <see cref="AbstractHsDevice.AssociatedDevices"/> set to link to the desired device</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if a <paramref name="devRef"/> is supplied that is less than or equal to 0</exception>
        public FeatureFactory OnDevice(int devRef) {

            if (devRef <= 0) {
                throw new ArgumentOutOfRangeException(nameof(devRef));
            }
            
            _feature.AssociatedDevices = new HashSet<int> {devRef};

            return this;
        }

        /// <summary>
        /// Add an <see cref="AbstractHsDevice.Address"/> to the feature
        /// </summary>
        /// <param name="address">The string to set the address to</param>
        /// <returns>The <see cref="FeatureFactory"/> with the updated address value</returns>
        public FeatureFactory WithAddress(string address) {
            _feature.Address = address;
            return this;
        }

        /// <summary>
        /// Set the name of the <see cref="HsFeature"/>. This sets <see cref="AbstractHsDevice.Name"/>
        /// </summary>
        /// <param name="name">The name of the feature</param>
        /// <returns>The calling FeatureFactory updated with the desired name</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="name"/> is empty or whitespace</exception>
        public FeatureFactory WithName(string name) {

            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentNullException(nameof(name));
            }

            _feature.Name = name;

            return this;
        }
        
        /// <summary>
        /// Set the <see cref="AbstractHsDevice.PlugExtraData"/> for the <see cref="HsFeature"/>
        /// </summary>
        /// <param name="extraData"><see cref="PlugExtraData"/> to set on the <see cref="HsFeature"/></param>
        /// <returns>The calling FeatureFactory with the specified <see cref="PlugExtraData"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="extraData"/> is null</exception>
        public FeatureFactory WithExtraData(PlugExtraData extraData) {

            if (extraData == null) {
                throw new ArgumentNullException(nameof(extraData));
            }

            _feature.PlugExtraData = extraData;

            return this;
        }
        
        /// <summary>
        /// Add a <see cref="EMiscFlag"/> to the feature
        /// </summary>
        /// <param name="miscFlags"><see cref="EMiscFlag"/>(s) to add</param>
        /// <returns>The FeatureFactory updated by adding the specified <see cref="EMiscFlag"/>(s)</returns>
        /// <exception cref="ArgumentNullException">Thrown when no <paramref name="miscFlags"/> are specified</exception>
        public FeatureFactory WithMiscFlags(params EMiscFlag[] miscFlags) {

            if (miscFlags == null || miscFlags.Length == 0) {
                throw new ArgumentNullException(nameof(miscFlags));
            }

            foreach (var miscFlag in miscFlags.Distinct()) {
                _feature.AddMiscFlag(miscFlag);
            }
            
            return this;
        }

        /// <summary>
        /// Remove a <see cref="EMiscFlag"/> from the feature
        /// </summary>
        /// <param name="miscFlags"><see cref="EMiscFlag"/>(s) to remove</param>
        /// <returns>The FeatureFactory updated by removing the specified <see cref="EMiscFlag"/>(s)</returns>
        /// <exception cref="ArgumentNullException">Thrown when no <paramref name="miscFlags"/> are specified</exception>
        public FeatureFactory WithoutMiscFlags(params EMiscFlag[] miscFlags)
        {
            if (miscFlags == null || miscFlags.Length == 0)
            {
                throw new ArgumentNullException(nameof(miscFlags));
            }

            foreach (var miscFlag in miscFlags.Distinct())
            {
                _feature.RemoveMiscFlag(miscFlag);
            }

            return this;
        }

        /// <summary>
        /// Set the Location property on the feature.
        /// </summary>
        /// <remarks>
        /// You should only adjust this if the location on the owning device is insufficient for this feature.
        ///  This will be a location IN ADDITION to the device location.
        /// </remarks>
        /// <param name="location">The location to set on the feature</param>
        /// <returns>The FeatureFactory updated with the specified location</returns>
        /// <remarks>Null or whitespace strings will be converted to empty strings ""</remarks>
        public FeatureFactory WithLocation(string location) {
            // 09-15-2020 JLW - Default null or whitespace strings to empty string "" instead of throwing an exception PSDK-98
            _feature.Location = string.IsNullOrWhiteSpace(location) ? "" : location;

            return this;
        }

        /// <summary>
        /// Set the Location2 property on the feature.
        /// </summary>
        /// <remarks>
        /// You should only adjust this if the location2 on the owning device is insufficient for this feature.
        ///  This will be a location2 IN ADDITION to the device location2.
        /// </remarks>
        /// <param name="location2">The location2 to set on the feature</param>
        /// <returns>The FeatureFactory updated with the specified location2</returns>
        /// <remarks>Null or whitespace strings will be converted to empty strings ""</remarks>
        public FeatureFactory WithLocation2(string location2) {
            // 09-15-2020 JLW - Default null or whitespace strings to empty string "" instead of throwing an exception PSDK-98
            _feature.Location2 = string.IsNullOrWhiteSpace(location2) ? "" : location2;

            return this;
        }

        /// <summary>
        /// Set the value the feature is created with
        /// </summary>
        /// <param name="value">The value the feature should default to when it is created</param>
        /// <returns>The FeatureFactory updated with the specified value</returns>
        public FeatureFactory WithDefaultValue(double value) {

            _feature.Value = value;

            return this;
        }

        /// <summary>
        /// Set the <see cref="HsFeature.DisplayType"/> for the feature
        /// </summary>
        /// <param name="displayType">The <see cref="EFeatureDisplayType"/> to set</param>
        /// <returns>The <see cref="FeatureFactory"/> with the updated display type value</returns>
        public FeatureFactory WithDisplayType(EFeatureDisplayType displayType) {
            _feature.DisplayType = displayType;
            return this;
        }
        
        #endregion
        
        /// <summary>
        /// Set the <see cref="AbstractHsDevice.TypeInfo"/> of the <see cref="HsFeature"/>.
        /// </summary>
        /// <param name="featureType">The <see cref="EFeatureType"/> of the <see cref="HsFeature"/></param>
        /// <param name="featureSubType">An int value representing a <see cref="HsFeature"/> sub type.
        /// See <see cref="Identification"/> for enums listed as "FeatureSubType" for current lists.</param>
        /// <returns>The calling <see cref="FeatureFactory"/> with an updated <see cref="AbstractHsDevice.TypeInfo"/></returns>
        /// <seealso cref="EFeatureType"/>
        /// <seealso cref="EGenericFeatureSubType"/>
        /// <seealso cref="EEnergyFeatureSubType"/>
        /// <seealso cref="EMediaFeatureSubType"/>
        /// <seealso cref="EThermostatControlFeatureSubType"/>
        /// <seealso cref="EThermostatStatusFeatureSubType"/>
        public FeatureFactory AsType(EFeatureType featureType, int featureSubType) {

            _feature.TypeInfo = new TypeInfo()
                                 {
                                     ApiType = EApiType.Feature,
                                     Type    = (int) featureType,
                                     SubType = featureSubType
                                 };

            return this;
        }

        #region Controls

        /// <summary>
        /// Add a <see cref="StatusControl"/> to the <see cref="HsFeature"/> being built
        /// </summary>
        /// <param name="statusControl">The <see cref="StatusControl"/> to add.</param>
        /// <returns>The calling <see cref="FeatureFactory"/> with an added <see cref="StatusControl"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="statusControl"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when a <see cref="StatusControl"/> for the values targeted by <paramref name="statusControl"/> already exists.</exception>
        public FeatureFactory AddControl(StatusControl statusControl) {

            if (statusControl == null) {
                throw new ArgumentNullException(nameof(statusControl));
            }
            if (statusControl.IsRange ) {
                //If the control is a range
                if (_feature.HasControlForRange(statusControl.TargetRange)) {
                    //If the feature already has a control for the range
                    throw new ArgumentException("A value targeted by the specified statusControl already has a control bound to it.", nameof(statusControl));
                }
            }
            else if (_feature.HasControlForValue(statusControl.TargetValue)) {
                //The control is not a range
                //If the feature already has a control for the value
                throw new ArgumentException("The value targeted by the specified statusControl already has a control bound to it.", nameof(statusControl));
            }
            
            _feature.AddStatusControl(statusControl);

            return this;
        }
        
        /// <summary>
        /// Add a button to the <see cref="HsFeature"/>.
        /// </summary>
        /// <param name="targetValue">The unique value associated with this control.</param>
        /// <param name="targetStatus">The text displayed on the button.</param>
        /// <param name="location">The location of the control in the grid. See <see cref="ControlLocation"/></param>
        /// <param name="controlUse">The specific use for this control. See <see cref="EControlUse"/></param>
        /// <returns>The calling <see cref="FeatureFactory"/> with a new <see cref="StatusControl"/> added</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="targetStatus"/> is empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when a control, targeting the specified <paramref name="targetValue"/>, already exists.</exception>
        /// <seealso cref="EControlType.Button"/>
        public FeatureFactory AddButton(double targetValue, string targetStatus, ControlLocation location = null, EControlUse controlUse = EControlUse.NotSpecified) {

            if (string.IsNullOrWhiteSpace(targetStatus)) {
                throw new ArgumentNullException(nameof(targetStatus));
            }
            
            if (_feature.HasControlForValue(targetValue)) {
                throw new ArgumentException($"The value {targetValue} already has a control bound to it.", nameof(targetValue));
            }

            var button = new StatusControl(EControlType.Button)
                         {
                             TargetValue = targetValue, 
                             Label = targetStatus, 
                             ControlUse = controlUse,
                             Location = location ?? new ControlLocation()
                         };

            _feature.AddStatusControl(button);
            
            return this;
        }
        
        /// <summary>
        /// Add a text input field to the <see cref="HsFeature"/>.
        /// </summary>
        /// <param name="targetValue">The unique value associated with this control.</param>
        /// <param name="hintText">The text displayed to the user to help them know what kind of value to input.</param>
        /// <param name="location">The location of the control in the grid. See <see cref="ControlLocation"/></param>
        /// <param name="controlUse">The specific use for this control. See <see cref="EControlUse"/></param>
        /// <returns>The calling <see cref="FeatureFactory"/> with a new <see cref="StatusControl"/> added</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="hintText"/> is empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when a control, targeting the specified <paramref name="targetValue"/>, already exists.</exception>
        /// <seealso cref="EControlType.TextBoxString"/>
        public FeatureFactory AddTextInputField(double targetValue, string hintText, ControlLocation location = null, EControlUse controlUse = EControlUse.NotSpecified) {

            if (string.IsNullOrWhiteSpace(hintText)) {
                throw new ArgumentNullException(nameof(hintText));
            }
            
            if (_feature.HasControlForValue(targetValue)) {
                throw new ArgumentException($"The value {targetValue} already has a control bound to it.", nameof(targetValue));
            }

            var textInput = new StatusControl(EControlType.TextBoxString)
                         {
                             TargetValue = targetValue, 
                             Label       = hintText, 
                             ControlUse  = controlUse,
                             Location = location ?? new ControlLocation()
                         };

            _feature.AddStatusControl(textInput);
            
            return this;
        }
        
        /// <summary>
        /// Add a number input field to the <see cref="HsFeature"/>.
        /// </summary>
        /// <param name="targetValue">The unique value associated with this control.</param>
        /// <param name="hintText">The text displayed to the user to help them know what kind of value to input.</param>
        /// <param name="location">The location of the control in the grid. See <see cref="ControlLocation"/></param>
        /// <param name="controlUse">The specific use for this control. See <see cref="EControlUse"/></param>
        /// <returns>The calling <see cref="FeatureFactory"/> with a new <see cref="StatusControl"/> added</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="hintText"/> is empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when a control, targeting the specified <paramref name="targetValue"/>, already exists.</exception>
        /// <seealso cref="EControlType.TextBoxNumber"/>
        public FeatureFactory AddNumberInputField(double targetValue, string hintText, ControlLocation location = null, EControlUse controlUse = EControlUse.NotSpecified) {

            if (string.IsNullOrWhiteSpace(hintText)) {
                throw new ArgumentNullException(nameof(hintText));
            }
            
            if (_feature.HasControlForValue(targetValue)) {
                throw new ArgumentException($"The value {targetValue} already has a control bound to it.", nameof(targetValue));
            }

            var numInput = new StatusControl(EControlType.TextBoxNumber)
                            {
                                TargetValue = targetValue, 
                                Label       = hintText, 
                                ControlUse  = controlUse,
                                Location = location ?? new ControlLocation()
                            };

            _feature.AddStatusControl(numInput);
            
            return this;
        }
        
        /// <summary>
        /// Add a slider control to the <see cref="HsFeature"/>.
        /// </summary>
        /// <param name="targetRange">A <see cref="ValueRange"/></param>
        /// <param name="location">The location of the control in the grid. See <see cref="ControlLocation"/></param>
        /// <param name="controlUse">The specific use for this control. See <see cref="EControlUse"/></param>
        /// <returns>The calling <see cref="FeatureFactory"/> with a new <see cref="StatusControl"/> added</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="targetRange"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when a control, targeting any of the specified values, already exists.</exception>
        /// <seealso cref="EControlType.ValueRangeSlider"/>
        public FeatureFactory AddSlider(ValueRange targetRange, ControlLocation location = null, EControlUse controlUse = EControlUse.NotSpecified) {

            if (targetRange == null) {
                throw new ArgumentNullException(nameof(targetRange));
            }
            
            if (_feature.HasControlForRange(targetRange)) {
                throw new ArgumentException($"Some or all of the values in the range {targetRange.Min}-{targetRange.Max} already has a control bound to it.", nameof(targetRange));
            }
            
            var slider = new StatusControl(EControlType.ValueRangeSlider)
                           {
                               TargetRange = targetRange, 
                               ControlUse  = controlUse,
                               Location = location ?? new ControlLocation()
                           };

            _feature.AddStatusControl(slider);
            
            return this;
        }
        
        /// <summary>
        /// Add a select list control to the <see cref="HsFeature"/> for a range of numbers.
        /// </summary>
        /// <param name="targetRange">A <see cref="ValueRange"/></param>
        /// <param name="location">The location of the control in the grid. See <see cref="ControlLocation"/></param>
        /// <param name="controlUse">The specific use for this control. See <see cref="EControlUse"/></param>
        /// <returns>
        /// The calling <see cref="FeatureFactory"/> with a new <see cref="StatusControl"/> added
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="targetRange"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when a control, targeting any of the specified values, already exists.</exception>
        /// <seealso cref="EControlType.ValueRangeDropDown"/>
        [Obsolete("Due to a lack of ValueRange.Divisor property this control type cannot be properly created at this time.", false)]
        public FeatureFactory AddValueDropDown(ValueRange targetRange, ControlLocation location = null, EControlUse controlUse = EControlUse.NotSpecified) {

            if (targetRange == null) {
                throw new ArgumentNullException(nameof(targetRange));
            }
            
            if (_feature.HasControlForRange(targetRange)) {
                throw new ArgumentException($"Some or all of the values in the range {targetRange.Min}-{targetRange.Max} already has a control bound to it.", nameof(targetRange));
            }
            
            var dropDown = new StatusControl(EControlType.ValueRangeDropDown)
                         {
                             TargetRange = targetRange, 
                             ControlUse  = controlUse,
                             Location = location ?? new ControlLocation()
                         };

            _feature.AddStatusControl(dropDown);
            
            return this;
        }
        
        /// <summary>
        /// Add a select list control to the <see cref="HsFeature"/>
        /// </summary>
        /// <param name="textOptions">
        /// A <see cref="SortedDictionary{TKey,TValue}"/> of options where the key is the
        ///  <see cref="StatusControl.Label"/> and the value is the <see cref="StatusControl.TargetValue"/>
        /// </param>
        /// <param name="location">The location of the control in the grid. See <see cref="ControlLocation"/></param>
        /// <param name="controlUse">The specific use for this control. See <see cref="EControlUse"/>. This is applied to all options.</param>
        /// <returns>
        /// The calling <see cref="FeatureFactory"/> with <see cref="StatusControl"/>s with a
        ///  <see cref="StatusControl.ControlType"/> of <see cref="EControlType.TextSelectList"/> added
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when no <paramref name="textOptions"/> are specified.</exception>
        /// <exception cref="ArgumentException">Thrown when a control, targeting any of the specified values, already exists.</exception>
        /// <seealso cref="EControlType.TextSelectList"/>
        public FeatureFactory AddTextDropDown(SortedDictionary<string, double> textOptions, ControlLocation location = null, EControlUse controlUse = EControlUse.NotSpecified) {

            if (textOptions == null) {
                throw new ArgumentNullException(nameof(textOptions));
            }

            foreach (var textOption in textOptions) {
                
                if (_feature.HasControlForValue(textOption.Value)) {
                    throw new ArgumentException($"The value {textOption.Value} already has a control bound to it.", nameof(textOptions));
                }

                //TODO drop down default location
                var dropDownOption = new StatusControl(EControlType.TextSelectList)
                                     {
                                         TargetValue = textOption.Value,
                                         Label       = textOption.Key,
                                         ControlUse  = controlUse,
                                         Location    = location ?? new ControlLocation()
                                     };
                _feature.AddStatusControl(dropDownOption);
            }
            
            return this;
        }
        
        //TODO drop down builder
        
        /// <summary>
        /// Add a set of radio input controls to the <see cref="HsFeature"/>
        /// </summary>
        /// <param name="textOptions">
        /// A <see cref="SortedDictionary{TKey,TValue}"/> of options where the key is the
        ///  <see cref="StatusControl.Label"/> and the value is the <see cref="StatusControl.TargetValue"/>
        /// </param>
        /// <param name="location">The location of the control in the grid. See <see cref="ControlLocation"/></param>
        /// <param name="controlUse">The specific use for this control. See <see cref="EControlUse"/></param>
        /// <returns>
        /// The calling <see cref="FeatureFactory"/> with <see cref="StatusControl"/>s with a
        ///  <see cref="StatusControl.ControlType"/> of <see cref="EControlType.RadioOption"/> added
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when no <paramref name="textOptions"/> are specified.</exception>
        /// <exception cref="ArgumentException">Thrown when a control, targeting the specified value, already exists.</exception>
        /// <seealso cref="EControlType.RadioOption"/>
        public FeatureFactory AddRadioSelectList(SortedDictionary<string, double> textOptions, ControlLocation location = null, EControlUse controlUse = EControlUse.NotSpecified) {
            
            if (textOptions == null) {
                throw new ArgumentNullException(nameof(textOptions));
            }

            foreach (var textOption in textOptions) {
                
                if (_feature.HasControlForValue(textOption.Value)) {
                    throw new ArgumentException($"The value {textOption.Value} already has a control bound to it.", nameof(textOptions));
                }

                //TODO radio select list default location
                var dropDownOption = new StatusControl(EControlType.RadioOption)
                                     {
                                         TargetValue = textOption.Value,
                                         Label       = textOption.Key,
                                         ControlUse  = controlUse,
                                         Location    = location ?? new ControlLocation()
                                     };
                _feature.AddStatusControl(dropDownOption);
            }
            
            return this;
        }
        
        /// <summary>
        /// Deprecated
        /// </summary>
        /// <param name="location">The location of the control in the grid</param>
        /// <param name="controlUse">The specific use for this control</param>
        /// <returns>A FeatureFactory with the new color control added</returns>
        /// <exception cref="ArgumentException">Thrown when a control, targeting the specified value, already exists</exception>
        [Obsolete("This signature has been deprecated. Use one of the new signatures.", true)]
        public FeatureFactory AddColorPicker(ControlLocation location = null, EControlUse controlUse = EControlUse.NotSpecified) {
            
            var targetRange = new ValueRange(0,16777215);
            
            if (_feature.HasControlForRange(targetRange)) {
                throw new ArgumentException($"Some or all of the values in the range {targetRange.Min}-{targetRange.Max} already has a control bound to it.", nameof(targetRange));
            }
            
            var colorPicker = new StatusControl(EControlType.ColorPicker)
                         {
                             TargetRange = targetRange, 
                             ControlUse  = controlUse,
                             Location    = location ?? new ControlLocation()
                         };

            _feature.AddStatusControl(colorPicker);
            
            return this;
        }
        
        /// <summary>
        /// Add a color picker control to the feature
        /// </summary>
        /// <remarks>
        /// Color pickers do not use the value of the feature to operate. They use a control string;
        ///  so the <paramref name="targetValue"/> is superficial and does not correspond to the actual selected color.
        ///  A unique <paramref name="targetValue"/> must still be specified.
        /// </remarks>
        /// <param name="targetValue">The value this control occupies on the feature.</param>
        /// <param name="location">The location of the control in the grid</param>
        /// <param name="controlUse">The specific use for this control</param>
        /// <returns>A FeatureFactory with the new color control added</returns>
        /// <exception cref="ArgumentException">Thrown when a control, targeting the specified value, already exists</exception>
        /// <seealso cref="EControlType.ColorPicker"/>
        public FeatureFactory AddColorPicker(double targetValue, ControlLocation location = null, EControlUse controlUse = EControlUse.NotSpecified) {
            
            if (_feature.HasControlForValue(targetValue)) {
                throw new ArgumentException($"The value {targetValue} already has a control bound to it.", nameof(targetValue));
            }
            
            var colorPicker = new StatusControl(EControlType.ColorPicker)
                              {
                                  TargetValue = targetValue, 
                                  ControlUse  = controlUse,
                                  Location    = location ?? new ControlLocation()
                              };

            _feature.AddStatusControl(colorPicker);
            
            return this;
        }
        
        /// <summary>
        /// Add a color picker control to the feature
        /// </summary>
        /// <remarks>
        /// Color pickers do not use the value of the feature to operate. They use a control string;
        ///  so the <paramref name="targetRange"/> is superficial and does not correspond to the actual selected color.
        /// </remarks>
        /// <param name="targetRange">The values this control occupies on the feature.</param>
        /// <param name="location">The location of the control in the grid</param>
        /// <param name="controlUse">The specific use for this control</param>
        /// <returns>A FeatureFactory with the new color control added</returns>
        /// <exception cref="ArgumentException">Thrown when a control, targeting the specified value, already exists</exception>
        /// <seealso cref="EControlType.ColorPicker"/>
        public FeatureFactory AddColorPicker(ValueRange targetRange, ControlLocation location = null, EControlUse controlUse = EControlUse.NotSpecified) {
            
            if (_feature.HasControlForRange(targetRange)) {
                throw new ArgumentException($"Some or all of the values in the range {targetRange.Min}-{targetRange.Max} already has a control bound to it.", nameof(targetRange));
            }
            
            var colorPicker = new StatusControl(EControlType.ColorPicker)
                              {
                                  TargetRange = targetRange, 
                                  IsRange = true,
                                  ControlUse  = controlUse,
                                  Location    = location ?? new ControlLocation()
                              };

            _feature.AddStatusControl(colorPicker);
            
            return this;
        }
        
        #endregion
        
        #region Status Graphics
        
        //Add Status Graphics
        
        /// <summary>
        /// Add a <see cref="StatusGraphic"/> to the <see cref="HsFeature"/> being built
        /// </summary>
        /// <param name="statusGraphic">The <see cref="StatusGraphic"/> to add.</param>
        /// <returns>The calling <see cref="FeatureFactory"/> with an added <see cref="StatusGraphic"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="statusGraphic"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when a <see cref="StatusGraphic"/> for the values targeted by <paramref name="statusGraphic"/> already exists.</exception>
        public FeatureFactory AddGraphic(StatusGraphic statusGraphic) {

            if (statusGraphic == null) {
                throw new ArgumentNullException(nameof(statusGraphic));
            }
            if (statusGraphic.IsRange && _feature.HasGraphicForRange(statusGraphic.TargetRange)) {
                throw new ArgumentException("A value targeted by the specified statusGraphic already has a graphic bound to it.", nameof(statusGraphic));
            }
            if (_feature.HasGraphicForValue(statusGraphic.Value)) {
                throw new ArgumentException("The value targeted by the specified statusGraphic already has a graphic bound to it.", nameof(statusGraphic));
            }
            
            _feature.AddStatusGraphic(statusGraphic);
            
            return this;
        }

		/// <summary>
        /// Add a <see cref="StatusGraphic"/> that targets a single value to the <see cref="HsFeature"/> being built
        /// </summary>
        /// <param name="imagePath">A path to an image file relative to the HomeSeer root directory</param>
        /// <param name="targetValue">The <see cref="StatusGraphic.Value"/> targeted by the <see cref="StatusGraphic"/></param>
        /// <param name="statusText">The text displayed for the <paramref name="targetValue"/>. Default is a blank string.</param>
        /// <returns>The calling <see cref="FeatureFactory"/> with an added <see cref="StatusGraphic"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="imagePath"/> is empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when a <see cref="StatusGraphic"/> for the <paramref name="targetValue"/> already exists.</exception>
		public FeatureFactory AddGraphicForValue(string imagePath, double targetValue, string statusText = "") {

            if (string.IsNullOrWhiteSpace(imagePath)) {
                throw new ArgumentNullException(nameof(imagePath));
            }
            
            if (_feature.HasGraphicForValue(targetValue)) {
                throw new ArgumentException($"The value {targetValue} already has a graphic bound to it.", nameof(targetValue));
            }
            
            var statusGraphic = new StatusGraphic(imagePath, targetValue, statusText);
            _feature.AddStatusGraphic(statusGraphic);
            
            return this;
        }
        
        /// <summary>
        /// Add a <see cref="StatusGraphic"/> that targets a range of values to the <see cref="HsFeature"/> being built
        /// </summary>
        /// <param name="imagePath">A path to an image file relative to the HomeSeer root directory</param>
        /// <param name="minValue">The minimum value handled by the <see cref="StatusGraphic"/>.</param>
        /// <param name="maxValue">The maximum value handled by the <see cref="StatusGraphic"/></param>
        /// <param name="statusText">The text displayed by the <see cref="StatusGraphic"/>. Default is a blank string.</param>
        /// <returns>The calling <see cref="FeatureFactory"/> with an added <see cref="StatusGraphic"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="imagePath"/> is empty or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when a <see cref="StatusGraphic"/> for a value between <paramref name="minValue"/> and <paramref name="maxValue"/> already exists.</exception>
        public FeatureFactory AddGraphicForRange(string imagePath, double minValue, double maxValue, string statusText = "") {

            if (string.IsNullOrWhiteSpace(imagePath)) {
                throw new ArgumentNullException(nameof(imagePath));
            }
            
            var tempRange = new ValueRange(minValue, maxValue);
            if (_feature.HasGraphicForRange(tempRange)) {
                throw new ArgumentException($"Some or all of the values in the range {tempRange.Min}-{tempRange.Max} already has a control bound to it.");
            }

            var statusGraphic = new StatusGraphic(imagePath, minValue, maxValue) {Label = statusText};
            _feature.AddStatusGraphic(statusGraphic);
            
            return this;
        }

        #endregion
        
        /// <summary>
        /// Prepare the <see cref="FeatureFactory"/> to be sent to HomeSeer and added to a specific device.
        /// </summary>
        /// <param name="devRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="HsDevice"/> the
        ///  <see cref="HsFeature"/> is being added to</param>
        /// <returns><see cref="NewFeatureData"/> ready to be sent to HomeSeer via <see cref="IHsController.CreateFeatureForDevice"/></returns>
        /// <remarks>
        /// You can either use this to assign the feature to a device as the last step before creation, or you can call
        ///  <see cref="OnDevice"/> to set the owning device and then <see cref="PrepareForHs"/> when you are
        ///  ready to send it to HomeSeer.
        /// </remarks>
        /// <seealso cref="IHsController.CreateFeatureForDevice"/>
        public NewFeatureData PrepareForHsDevice(int devRef) {
            return new NewFeatureData(devRef, _feature);
        }
        
        /// <summary>
        /// Prepare the <see cref="FeatureFactory"/> to be sent to HomeSeer for creation.
        /// </summary>
        /// <returns><see cref="NewFeatureData"/> ready to be sent to HomeSeer via <see cref="IHsController.CreateFeatureForDevice"/></returns>
        /// <remarks>
        /// Make sure the <see cref="HsFeature"/> is associated with a device by calling <see cref="OnDevice"/> to
        ///  set the owning device prior to calling this method.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when the <see cref="HsFeature"/> isn't correctly associated with a device.</exception>
        /// <seealso cref="IHsController.CreateFeatureForDevice"/>
        public NewFeatureData PrepareForHs() {
            if (_feature.AssociatedDevices.Count == 0) {
                throw new InvalidOperationException("This feature is not associated with any devices. Associate this with a device by calling OnDevice() first or use PrepareForHsDevice() instead.");
            }
            if (_feature.AssociatedDevices.Count > 1) {
                throw new InvalidOperationException("This feature has too many associations. Features can only have 1 association. Associate this with a device by calling OnDevice() first or use PrepareForHsDevice() instead.");
            }
            return new NewFeatureData(_feature);
        }
        
    }

}