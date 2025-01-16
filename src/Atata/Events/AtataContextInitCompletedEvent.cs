#nullable enable

namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataContext"/> is initialized.
/// </summary>
public sealed class AtataContextInitCompletedEvent
{
    internal AtataContextInitCompletedEvent(AtataContext context) =>
        Context = context;

    /// <summary>
    /// Gets the context.
    /// </summary>
    public AtataContext Context { get; }
}
