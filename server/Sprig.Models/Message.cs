namespace Sprig.Models;

using MessagePack;
using static Sprig.Models.Message;

public class Message(Kind kind, int protocolVersion = 1)
{
    [Key(0)]
    public int ProtocolVersion { get; } = protocolVersion;

    public enum Kind
    {
        Unknown,
        BeginSessionRequestMessage,
    }

    [Key(1)]
    Kind MessageKind { get; } = kind;
}
