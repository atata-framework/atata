#nullable enable

namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataContext"/> is started to initialize.
/// </summary>
public sealed class AtataContextInitStartedEvent
{
    internal AtataContextInitStartedEvent(AtataContext context, AtataContextBuilder contextBuilder)
    {
        Context = context;
        ContextBuilder = contextBuilder;
    }

    /// <summary>
    /// Gets the context.
    /// </summary>
    public AtataContext Context { get; }

    /// <summary>
    /// Gets the context builder.
    /// </summary>
    public AtataContextBuilder ContextBuilder { get; }
}
