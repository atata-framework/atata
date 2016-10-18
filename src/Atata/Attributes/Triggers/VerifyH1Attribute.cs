namespace Atata
{
    /// <summary>
    /// Specifies the verification of the &lt;h1&gt; element content. By default occurs upon the page object initialization. If no value is specified, it uses the class name as the expected value with the <c>TermCase.Title</c> casing applied.
    /// </summary>
    public class VerifyH1Attribute : VerifyHeadingTriggerAttribute
    {
        public VerifyH1Attribute(TermCase termCase)
            : base(termCase)
        {
        }

        public VerifyH1Attribute(TermMatch match, TermCase termCase = TermCase.Inherit)
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

        protected override void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values)
        {
            string name = TermResolver.ToDisplayString(values);

            var headingControl = context.Component.Owner.Controls.Create<H1<TOwner>>(
                name,
                Index >= 0 ? new FindByIndexAttribute(Index) : null);
            headingControl.Should.WithRetry.MatchAny(Match, values);
        }
    }
}
