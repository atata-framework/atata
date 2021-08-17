using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for option selection of <see cref="Select{T, TOwner}"/> control using option <c>value</c> attribute.
    /// </summary>
    [Obsolete("Use " + nameof(SelectsOptionByValueAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class SelectByValueAttribute : SelectsOptionByValueAttribute
    {
        public SelectByValueAttribute()
        {
        }

        public SelectByValueAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public SelectByValueAttribute(TermMatch match)
            : base(match)
        {
        }

        public SelectByValueAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }
    }
}
