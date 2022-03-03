Imports HomeSeer.Jui.Views
Imports HomeSeer.PluginSdk

Public Class HSPI
    Inherits AbstractPlugin
    
   ''' <inheritdoc />
   ''' <remarks>
   ''' This ID is used to identify the plugin and should be unique across all plugins
   ''' <para>
   ''' This must match the MSBuild property $(PluginId) as this will be used to copy
   '''  all of the HTML feature pages located in .\html\ to a relative directory
   '''  within the HomeSeer html folder.
   ''' </para>
   ''' <para>
   ''' The relative address for all of the HTML pages will end up looking like this:
   '''  ..\Homeseer\Homeseer\html\HelloWorldPlugin_VB\
   ''' </para>
   ''' </remarks>
    Public Overrides ReadOnly Property Id As String
        Get
            Return "HelloWorldPlugin_VB"
        End Get
    End Property
   
    ''' <inheritdoc />
    ''' <remarks>
    ''' This is the readable name for the plugin that is displayed throughout HomeSeer
    ''' </remarks>
    Public Overrides ReadOnly Property Name As String
        Get 
            Return "HelloWorldPlugin-VB"
        End Get
    End Property

   ''' <inheritdoc />
   ''' <remarks>
   ''' This is not required as an override
   ''' </remarks>
    Protected Overrides ReadOnly Property SettingsFileName As String
        Get 
            Return "HelloWorldPlugin-VB.ini"
        End Get
    End Property

    Public Sub New()
        'Initialize the plugin 

        'Enable internal debug logging to console
        LogDebug = True
        'Setup anything that needs to be configured before a connection to HomeSeer is established
        ' like initializing the starting state of anything needed for the operation of the plugin
        
    End Sub

   ''' <inheritdoc />
   ''' <remarks>
   ''' Required override
   ''' </remarks>
    Protected Overrides Sub Initialize()
        Console.WriteLine("Initialized")
        Status = PluginStatus.Ok()
    End Sub

   ''' <inheritdoc />
   ''' <remarks>
   ''' Required override
   ''' </remarks>
    Protected Overrides Function OnSettingChange(pageId As String, currentView As AbstractView, changedView As AbstractView) As Boolean
        Return True
    End Function

    ''' <inheritdoc />
    ''' <remarks>
    ''' Required override
    ''' </remarks>
    Protected Overrides Sub BeforeReturnStatus()
    End Sub
   
End Class