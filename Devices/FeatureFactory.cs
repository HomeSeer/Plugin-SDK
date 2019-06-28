using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeSeer.PluginSdk.Devices {

    public class FeatureFactory {
        
        internal HsDevice Feature => _feature;

        private HsDevice _feature;

        public static FeatureFactory CreateFeature(string pluginId) {
            var ff = new FeatureFactory();
            var feature = new HsDevice
                          {
                              Relationship = ERelationship.Feature,
                              Interface    = pluginId
                          };
            ff._feature = feature;

            return ff;
        }
        
        #region Device Properties
        
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
        
        public FeatureFactory WithMiscFlags(params EDeviceMiscFlag[] miscFlags) {

            if (miscFlags == null || miscFlags.Length == 0) {
                throw new ArgumentNullException(nameof(miscFlags));
            }

            foreach (var miscFlag in miscFlags.Distinct()) {
                _feature.AddMiscFlag(miscFlag);
            }
            
            return this;
        }
        
        #endregion
        
        public FeatureFactory AsType(EFeatureType featureType, int featureSubType) {

            _feature.DeviceType = new DeviceTypeInfo()
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

        public FeatureFactory AddGraphicForValue(string imagePath, double targetValue) {

            if (string.IsNullOrWhiteSpace(imagePath)) {
                throw new ArgumentNullException(nameof(imagePath));
            }
            
            if (_feature.HasGraphicForValue(targetValue)) {
                throw new ArgumentException($"The value {targetValue} already has a graphic bound to it.", nameof(targetValue));
            }
            
            var statusGraphic = new StatusGraphic(imagePath, targetValue);
            _feature.AddStatusGraphic(statusGraphic);
            
            return this;
        }
        
        public FeatureFactory AddGraphicForRange(string imagePath, double minValue, double maxValue) {

            if (string.IsNullOrWhiteSpace(imagePath)) {
                throw new ArgumentNullException(nameof(imagePath));
            }
            
            var tempRange = new ValueRange(minValue, maxValue);

            if (_feature.HasGraphicForRange(tempRange)) {
                throw new ArgumentException($"Some or all of the values in the range {tempRange.Min}-{tempRange.Max} already has a control bound to it.");
            }
            
            var statusGraphic = new StatusGraphic(imagePath, minValue, maxValue);
            _feature.AddStatusGraphic(statusGraphic);
            
            return this;
        }
        
        //Remove status graphics?
        
        #endregion
        
    }

}