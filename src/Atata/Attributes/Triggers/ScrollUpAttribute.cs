namespace Atata
{
    public class ScrollUpAttribute : TriggerAttribute
    {
        public ScrollUpAttribute(TriggerEvents on = TriggerEvents.BeforeAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
        }

        public override void Run(TriggerContext context)
        {
            context.Driver.ExecuteScript("scroll(0,0);");
        }
    }
}
