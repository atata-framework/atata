namespace Atata
{
    /// <summary>
    /// Specifies the verification of the page content.
    /// Verifies whether the component content matches any of the specified values.
    /// By default occurs upon the page object initialization.
    /// </summary>
    public class VerifyContentMatchesAttribute : WaitingTriggerAttribute
    {
        public VerifyContentMatchesAttribute(TermMatch match, params string[] values)
            : base(TriggerEvents.Init)
        {
            Values = values;
            Match = match;
        }

        public new TermMatch Match { get; set; }

        public string[] Values { get; }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
            context.Component.Content.Should.WithinSeconds(Timeout, RetryInterval).MatchAny(Match, Values);
    }
}
