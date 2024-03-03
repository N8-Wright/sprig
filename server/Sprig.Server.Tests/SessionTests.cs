namespace Sprig.Server.Tests;
using Sprig;
using Sprig.Models;

public class SessionTests : IDisposable
{
    private readonly Client m_client;
    private readonly Server m_server;
    private readonly CancellationTokenSource m_tokenSource;
    private readonly Task m_serverRun;

    public SessionTests()
    {
        m_server = new Server(port: 9999);
        m_tokenSource = new CancellationTokenSource();
        m_tokenSource.CancelAfter(TimeSpan.FromSeconds(5));
        m_serverRun = m_server.Run(m_tokenSource.Token);
        m_client = new Client("localhost", 9999);

    }

    public void Dispose()
    {
        m_tokenSource.Cancel();
        m_serverRun.Wait();
        m_tokenSource.Dispose();
        m_client.Dispose();
        m_server.Dispose();
    }

    [Fact]
    public async Task BeginSession_CurrentProtocolVersion_OkResponse()
    {
        await m_client.Send(new BeginSessionRequest(), m_tokenSource.Token);
        var message = await m_client.Receive(m_tokenSource.Token);
        if (message is Response response)
        {
            Assert.Equal(Response.Code.Ok, response.ResponseCode);
        }
        else
        {
            Assert.Fail("Message received was not a response");
        }
    }

    [Fact]
    public async Task BeginSession_InvalidProtocolVersion_InvalidResponse()
    {
        var beginSessionRequest = new BeginSessionRequest
        {
            ProtocolVersion = int.MinValue
        };

        await m_client.Send(beginSessionRequest, m_tokenSource.Token);
        var message = await m_client.Receive(m_tokenSource.Token);
        if (message is Response response)
        {
            Assert.Equal(Response.Code.InvalidRequest, response.ResponseCode);
        }
        else
        {
            Assert.Fail("Message received was not a response");
        }
    }
}
