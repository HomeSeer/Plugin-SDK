namespace HomeSeer.Jui.Types {

	/// <summary>
	/// A page's type determines where it is published in HomeSeer and what kinds of tasks it is used for
	/// </summary>
	public enum EPageType {

		/// <summary>
		/// A settings page is used to configure the way the plug-in behaves
		/// </summary>
		Settings = 0,
		// <summary>
		// A feature page is a page of content defined by a plug-in that introduces new
		// functionality to the HomeSeer system and uses HS-JUI for the UI
		// </summary>
		//Feature = 1,
		// <summary>
		// An HTML feature page is a page of content defined by a plug-in that introduces new
		// functionality to the HomeSeer system and uses HTML for the UI
		// </summary>
		//FeatureHtml = 2,
		// <summary>
		// A guided process, or step-by-step guide, is a page that will walk the user through a
		// set of instructions in order to complete a particular task
		// </summary>
		//Guide = 3,
		// <summary>
		// A device inclusion process is a guided process that is specifically used to
		// configure new devices to work with the HomeSeer software
		// </summary>
		//DeviceInclude = 4,
		/// <summary>
		/// These pages can be used to include customization options and procedures that can
		/// be executed by the user for a specific device
		/// </summary>
		DeviceConfig = 5,
		/// <summary>
		/// These pages can be used for whatever you would like if you want to use JUI to
		/// generate MDBootstrap compatible HTML elements
		/// </summary>
		Generic = 6
	}

}