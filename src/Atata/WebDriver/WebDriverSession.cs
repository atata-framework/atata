namespace Atata;

/// <summary>
/// <para>
/// Represents a session that manages <see cref="IWebDriver"/> instance
/// and provides a set of functionality to manipulate the driver.
/// </para>
/// <para>
/// The class adds additional variable to its <see cref="AtataSession.Variables"/>: <c>{driver-alias}</c>.
/// </para>
/// </summary>
public class WebDriverSession : WebSession, IDisposable
{
    private IDriverFactory _driverFactory;

    private IWebDriver _driver;

    private bool _disposed;

    public WebDriverSession(AtataContext context)
        : base(context) =>
        Go = new AtataNavigator(this);

    ~WebDriverSession() =>
        Dispose(false);

    public static new WebDriverSession Current =>
        AtataContext.Current?.Sessions.Get<WebDriverSession>()
            ?? throw AtataContextNotFoundException.Create();

    internal IDriverFactory DriverFactory
    {
        get => _driverFactory;
        set
        {
            _driverFactory = value;

            // TODO: Review the "driver-alias" variable set along with DriverFactory set.
            Variables.SetInitialValue("driver-alias", DriverAlias);
        }
    }

    /// <summary>
    /// Gets the driver.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IWebDriver Driver
    {
        get
        {
            switch (DriverInitializationStage)
            {
                case AtataContextDriverInitializationStage.Build:
                    return _driver;
                case AtataContextDriverInitializationStage.OnDemand:
                    if (_driver is null)
                        InitDriver();
                    return _driver;
                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether this instance has <see cref="Driver"/> instance.
    /// </summary>
    // TODO: Remove HasDriver property.
    public bool HasDriver =>
        _driver != null;

    /// <summary>
    /// Gets the driver alias.
    /// </summary>
    public string DriverAlias =>
        DriverFactory?.Alias;

    internal bool DisposeDriver { get; set; }

    /// <summary>
    /// Gets the driver initialization stage.
    /// </summary>
    public AtataContextDriverInitializationStage DriverInitializationStage { get; internal set; }

    /// <summary>
    /// Gets the default control visibility.
    /// The default value is <see cref="Visibility.Any"/>.
    /// </summary>
    public Visibility DefaultControlVisibility { get; internal set; }

    /// <summary>
    /// Gets the UI component access chain scope cache.
    /// </summary>
    public UIComponentAccessChainScopeCache UIComponentAccessChainScopeCache { get; } = new UIComponentAccessChainScopeCache();

    internal void InitDriver() =>
        Log.ExecuteSection(
            new LogSection("Initialize Driver", LogLevel.Trace),
            () =>
            {
                if (DriverFactory is null)
                    throw new WebDriverInitializationException(
                        $"Failed to create a driver as driver factory is not specified.");

                _driver = DriverFactory.Create()
                    ?? throw new WebDriverInitializationException(
                        $"Driver factory returned null as a driver.");

                // TODO: v4. Move these RetrySettings out of here.
                RetrySettings.Timeout = ElementFindTimeout;
                RetrySettings.Interval = ElementFindRetryInterval;

                EventBus.Publish(new DriverInitEvent(_driver));
            });

    /// <summary>
    /// Restarts the driver.
    /// </summary>
    public void RestartDriver() =>
        Log.ExecuteSection(
            new LogSection("Restart driver"),
            () =>
            {
                CleanUpTemporarilyPreservedPageObjectList();

                if (PageObject != null)
                {
                    UIComponentResolver.CleanUpPageObject(PageObject);
                    PageObject = null;
                }

                EventBus.Publish(new DriverDeInitEvent(_driver));
                DisposeDriverSafely();

                InitDriver();
            });

    protected override IScreenshotTaker CreateScreenshotTaker() =>
        new ScreenshotTaker<WebDriverSession>(
            null, //BuildingContext.Screenshots.Strategy,
            WebDriverViewportScreenshotStrategy.Instance,
            FullPageOrViewportScreenshotStrategy.Instance,
            null, //BuildingContext.Screenshots.FileNameTemplate,
            this);

    protected override IPageSnapshotTaker CreatePageSnapshotTaker() =>
        new PageSnapshotTaker<WebDriverSession>(
            null, //BuildingContext.PageSnapshots.Strategy,
            null, //BuildingContext.PageSnapshots.FileNameTemplate,
            this);

    public void Dispose()
    {
        if (_disposed)
            return;

        Dispose(true);

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            CleanUpTemporarilyPreservedPageObjectList();

            if (PageObject != null)
                UIComponentResolver.CleanUpPageObject(PageObject);

            UIComponentAccessChainScopeCache.Release();

            if (_driver is not null)
            {
                EventBus.Publish(new DriverDeInitEvent(_driver));

                if (DisposeDriver)
                    DisposeDriverSafely();
            }
        }
    }

    private void DisposeDriverSafely()
    {
        try
        {
            _driver.Dispose();
        }
        catch (Exception exception)
        {
            Log.Warn(exception, "Deinitialization of driver failed.");
        }
    }
}
