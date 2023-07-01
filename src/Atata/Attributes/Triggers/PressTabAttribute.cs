namespace Atata;

/// <summary>
/// Indicates that the Tab key should be pressed on the specified event.
/// By default occurs after the set.
/// </summary>
public class PressTabAttribute : TriggerAttribute
{
    public PressTabAttribute(TriggerEvents on = TriggerEvents.AfterSet, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        context.Component.Owner.Press(Keys.Tab);
}
