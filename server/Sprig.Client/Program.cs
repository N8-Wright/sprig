using Sprig;
using Sprig.Models;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("Canceling...");
    cts.Cancel();
    e.Cancel = true;
};

using var client = new Client("localhost", 8989);
await client.Send(new BeginSessionRequest(), cts.Token);
var message = await client.Receive(cts.Token);
switch (message)
{
    case Response response:
        Console.WriteLine($"Received response of {response.ResponseCode}");
        break;
    default:
        Console.WriteLine("Received unknown message");
        break;
}
