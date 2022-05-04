Module Program
    
    ''' <summary>
    ''' The instance of the plugin class
    ''' </summary>
    Dim _plugin As HSPI
    
    Sub Main(args As String())
        
        'Create a new instance of the plugin class
        _plugin = New HSPI()
        
        'Perform any initialization that needs to occur before a connection is made to HomeSeer here or in the
        ' constructor for HSPI
        
        'Attempt to connect to HomeSeer
        _plugin.Connect(args)
        
    End Sub
End Module
