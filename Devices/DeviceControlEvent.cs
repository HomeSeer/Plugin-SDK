using System;
using System.Collections.Generic;
using HomeSeer.PluginSdk.Devices;

namespace HomeSeer.PluginSdk.Devices {

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class DeviceControlEvent {
        
        private string _label = "";
        private EControlUse _controlUse = EControlUse.NotSpecified;
        private EControlType _controlType;
        private double _controlValue;
        private string _controlString = "";
        private int _deviceRef;
        private int _index = -1;
        
        public int CCIndex {
            get => _index;
            set => _index = value;
        }
        
        public int DeviceRef {
            get => _deviceRef;
            set => _deviceRef = value;
        }

        public string Label {
            get => _label;
            set => _label = value;
        }
        
        public EControlType ControlType {
            get => _controlType;
            set => _controlType = value;
        }
        
        public EControlUse ControlUse {
            get => _controlUse;
            set => _controlUse = value;
        }
        public double ControlValue {
            get => _controlValue;
            set => _controlValue = value;
        }
        public string ControlString {
            get => _controlString;
            set {
                if (value == null) {
                    _controlString = "";
                    return;
                }
                _controlString = value;
            }
        }
    
        public DeviceControlEvent(int devRef) {
            _deviceRef = devRef;
        }
        
    }

}