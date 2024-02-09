using System.Net;
using System.Net.Sockets;
using Sprig.Core;
using Sprig.Core.Messages;

namespace Sprig.Client;

class Client
{
    private readonly TcpClient _client;
    public Client(string url, int port)
    {
        _client = new TcpClient(url, port);
    }

    public async Task SendAsync(Message message, CancellationToken cancellationToken)
    {
        var buffer = Serializer.Serialize(message);
        var networkStream = _client.GetStream();
        await networkStream.WriteAsync(buffer, cancellationToken);
    }

    public async Task<Message> ReceiveAsync(CancellationToken cancellationToken)
    {
        var networkStream = _client.GetStream();
        return await MessageStream.ReadAsync(networkStream, cancellationToken);
    }
}
