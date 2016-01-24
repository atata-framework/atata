namespace Atata
{
    public class ClickParentAttribute : TriggerAttribute
    {
        public ClickParentAttribute(TriggerEvent on = TriggerEvent.BeforeAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope applyTo = TriggerScope.Self)
            : base(on, priority, applyTo)
        {
        }

        public override void Run(TriggerContext context)
        {
            ((IClickable)context.ParentComponent).Click();
        }
    }
}
