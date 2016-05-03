namespace Atata
{
    public class VerifyContentContainsAllAttribute : TriggerAttribute
    {
        public VerifyContentContainsAllAttribute(params string[] values)
            : base(TriggerEvents.OnPageObjectInit)
        {
            Values = values;
        }

        public string[] Values { get; private set; }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Component.VerifyContentContainsAll(Values);
        }
    }
}
