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
            feature.Changes.Add(EProperty.Misc, (uint) EMiscFlag.ShowValues);
            feature.Changes.Add(EProperty.UserAccess, "Any");
            ff._feature = feature;

            return ff;
        }
        
        public static FeatureFactory CreateFeature(string pluginId, int devRef) {
            if (devRef <= 0) {
                throw new ArgumentOutOfRangeException(nameof(devRef));
            }
            
            var ff = new FeatureFactory();
            var feature = new HsFeature
                          {
                              Relationship = ERelationship.Feature,
                              Interface    = pluginId
                          };
            feature.Changes.Add(EProperty.Misc, (uint) EMiscFlag.ShowValues);
            feature.Changes.Add(EProperty.UserAccess, "Any");
            ff._feature = feature;
            ff._feature.AssociatedDevices = new HashSet<int> {devRef};

            return ff;
        }
        
        public static FeatureFactory CreateBinaryControlFeature(string pluginId, string name, 
                                                                string onText, string offText, 
                                                                double onValue = 1, double offValue = 0) {

            var ff = new FeatureFactory();
            var feature = new HsFeature
                          {
                              Name = name,
                              Relationship = ERelationship.Feature,
                              Interface    = pluginId
                          };
            feature.Changes.Add(EProperty.Misc, (uint) EMiscFlag.ShowValues);
            feature.Changes.Add(EProperty.UserAccess, "Any");
            ff._feature = feature;
            ff.AsType(EFeatureType.Generic, (int) EGenericFeatureType.BinaryControl);
            ff.AddButton(offValue,
                         offText,
                         new ControlLocation
                         {
                             Column = 0,
                             Row    = 0,
                             Width  = 1
                         },
                         EControlUse.Off);
            ff.AddButton(onValue,
                         onText,
                         new ControlLocation
                         {
                             Column = 1,
                             Row    = 0,
                             Width  = 1
                         },
                         EControlUse.On);
            ff.AddGraphicForValue("/images/HomeSeer/status/off.gif", offValue, offText);
            ff.AddGraphicForValue("/images/HomeSeer/status/on.gif", onValue, onText);

            return ff;
        }
        
        public static FeatureFactory CreateBinarySensorFeature(string pluginId, string name, 
                                                               string onText, string offText, 
                                                               double onValue = 1, double offValue = 0) {

            var ff = new FeatureFactory();
            var feature = new HsFeature
                          {
                              Name         = name,
                              Relationship = ERelationship.Feature,
                              Interface    = pluginId
                          };
            feature.Changes.Add(EProperty.Misc,  (uint)EMiscFlag.ShowValues);
            feature.Changes.Add(EProperty.UserAccess, "Any");
            ff._feature = feature;
            ff.AsType(EFeatureType.Generic, (int) EGenericFeatureType.BinarySensor);
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
        /// <exception cref="ArgumentNullException">Thrown when in invalid location was specified</exception>
        public FeatureFactory WithLocation(string location) {

            if (string.IsNullOrWhiteSpace(location)) {
                throw new ArgumentNullException(nameof(location));
            }

            _feature.Location = location;

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
        /// <exception cref="ArgumentNullException">Thrown when in invalid location2 was specified</exception>
        public FeatureFactory WithLocation2(string location2) {

            if (string.IsNullOrWhiteSpace(location2)) {
                throw new ArgumentNullException(nameof(location2));
            }

            _feature.Location2 = location2;

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
            
            var statusGraphic = new StatusGraphic(imagePath, targetValue);
            statusGraphic.Label = statusText;
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