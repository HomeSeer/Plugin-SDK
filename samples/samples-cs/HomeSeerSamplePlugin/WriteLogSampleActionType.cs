using System.Collections.Generic;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk.Events;
using HomeSeer.PluginSdk.Logging;

namespace HSPI_HomeSeerSamplePlugin {

    /// <summary>
    /// A sample event action type that writes a message to the HomeSeer log
    /// </summary>
    public class WriteLogSampleActionType : AbstractActionType {

        /// <summary>
        /// The name of this action in the list of available actions on the event page
        /// </summary>
        private const string ActionName = "Sample Plugin Action - Write to Log";

        /// <summary>
        /// The ID of the instructions label view
        /// </summary>
        private string InstructionsLabelId => $"{PageId}-instructlabel";
        /// <summary>
        /// The value of the instructions label view
        /// </summary>
        private const string InstructionsLabelValue = "Write a message to the log with a type of...";
        /// <summary>
        /// The ID of the log type select list view
        /// </summary>
        private string LogTypeSelectListId => $"{PageId}-logtypesl";
        /// <summary>
        /// The ID of the log message input view
        /// </summary>
        private string LogMessageInputId => $"{PageId}-messageinput";
        /// <summary>
        /// The available log types to select from
        /// </summary>
        private readonly List<string> _logTypeOptions = new List<string>
                                                 {
                                                     "Trace",
                                                     "Debug",
                                                     "Info",
                                                     "Warning",
                                                     "Error"
                                                 };

        /// <summary>
        /// The interface bridge to HSPI that enables proper encapsulation of access to the HomeSeer System
        /// </summary>
        private IWriteLogActionListener Listener => ActionListener as IWriteLogActionListener;

        /// <inheritdoc />
        /// <remarks>
        /// All action types must implement this constructor
        /// </remarks>
        public WriteLogSampleActionType(int id, int eventRef, byte[] dataIn, ActionTypeCollection.IActionTypeListener listener, bool logDebug = false) : base(id, eventRef, dataIn, listener, logDebug) { }
        /// <inheritdoc />
        /// <remarks>
        /// All trigger types must implement this constructor
        /// </remarks>
        public WriteLogSampleActionType() { }
        
        /// <inheritdoc />
        /// <remarks>
        /// Return the name of the action type
        /// </remarks>
        protected override string GetName() {
            return ActionName;
        }

        /// <inheritdoc />
        /// <remarks>
        /// This action type always starts with a single select list.
        /// </remarks>
        protected override void OnNewAction() {
            var confPage = InitNewConfigPage();
            ConfigPage = confPage.Page;
        }

        /// <inheritdoc />
        /// <remarks>
        /// This action type is only fully configured once a message is specified in the input view
        /// </remarks>
        public override bool IsFullyConfigured() {
            switch (ConfigPage.ViewCount) {
                case 3: 
                    var inputView = ConfigPage.GetViewById(LogMessageInputId) as InputView;
                    return (inputView?.Value?.Length ?? 0) > 0;
                default:
                    return false;
            }
        }
        
        /// <inheritdoc />
        /// <remarks>
        /// This is where we validate data entry and update the <see cref="AbstractActionType.ConfigPage"/>
        ///  so that it represents the next state it should be in for configuration.
        /// </remarks>
        protected override bool OnConfigItemUpdate(AbstractView configViewChange) {
            
            if (configViewChange.Id != LogTypeSelectListId) {
                //When the ID being changed is not the log type select list, always save and continue.
                // No more configuration is needed
                return true;
            }
            
            //Log Type selection change
            //Make sure the change is to a select list view
            if (!(configViewChange is SelectListView changedLogTypeSl)) {
                return false;
            }

            //Make sure the target select list view casts correctly
            if (!(ConfigPage.GetViewById(LogTypeSelectListId) is SelectListView currentLogTypeSl)) {
                return false;
            }
                
            if (currentLogTypeSl.Selection == changedLogTypeSl.Selection) {
                //If the selection didn't change then return false because the user may still need to supply a message
                return false;
            }

            //Initialize the new state of the page so it asks for a message
            var newConfPage = InitConfigPageWithInput();
            ConfigPage = newConfPage.Page;

            //Save the change to the log type select list
            return true;
        }

        /// <inheritdoc />
        public override string GetPrettyString() {
            var selectList = ConfigPage.GetViewById(LogTypeSelectListId) as SelectListView;
            var message = ConfigPage?.GetViewById(LogMessageInputId)?.GetStringValue() ?? "Error retrieving log message";
            return $"write the message \"{message}\" to the log with the type of {selectList?.GetSelectedOption() ?? "Unknown Selection"}";
        }

        /// <inheritdoc />
        /// <remarks>
        /// This will call to HSPI through the <see cref="IWriteLogActionListener"/> interface to write a log message
        /// </remarks>
        public override bool OnRunAction() {
            var iLogType = (ConfigPage?.GetViewById(LogTypeSelectListId) as SelectListView)?.Selection ?? 0;
            ELogType logType;
            switch (iLogType) {
                case 0:
                    logType = ELogType.Trace;
                    break;
                case 1:
                    logType = ELogType.Debug;
                    break;
                case 2:
                    logType = ELogType.Info;
                    break;
                case 3:
                    logType = ELogType.Warning;
                    break;
                case 4:
                    logType = ELogType.Error;
                    break;
                default:
                    logType = ELogType.Info;
                    break;
                    
            }

            var message = ConfigPage?.GetViewById(LogMessageInputId)?.GetStringValue() ?? "Error retrieving log message";
            Listener?.WriteLog(logType, message);
            return true;
        }

        /// <inheritdoc />
        /// <remarks>
        /// This action type does not do anything with devices/features; so we should always return false here.
        /// </remarks>
        public override bool ReferencesDeviceOrFeature(int devOrFeatRef) {
            return false;
        }

        /// <summary>
        /// Initialize a new ConfigPage for initial setup of the action where the user must select the log type.
        /// </summary>
        /// <returns>A <see cref="PageFactory"/> representing the new ConfigPage</returns>
        private PageFactory InitNewConfigPage() {
            var confPage = PageFactory.CreateEventActionPage(PageId, ActionName);
            confPage.WithLabel(InstructionsLabelId, null, InstructionsLabelValue);
            confPage.WithDropDownSelectList(LogTypeSelectListId, "Log Type", _logTypeOptions);
            return confPage;
        }
        
        /// <summary>
        /// Initialize a new ConfigPage so the user can supply a message to write to the log
        /// </summary>
        /// <returns>A <see cref="PageFactory"/> representing the new ConfigPage</returns>
        private PageFactory InitConfigPageWithInput() {
            var confPage = InitNewConfigPage();
            confPage.WithInput(LogMessageInputId, "Message");
            return confPage;
        }
        
        /// <summary>
        /// An interface bridge to HSPI that enables proper encapsulation of access to the HomeSeer System
        /// </summary>
        public interface IWriteLogActionListener : ActionTypeCollection.IActionTypeListener {

            /// <summary>
            /// Write a log message. Called by <see cref="WriteLogSampleActionType"/>.
            /// </summary>
            /// <param name="logType">The <see cref="ELogType"/> of the message.</param>
            /// <param name="message">The message to write to the log.</param>
            void WriteLog(ELogType logType, string message);

        }

    }

}