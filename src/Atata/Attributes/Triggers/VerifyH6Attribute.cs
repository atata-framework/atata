namespace Atata
{
    /// <summary>
    /// Indicates the verification of the content of the &lt;h6&gt; tag when the page object is initialized.
    /// </summary>
    public class VerifyH6Attribute : VerifyHeadingTriggerAttribute
    {
        protected VerifyH6Attribute(TermFormat format = TermFormat.Inherit)
            : base(format)
        {
        }

        protected VerifyH6Attribute(params string[] values)
            : base(values)
        {
        }

        protected VerifyH6Attribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        protected VerifyH6Attribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        protected override void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values)
        {
            string name = TermResolver.ToDisplayString(values);
            var headingControl = context.Owner.CreateControl<H6<TOwner>>(name, new FindByIndexAttribute(Index));
            headingControl.VerifyUntilMatchesAny(Match, values);
        }
    }
}
