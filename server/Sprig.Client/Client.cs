namespace Sprig;
using System.Net.Sockets;
using MessagePack;
using Sprig.Models;

public class Client : IDisposable
{
    private readonly TcpClient m_tcpClient;

    public Client(string hostname, int port)
    {
        m_tcpClient = new TcpClient(hostname, port);
        var stream = m_tcpClient.GetStream();
    }

    public void Dispose()
    {
        m_tcpClient.Dispose();
    }

    public async Task Send(Message message, CancellationToken cancellationToken)
    {
        var bytes = MessagePackSerializer.Serialize(message, cancellationToken: cancellationToken);
        var stream = m_tcpClient.GetStream();
        await stream.WriteAsync(bytes, cancellationToken);
    }

    public async Task<Message> Receive(CancellationToken cancellationToken)
    {
        var stream = m_tcpClient.GetStream();
        var reader = new MessagePackStreamReader(stream);
        var messageBytes = await reader.ReadAsync(cancellationToken);
        if (messageBytes.HasValue)
        {
            Console.WriteLine($"Received message on thread ${Environment.CurrentManagedThreadId}, {MessagePackSerializer.ConvertToJson(messageBytes.Value, cancellationToken: cancellationToken)}");
            return MessagePackSerializer.Deserialize<Message>(messageBytes.Value, cancellationToken: cancellationToken);
        }
        else
        {
            throw new InvalidOperationException("Unable to read message from socket");
        }
    }
}
