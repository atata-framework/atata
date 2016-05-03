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

        public new TermMatch Match { get; set; }
        public string[] Values { get; private set; }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Component.Content.VerifyUntilMatchesAny(Match, Values);
        }
    }
}
