namespace Atata;

/// <summary>
/// Indicates that a screenshot should be captured with an optional title.
/// By default occurs before the click.
/// </summary>
[Obsolete("Use " + nameof(TakeScreenshotAttribute) + " instead.")] // Obsolete since v2.4.0.
public class ScreenshotAttribute : TriggerAttribute
{
    public ScreenshotAttribute(TriggerEvents on = TriggerEvents.BeforeClick, TriggerPriority priority = TriggerPriority.Medium)
        : this(null, on, priority)
    {
    }

    public ScreenshotAttribute(string title = null, TriggerEvents on = TriggerEvents.BeforeClick, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority) =>
        Title = title;

    /// <summary>
    /// Gets the title of a screenshot.
    /// </summary>
    public string Title { get; }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        context.Component.Context.TakeScreenshot(Title);
}
