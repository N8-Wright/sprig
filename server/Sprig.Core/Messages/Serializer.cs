using System.Net;
using Microsoft.VisualBasic;

namespace Sprig.Core.Messages;

public static class Serializer
{
    public const int MessageSize = sizeof(int);
    public const int ResponseMessageSize = MessageSize + sizeof(int);
    public const int HandshakeRequestSize = MessageSize + sizeof(int);
    public const int HandshakeResponseSize = ResponseMessageSize + sizeof(int);

    /// <summary>
    /// Serialize a generic <see cref="Message"/>.
    /// </summary>
    /// <param name="message">The message to serialize.</param>
    /// <returns>The serialized bytes.</returns>
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

    /// <summary>
    /// Generic method to deserialize a message buffer.
    /// </summary>
    /// <param name="message">The bytes to deserialize.</param>
    /// <returns>An instance of a <see cref="Message"/>.</returns>
    /// <exception cref="InvalidOperationException">When unable to deserialize the message.</exception>
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

    /// <summary>
    /// Serializes a <see cref="HandshakeRequest" /> message.
    /// </summary>
    /// <param name="message">The message to serialize.</param>
    /// <returns>The serialized bytes.</returns>
    /// <exception cref="InvalidOperationException">On failure to write.</exception>
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

    /// <summary>
    /// Deserialize a <see cref="HandshakeRequest"/> message.
    /// </summary>
    /// <param name="message">The bytes to deserialize.</param>
    /// <returns>An instance of <see cref="HandshakeRequest"/>.</returns>
    public static HandshakeRequest DeserializeHandshakeRequest(ReadOnlySpan<byte> message)
    {
        var desiredProtocolVersion = BitConverter.ToInt32(message);
        return new HandshakeRequest(IPAddress.NetworkToHostOrder(desiredProtocolVersion));
    }

    /// <summary>
    /// Serializes a <see cref="HandshakeResponse"/> message.
    /// </summary>
    /// <param name="message">The message to serialize.</param>
    /// <returns>Serialized bytes of the message.</returns>
    /// <exception cref="InvalidOperationException">On failure to write bytes.</exception>
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

    /// <summary>
    /// Deserialize a <see cref="HandshakeResponse"/> message.
    /// </summary>
    /// <param name="message">The bytes to deserialize.</param>
    /// <returns>An instance of <see cref="HandshakeResponse"/>.</returns>
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

    /// <summary>
    /// Deserialize a <see cref="Response"/> message.
    /// </summary>
    /// <param name="message">The bytes to deserialize.</param>
    /// <param name="kind">The kind of response to create.</param>
    /// <returns>An instance of <see cref="Response"/>.</returns>
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
