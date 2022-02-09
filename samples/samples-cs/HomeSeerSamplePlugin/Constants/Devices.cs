using System.Collections.Generic;

namespace HSPI_HomeSeerSamplePlugin.Constants {

    public static class Devices {
        
        public static List<string> SampleDeviceTypeList => new List<string> {
                                                                    "Line-powered switch",
                                                                    "Line-powered sensor"
                                                                };
        
        public static List<string[]> SampleDeviceTypeFeatures => new List<string[]>
                                                                     {
                                                                         LinePoweredSwitchFeatures,
                                                                         LinePoweredSensorFeatures
                                                                     };
        
        public static string[] LinePoweredSwitchFeatures => new []{ "On-Off control feature" };
        public static string[] LinePoweredSensorFeatures => new []{ "Open-Close status feature" };

        public const string DeviceConfigPageId = "device-config-page";
        public const string DeviceConfigPageName = "Sample Device Config";

        public static string DeviceConfigLabelWTitleId => $"{DeviceConfigPageId}-samplelabel1";
        public const string DeviceConfigLabelWTitleName = "Sample label with title";
        public const string DeviceConfigLabelWTitleValue = "This is a label with a title";
        public static string DeviceConfigLabelWoTitleId => $"{DeviceConfigPageId}-samplelabel2";
        public const string DeviceConfigLabelWoTitleValue = "This is a label without a title";
        public static string DeviceConfigSampleToggleId => $"{DeviceConfigPageId}-sampletoggle1";
        public const string DeviceConfigSampleToggleName = "Sample Toggle Switch";
        public static string DeviceConfigSampleCheckBoxId => $"{DeviceConfigPageId}-samplecheckbox1";
        public const string DeviceConfigSampleCheckBoxName = "Sample Checkbox";
        public static string DeviceConfigSelectListId => $"{DeviceConfigPageId}-sampleselectlist1";
        public const string DeviceConfigSelectListName = "Sample Dropdown Select List";
        public static string DeviceConfigRadioSlId => $"{DeviceConfigPageId}-sampleselectlist2";
        public const string DeviceConfigRadioSlName = "Sample Radio Select List";
        public static List<string> DeviceConfigSelectListOptions => new List<string> {
                                                                                "Option 1",
                                                                                "Option 2",
                                                                                "Option 3"
                                                                            };
        public static string DeviceConfigInputId => $"{DeviceConfigPageId}-sampleinput";
        public const string DeviceConfigInputName = "Sample Text Input";
        public const string DeviceConfigInputValue = "This is a text input";

        public static string DeviceConfigTextAreaId => $"{DeviceConfigPageId}-sampletextarea";
        public const string DeviceConfigTextAreaName = "Sample Text Area";
        
        public static string DeviceConfigTimeSpanId => $"{DeviceConfigPageId}-sampletimespan";
        public const string DeviceConfigTimeSpanName = "Sample Time Span";

    }

}