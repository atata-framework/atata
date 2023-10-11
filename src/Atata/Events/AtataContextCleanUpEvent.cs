namespace Atata;

[Obsolete("Use AtataContextDeInitEvent or AtataContextDeInitCompletedEvent instead.")] // Obsolete since v2.11.0.
public class AtataContextCleanUpEvent
{
    public AtataContextCleanUpEvent(AtataContext context) =>
        Context = context;

    /// <summary>
    /// Gets the context.
    /// </summary>
    public AtataContext Context { get; }
}
