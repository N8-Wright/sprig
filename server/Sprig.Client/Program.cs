using Sprig;
using Sprig.Models;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("Canceling...");
    cts.Cancel();
    e.Cancel = true;
};

var client = new Client("localhost", 8989);
await client.Send(new BeginSessionRequest(), cts.Token);
