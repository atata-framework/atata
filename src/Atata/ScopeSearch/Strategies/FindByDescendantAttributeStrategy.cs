namespace Atata
{
    public class FindByDescendantAttributeStrategy : XPathComponentScopeFindStrategy
    {
        private readonly string _attributeName;

        public FindByDescendantAttributeStrategy(string attributeName)
        {
            _attributeName = attributeName;
        }

        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath.ComponentXPath[c => c.Descendant.Any[d => d.TermsConditionOf(_attributeName)]]);
        }
    }
}
