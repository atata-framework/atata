#nullable enable

namespace Atata;

/// <summary>
/// Indicates that the right click on the parent component should occur on the specified event.
/// By default occurs before any access to the component.
/// Is useful for the context menu item controls.
/// </summary>
public class RightClickParentAttribute : TriggerAttribute
{
    public RightClickParentAttribute(TriggerEvents on = TriggerEvents.BeforeAccess, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
        ((Control<TOwner>)context.Component.Parent!).RightClick();
}
