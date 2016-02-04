using OpenQA.Selenium;
using System.Text;

namespace Atata
{
    public class XPathElementFindStrategy : IXPathElementFindStrategy
    {
        public XPathElementFindStrategy(XPathPrefixKind xPathPrefix = XPathPrefixKind.Descendant, bool applyIndex = true)
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

            string xPath = BuildXPath(options);
            return new ElementLocator(scope, xPath);
        }

        public string BuildXPath(ElementFindOptions options)
        {
            StringBuilder builder = new StringBuilder(options.ElementXPath);
            BuildXPath(builder, options);

            if (ApplyIndex && options.HasIndex)
                builder.AppendFormat(options.GetPositionWrappedXPathCondition());

            if (XPathPrefix != XPathPrefixKind.None)
                builder.Insert(0, XPathPrefix == XPathPrefixKind.Descendant ? ".//" : "descendant-or-self::");

            return builder.ToString();
        }

        protected virtual void BuildXPath(StringBuilder builder, ElementFindOptions options)
        {
        }
    }
}
