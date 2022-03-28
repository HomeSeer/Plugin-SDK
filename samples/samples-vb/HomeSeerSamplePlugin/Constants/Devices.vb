Public Module Devices
    
    Public ReadOnly Property SampleDeviceTypeList As List(Of String)
        Get
            Return New List(Of String) From {
                "Line-powered switch",
                "Line-powered sensor",
                "Battery-powered sensor",
                "Thermostat"
                }
        End Get
    End Property

    Public ReadOnly Property SampleDeviceTypeFeatures As List(Of String())
        Get
            Return New List(Of String()) From {
                LinePoweredSwitchFeatures,
                LinePoweredSensorFeatures,
                BatteryPoweredSensorFeatures,
                ThermostatFeatures
                }
        End Get
    End Property

    Public ReadOnly Property LinePoweredSwitchFeatures As String()
        Get
            Return {"On-Off control feature"}
        End Get
    End Property

    Public ReadOnly Property LinePoweredSensorFeatures As String()
        Get
            Return {"Open-Close status feature"}
        End Get
    End Property

    Public ReadOnly Property BatteryPoweredSensorFeatures As String()
        Get
            Return {"Open-Close status feature", "Battery status feature"}
        End Get
    End Property

    Public ReadOnly Property ThermostatFeatures As String()
        Get
            Return {"Ambient Temperature feature", "Heating Setpoint feature", "Cooling Setpoint feature", "HVAC Mode feature", "HVAC Status feature", "Fan Mode feature", "Fan Status feature", "Humidity feature"}
        End Get
    End Property

    Public Const ThermostatHvacModeHeat As Integer = 1
    Public Const ThermostatHvacModeCool As Integer = 2
    Public Const ThermostatHvacModeAuto As Integer = 3
    Public Const ThermostatHvacModeAuxHeat As Integer = 4
    Public Const ThermostatHvacModeOff As Integer = 5
    Public Const ThermostatHvacStatusIdle As Integer = 0
    Public Const ThermostatHvacStatusHeating As Integer = 1
    Public Const ThermostatHvacStatusCooling As Integer = 2
    Public Const ThermostatFanModeAuto As Integer = 1
    Public Const ThermostatFanModeOn As Integer = 2
    Public Const ThermostatFanStatusOff As Integer = 0
    Public Const ThermostatFanStatusOn As Integer = 1
    Public Const ThermostatSetpointDecrement As Integer = 1000
    Public Const ThermostatSetpointIncrement As Integer = 1001

    Public Const DeviceConfigPageId As String = "device-config-page"
    Public Const DeviceConfigPageName As String = "Sample Device Config"

    Public ReadOnly Property DeviceConfigLabelWTitleId As String
        Get
            Return $"{DeviceConfigPageId}-samplelabel1"
        End Get
    End Property

    Public Const DeviceConfigLabelWTitleName As String = "Sample label with title"
    Public Const DeviceConfigLabelWTitleValue As String = "This is a label with a title"

    Public ReadOnly Property DeviceConfigLabelWoTitleId As String
        Get
            Return $"{DeviceConfigPageId}-samplelabel2"
        End Get
    End Property

    Public Const DeviceConfigLabelWoTitleValue As String = "This is a label without a title"

    Public ReadOnly Property DeviceConfigSampleToggleId As String
        Get
            Return $"{DeviceConfigPageId}-sampletoggle1"
        End Get
    End Property

    Public Const DeviceConfigSampleToggleName As String = "Sample Toggle Switch"

    Public ReadOnly Property DeviceConfigSampleCheckBoxId As String
        Get
            Return $"{DeviceConfigPageId}-samplecheckbox1"
        End Get
    End Property

    Public Const DeviceConfigSampleCheckBoxName As String = "Sample Checkbox"

    Public ReadOnly Property DeviceConfigSelectListId As String
        Get
            Return $"{DeviceConfigPageId}-sampleselectlist1"
        End Get
    End Property

    Public Const DeviceConfigSelectListName As String = "Sample Dropdown Select List"

    Public ReadOnly Property DeviceConfigRadioSlId As String
        Get
            Return $"{DeviceConfigPageId}-sampleselectlist2"
        End Get
    End Property

    Public Const DeviceConfigRadioSlName As String = "Sample Radio Select List"

    Public ReadOnly Property DeviceConfigSelectListOptions As List(Of String)
        Get
            Return New List(Of String) From {
                "Option 1",
                "Option 2",
                "Option 3"
            }
        End Get
    End Property

    Public ReadOnly Property DeviceConfigInputId As String
        Get
            Return $"{DeviceConfigPageId}-sampleinput"
        End Get
    End Property

    Public Const DeviceConfigInputName As String = "Sample Text Input"
    Public Const DeviceConfigInputValue As String = "This is a text input"

    Public ReadOnly Property DeviceConfigDateInputId As String
        Get
            Return $"{DeviceConfigPageId}-sampledateinput"
        End Get
    End Property

    Public Const DeviceConfigDateInputName As String = "Sample Date Input"
    Public Const DeviceConfigDateInputValue As String = ""

    Public ReadOnly Property DeviceConfigTimeInputId As String
        Get
            Return $"{DeviceConfigPageId}-sampletimeinput"
        End Get
    End Property

    Public Const DeviceConfigTimeInputName As String = "Sample Time Input"
    Public Const DeviceConfigTimeInputValue As String = ""

    Public ReadOnly Property DeviceConfigTextAreaId As String
        Get
            Return $"{DeviceConfigPageId}-sampletextarea"
        End Get
    End Property

    Public Const DeviceConfigTextAreaName As String = "Sample Text Area"

    Public ReadOnly Property DeviceConfigTimeSpanId As String
        Get
            Return $"{DeviceConfigPageId}-sampletimespan"
        End Get
    End Property

    Public Const DeviceConfigTimeSpanName As String = "Sample Time Span"

    Public ReadOnly Property DeviceConfigNavButton1Id As String
        Get
            Return $"{DeviceConfigPageId}-samplenavbutton1"
        End Get
    End Property
    Public ReadOnly Property DeviceConfigNavButton2Id As String
        Get
            Return $"{DeviceConfigPageId}-samplenavbutton2"
        End Get
    End Property
End Module