#nullable enable

namespace Atata;

/// <summary>
/// Provides reporting functionality for <see cref="WebSession"/>.
/// </summary>
/// <typeparam name="TOwner">The type of the owner.</typeparam>
public class WebSessionReport<TOwner> : Report<TOwner>, IWebSessionReport<TOwner>
{
    private readonly WebSession _session;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebSessionReport{TOwner}" /> class.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="session">The session.</param>
    public WebSessionReport(TOwner owner, WebSession session)
        : base(owner, session.ExecutionUnit) =>
        _session = session;

    /// <inheritdoc/>
    public TOwner Screenshot(string? title = null)
    {
        _session.TakeScreenshot(title);
        return Owner;
    }

    /// <inheritdoc/>
    public TOwner Screenshot(ScreenshotKind kind, string? title = null)
    {
        _session.TakeScreenshot(kind, title);
        return Owner;
    }

    /// <inheritdoc/>
    public TOwner PageSnapshot(string? title = null)
    {
        _session.TakePageSnapshot(title);
        return Owner;
    }
}
