Imports HomeSeer.PluginSdk
Imports HomeSeer.PluginSdk.Devices
Imports HomeSeer.PluginSdk.Devices.Controls
Imports HomeSeer.PluginSdk.Devices.Identification
Imports Newtonsoft.Json

<JsonObject>
Public Class DeviceAddPostData

    Private Const IMAGES_STATUS_DIR As String = "images/HomeSeer/status/"
    Private Const EPSILON As Double = 0.0001

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

        Public Function BuildDevice(ByVal pluginId As String, ByVal hs As IHsController) As NewDeviceData
            Dim df = DeviceFactory.CreateDevice(pluginId)
            df = df.WithName(Name).WithLocation(Location1).WithLocation2(Location2)

            Select Case _type
                Case 0
                    df = BuildLpSwitch(pluginId, df)
                Case 1
                    df = BuildLpSensor(pluginId, df)
                Case 2
                    df = BuildBpSensor(pluginId, df)
                Case 3
                    df = BuildThermostat(pluginId, df, hs)
                Case Else
                    Throw New Exception("Cannot create device with this configuration.")
            End Select

            Return df.PrepareForHs()
        End Function

        Private Function BuildLpSwitch(ByVal pluginId As String, ByVal df As DeviceFactory) As DeviceFactory
            Dim ff = FeatureFactory.CreateGenericBinaryControl(pluginId, $"Controls", "On", "Off", 1, 0).
                WithLocation(Location1).WithLocation2(Location2).WithDisplayType(EFeatureDisplayType.Important)
            df.WithFeature(ff)
            Return df
        End Function

        Private Function BuildLpSensor(ByVal pluginId As String, ByVal df As DeviceFactory) As DeviceFactory
            Dim ff = FeatureFactory.CreateGenericBinarySensor(pluginId, $"Sensor State", "Sensor tripped", "No event", 1, 0).
                WithLocation(Location1).WithLocation2(Location2).WithDisplayType(EFeatureDisplayType.Important)
            df.WithFeature(ff)
            Return df
        End Function

        Private Function BuildBpSensor(ByVal pluginId As String, ByVal df As DeviceFactory) As DeviceFactory
            Dim ff = FeatureFactory.CreateGenericBinarySensor(pluginId, $"Sensor State", "Sensor tripped", "No event", 1, 0).
                WithLocation(Location1).WithLocation2(Location2).WithDisplayType(EFeatureDisplayType.Important)
            Dim ffbat = FeatureFactory.CreateFeature(pluginId).WithName("Battery").
                AsType(EFeatureType.Generic, EGenericFeatureSubType.Battery).
                WithLocation(Location1).WithLocation2(Location2).WithDefaultValue(95).WithDisplayType(EFeatureDisplayType.Normal)
            Dim range = New ValueRange(0, 100)
            range.Suffix = "%"
            Dim sg = New StatusGraphic("images/HomeSeer/status/battery_100.png", range)
            ffbat.AddGraphic(sg)
            df.WithFeature(ff).WithFeature(ffbat)
            Return df
        End Function

        Private Function BuildThermostat(ByVal pluginId As String, ByVal df As DeviceFactory, ByVal hs As IHsController) As DeviceFactory
            Dim useCelsius As Boolean = Not Convert.ToBoolean(hs.GetINISetting("Settings", "gGlobalTempScaleF", "True", "settings.ini"))
            df.AsType(EDeviceType.Thermostat, 0).
                WithFeature(BuildAmbientTempFeature(pluginId, useCelsius)).
                WithFeature(BuildSetpointFeature(pluginId, False, useCelsius)).
                WithFeature(BuildSetpointFeature(pluginId, True, useCelsius)).
                WithFeature(BuildHvacModeFeature(pluginId)).
                WithFeature(BuildHvacStatusFeature(pluginId)).
                WithFeature(BuildFanModeFeature(pluginId)).
                WithFeature(BuildFanStatusFeature(pluginId)).
                WithFeature(BuildHumidityFeature(pluginId))
            Return df
        End Function

        Private Function BuildSetpointFeature(ByVal pluginId As String, ByVal isCoolSetpoint As Boolean, ByVal useCelsius As Boolean) As FeatureFactory
            Dim ff = FeatureFactory.CreateFeature(pluginId).WithName($"{(If(isCoolSetpoint, "Cooling", "Heating"))} Setpoint").
                WithLocation(Location1).
                WithLocation2(Location2).
                AsType(EFeatureType.ThermostatControl, If(isCoolSetpoint, CInt(EThermostatControlFeatureSubType.CoolingSetPoint), CInt(EThermostatControlFeatureSubType.HeatingSetPoint))).WithDefaultValue(If(useCelsius, 22, 71))
            Dim range = New ValueRange(0, 100) With {
                .DecimalPlaces = 1,
                .Suffix = If(useCelsius, " °C", " °F")
            }
            ff.AddValueDropDown(range, New ControlLocation(1, 1), If(isCoolSetpoint, EControlUse.CoolSetPoint, EControlUse.HeatSetPoint)).
                AddButton(Devices.ThermostatSetpointDecrement, "-", New ControlLocation(1, 2)).
                AddButton(Devices.ThermostatSetpointIncrement, "+", New ControlLocation(1, 3))
            AddTemperatureRangeGraphics(ff, useCelsius)
            Return ff
        End Function

        Private Sub AddTemperatureRangeGraphics(ByVal ff As FeatureFactory, ByVal useCelsius As Boolean)
            Dim sg = New StatusGraphic(IMAGES_STATUS_DIR & "Thermometer-00.png", -200, If(useCelsius, -18, 0))
            sg.TargetRange.DecimalPlaces = 1
            sg.TargetRange.Suffix = If(useCelsius, " °C", " °F")
            ff.AddGraphic(sg)
            sg = New StatusGraphic(IMAGES_STATUS_DIR & "Thermometer-10.png", (If(useCelsius, -18, 0)) + EPSILON, If(useCelsius, -12, 10))
            sg.TargetRange.DecimalPlaces = 1
            sg.TargetRange.Suffix = If(useCelsius, " °C", " °F")
            ff.AddGraphic(sg)
            sg = New StatusGraphic(IMAGES_STATUS_DIR & "Thermometer-20.png", (If(useCelsius, -12, 10)) + EPSILON, If(useCelsius, -7, 20))
            sg.TargetRange.DecimalPlaces = 1
            sg.TargetRange.Suffix = If(useCelsius, " °C", " °F")
            ff.AddGraphic(sg)
            sg = New StatusGraphic(IMAGES_STATUS_DIR & "Thermometer-30.png", (If(useCelsius, -7, 20)) + EPSILON, If(useCelsius, -1, 30))
            sg.TargetRange.DecimalPlaces = 1
            sg.TargetRange.Suffix = If(useCelsius, " °C", " °F")
            ff.AddGraphic(sg)
            sg = New StatusGraphic(IMAGES_STATUS_DIR & "Thermometer-40.png", (If(useCelsius, -1, 30)) + EPSILON, If(useCelsius, 4, 40))
            sg.TargetRange.DecimalPlaces = 1
            sg.TargetRange.Suffix = If(useCelsius, " °C", " °F")
            ff.AddGraphic(sg)
            sg = New StatusGraphic(IMAGES_STATUS_DIR & "Thermometer-50.png", (If(useCelsius, 4, 40)) + EPSILON, If(useCelsius, 10, 50))
            sg.TargetRange.DecimalPlaces = 1
            sg.TargetRange.Suffix = If(useCelsius, " °C", " °F")
            ff.AddGraphic(sg)
            sg = New StatusGraphic(IMAGES_STATUS_DIR & "Thermometer-60.png", (If(useCelsius, 10, 50)) + EPSILON, If(useCelsius, 16, 60))
            sg.TargetRange.DecimalPlaces = 1
            sg.TargetRange.Suffix = If(useCelsius, " °C", " °F")
            ff.AddGraphic(sg)
            sg = New StatusGraphic(IMAGES_STATUS_DIR & "Thermometer-70.png", (If(useCelsius, 16, 60)) + EPSILON, If(useCelsius, 21, 70))
            sg.TargetRange.DecimalPlaces = 1
            sg.TargetRange.Suffix = If(useCelsius, " °C", " °F")
            ff.AddGraphic(sg)
            sg = New StatusGraphic(IMAGES_STATUS_DIR & "Thermometer-80.png", (If(useCelsius, 21, 70)) + EPSILON, If(useCelsius, 27, 80))
            sg.TargetRange.DecimalPlaces = 1
            sg.TargetRange.Suffix = If(useCelsius, " °C", " °F")
            ff.AddGraphic(sg)
            sg = New StatusGraphic(IMAGES_STATUS_DIR & "Thermometer-90.png", (If(useCelsius, 27, 80)) + EPSILON, If(useCelsius, 32, 90))
            sg.TargetRange.DecimalPlaces = 1
            sg.TargetRange.Suffix = If(useCelsius, " °C", " °F")
            ff.AddGraphic(sg)
            sg = New StatusGraphic(IMAGES_STATUS_DIR & "Thermometer-100.png", (If(useCelsius, 32, 90)) + EPSILON, If(useCelsius, 38, 100))
            sg.TargetRange.DecimalPlaces = 1
            sg.TargetRange.Suffix = If(useCelsius, " °C", " °F")
            ff.AddGraphic(sg)
            sg = New StatusGraphic(IMAGES_STATUS_DIR & "Thermometer-110.png", (If(useCelsius, 38, 100)) + EPSILON, 200)
            sg.TargetRange.DecimalPlaces = 1
            sg.TargetRange.Suffix = If(useCelsius, " °C", " °F")
            ff.AddGraphic(sg)
        End Sub

        Private Function BuildAmbientTempFeature(ByVal pluginId As String, ByVal useCelsius As Boolean) As FeatureFactory
            Dim ff = FeatureFactory.CreateFeature(pluginId).
                WithName("Ambient Temperature").
                WithLocation(Location1).
                WithLocation2(Location2).
                AsType(EFeatureType.ThermostatStatus, CInt(EThermostatStatusFeatureSubType.Temperature)).
                WithDefaultValue(If(useCelsius, 22, 71))
            AddTemperatureRangeGraphics(ff, useCelsius)
            Return ff
        End Function

        Private Function BuildHvacModeFeature(ByVal pluginId As String) As FeatureFactory
            Dim ff = FeatureFactory.CreateFeature(pluginId).
                WithName("HVAC Mode").
                WithLocation(Location1).
                WithLocation2(Location2).
                AsType(EFeatureType.ThermostatControl, CInt(EThermostatControlFeatureSubType.ModeSet)).
                AddButton(Devices.ThermostatHvacModeHeat, "Heat", New ControlLocation(1, 1), EControlUse.ThermModeHeat).
                AddButton(Devices.ThermostatHvacModeCool, "Cool", New ControlLocation(1, 2), EControlUse.ThermModeCool).
                AddButton(Devices.ThermostatHvacModeAuto, "Auto", New ControlLocation(1, 3), EControlUse.ThermModeAuto).
                AddButton(Devices.ThermostatHvacModeAuxHeat, "Aux Heat", New ControlLocation(2, 1), EControlUse.ThermModeAuto).
                AddButton(Devices.ThermostatHvacModeOff, "Off", New ControlLocation(2, 2), EControlUse.ThermModeOff).
                AddGraphicForValue(IMAGES_STATUS_DIR & "heating.png", Devices.ThermostatHvacModeHeat).
                AddGraphicForValue(IMAGES_STATUS_DIR & "cooling.png", Devices.ThermostatHvacModeCool).
                AddGraphicForValue(IMAGES_STATUS_DIR & "auto-mode.png", Devices.ThermostatHvacModeAuto).
                AddGraphicForValue(IMAGES_STATUS_DIR & "AuxHeat.png", Devices.ThermostatHvacModeAuxHeat).
                AddGraphicForValue(IMAGES_STATUS_DIR & "off.gif", Devices.ThermostatHvacModeOff).
                WithDefaultValue(Devices.ThermostatHvacModeOff)
            Return ff
        End Function

        Private Function BuildHvacStatusFeature(ByVal pluginId As String) As FeatureFactory
            Dim ff = FeatureFactory.CreateFeature(pluginId).
                WithName("HVAC Status").
                WithLocation(Location1).
                WithLocation2(Location2).
                AsType(EFeatureType.ThermostatStatus, CInt(EThermostatStatusFeatureSubType.OperatingState)).
                AddGraphicForValue(IMAGES_STATUS_DIR & "idle.png", Devices.ThermostatHvacStatusIdle, "Idle").
                AddGraphicForValue(IMAGES_STATUS_DIR & "heating.png", Devices.ThermostatHvacStatusHeating, "Heating").
                AddGraphicForValue(IMAGES_STATUS_DIR & "cooling.png", Devices.ThermostatHvacStatusCooling, "Cooling").
                WithDefaultValue(Devices.ThermostatHvacStatusIdle)
            Return ff
        End Function

        Private Function BuildFanModeFeature(ByVal pluginId As String) As FeatureFactory
            Dim ff = FeatureFactory.CreateFeature(pluginId).
                WithName("Fan Mode").WithLocation(Location1).
                WithLocation2(Location2).AsType(EFeatureType.ThermostatControl, CInt(EThermostatControlFeatureSubType.FanModeSet)).
                AddButton(Devices.ThermostatFanModeAuto, "Auto", New ControlLocation(1, 1), EControlUse.ThermFanAuto).
                AddButton(Devices.ThermostatFanModeOn, "On", New ControlLocation(1, 2), EControlUse.ThermFanOn).
                AddGraphicForValue(IMAGES_STATUS_DIR & "fan-auto.png", Devices.ThermostatFanModeAuto).
                AddGraphicForValue(IMAGES_STATUS_DIR & "fan-on.png", Devices.ThermostatFanModeOn).
                WithDefaultValue(Devices.ThermostatFanModeAuto)
            Return ff
        End Function

        Private Function BuildFanStatusFeature(ByVal pluginId As String) As FeatureFactory
            Dim ff = FeatureFactory.CreateFeature(pluginId).
                WithName("Fan Status").
                WithLocation(Location1).
                WithLocation2(Location2).
                AsType(EFeatureType.ThermostatStatus, CInt(EThermostatStatusFeatureSubType.FanStatus)).
                AddGraphicForValue(IMAGES_STATUS_DIR & "fan-state-off.png", Devices.ThermostatFanStatusOff, "Off").
                AddGraphicForValue(IMAGES_STATUS_DIR & "fan-state-on.png", Devices.ThermostatFanStatusOn, "On").
                WithDefaultValue(Devices.ThermostatFanStatusOff)
            Return ff
        End Function

        Private Function BuildHumidityFeature(ByVal pluginId As String) As FeatureFactory
            Dim range = New ValueRange(0, 100) With {
                .DecimalPlaces = 0,
                .Prefix = "",
                .Suffix = " %"
            }
            Dim sg = New StatusGraphic(IMAGES_STATUS_DIR & "water.gif", range)
            Dim ff = FeatureFactory.CreateFeature(pluginId).
                WithName("Humidity").
                WithLocation(Location1).
                WithLocation2(Location2).
                AsType(EFeatureType.ThermostatStatus, CInt(EThermostatStatusFeatureSubType.Humidity)).
                AddGraphic(sg).
                WithDefaultValue(45)
            Return ff
        End Function
    End Class

End Class