namespace Atata;

public abstract class WebSession : AtataSession
{
    protected WebSession(AtataContext context)
        : base(context)
    {
    }

    public static WebSession Current =>
        AtataContext.Current?.Sessions.Get<WebSession>()
            ?? throw AtataContextNotFoundException.Create();

    /// <summary>
    /// Gets the <see cref="AtataNavigator"/> instance,
    /// which provides the navigation functionality between pages and windows.
    /// </summary>
    public AtataNavigator Go { get; private protected set; }

    /// <summary>
    /// Gets or sets the base URL.
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// Gets the element find timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan ElementFindTimeout { get; internal set; }

    /// <summary>
    /// Gets the element find retry interval.
    /// The default value is <c>500</c> milliseconds.
    /// </summary>
    public TimeSpan ElementFindRetryInterval { get; internal set; }

    /// <summary>
    /// Gets the current page object.
    /// </summary>
    public UIComponent PageObject { get; internal set; }

    internal bool IsNavigated { get; set; }

    internal List<UIComponent> TemporarilyPreservedPageObjectList { get; private set; } = [];

    public IReadOnlyList<UIComponent> TemporarilyPreservedPageObjects =>
        TemporarilyPreservedPageObjectList;

    /// <summary>
    /// Gets the name of the DOM test identifier attribute.
    /// The default value is <c>"data-testid"</c>.
    /// </summary>
    public string DomTestIdAttributeName { get; internal set; }

    /// <summary>
    /// Gets the default case of the DOM test identifier attribute.
    /// The default value is <see cref="TermCase.Kebab"/>.
    /// </summary>
    public TermCase DomTestIdAttributeDefaultCase { get; internal set; }

    /// <summary>
    /// Takes a screenshot of the current page with an optionally specified title.
    /// </summary>
    /// <param name="title">The title of a screenshot.</param>
    public void TakeScreenshot(string title = null) =>
        ResolveScreenshotTaker()
            .TakeScreenshot(title);

    /// <summary>
    /// Takes a screenshot of the current page of a certain kind with an optionally specified title.
    /// </summary>
    /// <param name="kind">The kind of a screenshot.</param>
    /// <param name="title">The title of a screenshot.</param>
    public void TakeScreenshot(ScreenshotKind kind, string title = null) =>
        ResolveScreenshotTaker()
            .TakeScreenshot(kind, title);

    /// <summary>
    /// Takes a snapshot (HTML or MHTML file) of the current page with an optionally specified title.
    /// </summary>
    /// <param name="title">The title of a snapshot.</param>
    public void TakePageSnapshot(string title = null) =>
        ResolvePageSnapshotTaker()
            .TakeSnapshot(title);

    internal void CleanUpTemporarilyPreservedPageObjectList()
    {
        UIComponentResolver.CleanUpPageObjects(TemporarilyPreservedPageObjects);
        TemporarilyPreservedPageObjectList.Clear();
    }

    protected abstract IScreenshotTaker ResolveScreenshotTaker();

    protected abstract IPageSnapshotTaker ResolvePageSnapshotTaker();
}
