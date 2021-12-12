namespace Atata
{
    public class FindByChildContentStrategy : XPathComponentScopeFindStrategy
    {
        private readonly int _childIndex;

        public FindByChildContentStrategy(int childIndex)
        {
            _childIndex = childIndex;
        }

        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath.ComponentXPath[y => y.Any[_childIndex + 1][z => z.TermsConditionOfContent]]);
        }
    }
}
