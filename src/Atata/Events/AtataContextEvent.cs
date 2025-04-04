namespace Atata;

/// <summary>
/// Represents a base class for events associated with <see cref="AtataContext"/>.
/// </summary>
public abstract class AtataContextEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AtataContextEvent"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    protected AtataContextEvent(AtataContext context) =>
        Context = context;

    /// <summary>
    /// Gets the context.
    /// </summary>
    public AtataContext Context { get; }
}
