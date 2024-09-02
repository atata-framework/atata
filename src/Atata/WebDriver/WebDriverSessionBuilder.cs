using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Atata;

public class WebDriverSessionBuilder : WebSessionBuilder<WebDriverSession, WebDriverSessionBuilder>
{
    /// <summary>
    /// Gets the driver factories.
    /// </summary>
    public List<IDriverFactory> DriverFactories { get; private set; } = [];

    /// <summary>
    /// Gets the driver factory to use.
    /// </summary>
    public IDriverFactory DriverFactoryToUse { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether to dispose the <see cref="WebDriverSession.Driver"/>
    /// when <see cref="WebDriverSession.Dispose()"/> method is invoked.
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
    public string LocalBrowserToUseName =>
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
    /// Gets the screenshots configuration options.
    /// </summary>
    public ScreenshotsWebDriverSessionOptions Screenshots { get; private set; } = new();

    /// <summary>
    /// Gets the page snapshots configuration options.
    /// </summary>
    public PageSnapshotsWebDriverSessionOptions PageSnapshots { get; private set; } = new();

    /// <summary>
    /// Gets the driver factory by the specified alias.
    /// </summary>
    /// <param name="alias">The alias of the driver factory.</param>
    /// <returns>The driver factory or <see langword="null"/>.</returns>
    public IDriverFactory GetDriverFactory(string alias)
    {
        alias.CheckNotNullOrWhitespace(nameof(alias));

        return DriverFactories.LastOrDefault(x => alias.Equals(x.Alias, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Returns an existing or creates a new builder for <typeparamref name="TDriverBuilder"/> by the specified alias.
    /// </summary>
    /// <typeparam name="TDriverBuilder">The type of the driver builder.</typeparam>
    /// <param name="alias">The driver alias.</param>
    /// <param name="driverBuilderCreator">The function that creates a driver builder.</param>
    /// <returns>The <typeparamref name="TDriverBuilder"/> instance.</returns>
    public TDriverBuilder ConfigureDriver<TDriverBuilder>(string alias, Func<TDriverBuilder> driverBuilderCreator)
        where TDriverBuilder : DriverAtataContextBuilder<TDriverBuilder>
    {
        alias.CheckNotNullOrWhitespace(nameof(alias));
        driverBuilderCreator.CheckNotNull(nameof(driverBuilderCreator));

        var driverFactory = GetDriverFactory(alias);

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

        return (TDriverBuilder)driverFactory;
    }

    /// <summary>
    /// Use the driver builder.
    /// </summary>
    /// <typeparam name="TDriverBuilder">The type of the driver builder.</typeparam>
    /// <returns>The <typeparamref name="TDriverBuilder"/> instance.</returns>
    public TDriverBuilder UseDriver<TDriverBuilder>()
        where TDriverBuilder : DriverAtataContextBuilder<TDriverBuilder>, new() =>
        UseDriver(new TDriverBuilder());

    /// <summary>
    /// Use the driver builder.
    /// </summary>
    /// <typeparam name="TDriverBuilder">The type of the driver builder.</typeparam>
    /// <param name="driverBuilder">The driver builder.</param>
    /// <returns>The <typeparamref name="TDriverBuilder"/> instance.</returns>
    public TDriverBuilder UseDriver<TDriverBuilder>(TDriverBuilder driverBuilder)
        where TDriverBuilder : DriverAtataContextBuilder<TDriverBuilder>
    {
        driverBuilder.CheckNotNull(nameof(driverBuilder));

        DriverFactories.Add(driverBuilder);
        DriverFactoryToUse = driverBuilder;

        return driverBuilder;
    }

    /// <summary>
    /// Sets the alias of the driver to use.
    /// </summary>
    /// <param name="alias">The alias of the driver.</param>
    /// <returns>The same <see cref="WebDriverSessionBuilder"/> instance.</returns>
    public WebDriverSessionBuilder UseDriver(string alias)
    {
        alias.CheckNotNullOrWhitespace(nameof(alias));

        IDriverFactory driverFactory = GetDriverFactory(alias);

        if (driverFactory != null)
            DriverFactoryToUse = driverFactory;
        else if (UsePredefinedDriver(alias) == null)
            throw new ArgumentException($"No driver with \"{alias}\" alias defined.", nameof(alias));

        return this;
    }

    /// <summary>
    /// Use the specified driver instance.
    /// </summary>
    /// <param name="driver">The driver to use.</param>
    /// <returns>The <see cref="CustomDriverAtataContextBuilder"/> instance.</returns>
    public CustomDriverAtataContextBuilder UseDriver(IWebDriver driver)
    {
        driver.CheckNotNull(nameof(driver));

        return UseDriver(() => driver);
    }

    /// <summary>
    /// Use the custom driver factory method.
    /// </summary>
    /// <param name="driverFactory">The driver factory method.</param>
    /// <returns>The <see cref="CustomDriverAtataContextBuilder"/> instance.</returns>
    public CustomDriverAtataContextBuilder UseDriver(Func<IWebDriver> driverFactory)
    {
        driverFactory.CheckNotNull(nameof(driverFactory));

        return UseDriver(new CustomDriverAtataContextBuilder(driverFactory));
    }

    private IDriverFactory UsePredefinedDriver(string alias) =>
        alias.ToLowerInvariant() switch
        {
            DriverAliases.Chrome => UseChrome(),
            DriverAliases.Firefox => UseFirefox(),
            DriverAliases.InternetExplorer => UseInternetExplorer(),
            DriverAliases.Safari => UseSafari(),
            DriverAliases.Edge => UseEdge(),
            _ => null
        };

    /// <summary>
    /// Sets a value indicating whether to dispose the <see cref="WebDriverSession.Driver"/>
    /// when <see cref="WebDriverSession.Dispose()"/> method is invoked.
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
    /// Creates and returns a new builder for <see cref="ChromeDriver"/>
    /// with default <see cref="DriverAliases.Chrome"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
    public ChromeAtataContextBuilder UseChrome() =>
        UseDriver(new ChromeAtataContextBuilder());

    /// <summary>
    /// Creates and returns a new builder for <see cref="FirefoxDriver"/>
    /// with default <see cref="DriverAliases.Firefox"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
    public FirefoxAtataContextBuilder UseFirefox() =>
        UseDriver(new FirefoxAtataContextBuilder());

    /// <summary>
    /// Creates and returns a new builder for <see cref="InternetExplorerDriver"/>
    /// with default <see cref="DriverAliases.InternetExplorer"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <returns>The <see cref="InternetExplorerAtataContextBuilder"/> instance.</returns>
    public InternetExplorerAtataContextBuilder UseInternetExplorer() =>
        UseDriver(new InternetExplorerAtataContextBuilder());

    /// <summary>
    /// Creates and returns a new builder for <see cref="EdgeDriver"/>
    /// with default <see cref="DriverAliases.Edge"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
    public EdgeAtataContextBuilder UseEdge() =>
        UseDriver(new EdgeAtataContextBuilder());

    /// <summary>
    /// Creates and returns a new builder for <see cref="SafariDriver"/>
    /// with default <see cref="DriverAliases.Safari"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
    public SafariAtataContextBuilder UseSafari() =>
        UseDriver(new SafariAtataContextBuilder());

    /// <summary>
    /// Creates and returns a new builder for <see cref="RemoteWebDriver"/>
    /// with default <see cref="DriverAliases.Remote"/> alias.
    /// Sets this builder as a one to use for a driver creation.
    /// </summary>
    /// <returns>The <see cref="RemoteDriverAtataContextBuilder"/> instance.</returns>
    public RemoteDriverAtataContextBuilder UseRemoteDriver() =>
        UseDriver(new RemoteDriverAtataContextBuilder());

    /// <summary>
    /// Returns an existing or creates a new builder for <see cref="ChromeDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">
    /// The driver alias.
    /// The default value is <see cref="DriverAliases.Chrome"/>.
    /// </param>
    /// <returns>The <see cref="ChromeAtataContextBuilder"/> instance.</returns>
    public ChromeAtataContextBuilder ConfigureChrome(string alias = DriverAliases.Chrome) =>
        ConfigureDriver(
            alias,
            () => new ChromeAtataContextBuilder().WithAlias(alias));

    /// <summary>
    /// Returns an existing or creates a new builder for <see cref="FirefoxDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">
    /// The driver alias.
    /// The default value is <see cref="DriverAliases.Firefox"/>.
    /// </param>
    /// <returns>The <see cref="FirefoxAtataContextBuilder"/> instance.</returns>
    public FirefoxAtataContextBuilder ConfigureFirefox(string alias = DriverAliases.Firefox) =>
        ConfigureDriver(
            alias,
            () => new FirefoxAtataContextBuilder().WithAlias(alias));

    /// <summary>
    /// Returns an existing or creates a new builder for <see cref="InternetExplorerDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">
    /// The driver alias.
    /// The default value is <see cref="DriverAliases.InternetExplorer"/>.
    /// </param>
    /// <returns>The <see cref="InternetExplorerAtataContextBuilder"/> instance.</returns>
    public InternetExplorerAtataContextBuilder ConfigureInternetExplorer(string alias = DriverAliases.InternetExplorer) =>
        ConfigureDriver(
            alias,
            () => new InternetExplorerAtataContextBuilder().WithAlias(alias));

    /// <summary>
    /// Returns an existing or creates a new builder for <see cref="EdgeDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">
    /// The driver alias.
    /// The default value is <see cref="DriverAliases.Edge"/>.
    /// </param>
    /// <returns>The <see cref="EdgeAtataContextBuilder"/> instance.</returns>
    public EdgeAtataContextBuilder ConfigureEdge(string alias = DriverAliases.Edge) =>
        ConfigureDriver(
            alias,
            () => new EdgeAtataContextBuilder().WithAlias(alias));

    /// <summary>
    /// Returns an existing or creates a new builder for <see cref="SafariDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">
    /// The driver alias.
    /// The default value is <see cref="DriverAliases.Safari"/>.
    /// </param>
    /// <returns>The <see cref="SafariAtataContextBuilder"/> instance.</returns>
    public SafariAtataContextBuilder ConfigureSafari(string alias = DriverAliases.Safari) =>
        ConfigureDriver(
            alias,
            () => new SafariAtataContextBuilder().WithAlias(alias));

    /// <summary>
    /// Returns an existing or creates a new builder for <see cref="RemoteWebDriver"/> by the specified alias.
    /// </summary>
    /// <param name="alias">
    /// The driver alias.
    /// The default value is <see cref="DriverAliases.Remote"/>.
    /// </param>
    /// <returns>The <see cref="RemoteDriverAtataContextBuilder"/> instance.</returns>
    public RemoteDriverAtataContextBuilder ConfigureRemoteDriver(string alias = DriverAliases.Remote) =>
        ConfigureDriver(
            alias,
            () => new RemoteDriverAtataContextBuilder().WithAlias(alias));

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

    protected override void ConfigureSession(WebDriverSession session, AtataContext context)
    {
        base.ConfigureSession(session, context);

        session.DriverFactory = DriverFactoryToUse ?? DriverFactories.LastOrDefault();
        session.DisposeDriver = DisposeDriver;

        session.DefaultControlVisibility = DefaultControlVisibility;
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

        copy.DriverFactories = [.. DriverFactories];
        copy.Screenshots = Screenshots.Clone();
        copy.PageSnapshots = PageSnapshots.Clone();
    }
}
