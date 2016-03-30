namespace Atata
{
    public class VerifyMissingAttribute : TriggerAttribute
    {
        public VerifyMissingAttribute(TriggerEvents on = TriggerEvents.OnPageObjectInit, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
        }

        public override void Run(TriggerContext context)
        {
            context.Component.VerifyMissing();
        }
    }
}
