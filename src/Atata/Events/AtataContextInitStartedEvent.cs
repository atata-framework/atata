namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataContext"/> is started to initialize.
/// </summary>
public sealed class AtataContextInitStartedEvent : AtataContextEvent
{
    internal AtataContextInitStartedEvent(AtataContext context, AtataContextBuilder contextBuilder)
        : base(context) =>
        ContextBuilder = contextBuilder;

    /// <summary>
    /// Gets the context builder.
    /// </summary>
    public AtataContextBuilder ContextBuilder { get; }
}
