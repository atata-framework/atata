#nullable enable

namespace Atata;

/// <summary>
/// Specifies that a control should be found by the specified attribute of any control's descendant.
/// Finds the control that has any descendant having specified attribute matching the specified term(s).
/// Uses <see cref="TermCase.Title"/> as the default term case.
/// </summary>
public class FindByDescendantAttributeAttribute : TermFindAttribute
{
    public FindByDescendantAttributeAttribute(string attributeName, TermCase termCase)
        : base(termCase) =>
        AttributeName = attributeName.CheckNotNullOrWhitespace(nameof(attributeName));

    public FindByDescendantAttributeAttribute(string attributeName, TermMatch match, TermCase termCase)
        : base(match, termCase) =>
        AttributeName = attributeName.CheckNotNullOrWhitespace(nameof(attributeName));

    public FindByDescendantAttributeAttribute(string attributeName, TermMatch match, params string[] values)
        : base(match, values) =>
        AttributeName = attributeName.CheckNotNullOrWhitespace(nameof(attributeName));

    public FindByDescendantAttributeAttribute(string attributeName, params string[] values)
        : base(values) =>
        AttributeName = attributeName.CheckNotNullOrWhitespace(nameof(attributeName));

    public string AttributeName { get; }

    protected override TermCase DefaultCase => TermCase.Title;

    protected override Type DefaultStrategy => typeof(FindByDescendantAttributeStrategy);

    protected override IEnumerable<object> GetStrategyArguments()
    {
        yield return AttributeName;
    }
}
