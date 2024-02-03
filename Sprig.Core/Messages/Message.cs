namespace Sprig.Core.Messages;

public struct Message
{
    /// <summary>
    /// The kind of message being send/received.
    /// </summary>
    public MessageKind Kind;

    /// <summary>
    /// The size of the message in bytes (including the size of <see cref="Kind"/> and <see cref="Size"/> )
    /// </summary>
    public uint Size;
}