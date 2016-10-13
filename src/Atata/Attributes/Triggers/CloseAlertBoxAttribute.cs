namespace Atata
{
    /// <summary>
    /// Indicates that the alert box should be closed on the specified event. Be default occurs after the click.
    /// </summary>
    public class CloseAlertBoxAttribute : TriggerAttribute
    {
        public CloseAlertBoxAttribute(TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
        }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Driver.SwitchTo().Alert().Accept();
            context.Driver.SwitchTo().DefaultContent();
        }
    }
}
