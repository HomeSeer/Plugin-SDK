Imports HomeSeer.PluginSdk.Devices
Imports Newtonsoft.Json

<JsonObject>
Public Class DeviceAddPostData
    
    <JsonProperty("action")>
    Public Action As String
    
    <JsonProperty("device")>
    Public Device As DeviceAddData
    
    <JsonObject>
    Public Class DeviceAddData
        
        <JsonProperty("ref")>
        Public Ref As Integer
        
        <JsonProperty("type")>
        Public Property Type As String
            Get
                Return _type.ToString()
            End Get
            Set
                Dim index As Integer
                If Not Integer.TryParse(Value, index) Then
                    Return
                End If
                
                _type = index
                TypeDescription = Devices.SampleDeviceTypeList(index)
                Features = Devices.SampleDeviceTypeFeatures(index)
                
            End Set

        End Property
        
        <JsonIgnore>
        Private _type As Integer
        
        <JsonProperty("typeDesc")>
        Public TypeDescription As String
        
        <JsonProperty("name")>
        Public Name As String
        
        <JsonProperty("location")>
        Public Location1 As String
        
        <JsonProperty("location2")>
        Public Location2 As String
        
        <JsonProperty("features")>
        Public Features() As String
        
        Public Function BuildDevice(ByVal pluginId As String) As NewDeviceData
            Dim df = DeviceFactory.CreateDevice(pluginId)
            df = df.WithName(Name).WithLocation(Location1).WithLocation2(Location2)

            Select Case _type
                Case 0
                    df = BuildLpSwitch(pluginId, df)
                Case 1
                    df = BuildLpSensor(pluginId, df)
                Case Else
                    Throw New Exception("Cannot create device with this configuration.")
            End Select

            Return df.PrepareForHs()
        End Function

        Private Function BuildLpSwitch(ByVal pluginId As String, ByVal df As DeviceFactory) As DeviceFactory
            Dim ff = FeatureFactory.CreateGenericBinaryControl(pluginId, $"Controls", "On", "Off", 1, 0).WithLocation(Location1).WithLocation2(Location2)
            df.WithFeature(ff)
            Return df
        End Function

        Private Function BuildLpSensor(ByVal pluginId As String, ByVal df As DeviceFactory) As DeviceFactory
            Dim ff = FeatureFactory.CreateGenericBinarySensor(pluginId, $"Sensor State", "Sensor tripped", "No event", 1, 0).WithLocation(Location1).WithLocation2(Location2)
            df.WithFeature(ff)
            Return df
        End Function
        
    End Class
    
End Class