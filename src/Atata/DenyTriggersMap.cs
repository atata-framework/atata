namespace Atata;

internal static class DenyTriggersMap
{
    internal static Dictionary<TriggerEvents, TriggerEvents[]> Values { get; } = new()
    {
        [TriggerEvents.BeforeAccess] = [TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess],
        [TriggerEvents.AfterAccess] = [TriggerEvents.BeforeAccess, TriggerEvents.AfterAccess]
    };
}
