namespace Atata;

/// <summary>
/// Indicates that the control blurring (removing focus) should be performed on the specified event.
/// By default occurs after set.
/// </summary>
public class BlurAttribute : TriggerAttribute
{
    public BlurAttribute(
        TriggerEvents on = TriggerEvents.AfterSet,
        TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
    {
        if (context.Component is Control<TOwner> componentAsControl)
        {
            if (context.Event is not TriggerEvents.BeforeBlur and not TriggerEvents.AfterBlur)
                componentAsControl.Blur();
        }
        else
        {
            throw new InvalidOperationException($"{nameof(BlurAttribute)} trigger can be executed only against control. But was: {context.Component.GetType().FullName}.");
        }
    }
}
