namespace Sprig.Core.Messages;

public class HandshakeResponse : Response
{
    public int ProtocolVersion;

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

    private HandshakeResponse(ResponseCode code, int version)
        : base(code, MessageKind.HandshakeResponse)
    {
        ProtocolVersion = version;
    }
}
