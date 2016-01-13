using OpenQA.Selenium;

namespace Atata
{
    public interface IElementFindStrategy
    {
        ElementLocator Find(IWebElement scope, ElementFindOptions options);
    }
}
