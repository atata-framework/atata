#nullable enable

namespace Atata;

/// <summary>
/// Specifies that a control should be found by the content text of the preceding sibling.
/// Uses <see cref="TermCase.Title"/> as the default term case.
/// </summary>
public class FindByPrecedingSiblingContentAttribute : TermFindAttribute
{
    public FindByPrecedingSiblingContentAttribute(TermCase termCase)
        : base(termCase)
    {
    }

    public FindByPrecedingSiblingContentAttribute(TermMatch match, TermCase termCase)
        : base(match, termCase)
    {
    }

    public FindByPrecedingSiblingContentAttribute(TermMatch match, params string[] values)
        : base(match, values)
    {
    }

    public FindByPrecedingSiblingContentAttribute(params string[] values)
        : base(values)
    {
    }

    /// <summary>
    /// Gets or sets the sibling XPath.
    /// The default value is <c>"*"</c>.
    /// </summary>
    public string SiblingXPath { get; set; } = "*";

    protected override TermCase DefaultCase => TermCase.Title;

    protected override Type DefaultStrategy => typeof(FindByRelativeElementContentStrategy);

    protected override IEnumerable<object> GetStrategyArguments()
    {
        yield return $"preceding-sibling::{SiblingXPath}";
    }
}
