namespace Atata
{
    public class ClickLogSection : UIComponentLogSection
    {
        public ClickLogSection(UIComponent component)
            : base(component) =>
            Message = $"Click {component.ComponentFullName}";
    }
}
