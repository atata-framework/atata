namespace Atata
{
    public class FindByColumnIndexStrategy : XPathComponentScopeLocateStrategy
    {
        private readonly int columnIndex;

        public FindByColumnIndexStrategy(int columnIndex)
        {
            this.columnIndex = columnIndex;
        }

        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeLocateOptions options)
        {
            return builder.
                WrapWithIndex(x => x.OuterXPath._("td").WhereIndex(columnIndex).DescendantOrSelf.ComponentXPath);
        }
    }
}
