namespace Atata
{
    /// <summary>
    /// Specifies the verification of the page content.
    /// Verifies whether the component contains the specified content values.
    /// By default occurs upon the page object initialization.
    /// </summary>
    public class VerifyContentAttribute : WaitingTriggerAttribute
    {
        public VerifyContentAttribute(params string[] values)
            : base(TriggerEvents.Init) =>
            Values = values;

        public string[] Values { get; }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
            context.Component.Content.Should.WithinSeconds(Timeout, RetryInterval).ContainAll(Values);
    }
}
