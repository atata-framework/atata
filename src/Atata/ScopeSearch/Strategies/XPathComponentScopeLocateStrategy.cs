using System.Text;
using OpenQA.Selenium;

namespace Atata
{
    public class XPathComponentScopeLocateStrategy : IComponentScopeLocateStrategy
    {
        public XPathComponentScopeLocateStrategy(XPathPrefixKind xPathPrefix = XPathPrefixKind.Descendant, IndexUsage useIndex = IndexUsage.IfNotNull)
        {
            XPathPrefix = xPathPrefix;
            UseIndex = useIndex;
        }

        public enum XPathPrefixKind
        {
            None,
            Descendant,
            DescendantOrSelf
        }

        public enum IndexUsage
        {
            None,
            IfNotNull,
            AnyCase
        }

        protected XPathPrefixKind XPathPrefix { get; private set; }

        protected IndexUsage UseIndex { get; private set; }

        public virtual ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            string xPath = BuildXPath(options);
            return new XPathComponentScopeLocateResult(xPath, scope, searchOptions);
        }

        private string BuildXPath(ComponentScopeLocateOptions options)
        {
            StringBuilder builder = new StringBuilder(options.ElementXPath);
            BuildXPath(builder, options);

            if (XPathPrefix != XPathPrefixKind.None)
                builder.Insert(0, XPathPrefix == XPathPrefixKind.Descendant ? ".//" : "descendant-or-self::");

            if (UseIndex == IndexUsage.AnyCase || (UseIndex == IndexUsage.IfNotNull && options.HasIndex))
                builder.Insert(0, "(").Append(")").AppendFormat(options.GetPositionWrappedXPathCondition());

            return builder.ToString();
        }

        protected virtual void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
        }
    }
}
