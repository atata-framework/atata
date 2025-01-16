#nullable enable

namespace Atata;

[Obsolete("Use AtataContextDeInitStartedEvent instead.")] // Obsolete since v4.0.0.
public sealed class AtataContextDeInitEvent
{
    internal AtataContextDeInitEvent(AtataContext context) =>
        Context = context;

    public AtataContext Context { get; }
}
