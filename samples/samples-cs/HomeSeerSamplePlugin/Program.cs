namespace HSPI_HomeSeerSamplePlugin {

	/// <summary>
	/// The console application for the sample plugin
	/// </summary>
	internal static class Program {

		/// <summary>
		/// The instance of the plugin class
		/// </summary>
		public static HSPI _plugin;

		public static void Main(string[] args) {
			
			//Create a new instance of the plugin class
			_plugin = new HSPI();
			
			//Perform any initialization that needs to occur before a connection is made to HomeSeer
			
			//Attempt to connect to HomeSeer
			_plugin.Connect(args);
		}

	}

}