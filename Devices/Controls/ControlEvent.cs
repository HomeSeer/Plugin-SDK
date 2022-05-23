using System;

namespace HomeSeer.PluginSdk.Devices.Controls {

    /// <summary>
    /// The description of an event representing a controlled change of a <see cref="HsFeature"/>.
    /// </summary>
    /// <seealso cref="StatusControl"/>
    /// <seealso cref="HsFeature.CreateControlEvent"/>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Serializable]
    public class ControlEvent {
        
        private string _label = "";
        private EControlUse _controlUse = EControlUse.NotSpecified;
        private EControlType _controlType;
        private double _controlValue;
        private string _controlString = "";
        private int _ref;
        private int _index = -1;
        
        /// <summary>
        /// A unique index representing the CAPIControl in HomeSeer.
        /// </summary>
        /// <remarks>This is a carry over from the legacy API. It should not need to be used anymore and is maintained for compatibility.</remarks>
        [Obsolete("This is a carry over from the legacy API. It should not need to be used anymore and is maintained for compatibility.", false)]
        public int CCIndex {
            get => _index;
            set => _index = value;
        }
        
        /// <summary>
        /// The <see cref="AbstractHsDevice.Ref"/> of the <see cref="HsFeature"/> being controlled.
        /// </summary>
        public int TargetRef {
            get => _ref;
            set => _ref = value;
        }

        /// <summary>
        /// A human-readable string representing the label to display on the <see cref="HsFeature"/> for this event.
        /// </summary>
        public string Label {
            get => _label;
            set => _label = value;
        }
        
        /// <summary>
        /// The <see cref="EControlType"/> of the <see cref="StatusControl"/> used to generate this event.
        /// </summary>
        public EControlType ControlType {
            get => _controlType;
            set => _controlType = value;
        }
        
        /// <summary>
        /// The <see cref="EControlUse"/> of the <see cref="StatusControl"/> used to generate this event.
        /// </summary>
        public EControlUse ControlUse {
            get => _controlUse;
            set => _controlUse = value;
        }
        
        /// <summary>
        /// The target value for the event. This is the desired <see cref="AbstractHsDevice.Value"/>
        /// </summary>
        public double ControlValue {
            get => _controlValue;
            set => _controlValue = value;
        }
        
        /// <summary>
        /// A string containing control data for the event. This is used for <see cref="EControlType.ColorPicker"/>s
        /// </summary>
        /// <remarks>
        /// This is NOT the <see cref="Label"/> to display in the <see cref="AbstractHsDevice.DisplayedStatus"/> of the <see cref="HsFeature"/>. This is not displayed anywhere.
        /// </remarks>
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
    
        /// <summary>
        /// Create a new <see cref="ControlEvent"/> for a <see cref="AbstractHsDevice.Ref"/>
        /// </summary>
        /// <param name="devRef">The <see cref="AbstractHsDevice.Ref"/> of the <see cref="HsFeature"/> being controlled</param>
        public ControlEvent(int devRef) {
            _ref = devRef;
        }
        
    }

}