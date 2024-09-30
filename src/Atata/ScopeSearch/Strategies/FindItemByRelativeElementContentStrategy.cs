namespace Atata;

/// <summary>
/// Represents an item find strategy that finds the item by relative element content using its XPath.
/// </summary>
public class FindItemByRelativeElementContentStrategy : TermItemElementFindStrategy
{
    private readonly UIComponent _component;

    public FindItemByRelativeElementContentStrategy(UIComponent component, string relativeElementXPath)
    {
        _component = component;
        RelativeElementXPath = relativeElementXPath;
    }

    public string RelativeElementXPath { get; }

    public override string GetXPathCondition(object parameter, TermOptions termOptions) =>
        $"[{RelativeElementXPath}[{TermResolver.CreateXPathCondition(parameter, termOptions)}]]";

    protected override string GetParameterAsString(IWebElement element) =>
        element.GetWithLogging(_component.Log, By.XPath(RelativeElementXPath).AtOnce()).Text;
}
