namespace Atata
{
    /// <summary>
    /// Indicates the verification of the content of the &lt;h3&gt; tag when the page object is initialized.
    /// </summary>
    public class VerifyH3Attribute : VerifyHeadingTriggerAttribute
    {
        public VerifyH3Attribute(TermCase termCase = TermCase.Inherit)
            : base(termCase)
        {
        }

        public VerifyH3Attribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public VerifyH3Attribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public VerifyH3Attribute(params string[] values)
            : base(values)
        {
        }

        protected override void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values)
        {
            string name = TermResolver.ToDisplayString(values);
            var headingControl = context.Component.Owner.Controls.Create<H3<TOwner>>(name, new FindByIndexAttribute(Index));
            headingControl.Should.WithRetry.MatchAny(Match, values);
        }
    }
}
