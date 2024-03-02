namespace Sprig.Models;

using MessagePack;

[MessagePackObject]
public class BeginSessionRequest : IMessage
{
    [Key(0)]
    public int ProtocolVersion => 1;
}
