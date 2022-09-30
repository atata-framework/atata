namespace Atata
{
    /// <summary>
    /// Specifies the verification of the <c>&lt;h1&gt;</c> element content.
    /// By default occurs upon the page object initialization.
    /// If no value is specified, it uses the class name as the expected value with the <see cref="TermCase.Title"/> casing applied.
    /// </summary>
    public class VerifyH1Attribute : VerifyHeadingTriggerAttribute
    {
        public VerifyH1Attribute(TermCase termCase)
            : base(termCase)
        {
        }

        public VerifyH1Attribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public VerifyH1Attribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public VerifyH1Attribute(params string[] values)
            : base(values)
        {
        }

        protected override void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values) =>
            Verify<H1<TOwner>, TOwner>(context, values);
    }
}
