namespace Sprig.Core.Messages;

public class HandshakeRequest(uint desiredProtocolVersion) : Message(MessageKind.HandshakeRequest)
{
    /// <summary>
    /// Indicates the protocol version a client wants to communicate with.
    /// </summary>
    public uint DesiredProtocolVersion = desiredProtocolVersion;
}