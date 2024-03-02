namespace Sprig.Models;

using MessagePack;

[MessagePackObject]
public class BeginSessionRequest : Message
{
    public BeginSessionRequest() : base(Kind.BeginSessionRequestMessage)
    {
    }
}
