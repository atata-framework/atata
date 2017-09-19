namespace Atata
{
    public class ScrollToComponentLogSection : UIComponentLogSection
    {
        public ScrollToComponentLogSection(UIComponent component)
            : base(component)
        {
            Message = $"Scroll to {component.ComponentFullName}";
        }
    }
}
