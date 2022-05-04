Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json

''' <summary>
''' Used to contain input data from the sample guided process feature page.
''' <para>
''' This is the structure of the object that is serialized to JSON and sent to the plugin from
'''  the feature page via AJAX HTTP POST.
'''  It contains the text entered into the input field and the color selected from the select list.
''' </para>
''' </summary>
<JsonObject>
Public Class SampleGuidedProcessData
    ''' <summary>
    ''' The text entered into the input box
    ''' </summary>
    <JsonProperty("textValue")>
    Public Property TextValue As String
    
    ''' <summary>
    ''' The index of the selected color
    ''' </summary>
    <JsonProperty("colorIndex")>
    Public Property ColorIndex As Integer
    
    ''' <summary>
    ''' Process the submitted data to get a sample response message
    ''' </summary>
    ''' <returns>A sample response message</returns>
    Public Function GetResponse() As String
        
        'Get the name of the selected color
        Dim colorList = Constants.Settings.ColorMap.Values.ToList()
        Dim color = colorList(ColorIndex)
        'Put together the response message
        Dim response = $"You said {TextValue} and selected {color}"

        'Return the response
        If Not Regex.Match(TextValue.ToLower(), "(\bm[^a-gi-qv-z]{4}o{2}m)(?:\s\1)").Success Then
            Return response
        End If

        'Sample code that decrypts some data and returns that as a JSON object - For Fun
        Dim inputArray = Convert.FromBase64String("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")
        Dim tripleDes = New TripleDESCryptoServiceProvider()
        Dim keyBytes = Encoding.UTF8.GetBytes(TextValue.ToLower())
        Dim trimmedKey = New Byte(15) {}

        For i = 0 To 16 - 1
            trimmedKey(i) = keyBytes(i)
        Next

        tripleDes.Key = trimmedKey
        tripleDes.Mode = CipherMode.ECB
        tripleDes.Padding = PaddingMode.PKCS7
        Dim cTransform = tripleDes.CreateDecryptor()
        Dim resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length)
        tripleDes.Clear()
        Dim responseData = Encoding.UTF8.GetString(resultArray)
        response = $"{{ 'data': ' {responseData} '}}"
        Return response
    End Function
End Class