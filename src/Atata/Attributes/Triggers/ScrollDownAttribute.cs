namespace Atata
{
    /// <summary>
    /// Indicates that the scroll down should be performed on the specified event. By default occurs before any action.
    /// </summary>
    public class ScrollDownAttribute : TriggerAttribute
    {
        public ScrollDownAttribute(TriggerEvents on = TriggerEvents.BeforeAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
        }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Driver.ExecuteScript("window.scrollTo(0,document.body.scrollHeight);");
        }
    }
}
