using System;
using System.Collections.Generic;
using System.Linq;
using HomeSeer.PluginSdk.Devices.Controls;
using HomeSeer.PluginSdk.Devices.Identification;

namespace HomeSeer.PluginSdk.Devices {

    public class FeatureFactory {
        
        internal HsFeature Feature => _feature;

        private HsFeature _feature;

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
                         new ControlLocation(1,1,1),
                         EControlUse.Off);
            ff.AddButton(onValue,
                         onText,
                         new ControlLocation(1,2,1),
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

        public FeatureFactory OnDevice(int devRef) {

            if (devRef <= 0) {
                throw new ArgumentOutOfRangeException(nameof(devRef));
            }
            
            _feature.AssociatedDevices = new HashSet<int> {devRef};

            return this;
        }
        
        public FeatureFactory WithName(string name) {

            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentNullException(nameof(name));
            }

            _feature.Name = name;

            return this;
        }
        
        public FeatureFactory WithExtraData(PlugExtraData extraData) {

            if (extraData == null) {
                throw new ArgumentNullException(nameof(extraData));
            }

            _feature.PlugExtraData = extraData;

            return this;
        }
        
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
        
        #endregion
        
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
        
        //Add Controls

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
        
        public FeatureFactory AddTextDropDown(SortedDictionary<string, double> textOptions, ControlLocation location = null, EControlUse controlUse = EControlUse.NotSpecified) {

            if (textOptions == null) {
                throw new ArgumentNullException(nameof(textOptions));
            }

            foreach (var textOption in textOptions) {
                
                if (_feature.HasControlForValue(textOption.Value)) {
                    throw new ArgumentException($"The value {textOption.Value} already has a control bound to it.", nameof(textOptions));
                }

                //TODO drop down location
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
        
        public FeatureFactory AddRadioSelectList(SortedDictionary<string, double> textOptions, ControlLocation location = null, EControlUse controlUse = EControlUse.NotSpecified) {
            
            if (textOptions == null) {
                throw new ArgumentNullException(nameof(textOptions));
            }

            foreach (var textOption in textOptions) {
                
                if (_feature.HasControlForValue(textOption.Value)) {
                    throw new ArgumentException($"The value {textOption.Value} already has a control bound to it.", nameof(textOptions));
                }

                //TODO radio select list location
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
        
        [Obsolete("This signature has been deprecated. Use one of the new signatures.", false)]
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
        ///  so the <see cref="targetValue"/> is superficial and does not correspond to the actual selected color.
        /// </remarks>
        /// <param name="targetValue">The value this control occupies on the feature.</param>
        /// <param name="location">The location of the control in the grid</param>
        /// <param name="controlUse">The specific use for this control</param>
        /// <returns>A FeatureFactory with the new color control added</returns>
        /// <exception cref="ArgumentException">Thrown when a control, targeting the specified value, already exists</exception>
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
        ///  so the <see cref="targetRange"/> is superficial and does not correspond to the actual selected color.
        /// </remarks>
        /// <param name="targetRange">The values this control occupies on the feature.</param>
        /// <param name="location">The location of the control in the grid</param>
        /// <param name="controlUse">The specific use for this control</param>
        /// <returns>A FeatureFactory with the new color control added</returns>
        /// <exception cref="ArgumentException">Thrown when a control, targeting the specified value, already exists</exception>
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
        
        //TODO Button Script?
        
        #endregion
        
        #region Status Graphics
        
        //Add Status Graphics

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
        
        public FeatureFactory AddGraphicForRange(string imagePath, double minValue, double maxValue, string statusText = "") {

            if (string.IsNullOrWhiteSpace(imagePath)) {
                throw new ArgumentNullException(nameof(imagePath));
            }
            
            var tempRange = new ValueRange(minValue, maxValue);

            if (_feature.HasGraphicForRange(tempRange)) {
                throw new ArgumentException($"Some or all of the values in the range {tempRange.Min}-{tempRange.Max} already has a control bound to it.");
            }
            
            var statusGraphic = new StatusGraphic(imagePath, minValue, maxValue);
            statusGraphic.Label = statusText;
            _feature.AddStatusGraphic(statusGraphic);
            
            return this;
        }
        
        //Remove status graphics?
        
        #endregion
        
        public NewFeatureData PrepareForHsDevice(int devRef) {
            return new NewFeatureData(devRef, _feature);
        }
        
        public NewFeatureData PrepareForHs() {
            return new NewFeatureData(_feature);
        }
        
    }

}