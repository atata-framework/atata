#nullable enable

namespace Atata;

/// <summary>
/// Indicates to wait for an alert box to be present on the specified event.
/// Be default occurs after the click.
/// </summary>
public class WaitForAlertBoxAttribute : WaitingTriggerAttribute
{
    public WaitForAlertBoxAttribute(TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        context.Component.Owner.SwitchToAlertBox();
}
