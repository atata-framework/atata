using OpenQA.Selenium;
using System.Text;

namespace Atata
{
    public class SimpleElementFindStrategy : IElementFindStrategy
    {
        public SimpleElementFindStrategy(XPathPrefixKind xPathPrefix = XPathPrefixKind.Descendant, bool applyIndex = true)
        {
            XPathPrefix = xPathPrefix;
        }

        public enum XPathPrefixKind
        {
            None,
            Descendant,
            DescendantOrSelf
        }

        protected XPathPrefixKind XPathPrefix { get; private set; }
        protected bool ApplyIndex { get; private set; }

        public ElementLocator Find(IWebElement scope, ElementFindOptions options)
        {
            if (scope == null && options.IsSafely)
                return null;

            StringBuilder builder = new StringBuilder(options.ElementXPath);
            BuildXPath(builder, options);

            if (ApplyIndex && options.HasIndex)
                builder.AppendFormat(options.GetPositionWrappedXPathCondition());

            if (XPathPrefix != XPathPrefixKind.None)
                builder.Insert(0, XPathPrefix == XPathPrefixKind.Descendant ? ".//" : "descendant-or-self::");

            string xPath = builder.ToString();
            return new ElementLocator(scope, xPath);
        }

        protected virtual void BuildXPath(StringBuilder builder, ElementFindOptions options)
        {
        }
    }
}
