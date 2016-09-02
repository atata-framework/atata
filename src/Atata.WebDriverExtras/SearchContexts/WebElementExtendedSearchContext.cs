using System;
using OpenQA.Selenium;

namespace Atata
{
    public class WebElementExtendedSearchContext : ExtendedSearchContext<IWebElement>
    {
        public WebElementExtendedSearchContext(IWebElement element)
            : base(element)
        {
        }

        public WebElementExtendedSearchContext(IWebElement element, TimeSpan timeout)
            : base(element, timeout)
        {
        }

        public WebElementExtendedSearchContext(IWebElement element, TimeSpan timeout, TimeSpan retryInterval)
            : base(element, timeout, retryInterval)
        {
        }

        public bool HasContent(string content)
        {
            return Until(x => x.Text.Contains(content));
        }
    }
}
