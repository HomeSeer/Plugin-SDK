using System.Collections.Generic;

namespace HSPI_HomeSeerSamplePlugin.Constants {

    public static class Settings {

        public const  string SettingsPage1Id   = "settings-page1";
        public const  string SettingsPage1Name = "Sample Settings";
        public static string Sp1ColorGroupId        => $"{SettingsPage1Id}-colorgroup";
        public const string Sp1ColorGroupName = "Available colors";
        public static string Sp1ColorLabelId        => $"{SettingsPage1Id}-colorlabel";
        public const string Sp1ColorLabelValue = "These control the list of colors presented for selection in the Sample Guided Process feature page.";
        public static string Sp1ColorToggleRedId    => $"{SettingsPage1Id}-red";
        public static string Sp1ColorToggleOrangeId => $"{SettingsPage1Id}-orange";
        public static string Sp1ColorToggleYellowId => $"{SettingsPage1Id}-yellow";
        public static string Sp1ColorToggleGreenId  => $"{SettingsPage1Id}-green";
        public static string Sp1ColorToggleBlueId   => $"{SettingsPage1Id}-blue";
        public static string Sp1ColorToggleIndigoId => $"{SettingsPage1Id}-indigo";
        public static string Sp1ColorToggleVioletId => $"{SettingsPage1Id}-violet";
        public const string ColorRedName = "Red";
        public const string ColorOrangeName = "Orange";
        public const string ColorYellowName = "Yellow";
        public const string ColorGreenName = "Green";
        public const string ColorBlueName = "Blue";
        public const string ColorIndigoName = "Indigo";
        public const string ColorVioletName = "Violet";
        public static Dictionary<string, string> ColorMap => new Dictionary<string, string> {
                                                    {Sp1ColorToggleRedId, ColorRedName},
                                                    {Sp1ColorToggleOrangeId, ColorOrangeName},
                                                    {Sp1ColorToggleYellowId, ColorYellowName},
                                                    {Sp1ColorToggleGreenId, ColorGreenName},
                                                    {Sp1ColorToggleBlueId, ColorBlueName},
                                                    {Sp1ColorToggleIndigoId, ColorIndigoName},
                                                    {Sp1ColorToggleVioletId, ColorVioletName}
                                                };
        public static string Sp1PageToggleGroupId => $"{SettingsPage1Id}-pagetogglegroup";
        public const  string Sp1PageToggleGroupName = "These toggle the visibility of the other 2 settings pages";
        public static string Sp1PageVisToggle1Id => $"{SettingsPage1Id}-page2toggle";
        public const  string Sp1PageVisToggle1Name = "Settings Page 2";
        public static string Sp1PageVisToggle2Id => $"{SettingsPage1Id}-page3toggle";
        public const  string Sp1PageVisToggle2Name = "Settings Page 3";
        
        public const string SettingsPage2Id   = "settings-page2";
        public const string SettingsPage2Name = "View Samples";
        
        public static string Sp2LabelWTitleId       => $"{SettingsPage2Id}-samplelabel1";
        public const string Sp2LabelWTitleName = "Sample label with title";
        public const string Sp2LabelWTitleValue = "This is a label with a title";
        public static string Sp2LabelWoTitleId => $"{SettingsPage2Id}-samplelabel2";
        public const  string Sp2LabelWoTitleValue = "This is a label without a title";
        public static string Sp2SampleToggleId => $"{SettingsPage2Id}-sampletoggle1";
        public const  string Sp2SampleToggleName = "Sample Toggle Switch";
        public static string Sp2SampleCheckBoxId => $"{SettingsPage2Id}-samplecheckbox1";
        public const  string Sp2SampleCheckBoxName = "Sample Checkbox";
        public static string Sp2SelectListId        => $"{SettingsPage2Id}-sampleselectlist1";
        public const string Sp2SelectListName = "Sample Dropdown Select List";
        public static string Sp2RadioSlId           => $"{SettingsPage2Id}-sampleselectlist2";
        public const string Sp2RadioSlName = "Sample Radio Select List";

        public static string Sp2TextAreaId => $"{SettingsPage2Id}-textarea";
        public const string Sp2TextAreaName = "Sample Text Area";
        public static List<string> Sp2SelectListOptions => new List<string> {
                                                                                "Option 1",
                                                                                "Option 2",
                                                                                "Option 3"
                                                                            };
        public static string Sp2SampleTimeSpanId => $"{SettingsPage2Id}-sampletimespan";
        public const string Sp2SampleTimeSpanName = "Sample Time Span";

        public const string SettingsPage3Id   = "settings-page3";
        public const string SettingsPage3Name = "Input View Samples";
        public static string Sp3ViewGroupId => $"{SettingsPage3Id}-sampleviewgroup1";
        public const  string Sp3ViewGroupName = "Sample View Group of Input";
        public static string Sp3SampleInput1Id => $"{SettingsPage3Id}-sampleinput1";
        public const string Sp3SampleInput1Name = "Sample Text Input";
        public static string Sp3SampleInput2Id => $"{SettingsPage3Id}-sampleinput2";
        public const string Sp3SampleInput2Name = "Sample Number Input";
        public static string Sp3SampleInput3Id => $"{SettingsPage3Id}-sampleinput3";
        public const string Sp3SampleInput3Name = "Sample Email Input";
        public static string Sp3SampleInput4Id => $"{SettingsPage3Id}-sampleinput4";
        public const string Sp3SampleInput4Name = "Sample URL Input";
        public static string Sp3SampleInput5Id => $"{SettingsPage3Id}-sampleinput5";
        public const string Sp3SampleInput5Name = "Sample Password Input";
        public static string Sp3SampleInput6Id => $"{SettingsPage3Id}-sampleinput6";
        public const string Sp3SampleInput6Name = "Sample Decimal Input";
        
    }

}