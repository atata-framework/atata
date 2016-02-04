using OpenQA.Selenium;

namespace Atata
{
    public class FindByCssStrategy : IElementFindStrategy
    {
        private readonly XPathElementFindStrategy simpleElementFindStrategy =
            new XPathElementFindStrategy(XPathElementFindStrategy.XPathPrefixKind.DescendantOrSelf, false);

        public ElementLocator Find(IWebElement scope, ElementFindOptions options)
        {
            string cssSelector = string.Join(",", options.Terms);
            IWebElement element = GetElementByCss(scope, By.CssSelector(cssSelector), options.Index, options.IsSafely);

            return simpleElementFindStrategy.Find(element, options);
        }

        private IWebElement GetElementByCss(IWebElement scope, By by, int? index, bool isSafely)
        {
            if (index.HasValue)
            {
                var elements = scope.GetAll(by);
                if (elements.Count <= index.Value)
                    throw ExceptionsFactory.CreateForNoSuchElement(by: by);
                else
                    return elements[index.Value];
            }
            else
            {
                return scope.Get(by.Safely(isSafely));
            }
        }
    }
}
