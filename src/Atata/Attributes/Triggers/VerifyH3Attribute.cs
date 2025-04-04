namespace Atata;

/// <summary>
/// Specifies the verification of the <c>&lt;h3&gt;</c> element content.
/// By default occurs upon the page object initialization.
/// If no value is specified, it uses the class name as the expected value with the <see cref="TermCase.Title"/> casing applied.
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

    protected override void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values) =>
        Verify<H3<TOwner>, TOwner>(context, values);
}
