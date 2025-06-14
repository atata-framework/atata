namespace Atata;

public class ClickWithHoldLogSection : UIComponentLogSection
{
    public ClickWithHoldLogSection(UIComponent component, TimeSpan interval)
        : base(component) =>
        Message = $"Click {component.ComponentFullName} with {interval.ToShortIntervalString()} hold";
}
