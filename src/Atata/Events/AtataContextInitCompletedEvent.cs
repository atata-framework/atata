namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataContext"/> is initialized.
/// </summary>
public class AtataContextInitCompletedEvent
{
    public AtataContextInitCompletedEvent(AtataContext context) =>
        Context = context;

    /// <summary>
    /// Gets the context.
    /// </summary>
    public AtataContext Context { get; }
}
