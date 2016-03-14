using OpenQA.Selenium;

namespace Atata
{
    public interface IItemElementFindStrategy
    {
        string GetXPathCondition(object parameter);
        T GetParameter<T>(IWebElement element);
        string ConvertToString(object parameter);
    }
}
