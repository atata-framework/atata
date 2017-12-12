namespace Atata
{
    /// <summary>
    /// Represents the behavior for option selection of <see cref="Select{T, TOwner}"/> control using option "label" attribute.
    /// </summary>
    public class SelectByLabelAttribute : SelectByAttribute
    {
        public const string LabelAttributeName = "label";

        public SelectByLabelAttribute()
            : base(LabelAttributeName)
        {
        }

        public SelectByLabelAttribute(TermCase termCase)
            : base(LabelAttributeName, termCase)
        {
        }

        public SelectByLabelAttribute(TermMatch match)
            : base(LabelAttributeName, match)
        {
        }

        public SelectByLabelAttribute(TermMatch match, TermCase termCase)
            : base(LabelAttributeName, match, termCase)
        {
        }
    }
}
