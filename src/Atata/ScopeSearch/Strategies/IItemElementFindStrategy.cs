using OpenQA.Selenium;

namespace Atata
{
    public interface IItemElementFindStrategy
    {
        string GetXPathCondition(object parameter);
        object GetParameter(IWebElement element);
    }
}
