namespace Atata;

/// <summary>
/// Indicates that a screenshot should be captured with an optional title.
/// </summary>
public class TakeScreenshotAttribute : TriggerAttribute
{
    public TakeScreenshotAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : this(ScreenshotKind.Default, null, on, priority)
    {
    }

    public TakeScreenshotAttribute(string title, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : this(ScreenshotKind.Default, title, on, priority)
    {
    }

    public TakeScreenshotAttribute(ScreenshotKind kind, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : this(kind, null, on, priority)
    {
    }

    public TakeScreenshotAttribute(ScreenshotKind kind, string title, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
        Kind = kind;
        Title = title;
    }

    /// <summary>
    /// Gets the kind of a screenshot.
    /// </summary>
    public ScreenshotKind Kind { get; }

    /// <summary>
    /// Gets the title of a screenshot.
    /// </summary>
    public string Title { get; }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        context.Component.Context.TakeScreenshot(Kind, Title);
}
