namespace Atata;

/// <summary>
/// Represents an event that occurs before <see cref="AtataContext"/> initialization.
/// </summary>
public class AtataContextPreInitEvent
{
    public AtataContextPreInitEvent(AtataContext context) =>
        Context = context;

    /// <summary>
    /// Gets the context.
    /// </summary>
    public AtataContext Context { get; }
}
