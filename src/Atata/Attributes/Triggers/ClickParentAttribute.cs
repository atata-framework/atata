namespace Atata
{
    public class ClickParentAttribute : TriggerAttribute
    {
        public ClickParentAttribute(TriggerEvent on = TriggerEvent.BeforeAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
        }

        public override void Run(TriggerContext context)
        {
            ((IClickable)context.ParentComponent).Click();
        }
    }
}
