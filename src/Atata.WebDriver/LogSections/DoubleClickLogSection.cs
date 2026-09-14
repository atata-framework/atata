namespace Atata.WebDriver;

public class DoubleClickLogSection : UIComponentLogSection
{
    public DoubleClickLogSection(UIComponent component)
        : base(component) =>
        Message = $"Double-click {component.ComponentFullName}";
}
