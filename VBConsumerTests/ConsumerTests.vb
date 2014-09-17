Imports System.Collections.Immutable
Imports ImmutableObjectGraph
Imports Xunit
Imports ImmutableObjectGraph.Tests

Public Class ConsumerTests
    <Fact>
    Public Sub ConsumerTest1()
        Dim xe = XmlElement.Create("TagName", [Optional](Of String).Missing, [ImmutableList].Create(Of XmlNode)(XmlAttribute.Create("Att1", [Optional](Of String).Missing, [Optional](Of String).Missing), XmlElement.Create("ChildElement", [Optional](Of String).Missing, [Optional](Of [ImmutableList](Of XmlNode)).Missing)))
        
        Assert.Equal("TagName", xe.LocalName)
        Dim builder = xe.ToBuilder()
        builder.LocalName = "TagName2"
        Dim xe2 = builder.ToImmutable()
        Assert.Equal("TagName2", xe2.LocalName)
    End Sub
End Class
