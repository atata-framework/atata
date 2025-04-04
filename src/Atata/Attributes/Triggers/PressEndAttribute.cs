namespace Atata;

/// <summary>
/// Indicates that the End key should be pressed on the specified event.
/// By default occurs after the set.
/// </summary>
public class PressEndAttribute : TriggerAttribute
{
    public PressEndAttribute(TriggerEvents on = TriggerEvents.AfterSet, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        context.Component.Owner.Press(Keys.End);
}
