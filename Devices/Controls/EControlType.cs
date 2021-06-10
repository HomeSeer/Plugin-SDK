namespace HomeSeer.PluginSdk.Devices.Controls {

    public enum EControlType {

        [Obsolete("This is a legacy type. It is read only and for legacy support only.", false)]
        StatusOnly         = 1,
        [Obsolete("This type is being worked on. Its display behavior may change.", false)]
        Values             = 2,
        TextSelectList     = 3,
        Button             = 5,
        [Obsolete("Due to a lack of ValueRange.Divisor property this type cannot be properly used at this time.", false)]
        ValueRangeDropDown = 6,
        ValueRangeSlider   = 7,
        TextBoxNumber      = 9,
        TextBoxString      = 10,
        RadioOption        = 11,
        [Obsolete("This is a legacy type. It is read only and for legacy support only.", false)]
        ButtonScript       = 12,
        ColorPicker        = 13  //TODO more docs on color picker behavior

    }

}