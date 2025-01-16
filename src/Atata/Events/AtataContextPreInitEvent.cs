#nullable enable

namespace Atata;

/// <summary>
/// Represents an event that occurs before <see cref="AtataContext"/> initialization.
/// </summary>
public sealed class AtataContextPreInitEvent : AtataContextEvent
{
    internal AtataContextPreInitEvent(AtataContext context)
        : base(context)
    {
    }
}
