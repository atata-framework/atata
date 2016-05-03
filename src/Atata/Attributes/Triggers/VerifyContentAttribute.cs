namespace Atata
{
    public class VerifyContentAttribute : TriggerAttribute
    {
        public VerifyContentAttribute(TermMatch match, params string[] values)
            : base(TriggerEvents.OnPageObjectInit)
        {
            Values = values;
            Match = match;
        }

        public string[] Values { get; private set; }
        public new TermMatch Match { get; set; }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Component.VerifyContent(Values, Match);
        }
    }
}
