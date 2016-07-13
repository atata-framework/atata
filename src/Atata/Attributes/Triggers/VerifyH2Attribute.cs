namespace Atata
{
    /// <summary>
    /// Indicates the verification of the content of the &lt;h2&gt; tag when the page object is initialized.
    /// </summary>
    public class VerifyH2Attribute : VerifyHeadingTriggerAttribute
    {
        public VerifyH2Attribute(TermCase termCase = TermCase.Inherit)
            : base(termCase)
        {
        }

        public VerifyH2Attribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public VerifyH2Attribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public VerifyH2Attribute(params string[] values)
            : base(values)
        {
        }

        protected override void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values)
        {
            string name = TermResolver.ToDisplayString(values);
            var headingControl = context.Component.Owner.CreateControl<H2<TOwner>>(name, new FindByIndexAttribute(Index));
            headingControl.Should.WithRetry.MatchAny(Match, values);
        }
    }
}
