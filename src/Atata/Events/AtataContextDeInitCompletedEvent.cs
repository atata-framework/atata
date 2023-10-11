namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataContext"/> is deinitialized.
/// </summary>
public sealed class AtataContextDeInitCompletedEvent
{
    public AtataContextDeInitCompletedEvent(AtataContext context) =>
        Context = context;

    /// <summary>
    /// Gets the context.
    /// </summary>
    public AtataContext Context { get; }
}
