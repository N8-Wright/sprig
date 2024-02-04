using System.Net;
using Microsoft.VisualBasic;

namespace Sprig.Core.Messages;

public static class Serializer
{
    public const int MessageSize = sizeof(int);
    public const int ResponseMessageSize = MessageSize + sizeof(int);
    public const int HandshakeRequestSize = MessageSize + sizeof(int);
    public const int HandshakeResponseSize = ResponseMessageSize + sizeof(int);

    public static byte[] Serialize(Message message)
    {
        byte[] serialized = message.Kind switch
        {
            MessageKind.HandshakeRequest => Serialize((HandshakeRequest)message),
            MessageKind.HandshakeResponse => Serialize((HandshakeResponse)message),
            _ => [],
        };
        return serialized;
    }

    public static Message Deserialize(byte[] message)
    {
        var baseMessage = DeserializeBaseMessage(message);
        var restOfMessage = message.AsSpan()[MessageSize..];
        return baseMessage.Kind switch
        {
            MessageKind.HandshakeRequest => DeserializeHandshakeRequest(restOfMessage),
            MessageKind.HandshakeResponse => DeserializeHandshakeResponse(restOfMessage),
            _ => throw new InvalidOperationException("Unable to determine message kind."),
        };
    }

    public static byte[] Serialize(HandshakeRequest message)
    {
        var bytes = new byte[HandshakeRequestSize];
        var offset = SerializeBaseMessage(message, bytes);
        var desiredProtocolVersion = IPAddress.HostToNetworkOrder(message.DesiredProtocolVersion);
        if (!BitConverter.TryWriteBytes(bytes.AsSpan(offset), desiredProtocolVersion))
        {
            throw new InvalidOperationException("Unable to write desired protocol version.");
        }
        return bytes;
    }

    public static HandshakeRequest DeserializeHandshakeRequest(ReadOnlySpan<byte> message)
    {
        var desiredProtocolVersion = BitConverter.ToInt32(message);
        return new HandshakeRequest(IPAddress.NetworkToHostOrder(desiredProtocolVersion));
    }

    public static byte[] Serialize(HandshakeResponse message)
    {
        var bytes = new byte[HandshakeResponseSize];
        var offset = SerializeResponseMessage(message, bytes);
        var protocolVersion = IPAddress.HostToNetworkOrder(message.ProtocolVersion);
        if (!BitConverter.TryWriteBytes(bytes.AsSpan()[offset..], protocolVersion))
        {
            throw new InvalidOperationException("Unable to write protocol version");
        }
        return bytes;
    }

    public static HandshakeResponse DeserializeHandshakeResponse(ReadOnlySpan<byte> message)
    {
        var response = DeserializeResponseMessage(message, MessageKind.HandshakeResponse);
        var offset = ResponseMessageSize - MessageSize;
        var protocolVersion = BitConverter.ToInt32(message[offset..]);
        return new HandshakeResponse(response.Code, IPAddress.NetworkToHostOrder(protocolVersion));
    }

    /// <summary>
    /// Serializes the properties of a <see cref="Response"/> message.
    /// </summary>
    /// <param name="message">The message to serialize.</param>
    /// <param name="buffer">The buffer to write into.</param>
    /// <returns>The number of bytes that were written.</returns>
    /// <exception cref="InvalidOperationException">On failure to write bytes.</exception>
    private static int SerializeResponseMessage(Response message, Span<byte> buffer)
    {
        var offset = SerializeBaseMessage(message, buffer);
        var responseCode = IPAddress.HostToNetworkOrder((int)message.Code);
        if (!BitConverter.TryWriteBytes(buffer[offset..], responseCode))
        {
            throw new InvalidOperationException("Unable to write response code.");
        }
        return ResponseMessageSize;
    }

    private static Response DeserializeResponseMessage(ReadOnlySpan<byte> message, MessageKind kind)
    {
        var responseCode = BitConverter.ToInt32(message);
        var responseCodeEnum = (ResponseCode)IPAddress.NetworkToHostOrder(responseCode);
        return new Response(responseCodeEnum, kind);
    }

    /// <summary>
    /// Serializes the base properties of a message
    /// </summary>
    /// <param name="message">The message to serialize.</param>
    /// <param name="buffer">The buffer to write into.</param>
    /// <returns>The number of bytes that were written.</returns>
    /// <exception cref="InvalidOperationException">On failure to write bytes.</exception>
    private static int SerializeBaseMessage(Message message, Span<byte> buffer)
    {
        int kind = IPAddress.HostToNetworkOrder((int)message.Kind);
        if (!BitConverter.TryWriteBytes(buffer, kind))
        {
            throw new InvalidOperationException("Unable to write kind.");
        }
        return MessageSize;
    }

    /// <summary>
    /// Deserializes a message buffer into the base <see cref="Message"/>.
    /// </summary>
    /// <param name="message">The buffer to deserialize.</param>
    /// <returns>A <see cref="Message"/> instance.</returns>
    private static Message DeserializeBaseMessage(ReadOnlySpan<byte> message)
    {
        var kind = BitConverter.ToInt32(message);
        var kindEnum = (MessageKind)IPAddress.NetworkToHostOrder(kind);
        return new Message(kindEnum);
    }
}
