using System;

namespace Atata
{
    /// <summary>
    /// Indicates that the focusing on the control should be performed on the specified event.
    /// By default occurs before set.
    /// </summary>
    public class FocusAttribute : TriggerAttribute
    {
        public FocusAttribute(
            TriggerEvents on = TriggerEvents.BeforeSet,
            TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            if (context.Component is Control<TOwner> componentAsControl)
            {
                if (context.Event != TriggerEvents.BeforeFocus && context.Event != TriggerEvents.AfterFocus)
                    componentAsControl.Focus();
            }
            else
            {
                throw new InvalidOperationException($"{nameof(FocusAttribute)} trigger can be executed only against control. But was: {context.Component.GetType().FullName}.");
            }
        }
    }
}
