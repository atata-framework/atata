#nullable enable

namespace Atata;

/// <summary>
/// Represents the behavior to find an item of <see cref="OptionList{TValue, TOwner}"/> control by following sibling element content.
/// </summary>
public class FindItemByFollowingSiblingContentAttribute : FindItemByRelativeElementContentAttribute
{
    public const string FollowingSiblingElementXPath = "following-sibling::*[1]";

    public FindItemByFollowingSiblingContentAttribute()
        : base(FollowingSiblingElementXPath)
    {
    }

    public FindItemByFollowingSiblingContentAttribute(TermCase termCase)
        : base(FollowingSiblingElementXPath, termCase)
    {
    }

    public FindItemByFollowingSiblingContentAttribute(TermMatch match)
        : base(FollowingSiblingElementXPath, match)
    {
    }

    public FindItemByFollowingSiblingContentAttribute(TermMatch match, TermCase termCase)
        : base(FollowingSiblingElementXPath, match, termCase)
    {
    }
}
