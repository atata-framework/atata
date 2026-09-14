namespace Atata.WebDriver;

public class FocusLogSection : UIComponentLogSection
{
    public FocusLogSection(UIComponent component)
        : base(component) =>
        Message = $"Focus on {component.ComponentFullName}";
}
