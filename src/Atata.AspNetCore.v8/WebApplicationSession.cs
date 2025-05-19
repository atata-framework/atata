namespace Atata.AspNetCore;

/// <summary>
/// Represents a session for a web application.
/// Creates and launches an application using <see cref="WebApplicationFactory{TStartup}"/>.
/// </summary>
public class WebApplicationSession : AtataSession
{
    private readonly Lazy<FakeLogCollector?> _fakeLogCollector;

    public WebApplicationSession() =>
        _fakeLogCollector = new(ResolveFakeLogCollector);

    /// <summary>
    /// Gets the <see cref="TestServer"/> instance associated with the session.
    /// </summary>
    public TestServer Server { get; internal set; } = null!;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> for resolving application services.
    /// </summary>
    public IServiceProvider Services { get; internal set; } = null!;

    /// <summary>
    /// Gets the client options used for HTTP client creation.
    /// </summary>
    public WebApplicationFactoryClientOptions ClientOptions { get; internal set; } = null!;

    internal Action<WebApplicationSession> StartAction { get; set; } = null!;

    internal Func<HttpClient> CreateClientFunction { get; set; } = null!;

    internal Func<WebApplicationFactoryClientOptions, HttpClient> CreateClientWithOptionsFunction { get; set; } = null!;

    internal Func<DelegatingHandler[], HttpClient> CreateDefaultClientFunction { get; set; } = null!;

    internal Func<Uri, DelegatingHandler[], HttpClient> CreateDefaultClientWithBaseAddressFunction { get; set; } = null!;

    internal IAsyncDisposable WebApplicationFactoryToDispose { get; set; } = null!;

    /// <summary>
    /// Gets the <see cref="Microsoft.Extensions.Logging.Testing.FakeLogCollector"/> of application.
    /// </summary>
    public FakeLogCollector FakeLogCollector =>
        _fakeLogCollector.Value!;

    /// <inheritdoc cref="WebApplicationFactory{TEntryPoint}.CreateClient()"/>
    public HttpClient CreateClient() =>
        CreateClientFunction.Invoke();

    /// <inheritdoc cref="WebApplicationFactory{TEntryPoint}.CreateClient(WebApplicationFactoryClientOptions)"/>
    public HttpClient CreateClient(WebApplicationFactoryClientOptions options) =>
        CreateClientWithOptionsFunction.Invoke(options);

    /// <inheritdoc cref="WebApplicationFactory{TEntryPoint}.CreateDefaultClient(DelegatingHandler[])"/>
    public HttpClient CreateDefaultClient(params DelegatingHandler[] handlers) =>
        CreateDefaultClientFunction.Invoke(handlers);

    /// <inheritdoc cref="WebApplicationFactory{TEntryPoint}.CreateDefaultClient(Uri, DelegatingHandler[])"/>
    public HttpClient CreateDefaultClient(Uri baseAddress, params DelegatingHandler[] handlers) =>
        CreateDefaultClientWithBaseAddressFunction.Invoke(baseAddress, handlers);

    /// <inheritdoc/>
    protected override async Task StartAsync(CancellationToken cancellationToken) =>
        await Task.Run(DoStart, cancellationToken)
            .ConfigureAwait(false);

    /// <summary>
    /// Configures the web host builder for the session.
    /// </summary>
    /// <param name="builder">The web host builder to configure.</param>
    protected internal virtual void ConfigureWebHost(IWebHostBuilder builder)
    {
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeAsyncCore()
    {
        await base.DisposeAsyncCore()
            .ConfigureAwait(false);

        if (WebApplicationFactoryToDispose is not null)
            await WebApplicationFactoryToDispose.DisposeAsync()
                .ConfigureAwait(false);
    }

    private void DoStart() =>
        StartAction.Invoke(this);

    private FakeLogCollector? ResolveFakeLogCollector() =>
        Services.GetService<FakeLogCollector>();
}
