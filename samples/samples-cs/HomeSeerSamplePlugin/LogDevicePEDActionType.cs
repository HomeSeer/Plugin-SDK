using System.Collections.Generic;
using System.Linq;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk.Events;
using HomeSeer.PluginSdk.Logging;

namespace HSPI_HomeSeerSamplePlugin {

    /// <summary>
    /// An event action type that writes the Plugin Extra Data of a device to the HomeSeer log
    /// </summary>
    public class LogDevicePEDActionType : AbstractActionType2 {

        /// <summary>
        /// The name of this action in the list of available actions on the event page
        /// </summary>
        private const string ActionName = "Sample Plugin Action - Write Device PED to Log";

        /// <summary>
        /// The ID of the instructions label view
        /// </summary>
        private string InstructionsLabelId => $"{PageId}-instructlabel";
        /// <summary>
        /// The value of the instructions label view
        /// </summary>
        private const string InstructionsLabelValue = "Write Plugin Extra Data to the Log for device...";
        /// <summary>
        /// The ID of the device select list view
        /// </summary>
        private string DeviceSelectListId => $"{PageId}-devicesl";


        /// <summary>
        /// The interface bridge to HSPI that enables proper encapsulation of access to the HomeSeer System
        /// </summary>
        private ILogDevicePEDActionListener Listener => ActionListener as ILogDevicePEDActionListener;

        /// <inheritdoc />
        /// <remarks>
        /// All action types must implement this constructor
        /// </remarks>
        public LogDevicePEDActionType(int id, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener, bool logDebug = false) : base(id, eventRef, dataIn, listener, logDebug) { }
        /// <inheritdoc />
        /// <remarks>
        /// All action types must implement this constructor
        /// </remarks>
        public LogDevicePEDActionType() { }
        
        /// <inheritdoc />
        /// <remarks>
        /// Return the name of the action type
        /// </remarks>
        protected override string GetName() {
            return ActionName;
        }

        /// <inheritdoc />
        /// <remarks>
        /// This action type contains a label and a select list
        /// </remarks>
        protected override void OnInstantiateAction(Dictionary<string, string> viewIdValuePairs) {
            var confPage = PageFactory.CreateEventActionPage(PageId, ActionName);
            var deviceNames = Listener.GetAllDeviceNames().OrderBy(key => key.Value);
            var options = new List<string>();
            var optionKeys = new List<string>();
            int selectedRef = -1;
            int selectionIndex = -1;

            if(viewIdValuePairs?.ContainsKey(DeviceSelectListId) ?? false) {
                int.TryParse(viewIdValuePairs[DeviceSelectListId], out selectedRef);
            }

            int i = 0;
            foreach (var d in deviceNames) {
                options.Add(d.Value);
                optionKeys.Add(d.Key.ToString());
                if(selectedRef == d.Key) {
                    selectionIndex = i;
                }
                i++;
            }
            var deviceSL = new SelectListView(DeviceSelectListId, "Device", options, optionKeys, HomeSeer.Jui.Types.ESelectListType.SearchableDropDown, selectionIndex);
            //For viewIdValuePairs to contains the selected device ref rather than the selection index, UseOptionKeyAsSelectionValue needs to be true
            deviceSL.UseOptionKeyAsSelectionValue = true;

            confPage.WithLabel(InstructionsLabelId, null, InstructionsLabelValue);
            confPage.WithView(deviceSL);
            
            ConfigPage = confPage.Page;
        }

        /// <inheritdoc />
        /// <remarks>
        /// This action type is fully configured if a device is selected 
        /// </remarks>
        public override bool IsFullyConfigured() {
            var deviceSL = ConfigPage?.GetViewById(DeviceSelectListId) as SelectListView;
            if (deviceSL != null && !string.IsNullOrEmpty(deviceSL.GetSelectedOptionKey())) {
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Because all of the available configuration options are select lists, no data validation is needed.
        ///  This means that we can always return true here so that all changes are saved.
        /// </remarks>
        protected override bool OnConfigItemUpdate(AbstractView configViewChange) {
            return true;
        }

        /// <inheritdoc />
        public override string GetPrettyString() {
            var deviceSL = ConfigPage?.GetViewById(DeviceSelectListId) as SelectListView;
            var selectedDevice = deviceSL?.GetSelectedOption();
            return $"Write Plugin Extra Data to the log for {selectedDevice ?? "Unknown Device"}";
        }

        /// <inheritdoc />
        /// <remarks>
        /// This will call to HSPI through the <see cref="ILogDevicePEDActionListener"/> interface to write the PED to the log
        /// </remarks>
        public override bool OnRunAction() {
            var deviceSL = ConfigPage?.GetViewById(DeviceSelectListId) as SelectListView;
            var selectedDeviceRef = deviceSL?.GetSelectedOptionKey();
            int deviceRef;

            if (int.TryParse(selectedDeviceRef, out deviceRef)) {
                Listener.WriteDevicePEDToLog(deviceRef);
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public override bool ReferencesDeviceOrFeature(int devOrFeatRef) {
            var deviceSL = ConfigPage?.GetViewById(DeviceSelectListId) as SelectListView;
            var selectedDeviceRef = deviceSL?.GetSelectedOptionKey();

            return selectedDeviceRef == devOrFeatRef.ToString();
        }
        
        /// <summary>
        /// An interface bridge to HSPI that enables proper encapsulation of access to the HomeSeer System
        /// </summary>
        public interface ILogDevicePEDActionListener : ActionTypeCollection.IActionTypeListener {

            /// <summary>
            /// Write the device Plugin Extra Data to the HomeSeer Log
            /// </summary>
            /// <param name="deviceRef">The reference of the device</param>
            void WriteDevicePEDToLog(int deviceRef);

            /// <summary>
            /// Get all the device names in the system
            /// </summary>
            /// <returns>A collection of device ref - device name pairs</returns>
            Dictionary<int, string> GetAllDeviceNames();

        }

    }

}