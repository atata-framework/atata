namespace Atata;

public class ListItemDefinitionAttribute : ControlDefinitionAttribute
{
    public ListItemDefinitionAttribute(string scopeXPath = "li", string componentTypeName = "list item")
        : base(scopeXPath) =>
        ComponentTypeName = componentTypeName;
}
