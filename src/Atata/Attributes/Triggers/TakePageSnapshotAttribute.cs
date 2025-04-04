namespace Atata;

/// <summary>
/// Indicates that a page snapshot should be captured with an optional title.
/// </summary>
public class TakePageSnapshotAttribute : TriggerAttribute
{
    public TakePageSnapshotAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : this(null, on, priority)
    {
    }

    public TakePageSnapshotAttribute(string? title, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority) =>
        Title = title;

    /// <summary>
    /// Gets the title of a page snapshot.
    /// </summary>
    public string? Title { get; }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        context.Component.Session.TakePageSnapshot(Title);
}
