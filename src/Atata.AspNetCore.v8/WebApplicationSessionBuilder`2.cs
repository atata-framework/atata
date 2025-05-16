namespace Atata.AspNetCore;

/// <summary>
/// Provides a builder for creating and configuring a <see cref="WebApplicationSession"/>.
/// </summary>
/// <typeparam name="TSession">The type of the session to build, which must inherit from <see cref="WebApplicationSession"/>.</typeparam>
/// <typeparam name="TBuilder">The type of the inherited builder.</typeparam>
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

    /// <summary>
    /// Gets or sets a value indicating whether to collect application logs.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool CollectApplicationLogs { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to transmit application logs to Atata.
    /// The default value is <see langword="true"/>.
    /// When <see langword="true"/>, adds an <see cref="AtataLoggerProvider"/> instance,
    /// with <see cref="SourceNameForAtataLog"/> as Atata log source,
    /// to the application builder's <see cref="ILoggingBuilder"/>.
    /// </summary>
    public bool TransmitApplicationLogsToAtata { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to clear predefined application logging providers.
    /// The default value is <see langword="true"/>.
    /// When <see langword="true"/>, calls <c>ClearProviders</c> method for application builder's <see cref="ILoggingBuilder"/>.
    /// </summary>
    public bool ClearPredefinedApplicationLoggingProviders { get; set; } = true;

    /// <summary>
    /// Gets or sets the source name for Atata log.
    /// The default value is <c>"App"</c>.
    /// </summary>
    public string SourceNameForAtataLog { get; set; } = "App";

    /// <summary>
    /// Gets or sets the default application log level.
    /// The default value is <see langword="null"/>, which doesn't change the predefined application configuration.
    /// When a value is set, adds <c>Logging:LogLevel:Default</c> to the application configuration.
    /// </summary>
    public MSLogLevel? DefaultApplicationLogLevel { get; set; }

    /// <summary>
    /// Gets or sets the minimum application log level.
    /// The default value is <see langword="null"/>, which doesn't change the predefined application configuration.
    /// When a value is set, calls <c>SetMinimumLevel</c> method for application builder's <see cref="ILoggingBuilder"/>.
    /// </summary>
    public MSLogLevel? MinimumApplicationLogLevel { get; set; }

    /// <summary>
    /// Gets or sets the minimum log level for Atata log.
    /// </summary>
    public MSLogLevel MinimumLogLevelForAtataLog { get; set; }

    /// <summary>
    /// Sets the <see cref="WebApplicationFactory{TEntryPoint}"/> to use for the session.
    /// </summary>
    /// <typeparam name="TEntryPoint">The entry point type of the application.</typeparam>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder Use<TEntryPoint>()
        where TEntryPoint : class
        =>
        Use(() => new WebApplicationFactory<TEntryPoint>());

    /// <summary>
    /// Sets the specified <see cref="WebApplicationFactory{TEntryPoint}"/> instance to use for the session.
    /// </summary>
    /// <typeparam name="TEntryPoint">The entry point type of the application.</typeparam>
    /// <param name="webApplicationFactory">The web application factory instance.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder Use<TEntryPoint>(WebApplicationFactory<TEntryPoint> webApplicationFactory)
        where TEntryPoint : class
    {
        Guard.ThrowIfNull(webApplicationFactory);

        return Use(() => webApplicationFactory);
    }

    /// <summary>
    /// Configures and sets a new <see cref="WebApplicationFactory{TEntryPoint}"/> instance to use for the session.
    /// </summary>
    /// <typeparam name="TEntryPoint">The entry point type of the application.</typeparam>
    /// <param name="configure">The action to configure the web application factory.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder Use<TEntryPoint>(Action<WebApplicationFactory<TEntryPoint>> configure)
        where TEntryPoint : class
        =>
        Use(() =>
        {
            WebApplicationFactory<TEntryPoint> webApplicationFactory = new();
            configure?.Invoke(webApplicationFactory);
            return webApplicationFactory;
        });

    /// <summary>
    /// Sets the <see cref="WebApplicationFactory{TEntryPoint}"/> to use for the session using a factory method.
    /// </summary>
    /// <typeparam name="TEntryPoint">The entry point type of the application.</typeparam>
    /// <param name="webApplicationFactoryCreator">The function to create the web application factory.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
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

    /// <summary>
    /// Adds a configuration action to be applied to the <see cref="IWebHostBuilder"/>.
    /// </summary>
    /// <param name="configure">The configuration action.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
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
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseDisposeWebApplicationFactory(bool disposeFactory)
    {
        DisposeWebApplicationFactory = disposeFactory;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets a value indicating whether to collect application logs.
    /// The default value is <see langword="true"/>.
    /// </summary>
    /// <param name="collectLogs">Whether to collect logs.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseCollectApplicationLogs(bool collectLogs)
    {
        CollectApplicationLogs = collectLogs;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets a value indicating whether to transmit application logs to Atata.
    /// The default value is <see langword="true"/>.
    /// When <see langword="true"/>, adds an <see cref="AtataLoggerProvider"/> instance,
    /// with <see cref="SourceNameForAtataLog"/> as Atata log source,
    /// to the application builder's <see cref="ILoggingBuilder"/>.
    /// </summary>
    /// <param name="transmitLogs">Whether to transmit logs to Atata.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseTransmitApplicationLogsToAtata(bool transmitLogs)
    {
        TransmitApplicationLogsToAtata = transmitLogs;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets a value indicating whether to clear predefined application logging providers.
    /// The default value is <see langword="true"/>.
    /// When <see langword="true"/>, calls <c>ClearProviders</c> method for application builder's <see cref="ILoggingBuilder"/>.
    /// </summary>
    /// <param name="clearProviders">Whether to clear providers.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseClearPredefinedApplicationLoggingProviders(bool clearProviders)
    {
        ClearPredefinedApplicationLoggingProviders = clearProviders;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the source name for Atata log.
    /// The default value is <c>"App"</c>.
    /// </summary>
    /// <param name="sourceName">The source name.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseSourceNameForAtataLog(string sourceName)
    {
        SourceNameForAtataLog = sourceName;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the default application log level.
    /// The default value is <see langword="null"/>, which doesn't change the predefined application configuration.
    /// When a value is set, adds <c>Logging:LogLevel:Default</c> to the application configuration.
    /// </summary>
    /// <param name="logLevel">The log level.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseDefaultApplicationLogLevel(MSLogLevel? logLevel)
    {
        DefaultApplicationLogLevel = logLevel;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the minimum application log level.
    /// The default value is <see langword="null"/>, which doesn't change the predefined application configuration.
    /// When a value is set, calls <c>SetMinimumLevel</c> method for application builder's <see cref="ILoggingBuilder"/>.
    /// </summary>
    /// <param name="logLevel">The log level.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseMinimumApplicationLogLevel(MSLogLevel? logLevel)
    {
        MinimumApplicationLogLevel = logLevel;
        return (TBuilder)this;
    }

    /// <summary>
    /// Sets the minimum log level for Atata log.
    /// </summary>
    /// <param name="logLevel">The log level.</param>
    /// <returns>The same <typeparamref name="TBuilder"/> instance.</returns>
    public TBuilder UseMinimumLogLevelForAtataLog(MSLogLevel logLevel)
    {
        MinimumLogLevelForAtataLog = logLevel;
        return (TBuilder)this;
    }

    /// <inheritdoc/>
    protected override void ValidateConfiguration()
    {
        base.ValidateConfiguration();

        if (_sessionStartAction is null)
            throw new AtataSessionBuilderValidationException("WebApplicationFactory is not set. Use 'Use' method to set it.");
    }

    /// <inheritdoc/>
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
