namespace Atata;

public interface IItemElementFindStrategy
{
    string GetXPathCondition(object parameter, TermOptions termOptions);

    T GetParameter<T>(IWebElement element, TermOptions termOptions);
}
