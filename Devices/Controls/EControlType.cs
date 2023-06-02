using System;

namespace HomeSeer.PluginSdk.Devices.Controls {

    /// <summary>
    /// Render style and general behavior for a <see cref="StatusControl"/>.
    /// </summary>
    /// <remarks>
    /// This is used by HomeSeer to determine how a <see cref="StatusControl"/> should look, feel, and behave for a user.
    /// </remarks>
    /// <seealso cref="StatusControl.ControlType"/>
    public enum EControlType {

        /// <summary>
        /// This is not rendered.
        /// </summary>
        /// <remarks>
        /// This is a legacy type. It is read only and for legacy support only. You should not use this for new <see cref="StatusControl"/>s
        /// </remarks>
        [Obsolete("This is a legacy type. It is read only and for legacy support only.", false)]
        StatusOnly         = 1,
        /// <summary>
        /// Rendered as a select list option.
        /// </summary>
        /// <remarks>
        /// <para>
        /// There must be multiple <see cref="StatusControl"/>s defined as <see cref="Values"/> for this to work
        ///  properly. All <see cref="StatusControl"/>s set as <see cref="Values"/> are collected and displayed
        ///  together as a single select list.
        /// </para>
        /// <para>
        /// This renders and behaves slightly different than <see cref="TextSelectList"/>. The exact difference is being worked on at this time.
        /// </para>
        /// </remarks>
        /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/select">Mozilla Docs - Select Input</seealso>
		[Obsolete("This type is being worked on. Its display behavior may change.", false)]
        Values             = 2,
        /// <summary>
        /// Rendered as a select list.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Possible options are defined by <see cref="StatusControl.ControlStates"/>, or there should be multiple
        ///  <see cref="StatusControl"/>s defined as <see cref="TextSelectList"/> for this to work
        ///  properly. All <see cref="StatusControl"/>s set as <see cref="TextSelectList"/> are collected and displayed
        ///  together as a single select list.
        /// </para>
        /// <para>
        /// Multiple <see cref="TextSelectList"/> controls are used when a unique value needs to be defined for the
        ///  different options that can be selected. This is useful for integration with 3rd-party systems where an
        ///  analysis of the selected string is not possible.
        /// </para>
        /// </remarks>
        /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/select">Mozilla Docs - Select Input</seealso>
        TextSelectList     = 3,
        /// <summary>
        /// Rendered as a button.
        /// </summary>
        /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/button">Mozilla Docs - Button Input</seealso>
        Button             = 5,
        /// <summary>
        /// Rendered as a select list.
        /// <para>
        /// Options are determined by the <see cref="StatusControl.TargetRange"/>'s
        ///  <see cref="ValueRange.Min"/>, <see cref="ValueRange.Max"/>, and <see cref="ValueRange.Divisor"/>
        /// </para>
        /// </summary>
        /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/select">Mozilla Docs - Select Input</seealso>
        ValueRangeDropDown = 6,
        /// <summary>
        /// Rendered as a slider.
        /// </summary>
        /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/range">Mozilla Docs - Range Input</seealso>
        ValueRangeSlider   = 7,
        /// <summary>
        /// Rendered as a number input box.
        /// </summary>
        /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/number">Mozilla Docs - Number Input</seealso>
        TextBoxNumber      = 9,
        /// <summary>
        /// Rendered as a text input box.
        /// </summary>
        /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/text">Mozilla Docs - Text Input</seealso>
        TextBoxString      = 10,
        /// <summary>
        /// Rendered as a radio button
        /// </summary>
        /// <remarks>
        /// <para>There should be multiple <see cref="StatusControl"/>s defined as <see cref="RadioOption"/> for this to work
        ///  properly. All <see cref="StatusControl"/>s set as <see cref="RadioOption"/> are collected and displayed together.</para>
        /// </remarks>
        /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/radio">Mozilla Docs - Radio Input</seealso>
        RadioOption        = 11,
        /// <summary>
        /// Rendered as a button. Executes a script when activated.
        /// </summary>
        /// <remarks>
        /// This is a legacy type. It is read only and for legacy support only at this time. You should not use this for new <see cref="StatusControl"/>s
        /// </remarks>
        /// <seealso href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/button">Mozilla Docs - Button Input</seealso>
        [Obsolete("This is a legacy type. It is read only and for legacy support only.", false)]
        ButtonScript       = 12,
        /// <summary>
        /// Rendered as a color picker.
        /// </summary>
        ColorPicker        = 13  //TODO more docs on color picker behavior

    }

}