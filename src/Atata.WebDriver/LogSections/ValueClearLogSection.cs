namespace Atata.WebDriver;

public class ValueClearLogSection : UIComponentLogSection
{
    public ValueClearLogSection(UIComponent component)
        : base(component) =>
        Message = $"Clear {component.ComponentFullName}";
}
