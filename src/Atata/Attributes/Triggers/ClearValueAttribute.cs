namespace Atata
{
    public class ClearValueAttribute : TriggerAttribute
    {
        public ClearValueAttribute(TriggerEvents on = TriggerEvents.BeforeSet, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Component.Scope.ClearWithLogging();
        }
    }
}
