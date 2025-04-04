namespace Atata;

/// <summary>
/// Indicates that the confirm box should be closed on the specified event.
/// Be default occurs after the click.
/// By default accepts the confirm box.
/// </summary>
public class CloseConfirmBoxAttribute : TriggerAttribute
{
    public CloseConfirmBoxAttribute(bool accept = true, TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority) =>
        Accept = accept;

    public bool Accept { get; }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
    {
        var confirmBox = context.Component.Owner.SwitchToConfirmBox();

        if (Accept)
            confirmBox.Accept();
        else
            confirmBox.Cancel();
    }
}
