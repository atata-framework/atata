using Humanizer;
using System.Text;

namespace Atata
{
    public class FindByColumnIndexStrategy : SimpleElementFindStrategy
    {
        private readonly int columnIndex;

        public FindByColumnIndexStrategy(int columnIndex)
        {
            this.columnIndex = columnIndex;
        }

        protected override void BuildXPath(StringBuilder builder, ElementFindOptions options)
        {
            builder.Insert(0, "td[{0}]/descendant-or-self::".FormatWith(columnIndex + 1));
        }
    }
}
