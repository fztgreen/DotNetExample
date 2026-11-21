using DotNetExample.DataAccess.Wrappers;

using FluentAssertions;

using RandomTestValues;

namespace DotNetExample.IntegrationTests.Wrappers;

public abstract class HttpClientWrapperContract
{
    private LocalHttpServer _server;

    protected IHttpClientWrapper Wrapper { get; private set; }

    [TestInitialize]
    public async Task StartServerAndCreateWrapper()
    {
        _server = await LocalHttpServer.StartAsync();
        Wrapper = CreateWrapper();
    }

    [TestCleanup]
    public async Task StopServer()
    {
        await _server.DisposeAsync();
    }

    [TestMethod]
    public async Task ReturnTheResponseBody_FromTheExactRequestUri()
    {
        // Arrange
        var pathAndQuery = $"/{RandomValue.Guid()}?{RandomValue.Guid():N}={RandomValue.Guid():N}";

        // Act
        var body = await Wrapper.GetAsync($"{_server.BaseAddress}{pathAndQuery}");

        // Assert
        body.Should().Be(pathAndQuery);
    }

    [TestMethod]
    public async Task Throw_WhenTheResponseIsNotSuccessful()
    {
        // Arrange
        var failingUri = $"{_server.BaseAddress}{LocalHttpServer.FailurePath}";

        // Act
        var requestingFailure = Wrapper.Awaiting(wrapper => wrapper.GetAsync(failingUri));

        // Assert
        await requestingFailure.Should().ThrowAsync<HttpRequestException>();
    }

    protected abstract IHttpClientWrapper CreateWrapper();
}
