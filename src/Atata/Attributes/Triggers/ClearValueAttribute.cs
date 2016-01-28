namespace Atata
{
    public class ClearValueAttribute : TriggerAttribute
    {
        public ClearValueAttribute(TriggerEvent on = TriggerEvent.BeforeSet, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
        }

        public override void Run(TriggerContext context)
        {
            context.ComponentScopeFinder(false).Clear();
        }
    }
}
