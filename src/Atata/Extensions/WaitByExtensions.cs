using OpenQA.Selenium;

namespace Atata
{
    public static class WaitByExtensions
    {
        public static By GetBy(this WaitBy waitBy, string selector)
        {
            switch (waitBy)
            {
                case WaitBy.Id:
                    return By.Id(selector);
                case WaitBy.Name:
                    return By.Name(selector);
                case WaitBy.Class:
                    return By.ClassName(selector);
                case WaitBy.Css:
                    return By.CssSelector(selector);
                case WaitBy.XPath:
                    return By.XPath(selector);
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(waitBy, nameof(waitBy));
            }
        }
    }
}
