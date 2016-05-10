namespace Atata
{
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
