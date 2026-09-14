namespace Atata.WebDriver;

public class ListItemDefinitionAttribute : ControlDefinitionAttribute
{
    public ListItemDefinitionAttribute(string scopeXPath = "li", string componentTypeName = "list item")
        : base(scopeXPath) =>
        ComponentTypeName = componentTypeName;
}
