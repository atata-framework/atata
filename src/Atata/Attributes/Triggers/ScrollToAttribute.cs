#nullable enable

namespace Atata;

/// <summary>
/// Indicates that the scrolling to the control should be performed on the specified event.
/// By default occurs before: set, click, hover and focus.
/// </summary>
public class ScrollToAttribute : TriggerAttribute
{
    public ScrollToAttribute(
        TriggerEvents on = TriggerEvents.BeforeSet | TriggerEvents.BeforeClick | TriggerEvents.BeforeHover | TriggerEvents.BeforeFocus,
        TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
    {
        if (context.Component is Control<TOwner> componentAsControl)
        {
            if (context.Event is not TriggerEvents.BeforeScroll and not TriggerEvents.AfterScroll)
                componentAsControl.ScrollTo();
        }
        else
        {
            throw new InvalidOperationException($"{nameof(ScrollToAttribute)} trigger can be executed only against control. But was: {context.Component.GetType().FullName}.");
        }
    }
}
