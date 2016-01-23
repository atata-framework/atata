namespace Atata
{
    public class ClickParentAttribute : TriggerAttribute
    {
        public ClickParentAttribute(TriggerEvent on = TriggerEvent.BeforeAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope scope = TriggerScope.Self)
            : base(on, priority, scope)
        {
        }

        public override void Run(TriggerContext context)
        {
            ((IClickable)context.ParentComponent).Click();
        }
    }
}
