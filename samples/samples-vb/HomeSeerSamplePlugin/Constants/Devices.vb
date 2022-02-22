Public Module Devices
    
    Public ReadOnly Property SampleDeviceTypeList As List(Of String)
        Get
            Return New List(Of String) From {
                "Line-powered switch",
                "Line-powered sensor"
                }
        End Get
    End Property

    Public ReadOnly Property SampleDeviceTypeFeatures As List(Of String())
        Get
            Return New List(Of String()) From {
                LinePoweredSwitchFeatures,
                LinePoweredSensorFeatures
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
End Module