namespace Atata.AspNetCore;

public abstract class WebApplicationSessionBuilder<TSession, TBuilder> : AtataSessionBuilder<TSession, TBuilder>
    where TSession : WebApplicationSession, new()
    where TBuilder : WebApplicationSessionBuilder<TSession, TBuilder>
{
    private readonly List<Action<IWebHostBuilder>> _webHostConfigurationActions = [];

    private Action<WebApplicationSession>? _sessionStartAction;

    /// <summary>
    /// Gets or sets a value indicating whether to dispose the <see cref="WebApplicationFactory{TEntryPoint}"/>
    /// when <see cref="AtataSession.DisposeAsync()"/> method is invoked.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool DisposeWebApplicationFactory { get; set; } = true;

    public bool CollectApplicationLogs { get; set; } = true;

    public bool TransmitApplicationLogsToAtata { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to clear predefined application logging providers.
    /// The default value is <see langword="true"/>.
    /// When <see langword="true"/>, calls <c>ClearProviders</c> method for application builder's <see cref="ILoggingBuilder"/>.
    /// </summary>
    public bool ClearPredefinedApplicationLoggingProviders { get; set; } = true;

    public string SourceNameForAtataLog { get; set; } = "App";

    public MSLogLevel? DefaultApplicationLogLevel { get; set; }

    /// <summary>
    /// Gets or sets the minimum application log level.
    /// The default value is <see langword="null"/>, which doesn't change the predefined application configuration.
    /// When a value is set, calls <c>SetMinimumLevel</c> method for application builder's <see cref="ILoggingBuilder"/>.
    /// </summary>
    public MSLogLevel? MinimumApplicationLogLevel { get; set; }

    public MSLogLevel MinimumLogLevelForAtataLog { get; set; }

    public TBuilder Use<TEntryPoint>()
        where TEntryPoint : class
        =>
        Use(() => new WebApplicationFactory<TEntryPoint>());

    public TBuilder Use<TEntryPoint>(WebApplicationFactory<TEntryPoint> webApplicationFactory)
        where TEntryPoint : class
    {
        Guard.ThrowIfNull(webApplicationFactory);

        return Use(() => webApplicationFactory);
    }

    public TBuilder Use<TEntryPoint>(Action<WebApplicationFactory<TEntryPoint>> configure)
        where TEntryPoint : class
        =>
        Use(() =>
        {
            WebApplicationFactory<TEntryPoint> webApplicationFactory = new();
            configure?.Invoke(webApplicationFactory);
            return webApplicationFactory;
        });

    public TBuilder Use<TEntryPoint>(Func<WebApplicationFactory<TEntryPoint>> webApplicationFactoryCreator)
        where TEntryPoint : class
    {
        Guard.ThrowIfNull(webApplicationFactoryCreator);

        _sessionStartAction = session =>
        {
            WebApplicationFactory<TEntryPoint> originalWebApplicationFactory = webApplicationFactoryCreator.Invoke();
            WebApplicationFactory<TEntryPoint> webApplicationFactory = originalWebApplicationFactory
                .WithWebHostBuilder(builder => ConfigureWebHost(builder, session));

            session.Server = webApplicationFactory.Server;
            session.Services = webApplicationFactory.Services;
            session.ClientOptions = webApplicationFactory.ClientOptions;
            session.CreateClientFunction = webApplicationFactory.CreateClient;
            session.CreateClientWithOptionsFunction = webApplicationFactory.CreateClient;
            session.CreateDefaultClientFunction = webApplicationFactory.CreateDefaultClient;
            session.CreateDefaultClientWithBaseAddressFunction = webApplicationFactory.CreateDefaultClient;

            if (DisposeWebApplicationFactory)
                session.WebApplicationFactoryToDispose = originalWebApplicationFactory;
        };

        return (TBuilder)this;
    }

    public TBuilder UseConfiguration(Action<IWebHostBuilder> configure)
    {
        Guard.ThrowIfNull(configure);

        _webHostConfigurationActions.Add(configure);
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets a value indicating whether to dispose the <see cref="WebApplicationFactory{TEntryPoint}"/>
    /// when <see cref="AtataSession.DisposeAsync"/> method is invoked.
    /// The default value is <see langword="true"/>.
    /// </summary>
    /// <param name="disposeFactory">Whether to dispose factory.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public TBuilder UseDisposeWebApplicationFactory(bool disposeFactory)
    {
        DisposeWebApplicationFactory = disposeFactory;
        return (TBuilder)this;
    }

    public TBuilder UseCollectApplicationLogs(bool collectLogs)
    {
        CollectApplicationLogs = collectLogs;
        return (TBuilder)this;
    }

    public TBuilder UseTransmitApplicationLogsToAtata(bool transmitLogs)
    {
        TransmitApplicationLogsToAtata = transmitLogs;
        return (TBuilder)this;
    }

    public TBuilder UseClearPredefinedApplicationLoggingProviders(bool clearProviders)
    {
        ClearPredefinedApplicationLoggingProviders = clearProviders;
        return (TBuilder)this;
    }

    public TBuilder UseSourceNameForAtataLog(string sourceName)
    {
        SourceNameForAtataLog = sourceName;
        return (TBuilder)this;
    }

    public TBuilder UseDefaultApplicationLogLevel(MSLogLevel? logLevel)
    {
        DefaultApplicationLogLevel = logLevel;
        return (TBuilder)this;
    }

    public TBuilder UseMinimumApplicationLogLevel(MSLogLevel? logLevel)
    {
        MinimumApplicationLogLevel = logLevel;
        return (TBuilder)this;
    }

    public TBuilder UseMinimumLogLevelForAtataLog(MSLogLevel logLevel)
    {
        MinimumLogLevelForAtataLog = logLevel;
        return (TBuilder)this;
    }

    protected override void ValidateConfiguration()
    {
        base.ValidateConfiguration();

        if (_sessionStartAction is null)
            throw new AtataSessionBuilderValidationException("WebApplicationFactory is not set. Use 'Use' method to set it.");
    }

    protected override void ConfigureSession(TSession session)
    {
        base.ConfigureSession(session);

        session.StartAction = _sessionStartAction!;
    }

    private void ConfigureWebHost(IWebHostBuilder builder, WebApplicationSession session)
    {
        if (DefaultApplicationLogLevel is not null)
        {
            string defaultLoggingLevelValue = DefaultApplicationLogLevel.Value.ToString();

            builder.ConfigureAppConfiguration(x => x
                .AddInMemoryCollection([new KeyValuePair<string, string?>("Logging:LogLevel:Default", defaultLoggingLevelValue)]));
        }

        builder.ConfigureLogging(loggingBuilder =>
        {
            if (ClearPredefinedApplicationLoggingProviders)
                loggingBuilder.ClearProviders();

            if (MinimumApplicationLogLevel is not null)
                loggingBuilder.SetMinimumLevel(MinimumApplicationLogLevel.Value);

            if (TransmitApplicationLogsToAtata)
                loggingBuilder.AddProvider(new AtataLoggerProvider(session, SourceNameForAtataLog));

            if (CollectApplicationLogs)
                loggingBuilder.AddFakeLogging();
        });

        session.ConfigureWebHost(builder);

        foreach (Action<IWebHostBuilder> configurationAction in _webHostConfigurationActions)
            configurationAction.Invoke(builder);
    }
}
