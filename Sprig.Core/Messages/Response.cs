namespace Sprig.Core.Messages;

public class Response(ResponseCode code, MessageKind kind) : Message(kind)
{
    /// <summary>
    /// The code used to indicate success/failure or other information.
    /// </summary>
    ResponseCode Code = code;
}