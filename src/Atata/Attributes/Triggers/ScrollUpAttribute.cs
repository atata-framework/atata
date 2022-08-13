namespace Atata
{
    /// <summary>
    /// Indicates that the scroll up should be performed on the specified event.
    /// By default occurs before any access to the component.
    /// </summary>
    public class ScrollUpAttribute : TriggerAttribute
    {
        public ScrollUpAttribute(TriggerEvents on = TriggerEvents.BeforeAccess, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
            context.Component.Owner.ScrollUp();
    }
}
