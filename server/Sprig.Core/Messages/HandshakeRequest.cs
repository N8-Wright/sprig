namespace Sprig.Core.Messages;

public class HandshakeRequest(int desiredProtocolVersion) : Message(MessageKind.HandshakeRequest)
{
    /// <summary>
    /// Indicates the protocol version a client wants to communicate with.
    /// </summary>
    public int DesiredProtocolVersion = desiredProtocolVersion;
}
