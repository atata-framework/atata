namespace Atata
{
    /// <summary>
    /// Specifies the verification of the <c>&lt;h6&gt;</c> element content.
    /// By default occurs upon the page object initialization.
    /// If no value is specified, it uses the class name as the expected value with the <see cref="TermCase.Title"/> casing applied.
    /// </summary>
    public class VerifyH6Attribute : VerifyHeadingTriggerAttribute
    {
        public VerifyH6Attribute(TermCase termCase)
            : base(termCase)
        {
        }

        public VerifyH6Attribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public VerifyH6Attribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public VerifyH6Attribute(params string[] values)
            : base(values)
        {
        }

        protected override void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values) =>
            Verify<H6<TOwner>, TOwner>(context, values);
    }
}
