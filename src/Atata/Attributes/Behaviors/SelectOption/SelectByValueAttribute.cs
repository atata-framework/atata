namespace Atata
{
    /// <summary>
    /// Represents the behavior for option selection of <see cref="Select{T, TOwner}"/> control using option <c>value</c> attribute.
    /// </summary>
    public class SelectByValueAttribute : SelectByAttribute
    {
        public const string ValueAttributeName = "value";

        public SelectByValueAttribute()
            : base(ValueAttributeName)
        {
        }

        public SelectByValueAttribute(TermCase termCase)
            : base(ValueAttributeName, termCase)
        {
        }

        public SelectByValueAttribute(TermMatch match)
            : base(ValueAttributeName, match)
        {
        }

        public SelectByValueAttribute(TermMatch match, TermCase termCase)
            : base(ValueAttributeName, match, termCase)
        {
        }
    }
}
