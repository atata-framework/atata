using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for option selection of <see cref="Select{T, TOwner}"/> control using specified option attribute.
    /// </summary>
    [Obsolete("Use " + nameof(SelectsOptionByAttributeAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class SelectByAttribute : SelectsOptionByAttributeAttribute
    {
        public SelectByAttribute(string attributeName)
            : base(attributeName)
        {
        }

        public SelectByAttribute(string attributeName, TermCase termCase)
            : base(attributeName, termCase)
        {
        }

        public SelectByAttribute(string attributeName, TermMatch match)
            : base(attributeName, match)
        {
        }

        public SelectByAttribute(string attributeName, TermMatch match, TermCase termCase)
            : base(attributeName, match, termCase)
        {
        }
    }
}
