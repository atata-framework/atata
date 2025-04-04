namespace Atata;

public class FindItemByLabelStrategy : TermItemElementFindStrategy
{
    private readonly UIComponent _component;

    public FindItemByLabelStrategy(UIComponent component) =>
        _component = component;

    public override string GetXPathCondition(object parameter, TermOptions termOptions)
    {
        ISearchContext scopeContext = _component.ScopeSource.GetScopeContext(_component, SearchOptions.SafelyAtOnce());

        IWebElement? label = scopeContext.GetWithLogging(
            _component.Log,
            By.XPath($".//label[{TermResolver.CreateXPathCondition(parameter, termOptions)}]")
                .SafelyAtOnce()
                .Label(TermResolver.ToDisplayString(parameter)));

        if (label is not null)
        {
            string? elementId = label.GetAttribute("for");

            if (elementId?.Length > 0)
                return $"[@id='{elementId}']";
        }

        return $"[ancestor::label[{TermResolver.CreateXPathCondition(parameter, termOptions)}]]";
    }

    protected override string GetParameterAsString(IWebElement element)
    {
        string? elementId = element.GetAttribute("id");

        if (elementId?.Length > 0)
        {
            ISearchContext scopeContext = _component.ScopeSource.GetScopeContext(_component, SearchOptions.SafelyAtOnce());

            IWebElement? label = scopeContext.GetWithLogging(
                _component.Log,
                By.XPath($".//label[@for='{elementId}']").SafelyAtOnce());

            if (label is not null)
                return label.Text;
        }

        return element.GetWithLogging(
            _component.Log,
            By.XPath("ancestor::label").AtOnce())!
            .Text;
    }
}
