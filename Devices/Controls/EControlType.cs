namespace HomeSeer.PluginSdk.Devices.Controls {

    public enum EControlType {

        [Obsolete("This is a legacy type. It is read only and for legacy support only.", false)]
        StatusOnly         = 1,
        Values             = 2,
        TextSelectList     = 3,
        Button             = 5,
        ValueRangeDropDown = 6, // Rendered as a drop-list by default.
        ValueRangeSlider   = 7,
        TextBoxNumber      = 9,
        TextBoxString      = 10,
        RadioOption        = 11,
        [Obsolete("This is a legacy type. It is read only and for legacy support only.", false)]
        ButtonScript       = 12,
        ColorPicker        = 13  //TODO more docs on color picker behavior

    }

}