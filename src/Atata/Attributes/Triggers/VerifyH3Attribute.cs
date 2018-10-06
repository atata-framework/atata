namespace Atata
{
    /// <summary>
    /// Specifies the verification of the &lt;h3&gt; element content.
    /// By default occurs upon the page object initialization.
    /// If no value is specified, it uses the class name as the expected value with the <c>TermCase.Title</c> casing applied.
    /// </summary>
    public class VerifyH3Attribute : VerifyHeadingTriggerAttribute
    {
        public VerifyH3Attribute(TermCase termCase)
            : base(termCase)
        {
        }

        public VerifyH3Attribute(TermMatch match, TermCase termCase)
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
            if (Index >= 0)
            {
                var headingControl = context.Component.Owner.Controls.Create<H3<TOwner>>(
                    (Index + 1).Ordinalize(),
                    new FindByIndexAttribute(Index));

                headingControl.Should.WithRetry.MatchAny(Match, values);
            }
            else
            {
                var headingControl = context.Component.Owner.Controls.Create<H3<TOwner>>(
                    Match.FormatComponentName(values),
                    new FindByContentAttribute(Match, values));

                headingControl.Should.WithRetry.Exist();
            }
        }
    }
}
