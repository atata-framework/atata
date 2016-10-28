namespace Atata
{
    public class FindByChildContentStrategy : XPathComponentScopeLocateStrategy
    {
        private readonly int childIndex;

        public FindByChildContentStrategy(int childIndex)
        {
            this.childIndex = childIndex;
        }

        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.
                WrapWithIndex(x => x.Descendant.ComponentXPath.Where(
                    y => y.Any.WhereIndex(childIndex).Where(z => z.TermsConditionOfContent)));
        }
    }
}
