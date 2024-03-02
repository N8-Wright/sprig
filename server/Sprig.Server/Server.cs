namespace Sprig;

using System.Net;
using System.Net.Sockets;
using MessagePack;
using Sprig.Models;

class Server
{
    private readonly TcpListener m_tcpListener;

    public Server(IPAddress? address = null, int port = 8989)
    {
        address ??= IPAddress.Any;
        m_tcpListener = new TcpListener(address, port);
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        m_tcpListener.Start();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using TcpClient handler = await m_tcpListener.AcceptTcpClientAsync(cancellationToken);
                await using NetworkStream stream = handler.GetStream();
                using var reader = new MessagePackStreamReader(stream);
                var messageBytes = await reader.ReadAsync(cancellationToken);
                if (messageBytes.HasValue)
                {
                    var message = MessagePackSerializer.Deserialize<Message>(messageBytes.Value, cancellationToken: cancellationToken);
                    Console.WriteLine($"Received message, {message}");
                }
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
    }
}
