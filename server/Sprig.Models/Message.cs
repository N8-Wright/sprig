using MessagePack;

namespace Sprig.Models;

[Union(0, typeof(BeginSessionRequest))]
[Union(1, typeof(Response))]
[MessagePackObject]
public abstract class Message
{
    public const int CurrentProtocolVersion = 1;

    [Key("ProtocolVersion")]
    public int ProtocolVersion { get; set; } = CurrentProtocolVersion;
}
