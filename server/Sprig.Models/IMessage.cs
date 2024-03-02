namespace Sprig.Models;

[MessagePack.Union(0, typeof(BeginSessionRequest))]
public interface IMessage
{
    int ProtocolVersion { get; }
}
