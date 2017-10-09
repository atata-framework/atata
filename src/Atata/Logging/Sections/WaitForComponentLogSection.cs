namespace Atata
{
    public class WaitForComponentLogSection : UIComponentLogSection
    {
        public WaitForComponentLogSection(UIComponent component, WaitUnit waitUnit)
            : base(component)
        {
            Level = LogLevel.Trace;
            Message = $"Wait until {component.ComponentFullName} is {waitUnit.GetWaitingText()}";
        }
    }
}
