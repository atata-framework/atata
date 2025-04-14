namespace Atata;

public class WaitForElementLogSection : UIComponentLogSection
{
    public WaitForElementLogSection(UIComponent component, WaitBy waitBy, string selector, WaitUnit waitUnit)
        : base(component) =>
        Message = $"Wait until element by \"{selector}\" {ConvertToString(waitBy)} is {waitUnit.GetWaitingText()}";

    private static string ConvertToString(WaitBy waitBy) =>
        waitBy switch
        {
            WaitBy.Id => "id",
            WaitBy.Name => "name",
            WaitBy.Class => "class",
            WaitBy.Css => "CSS selector",
            WaitBy.XPath => "XPath",
            _ => throw Guard.CreateArgumentExceptionForUnsupportedValue(waitBy)
        };
}
