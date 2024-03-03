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
                var stream = handler.GetStream();
                var reader = new MessagePackStreamReader(stream);
                var messageBytes = await reader.ReadAsync(cancellationToken);
                if (messageBytes.HasValue)
                {
                    var message = MessagePackSerializer.Deserialize<Message>(messageBytes.Value, cancellationToken: cancellationToken);
                    Console.WriteLine($"Received message on thread ${Environment.CurrentManagedThreadId}, {MessagePackSerializer.ConvertToJson(messageBytes.Value, cancellationToken: cancellationToken)}");
                    switch (message)
                    {
                        case BeginSessionRequest beginSessionRequest:
                            {
                                Message response;
                                if (beginSessionRequest.ProtocolVersion == Message.CurrentProtocolVersion)
                                {
                                    response = new Response(Response.Code.Ok);
                                }
                                else
                                {
                                    response = new Response(Response.Code.InvalidRequest);
                                }

                                await stream.WriteAsync(MessagePackSerializer.Serialize(response, cancellationToken: cancellationToken), cancellationToken);
                            }
                            break;
                    }
                }
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
    }
}
