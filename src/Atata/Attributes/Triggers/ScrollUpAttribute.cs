namespace Atata
{
    /// <summary>
    /// Indicates that the scroll up should be performed on the specified event. By default occurs before any action.
    /// </summary>
    public class ScrollUpAttribute : TriggerAttribute
    {
        public ScrollUpAttribute(TriggerEvents on = TriggerEvents.BeforeAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
        }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Driver.ExecuteScript("scroll(0,0);");
        }
    }
}
