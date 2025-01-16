#nullable enable

namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataContext"/> is deinitialized.
/// </summary>
public sealed class AtataContextDeInitCompletedEvent : AtataContextEvent
{
    internal AtataContextDeInitCompletedEvent(AtataContext context)
        : base(context)
    {
    }
}
