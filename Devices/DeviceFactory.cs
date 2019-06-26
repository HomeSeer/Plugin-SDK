using System;
using System.Collections.Generic;

namespace HomeSeer.PluginSdk.Devices {

    public class DeviceFactory {

        public bool IsFeature => _device?.Relationship == ERelationship.Child;

        private HsDevice _device;
        private List<HsDevice> _features;
        
        #region Create
        
        //Create
        public static DeviceFactory CreateDevice() {
            var df = new DeviceFactory();
            var device = new HsDevice
                         {
                             Relationship = ERelationship.Standalone,
                         };
            df._device = device;

            return df;
        }
        
        public static DeviceFactory CreateFeature() {
            var df = new DeviceFactory();
            var feature = new HsDevice
                         {
                             Relationship = ERelationship.Child,
                         };
            df._device = feature;

            return df;
        }
        
        #endregion
        
        //Configure Device/Feature
        //Add Features
        public DeviceFactory AddFeature(DeviceFactory feature) {

            if (feature?._device == null) {
                throw new ArgumentNullException(nameof(feature));
            }

            if (feature.IsFeature) {
                throw new ArgumentException("Cannot add a device to a device.  Try adding a feature instead.");
            }

            if (_features == null) {
                _features = new List<HsDevice>();
            }
            
            _features.Add(feature._device);

            return this;
        }
        
        #region Controls
        
        //Add Controls

        public DeviceFactory AddButton(double targetValue, string targetStatus, EControlUse controlUse = EControlUse.NotSpecified) {

            if (_device.Relationship != ERelationship.Child && _device.Relationship != ERelationship.Standalone) {
                throw new Exception("You cannot add controls to a device with features.  They must be added to a feature instead.");
            }

            if (string.IsNullOrWhiteSpace(targetStatus)) {
                throw new ArgumentNullException(nameof(targetStatus));
            }
            
            //TODO control location

            if (_device.HasControlForValue(targetValue)) {
                throw new ArgumentException("This value already has a control bound to it.", nameof(targetValue));
            }

            var button = new StatusControl(EControlType.Button)
                         {
                             TargetValue = targetValue, 
                             Label = targetStatus, 
                             ControlUse = controlUse
                         };

            _device.AddStatusControl(button);
            
            return this;
        }
        
        public DeviceFactory AddTextInputField(double targetValue, string hintText, EControlUse controlUse = EControlUse.NotSpecified) {

            if (_device.Relationship != ERelationship.Child && _device.Relationship != ERelationship.Standalone) {
                throw new Exception("You cannot add controls to a device with features.  They must be added to a feature instead.");
            }

            if (string.IsNullOrWhiteSpace(hintText)) {
                throw new ArgumentNullException(nameof(hintText));
            }
            
            //TODO control location

            if (_device.HasControlForValue(targetValue)) {
                throw new ArgumentException("This value already has a control bound to it.", nameof(targetValue));
            }

            var textInput = new StatusControl(EControlType.TextBoxString)
                         {
                             TargetValue = targetValue, 
                             Label       = hintText, 
                             ControlUse  = controlUse
                         };

            _device.AddStatusControl(textInput);
            
            return this;
        }
        
        public DeviceFactory AddNumberInputField(double targetValue, string hintText, EControlUse controlUse = EControlUse.NotSpecified) {

            if (_device.Relationship != ERelationship.Child && _device.Relationship != ERelationship.Standalone) {
                throw new Exception("You cannot add controls to a device with features.  They must be added to a feature instead.");
            }

            if (string.IsNullOrWhiteSpace(hintText)) {
                throw new ArgumentNullException(nameof(hintText));
            }
            
            //TODO control location

            if (_device.HasControlForValue(targetValue)) {
                throw new ArgumentException("This value already has a control bound to it.", nameof(targetValue));
            }

            var numInput = new StatusControl(EControlType.TextBoxNumber)
                            {
                                TargetValue = targetValue, 
                                Label       = hintText, 
                                ControlUse  = controlUse
                            };

            _device.AddStatusControl(numInput);
            
            return this;
        }
        
        public DeviceFactory AddSlider(ValueRange targetRange, EControlUse controlUse = EControlUse.NotSpecified) {

            if (_device.Relationship != ERelationship.Child && _device.Relationship != ERelationship.Standalone) {
                throw new Exception("You cannot add controls to a device with features.  They must be added to a feature instead.");
            }

            if (targetRange == null) {
                throw new ArgumentNullException(nameof(targetRange));
            }
            
            if (_device.HasControlForRange(targetRange)) {
                throw new ArgumentException("This value range already has a control bound to it.", nameof(targetRange));
            }
            
            //TODO control location

            var slider = new StatusControl(EControlType.ValueRangeSlider)
                           {
                               TargetRange = targetRange, 
                               ControlUse  = controlUse
                           };

            _device.AddStatusControl(slider);
            
            return this;
        }
        
        public DeviceFactory AddValueDropDown(ValueRange targetRange, EControlUse controlUse = EControlUse.NotSpecified) {

            if (_device.Relationship != ERelationship.Child && _device.Relationship != ERelationship.Standalone) {
                throw new Exception("You cannot add controls to a device with features.  They must be added to a feature instead.");
            }

            if (targetRange == null) {
                throw new ArgumentNullException(nameof(targetRange));
            }
            
            if (_device.HasControlForRange(targetRange)) {
                throw new ArgumentException("This value range already has a control bound to it.", nameof(targetRange));
            }
            
            //TODO control location

            var dropDown = new StatusControl(EControlType.ValueRangeDropDown)
                         {
                             TargetRange = targetRange, 
                             ControlUse  = controlUse
                         };

            _device.AddStatusControl(dropDown);
            
            return this;
        }
        
        //TODO Text Select List
        //TODO Radio Option
        //TODO Color Picker
        //TODO Button Script?
        
        #endregion
        
        #region Status Graphics
        
        //Add Status Graphics

        public DeviceFactory AddGraphicForValue(string imagePath, double targetValue) {

            if (_device.Relationship != ERelationship.Child && _device.Relationship != ERelationship.Standalone) {
                throw new Exception("You cannot add status graphics to a device with features.  They must be added to a feature instead.");
            }

            if (string.IsNullOrWhiteSpace(imagePath)) {
                throw new ArgumentNullException(nameof(imagePath));
            }
            
            if (_device.HasGraphicForValue(targetValue)) {
                throw new ArgumentException("This value already has a graphic bound to it.", nameof(targetValue));
            }
            
            var statusGraphic = new StatusGraphic(imagePath, targetValue);
            _device.AddStatusGraphic(statusGraphic);
            
            return this;
        }
        
        public DeviceFactory AddGraphicForRange(string imagePath, double minValue, double maxValue) {

            if (_device.Relationship != ERelationship.Child && _device.Relationship != ERelationship.Standalone) {
                throw new Exception("You cannot add status graphics to a device with features.  They must be added to a feature instead.");
            }

            if (string.IsNullOrWhiteSpace(imagePath)) {
                throw new ArgumentNullException(nameof(imagePath));
            }
            
            if (_device.HasGraphicForValue(minValue)) {
                throw new ArgumentException("This value already has a graphic bound to it.", nameof(minValue));
            }
            
            if (_device.HasGraphicForValue(maxValue)) {
                throw new ArgumentException("This value already has a graphic bound to it.", nameof(maxValue));
            }
            
            var statusGraphic = new StatusGraphic(imagePath, minValue, maxValue);
            _device.AddStatusGraphic(statusGraphic);
            
            return this;
        }
        
        //Remove status graphics?
        
        #endregion
    }

}