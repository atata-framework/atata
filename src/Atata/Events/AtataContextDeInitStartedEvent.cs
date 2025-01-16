#nullable enable

namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataContext"/> is started to deinitialize.
/// </summary>
public sealed class AtataContextDeInitStartedEvent
{
    internal AtataContextDeInitStartedEvent(AtataContext context) =>
        Context = context;

    /// <summary>
    /// Gets the context.
    /// </summary>
    public AtataContext Context { get; }
}
