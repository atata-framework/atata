namespace Atata;

/// <summary>
/// Specifies that a control should be found by the specified attribute.
/// Finds the control that has the specified attribute matching the specified term(s).
/// Uses <see cref="TermCase.Title"/> as the default term case.
/// </summary>
public class FindByAttributeAttribute : TermFindAttribute
{
    public FindByAttributeAttribute(string attributeName, TermCase termCase)
        : base(termCase) =>
        AttributeName = Guard.ReturnOrThrowIfNullOrWhitespace(attributeName);

    public FindByAttributeAttribute(string attributeName, TermMatch match, TermCase termCase)
        : base(match, termCase) =>
        AttributeName = Guard.ReturnOrThrowIfNullOrWhitespace(attributeName);

    public FindByAttributeAttribute(string attributeName, TermMatch match, params string[] values)
        : base(match, values) =>
        AttributeName = Guard.ReturnOrThrowIfNullOrWhitespace(attributeName);

    public FindByAttributeAttribute(string attributeName, params string[] values)
        : base(values) =>
        AttributeName = Guard.ReturnOrThrowIfNullOrWhitespace(attributeName);

    public string AttributeName { get; }

    protected override TermCase DefaultCase => TermCase.Title;

    protected override Type DefaultStrategy => typeof(FindByAttributeStrategy);

    protected override IEnumerable<object> GetStrategyArguments()
    {
        yield return AttributeName;
    }
}
