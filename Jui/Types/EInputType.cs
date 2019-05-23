namespace HomeSeer.Jui.Types {

	/// <summary>
	/// The input type determines the type of data that will be accepted by an input
	/// view and the keyboard that is shown to the user when they are typing in it.
	/// </summary>
	public enum EInputType {

		/// <summary>
		/// Used for text of any kind
		/// </summary>
		Text = 0,
		/// <summary>
		/// Used for whole numbers
		/// </summary>
		Number = 1,
		/// <summary>
		/// Used for Email Addresses in the format of *@*.*
		/// </summary>
		Email = 2,
		/// <summary>
		/// Used for web addresses; values are parsed to ensure they are a valid URI
		/// </summary>
		Url = 3,
		/// <summary>
		/// Used to mask secure text
		/// </summary>
		Password = 4,
		/// <summary>
		/// Used for decimal numbers
		/// </summary>
		Decimal = 5
		/*Color = 6,
		Date = 7,
		DateTime = 8,
		Time = 9*/

	}

}