using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for option selection of <see cref="Select{T, TOwner}"/> control using option text.
    /// </summary>
    [Obsolete("Use " + nameof(SelectsOptionByTextAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class SelectByTextAttribute : SelectsOptionByTextAttribute
    {
        public SelectByTextAttribute()
        {
        }

        public SelectByTextAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public SelectByTextAttribute(TermMatch match)
            : base(match)
        {
        }

        public SelectByTextAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }
    }
}
