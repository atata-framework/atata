using OpenQA.Selenium;
using System.Text;

namespace Atata
{
    public class XPathComponentScopeLocateStrategy : IComponentScopeLocateStrategy
    {
        public XPathComponentScopeLocateStrategy(XPathPrefixKind xPathPrefix = XPathPrefixKind.Descendant, bool applyIndex = true)
        {
            XPathPrefix = xPathPrefix;
            ApplyIndex = applyIndex;
        }

        public enum XPathPrefixKind
        {
            None,
            Descendant,
            DescendantOrSelf
        }

        protected XPathPrefixKind XPathPrefix { get; private set; }
        protected bool ApplyIndex { get; private set; }

        public ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            string xPath = BuildXPath(options);
            return new XPathComponentScopeLocateResult(xPath);
        }

        private string BuildXPath(ComponentScopeLocateOptions options)
        {
            StringBuilder builder = new StringBuilder(options.ElementXPath);
            BuildXPath(builder, options);

            if (ApplyIndex && options.HasIndex)
                builder.AppendFormat(options.GetPositionWrappedXPathCondition());

            if (XPathPrefix != XPathPrefixKind.None)
                builder.Insert(0, XPathPrefix == XPathPrefixKind.Descendant ? ".//" : "descendant-or-self::");

            return builder.ToString();
        }

        protected virtual void BuildXPath(StringBuilder builder, ComponentScopeLocateOptions options)
        {
        }
    }
}
