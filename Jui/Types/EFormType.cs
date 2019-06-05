namespace HomeSeer.Jui.Types {

	/// <summary>
	/// The type that a form is set to determines what class a client application should deserialize it as.
	/// </summary>
	internal enum EFormType {

		/// <summary>
		/// A form used for communicating information, like instructions, to the user.
		/// See <see cref="HomeSeer.Jui.Forms.InfoFormView"/>
		/// </summary>
		Info = 0,
		/// <summary>
		/// A form used for collecting input from a user.  See <see cref="HomeSeer.Jui.Forms.InputFormView"/>
		/// </summary>
		Input = 1,
		/// <summary>
		/// A form used for indicating to the user that a task is running on the HomeSeer system and they
		/// should wait until it completes before performing any other tasks.
		/// See <see cref="HomeSeer.Jui.Forms.ProgressFormView"/>
		/// </summary>
		Progress = 2

	}

}