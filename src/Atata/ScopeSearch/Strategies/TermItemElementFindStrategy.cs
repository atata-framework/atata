namespace Atata;

public abstract class TermItemElementFindStrategy : IItemElementFindStrategy
{
    protected TermItemElementFindStrategy()
    {
    }

    public abstract string GetXPathCondition(object parameter, TermOptions termOptions);

    public T GetParameter<T>(IWebElement element, TermOptions termOptions)
    {
        string value = GetParameterAsString(element);
        return TermResolver.FromString<T>(value, termOptions);
    }

    protected abstract string GetParameterAsString(IWebElement element);
}
