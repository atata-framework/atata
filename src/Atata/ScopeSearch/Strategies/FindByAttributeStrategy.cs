namespace Atata
{
    public class FindByAttributeStrategy : XPathComponentScopeLocateStrategy
    {
        private readonly string attributeName;

        public FindByAttributeStrategy(string attributeName)
        {
            this.attributeName = attributeName;
        }

        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.
                WrapWithIndex(x => x.Descendant.ComponentXPath[y => y.TermsConditionOf(attributeName)]);
        }
    }
}
