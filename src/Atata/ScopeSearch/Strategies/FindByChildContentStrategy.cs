using System.Text;

namespace Atata
{
    public class FindByChildContentStrategy : XPathComponentScopeLocateStrategy
    {
        private readonly int childIndex;

        public FindByChildContentStrategy(int childIndex)
        {
            this.childIndex = childIndex;
        }

        protected override void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
            builder.AppendFormat(
                "/*[{0}][{1}]",
                childIndex,
                options.GetTermsXPathCondition());
        }
    }
}
