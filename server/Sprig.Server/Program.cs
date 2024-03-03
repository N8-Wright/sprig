// See https://aka.ms/new-console-template for more information
using Sprig;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("Canceling...");
    cts.Cancel();
    e.Cancel = true;
};

var server = new Server();
await server.Run(cts.Token);
