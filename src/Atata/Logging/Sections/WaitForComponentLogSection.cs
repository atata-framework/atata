namespace Atata;

public class WaitForComponentLogSection : UIComponentLogSection
{
    public WaitForComponentLogSection(UIComponent component, WaitUnit waitUnit)
        : base(component) =>
        Message = $"Wait until {component.ComponentFullName} is {waitUnit.GetWaitingText()}";
}
