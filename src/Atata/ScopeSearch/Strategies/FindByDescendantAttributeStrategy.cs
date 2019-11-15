namespace Atata
{
    public class FindByDescendantAttributeStrategy : XPathComponentScopeLocateStrategy
    {
        private readonly string attributeName;

        public FindByDescendantAttributeStrategy(string attributeName)
        {
            this.attributeName = attributeName;
        }

        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath.ComponentXPath[c => c.Descendant.Any[d => d.TermsConditionOf(attributeName)]]);
        }
    }
}
