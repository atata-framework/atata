namespace Atata
{
    /// <summary>
    /// Indicates the verification of the content of the &lt;h3&gt; tag when the page object is initialized.
    /// </summary>
    public class VerifyH3Attribute : VerifyHeadingTriggerAttribute
    {
        protected VerifyH3Attribute(TermFormat format = TermFormat.Inherit)
            : base(format)
        {
        }

        protected VerifyH3Attribute(params string[] values)
            : base(values)
        {
        }

        protected VerifyH3Attribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        protected VerifyH3Attribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        protected override void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values)
        {
            string name = TermResolver.ToDisplayString(values);
            var headingControl = context.Owner.CreateControl<H3<TOwner>>(name, new FindByIndexAttribute(Index));
            headingControl.VerifyUntilMatchesAny(Match, values);
        }
    }
}
