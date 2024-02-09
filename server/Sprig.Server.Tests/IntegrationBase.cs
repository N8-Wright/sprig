namespace Sprig.Server.Tests;
using Sprig.Client;
using Sprig.Server;

public class IntegrationBase : IDisposable
{
    private readonly CancellationTokenSource _tokenSource;
    private readonly Task _serverTask;
    private bool _disposed;
    public IntegrationBase(int port)
    {
        _tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        Server = new Server(port);
        _serverTask = Server.RunAsync(_tokenSource.Token);

        Client = new Client("localhost", port);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Client Client { get; }
    public Server Server { get; }

    /// <summary>
    /// A cancellation token that tests should honor so that they can be cancelled if taking too long.
    /// </summary>
    public CancellationToken Cancellation { get; }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }

        _disposed = true;
    }
}
