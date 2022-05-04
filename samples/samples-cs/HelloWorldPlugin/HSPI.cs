using System;
using HomeSeer.Jui.Views;
using HomeSeer.PluginSdk;

namespace HSPI_HelloWorldPlugin_CS {
    
    public class HSPI : AbstractPlugin {


        #region Properties

        /// <inheritdoc />
        /// <remarks>
        /// This ID is used to identify the plugin and should be unique across all plugins
        /// <para>
        /// This must match the MSBuild property $(PluginId) as this will be used to copy
        ///  all of the HTML feature pages located in .\html\ to a relative directory
        ///  within the HomeSeer html folder.
        /// </para>
        /// <para>
        /// The relative address for all of the HTML pages will end up looking like this:
        ///  ..\Homeseer\Homeseer\html\HelloWorldPlugin_CS\
        /// </para>
        /// </remarks>
        public override string Id { get; } = "HelloWorldPlugin_CS";
        
        /// <inheritdoc />
        /// <remarks>
        /// This is the readable name for the plugin that is displayed throughout HomeSeer
        /// </remarks>
        public override string Name { get; } = "HelloWorldPlugin-CS";
        
        /// <inheritdoc />
        protected override string SettingsFileName { get; } = "HelloWorldPlugin-CS.ini";

        #endregion

        public HSPI() {
            //Initialize the plugin 

            //Enable internal debug logging to console
            LogDebug = true;
            //Setup anything that needs to be configured before a connection to HomeSeer is established
            // like initializing the starting state of anything needed for the operation of the plugin
        }

        protected override void Initialize() {
            Console.WriteLine("Initialized");
            Status = PluginStatus.Ok();
        }

        /// <inheritdoc />
        /// <remarks>
        /// Required override
        /// </remarks>
        protected override bool OnSettingChange(string pageId, AbstractView currentView, AbstractView changedView) {
            return true;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Required override
        /// </remarks>
        protected override void BeforeReturnStatus() {}

    }

}