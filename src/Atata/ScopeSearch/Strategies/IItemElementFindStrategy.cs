using OpenQA.Selenium;

namespace Atata
{
    public interface IItemElementFindStrategy
    {
        string GetConditionXPath(object parameter);
        string GetParameter(IWebElement element);
    }
}
