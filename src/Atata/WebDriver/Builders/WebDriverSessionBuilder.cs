using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Atata;

/// <summary>
/// Represents a builder for creating and configuring a <see cref="WebDriverSession"/>.
/// </summary>
public class WebDriverSessionBuilder : WebSessionBuilder<WebDriverSession, WebDriverSessionBuilder>
{
    public WebDriverSessionBuilder()
    {
        Screenshots = new(this);
        PageSnapshots = new(this);
        BrowserLogs = new(this);
    }

    /// <summary>
    /// Gets the driver factories.
    /// </summary>
    public List<IWebDriverFactory> DriverFactories { get; private set; } = [];

    /// <summary>
    /// Gets the driver factory to use.
    /// </summary>
    public IWebDriverFactory? DriverFactoryToUse { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether to dispose the <see cref="WebDriverSession.Driver"/>
    /// when <see cref="AtataSession.DisposeAsync()"/> method is invoked.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool DisposeDriver { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether it uses a local browser.
    /// Basically, determines whether <see cref="DriverFactoryToUse"/> is <see cref="IUsesLocalBrowser"/>.
    /// </summary>
    public bool UsesLocalBrowser =>
        DriverFactoryToUse is IUsesLocalBrowser;

    /// <summary>
    /// Gets the name of the local browser to use or <see langword="null"/>.
    /// Returns <see cref="IUsesLocalBrowser.BrowserName"/> value
    /// if <see cref="DriverFactoryToUse"/> is <see cref="IUsesLocalBrowser"/>.
    /// </summary>
    public string? LocalBrowserToUseName =>
        (DriverFactoryToUse as IUsesLocalBrowser)?.BrowserName;

    /// <summary>
    /// Gets the names of local browsers that this instance uses.
    /// Distinctly returns <see cref="IUsesLocalBrowser.BrowserName"/> values of all
    /// <see cref="DriverFactories"/> that are <see cref="IUsesLocalBrowser"/>.
    /// </summary>
    public IEnumerable<string> ConfiguredLocalBrowserNames =>
        DriverFactories.OfType<IUsesLocalBrowser>().Select(x => x.BrowserName).Distinct();

    /// <summary>
    /// Gets or sets the default control visibility.
    /// The default value is <see cref="Visibility.Any"/>.
    /// </summary>
    public Visibility DefaultControlVisibility { get; set; }

    /// <summary>
    /// Gets the screenshots configuration builder.
    /// </summary>
    public ScreenshotsWebDriverSessionBuilder Screenshots { get; private set; }

    /// <summary>
    /// Gets the page snapshots configuration builder.
    /// </summary>
    public PageSnapshotsWebDriverSessionBuilder PageSnapshots { get; private set; }

    /// <summary>
    /// Gets the configuration builder of browser logs monitoring and handling.
    /// </summary>
    public BrowserLogsWebDriverSessionBuilder BrowserLogs { get; private set; }

    /// <summary>
    /// Gets the driver factory by the specified alias.
    /// </summary>
    /// <param name="alias">The alias of the driver factory.</param>
    /// <returns>The driver factory or <see langword="null"/>.</returns>
    public IWebDriverFactory? GetDriverFactory(string alias)
    {
        Guard.ThrowIfNullOrWhitespace(alias);

        return DriverFactories.LastOrDefault(x => alias.Equals(x.Alias, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Configures an existing or creates a new builder for <typeparamref name="TDriverBuilder"/> by the specified alias.
    /// </summary>
    /// <typeparam name="TDriverBuilder">The type of the driver builder.</typeparam>
    /// <param name="alias">The driver alias.</param>
    /// <param name="driverBuilderCreator">The function that creates a driver builder.</param>
    /// <param name="configure">An action delegate to configure the provided <typeparamref name="TDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureDriver<TDriverBuilder>(
        string alias,
        Func<TDriverBuilder> driverBuilderCreator,
        Action<TDriverBuilder>? configure = null)
        where TDriverBuilder : WebDriverBuilder<TDriverBuilder>
    {
        Guard.ThrowIfNullOrWhitespace(alias);
        Guard.ThrowIfNull(driverBuilderCreator);

        IWebDriverFactory? driverFactory = GetDriverFactory(alias);

        if (driverFactory is null)
        {
            driverFactory = driverBuilderCreator.Invoke();
            DriverFactories.Add(driverFactory);
        }
        else if (driverFactory is not TDriverBuilder)
        {
            throw new ArgumentException(
                $"""
                Existing driver with "{alias}" alias has other factory type in {nameof(AtataContextBuilder)}.
                Expected: {typeof(TDriverBuilder).FullName}
                Actual: {driverFactory.GetType().FullName}
                """,
                nameof(alias));
        }

        configure?.Invoke((TDriverBuilder)driverFactory);

        return this;
    }

    /// <summary>
    /// Use the driver builder.
    /// </summary>
    /// <typeparam name="TDriverBuilder">The type of the driver builder.</typeparam>
    /// <param name="configure">An action delegate to configure the provided <typeparamref name="TDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseDriver<TDriverBuilder>(Action<TDriverBuilder>? configure = null)
        where TDriverBuilder : WebDriverBuilder<TDriverBuilder>, new()
        =>
        UseDriver(new TDriverBuilder(), configure);

    /// <summary>
    /// Use the driver builder.
    /// </summary>
    /// <typeparam name="TDriverBuilder">The type of the driver builder.</typeparam>
    /// <param name="driverBuilder">The driver builder.</param>
    /// <param name="configure">An action delegate to configure the provided <typeparamref name="TDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseDriver<TDriverBuilder>(TDriverBuilder driverBuilder, Action<TDriverBuilder>? configure = null)
        where TDriverBuilder : WebDriverBuilder<TDriverBuilder>
    {
        Guard.ThrowIfNull(driverBuilder);

        configure?.Invoke(driverBuilder);

        DriverFactories.Add(driverBuilder);
        DriverFactoryToUse = driverBuilder;

        return this;
    }

    /// <summary>
    /// Sets the driver to use by the specified alias.
    /// </summary>
    /// <param name="alias">The driver alias.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseDriver(string alias)
    {
        Guard.ThrowIfNullOrWhitespace(alias);

        IWebDriverFactory? driverFactory = GetDriverFactory(alias);

        if (driverFactory is not null)
            DriverFactoryToUse = driverFactory;
        else if (!TryUsePredefinedDriver(alias))
            throw new ArgumentException($"No driver with \"{alias}\" alias defined.", nameof(alias));

        return this;
    }

    /// <summary>
    /// Use the specified driver instance.
    /// </summary>
    /// <param name="driver">The driver to use.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="CustomWebDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseDriver(IWebDriver driver, Action<CustomWebDriverBuilder>? configure = null)
    {
        Guard.ThrowIfNull(driver);

        return UseDriver(() => driver, configure);
    }

    /// <summary>
    /// Use the custom driver factory method.
    /// </summary>
    /// <param name="driverFactory">The driver factory method.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="CustomWebDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseDriver(Func<IWebDriver> driverFactory, Action<CustomWebDriverBuilder>? configure = null)
    {
        Guard.ThrowIfNull(driverFactory);

        return UseDriver(new CustomWebDriverBuilder(driverFactory), configure);
    }

    private bool TryUsePredefinedDriver(string alias)
    {
        switch (alias.ToLowerInvariant())
        {
            case WebDriverAliases.Chrome:
                UseChrome();
                return true;
            case WebDriverAliases.Firefox:
                UseFirefox();
                return true;
            case WebDriverAliases.InternetExplorer:
                UseInternetExplorer();
                return true;
            case WebDriverAliases.Safari:
                UseSafari();
                return true;
            case WebDriverAliases.Edge:
                UseEdge();
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Sets a value indicating whether to dispose the <see cref="WebDriverSession.Driver"/>
    /// when <see cref="AtataSession.DisposeAsync"/> method is invoked.
    /// The default value is <see langword="true"/>.
    /// </summary>
    /// <param name="disposeDriver">Whether to dispose driver.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseDisposeDriver(bool disposeDriver)
    {
        DisposeDriver = disposeDriver;
        return this;
    }

    /// <summary>
    /// Creates and configures a new builder for <see cref="ChromeDriver"/>
    /// with default <see cref="WebDriverAliases.Chrome"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="ChromeDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseChrome(Action<ChromeDriverBuilder>? configure = null) =>
        UseDriver(new ChromeDriverBuilder(), configure);

    /// <summary>
    /// Creates and configures a new builder for <see cref="FirefoxDriver"/>
    /// with default <see cref="WebDriverAliases.Firefox"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="FirefoxDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseFirefox(Action<FirefoxDriverBuilder>? configure = null) =>
        UseDriver(new FirefoxDriverBuilder(), configure);

    /// <summary>
    /// Creates and configures a new builder for <see cref="InternetExplorerDriver"/>
    /// with default <see cref="WebDriverAliases.InternetExplorer"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="InternetExplorerDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseInternetExplorer(Action<InternetExplorerDriverBuilder>? configure = null) =>
        UseDriver(new InternetExplorerDriverBuilder(), configure);

    /// <summary>
    /// Creates and configures a new builder for <see cref="EdgeDriver"/>
    /// with default <see cref="WebDriverAliases.Edge"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="EdgeDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseEdge(Action<EdgeDriverBuilder>? configure = null) =>
        UseDriver(new EdgeDriverBuilder(), configure);

    /// <summary>
    /// Creates and configures a new builder for <see cref="SafariDriver"/>
    /// with default <see cref="WebDriverAliases.Safari"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="SafariDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseSafari(Action<SafariDriverBuilder>? configure = null) =>
        UseDriver(new SafariDriverBuilder(), configure);

    /// <summary>
    /// Creates and configures a new builder for <see cref="RemoteWebDriver"/>
    /// with default <see cref="WebDriverAliases.Remote"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="RemoteWebDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseRemoteDriver(Action<RemoteWebDriverBuilder>? configure = null) =>
        UseDriver(new RemoteWebDriverBuilder(), configure);

    /// <summary>
    /// Configures an existing or creates a new builder for <see cref="ChromeDriver"/> by the specified alias.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="ChromeDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureChrome(Action<ChromeDriverBuilder>? configure = null) =>
        ConfigureChrome(WebDriverAliases.Chrome, configure);

    /// <summary>
    /// Configures an existing or creates a new builder for <see cref="ChromeDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">The driver alias.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="ChromeDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureChrome(string alias, Action<ChromeDriverBuilder>? configure = null) =>
        ConfigureDriver(
            alias,
            () => new ChromeDriverBuilder().WithAlias(alias),
            configure);

    /// <summary>
    /// Configures an existing or creates a new builder for <see cref="FirefoxDriver"/> by the specified alias.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="FirefoxDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureFirefox(Action<FirefoxDriverBuilder>? configure = null) =>
        ConfigureFirefox(WebDriverAliases.Firefox, configure);

    /// <summary>
    /// Configures an existing or creates a new builder for <see cref="FirefoxDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">The driver alias.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="FirefoxDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureFirefox(string alias, Action<FirefoxDriverBuilder>? configure = null) =>
        ConfigureDriver(
            alias,
            () => new FirefoxDriverBuilder().WithAlias(alias),
            configure);

    /// <summary>
    /// Configures an existing or creates a new builder for <see cref="InternetExplorerDriver"/> by the specified alias.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="InternetExplorerDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureInternetExplorer(Action<InternetExplorerDriverBuilder>? configure = null) =>
        ConfigureInternetExplorer(WebDriverAliases.InternetExplorer, configure);

    /// <summary>
    /// Configures an existing or creates a new builder for <see cref="InternetExplorerDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">The driver alias.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="InternetExplorerDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureInternetExplorer(string alias, Action<InternetExplorerDriverBuilder>? configure = null) =>
        ConfigureDriver(
            alias,
            () => new InternetExplorerDriverBuilder().WithAlias(alias),
            configure);

    /// <summary>
    /// Configures an existing or creates a new builder for <see cref="EdgeDriver"/> by the specified alias.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="EdgeDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureEdge(Action<EdgeDriverBuilder>? configure = null) =>
        ConfigureEdge(WebDriverAliases.Edge, configure);

    /// <summary>
    /// Configures an existing or creates a new builder for <see cref="EdgeDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">The driver alias.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="EdgeDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureEdge(string alias, Action<EdgeDriverBuilder>? configure = null) =>
        ConfigureDriver(
            alias,
            () => new EdgeDriverBuilder().WithAlias(alias),
            configure);

    /// <summary>
    /// Configures an existing or creates a new builder for <see cref="SafariDriver"/> by the specified alias.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="SafariDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureSafari(Action<SafariDriverBuilder>? configure = null) =>
        ConfigureSafari(WebDriverAliases.Safari, configure);

    /// <summary>
    /// Configures an existing or creates a new builder for <see cref="SafariDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">The driver alias.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="SafariDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureSafari(string alias, Action<SafariDriverBuilder>? configure = null) =>
        ConfigureDriver(
            alias,
            () => new SafariDriverBuilder().WithAlias(alias),
            configure);

    /// <summary>
    /// Configures an existing or creates a new builder for <see cref="RemoteWebDriver"/> by the specified alias.
    /// </summary>
    /// <param name="configure">An action delegate to configure the provided <see cref="RemoteWebDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureRemoteDriver(Action<RemoteWebDriverBuilder>? configure = null) =>
        ConfigureRemoteDriver(WebDriverAliases.Remote, configure);

    /// <summary>
    /// Configures an existing or creates a new builder for <see cref="RemoteWebDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">The driver alias.</param>
    /// <param name="configure">An action delegate to configure the provided <see cref="RemoteWebDriverBuilder"/>.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder ConfigureRemoteDriver(string alias, Action<RemoteWebDriverBuilder>? configure = null) =>
        ConfigureDriver(
            alias,
            () => new RemoteWebDriverBuilder().WithAlias(alias),
            configure);

    /// <summary>
    /// Sets the default control visibility.
    /// The default value is <see cref="Visibility.Any"/>.
    /// </summary>
    /// <param name="visibility">The visibility.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseDefaultControlVisibility(Visibility visibility)
    {
        DefaultControlVisibility = visibility;
        return this;
    }

    protected override void ConfigureSession(WebDriverSession session)
    {
        base.ConfigureSession(session);

        var driverFactory = DriverFactoryToUse ?? DriverFactories[^1];
        session.DriverFactory = driverFactory;
        session.Variables.SetInitialValue("driver-alias", driverFactory.Alias);

        session.DisposeDriver = DisposeDriver;

        session.DefaultControlVisibility = DefaultControlVisibility;

        session.TakeScreenshotOnFailure = Screenshots.TakeOnFailure;
        session.TakePageSnapshotOnFailure = PageSnapshots.TakeOnFailure;

        InitBrowserLogMonitoring(session);
    }

    protected override IScreenshotTaker CreateScreenshotTaker(WebDriverSession session) =>
        new ScreenshotTaker<WebDriverSession>(
            Screenshots.Strategy,
            WebDriverViewportScreenshotStrategy.Instance,
            FullPageOrViewportScreenshotStrategy.Instance,
            Screenshots.FileNameTemplate,
            session);

    protected override IPageSnapshotTaker CreatePageSnapshotTaker(WebDriverSession session) =>
        new PageSnapshotTaker<WebDriverSession>(
            PageSnapshots.Strategy,
            PageSnapshots.FileNameTemplate,
            session);

    protected override void OnClone(WebDriverSessionBuilder copy)
    {
        base.OnClone(copy);

        copy.DriverFactories = [.. DriverFactories
            .Select(x => x is ICloneable cloneable ? (IWebDriverFactory)cloneable.Clone() : x)];

        if (DriverFactoryToUse is not null)
        {
            int indexOfDriverFactoryToUse = DriverFactories.IndexOf(DriverFactoryToUse);

            copy.DriverFactoryToUse = indexOfDriverFactoryToUse != -1
                ? copy.DriverFactories[indexOfDriverFactoryToUse]
                : DriverFactoryToUse is ICloneable cloneable
                    ? (IWebDriverFactory)cloneable.Clone() :
                    DriverFactoryToUse;
        }

        copy.Screenshots = Screenshots.CloneFor(copy);
        copy.PageSnapshots = PageSnapshots.CloneFor(copy);
        copy.BrowserLogs = BrowserLogs.CloneFor(copy);
    }

    protected override void ValidateConfiguration()
    {
        base.ValidateConfiguration();

        if (DriverFactoryToUse is null && DriverFactories.Count == 0)
        {
            throw new AtataSessionBuilderValidationException(
                $"Cannot build {nameof(WebDriverSession)} as no driver is specified. " +
                $"Use one of \"Use*\" methods to specify the driver to use, e.g.: UseChrome().");
        }
    }

    private static void EnableBrowserLogMonitoringOnWebDriverInitCompletedEvent(
        IWebDriver driver,
        WebDriverSession session,
        IEnumerable<IBrowserLogHandler> browserLogHandlers)
    {
        if (driver is RemoteWebDriver remoteWebDriver)
            remoteWebDriver.RegisterCustomDriverCommand(
                DriverCommand.GetLog,
                new HttpCommandInfo(HttpCommandInfo.PostCommand, "/session/{sessionId}/se/log"));

        if (driver is ChromiumDriver or RemoteWebDriver)
        {
            ChromiumBrowserLogMonitoringStrategy logMonitoringStrategy = new(driver, browserLogHandlers);

            try
            {
                logMonitoringStrategy.Start();
            }
            catch (Exception exception)
            {
                session.Log.Warn(exception, "Browser logs monitoring failed to enable.");
                return;
            }

            object? driverDeInitEventSubscription = null;

            var eventBus = session.EventBus;
            driverDeInitEventSubscription = eventBus.Subscribe<WebDriverDeInitStartedEvent>(() =>
            {
                logMonitoringStrategy.Stop();
                eventBus.Unsubscribe(driverDeInitEventSubscription!);
            });
        }
        else
        {
            session.Log.Warn("Browser logs monitoring cannot be enabled. The feature is currently only available for Chrome and Edge.");
        }
    }

    private void InitBrowserLogMonitoring(WebDriverSession session)
    {
        if (BrowserLogs.HasPropertiesToUse)
        {
            if (session.DriverFactory is ChromeDriverBuilder chromeBuilder)
            {
                chromeBuilder.WithOptions(x => x.SetLoggingPreference(LogType.Browser, OpenQA.Selenium.LogLevel.All));
            }
            else if (session.DriverFactory is EdgeDriverBuilder edgeBuilder)
            {
                edgeBuilder.WithOptions(x => x.SetLoggingPreference(LogType.Browser, OpenQA.Selenium.LogLevel.All));
            }
            else if (session.DriverFactory is RemoteWebDriverBuilder remoteBuilder)
            {
                remoteBuilder.WithOptions(x => x.SetLoggingPreference(LogType.Browser, OpenQA.Selenium.LogLevel.All));
            }

            List<IBrowserLogHandler> browserLogHandlers = new(2);

            if (BrowserLogs.Log)
                browserLogHandlers.Add(new LoggingBrowserLogHandler(session));

            if (BrowserLogs.MinLevelOfWarning is not null)
                browserLogHandlers.Add(new WarningBrowserLogHandler(session, BrowserLogs.MinLevelOfWarning.Value));

            session.EventBus.Subscribe<WebDriverInitCompletedEvent>(
                (e, _) => EnableBrowserLogMonitoringOnWebDriverInitCompletedEvent(e.Driver, session, browserLogHandlers));
        }
    }
}
