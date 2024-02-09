using Sprig.Client;
using Sprig.Core.Messages;

var client = new Client("localhost", 8080);

Console.WriteLine("Sending handshake request");
await client.SendAsync(new HandshakeRequest(1), CancellationToken.None);

var message = await client.ReceiveAsync(CancellationToken.None);
if (message.Kind != MessageKind.HandshakeResponse)
{
    throw new InvalidOperationException();
}

var handshakeResponse = (HandshakeResponse)message;
if (handshakeResponse.Code == ResponseCode.Ok)
{
    Console.WriteLine("Received successful handshake response");
}
else
{
    Console.WriteLine("Received failed handshake response");
}
