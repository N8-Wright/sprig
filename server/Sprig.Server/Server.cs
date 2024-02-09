using System.Net;
using System.Net.Sockets;
using Sprig.Core;
using Sprig.Core.Messages;

namespace Sprig.Server;

class Server
{
    private readonly TcpListener _listener;

    public Server()
    {
        _listener = new TcpListener(IPAddress.Any, 8080);
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _listener.Start();
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var client = await _listener.AcceptTcpClientAsync(cancellationToken);
                if (await HandleHandshake(client, cancellationToken))
                {
                    Console.WriteLine("Received successful handshake request");
                    await ProcessClient(client, cancellationToken);
                }
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _listener.Stop();
        }
    }

    private Task ProcessClient(TcpClient client, CancellationToken cancellationToken)
    {
        var networkStream = client.GetStream();
        return Task.CompletedTask;
    }

    private static async Task<bool> HandleHandshake(TcpClient client, CancellationToken cancellationToken)
    {
        var networkStream = client.GetStream();
        var handshakeRequest = (HandshakeRequest?)await MessageStream.ReadAsync(networkStream, MessageKind.HandshakeRequest, cancellationToken);
        if (handshakeRequest is null)
        {
            return false;
        }

        HandshakeResponse response;
        if (handshakeRequest.DesiredProtocolVersion == 1)
        {
            response = HandshakeResponse.Accept(1);
        }
        else
        {
            response = HandshakeResponse.Reject(1);
        }

        var responseBytes = Serializer.Serialize(response);
        await networkStream.WriteAsync(responseBytes, cancellationToken);
        return true;
    }
}
