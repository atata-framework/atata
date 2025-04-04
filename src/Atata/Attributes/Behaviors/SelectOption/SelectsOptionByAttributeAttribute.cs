#nullable enable

namespace Atata;

/// <summary>
/// Represents the behavior for option selection of <see cref="Select{TValue, TOwner}"/> control using specified option attribute.
/// </summary>
public class SelectsOptionByAttributeAttribute : SelectOptionBehaviorAttribute
{
    public SelectsOptionByAttributeAttribute(string attributeName) =>
        AttributeName = attributeName;

    public SelectsOptionByAttributeAttribute(string attributeName, TermCase termCase)
        : base(termCase) =>
        AttributeName = attributeName;

    public SelectsOptionByAttributeAttribute(string attributeName, TermMatch match)
        : base(match) =>
        AttributeName = attributeName;

    public SelectsOptionByAttributeAttribute(string attributeName, TermMatch match, TermCase termCase)
        : base(match, termCase) =>
        AttributeName = attributeName;

    /// <summary>
    /// Gets the name of the attribute.
    /// </summary>
    public string AttributeName { get; }

    public override string FormatOptionXPathCondition(string value) =>
        Match.CreateXPathCondition(value, "@" + AttributeName);

    public override string GetOptionRawValue(IWebElement optionElement) =>
        optionElement.GetValue();
}
