namespace Atata
{
    /// <summary>
    /// Represents the behavior for option selection of <see cref="Select{TValue, TOwner}"/> control using option <c>value</c> attribute.
    /// </summary>
    public class SelectsOptionByValueAttribute : SelectsOptionByAttributeAttribute
    {
        public const string ValueAttributeName = "value";

        public SelectsOptionByValueAttribute()
            : base(ValueAttributeName)
        {
        }

        public SelectsOptionByValueAttribute(TermCase termCase)
            : base(ValueAttributeName, termCase)
        {
        }

        public SelectsOptionByValueAttribute(TermMatch match)
            : base(ValueAttributeName, match)
        {
        }

        public SelectsOptionByValueAttribute(TermMatch match, TermCase termCase)
            : base(ValueAttributeName, match, termCase)
        {
        }
    }
}
