using System.Text;

namespace Atata
{
    public class FindByColumnIndexStrategy : XPathComponentScopeLocateStrategy
    {
        private readonly int columnIndex;

        public FindByColumnIndexStrategy(int columnIndex)
        {
            this.columnIndex = columnIndex;
        }

        protected override void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
            builder.Insert(0, "td[{0}]/descendant-or-self::".FormatWith(columnIndex + 1));
        }
    }
}
