namespace Atata
{
    public class FindByAttributeAttribute : TermFindAttribute
    {
        public FindByAttributeAttribute(string attributeName, TermMatch match)
            : this(attributeName, null, TermFormat.Inherit, match)
        {
        }

        public FindByAttributeAttribute(string attributeName, TermFormat format, TermMatch match = TermMatch.Inherit)
            : this(attributeName, null, format, match)
        {
        }

        public FindByAttributeAttribute(string attributeName, string value, TermMatch match)
            : this(attributeName, new[] { value }, match: match)
        {
        }

        public FindByAttributeAttribute(string attributeName, params string[] values)
            : this(attributeName, values, TermFormat.Inherit)
        {
        }

        private FindByAttributeAttribute(string attributeName, string[] values = null, TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(values, format, match)
        {
            AttributeName = attributeName.CheckNotNullOrWhitespace("attributeName");
        }

        public string AttributeName { get; private set; }

        protected override TermFormat DefaultFormat
        {
            get { return TermFormat.Title; }
        }

        public override IComponentScopeLocateStrategy CreateStrategy(UIComponentMetadata metadata)
        {
            return new FindByAttributeStrategy(AttributeName);
        }
    }
}
