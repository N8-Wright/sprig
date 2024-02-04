using System.Diagnostics.CodeAnalysis;

namespace Sprig.Core.Messages;

public class HandshakeResponse(ResponseCode code, int version) : Response(code, MessageKind.HandshakeResponse), IEqualityComparer<HandshakeResponse>
{
    public int ProtocolVersion = version;

    /// <summary>
    /// Creates a successful handshake response.
    /// </summary>
    /// <param name="desiredVersion">The protocol version that will be used.</param>
    /// <returns></returns>
    public static HandshakeResponse Accept(int desiredVersion)
    {
        return new HandshakeResponse(ResponseCode.Ok, desiredVersion);
    }

    /// <summary>
    /// Creates a <see cref="HandshakeResponse"/> that rejects the handshake request and
    /// denotes the minimum supported protocol version to use.
    /// </summary>
    /// <param name="minimumSupportedProtocol">The minimum version of the protocol that is supported.</param>
    /// <returns></returns>
    public static HandshakeResponse Reject(int minimumSupportedProtocol)
    {
        return new HandshakeResponse(ResponseCode.Invalid, minimumSupportedProtocol);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        return Equals(this, obj as HandshakeResponse);
    }

    public bool Equals(HandshakeResponse? x, HandshakeResponse? y)
    {
        if (x is null && y is null)
        {
            return true;
        }
        else if (x is not null && y is not null)
        {
            return x.Kind == y.Kind && x.Code == y.Code && x.ProtocolVersion == y.ProtocolVersion;
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return GetHashCode(this);
    }

    public int GetHashCode([DisallowNull] HandshakeResponse obj)
    {
        return HashCode.Combine(obj.Kind, obj.Code, obj.ProtocolVersion);
    }
}
