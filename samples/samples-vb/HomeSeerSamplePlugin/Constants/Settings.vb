Namespace Constants
    Public Module Settings

        Public Const SettingsPage1Id As String = "settings-page1"
        Public Const SettingsPage1Name As String = "Sample Settings"

        Public ReadOnly Property Sp1ColorGroupId As String
            Get
                Return $"{SettingsPage1Id}-colorgroup"
            End Get
        End Property

        Public Const Sp1ColorGroupName As String = "Available colors"

        Public ReadOnly Property Sp1ColorLabelId As String
            Get
                Return $"{SettingsPage1Id}-colorlabel"
            End Get
        End Property

        Public Const Sp1ColorLabelValue As String = "These control the list of colors presented for selection in the Sample Guided Process feature page."

        Public ReadOnly Property Sp1ColorToggleRedId As String
            Get
                Return $"{SettingsPage1Id}-red"
            End Get
        End Property

        Public ReadOnly Property Sp1ColorToggleOrangeId As String
            Get
                Return $"{SettingsPage1Id}-orange"
            End Get
        End Property

        Public ReadOnly Property Sp1ColorToggleYellowId As String
            Get
                Return $"{SettingsPage1Id}-yellow"
            End Get
        End Property

        Public ReadOnly Property Sp1ColorToggleGreenId As String
            Get
                Return $"{SettingsPage1Id}-green"
            End Get
        End Property

        Public ReadOnly Property Sp1ColorToggleBlueId As String
            Get
                Return $"{SettingsPage1Id}-blue"
            End Get
        End Property

        Public ReadOnly Property Sp1ColorToggleIndigoId As String
            Get
                Return $"{SettingsPage1Id}-indigo"
            End Get
        End Property

        Public ReadOnly Property Sp1ColorToggleVioletId As String
            Get
                Return $"{SettingsPage1Id}-violet"
            End Get
        End Property

        Public Const ColorRedName As String = "Red"
        Public Const ColorOrangeName As String = "Orange"
        Public Const ColorYellowName As String = "Yellow"
        Public Const ColorGreenName As String = "Green"
        Public Const ColorBlueName As String = "Blue"
        Public Const ColorIndigoName As String = "Indigo"
        Public Const ColorVioletName As String = "Violet"
        Public ReadOnly Property ColorMap As Dictionary(Of String, String)
            Get
                Return New Dictionary(Of String, String) From {
                {Sp1ColorToggleRedId, ColorRedName},
                {Sp1ColorToggleOrangeId, ColorOrangeName},
                {Sp1ColorToggleYellowId, ColorYellowName},
                {Sp1ColorToggleGreenId, ColorGreenName},
                {Sp1ColorToggleBlueId, ColorBlueName},
                {Sp1ColorToggleIndigoId, ColorIndigoName},
                {Sp1ColorToggleVioletId, ColorVioletName}
                }
            End Get
        End Property

        Public ReadOnly Property Sp1PageToggleGroupId As String
            Get
                Return $"{SettingsPage1Id}-pagetogglegroup"
            End Get
        End Property

        Public Const Sp1PageToggleGroupName As String = "These toggle the visibility of the other 2 settings pages"

        Public ReadOnly Property Sp1PageVisToggle1Id As String
            Get
                Return $"{SettingsPage1Id}-page2toggle"
            End Get
        End Property

        Public Const Sp1PageVisToggle1Name As String = "Settings Page 2"

        Public ReadOnly Property Sp1PageVisToggle2Id As String
            Get
                Return $"{SettingsPage1Id}-page3toggle"
            End Get
        End Property

        Public Const Sp1PageVisToggle2Name As String = "Settings Page 3"
        Public Const SettingsPage2Id As String = "settings-page2"
        Public Const SettingsPage2Name As String = "View Samples"

        Public ReadOnly Property Sp2LabelWTitleId As String
            Get
                Return $"{SettingsPage2Id}-samplelabel1"
            End Get
        End Property

        Public Const Sp2LabelWTitleName As String = "Sample label with title"
        Public Const Sp2LabelWTitleValue As String = "This is a label with a title"

        Public ReadOnly Property Sp2LabelWoTitleId As String
            Get
                Return $"{SettingsPage2Id}-samplelabel2"
            End Get
        End Property

        Public Const Sp2LabelWoTitleValue As String = "This is a label without a title"

        Public ReadOnly Property Sp2SampleToggleId As String
            Get
                Return $"{SettingsPage2Id}-sampletoggle1"
            End Get
        End Property

        Public Const Sp2SampleToggleName As String = "Sample Toggle Switch"

        Public ReadOnly Property Sp2SampleCheckBoxId As String
            Get
                Return $"{SettingsPage2Id}-samplecheckbox1"
            End Get
        End Property

        Public Const Sp2SampleCheckBoxName As String = "Sample Checkbox"

        Public ReadOnly Property Sp2SelectListId As String
            Get
                Return $"{SettingsPage2Id}-sampleselectlist1"
            End Get
        End Property

        Public Const Sp2SelectListName As String = "Sample Dropdown Select List"

        Public ReadOnly Property Sp2RadioSlId As String
            Get
                Return $"{SettingsPage2Id}-sampleselectlist2"
            End Get
        End Property

        Public Const Sp2RadioSlName As String = "Sample Radio Select List"

        Public ReadOnly Property Sp2TextAreaId As String
            Get
                Return $"{SettingsPage2Id}-textarea"
            End Get
        End Property

        Public Const Sp2TextAreaName As String = "Sample Text Area"

        Public ReadOnly Property Sp2SelectListOptions As List(Of String)
            Get
                Return New List(Of String) From {
            "Option 1",
            "Option 2",
            "Option 3"
        }
            End Get
        End Property

        Public ReadOnly Property Sp2SampleTimeSpanId As String
            Get
                Return $"{SettingsPage2Id}-sampletimespan"
            End Get
        End Property

        Public Const Sp2SampleTimeSpanName As String = "Sample Time Span"

        Public Const SettingsPage3Id As String = "settings-page3"
        Public Const SettingsPage3Name As String = "Input View Samples"

        Public ReadOnly Property Sp3ViewGroupId As String
            Get
                Return $"{SettingsPage3Id}-sampleviewgroup1"
            End Get
        End Property

        Public Const Sp3ViewGroupName As String = "Sample View Group of Input"

        Public ReadOnly Property Sp3SampleInput1Id As String
            Get
                Return $"{SettingsPage3Id}-sampleinput1"
            End Get
        End Property

        Public Const Sp3SampleInput1Name As String = "Sample Text Input"

        Public ReadOnly Property Sp3SampleInput2Id As String
            Get
                Return $"{SettingsPage3Id}-sampleinput2"
            End Get
        End Property

        Public Const Sp3SampleInput2Name As String = "Sample Number Input"

        Public ReadOnly Property Sp3SampleInput3Id As String
            Get
                Return $"{SettingsPage3Id}-sampleinput3"
            End Get
        End Property

        Public Const Sp3SampleInput3Name As String = "Sample Email Input"

        Public ReadOnly Property Sp3SampleInput4Id As String
            Get
                Return $"{SettingsPage3Id}-sampleinput4"
            End Get
        End Property

        Public Const Sp3SampleInput4Name As String = "Sample URL Input"

        Public ReadOnly Property Sp3SampleInput5Id As String
            Get
                Return $"{SettingsPage3Id}-sampleinput5"
            End Get
        End Property

        Public Const Sp3SampleInput5Name As String = "Sample Password Input"

        Public ReadOnly Property Sp3SampleInput6Id As String
            Get
                Return $"{SettingsPage3Id}-sampleinput6"
            End Get
        End Property

        Public Const Sp3SampleInput6Name As String = "Sample Decimal Input"

    End Module
End Namespace