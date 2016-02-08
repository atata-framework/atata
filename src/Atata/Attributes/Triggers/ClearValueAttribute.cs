namespace Atata
{
    public class ClearValueAttribute : TriggerAttribute
    {
        public ClearValueAttribute(TriggerEvents on = TriggerEvents.BeforeSet, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
        }

        public override void Run(TriggerContext context)
        {
            context.ComponentScopeLocator.GetElement().Clear();
        }
    }
}
