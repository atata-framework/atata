#nullable enable

namespace Atata;

/// <summary>
/// Specifies that a control should be found by the content text of the relative element using its XPath.
/// Uses <see cref="TermCase.Title"/> as the default term case.
/// </summary>
public class FindByRelativeElementContentAttribute : TermFindAttribute
{
    public FindByRelativeElementContentAttribute(string relativeElementXPath, TermCase termCase)
        : base(termCase) =>
        RelativeElementXPath = relativeElementXPath;

    public FindByRelativeElementContentAttribute(string relativeElementXPath, TermMatch match, TermCase termCase)
        : base(match, termCase) =>
        RelativeElementXPath = relativeElementXPath;

    public FindByRelativeElementContentAttribute(string relativeElementXPath, TermMatch match, params string[] values)
        : base(match, values) =>
        RelativeElementXPath = relativeElementXPath;

    public FindByRelativeElementContentAttribute(string relativeElementXPath, params string[] values)
        : base(values) =>
        RelativeElementXPath = relativeElementXPath;

    /// <summary>
    /// Gets the relative element XPath.
    /// </summary>
    public string RelativeElementXPath { get; }

    protected override TermCase DefaultCase => TermCase.Title;

    protected override Type DefaultStrategy => typeof(FindByRelativeElementContentStrategy);

    protected override IEnumerable<object> GetStrategyArguments()
    {
        yield return RelativeElementXPath;
    }
}
