namespace Atata
{
    public class ScrollUpAttribute : TriggerAttribute
    {
        public ScrollUpAttribute(TriggerEvent on = TriggerEvent.BeforeAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope scope = TriggerScope.Self)
            : base(on, priority, scope)
        {
        }

        public override void Run(TriggerContext context)
        {
            context.Driver.ExecuteScript("scroll(0,0);");
        }
    }
}
