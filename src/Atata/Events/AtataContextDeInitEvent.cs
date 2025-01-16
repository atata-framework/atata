#nullable enable

namespace Atata;

[Obsolete("Use AtataContextDeInitStartedEvent instead.")] // Obsolete since v4.0.0.
public sealed class AtataContextDeInitEvent : AtataContextEvent
{
    internal AtataContextDeInitEvent(AtataContext context)
        : base(context)
    {
    }
}
