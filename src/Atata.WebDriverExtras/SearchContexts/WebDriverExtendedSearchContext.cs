using System;
using OpenQA.Selenium;

namespace Atata
{
    public class WebDriverExtendedSearchContext : ExtendedSearchContext<IWebDriver>
    {
        public WebDriverExtendedSearchContext(IWebDriver driver)
            : base(driver)
        {
        }

        public WebDriverExtendedSearchContext(IWebDriver driver, TimeSpan timeout)
            : base(driver, timeout)
        {
        }

        public WebDriverExtendedSearchContext(IWebDriver driver, TimeSpan timeout, TimeSpan retryInterval)
            : base(driver, timeout, retryInterval)
        {
        }

        public bool TitleContains(string text)
        {
            return Until(x => x.TitleContains(text));
        }
    }
}
