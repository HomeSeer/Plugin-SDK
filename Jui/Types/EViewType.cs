namespace HomeSeer.Jui.Types {

	/// <summary>
	/// The type that a view is set to determines what class a client application will deserialize it as.
	/// Each view type has a different set of properties that specifies how it behaves and how it should
	/// be displayed to the user
	/// <para>
	/// Some view types are ignored by different page types.
	/// Refer to the page type you are using to determine what views are available for use.
	/// </para>
	/// </summary>
	public enum EViewType {

		/// <summary>
		/// No type is defined.  This will cause an error
		/// </summary>
		Undefined = -1,
		/// <summary>
		/// A <see cref="HomeSeer.Jui.Views.ViewGroup"/>
		/// </summary>
		Group = 0,
		/// <summary>
		/// An <see cref="HomeSeer.Jui.Forms.AbstractFormView"/>
		/// </summary>
		Form = 1,
		/// <summary>
		/// A <see cref="HomeSeer.Jui.Views.LabelView"/>
		/// </summary>
		Label = 2,
		/// <summary>
		/// A <see cref="HomeSeer.Jui.Views.SelectListView"/>
		/// </summary>
		SelectList = 3,
		/// <summary>
		/// An <see cref="HomeSeer.Jui.Views.InputView"/>
		/// </summary>
		Input = 4,
		/// <summary>
		/// A <see cref="HomeSeer.Jui.Views.ToggleView"/>
		/// </summary>
		Toggle = 5,
		/// <summary>
		/// A <see cref="HomeSeer.Jui.Views.ButtonView"/>
		/// </summary>
		Button = 6,
		/// <summary>
		/// Not implemented yet
		/// </summary>
		Image = 7,
		/// <summary>
		/// Not implemented yet
		/// </summary>
		DateTime = 8,
		/// <summary>
		/// Not implemented yet
		/// </summary>
		MessageArchive = 9,
		/// <summary>
		/// Not implemented yet
		/// </summary>
		Table = 10,
		/// <summary>
		/// Not implemented yet
		/// </summary>
		Chart = 11

	}

}