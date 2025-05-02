namespace Atata;

/// <summary>
/// Represents a base web UI session.
/// </summary>
public abstract class WebSession : AtataSession
{
    /// <summary>
    /// Gets the current <see cref="WebSession"/> instance in scope of <see cref="AtataContext.Current"/>.
    /// Returns <see langword="null"/> if there is no such session or <see cref="AtataContext.Current"/> is <see langword="null"/>.
    /// </summary>
    public static WebSession? Current =>
        AtataContext.Current?.Sessions.GetOrNull<WebSession>();

    /// <summary>
    /// Gets the <see cref="IWebSessionReport{TOwner}"/> instance that provides a reporting functionality.
    /// </summary>
    public new IWebSessionReport<WebSession> Report =>
        (IWebSessionReport<WebSession>)base.Report;

    /// <summary>
    /// Gets or sets the base URL.
    /// </summary>
    public string? BaseUrl { get; set; }

    /// <summary>
    /// Gets the element find timeout.
    /// The default value is <c>5</c> seconds.
    /// </summary>
    public TimeSpan ElementFindTimeout =>
        ElementFindTimeoutOptional ?? BaseRetryTimeoutOptional ?? Context.BaseRetryTimeout;

    /// <summary>
    /// Gets the element find retry interval.
    /// The default value is <c>200</c> milliseconds.
    /// </summary>
    public TimeSpan ElementFindRetryInterval =>
        ElementFindRetryIntervalOptional ?? BaseRetryIntervalOptional ?? Context.BaseRetryInterval;

    /// <summary>
    /// Gets the current page object.
    /// </summary>
    public UIComponent? PageObject { get; internal set; }

    internal bool IsNavigated { get; set; }

    internal TimeSpan? ElementFindTimeoutOptional { get; set; }

    internal TimeSpan? ElementFindRetryIntervalOptional { get; set; }

    internal List<UIComponent> TemporarilyPreservedPageObjectList { get; private set; } = [];

    public IReadOnlyList<UIComponent> TemporarilyPreservedPageObjects =>
        TemporarilyPreservedPageObjectList;

    /// <summary>
    /// Gets the name of the DOM test identifier attribute.
    /// The default value is <c>"data-testid"</c>.
    /// </summary>
    public string DomTestIdAttributeName { get; internal set; } = null!;

    /// <summary>
    /// Gets the default case of the DOM test identifier attribute.
    /// The default value is <see cref="TermCase.Kebab"/>.
    /// </summary>
    public TermCase DomTestIdAttributeDefaultCase { get; internal set; }

    internal IScreenshotTaker ScreenshotTaker { get; set; } = null!;

    internal IPageSnapshotTaker PageSnapshotTaker { get; set; } = null!;

    internal bool TakeScreenshotOnFailure { get; set; } = true;

    internal bool TakePageSnapshotOnFailure { get; set; } = true;

    /// <summary>
    /// Takes a screenshot of the current page with an optionally specified title.
    /// </summary>
    /// <param name="title">The title of a screenshot.</param>
    /// <returns>A <see cref="FileSubject"/> for a taken screenshot file.</returns>
    public FileSubject? TakeScreenshot(string? title = null) =>
        ScreenshotTaker.TakeScreenshot(title);

    /// <summary>
    /// Takes a screenshot of the current page of a certain kind with an optionally specified title.
    /// </summary>
    /// <param name="kind">The kind of a screenshot.</param>
    /// <param name="title">The title of a screenshot.</param>
    /// <returns>A <see cref="FileSubject"/> for a taken screenshot file.</returns>
    public FileSubject? TakeScreenshot(ScreenshotKind kind, string? title = null) =>
        ScreenshotTaker.TakeScreenshot(kind, title);

    /// <summary>
    /// Takes a snapshot (HTML or MHTML file) of the current page with an optionally specified title.
    /// </summary>
    /// <param name="title">The title of a snapshot.</param>
    /// <returns>A <see cref="FileSubject"/> for a taken snapshot file.</returns>
    public FileSubject? TakePageSnapshot(string? title = null) =>
        PageSnapshotTaker.TakeSnapshot(title);

    internal void CleanUpTemporarilyPreservedPageObjectList()
    {
        UIComponentResolver.CleanUpPageObjects(TemporarilyPreservedPageObjects);
        TemporarilyPreservedPageObjectList.Clear();
    }

    protected internal override void LogConfiguration()
    {
        base.LogConfiguration();

        if (BaseUrl?.Length > 0)
            Log.Trace($"Set: BaseUrl={BaseUrl}");

        LogRetrySettings();
    }

    protected internal override void TakeFailureSnapshot()
    {
        base.TakeFailureSnapshot();

        const string failureTitle = "Failure";

        if (TakeScreenshotOnFailure)
            ScreenshotTaker.TakeScreenshot(failureTitle);

        if (TakePageSnapshotOnFailure)
            PageSnapshotTaker.TakeSnapshot(failureTitle);
    }

    private void LogRetrySettings()
    {
        const string messageFormat = "Set: {0}Timeout={1}; {0}RetryInterval={2}";

        Log.Trace(
            messageFormat.FormatWith(
                "ElementFind",
                ElementFindTimeout.ToShortIntervalString(),
                ElementFindRetryInterval.ToShortIntervalString()));

        Log.Trace(
            messageFormat.FormatWith(
                "Waiting",
                WaitingTimeout.ToShortIntervalString(),
                WaitingRetryInterval.ToShortIntervalString()));

        Log.Trace(
            messageFormat.FormatWith(
                "Verification",
                VerificationTimeout.ToShortIntervalString(),
                VerificationRetryInterval.ToShortIntervalString()));
    }
}
