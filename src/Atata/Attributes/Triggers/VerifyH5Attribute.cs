namespace Atata
{
    /// <summary>
    /// Indicates the verification of the content of the &lt;h5&gt; tag when the page object is initialized.
    /// </summary>
    public class VerifyH5Attribute : VerifyHeadingTriggerAttribute
    {
        public VerifyH5Attribute(TermCase termCase)
            : base(termCase)
        {
        }

        public VerifyH5Attribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }

        public VerifyH5Attribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public VerifyH5Attribute(params string[] values)
            : base(values)
        {
        }

        protected override void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values)
        {
            string name = TermResolver.ToDisplayString(values);

            var headingControl = context.Component.Owner.Controls.Create<H5<TOwner>>(
                name,
                Index >= 0 ? new FindByIndexAttribute(Index) : null);
            headingControl.Should.WithRetry.MatchAny(Match, values);
        }
    }
}
