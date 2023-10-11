namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataContext"/> is deinitializing.
/// </summary>
public sealed class AtataContextDeInitEvent
{
    public AtataContextDeInitEvent(AtataContext context) =>
        Context = context;

    /// <summary>
    /// Gets the context.
    /// </summary>
    public AtataContext Context { get; }
}
