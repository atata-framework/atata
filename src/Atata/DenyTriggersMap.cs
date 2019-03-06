using System.Collections.Generic;

namespace Atata
{
    internal static class DenyTriggersMap
    {
        internal static Dictionary<TriggerEvents, TriggerEvents[]> Values { get; } = new Dictionary<TriggerEvents, TriggerEvents[]>
        {
            [TriggerEvents.BeforeAccess] = new[] { TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess },
            [TriggerEvents.AfterAccess] = new[] { TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess }
        };
    }
}
