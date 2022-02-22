using System;
using System.Collections.Generic;
using System.Linq;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk.Events;

namespace HSPI_HomeSeerSamplePlugin {

    /// <summary>
    /// A sample event trigger type fired from the Trigger Feature page.
    /// </summary>
    public class SampleTriggerType : AbstractTriggerType {

        /// <summary>
        /// The 1 based index of this trigger type. Used for reference in this class.
        /// </summary>
        public const int TriggerNumber = 1;

        /// <summary>
        /// The name of this trigger in the list of available triggers on the event page
        /// </summary>
        private const string TriggerName = "Sample Trigger";
        
        /// <summary>
        /// The ID of the option count select list
        /// </summary>
        private string OptionCountSlId => $"{PageId}-optioncountsl";
        /// <summary>
        /// The name of the option count select list
        /// </summary>
        private const string OptionCountSlName = "Number of Options Checked";
        /// <summary>
        /// The ID of the required option select list
        /// </summary>
        private string OptionNumSlId => $"{PageId}-optionnumsl";
        /// <summary>
        /// The name of the required option select list
        /// </summary>
        private const string OptionNumSlName = "Required Option";

        /// <summary>
        /// Determine whether the trigger should fire based on the specified option selections
        /// </summary>
        /// <param name="triggerOptions">
        /// A boolean array describing the state of the options when the trigger button was clicked
        /// </param>
        /// <returns>
        /// TRUE if the trigger should fire,
        ///  FALSE if it shouldn't
        /// </returns>
        public bool ShouldTriggerFire(params bool[] triggerOptions) {
            switch (SelectedSubTriggerIndex) {
                case 0:
                    var numRequiredOptions = GetSelectedOptionCount() + 1;
                    return numRequiredOptions != 0 && triggerOptions.Count(triggerOption => triggerOption) == numRequiredOptions;
                case 1:
                    var specificRequiredOption = GetSelectedSpecificOptionNum();
                    if (triggerOptions.Length < specificRequiredOption + 1) {
                        return false;
                    }

                    return triggerOptions[specificRequiredOption];
                case 2:
                    return !triggerOptions.Any(triggerOption => triggerOption);
                case 3:
                    return triggerOptions.Any(triggerOption => triggerOption);
                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public SampleTriggerType(TrigActInfo trigInfo, TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug = false) : base(trigInfo, listener, logDebug) { }
        /// <inheritdoc />
        /// <remarks>
        /// All trigger types must implement this constructor
        /// </remarks>
        public SampleTriggerType(int id, int eventRef, int selectedSubTriggerIndex, byte[] dataIn, TriggerTypeCollection.ITriggerTypeListener listener, bool logDebug = false) : base(id, eventRef, selectedSubTriggerIndex, dataIn, listener, logDebug) { }
        /// <inheritdoc />
        /// <remarks>
        /// All trigger types must implement this constructor
        /// </remarks>
        public SampleTriggerType() { }

        /// <inheritdoc />
        /// <remarks>
        /// This trigger type has 4 subtypes.
        /// </remarks>
        protected override List<string> SubTriggerTypeNames { get; set; } = new List<string>
                                                                            {
                                                                                "Button click with X options checked",
                                                                                "Button click with specific option checked",
                                                                                "Button click with no options checked",
                                                                                "Button click with any options checked"
                                                                            };

        /// <inheritdoc />
        /// <remarks>
        /// Return the name of the trigger type
        /// </remarks>
        protected override string GetName() => TriggerName;

        /// <inheritdoc />
        /// <remarks>
        /// This trigger type has 3 states, 2 of which require additional configuration.
        /// </remarks>
        protected override void OnNewTrigger() {
            switch (SelectedSubTriggerIndex) {
                case 0:
                    ConfigPage = InitializeXOptionsPage().Page;
                    break;
                case 1:
                    ConfigPage = InitializeSpecificOptionPage().Page;
                    break;
                default:
                    ConfigPage = InitializeDefaultPage().Page;
                    break;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// This trigger type has 3 states, 2 of which require additional configuration.
        /// </remarks>
        public override bool IsFullyConfigured() {
            switch (SelectedSubTriggerIndex) {
                case 0:
                    //Check to see if the input for the number of options is valid
                    return GetSelectedOptionCount() >= 0;
                case 1:
                    //Check to see if the input for the required option is valid
                    return GetSelectedSpecificOptionNum() >= 0;
                default:
                    //The last two sub trigger types do not require any additional configuration
                    return true;
            }
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
            switch (SelectedSubTriggerIndex) {
                case 0:
                    try {
                        var optionCountSl = ConfigPage?.GetViewById(OptionCountSlId) as SelectListView;
                        return $"the button on the Sample Plugin Trigger Feature page is clicked and {(optionCountSl?.GetSelectedOption() ?? "???")} options are checked";
                    }
                    catch (Exception exception) {
                        if (LogDebug) {
                            Console.WriteLine(exception);
                        }
                        return "the button on the Sample Plugin Trigger Feature page is clicked and ??? options are checked";
                    }
                case 1:
                    try {
                        var optionNumSl = ConfigPage?.GetViewById(OptionNumSlId) as SelectListView;
                        return $"the button on the Sample Plugin Trigger Feature page is clicked and option number {(optionNumSl?.GetSelectedOption() ?? "???")} is checked";
                    }
                    catch (Exception exception) {
                        if (LogDebug) {
                            Console.WriteLine(exception);
                        }
                        return "the button on the Sample Plugin Trigger Feature page is clicked and option number ??? is checked";
                    }
                case 2:
                    return "the button the Sample Plugin Trigger Feature page is clicked and no options are checked";
                default:
                    return "the button the Sample Plugin Trigger Feature page is clicked";
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// This trigger type is never used as a condition; so we can always return true here so manual trigger fires
        ///  are always executed.
        /// </remarks>
        public override bool IsTriggerTrue(bool isCondition) => true;

        /// <inheritdoc />
        /// <remarks>
        /// This trigger type does not do anything with devices/features; so we should always return false here.
        /// </remarks>
        public override bool ReferencesDeviceOrFeature(int devOrFeatRef) => false;

        /// <summary>
        /// Initialize a new <see cref="AbstractTriggerType.ConfigPage"/> for a trigger based on a number of options
        ///  selected.
        /// </summary>
        /// <returns>A <see cref="PageFactory"/> initialized for the first subtype</returns>
        private PageFactory InitializeXOptionsPage() {
            var cpf = InitializeDefaultPage();
            cpf.WithDropDownSelectList(OptionCountSlId, OptionCountSlName, new[] {"1", "2", "3", "4"}.ToList());
            return cpf;
        }
        
        /// <summary>
        /// Initialize a new <see cref="AbstractTriggerType.ConfigPage"/> for a trigger based on a specific option
        ///  being selected.
        /// </summary>
        /// <returns>A <see cref="PageFactory"/> initialized for the second subtype</returns>
        private PageFactory InitializeSpecificOptionPage() {
            var cpf = InitializeDefaultPage();
            cpf.WithDropDownSelectList(OptionNumSlId, OptionNumSlName, new[] {"1", "2", "3", "4"}.ToList());
            return cpf;
        }
        
        /// <summary>
        /// Initialize a new <see cref="AbstractTriggerType.ConfigPage"/> for a trigger with no additional configurations.
        /// </summary>
        /// <returns>A <see cref="PageFactory"/> initialized for the third or fourth subtype</returns>
        private PageFactory InitializeDefaultPage() {
            var cpf = PageFactory.CreateEventTriggerPage(PageId, TriggerName);
            return cpf;
        }

        /// <summary>
        /// Get the currently selected specific option index
        /// </summary>
        /// <returns>The index of the required specific option</returns>
        private int GetSelectedSpecificOptionNum() {
            try {
                var optionNumSl = ConfigPage?.GetViewById(OptionNumSlId) as SelectListView;
                return optionNumSl?.Selection ?? -1;
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return -1;
            }
        }

        /// <summary>
        /// Get the currently selected required option count
        /// </summary>
        /// <returns>The number of options that must be selected</returns>
        private int GetSelectedOptionCount() {
            try {
                var optionCountSl = ConfigPage?.GetViewById(OptionCountSlId) as SelectListView;
                return (optionCountSl?.Selection ?? -1);
            }
            catch (Exception exception) {
                if (LogDebug) {
                    Console.WriteLine(exception);
                }
                return -1;
            }
        }

    }

}