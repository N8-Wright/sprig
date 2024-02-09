using Sprig.Server;

using var cancellationTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    cancellationTokenSource.Cancel();
    eventArgs.Cancel = true;
};

var server = new Server(port: 8080);
await server.RunAsync(cancellationTokenSource.Token);
