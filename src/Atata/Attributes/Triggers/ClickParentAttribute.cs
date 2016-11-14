namespace Atata
{
    /// <summary>
    /// Indicates that the click on the parent component should occur on the specified event. Be default occurs before any action. Is useful for the drop-down button/menu controls.
    /// </summary>
    public class ClickParentAttribute : TriggerAttribute
    {
        public ClickParentAttribute(TriggerEvents on = TriggerEvents.BeforeAnyAction, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            ((Control<TOwner>)context.Component.Parent).Click();
        }
    }
}
