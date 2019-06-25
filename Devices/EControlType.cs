namespace HomeSeer.PluginSdk.Devices {

    public enum EControlType {

        NotSpecified         = 1,
        Values                = 2, // This is the default to use if one of the others is not specified.
        SingleTextFromList = 3,
        ListTextFromList   = 4,
        Button                = 5,
        ValuesRange           = 6, // Rendered as a drop-list by default.
        ValuesRangeSlider     = 7,
        TextList              = 8,
        TextBoxNumber        = 9,
        TextBoxString        = 10,
        RadioOption          = 11,
        ButtonScript         = 12, // Rendered as a button, executes a script when activated.
        ColorPicker          = 13

    }

}