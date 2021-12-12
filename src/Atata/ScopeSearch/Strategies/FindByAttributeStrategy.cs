namespace Atata
{
    public class FindByAttributeStrategy : XPathComponentScopeFindStrategy
    {
        private readonly string _attributeName;

        public FindByAttributeStrategy(string attributeName)
        {
            _attributeName = attributeName;
        }

        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath.ComponentXPath[y => y.TermsConditionOf(_attributeName)]);
        }
    }
}
