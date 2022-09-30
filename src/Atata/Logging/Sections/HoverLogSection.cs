namespace Atata
{
    public class HoverLogSection : UIComponentLogSection
    {
        public HoverLogSection(UIComponent component)
            : base(component) =>
            Message = $"Hover on {component.ComponentFullName}";
    }
}
