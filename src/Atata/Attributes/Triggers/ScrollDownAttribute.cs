namespace Atata
{
    /// <summary>
    /// Indicates that the scroll down should be performed on the specified event. By default occurs before any action.
    /// </summary>
    public class ScrollDownAttribute : TriggerAttribute
    {
        public ScrollDownAttribute(TriggerEvents on = TriggerEvents.BeforeAnyAction, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Driver.ExecuteScript("window.scrollTo(0,document.body.scrollHeight);");
        }
    }
}
