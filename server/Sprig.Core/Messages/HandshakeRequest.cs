using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace Sprig.Core.Messages;

public class HandshakeRequest(int desiredProtocolVersion) : Message(MessageKind.HandshakeRequest), IEqualityComparer<HandshakeRequest>
{
    /// <summary>
    /// Indicates the protocol version a client wants to communicate with.
    /// </summary>
    public int DesiredProtocolVersion = desiredProtocolVersion;

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        return Equals(this, obj as HandshakeRequest);
    }

    public bool Equals(HandshakeRequest? x, HandshakeRequest? y)
    {
        if (x is null && y is null)
        {
            return true;
        }
        else if (x is not null && y is not null)
        {
            return x.Kind == y.Kind && x.DesiredProtocolVersion == y.DesiredProtocolVersion;
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

    public int GetHashCode([DisallowNull] HandshakeRequest obj)
    {
        return HashCode.Combine(obj.Kind, obj.DesiredProtocolVersion);
    }
}
