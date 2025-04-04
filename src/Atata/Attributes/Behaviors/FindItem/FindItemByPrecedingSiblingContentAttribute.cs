namespace Atata;

/// <summary>
/// Represents the behavior to find an item of <see cref="OptionList{TValue, TOwner}"/> control by preceding sibling element content.
/// </summary>
public class FindItemByPrecedingSiblingContentAttribute : FindItemByRelativeElementContentAttribute
{
    public const string PrecedingSiblingElementXPath = "preceding-sibling::*[1]";

    public FindItemByPrecedingSiblingContentAttribute()
        : base(PrecedingSiblingElementXPath)
    {
    }

    public FindItemByPrecedingSiblingContentAttribute(TermCase termCase)
        : base(PrecedingSiblingElementXPath, termCase)
    {
    }

    public FindItemByPrecedingSiblingContentAttribute(TermMatch match)
        : base(PrecedingSiblingElementXPath, match)
    {
    }

    public FindItemByPrecedingSiblingContentAttribute(TermMatch match, TermCase termCase)
        : base(PrecedingSiblingElementXPath, match, termCase)
    {
    }
}
