namespace Atata.WebDriver;

public class ScrollToComponentLogSection : UIComponentLogSection
{
    public ScrollToComponentLogSection(UIComponent component)
        : base(component)
    {
        Level = LogLevel.Trace;
        Message = $"Scroll to {component.ComponentFullName}";
    }
}
