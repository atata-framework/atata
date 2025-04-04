namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataContext"/> is started to deinitialize.
/// </summary>
public sealed class AtataContextDeInitStartedEvent : AtataContextEvent
{
    internal AtataContextDeInitStartedEvent(AtataContext context)
        : base(context)
    {
    }
}
