namespace Sprig.Core.Tests;

using System.Diagnostics.CodeAnalysis;
using Sprig.Core.Messages;
using Xunit.Sdk;

public class MessageSerializerTests
{
    [Fact]
    public void SerializeHandshakeRequest()
    {
        var req = new HandshakeRequest(123);
        var serialized = Serializer.Serialize(req);
        Assert.True(serialized.Any(b => b != 0), "Serialized bytes should not be all zeros");
        Assert.Contains(serialized, b => b == 123);
    }

    [Fact]
    public void DeserializeHandshakeRequest()
    {
        var req = new HandshakeRequest(123);
        var serialized = Serializer.Serialize(req);

        var deserialized = Serializer.Deserialize(serialized);
        Assert.Equal(req, deserialized);
    }

    [Fact]
    public void SerializeHandshakeResponse()
    {
        var res = HandshakeResponse.Accept(123);
        var serialized = Serializer.Serialize(res);
        Assert.True(serialized.Any(b => b != 0), "Serialized bytes should not be all zeros");
        Assert.Contains(serialized, b => b == 123);
    }

    [Fact]
    public void DeserializeHandshakeResponse()
    {
        var res = HandshakeResponse.Accept(123);
        var serialized = Serializer.Serialize(res);

        var deserialized = Serializer.Deserialize(serialized);
        Assert.Equal(res, deserialized);
    }
}
