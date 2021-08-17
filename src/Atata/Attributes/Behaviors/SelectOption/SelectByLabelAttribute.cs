using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for option selection of <see cref="Select{T, TOwner}"/> control using option <c>label</c> attribute.
    /// </summary>
    [Obsolete("Use " + nameof(SelectsOptionByLabelAttributeAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class SelectByLabelAttribute : SelectsOptionByLabelAttributeAttribute
    {
        public SelectByLabelAttribute()
        {
        }

        public SelectByLabelAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public SelectByLabelAttribute(TermMatch match)
            : base(match)
        {
        }

        public SelectByLabelAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }
    }
}
