namespace Sprig;
using System.Net.Sockets;
using MessagePack;
using Sprig.Models;

class Client
{
    private readonly TcpClient m_tcpClient;

    public Client(string hostname, int port)
    {
        m_tcpClient = new TcpClient(hostname, port);
    }

    public async Task Send(IMessage message, CancellationToken cancellationToken)
    {
        var bytes = MessagePackSerializer.Serialize(message);
        using var stream = m_tcpClient.GetStream();
        await stream.WriteAsync(bytes, cancellationToken);
    }
}
