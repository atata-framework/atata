namespace Atata
{
    /// <summary>
    /// Indicates that the click on the parent component should occur on the specified event.
    /// By default occurs before any access to the component.
    /// Is useful for the drop-down button/menu controls.
    /// </summary>
    public class ClickParentAttribute : TriggerAttribute
    {
        public ClickParentAttribute(TriggerEvents on = TriggerEvents.BeforeAccess, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
            ((Control<TOwner>)context.Component.Parent).Click();
    }
}
