Imports HomeSeer.Jui.Views
Imports HomeSeer.PluginSdk.Events
Imports HomeSeer.PluginSdk.Logging

''' <summary>
''' A sample event action type that writes a message to the HomeSeer log
''' </summary>
Public Class WriteLogSampleActionType
    Inherits AbstractActionType

    ''' <summary>
    ''' The name of this action in the list of available actions on the event page
    ''' </summary>
    Private Const ActionName As String = "Sample Plugin Action - Write to Log"

    ''' <summary>
    ''' The ID of the instructions label view
    ''' </summary>
    Private ReadOnly Property InstructionsLabelId As String
        Get
            Return $"{PageId}-instructlabel"
        End Get
    End Property
    ''' <summary>
    ''' The value of the instructions label view
    ''' </summary>
    Private Const InstructionsLabelValue As String = "Write a message to the log with a type of..."
    ''' <summary>
    ''' The ID of the log type select list view
    ''' </summary>
    Private ReadOnly Property LogTypeSelectListId As String
        Get
            Return $"{PageId}-logtypesl"
        End Get
    End Property
    ''' <summary>
    ''' The ID of the log message input view
    ''' </summary>
    Private ReadOnly Property LogMessageInputId As String
        Get
            Return $"{PageId}-messageinput"
        End Get
    End Property

    Private mvarlogTypeOptions As List(Of String)
    ''' <summary>
    ''' The available log types to select from
    ''' </summary>
    ''' 
    Private ReadOnly Property _logTypeOptions As List(Of String)
        Get
            If mvarlogTypeOptions Is Nothing Then
                mvarlogTypeOptions = New List(Of String) From {
                    "Trace",
                    "Debug",
                    "Info",
                    "Warning",
                    "Error"
                }
            End If
            Return mvarlogTypeOptions
        End Get
    End Property

    ''' <summary>
    ''' The interface bridge to HSPI that enables proper encapsulation of access to the HomeSeer System
    ''' </summary>
    Private ReadOnly Property Listener As IWriteLogActionListener
        Get
            Return TryCast(ActionListener, IWriteLogActionListener)
        End Get
    End Property

    ''' <inheritdoc />
    ''' <remarks>
    ''' All action types must implement this constructor
    ''' </remarks>
    Public Sub New(ByVal id As Integer, ByVal eventRef As Integer, ByVal dataIn As Byte(), ByVal inListener As ActionTypeCollection.IActionTypeListener, Optional ByVal debugLog As Boolean = False)
        MyBase.New(id, eventRef, dataIn, inListener, debugLog)
    End Sub
    ''' <inheritdoc />
    ''' <remarks>
    ''' All action types must implement this constructor
    ''' </remarks>
    Public Sub New()
    End Sub

    ''' <inheritdoc />
    ''' <remarks>
    ''' Return the name of the action type
    ''' </remarks>
    Protected Overrides Function GetName() As String
        Return ActionName
    End Function

    ''' <inheritdoc />
    ''' <remarks>
    ''' This action type always starts with a single select list.
    ''' </remarks>
    Protected Overrides Sub OnNewAction()
        Dim confPage = InitNewConfigPage()
        ConfigPage = confPage.Page
    End Sub
    
    ''' <inheritdoc />
    ''' <remarks>
    ''' This action type is only fully configured once a message is specified in the input view
    ''' </remarks>
    Public Overrides Function IsFullyConfigured() As Boolean
        Select Case ConfigPage.ViewCount
            Case 3
                Dim inputView = TryCast(ConfigPage.GetViewById(LogMessageInputId), InputView)
                Return (If(inputView?.Value?.Length, 0)) > 0
            Case Else
                Return False
        End Select
    End Function

    ''' <inheritdoc />
    ''' <remarks>
    ''' This is where we validate data entry and update the <see cref="AbstractActionType.ConfigPage"/>
    '''  so that it represents the next state it should be in for configuration.
    ''' </remarks>
    Protected Overrides Function OnConfigItemUpdate(ByVal configViewChange As AbstractView) As Boolean
        If configViewChange.Id <> LogTypeSelectListId Then
            'When the ID being changed is not the log type select list, always save and continue.
            ' No more configuration is needed
            Return True
        End If

        'Log Type selection change
        
        Dim changedLogTypeSl As SelectListView = TryCast(configViewChange, SelectListView)
        'Make sure the change is to a select list view
        If changedLogTypeSl Is Nothing Then
            Return False
        End If

        
        Dim currentLogTypeSl As SelectListView = TryCast(ConfigPage.GetViewById(LogTypeSelectListId), SelectListView)
        'Make sure the target select list view casts correctly
        If currentLogTypeSl Is Nothing Then
            Return False
        End If

        If currentLogTypeSl.Selection = changedLogTypeSl.Selection Then
            'If the selection didn't change then return false because the user may still need to supply a message
            Return False
        End If

        'Initialize the new state of the page so it asks for a message
        Dim newConfPage = InitConfigPageWithInput()
        ConfigPage = newConfPage.Page
        'Save the change to the log type select list
        Return True
    End Function
    
    ''' <inheritdoc />
    Public Overrides Function GetPrettyString() As String
        Dim selectList = TryCast(ConfigPage.GetViewById(LogTypeSelectListId), SelectListView)
        Dim message = If(ConfigPage?.GetViewById(LogMessageInputId)?.GetStringValue(), "Error retrieving log message")
        Return $"write the message '{message}' to the log with the type of {If(selectList?.GetSelectedOption(), "Unknown Selection")}"
    End Function

    ''' <inheritdoc />
    ''' <remarks>
    ''' This will call to HSPI through the <see cref="IWriteLogActionListener"/> interface to write a log message
    ''' </remarks>
    Public Overrides Function OnRunAction() As Boolean
        Dim iLogType = If((TryCast(ConfigPage?.GetViewById(LogTypeSelectListId), SelectListView))?.Selection, 0)
        Dim logType As ELogType

        Select Case iLogType
            Case 0
                logType = ELogType.Trace
            Case 1
                logType = ELogType.Debug
            Case 2
                logType = ELogType.Info
            Case 3
                logType = ELogType.Warning
            Case 4
                logType = ELogType.[Error]
            Case Else
                logType = ELogType.Info
        End Select

        Dim message = If(ConfigPage?.GetViewById(LogMessageInputId)?.GetStringValue(), "Error retrieving log message")
        Listener?.WriteLog(logType, message)
        Return True
    End Function

    ''' <inheritdoc />
    ''' <remarks>
    ''' This action type does not do anything with devices/features; so we should always return false here.
    ''' </remarks>
    Public Overrides Function ReferencesDeviceOrFeature(ByVal devOrFeatRef As Integer) As Boolean
        Return False
    End Function

    ''' <summary>
    ''' Initialize a new ConfigPage for initial setup of the action where the user must select the log type.
    ''' </summary>
    ''' <returns>A <see cref="PageFactory"/> representing the new ConfigPage</returns>
    Private Function InitNewConfigPage() As PageFactory
        Dim confPage = PageFactory.CreateEventActionPage(PageId, ActionName)
        confPage.WithLabel(InstructionsLabelId, Nothing, InstructionsLabelValue)
        confPage.WithDropDownSelectList(LogTypeSelectListId, "Log Type", _logTypeOptions)
        Return confPage
    End Function

    ''' <summary>
    ''' Initialize a new ConfigPage so the user can supply a message to write to the log
    ''' </summary>
    ''' <returns>A <see cref="PageFactory"/> representing the new ConfigPage</returns>
    Private Function InitConfigPageWithInput() As PageFactory
        Dim confPage = InitNewConfigPage()
        confPage.WithInput(LogMessageInputId, "Message")
        Return confPage
    End Function

    ''' <summary>
    ''' An interface bridge to HSPI that enables proper encapsulation of access to the HomeSeer System
    ''' </summary>
    Interface IWriteLogActionListener
        Inherits ActionTypeCollection.IActionTypeListener

        ''' <summary>
        ''' Write a log message. Called by <see cref="WriteLogSampleActionType"/>.
        ''' </summary>
        ''' <param name="logType">The <see cref="ELogType"/> of the message.</param>
        ''' <param name="message">The message to write to the log.</param>
        Sub WriteLog(ByVal logType As ELogType, ByVal message As String)
    End Interface

End Class