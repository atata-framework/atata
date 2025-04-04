#nullable enable

namespace Atata;

/// <summary>
/// Indicates that the Home key should be pressed on the specified event.
/// By default occurs after the set.
/// </summary>
public class PressHomeAttribute : TriggerAttribute
{
    public PressHomeAttribute(TriggerEvents on = TriggerEvents.AfterSet, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        context.Component.Owner.Press(Keys.Home);
}
