using MessagePack;

namespace Sprig.Models;

[Union(0, typeof(BeginSessionRequest))]
[Union(1, typeof(Response))]
[MessagePackObject]
public abstract class Message
{
    public const int CurrentProtocolVersion = 1;

    [Key(0)]
    public int ProtocolVersion { get; set; } = CurrentProtocolVersion;
}
