Imports HomeSeer.Jui.Views
Imports HomeSeer.PluginSdk.Events

''' <summary>
''' A sample event trigger type fired from the Trigger Feature page.
''' </summary>
Public Class SampleTriggerType
    Inherits AbstractTriggerType

    ''' <summary>
    ''' The 1 based index of this trigger type. Used for reference in this class.
    ''' </summary>
    Public Const TriggerNumber As Integer = 1
    ''' <summary>
    ''' The name of this trigger in the list of available triggers on the event page
    ''' </summary>
    Private Const TriggerName As String = "Sample Trigger"

    ''' <summary>
    ''' The ID of the option count select list
    ''' </summary>
    Private ReadOnly Property OptionCountSlId As String
        Get
            Return $"{PageId}-optioncountsl"
        End Get
    End Property
    ''' <summary>
    ''' The name of the option count select list
    ''' </summary>
    Private Const OptionCountSlName As String = "Number of Options Checked"
    ''' <summary>
    ''' The ID of the required option select list
    ''' </summary>
    Private ReadOnly Property OptionNumSlId As String
        Get
            Return $"{PageId}-optionnumsl"
        End Get
    End Property
    ''' <summary>
    ''' The name of the required option select list
    ''' </summary>
    Private Const OptionNumSlName As String = "Required Option"

    ''' <summary>
    ''' Determine whether the trigger should fire based on the specified option selections
    ''' </summary>
    ''' <param name="triggerOptions">
    ''' A boolean array describing the state of the options when the trigger button was clicked
    ''' </param>
    ''' <returns>
    ''' TRUE if the trigger should fire,
    '''  FALSE if it shouldn't
    ''' </returns>
    Public Function ShouldTriggerFire(ParamArray triggerOptions As Boolean()) As Boolean
        Select Case SelectedSubTriggerIndex
            Case 0
                Dim numRequiredOptions = GetSelectedOptionCount() + 1
                Return numRequiredOptions <> 0 AndAlso triggerOptions.Count(Function(triggerOption) triggerOption) = numRequiredOptions
            Case 1
                Dim specificRequiredOption = GetSelectedSpecificOptionNum()

                If triggerOptions.Length < specificRequiredOption + 1 Then
                    Return False
                End If

                Return triggerOptions(specificRequiredOption)
            Case 2
                Return Not triggerOptions.Any(Function(triggerOption) triggerOption)
            Case 3
                Return triggerOptions.Any(Function(triggerOption) triggerOption)
            Case Else
                Return False
        End Select
    End Function

    ''' <inheritdoc />
    Public Sub New(ByVal trigInfo As TrigActInfo, ByVal inListener As TriggerTypeCollection.ITriggerTypeListener, Optional ByVal debugLog As Boolean = False)
        MyBase.New(trigInfo, inListener, debugLog)
    End Sub
    ''' <inheritdoc />
    ''' <remarks>
    ''' All trigger types must implement this constructor
    ''' </remarks>
    Public Sub New(ByVal id As Integer, ByVal eventRef As Integer, ByVal selectedSubTriggerIndex As Integer, ByVal dataIn As Byte(), ByVal inListener As TriggerTypeCollection.ITriggerTypeListener, Optional ByVal debugLog As Boolean = False)
        MyBase.New(id, eventRef, selectedSubTriggerIndex, dataIn, inListener, debugLog)
    End Sub
    ''' <inheritdoc />
    ''' <remarks>
    ''' All trigger types must implement this constructor
    ''' </remarks>
    Public Sub New()
    End Sub

    ''' <inheritdoc />
    ''' <remarks>
    ''' This trigger type has 4 subtypes.
    ''' </remarks>
    Protected Overrides Property SubTriggerTypeNames As List(Of String)
        Get
            Return New List(Of String) From {
            "Button click with X options checked",
            "Button click with specific option checked",
            "Button click with no options checked",
            "Button click with any options checked"
        }
        End Get
        Set(value As List(Of String))

        End Set
    End Property

    ''' <inheritdoc />
    ''' <remarks>
    ''' Return the name of the trigger type
    ''' </remarks>
    Protected Overrides Function GetName() As String
        Return TriggerName
    End Function
    
    ''' <inheritdoc />
    ''' <remarks>
    ''' This trigger type has 3 states, 2 of which require additional configuration.
    ''' </remarks>
    Protected Overrides Sub OnNewTrigger()
        Select Case SelectedSubTriggerIndex
            Case 0
                ConfigPage = InitializeXOptionsPage().Page
            Case 1
                ConfigPage = InitializeSpecificOptionPage().Page
            Case Else
                ConfigPage = InitializeDefaultPage().Page
        End Select
    End Sub

    ''' <inheritdoc />
    ''' <remarks>
    ''' This trigger type has 3 states, 2 of which require additional configuration.
    ''' </remarks>
    Public Overrides Function IsFullyConfigured() As Boolean
        Select Case SelectedSubTriggerIndex
            Case 0
                'Check to see if the input for the number of options is valid
                Return GetSelectedOptionCount() >= 0
            Case 1
                'Check to see if the input for the required option is valid
                Return GetSelectedSpecificOptionNum() >= 0
            Case Else
                'The last two sub trigger types do not require any additional configuration
                Return True
        End Select
    End Function

    ''' <inheritdoc />
    ''' <remarks>
    ''' Because all of the available configuration options are select lists, no data validation is needed.
    '''  This means that we can always return true here so that all changes are saved.
    ''' </remarks>
    Protected Overrides Function OnConfigItemUpdate(ByVal configViewChange As AbstractView) As Boolean
        Return True
    End Function
    
    ''' <inheritdoc />
    Public Overrides Function GetPrettyString() As String
        Select Case SelectedSubTriggerIndex
            Case 0

                Try
                    Dim optionCountSl = TryCast(ConfigPage?.GetViewById(OptionCountSlId), SelectListView)
                    Return $"the button on the Sample Plugin Trigger Feature page is clicked and {(If(optionCountSl?.GetSelectedOption(), "???"))} options are checked"
                Catch exception As Exception
                    If LogDebug Then
                        Console.WriteLine(exception)
                    End If
                    Return "the button on the Sample Plugin Trigger Feature page is clicked and ??? options are checked"
                End Try

            Case 1

                Try
                    Dim optionNumSl = TryCast(ConfigPage?.GetViewById(OptionNumSlId), SelectListView)
                    Return $"the button on the Sample Plugin Trigger Feature page is clicked and option number {(If(optionNumSl?.GetSelectedOption(), "???"))} is checked"
                Catch exception As Exception
                    If LogDebug Then
                        Console.WriteLine(exception)
                    End If
                    Return "the button on the Sample Plugin Trigger Feature page is clicked and option number ??? is checked"
                End Try

            Case 2
                Return "the button the Sample Plugin Trigger Feature page is clicked and no options are checked"
            Case Else
                Return "the button the Sample Plugin Trigger Feature page is clicked"
        End Select
    End Function

    ''' <inheritdoc />
    ''' <remarks>
    ''' This trigger type is never used as a condition; so we can always return true here so manual trigger fires
    '''  are always executed.
    ''' </remarks>
    Public Overrides Function IsTriggerTrue(ByVal isCondition As Boolean) As Boolean
        Return True
    End Function
 
    ''' <inheritdoc />
    ''' <remarks>
    ''' This trigger type does not do anything with devices/features; so we should always return false here.
    ''' </remarks>
    Public Overrides Function ReferencesDeviceOrFeature(ByVal devOrFeatRef As Integer) As Boolean
        Return False
    End Function

    ''' <summary>
    ''' Initialize a new <see cref="AbstractTriggerType.ConfigPage"/> for a trigger based on a number of options
    '''  selected.
    ''' </summary>
    ''' <returns>A <see cref="PageFactory"/> initialized for the first subtype</returns>
    Private Function InitializeXOptionsPage() As PageFactory
        Dim cpf = InitializeDefaultPage()
        cpf.WithDropDownSelectList(OptionCountSlId, OptionCountSlName, {"1", "2", "3", "4"}.ToList())
        Return cpf
    End Function

    ''' <summary>
    ''' Initialize a new <see cref="AbstractTriggerType.ConfigPage"/> for a trigger based on a specific option
    '''  being selected.
    ''' </summary>
    ''' <returns>A <see cref="PageFactory"/> initialized for the second subtype</returns>
    Private Function InitializeSpecificOptionPage() As PageFactory
        Dim cpf = InitializeDefaultPage()
        cpf.WithDropDownSelectList(OptionNumSlId, OptionNumSlName, {"1", "2", "3", "4"}.ToList())
        Return cpf
    End Function

    ''' <summary>
    ''' Initialize a new <see cref="AbstractTriggerType.ConfigPage"/> for a trigger with no additional configurations.
    ''' </summary>
    ''' <returns>A <see cref="PageFactory"/> initialized for the third or fourth subtype</returns>
    Private Function InitializeDefaultPage() As PageFactory
        Dim cpf = PageFactory.CreateEventTriggerPage(PageId, TriggerName)
        Return cpf
    End Function

    ''' <summary>
    ''' Get the currently selected specific option index
    ''' </summary>
    ''' <returns>The index of the required specific option</returns>
    Private Function GetSelectedSpecificOptionNum() As Integer
        Try
            Dim optionNumSl = TryCast(ConfigPage?.GetViewById(OptionNumSlId), SelectListView)
            Return If(optionNumSl?.Selection, -1)
        Catch exception As Exception
            If LogDebug Then
                Console.WriteLine(exception)
            End If
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' Get the currently selected required option count
    ''' </summary>
    ''' <returns>The number of options that must be selected</returns>
    Private Function GetSelectedOptionCount() As Integer
        Try
            Dim optionCountSl = TryCast(ConfigPage?.GetViewById(OptionCountSlId), SelectListView)
            Return (If(optionCountSl?.Selection, -1))
        Catch exception As Exception
            If LogDebug Then
                Console.WriteLine(exception)
            End If
            Return -1
        End Try
    End Function
End Class