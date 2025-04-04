#nullable enable

namespace Atata;

/// <summary>
/// Indicates to clear the value on the specified event.
/// By default occurs before the set.
/// </summary>
public class ClearValueAttribute : TriggerAttribute
{
    public ClearValueAttribute(TriggerEvents on = TriggerEvents.BeforeSet, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
    {
        if (context.Component is IClearable clearableComponent)
            clearableComponent.Clear();
        else
            context.Component.Scope.ClearWithLogging(context.Component.Session.Log);
    }
}
