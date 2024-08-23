namespace Atata;

/// <summary>
/// Provides reporting functionality for <see cref="WebSession"/>.
/// </summary>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public class WebSessionReport<TOwner> : Report<TOwner>
{
    private readonly WebSession _session;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebSessionReport{TOwner}" /> class.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="session">The session.</param>
    public WebSessionReport(TOwner owner, WebSession session)
        : base(owner, session.Context) =>
        _session = session;

    /// <inheritdoc cref="WebSession.TakeScreenshot(string)"/>
    /// <returns>The instance of the owner object.</returns>
    public TOwner Screenshot(string title = null)
    {
        _session.TakeScreenshot(title);
        return Owner;
    }

    /// <inheritdoc cref="WebSession.TakeScreenshot(ScreenshotKind, string)"/>
    /// <returns>The instance of the owner object.</returns>
    public TOwner Screenshot(ScreenshotKind kind, string title = null)
    {
        _session.TakeScreenshot(kind, title);
        return Owner;
    }

    /// <inheritdoc cref="WebSession.TakePageSnapshot(string)"/>
    /// <returns>The instance of the owner object.</returns>
    public TOwner PageSnapshot(string title = null)
    {
        _session.TakePageSnapshot(title);
        return Owner;
    }
}
