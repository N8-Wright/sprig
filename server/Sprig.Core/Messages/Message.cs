namespace Sprig.Core.Messages;

public class Message(MessageKind kind)
{
    /// <summary>
    /// The kind of message being send/received.
    /// </summary>
    public MessageKind Kind = kind;
}
