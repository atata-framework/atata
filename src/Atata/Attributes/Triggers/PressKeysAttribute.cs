#nullable enable

namespace Atata;

/// <summary>
/// Defines the keys to press on the specified event.
/// By default occurs after the set.
/// </summary>
public class PressKeysAttribute : TriggerAttribute
{
    public PressKeysAttribute(string keys, TriggerEvents on = TriggerEvents.AfterSet, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority) =>
        Keys = keys;

    public string Keys { get; protected set; }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        context.Component.Owner.Press(Keys);
}
