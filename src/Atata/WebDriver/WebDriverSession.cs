namespace Atata;

public class WebDriverSession : WebSession, IDisposable
{
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

    internal IDriverFactory DriverFactory { get; set; }

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
    public bool HasDriver =>
        _driver != null;

    /// <summary>
    /// Gets the driver alias.
    /// </summary>
    public string DriverAlias { get; internal set; }

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

    internal ScreenshotTaker ScreenshotTaker { get; set; }

    internal PageSnapshotTaker PageSnapshotTaker { get; set; }

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

                _driver.Manage().Timeouts().SetRetryTimeout(ElementFindTimeout, ElementFindRetryInterval);

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

    /// <summary>
    /// Takes a screenshot of the current page with an optionally specified title.
    /// </summary>
    /// <param name="title">The title of a screenshot.</param>
    public void TakeScreenshot(string title = null) =>
        ScreenshotTaker?.TakeScreenshot(title);

    /// <summary>
    /// Takes a screenshot of the current page of a certain kind with an optionally specified title.
    /// </summary>
    /// <param name="kind">The kind of a screenshot.</param>
    /// <param name="title">The title of a screenshot.</param>
    public void TakeScreenshot(ScreenshotKind kind, string title = null) =>
        ScreenshotTaker?.TakeScreenshot(kind, title);

    /// <summary>
    /// Takes a snapshot (HTML or MHTML file) of the current page with an optionally specified title.
    /// </summary>
    /// <param name="title">The title of a snapshot.</param>
    public void TakePageSnapshot(string title = null) =>
        PageSnapshotTaker?.TakeSnapshot(title);

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
