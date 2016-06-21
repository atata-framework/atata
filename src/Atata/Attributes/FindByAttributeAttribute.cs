namespace Atata
{
    public class FindByAttributeAttribute : TermFindAttribute
    {
        public FindByAttributeAttribute(string attributeName, TermFormat format)
            : this(attributeName, null, format: format)
        {
        }

        public FindByAttributeAttribute(string attributeName, TermMatch match, TermFormat format = TermFormat.Inherit)
            : this(attributeName, null, match, format)
        {
        }

        public FindByAttributeAttribute(string attributeName, TermMatch match, params string[] values)
            : this(attributeName, values, match)
        {
        }

        public FindByAttributeAttribute(string attributeName, params string[] values)
            : this(attributeName, values, format: TermFormat.Inherit)
        {
        }

        private FindByAttributeAttribute(string attributeName, string[] values = null, TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
            : base(values, match, format)
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
