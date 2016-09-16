namespace Atata
{
    public class HoverParentAttribute : TriggerAttribute
    {
        public HoverParentAttribute(TriggerEvents on = TriggerEvents.BeforeAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
        }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            ((Control<TOwner>)context.Component.Parent).Hover();
        }
    }
}
