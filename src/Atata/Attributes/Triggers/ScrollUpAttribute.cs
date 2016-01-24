namespace Atata
{
    public class ScrollUpAttribute : TriggerAttribute
    {
        public ScrollUpAttribute(TriggerEvent on = TriggerEvent.BeforeAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope applyTo = TriggerScope.Self)
            : base(on, priority, applyTo)
        {
        }

        public override void Run(TriggerContext context)
        {
            context.Driver.ExecuteScript("scroll(0,0);");
        }
    }
}
