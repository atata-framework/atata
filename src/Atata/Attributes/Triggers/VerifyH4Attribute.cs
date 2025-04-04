#nullable enable

namespace Atata;

/// <summary>
/// Specifies the verification of the <c>&lt;h4&gt;</c> element content.
/// By default occurs upon the page object initialization.
/// If no value is specified, it uses the class name as the expected value with the <see cref="TermCase.Title"/> casing applied.
/// </summary>
public class VerifyH4Attribute : VerifyHeadingTriggerAttribute
{
    public VerifyH4Attribute(TermCase termCase)
        : base(termCase)
    {
    }

    public VerifyH4Attribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public VerifyH4Attribute(TermMatch match, params string[] values)
        : base(match, values)
    {
    }

    public VerifyH4Attribute(params string[] values)
        : base(values)
    {
    }

    protected override void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values) =>
        Verify<H4<TOwner>, TOwner>(context, values);
}
