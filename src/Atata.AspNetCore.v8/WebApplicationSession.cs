namespace Atata.AspNetCore;

public class WebApplicationSession : AtataSession
{
    private readonly Lazy<FakeLogCollector?> _fakeLogCollector;

    public WebApplicationSession() =>
        _fakeLogCollector = new(ResolveFakeLogCollector);

    public TestServer Server { get; internal set; } = null!;

    public IServiceProvider Services { get; internal set; } = null!;

    public WebApplicationFactoryClientOptions ClientOptions { get; internal set; } = null!;

    internal Action<WebApplicationSession> StartAction { get; set; } = null!;

    internal Func<HttpClient> CreateClientFunction { get; set; } = null!;

    internal Func<WebApplicationFactoryClientOptions, HttpClient> CreateClientWithOptionsFunction { get; set; } = null!;

    internal Func<DelegatingHandler[], HttpClient> CreateDefaultClientFunction { get; set; } = null!;

    internal Func<Uri, DelegatingHandler[], HttpClient> CreateDefaultClientWithBaseAddressFunction { get; set; } = null!;

    internal IAsyncDisposable WebApplicationFactoryToDispose { get; set; } = null!;

    public FakeLogCollector FakeLogCollector =>
        _fakeLogCollector.Value!;

    public HttpClient CreateClient() =>
        CreateClientFunction.Invoke();

    public HttpClient CreateClient(WebApplicationFactoryClientOptions options) =>
        CreateClientWithOptionsFunction.Invoke(options);

    public HttpClient CreateDefaultClient(params DelegatingHandler[] handlers) =>
        CreateDefaultClientFunction.Invoke(handlers);

    public HttpClient CreateDefaultClient(Uri baseAddress, params DelegatingHandler[] handlers) =>
        CreateDefaultClientWithBaseAddressFunction.Invoke(baseAddress, handlers);

    protected override Task StartAsync(CancellationToken cancellationToken) =>
        Task.Run(() => StartAction.Invoke(this), cancellationToken);

    protected internal virtual void ConfigureWebHost(IWebHostBuilder builder)
    {
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await base.DisposeAsyncCore()
            .ConfigureAwait(false);

        if (WebApplicationFactoryToDispose is not null)
            await WebApplicationFactoryToDispose.DisposeAsync()
                .ConfigureAwait(false);
    }

    private FakeLogCollector? ResolveFakeLogCollector() =>
        Services.GetService<FakeLogCollector>();
}
