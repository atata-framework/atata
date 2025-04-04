#nullable enable

namespace Atata;

/// <summary>
/// Specifies that a control should be found by the DOM test identifier attribute, <c>data-testid</c> by default.
/// Finds the control that has the test identifier attribute matching the specified term(s).
/// Uses <see cref="TermCase.Kebab"/> as the default term case.
/// </summary>
public class FindByTestIdAttribute : TermFindAttribute
{
    internal const string DefaultAttributeName = "data-testid";

    internal const TermCase DefaultAttributeCase = TermCase.Kebab;

    public FindByTestIdAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public FindByTestIdAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public FindByTestIdAttribute(TermMatch match, params string[] values)
        : base(match, values)
    {
    }

    public FindByTestIdAttribute(params string[] values)
        : base(values)
    {
    }

    protected override TermCase DefaultCase =>
        WebSession.Current?.DomTestIdAttributeDefaultCase ?? DefaultAttributeCase;

    protected override Type DefaultStrategy => typeof(FindByAttributeStrategy);

    protected override IEnumerable<object> GetStrategyArguments()
    {
        yield return WebSession.Current?.DomTestIdAttributeName ?? DefaultAttributeName;
    }
}
