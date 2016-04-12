namespace Atata
{
    public class ScrollDownAttribute : TriggerAttribute
    {
        public ScrollDownAttribute(TriggerEvents on = TriggerEvents.BeforeAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
        }

        public override void Execute(TriggerContext context)
        {
            context.Driver.ExecuteScript("window.scrollTo(0,document.body.scrollHeight);");
        }
    }
}
