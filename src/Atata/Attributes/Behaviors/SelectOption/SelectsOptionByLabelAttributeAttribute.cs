namespace Atata;

/// <summary>
/// Represents the behavior for option selection of <see cref="Select{TValue, TOwner}"/> control
/// using option <c>label</c> attribute.
/// </summary>
public class SelectsOptionByLabelAttributeAttribute : SelectsOptionByAttributeAttribute
{
    public const string LabelAttributeName = "label";

    public SelectsOptionByLabelAttributeAttribute()
        : base(LabelAttributeName)
    {
    }

    public SelectsOptionByLabelAttributeAttribute(TermCase termCase)
        : base(LabelAttributeName, termCase)
    {
    }

    public SelectsOptionByLabelAttributeAttribute(TermMatch match)
        : base(LabelAttributeName, match)
    {
    }

    public SelectsOptionByLabelAttributeAttribute(TermMatch match, TermCase termCase)
        : base(LabelAttributeName, match, termCase)
    {
    }
}
