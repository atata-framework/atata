namespace Atata
{
    /// <summary>
    /// Indicates the verification of the content of the &lt;h1&gt; tag when the page object is initialized.
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
            var headingControl = context.Component.Owner.Controls.Create<H1<TOwner>>(name, new FindByIndexAttribute(Index));
            headingControl.Should.WithRetry.MatchAny(Match, values);
        }
    }
}
